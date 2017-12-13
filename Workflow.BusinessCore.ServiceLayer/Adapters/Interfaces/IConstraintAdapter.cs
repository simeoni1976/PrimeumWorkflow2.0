using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.Transverse.Helpers;
using Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    public interface IConstraintAdapter : IBaseAdapter<Constraint>
    {
        /// <summary>
        /// Ajoute une nouvelle contrainte (indépente des Wrokflow)
        /// </summary>
        /// <param name="constraint">Nouvelle contrainte</param>
        /// <returns>Message de retour</returns>
        Task<Constraint> AddConstraint(Constraint constraint);
    }
}
