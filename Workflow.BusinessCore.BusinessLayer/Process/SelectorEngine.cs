using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.BusinessLayer.Process.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Workflow.Transverse.Environment;
using System.Data.Common;
using System.Data;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Workflow.BusinessCore.BusinessLayer.Helpers;
using Microsoft.AspNetCore.Http;
using Workflow.BusinessCore.DataLayer.Common;
using Workflow.BusinessCore.DataLayer.Helpers;
using Microsoft.Extensions.Logging;

namespace Workflow.BusinessCore.BusinessLayer.Process
{
    /// <summary>
    /// SelectorProcess class
    /// </summary>
    /// <remarks>
    /// This class permits to define all the steps from a selector process.
    /// </remarks>
    public class SelectorEngine : ISelectorEngine
    {
        private readonly IServiceProvider _serviceProvider;


        private ILogger Logger
        {
            get
            {
                return _serviceProvider?.GetService<ILogger<SelectorEngine>>();
            }
        }


        private IDataSetDomain DataSetDomain
        {
            get
            {
                return _serviceProvider?.GetService<IDataSetDomain>();
            }
        }

        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }

        private ICriteriaDomain CriteriaDomain
        {
            get
            {
                return _serviceProvider?.GetService<ICriteriaDomain>();
            }
        }

        private IValueObjectDomain ValueObjectDomain
        {
            get
            {
                return _serviceProvider?.GetService<IValueObjectDomain>();
            }
        }

