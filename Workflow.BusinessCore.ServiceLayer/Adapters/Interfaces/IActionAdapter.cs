using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    public interface IActionAdapter : IBaseAdapter<DTO.Action>
    {
        /// <summary>
        /// Ajoute une action en base, indépendamment d'un workflow.
        /// </summary>
        /// <param name="action">Nouvelle action</param>
        /// <returns>Action enregistrée (avec son id)</returns>
        Task<DTO.Action> AddAction(DTO.Action action);
    }
}
