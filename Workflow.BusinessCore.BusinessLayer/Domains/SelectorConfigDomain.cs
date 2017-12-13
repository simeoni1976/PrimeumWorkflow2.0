using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Workflow.Transverse.Helpers;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// SelectorConfig domain class
    /// </summary>
    public class SelectorConfigDomain : AbstractDomain<SelectorConfig>, ISelectorConfigDomain
    {
        private readonly IServiceProvider _serviceProvider = null;

        private const string _ruleDuplicateSelectorConfigName = @"^(?<name>.+)\s+\((?<number>\d+)\)$";
        private Regex _regexSubStringName;

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

        private ISelectorInstanceDomain SelectorInstanceDomain
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorInstanceDomain>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        public SelectorConfigDomain(
            IServiceProvider serviceProvider,
            IUnitOfWork unitOfWork
            ) : base(unitOfWork.SelectorConfigRepository)
        {
            _serviceProvider = serviceProvider;
            _regexSubStringName = new Regex(_ruleDuplicateSelectorConfigName);
        }



        /// <summary>
        /// Ajoute un SelectorConfig en base.
        /// </summary>
        /// <param name="selectConf">SelectorConfig à ajouter</param>
        /// <returns>Message du résultat</returns>
        /// <remarks>L'objet SelectorConfig doit connaitre l'id de son WorkflowConfig parent. De plus, il doit avoir un nom unique. 
        /// L'opération sort en erreur si l'une des deux conditions, ou les deux, n'est pas respectée.</remarks>
        public override async Task<SelectorConfig> Add(SelectorConfig selectConf)
        {
            // Création de la transaction
            using (IDbContextTransaction transaction = UnitOfWork.GetDbContext().Database.BeginTransaction())
            {
                // Vérifications
                if (selectConf == null)
                    throw new WrongParameterException("SelectorConfig object is null!");
                if (selectConf.WorkflowConfig == null)
                    throw new DataLoadingException("SelectorConfig.WorkflowConfig don't load!");
                WorkflowConfig wfConf = await UnitOfWork.WorkflowConfigRepository.GetById(selectConf.WorkflowConfig.Id);
                if (wfConf == null)
                    throw new WrongParameterException("WorkflowConfig id doesn't exist!");
                if (string.IsNullOrWhiteSpace(selectConf.Name))
                    throw new WrongParameterException("SelectorConfig haven't a name!");
                IEnumerable<SelectorConfig> duplicata = await UnitOfWork.GetDbContext().Set<SelectorConfig>().Include(i => i.WorkflowConfig).Where(s => (s.WorkflowConfig != null && s.WorkflowConfig.Id == wfConf.Id) && s.Name == selectConf.Name).ToAsyncEnumerable().ToList();
                if ((duplicata != null) && (duplicata.Count() > 0))
                    throw new WrongParameterException("SelectorConfig's name exist already for this WorkflowConfig!");

                // Ajout
                SelectorConfig addedSelectConf = await UnitOfWork.SelectorConfigRepository.Insert(selectConf);
                transaction.Commit();
                return addedSelectConf;
            }
        }

        /// <summary>
        /// Ajoute un SelectorConfig en Previous Propagate (PrevPropagate) d'un autre SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="previousSelectorConf">SelectorConfig à ajouter en Previous Propagate</param>
        /// <returns>Message de resultat</returns>
        /// <remarks>Il faut que les id donnés en paramètre existent en base de donnée. Si le SelectorConfig cible
        /// posséde déjà le SelectorConfig en PrevPropagate, il ne se passe rien.</remarks>
        public async Task<HttpResponseMessageResult> AddPreviousPropagate(long idSelectorConfig, SelectorConfig previousSelectorConf)
        {
            // Création de la transaction
            using (IDbContextTransaction transaction = UnitOfWork.GetDbContext().Database.BeginTransaction())
            {
                SelectorConfig selectConfCible = await UnitOfWork.SelectorConfigRepository.GetById(idSelectorConfig);

                // Controles
                if ((selectConfCible == null) || (previousSelectorConf == null))
                    throw new WrongParameterException("Id SelectorConfig don't exist!");

                // Création en base du previous propagate SelectoConfig 
                previousSelectorConf = await UnitOfWork.SelectorConfigRepository.Insert(previousSelectorConf);
                // On place (remplacement) le previous SelectorConfig
                selectConfCible.PrevPropagateId = previousSelectorConf.Id;
                int nbrSauve = await UnitOfWork.GetDbContext().SaveChangesAsync();

                transaction.Commit();
                return new HttpResponseMessageResult() { IsSuccess = nbrSauve >= 1 };
            }
        }

        /// <summary>
        /// Ajoute un SelectorConfig en Propagate d'un autre SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="nextSelectorConf">SelectorConfig à ajouter en Propagate</param>
        /// <returns>Message de resultat</returns>
        /// <remarks>Il faut que les id donnés en paramètre existent en base de donnée. Si le SelectorConfig cible
        /// posséde déjà le SelectorConfig en Propagate, il ne se passe rien.</remarks>
        public async Task<HttpResponseMessageResult> AddPropagate(long idSelectorConfig, SelectorConfig nextSelectorConf)
        {
            // Création de la transaction
            using (IDbContextTransaction transaction = UnitOfWork.GetDbContext().Database.BeginTransaction())
            {
                SelectorConfig selectConfCible = await UnitOfWork.SelectorConfigRepository.GetById(idSelectorConfig);

                // Controles
                if ((selectConfCible == null) || (nextSelectorConf == null))
                    throw new WrongParameterException("Id SelectorConfig don't exist!");

                nextSelectorConf = await UnitOfWork.SelectorConfigRepository.Insert(nextSelectorConf);
                // On place (remplacement) le previous SelectorConfig
                selectConfCible.PropagateId = nextSelectorConf.Id;
                int nbrSauve = await UnitOfWork.GetDbContext().SaveChangesAsync();

                transaction.Commit();
                return new HttpResponseMessageResult() { IsSuccess = nbrSauve >= 1 };
            }
        }


        /// <summary>
        /// Ajoute un Criteria à un SelectorConfig pour cibler les données à modifier
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria définissant les valeurs à modifier</param>
        /// <returns>Message de résultat</returns>
        /// <remarks>Ajoute juste l'objet Criteria au SelectorConfig. Vérifie l'existance du SelectorConfig mais ne controle pas si le résultat 
        /// du Criteria sur les potentiels subset du SelectorConfig contient bel et bien des données.</remarks>
        public async Task<HttpResponseMessageResult> AddCriteriaToModifyValue(long idSelectorConfig, Criteria criteria)
        {
            // Création de la transaction
            using (IDbContextTransaction transaction = UnitOfWork.GetDbContext().Database.BeginTransaction())
            {

                SelectorConfig selectConfCible = await UnitOfWork.SelectorConfigRepository.GetById(idSelectorConfig);
                if (selectConfCible == null)
                    throw new WrongParameterException("Id SelectorConfig don't exist!");
                criteria.SelectorConfigModifyData = selectConfCible;

                // Ordre du conditionnned criteria :
                if ((criteria.Order == 0) && (criteria.Id == 0))
                {
                    List<int> lst = await UnitOfWork.GetDbContext().Criteria
                    .Include(d => d.SelectorConfigModifyData)
                    .Where(c => (c.SelectorConfigModifyData != null) && (c.SelectorConfigModifyData.Id == idSelectorConfig))
                    .Select(c => c.Order)
                    .ToAsyncEnumerable()
                    .ToList();
                    int newOrder = ((lst != null) && (lst.Count > 0)) ? lst.Max() : 0;
                    criteria.Order = newOrder + 1;
                }

                Criteria addedCriteria = null;
                if (criteria.Id == 0)
                    // On ajoute le Criteria
                    addedCriteria = await UnitOfWork.CriteriaRepository.Insert(criteria);
                else
                    // Mise à jour
                    addedCriteria = await UnitOfWork.CriteriaRepository.Update(criteria);

                if (addedCriteria == null)
                    throw new ManageDataException("Criteria can't insert!");

                List<IGrouping<int, Criteria>> res = await UnitOfWork.GetDbContext().Criteria
                    .Where(c => c.SelectorConfigModifyData != null && c.SelectorConfigModifyData.Id == idSelectorConfig)
                    .GroupBy(c => c.Order)
                    .Where(g => g.Count() > 1)
                    .ToListAsync();

                if (res.Count > 0)
                {
                    // Mise à jour de l'ordre des criteria existants
                    List<Criteria> lstCUdate = await UnitOfWork.GetDbContext().Criteria
                        .Include(d => d.SelectorConfig)
                        .Where(c => (c.SelectorConfigModifyData != null) && (c.SelectorConfigModifyData.Id == idSelectorConfig) && (c.Order >= criteria.Order) && (c.Id != criteria.Id))
                        .ToAsyncEnumerable()
                        .ToList();
                    if (lstCUdate.Count > 0)
                    {
                        foreach (Criteria c in lstCUdate)
                            c.Order += 1;
                        int nbrSauveTmp = await UnitOfWork.GetDbContext().SaveChangesAsync();
                        if (lstCUdate.Count != nbrSauveTmp)
                            throw new ManageDataException($"Only {nbrSauveTmp} Criteria from SelectorConfig {idSelectorConfig} are saved (for total of {lstCUdate.Count}).");
                    }
                }

                transaction.Commit();
                return new HttpResponseMessageResult() { IsSuccess = true };
            }
        }

        /// <summary>
        /// Ajoute un criteria dans la liste ordonnée des criterias de modificateurs d'un SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria destiné à la liste des modificateurs</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> AddModifiersCriteria(long idSelectorConfig, Criteria criteria)
        {
            // Création de la transaction
            using (IDbContextTransaction transaction = UnitOfWork.GetDbContext().Database.BeginTransaction())
            {
                SelectorConfig selectConfCible = await UnitOfWork.SelectorConfigRepository.GetById(idSelectorConfig);
                criteria.SelectorConfigModifiers = selectConfCible ?? throw new WrongParameterException("Id SelectorConfig don't exist!");

                // ordre du criteria
                if ((criteria.Order == 0) && (criteria.Id == 0))
                {
                    List<int> lst = await UnitOfWork.GetDbContext()
                    .Criteria
                    .Include(d => d.SelectorConfigModifiers)
                    .Where(c => (c.SelectorConfigModifiers != null) && (c.SelectorConfigModifiers.Id == idSelectorConfig))
                    .Select(c => c.Order)
                    .ToAsyncEnumerable()
                    .ToList();
                    int newOrder = ((lst != null) && (lst.Count > 0)) ? lst.Max() : 0;
                    criteria.Order = newOrder + 1;
                }

                Criteria addedCriteria = null;
                if (criteria.Id == 0)
                    addedCriteria = await UnitOfWork.CriteriaRepository.Insert(criteria);
                else
                    // Mise à jour
                    addedCriteria = await UnitOfWork.CriteriaRepository.Update(criteria);

                if (addedCriteria == null)
                    throw new ManageDataException("ConditionnedCriteria can't insert!");

                List<IGrouping<int, Criteria>> res = await UnitOfWork.GetDbContext()
                    .Criteria
                    .Where(c => c.SelectorConfigModifiers != null && c.SelectorConfigModifiers.Id == idSelectorConfig)
                    .GroupBy(c => c.Order)
                    .Where(g => g.Count() > 1)
                    .ToListAsync();

                if (res.Count > 0)
                {
                    // Mise à jour de l'ordre des criteria existants
                    List<Criteria> lstCUpdate = await UnitOfWork.GetDbContext()
                        .Criteria
                        .Include(d => d.SelectorConfigModifiers)
                        .Where(c => (c.SelectorConfigModifiers != null) && (c.SelectorConfigModifiers.Id == idSelectorConfig) && (c.Order >= criteria.Order) && (c.Id != criteria.Id))
                        .ToAsyncEnumerable()
                        .ToList();
                    if (lstCUpdate.Count > 0)
                    {
                        foreach (Criteria c in lstCUpdate)
                            c.Order += 1;
                        int nbrSauveTmp = await UnitOfWork.GetDbContext().SaveChangesAsync();
                        if (lstCUpdate.Count != nbrSauveTmp)
                            throw new ManageDataException($"Only {nbrSauveTmp} Criteria from SelectorConfig {idSelectorConfig} are saved (for total of {lstCUpdate.Count}).");
                    }
                }

                transaction.Commit();
                return new HttpResponseMessageResult() { IsSuccess = true };
            }
        }

        /// <summary>
        /// Ajoute un criteria dans la liste ordonnée des criterias de validateurs d'un SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria destiné à la liste des validateurs</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> AddValidatorsCriteria(long idSelectorConfig, Criteria criteria)
        {
            // Création de la transaction
            using (IDbContextTransaction transaction = UnitOfWork.GetDbContext().Database.BeginTransaction())
            {
                SelectorConfig selectConfCible = await UnitOfWork.SelectorConfigRepository.GetById(idSelectorConfig);
                criteria.SelectorConfigValidators = selectConfCible ?? throw new WrongParameterException("Id SelectorConfig don't exist!");

                // ordre du criteria
                if ((criteria.Order == 0) && (criteria.Id == 0))
                {
                    List<int> lst = await UnitOfWork.GetDbContext()
                    .Criteria
                    .Include(d => d.SelectorConfigValidators)
                    .Where(c => (c.SelectorConfigValidators != null) && (c.SelectorConfigValidators.Id == idSelectorConfig))
                    .Select(c => c.Order)
                    .ToAsyncEnumerable()
                    .ToList();
                    int newOrder = ((lst != null) && (lst.Count > 0)) ? lst.Max() : 0;
                    criteria.Order = newOrder + 1;
                }

                Criteria addedCriteria = null;
                if (criteria.Id == 0)
                    addedCriteria = await UnitOfWork.CriteriaRepository.Insert(criteria);
                else
                    // Mise à jour
                    addedCriteria = await UnitOfWork.CriteriaRepository.Update(criteria);

                if (addedCriteria == null)
                    throw new ManageDataException("ConditionnedCriteria can't insert!");

                List<IGrouping<int, Criteria>> res = await UnitOfWork.GetDbContext()
                    .Criteria
                    .Where(c => c.SelectorConfigValidators != null && c.SelectorConfigValidators.Id == idSelectorConfig)
                    .GroupBy(c => c.Order)
                    .Where(g => g.Count() > 1)
                    .ToListAsync();

                if (res.Count > 0)
                {
                    // Mise à jour de l'ordre des criteria existants
                    List<Criteria> lstCUpdate = await UnitOfWork.GetDbContext()
                        .Criteria
                        .Include(d => d.SelectorConfigValidators)
                        .Where(c => (c.SelectorConfigValidators != null) && (c.SelectorConfigValidators.Id == idSelectorConfig) && (c.Order >= criteria.Order) && (c.Id != criteria.Id))
                        .ToAsyncEnumerable()
                        .ToList();
                    if (lstCUpdate.Count > 0)
                    {
                        foreach (Criteria c in lstCUpdate)
                            c.Order += 1;
                        int nbrSauveTmp = await UnitOfWork.GetDbContext().SaveChangesAsync();
                        if (lstCUpdate.Count != nbrSauveTmp)
                            throw new ManageDataException($"Only {nbrSauveTmp} Criteria from SelectorConfig {idSelectorConfig} are saved (for total of {lstCUpdate.Count}).");
                    }
                }

                transaction.Commit();
                return new HttpResponseMessageResult() { IsSuccess = true };
            }
        }


        /// <summary>
        /// Duplicate un SelectorConfig pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="selectorConfig">SelectorConfig original</param>
        /// <returns>Duplicat du SelectorConfig original</returns>
        public async Task<SelectorConfig> CopyForStatic(SelectorConfig selectorConfig)
        {
            if (selectorConfig == null)
                throw new WrongParameterException("SelectorConfigDomain.CopyForStatic: SelectorConfig source is null!");

            int lastNumber = 1;
            string name = selectorConfig.Name;
            Match m = _regexSubStringName.Match(selectorConfig.Name);
            if (m.Success)
            {
                if (Int32.TryParse(m.Groups["number"].Value, out int number))
                    lastNumber = number + 1;
                name = m.Groups["name"].Value;
            }

            SelectorConfig duplicat = new SelectorConfig();
            UnitOfWork.SelectorConfigRepository.PrepareAddForObject(duplicat);

            duplicat.Name = $"{name} ({lastNumber})";
            duplicat.ActionSequenceRef = selectorConfig.ActionSequenceRef;
            duplicat.ConstraintSequenceRef = selectorConfig.ConstraintSequenceRef;
            duplicat.Description = selectorConfig.Description;
            // Copies des criteres
            foreach (Criteria c in selectorConfig.Criterias)
                duplicat.Criterias.Add(await CriteriaDomain.CopyForStatic(c));
            foreach (Criteria c in selectorConfig.Modifiers)
                duplicat.Modifiers.Add(await CriteriaDomain.CopyForStatic(c));
            foreach (Criteria c in selectorConfig.Validators)
                duplicat.Validators.Add(await CriteriaDomain.CopyForStatic(c));
            foreach (Criteria c in selectorConfig.ModifyCriterias)
                duplicat.ModifyCriterias.Add(await CriteriaDomain.CopyForStatic(c));

            // Copies des propagates...
            List<SelectorConfig> lstPrev = await UnitOfWork.GetDbContext().SelectorConfig
                .Where(sc => sc.Id == selectorConfig.PrevPropagateId && sc.WorkflowConfig != null)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();
            SelectorConfig prev = lstPrev.FirstOrDefault();
            if (prev != null)
            {
                SelectorConfig cpyPrev = await CopyForStatic(prev);
                duplicat.PrevPropagateId = cpyPrev.Id;
            }

            List<SelectorConfig> lstProp = await UnitOfWork.GetDbContext().SelectorConfig
                .Where(sc => sc.Id == selectorConfig.PropagateId && sc.WorkflowConfig != null)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();
            SelectorConfig prop = lstProp.FirstOrDefault();
            if (prop != null)
            {
                SelectorConfig cpyProp = await CopyForStatic(prop);
                duplicat.PropagateId = cpyProp.Id;
            }

            List<SelectorConfig> lstFail = await UnitOfWork.GetDbContext().SelectorConfig
                .Where(sc => sc.Id == selectorConfig.FailPropagateId && sc.WorkflowConfig != null)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();
            SelectorConfig fail = lstFail.FirstOrDefault();
            if (fail != null)
            {
                SelectorConfig cpyFail = await CopyForStatic(fail);
                duplicat.PropagateId = cpyFail.Id;
            }

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            return duplicat;
        }


        /// <summary>
        /// Récupere la liste des SelectorConfig et de leurs SelectorInstance lancés d'un WorkflowInstance.
        /// </summary>
        /// <param name="workflowInstanceId">Id du workflowInstance</param>
        /// <returns>Liste de SelectorConfig (et donc de leur SelectorInstance)</returns>
        public async Task<IEnumerable<SelectorConfig>> GetSelectors(long workflowInstanceId)
        {
            WorkflowInstance wfIns = await UnitOfWork.GetDbContext().WorkflowInstance
                .Include(wf => wf.WorkflowConfig)
                .Where(wf => wf.Id == workflowInstanceId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (wfIns == null)
                throw new WrongParameterException($"SelectorConfigDomain.GetSelectors: WorkflowInstance with id = {workflowInstanceId} don't exist.");

            List<SelectorConfig> lstSC = await UnitOfWork.GetDbContext().SelectorConfig
                .Include(sc => sc.SelectorInstance)
                .Include(sc => sc.WorkflowConfig)
                .Where(sc => sc.WorkflowConfig.Id == wfIns.WorkflowConfig.Id)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            return lstSC;
        }


    }
}