        private ISelectorInstanceDomain SelectorInstanceDomain
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorInstanceDomain>();
            }
        }

        private IGridConfigurationDomain GridConfigurationDomain
        {
            get
            {
                return _serviceProvider?.GetService<IGridConfigurationDomain>();
            }
        }

        private IDataSetDimensionDomain DataSetDimensionDomain
        {
            get
            {
                return _serviceProvider?.GetService<IDataSetDimensionDomain>();
            }
        }

        private ICriteriaValuesDomain CriteriaValuesDomain
        {
            get
            {
                return _serviceProvider?.GetService<ICriteriaValuesDomain>();
            }
        }

        private IActionSequenceDomain ActionSequenceDomain
        {
            get
            {
                return _serviceProvider?.GetService<IActionSequenceDomain>();
            }
        }

        private IConstraintSequenceDomain ConstraintSequenceDomain
        {
            get
            {
                return _serviceProvider?.GetService<IConstraintSequenceDomain>();
            }
        }


        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="serviceProvider">Fournisseur des services</param>
        public SelectorEngine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Génére tous les SelectorInstance depuis les criteria donnés.
        /// </summary>
        /// <param name="selectConf">SelectorConfig cible</param>
        /// <param name="wfInstance">WorkflowInstance nouvellement démarré</param>
        /// <param name="parent">Eventuel SelectorInstance pouvant être à l'origine de la création de nouvelles instances</param>
        /// <returns>Message de résultat</returns>
        public async Task<HttpResponseMessageResult> GenerateSelectorsInstances(SelectorConfig selectConf, WorkflowInstance wfInstance, SelectorInstance parent = null)
        {
            if (selectConf == null)
                throw new SequenceException("Process.SelectorConfig.Init: SelectorConfig are null.");

            if (selectConf.Criterias == null)
                throw new DataLoadingException("SelectorConfig : Criterias not loaded!");

            // WOR-174 - Créer en chaine les SelectorInstance selon la liste de Criterias...
            // On passe directement les criteria valorisés dans la requête de sélection...
            IEnumerable<IEnumerable<CriteriaValues>> lstCV = await CriteriaDomain.ExtractAllCriteriaValues(selectConf.Criterias, wfInstance);
            IEnumerable<IEnumerable<CriteriaValues>> lstCrit = await GetValueObjectsFromCriteria(lstCV, wfInstance.DataSetId);

            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
            foreach (IEnumerable<CriteriaValues> cvs in lstCrit)
            {
                SelectorInstance si = await SelectorInstanceDomain.Create(selectConf, cvs, parent, wfInstance);
                UnitOfWork.SelectorInstanceRepository.PrepareUpdateForObject(si);
                res.Append(await NextStep(si, wfInstance));
            }

            return res;
        }


        /// <summary>
        /// Modifie les données d'un SelectorInstance. Les données sont d'abord sauvées, puis le SelectorInstance passe en Act et en Constraints.
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <remarks>Les valeurs à modifier sont au format suivant : {id de la cellule}:{nouvelle valeur}</remarks>
        /// <returns>Message à modifier</returns>
        public async Task<HttpResponseMessageResult> SaveData(long selectorInstanceId, IEnumerable<KeyValuePair<long, double>> values)
        {
            // Création de la transaction
            using (IDbContextTransaction transaction = UnitOfWork.GetDbContext().Database.BeginTransaction())
            {
                SessionStatsHelper.HttpHitSaveDBTransaction(transaction, _serviceProvider);

                // Recherche du selectorInstance
                List<SelectorInstance> lstSelectInst = await UnitOfWork.GetDbContext().SelectorInstance
                    .Where(si => si.Id == selectorInstanceId)
                    .Include(si => si.SelectorConfig)
                    .Include(si => si.WorkflowInstance)
                    .ThenInclude(wfi => wfi.WorkflowConfig)
                    .ToAsyncEnumerable()
                    .ToList();
                SelectorInstance selectorInstance = lstSelectInst.FirstOrDefault();

                if (selectorInstance == null)
                    throw new WrongParameterException($"SelectorEngine.SaveData: bad SelectorInstance Id ({selectorInstanceId}).");

                // On pousse les valeurs volatiles vers les futures valeurs.
                HttpResponseMessageResult res = await SelectorInstanceDomain.PushVolatileToFuture(selectorInstance);

                // Passage aux étapes suivantes
                UnitOfWork.SelectorInstanceRepository.PrepareUpdateForObject(selectorInstance);
                res.Append(await NextStep(selectorInstance, selectorInstance.WorkflowInstance, SelectorStateEnum.Init, values));

                transaction.Commit();
                return res;
            }
        }



        /// <summary>
        /// Pour un SelectorInstance donné, passe à l'étape suivant dans le flux.
        /// </summary>
        /// <param name="selectIns">SelectorInstance sujet de la transition</param>
        /// <param name="wfInstance">Workflow instance</param>
        /// <returns>Message</returns>
        public async Task<HttpResponseMessageResult> NextStep(SelectorInstance selectIns, WorkflowInstance wfInstance, SelectorStateEnum scope = SelectorStateEnum.Void, IEnumerable<KeyValuePair<long, double>> values = null)
        {
            if (selectIns == null)
                throw new SequenceException("Process.SelectorInstance.NextStep: SelectorInstance is null.");

            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            switch (selectIns.PreviousStatus)
            {
                case SelectorStateEnum.Create:
                    if (selectIns.Status == SelectorStateEnum.Create)
                    {
                        selectIns.Status = SelectorStateEnum.PrevPropagate;
                        res = await PrevPropagate(selectIns, wfInstance);
                        selectIns.PreviousStatus = selectIns.Status;
                    }
                    if (selectIns.Status == SelectorStateEnum.PrevPropagate)
                    {
                        selectIns.Status = SelectorStateEnum.Init;
                        res = await Init(selectIns, wfInstance);
                        selectIns.PreviousStatus = SelectorStateEnum.Init;
                    }
                    break;
                case SelectorStateEnum.PrevPropagate:
                    selectIns.Status = SelectorStateEnum.Init;
                    res = await Init(selectIns, wfInstance);
                    selectIns.PreviousStatus = SelectorStateEnum.Init;
                    break;
                case SelectorStateEnum.Init:
                case SelectorStateEnum.Constraint:
                    selectIns.Status = SelectorStateEnum.Modify;
                    res = await Modify(selectIns, wfInstance, values);
                    selectIns.PreviousStatus = SelectorStateEnum.Modify;
                    selectIns.Status = SelectorStateEnum.Act;
                    res.Append(await Act(selectIns, wfInstance, values));
                    selectIns.PreviousStatus = SelectorStateEnum.Act;
                    selectIns.Status = SelectorStateEnum.Constraint;
                    res.Append(await Constraint(selectIns, wfInstance, values));
                    selectIns.PreviousStatus = SelectorStateEnum.Constraint;
                    selectIns.Status = SelectorStateEnum.Modify;
                    if (scope == SelectorStateEnum.Validate)
                    {
                        selectIns.Status = SelectorStateEnum.Validate;
                        res.Append(await Validate(selectIns, wfInstance));
                        selectIns.PreviousStatus = SelectorStateEnum.Validate;
                    }
                    break;
                case SelectorStateEnum.Modify:
                    selectIns.Status = SelectorStateEnum.Act;
                    res.Append(await Act(selectIns, wfInstance, values));
                    selectIns.PreviousStatus = SelectorStateEnum.Act;
                    selectIns.Status = SelectorStateEnum.Constraint;
                    res.Append(await Constraint(selectIns, wfInstance, values));
                    selectIns.PreviousStatus = SelectorStateEnum.Constraint;
                    selectIns.Status = SelectorStateEnum.Modify;
                    if (scope == SelectorStateEnum.Validate)
                    {
                        selectIns.Status = SelectorStateEnum.Validate;
                        res.Append(await Validate(selectIns, wfInstance));
                        selectIns.PreviousStatus = SelectorStateEnum.Validate;
                    }
                    break;
                case SelectorStateEnum.Act:
                    selectIns.Status = SelectorStateEnum.Constraint;
                    res.Append(await Constraint(selectIns, wfInstance, values));
                    selectIns.PreviousStatus = SelectorStateEnum.Constraint;
                    selectIns.Status = SelectorStateEnum.Modify;
                    if (scope == SelectorStateEnum.Validate)
                    {
                        selectIns.Status = SelectorStateEnum.Validate;
                        res.Append(await Validate(selectIns, wfInstance));
                        selectIns.PreviousStatus = SelectorStateEnum.Validate;
                    }
                    break;

                //...

                case SelectorStateEnum.Finish:
                    selectIns.Status = SelectorStateEnum.Finish;
                    res = await Finish(selectIns, wfInstance);
                    selectIns.PreviousStatus = SelectorStateEnum.Finish;
                    break;
            }

            // Afin de sauver les modifications d'états
            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            return res;
        }


        /// <summary>
        /// Déclenche le PrevPropagate sur l'ensemble des selectorInstances issus du SelectorConfig original.
        /// </summary>
        /// <param name="selectIns">SelectorInstance</param>
        /// <param name="wfInstance">Workflow instance</param>
        /// <returns>Message</returns>
        public async Task<HttpResponseMessageResult> PrevPropagate(SelectorInstance selectIns, WorkflowInstance wfInstance)
        {
            if ((selectIns == null) || (selectIns.SelectorConfig == null))
                throw new SequenceException("Process.SelectorInstance.PrevPropagate: SelectorInstance or SelectorInstance.SelectorConfig is null.");

            SelectorConfig selectConf = selectIns.SelectorConfig;

            // S'il y a d'autres SelectorConfig en PrevPropagate, on les lance avant
            if (selectConf.PrevPropagateId > 0)
            {
                SelectorConfig prevSelectConf = await UnitOfWork.GetDbContext()
                    .SelectorConfig
                    .Include(sc => sc.Criterias)
                    .ThenInclude(c => c.Dimension)
                    .Where(sc => sc.Id == selectConf.PrevPropagateId)
                    .AsNoTracking()
                    .ToAsyncEnumerable()
                    .FirstOrDefault();

                if (prevSelectConf == null)
                    throw new SequenceException("Process.SelectorConfig.Init: Bad PrevPropagate Id.");

                HttpResponseMessageResult prevRes = await GenerateSelectorsInstances(prevSelectConf, wfInstance, selectIns);

                return prevRes;
            }


            return await NextStep(selectIns, wfInstance);
        }


        /// <summary>
        /// This function permits to do a INIT action.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">Workflow instance</param>
        /// <returns>Message</returns>
        public async Task<HttpResponseMessageResult> Init(SelectorInstance selectorInstance, WorkflowInstance wfInstance)
        {
            if ((selectorInstance == null) || (wfInstance == null))
                throw new WrongParameterException("Process.SelectorInstance.Init: SelectorInstance or WorkflowInstance are null!");

            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            // Sélection des données et enregistrement des liaisons (subset)
            ICollection<CriteriaValues> lstCv = selectorInstance.CriteriaValues;
            Dictionary<long, DataSetDimension> dimIds = await DataSetDimensionDomain.GetDimensionColumns(wfInstance.DataSetId);
            List<Expression<Func<ValueObject, bool>>> where = new List<Expression<Func<ValueObject, bool>>>();

            res.Append(ValueObjectDomain.BuildFilterRequest(where, new List<IEnumerable<CriteriaValues>>() { lstCv }, dimIds, true));

            IQueryable<ValueObject> dbQuery = UnitOfWork.GetDbContext().Set<ValueObject>();
            // Vérification que le SelectorInstance n'utilise pas les données d'un autre SelectorInstance.
            int nbrLinkVoSi = await where
                .Aggregate(dbQuery, (current, predicate) => current.Where(predicate))
                .Include(vo => vo.SelectorInstanceValueObject)
                .Where(vo => vo.SelectorInstanceValueObject.Count > 0)
                .AsNoTracking()
                .CountAsync();

            if (nbrLinkVoSi > 0)
                throw new SequenceException($"SelecotrEngine.Init: Can't allocate data for SelectorInstance (from {selectorInstance?.SelectorConfig?.Name}) because there are already SelectorInstance for data.");

            IEnumerable<ValueObject> lstValueObject = await where
                .Aggregate(dbQuery, (current, predicate) => current.Where(predicate))
                .Include(vo => vo.DataSet)
                .Where(vo => vo.DataSet.Id == wfInstance.DataSetId)
                //.AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            selectorInstance.SelectorInstanceValueObject = new List<SelectorInstanceValueObject>();
            foreach (ValueObject vo in lstValueObject)
            {
                vo.FutureValue = null;
                SelectorInstanceValueObject sivo = new SelectorInstanceValueObject()
                {
                    SelectorInstance = selectorInstance,
                    ValueObject = vo
                };
                sivo.InitDatesAndUser("");

                selectorInstance.SelectorInstanceValueObject.Add(sivo);
                vo.SelectorInstanceValueObject.Add(sivo);
                vo.Status = ValueObjectStatusEnum.Modify;
            }
            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            // Création de la table temporaire de données...
            res.Append(await GridConfigurationDomain.CreateDataTableDB(selectorInstance, wfInstance));

            // Recherche des modificateurs
            res.Append(await SelectorInstanceDomain.FindModificator(selectorInstance, wfInstance));

            // Recherche des validateurs
            res.Append(await SelectorInstanceDomain.FindValidators(selectorInstance, wfInstance));

            // On régle les données modifiables
            res.Append(await SelectorInstanceDomain.SetModifyData(selectorInstance, wfInstance, dimIds));

            return res;
        }


        /// <summary>
        /// This function permits to do a MODIFY action.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">Workflow instance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message</returns>
        public async Task<HttpResponseMessageResult> Modify(SelectorInstance selectorInstance, WorkflowInstance wfInstance, IEnumerable<KeyValuePair<long, double>> values)
        {
            // Mise à jour de la table ValueObject
            IEnumerable<long> idsUpdated = values.Select(v => v.Key);
            List<ValueObject> lstVO = await UnitOfWork.GetDbContext().ValueObject
                .Where(vo => idsUpdated.Contains(vo.Id))
                .ToAsyncEnumerable()
                .ToList();
            Dictionary<long, double> dicVO = values.ToDictionary(k => k.Key, v => v.Value);
            foreach (ValueObject vo in lstVO)
            {
                if (!dicVO.ContainsKey(vo.Id))
                    continue;
                UnitOfWork.ValueObjectRepository.PrepareUpdateForObject(vo);
                vo.FutureValue = dicVO[vo.Id];
            }
            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            // Mise à jour de la table temporaire
            HttpResponseMessageResult res = await GridConfigurationDomain.SaveDataInTemporyTable(selectorInstance, selectorInstance.WorkflowInstance, values);

            return res;
        }

        /// <summary>
        /// This function permits to do a ACT action.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">Workflow instance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> Act(SelectorInstance selectorInstance, WorkflowInstance wfInstance, IEnumerable<KeyValuePair<long, double>> values)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            if (selectorInstance == null)
                throw new WrongParameterException("SelectorEngine.Act : SelectorInstance is null.");
            if (selectorInstance.SelectorConfig == null)
                throw new DataLoadingException($"SelectorEngine.Act : SelectorConfig of SelectorInstance (Id = {selectorInstance.Id}) is null.");
            if (wfInstance == null)
                throw new WrongParameterException("SelectorEngine.Act : WorkflowInstance is null.");
            if (wfInstance.WorkflowConfig == null)
                throw new DataLoadingException($"SelectorEngine.Act : WorkflowConfig of WorkflowInstance (Id = {wfInstance.Id}) is null.");

            long refSeq = -1;
            if (selectorInstance.SelectorConfig.ActionSequenceRef >= 0)
                refSeq = selectorInstance.SelectorConfig.ActionSequenceRef;
            else
                if (wfInstance.WorkflowConfig.ActionSequenceRef >= 0)
                    refSeq = wfInstance.WorkflowConfig.ActionSequenceRef;

            if (refSeq >= 0)
            {
                List<ActionSequence> lstSeqActions = await UnitOfWork.GetDbContext().ActionSequence
                    .Where(a => a.Reference == refSeq)
                    .OrderBy(a => a.Order)
                    .Include(a => a.Action)
                    .AsNoTracking()
                    .ToAsyncEnumerable()
                    .ToList();

                res.Append(await ActionSequenceDomain.ExecuteActionSequence(lstSeqActions, selectorInstance, wfInstance, values));
            }

            return res;
        }



        /// <summary>
        /// Etabli l'étape Constraint.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">Workflow Instance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> Constraint(SelectorInstance selectorInstance, WorkflowInstance wfInstance, IEnumerable<KeyValuePair<long, double>> values)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            if (selectorInstance == null)
                throw new WrongParameterException("SelectorEngine.Constraint : SelectorInstance is null.");
            if (selectorInstance.SelectorConfig == null)
                throw new DataLoadingException($"SelectorEngine.Constraint : SelectorConfig of SelectorInstance (Id = {selectorInstance.Id}) is null.");
            if (wfInstance == null)
                throw new WrongParameterException("SelectorEngine.Constraint : WorkflowInstance is null.");
            if (wfInstance.WorkflowConfig == null)
                throw new DataLoadingException($"SelectorEngine.Constraint : WorkflowConfig of WorkflowInstance (Id = {wfInstance.Id}) is null.");

            long refSeq = -1;
            if (selectorInstance.SelectorConfig.ConstraintSequenceRef >= 0)
                refSeq = selectorInstance.SelectorConfig.ConstraintSequenceRef;
            else
                if (wfInstance.WorkflowConfig.ConstraintSequenceRef >= 0)
                refSeq = wfInstance.WorkflowConfig.ConstraintSequenceRef;

            if (refSeq >= 0)
            {
                List<ConstraintSequence> lstSeqConstraints = await UnitOfWork.GetDbContext().ConstraintSequence
                    .Where(a => a.Reference == refSeq)
                    .OrderBy(a => a.Order)
                    .Include(a => a.Constraint)
                    .AsNoTracking()
                    .ToAsyncEnumerable()
                    .ToList();

                res.Append(await ConstraintSequenceDomain.CheckConstraintSequence(lstSeqConstraints, selectorInstance, wfInstance, values));
            }

            return res;
        }


        /// <summary>
        /// This function permits to do a VALIDATE action.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">Workflow Instance</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> Validate(SelectorInstance selectorInstance, WorkflowInstance wfInstance)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            return res;
            //selectorInstance.Status = SelectorStateEnum.Valid;

            //await SelectorInstanceDomain.Update(selectorInstance);

            //IEnumerable<ValueObject> valueObjects = await ValueObjectDomain.Get();

            //if (valueObjects.Any(s => s.SelectionInstance == selectorInstance
            //&& s.Status == ValueObjectStatusEnum.Locked))
            //{
            //    await Commit(selectorInstance);
            //}
            //else
            //{
            //    //await StartModify(selectorInstance);
            //}
        }


        public async Task<HttpResponseMessageResult> Finish(SelectorInstance selectorInstance, WorkflowInstance wfInstance)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult();

            if (selectorInstance.ParentSelectorInstanceId != 0)
            {
                // Construire l'appel pour passer en NoWait le parent.
            }

            return res;
        }



        private async Task<IEnumerable<IEnumerable<CriteriaValues>>> GetValueObjectsFromCriteria(IEnumerable<IEnumerable<CriteriaValues>> criteriaValues, long dataSetId)
        {
            List<Expression<Func<ValueObject, bool>>> where = new List<Expression<Func<ValueObject, bool>>>();
            Dictionary<long, DataSetDimension> dimIds = await DataSetDimensionDomain.GetDimensionColumns(dataSetId);

            HttpResponseMessageResult res = ValueObjectDomain.BuildFilterRequest(where, criteriaValues, dimIds, false);

            Dictionary<long, IEnumerable<CriteriaValues>> dico = CriteriaValuesDomain.GetCriteriaValuesByDimension(criteriaValues);

            IQueryable<ValueObject> dbQuery = UnitOfWork.GetDbContext().Set<ValueObject>();
            IEnumerable<ValueObject> lstValueObject = await where
                .Aggregate(dbQuery, (current, predicate) => current.Where(predicate))
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            Dictionary<string, List<CriteriaValues>> dicoProduitCartesien = new Dictionary<string, List<CriteriaValues>>();

            foreach (ValueObject vo in lstValueObject)
            {
                List<CriteriaValues> lstCV = new List<CriteriaValues>();

                if (vo.Dimension1 != null)
                    lstCV.Add(GetCriteriaValuesFromDimension(Constant.DATA_DIMENSION_1, cv => cv.Value == vo.Dimension1, dimIds, dico));
                if (vo.Dimension2 != null)
                    lstCV.Add(GetCriteriaValuesFromDimension(Constant.DATA_DIMENSION_2, cv => cv.Value == vo.Dimension2, dimIds, dico));
                if (vo.Dimension3 != null)
                    lstCV.Add(GetCriteriaValuesFromDimension(Constant.DATA_DIMENSION_3, cv => cv.Value == vo.Dimension3, dimIds, dico));
                if (vo.Dimension4 != null)
                    lstCV.Add(GetCriteriaValuesFromDimension(Constant.DATA_DIMENSION_4, cv => cv.Value == vo.Dimension4, dimIds, dico));
                if (vo.Dimension5 != null)
                    lstCV.Add(GetCriteriaValuesFromDimension(Constant.DATA_DIMENSION_5, cv => cv.Value == vo.Dimension5, dimIds, dico));
                if (vo.Dimension6 != null)
                    lstCV.Add(GetCriteriaValuesFromDimension(Constant.DATA_DIMENSION_6, cv => cv.Value == vo.Dimension6, dimIds, dico));
                if (vo.Dimension7 != null)
                    lstCV.Add(GetCriteriaValuesFromDimension(Constant.DATA_DIMENSION_7, cv => cv.Value == vo.Dimension7, dimIds, dico));
                if (vo.Dimension8 != null)
                    lstCV.Add(GetCriteriaValuesFromDimension(Constant.DATA_DIMENSION_8, cv => cv.Value == vo.Dimension8, dimIds, dico));
                if (vo.Dimension9 != null)
                    lstCV.Add(GetCriteriaValuesFromDimension(Constant.DATA_DIMENSION_9, cv => cv.Value == vo.Dimension9, dimIds, dico));
                if (vo.Dimension10 != null)
                    lstCV.Add(GetCriteriaValuesFromDimension(Constant.DATA_DIMENSION_10, cv => cv.Value == vo.Dimension10, dimIds, dico));

                lstCV = lstCV.Where(cv => cv != null).ToList();
                string keyLstCv = GetUniqueKeyFromCriteriaValues(lstCV);
                if (!dicoProduitCartesien.ContainsKey(keyLstCv))
                    dicoProduitCartesien.Add(keyLstCv, lstCV);
            }

            return dicoProduitCartesien.Select(kv => kv.Value);
        }

        private CriteriaValues GetCriteriaValuesFromDimension(string colDimName, Func<CriteriaValues, bool> predicat, Dictionary<long, DataSetDimension> dimIds, Dictionary<long, IEnumerable<CriteriaValues>> dico)
        {
            long idDim = dimIds.Where(kv => kv.Value.ColumnName == colDimName).Select(kv => kv.Value.Dimension.Id).FirstOrDefault();
            if ((idDim != 0) && dico.ContainsKey(idDim))
            {
                // Important, créer un nouvel objet pour chaque CriteriaValues, vu que chaque SelectorInstance doit avoir ses propres cv
                CriteriaValues cvLight = dico[idDim].Where(predicat).FirstOrDefault();
                if (cvLight != null)
                    return new CriteriaValues() { Criteria = cvLight.Criteria, Value = cvLight.Value };
            }
            return null;
        }

        private string GetUniqueKeyFromCriteriaValues(IEnumerable<CriteriaValues> lstCV)
        {
            StringBuilder key = new StringBuilder();
            foreach (CriteriaValues cv in lstCV.OrderBy(c => c.Criteria.Dimension.TypeDimension))
            {
                key.Append(cv.Criteria.Dimension.TypeDimension);
                key.Append("#");
                key.Append(cv.Value);
                key.Append("|");
            }

            return key.ToString();
        }





        /// <summary>
        /// This function permits to do a COMMIT action.
        /// </summary>
        /// <remarks>
        /// This method can to execute from the frontal
        /// </remarks>
        /// <param name="selectorInstance">SelectorInstance</param>
        public async Task Commit(SelectorInstance selectorInstance)
        {
            selectorInstance.Status = SelectorStateEnum.Commit;

            await SelectorInstanceDomain.Update(selectorInstance);

            await PropagateAsync(selectorInstance);
        }

        /// <summary>
        /// This function permits to do a PROPAGATE action.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        public async Task PropagateAsync(SelectorInstance selectorInstance)
        {
            //try
            //{
            //    selectorInstance.Status = SelectorStateEnum.Propagate;

            //    await SelectorInstanceDomain.Update(selectorInstance);

            //    // Create each sub selector instances
            //    Task<IEnumerable<SelectorInstance>> taskResult = SelectorInstanceDomain.CreateAllInstances(selectorInstance.WorkflowInstance);
            //    await taskResult.ContinueWith(t =>
            //    {
            //        if (t.IsCompleted)
            //        {
            //            foreach (SelectorInstance instance in t.Result)
            //            {
            //                 //Init(instance).Start();
            //            }
            //        }
            //    });

            //    //selectorInstance.Status = SelectorStateEnum.Rest;

            //    await SelectorInstanceDomain.Update(selectorInstance);

            //    await Task.FromResult(0);
            //}
            //catch
            //{
            //    throw new SequenceException("Process.Selector.Propagate: Error while propagating.");
            //}
        }

        /// <summary>
        /// This function permits to update values from the workflow.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        public async Task UpdateFutureValues(SelectorInstance selectorInstance)
        {
            IEnumerable<ValueObject> valueObjects = await ValueObjectDomain.Get();
            foreach (ValueObject valueObject in valueObjects.Where(s => s.SelectionInstance == selectorInstance))
            {
                if (valueObject.Status == ValueObjectStatusEnum.Rest)
                {
                    valueObject.Status = ValueObjectStatusEnum.Modify;
                    valueObject.FutureValue = valueObject.CurrentValue;
                    valueObject.CurrentValue = Convert.ToDouble(null);

                    await ValueObjectDomain.Update(valueObject);
                }
                else
                {
                    throw new SequenceException("Process.Selector.UpdateFutureValues: ValueObject (ID=" + valueObject.Id + ") is not in Rest state.");
                }
            }
        }
    }
}
