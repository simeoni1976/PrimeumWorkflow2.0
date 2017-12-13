using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.Transverse.Helpers;
using Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    public interface IConstraintSequenceAdapter : IBaseAdapter<ConstraintSequence>
    {
        /// <summary>
        /// Ajoute une nouvelle séquence de contrainte.
        /// </summary>
        /// <param name="constraintSequence">Nouvelle SequenceConstraint</param>
        /// <param name="constraintId">Id de la contrainte</param>
        /// <returns>Message de retour</returns>
        Task<ConstraintSequence> AddConstraintSequence(ConstraintSequence constraintSequence, long constraintId);
    }
}
