using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    ///  ISelectorInstance instance interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Selector instance business.
    /// </remarks>
    public interface ISelectorInstanceDomain
    {

        /// <summary>
        /// Permet d'instancier un nouvel SelectorInstance.
        /// </summary>
        /// <param name="selectConf">SelectorConfig à l'origine de l'instance</param>
        /// <param name="lstCriteriaValue">Liste des criteriaValue qui définissent la selection de données</param>
        /// <param name="parent">Eventuel SelectorInstance pouvant être à l'origine de la création</param>
        /// <returns>Instance du nouvel SelectorInstance</returns>
        Task<SelectorInstance> Create(SelectorConfig selectConf, IEnumerable<CriteriaValues> lstCriteriaValue, SelectorInstance parent, WorkflowInstance wfInstance);

        /// <summary>
        /// Recherche du modificateur pour le SelectorInstance donné en paramétre.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">WorkflowInstance</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> FindModificator(SelectorInstance selectorInstance, WorkflowInstance wfInstance);


        /// <summary>
        /// Sélectionne les données modifiables du SelectorInstance.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">WorkflowInstance</param>
        /// <param name="idsDimensionDS">Dictionnaire de DimensionDataSet par Id</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> SetModifyData(SelectorInstance selectorInstance, WorkflowInstance wfInstance, Dictionary<long, DataSetDimension> idsDimensionDS);

        /// <summary>
        /// This function permits to get all the Selector instances
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<SelectorInstance>> Get();

        /// <summary>
        /// This function permits to get all the Selector instances by an id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>SelectorInstance<returns>
        Task<SelectorInstance> Get(long id);

        /// <summary> 
        /// This function permits to add a new Selector instance
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <returns>SelectorInstance</returns>
        Task<SelectorInstance> Add(SelectorInstance selectorInstance);

        /// <summary>
        /// This function permits to update Selector instance
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <returns>SelectorInstance</returns>
        Task<SelectorInstance> Update(SelectorInstance selectorInstance);

        /// <summary>
        /// This function permits to delete Selector instance
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <returns>Task</returns>
        Task Delete(SelectorInstance selectorInstance);

        /// <summary>
        /// This function permits to delete Selector instance
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);

        /// <summary>
        /// Peremt de pousser les valeurs volatiles de l'ensemble de ValueObject d'un SelectorInstance vers leurs valeurs futures respectivement.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <returns>Retour ok si tout se passe bien.</returns>
        Task<HttpResponseMessageResult> PushVolatileToFuture(SelectorInstance selectorInstance);

        /// <summary>
        /// Récupére les informations supplémentaires pour un SelectorInstance.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <returns>Tuple de 3 info : le nom du modificateur, les noms des validateurs, la valeur de la 1ere dimension arbo.</returns>
        Task<Tuple<string, string, string>> GetAdditionnalInfo(SelectorInstance selectorInstance);


        /// <summary>
        /// Trouve les validateurs et les ajoute dans la liste des utilisateurs en validation.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">WorkflowInstance</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> FindValidators(SelectorInstance selectorInstance, WorkflowInstance wfInstance);

    }
}
