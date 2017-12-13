using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.DataLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Workflow.Transverse.Helpers;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// Criteria domain class
    /// </summary>
    public class CriteriaDomain : AbstractDomain<Criteria>, ICriteriaDomain
    {
        private readonly IServiceProvider _serviceProvider;

        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }

        private ISelectorConfigDomain SelectorConfigDomain
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorConfigDomain>();
            }
        }

        private ICriteriaValuesDomain CriteriaValuesDomain
        {
            get
            {
                return _serviceProvider?.GetService<ICriteriaValuesDomain>();
            }
        }


        private readonly string _ruleAutorizedValues = @"^(\*|\{([^,\s]+,?\s?)+\}|[\d\w\._-]+)$";
        private readonly string _ruleCharAllValues = @"^\*$";
        private readonly string _ruleBracketsChar = @"^\{(?<level>([^,\s]+,?\s?)+)\}$";
        private Regex _regexCheckValues;
        private Regex _regexCharAllValues;
        private Regex _regexBracketsChar;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        public CriteriaDomain(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork.CriteriaRepository)
        {
            _serviceProvider = serviceProvider;
            _regexCheckValues = new Regex(_ruleAutorizedValues);
            _regexCharAllValues = new Regex(_ruleCharAllValues);
            _regexBracketsChar = new Regex(_ruleBracketsChar);
        }

        /// <summary>
        /// Ajoute un criteria dans un SelectorConfig.
        /// </summary>
        /// <param name="criteria">Criteria à ajouter</param>
        /// <returns>Objet Criteria ajouter (id à jour).</returns>
        /// <remarks>L'objet Criteria doit contenir l'id du SelectorConfig dans lequel il faut l'ajouter. L'API retourne une erreur
        /// lorsque la dimension ou la valeur du Criteria n'est pas définie. 
        /// Les valeurs possibles d'un Criteria sont '*', chaine-de-caractères, '{valeur1, valeur2, ..., valeurn}' </remarks>
        public override async Task<Criteria> Add(Criteria criteria)
        {
            // Vérifications 
            if (criteria == null)
                throw new WrongParameterException("SelectorConfig object is null!");
            if (criteria.SelectorConfig == null)
                throw new DataLoadingException("Criteria.SelectorConfig don't load!");
            SelectorConfig selectConf = await SelectorConfigDomain.Get(criteria.SelectorConfig.Id);
            if (selectConf == null)
                throw new WrongParameterException("SelectorConfig id doesn't exist!");

            // Controle des valeurs
            if (!_regexCheckValues.IsMatch(criteria.Value))
                throw new WrongParameterException("SelectorConfig value don't check rules!");

            Criteria addedCriteria = null;
            if (criteria.Id == 0)
                // Ajout
                addedCriteria = await UnitOfWork.CriteriaRepository.Insert(criteria);
            else
                // Mise à jour
                addedCriteria = await UnitOfWork.CriteriaRepository.Update(criteria);

            if (addedCriteria == null)
                throw new ManageDataException($"Criteria for SelectorConfig {criteria.SelectorConfig.Id} can't insert!");

            return addedCriteria;
        }

        /// <summary>
        /// Envoi la liste des CriteriaValues existants pour ce Criteria.
        /// </summary>
        /// <param name="criteria">Criteria</param>
        /// <param name="wfInstance">Instance du WorkflowInstance</param>
        /// <returns>Liste des CriteriaValues</returns>
        public async Task<IEnumerable<CriteriaValues>> ExtractCriteriaValues(Criteria criteria, WorkflowInstance wfInstance)
        {
            if ((criteria == null) || (wfInstance == null))
                throw new WrongParameterException("ExtractCriteriaValues: criteria or wfInstance are null.");

            List<CriteriaValues> lst = new List<CriteriaValues>();

            if (_regexCharAllValues.IsMatch(criteria.Value))
            {
                List<string> distinctValue = await UnitOfWork.GetDbContext().DistinctValue
                    .Where(dv => dv.DataSetId == wfInstance.DataSetId && dv.DimensionId == criteria.Dimension.Id)
                    .Select(dv => dv.Value)
                    .AsNoTracking()
                    .ToAsyncEnumerable()
                    .ToList();

                foreach (string valeur in distinctValue)
                {
                    CriteriaValues cv = new CriteriaValues() { Criteria = criteria, Value = valeur };
                    lst.Add(cv);
                }
                return lst;
            }
            if (_regexBracketsChar.IsMatch(criteria.Value))
                if (criteria.Dimension.TypeDimension == DimensionTypeEnum.Tree)
                {
                    Match mb =_regexBracketsChar.Match(criteria.Value);
                    string niveau = mb.Groups["level"].Value;

                    List<string> nodes = await UnitOfWork.GetDbContext().DimensionTreeData //Dimension
                        .Include(dt => dt.Dimensions)
                        .Where(dt => dt.Dimensions.Any(ds => ds.Id == criteria.Dimension.Id))
                        .Where(dt => dt.LevelName == niveau)
                        .Select(dt => dt.ValueKey)
                        .AsNoTracking()
                        .ToAsyncEnumerable()
                        .ToList();

                    foreach (string valeur in nodes)
                    {
                        CriteriaValues cv = new CriteriaValues() { Criteria = criteria, Value = valeur };
                        lst.Add(cv);
                    }
                    return lst;
                }

            // Par défaut, on considére qu'il s'agit d'une valeur...
            List<string> existingValue = await UnitOfWork.GetDbContext().DistinctValue
                .Where(dv => dv.DataSetId == wfInstance.DataSetId && dv.DimensionId == criteria.Dimension.Id && dv.Value == criteria.Value)
                .Select(dv => dv.Value)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            // Controle de la valeur
            if ((existingValue == null) || (existingValue.Count == 0))
                throw new SequenceException($"Criteria value {criteria.Value} don't exist!");

            CriteriaValues cvSingle = new CriteriaValues() { Criteria = criteria, Value = criteria.Value };
            lst.Add(cvSingle);

            return lst;
        }

        /// <summary>
        /// Envoi la liste des listes de CriteriaValues existants pour une liste de Criteria.
        /// </summary>
        /// <param name="criterias">Liste de criteria</param>
        /// <param name="wfInstance">Instance du WorkflowInstance</param>
        /// <returns>Liste de liste des CriteriaValues</returns>
        public async Task<IEnumerable<IEnumerable<CriteriaValues>>> ExtractAllCriteriaValues(IEnumerable<Criteria> criterias, WorkflowInstance wfInstance)
        {
            List<IEnumerable<CriteriaValues>> lstCv = new List<IEnumerable<CriteriaValues>>();
            foreach (Criteria c in criterias)
            {
                if (c.Dimension == null)
                    throw new DataLoadingException($"CriteriaDomain.ExtractAllCriteriaValues : Dimension for Criteria ({c.Id}) not loaded!");

                IEnumerable<CriteriaValues> cvs = await ExtractCriteriaValues(c, wfInstance);
                lstCv.Add(cvs);
            }

            return lstCv;
        }


        /// <summary>
        /// Duplique un Criteria pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="criteria">Criteria original</param>
        /// <returns>Duplicat du Criteria original</returns>
        public async Task<Criteria> CopyForStatic(Criteria criteria)
        {
            if (criteria == null)
                throw new WrongParameterException("CriteriaDomain.CopyForStatic: Criteria source is null!");

            Criteria duplicat = new Criteria();
            UnitOfWork.CriteriaRepository.PrepareAddForObject(duplicat);
            List<Dimension> lstDim = await UnitOfWork.GetDbContext().Dimension
                .Where(d => d.Id == criteria.Dimension.Id)
                .ToAsyncEnumerable()
                .ToList();
            Dimension refDim = lstDim.FirstOrDefault();

            duplicat.Dimension = refDim;
            duplicat.Order = criteria.Order;
            duplicat.Value = criteria.Value;
            duplicat.ChainNumber = criteria.ChainNumber;

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            return duplicat;
        }

        /// <summary>
        /// Duplique un ConditionnedCriteria pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="criteria">ConditionnedCriteria original</param>
        /// <returns>Duplicat du ConditionnedCriteria original</returns>
        public async Task<ConditionnedCriteria> CopyForStatic(ConditionnedCriteria condCriteria)
        {
            if (condCriteria == null)
                throw new WrongParameterException("CriteriaDomain.CopyForStatic: ConditionnedCriteria source is null!");

            ConditionnedCriteria duplicat = new ConditionnedCriteria();
            UnitOfWork.ConditionnedCriteriaRepository.PrepareAddForObject(duplicat);
            duplicat.Formula = condCriteria.Formula;
            List<Dimension> lstDim = await UnitOfWork.GetDbContext().Dimension
                .Where(d => d.Id == condCriteria.Dimension.Id)
                .ToAsyncEnumerable()
                .ToList();
            Dimension refDim = lstDim.FirstOrDefault();
            duplicat.Dimension = refDim;
            duplicat.Order = condCriteria.Order;
            duplicat.Value = condCriteria.Value;

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            return duplicat;

        }

    }
}
