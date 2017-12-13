using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// CriteriaValuesDomain domain class
    /// </summary>
    public class CriteriaValuesDomain : AbstractDomain<CriteriaValues>, ICriteriaValuesDomain
    {
        private readonly IServiceProvider _serviceProvider;

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

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        public CriteriaValuesDomain(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork.CriteriaValuesRepository)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Avec une liste de liste de CriteriaValues, fournie un dictionnaire de liste de CriteriaValues classées par Id de dimension.
        /// </summary>
        /// <param name="criteriaValues">Liste de liste de CriteriaValues</param>
        /// <returns>Dictionnaire de liste de CriteriaValues, avec comme clé l'id de dimension</returns>
        public Dictionary<long, IEnumerable<CriteriaValues>> GetCriteriaValuesByDimension(IEnumerable<IEnumerable<CriteriaValues>> criteriaValues)
        {
            Dictionary<long, IEnumerable<CriteriaValues>> dico = new Dictionary<long, IEnumerable<CriteriaValues>>();

            foreach (IEnumerable<CriteriaValues> lstcv in criteriaValues)
                foreach (CriteriaValues cv in lstcv)
                {
                    if (!dico.ContainsKey(cv.Criteria.Dimension.Id))
                        dico.Add(cv.Criteria.Dimension.Id, new List<CriteriaValues>());
                    List<CriteriaValues> lst = dico[cv.Criteria.Dimension.Id] as List<CriteriaValues>;
                    lst?.Add(cv);
                }

            return dico;
        }
    }
}
