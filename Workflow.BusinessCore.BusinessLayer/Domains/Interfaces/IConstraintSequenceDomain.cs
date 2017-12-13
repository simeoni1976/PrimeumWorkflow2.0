using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    public interface IConstraintSequenceDomain
    {
        /// <summary>
        /// Permet de vérifier toutes les contraintes dans l'ordre d'une séquence de contraintes sur un SelectorInstance.
        /// </summary>
        /// <param name="sequences">Sequences de contraintes</param>
        /// <param name="selectorInstance">SelectorInstance cible</param>
        /// <param name="workflowInstance">WorkflowInstance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> CheckConstraintSequence(IEnumerable<ConstraintSequence> sequences, SelectorInstance selectorInstance, WorkflowInstance workflowInstance, IEnumerable<KeyValuePair<long, double>> values);

        /// <summary>
        /// Vérifie une contrainte sur un SelectorInstance.
        /// </summary>
        /// <param name="contrainte">Contrainte</param>
        /// <param name="referenceSequence">Reference de la sequence de contrainte</param>
        /// <param name="ordreSequence">Ordre de la contrainte dans la sequence</param>
        /// <param name="level">Niveau d'alerte si la vérification échoue</param>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="workflowInstance">WorkflowInstance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> CheckConstraint(Constraint contrainte, long referenceSequence, int ordreSequence, ConstraintLevelEnum level, SelectorInstance selectorInstance, WorkflowInstance workflowInstance, IEnumerable<KeyValuePair<long, double>> values);


        /// <summary>
        /// Ajoute une nouvelle contrainte (indépente des Wrokflow)
        /// </summary>
        /// <param name="constraint">Nouvelle contrainte</param>
        /// <returns>Message de retour</returns>
        Task<Constraint> AddConstraint(Constraint constraint);

        /// <summary>
        /// Ajoute une nouvelle séquence de contrainte.
        /// </summary>
        /// <param name="constraintSequence">Nouvelle SequenceConstraint</param>
        /// <param name="constraintId">Id de la contrainte</param>
        /// <returns>Message de retour</returns>
        Task<ConstraintSequence> AddConstraintSequence(ConstraintSequence constraintSequence, long constraintId);

        /// <summary>
        /// Ajoute un nouveau ConstraintParameter
        /// </summary>
        /// <param name="constraintParameter">Nouveau ConstraintParameter</param>
        /// <param name="constraintSequenceId">Id de la ConstraintSequence</param>
        /// <returns>Message de retour</returns>
        Task<ConstraintParameter> AddConstraintParameter(ConstraintParameter constraintParameter, long constraintSequenceId);

        /// <summary>
        /// Récupére toutes les Constraint
        /// </summary>
        /// <returns>Liste de Constraint</returns>
        Task<IEnumerable<Constraint>> GetAllConstraint();

        /// <summary>
        /// Recupére une Constraint par son Id.
        /// </summary>
        /// <param name="constraintId">Id de la Constraint</param>
        /// <returns>Constraint recherchée</returns>
        Task<Constraint> GetConstraint(long constraintId);

        /// <summary>
        /// Récupére toutes les ConstraintSequence
        /// </summary>
        /// <returns>Liste de ConstraintSequence</returns>
        Task<IEnumerable<ConstraintSequence>> GetAllConstraintSequence();

        /// <summary>
        /// Récupére une ConstraintSequence par son Id
        /// </summary>
        /// <param name="constraintSequenceId">Id de la ConstraintSequence</param>
        /// <returns>ConstraintSequence recherchée</returns>
        Task<ConstraintSequence> GetConstraintSequence(long constraintSequenceId);

        /// <summary>
        /// Récupére tous les ConstraintParameter
        /// </summary>
        /// <returns>Liste des ConstraintParameter</returns>
        Task<IEnumerable<ConstraintParameter>> GetAllConstraintParameter();

        /// <summary>
        /// Récupére un ConstraintParameter par son Id.
        /// </summary>
        /// <param name="constraintParameterId">Id du ConstraintParameter</param>
        /// <returns>ConstraintParameter recherché</returns>
        Task<ConstraintParameter> GetConstraintParameter(long constraintParameterId);
    }
}
