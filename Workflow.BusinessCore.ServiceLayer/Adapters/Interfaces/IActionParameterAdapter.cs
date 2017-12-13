using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.Transverse.DTO;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    public interface IActionParameterAdapter : IBaseAdapter<ActionParameter>
    {
        /// <summary>
        /// Ajoute un paramètre dans l'action d'une séquence d'action.
        /// </summary>
        /// <param name="actionParameter">ActionParameter à ajouter</param>
        /// <param name="actionSequenceId">Id de la séquence d'action cible</param>
        /// <returns>ActionParameter enregistré, avec un nouvel id.</returns>
        Task<ActionParameter> AddActionParameter(ActionParameter actionParameter, long actionSequenceId);
    }
}
