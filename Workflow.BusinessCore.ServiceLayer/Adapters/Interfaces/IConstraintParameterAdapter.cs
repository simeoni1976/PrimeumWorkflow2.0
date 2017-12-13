using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.Transverse.Helpers;
using Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    public interface IConstraintParameterAdapter : IBaseAdapter<ConstraintParameter>
    {
        /// <summary>
        /// Ajoute un nouveau ConstraintParameter
        /// </summary>
        /// <param name="constraintParameter">Nouveau ConstraintParameter</param>
        /// <param name="constraintSequenceId">Id de la ConstraintSequence</param>
        /// <returns>Message de retour</returns>
        Task<ConstraintParameter> AddConstraintParameter(ConstraintParameter constraintParameter, long constraintSequenceId);
    }
}
