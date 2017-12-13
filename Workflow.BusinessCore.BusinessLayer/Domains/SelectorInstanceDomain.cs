using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.DataLayer.Repositories;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.BusinessLayer.Helpers;
using Workflow.Transverse.Environment;
using System.Linq.Expressions;
using Workflow.BusinessCore.DataLayer.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// SelectorInstance domain class
    /// </summary>
    public class SelectorInstanceDomain : AbstractDomain<SelectorInstance>, ISelectorInstanceDomain
    {
        // On régle le problème des références cycliques du DI : le constructeur n'instancie pas les services, c'est seulement lors de leur première utilisation
        // qu'on les instancie. Il peut y avoir un appel cyclique, mais seulement entre les méthodes des services, ce qui descend la responsabilité au niveau de la
        // structure algorithmique (normal et même utilisé dans la récursivité) et non plus au niveau du framework.

        private readonly IServiceProvider _serviceProvider = null;

        private ISelectorConfigDomain SelectorConfigDomain
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorConfigDomain>();
            }
        }
        private ICriteriaDomain CriteriaDomain
        {
            get
            {
                return _serviceProvider?.GetService<ICriteriaDomain>();
            }
        }

        private IUserSetDomain UserSetDomain
        {
            get
            {
                return _serviceProvider?.GetService<IUserSetDomain>();
            }
        }

        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }

        private IDataSetDimensionDomain DataSetDimensionDomain
        {
            get
            {
                return _serviceProvider?.GetService<IDataSetDimensionDomain>();
            }
        }

        private IValueObjectDomain ValueObjectDomain
        {
            get
            {
                return _serviceProvider?.GetService<IValueObjectDomain>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        public SelectorInstanceDomain(
            IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider
            )
            : base(unitOfWork.SelectorInstanceRepository)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Permet d'instancier un nouvel SelectorInstance.
        /// </summary>
        /// <param name="selectConf">SelectorConfig à l'origine de l'instance</param>
        /// <param name="lstCriteriaValue">Liste des criteriaValue qui définissent la selection de données</param>
        /// <param name="parent">Eventuel SelectorInstance pouvant être à l'origine de la création</param>
        /// <returns>Instance du nouvel SelectorInstance</returns>
        public async Task<SelectorInstance> Create(SelectorConfig selectConf, IEnumerable<CriteriaValues> lstCriteriaValue, SelectorInstance parent, WorkflowInstance wfInstance)
        {
            if ((selectConf == null) || (wfInstance == null))
                throw new WrongParameterException("Process.SelectorInstance.Create: SelectorConfig or WorkflowInstance is null.");

            SelectorInstance si = new SelectorInstance();
            UnitOfWork.SelectorInstanceRepository.PrepareAddForObject(si);

            // TODO : Completer l'instantation
            si.Status = SelectorStateEnum.Create;
            si.PreviousStatus = SelectorStateEnum.Create;
            si.ChainNumberModifyer = -1; // Pas de modificateur sélectionné
            si.SelectorConfig = selectConf;
            si.WorkflowInstance = wfInstance;

            si.ParentSelectorInstanceId = 0;
            if (parent != null)
                si.ParentSelectorInstanceId = parent.Id;

            // Enregistrement des CriteriaValues
            si.CriteriaValues = new List<CriteriaValues>();
            foreach (CriteriaValues cv in lstCriteriaValue)
            {
                await UnitOfWork.CriteriaValuesRepository.Insert(cv);
                si.CriteriaValues.Add(cv);
            }


            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();
            return si;
        }

        /// <summary>
        /// Recherche du modificateur pour le SelectorInstance donné en paramétre.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">WorkflowInstance</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> FindModificator(SelectorInstance selectorInstance, WorkflowInstance wfInstance)
        {
            if (selectorInstance == null)
                throw new WrongParameterException($"SelectorInstanceDomain.FindModificator: SelectorInstance is null.");
            if (selectorInstance.SelectorConfig == null)
                throw new WrongParameterException($"SelectorInstanceDomain.FindModificator: SelectorConfig is null (Id SelectorInstance : {selectorInstance.Id}).");
            if (selectorInstance.SelectorConfig.Modifiers == null)
                throw new SequenceException($"SelectorInstance.FindModificator : no modificator found, error in SelectorConfig (no Modifiers list). Id SelectorConfig : {selectorInstance.SelectorConfig.Id}");

            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
            IEnumerable<IGrouping<int, Criteria>> lstGrp = selectorInstance.SelectorConfig.Modifiers.GroupBy(c => c.ChainNumber);

            int currentChain = -1;
            if ((lstGrp != null) && (lstGrp.Count() > 0))
                currentChain = lstGrp.Min(g => g.Key);
            User modificator = null;
            IEnumerable<UserSetUser> lstUsu = null;
            IGrouping<int, Criteria> grp = null;

            // On parcourt les chaines de criteria une à une afin de trouver le modificateur.
            while ((currentChain > -1) && (modificator == null))
            {
                grp = lstGrp.Where(g => g.Key == currentChain).FirstOrDefault();
                if (grp == null)
                    throw new SequenceException($"SelectorInstance.FindModificator : no modificator found, error in SelectorConfig (no group). Id SelectorConfig : {selectorInstance.SelectorConfig.Id}");

                IEnumerable<IEnumerable<CriteriaValues>> lstCv = await CriteriaDomain.ExtractAllCriteriaValues(grp, wfInstance);
                lstUsu = await UserSetDomain.GetUsersByRight(lstCv, wfInstance.DataSetId, RightEnum.Modification);
                IEnumerable<User> modificators = lstUsu.Select(usu => usu.User).Distinct(new UserComparer((x, y) => x.Id == y.Id));

                // Gestion des cas des modificateurs
                if ((modificators == null) || (modificators.Count() == 0))
                {
                    IEnumerable<int> lstNumber = lstGrp.Select(g => g.Key).Where(i => i > currentChain);
                    if (lstNumber.Count() > 0)
                        currentChain = lstNumber.OrderBy(i => i).First();
                    else
                        throw new ConfigurationException("SelectorInstanceDomain.FindModificator : no modificator was found.");
                }
                if (modificators.Count() > 1)
                    throw new ConfigurationException("SelectorInstanceDomain.FindModificator : more than one modificator was found.");
                if (modificators.Count() == 1)
                    modificator = modificators.ElementAt(0);
            }

            if (modificator == null)
                throw new DataLoadingException($"SelectorInstanceDomain.FindModificator : No modificator found for SelectorInstance (Id : {selectorInstance.Id}).");

            // Régler le SelectorInstance avec l'id de l'utilisateur trouvé.
            selectorInstance.ChainNumberModifyer = currentChain;
            selectorInstance.ModifyerId = modificator.Id;
            UserSetUser usuDistinct = lstUsu.FirstOrDefault();
            if (usuDistinct == null)
                throw new DataLoadingException("SelectorInstanceDomain.FindModificator : a problem with loading UserSetUser must stop this finding.");

            Dictionary<long, DataSetDimension> dimIds = await DataSetDimensionDomain.GetDimensionColumns(wfInstance.DataSetId);
            foreach (Criteria crit in grp)
            {
                string dimName = dimIds.Values.Where(v => v.Dimension.Id == crit.Dimension.Id).Select(v => v.ColumnName).FirstOrDefault();
                string value = GetValueFromDimension(usuDistinct, dimName);
                CriteriaValues cv = new CriteriaValues() { Criteria = crit, SelectorInstanceModifier = selectorInstance, Value = value };
                selectorInstance.ModifyCriteriasValues.Add(cv);
            }

            return res;
        }

        /// <summary>
        /// Sélectionne les données modifiables du SelectorInstance.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">WorkflowInstance</param>
        /// <param name="idsDimensionDS">Dictionnaire de DimensionDataSet par Id</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> SetModifyData(SelectorInstance selectorInstance, WorkflowInstance wfInstance, Dictionary<long, DataSetDimension> idsDimensionDS)
        {
            if (selectorInstance == null)
                throw new WrongParameterException($"SelectorInstanceDomain.SetModifyData: SelectorInstance is null.");
            if (selectorInstance.SelectorConfig == null)
                throw new WrongParameterException($"SelectorInstanceDomain.SetModifyData: SelectorConfig is null (Id SelectorInstance : {selectorInstance.Id}).");
            if (selectorInstance.SelectorConfig.ModifyCriterias == null)
                throw new SequenceException($"SelectorInstance.SetModifyData : no ModifyCriterias for values found, error in SelectorConfig. Id SelectorConfig : {selectorInstance.SelectorConfig.Id}");

            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            IEnumerable<IEnumerable<CriteriaValues>> lstCv = await CriteriaDomain.ExtractAllCriteriaValues(selectorInstance.SelectorConfig.ModifyCriterias, wfInstance);
            List<Expression<Func<ValueObject, bool>>> where = new List<Expression<Func<ValueObject, bool>>>();

            res.Append(ValueObjectDomain.BuildFilterRequest(where, lstCv, idsDimensionDS, false));

            if (where.Count > 0)
            {
                IQueryable<ValueObject> dbQuery = UnitOfWork.GetDbContext().Set<ValueObject>();
                IEnumerable<long> lstIdsValueObject = await where
                    .Aggregate(dbQuery, (current, predicate) => current.Where(predicate))
                    .Include(vo => vo.SelectorInstanceValueObject)
                    .Where(vo => vo.SelectorInstanceValueObject.Any(sivo => sivo.SelectorInstanceId == selectorInstance.Id))
                    .Select(vo => vo.Id)
                    .ToAsyncEnumerable()
                    .ToList();

                foreach (SelectorInstanceValueObject subSivo in selectorInstance.SelectorInstanceValueObject.Where(sivo => lstIdsValueObject.Contains(sivo.ValueObjectId)))
                    subSivo.IsEditable = true;

                int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();
            }

            return res;
        }


        /// <summary>
        /// Peremt de pousser les valeurs volatiles de l'ensemble de ValueObject d'un SelectorInstance vers leurs valeurs futures respectivement.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <returns>Retour ok si tout se passe bien.</returns>
        public async Task<HttpResponseMessageResult> PushVolatileToFuture(SelectorInstance selectorInstance)
        {
            if (selectorInstance == null)
                throw new WrongParameterException("SelectorInstanceDomain.PushVolatileToFuture : SelectorInstance is null.");

            List<ValueObject> lstVo = await UnitOfWork.GetDbContext().SelectorInstanceValueObject
                .Include(suvo => suvo.ValueObject)
                .Where(suvo => suvo.SelectorInstanceId == selectorInstance.Id)
                .Select(suvo => suvo.ValueObject)
                .ToAsyncEnumerable()
                .ToList();

            foreach (ValueObject vo in lstVo)
            {
                if (vo.VolatileValue.HasValue)
                {
                    UnitOfWork.ValueObjectRepository.PrepareUpdateForObject(vo);
                    vo.FutureValue = vo.VolatileValue;
                    vo.VolatileValue = null;
                }
            }

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            return new HttpResponseMessageResult() { IsSuccess = true };
        }


        /// <summary>
        /// Récupére les informations supplémentaires pour un SelectorInstance.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <returns>Tuple de 3 info : le nom du modificateur, les noms des validateurs, la valeur de la 1ere dimension arbo.</returns>
        public async Task<Tuple<string, string, string>> GetAdditionnalInfo(SelectorInstance selectorInstance)
        {
            if (selectorInstance == null)
                throw new WrongParameterException("SelectorInstanceDomaine.GetAdditionnalInfo: SelectorInstance can't be null.");

            string noeud = await UnitOfWork.GetDbContext().CriteriaValues
                .Include(cv => cv.Criteria)
                .ThenInclude(c => c.Dimension)
                .Where(cv => cv.SelectorInstance.Id == selectorInstance.Id && cv.Criteria.Dimension.TypeDimension == DimensionTypeEnum.Tree)
                .Select(cv => cv.Value)
                .FirstOrDefaultAsync();

            // TODO : la requête en dessous plante (System.Data.SqlClient.SqlException: Only one expression can be specified in the select list when the subquery is not introduced with EXISTS.)
            // peut être à cause des ThenInclude imbriqués.
            //string noeud = await UnitOfWork.GetDbContext().SelectorInstance
            //    .Include(si => si.CriteriaValues)
            //    .ThenInclude(cv => cv.Criteria)
            //    .ThenInclude(c => c.Dimension)
            //    .Where(si => si.Id == selectorInstance.Id)
            //    .Select(si => si.CriteriaValues.Where(cv => cv.Criteria.Dimension.TypeDimension == DimensionTypeEnum.Tree).Select(cv => cv.Value).FirstOrDefault())
            //    .FirstOrDefaultAsync();

            string modificatorName = await UnitOfWork.GetDbContext().Users
                .Where(u => u.Id == selectorInstance.ModifyerId)
                .Select(u => u.Name)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            string validatorName = ""; // TODO, lorsqu'on aura la méthode de selection des validateurs.

            return Tuple.Create(modificatorName, validatorName, noeud);
        }


        /// <summary>
        /// Trouve les validateurs et les ajoute dans la liste des utilisateurs en validation.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">WorkflowInstance</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> FindValidators(SelectorInstance selectorInstance, WorkflowInstance wfInstance)
        {
            if (selectorInstance == null)
                throw new WrongParameterException($"SelectorInstanceDomain.FindValidators: SelectorInstance is null.");
            if (selectorInstance.SelectorConfig == null)
                throw new WrongParameterException($"SelectorInstanceDomain.FindValidators: SelectorConfig is null (Id SelectorInstance : {selectorInstance.Id}).");
            if (selectorInstance.SelectorConfig.Validators == null)
                throw new SequenceException($"SelectorInstance.FindValidators: no validator found, error in SelectorConfig (no Validators list). Id SelectorConfig : {selectorInstance.SelectorConfig.Id}");

            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
            IEnumerable<IGrouping<int, Criteria>> lstGrp = selectorInstance.SelectorConfig.Validators.GroupBy(c => c.ChainNumber);

            int currentChain = -1;
            if ((lstGrp != null) && (lstGrp.Count() > 0))
                currentChain = lstGrp.Min(g => g.Key);
            IEnumerable<User> validators = null;
            IEnumerable<UserSetUser> lstUsu = null;
            IGrouping<int, Criteria> grp = null;

            // On parcourt les chaines de criteria une à une afin de trouver le modificateur.
            while ((currentChain > -1) && ((validators == null) || (validators.Count() == 0)))
            {
                grp = lstGrp.Where(g => g.Key == currentChain).FirstOrDefault();
                if (grp == null)
                    throw new SequenceException($"SelectorInstance.FindValidators: no validator found, error in SelectorConfig (no group). Id SelectorConfig : {selectorInstance.SelectorConfig.Id}");

                IEnumerable<IEnumerable<CriteriaValues>> lstCv = await CriteriaDomain.ExtractAllCriteriaValues(grp, wfInstance);
                lstUsu = await UserSetDomain.GetUsersByRight(lstCv, wfInstance.DataSetId, RightEnum.Validation);
                validators = lstUsu.Select(usu => usu.User).Distinct(new UserComparer((x, y) => x.Id == y.Id));

                // Gestion des cas des modificateurs
                if ((validators == null) || (validators.Count() == 0))
                {
                    IEnumerable<int> lstNumber = lstGrp.Select(g => g.Key).Where(i => i > currentChain);
                    if (lstNumber.Count() > 0)
                        currentChain = lstNumber.OrderBy(i => i).First();
                    else
                        throw new ConfigurationException("SelectorInstanceDomain.FindValidators: no validator was found.");
                }
            }

            if (validators != null)
                foreach (User val in validators)
                {
                    SelectorInstanceUser siu = new SelectorInstanceUser()
                    {
                        SelectorInstance = selectorInstance,
                        User = val,
                        Right = RightEnum.Validation
                    };
                    UnitOfWork.SelectorInstanceUserRepository.PrepareAddForObject(siu);
                    siu.InitDatesAndUser("");

                    selectorInstance.SelectorInstanceUser.Add(siu);
                    val.SelectorInstanceUser.Add(siu);
                }

            return res;
        }

        private string GetValueFromDimension(UserSetUser usu, string dimensionName)
        {
            if (dimensionName == Constant.DATA_DIMENSION_1)
                return usu.Position1;
            if (dimensionName == Constant.DATA_DIMENSION_2)
                return usu.Position2;
            if (dimensionName == Constant.DATA_DIMENSION_3)
                return usu.Position3;
            if (dimensionName == Constant.DATA_DIMENSION_4)
                return usu.Position4;
            if (dimensionName == Constant.DATA_DIMENSION_5)
                return usu.Position5;
            if (dimensionName == Constant.DATA_DIMENSION_6)
                return usu.Position6;
            if (dimensionName == Constant.DATA_DIMENSION_7)
                return usu.Position7;
            if (dimensionName == Constant.DATA_DIMENSION_8)
                return usu.Position8;
            if (dimensionName == Constant.DATA_DIMENSION_9)
                return usu.Position9;
            if (dimensionName == Constant.DATA_DIMENSION_10)
                return usu.Position10;
            return null;
        }


    }
}
