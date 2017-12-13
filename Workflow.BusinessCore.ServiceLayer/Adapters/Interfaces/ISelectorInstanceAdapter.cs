using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    ///  ISelectorInstanceAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the selector instance adapter.
    /// </remarks>
    public interface ISelectorInstanceAdapter : IBaseAdapter<DTO.SelectorInstance>
    {
        /// <summary>
        /// Modifie les données d'un SelectorInstance. Les données sont d'abord sauvées, puis le SelectorInstance passe en Act et en Constraints.
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <remarks>Les valeurs à modifier sont au format suivant : {id de la cellule}:{nouvelle valeur}</remarks>
        /// <returns>Message à modifier</returns>
        Task<HttpResponseMessageResult> SaveData(long selectorInstanceId, IEnumerable<KeyValuePair<long, double>> values);

    }
}
