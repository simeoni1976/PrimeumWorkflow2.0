using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// ICriteriaValues interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the CriteriaValues business.
    /// </remarks>
    public interface ICriteriaValuesDomain
    {
        /// <summary>
        /// This function permits to get all the CriteriaValues
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<CriteriaValues>> Get();

        /// <summary>
        /// This function permits to get a CriteriaValues
        /// </summary>
        /// <param name="id">CriteriaValues ID</param>
        /// <returns>CriteriaValues</returns>
        Task<CriteriaValues> Get(long id);

        /// <summary>
        /// This function permits to add a new CriteriaValues
        /// </summary>
        /// <param name="CriteriaValues">CriteriaValues</param>
        /// <returns>CriteriaValues</returns>
        Task<CriteriaValues> Add(CriteriaValues CriteriaValues);

        /// <summary>
        /// This function permits to update CriteriaValues
        /// </summary>
        /// <param name="CriteriaValues">CriteriaValues</param>
        /// <returns>CriteriaValues</returns>
        Task<CriteriaValues> Update(CriteriaValues CriteriaValues);

        /// <summary>
        /// This function permits to delete CriteriaValues
        /// </summary>
        /// <param name="CriteriaValues">CriteriaValues</param>
        /// <returns>Task</returns>
        Task Delete(CriteriaValues CriteriaValues);

        /// <summary>
        /// This function permits to delete CriteriaValues
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);

        /// <summary>
        /// Avec une liste de liste de CriteriaValues, fournie un dictionnaire de liste de CriteriaValues classées par Id de dimension.
        /// </summary>
        /// <param name="criteriaValues">Liste de liste de CriteriaValues</param>
        /// <returns>Dictionnaire de liste de CriteriaValues, avec comme clé l'id de dimension</returns>
        Dictionary<long, IEnumerable<CriteriaValues>> GetCriteriaValuesByDimension(IEnumerable<IEnumerable<CriteriaValues>> criteriaValues);
    }
}
