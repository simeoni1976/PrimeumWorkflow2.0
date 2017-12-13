using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    public interface IConfigVariableDomain
    {
        /// <summary>
        /// Charge les variables de conf de la base
        /// </summary>
        Task LoadVariables();


        /// <summary>
        /// Propriété concernant le format des nombres du Workflow.
        /// </summary>
        string Format { get; }

        /// <summary>
        /// Propriété concernant le caractère séparateur dans la représentation des dimensions de type arbre.
        /// </summary>
        char AlignmentChar { get; }
    }
}
