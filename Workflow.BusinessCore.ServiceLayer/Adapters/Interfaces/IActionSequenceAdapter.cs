using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.Transverse.DTO;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    public interface IActionSequenceAdapter : IBaseAdapter<ActionSequence>
    {
        /// <summary>
        /// Ajoute une nouvelle action dans une séquence (existante ou non)
        /// </summary>
        /// <param name="actionSequence">Nouvelle ActionSequence</param>
        /// <param name="actionId">Id de l'Action à exécuter</param>
        /// <returns>Message de retour</returns>
        Task<ActionSequence> AddActionSequence(ActionSequence actionSequence, long actionId);
    }
}
