using System.Threading.Tasks;
using Workflow.Transverse.Helpers;
using Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    /// Interface pour l'adaptateur des Criteria.
    /// </summary>
    public interface ICriteriaAdapter : IBaseAdapter<Criteria>
    {
        /// <summary>
        /// Ajoute un criteria dans un SelectorConfig.
        /// </summary>
        /// <param name="criteria">Criteria à ajouter</param>
        /// <returns>Message de retour.</returns>
        /// <remarks>L'objet Criteria doit contenir l'id du SelectorConfig dans lequel il faut l'ajouter. L'API retourne une erreur
        /// lorsque la dimension ou la valeur du Criteria n'est pas définie. 
        /// Les valeurs possibles d'un Criteria sont '*', chaine-de-caractères, '{valeur1, valeur2, ..., valeurn}' </remarks>
        Task<Criteria> Add(Criteria criteria);
    }
}