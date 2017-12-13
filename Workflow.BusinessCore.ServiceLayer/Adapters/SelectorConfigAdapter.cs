using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Process.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Selector.BusinessCore.ServiceLayer.Adapters
{
    /// <summary>
    ///  Selector config adapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Selector config adapter.
    /// </remarks>
    public class SelectorConfigAdapter : ISelectorConfigAdapter
    {
        private readonly IServiceProvider _serviceProvider = null;

        private ISelectorConfigDomain SelectorConfigDomain
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorConfigDomain>();
            }
        }

        private ISelectorEngine SelectorEngine
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorEngine>();
            }
        }

        private IMapper Mapper
        {
            get
            {
                return _serviceProvider.GetService<IMapper>();
            }
        }

        private ISelectorInstanceDomain SelectorInstanceDomain
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorInstanceDomain>();
            }
        }



        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public SelectorConfigAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Ajoute un SelectorConfig en base.
        /// </summary>
        /// <param name="selectConf">SelectorConfig à ajouter</param>
        /// <returns>Message du résultat</returns>
        /// <remarks>L'objet SelectorConfig doit connaitre l'id de son WorkflowConfig parent. De plus, il doit avoir un nom unique. 
        /// L'opération sort en erreur si l'une des deux conditions, ou les deux, n'est pas respectée.</remarks>
        public async Task<HttpResponseMessageResult> Add(DTO.SelectorConfig selectConf)
        {
            if (selectConf == null)
                throw new WrongParameterException("SelectorConfig object is null!");

            ENT.SelectorConfig selectConfEntity = Mapper.Map<DTO.SelectorConfig, ENT.SelectorConfig>(selectConf);
            selectConfEntity.WorkflowConfig = new ENT.WorkflowConfig() { Id = selectConf.IdWorkflowConfig };
            ENT.SelectorConfig res = await SelectorConfigDomain.Add(selectConfEntity);
            DTO.SelectorConfig addedSelectConf = Mapper.Map<ENT.SelectorConfig, DTO.SelectorConfig>(res);

            return new HttpResponseMessageResult() { IsSuccess = true, Json = JsonConvert.SerializeObject(addedSelectConf) };
        }

        /// <summary>
        /// Ajoute un SelectorConfig en Previous Propagate (PrevPropagate) d'un autre SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="previousSelectorConf">SelectorConfig à ajouter en Previous Propagate</param>
        /// <returns>Message de resultat</returns>
        /// <remarks>Il faut que les id donnés en paramètre existent en base de donnée. Si le SelectorConfig cible
        /// posséde déjà le SelectorConfig en PrevPropagate, il ne se passe rien.</remarks>
        public async Task<HttpResponseMessageResult> AddPreviousPropagate(long idSelectorConfig, DTO.SelectorConfig previousSelectorConf)
        {
            ENT.SelectorConfig entitySelectConf = Mapper.Map<DTO.SelectorConfig, ENT.SelectorConfig>(previousSelectorConf);
            return await SelectorConfigDomain.AddPreviousPropagate(idSelectorConfig, entitySelectConf);
        }

        /// <summary>
        /// Ajoute un SelectorConfig en Propagate d'un autre SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="nextSelectorConf">SelectorConfig à ajouter en Propagate</param>
        /// <returns>Message de resultat</returns>
        /// <remarks>Il faut que les id donnés en paramètre existent en base de donnée. Si le SelectorConfig cible
        /// posséde déjà le SelectorConfig en Propagate, il ne se passe rien.</remarks>
        public async Task<HttpResponseMessageResult> AddPropagate(long idSelectorConfig, DTO.SelectorConfig nextSelectorConf)
        {
            ENT.SelectorConfig entitySelectConf = Mapper.Map<DTO.SelectorConfig, ENT.SelectorConfig>(nextSelectorConf);
            return await SelectorConfigDomain.AddPropagate(idSelectorConfig, entitySelectConf);
        }


        /// <summary>
        /// Ajoute un Criteria à un SelectorConfig pour cibler les données à modifier
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria définissant les valeurs à modifier</param>
        /// <returns>Message de résultat</returns>
        /// <remarks>Ajoute juste l'objet Criteria au SelectorConfig. Vérifie l'existance du SelectorConfig mais ne controle pas si le résultat 
        /// du Criteria sur les potentiels subset du SelectorConfig contient bel et bien des données.</remarks>
        public async Task<HttpResponseMessageResult> AddCriteriaToModifyValue(long idSelectorConfig, DTO.Criteria criteria)
        {
            ENT.Criteria condCriteriaEntity = Mapper.Map<DTO.Criteria, ENT.Criteria>(criteria);
            return await SelectorConfigDomain.AddCriteriaToModifyValue(idSelectorConfig, condCriteriaEntity);
        }


        /// <summary>
        /// Ajoute un criteria dans la liste ordonnée des criterias de modificateurs d'un SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria destiné à la liste des modificateurs</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> AddModifiersCriteria(long idSelectorConfig, DTO.Criteria criteria)
        {
            ENT.Criteria criteriaEntity = Mapper.Map<DTO.Criteria, ENT.Criteria>(criteria);
            return await SelectorConfigDomain.AddModifiersCriteria(idSelectorConfig, criteriaEntity);
        }

        /// <summary>
        /// Ajoute un criteria dans la liste ordonnée des criterias de validateurs d'un SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria destiné à la liste des validateurs</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> AddValidatorsCriteria(long idSelectorConfig, DTO.Criteria criteria)
        {
            ENT.Criteria criteriaEntity = Mapper.Map<DTO.Criteria, ENT.Criteria>(criteria);
            return await SelectorConfigDomain.AddValidatorsCriteria(idSelectorConfig, criteriaEntity);
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.SelectorConfig>> GetAll()
        {
            IEnumerable<ENT.SelectorConfig> selectorConfig = await SelectorConfigDomain.Get();

            IEnumerable<DTO.SelectorConfig> dtoSelectorConfig = null;
            if (selectorConfig != null)
                dtoSelectorConfig = Mapper.Map<IEnumerable<ENT.SelectorConfig>, IEnumerable<DTO.SelectorConfig>>(selectorConfig);
            else
                dtoSelectorConfig = new List<DTO.SelectorConfig>();

            return dtoSelectorConfig;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.SelectorConfig> GetById(long id)
        {
            ENT.SelectorConfig selectorConfig = await SelectorConfigDomain.Get(id);

            DTO.SelectorConfig dtoSelectorConfig = null;
            if (selectorConfig != null)
                dtoSelectorConfig = Mapper.Map<ENT.SelectorConfig, DTO.SelectorConfig>(selectorConfig);

            return dtoSelectorConfig;
        }

        /// <summary>
        /// Récupere la liste des SelectorConfig et de leurs SelectorInstance lancés d'un WorkflowInstance.
        /// </summary>
        /// <param name="workflowInstanceId">Id du workflowInstance</param>
        /// <returns>Liste de SelectorConfig (et donc de leur SelectorInstance)</returns>
        public async Task<IEnumerable<DTO.SelectorConfig>> GetSelectors(long workflowInstanceId)
        {
            IEnumerable<ENT.SelectorConfig> lstEntSC = await SelectorConfigDomain.GetSelectors(workflowInstanceId);

            List<DTO.SelectorConfig> lstDTOSc = new List<DTO.SelectorConfig>();
            foreach (ENT.SelectorConfig entSC in lstEntSC)
            {
                DTO.SelectorConfig dtoSC = Mapper.Map<ENT.SelectorConfig, DTO.SelectorConfig>(entSC);

                dtoSC.SelectorInstance = new List<DTO.SelectorInstance>();
                foreach (ENT.SelectorInstance entSI in entSC.SelectorInstance)
                {
                    Tuple<string, string, string> tInfos = await SelectorInstanceDomain.GetAdditionnalInfo(entSI);
                    DTO.SelectorInstance dtoSI = Mapper.Map<ENT.SelectorInstance, DTO.SelectorInstance>(entSI);
                    dtoSI.ModificatorName = tInfos.Item1;
                    dtoSI.ValidatorsNames = tInfos.Item2;
                    dtoSI.DimensionValueImportant = tInfos.Item3;
                    dtoSC.SelectorInstance.Add(dtoSI);
                }
                lstDTOSc.Add(dtoSC);
            }

            return lstDTOSc;
        }

    }
}
