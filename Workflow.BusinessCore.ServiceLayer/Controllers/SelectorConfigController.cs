using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Filters;
using Workflow.Transverse.DTO;
using Workflow.Transverse.Environment;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{

    /// <summary>
    /// Controller SelectorConfigController, qui permet de configurer les SelectorConfig.
    /// </summary>
    public class SelectorConfigController : BaseController<SelectorConfig, ISelectorConfigAdapter>
    {
        protected override ISelectorConfigAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorConfigAdapter>();
            }
        }

        private ICriteriaAdapter CriteriaAdapter
        {
            get
            {
                return _serviceProvider?.GetService<ICriteriaAdapter>();
            }
        }

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="selectorConfigAdapter">Adaptateur de SelectorConfig</param>
        public SelectorConfigController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <summary>
        /// Ajoute un SelectorConfig en base.
        /// </summary>
        /// <param name="selectConf">SelectorConfig à ajouter</param>
        /// <returns>Message du résultat</returns>
        /// <remarks>L'objet SelectorConfig doit connaitre l'id de son WorkflowConfig parent. De plus, il doit avoir un nom unique. 
        /// L'opération sort en erreur si l'une des deux conditions, ou les deux, n'est pas respectée.</remarks>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]SelectorConfig selectConf)
        {
            try
            {
                HttpResponseMessageResult res = await Adapter.Add(selectConf);

                if (res.IsSuccess)
                {
                    Logger.LogInformation("Add SelectorConfig with success.");
                    return Ok(res);
                }
                else
                {
                    Logger.LogWarning(LoggingEvents.WARNING_ERROR, res.Message);
                    return StatusCode(500, res.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Ajoute un criteria dans un SelectorConfig.
        /// </summary>
        /// <param name="criteria">Criteria à ajouter</param>
        /// <returns>Message de retour.</returns>
        /// <remarks>L'objet Criteria doit contenir l'id du SelectorConfig dans lequel il faut l'ajouter. L'API retourne une erreur
        /// lorsque la dimension ou la valeur du Criteria n'est pas définie. 
        /// Les valeurs possibles d'un Criteria sont '*', chaine-de-caractères, '{valeur1, valeur2, ..., valeurn}' </remarks>
        [HttpPost("AddCriteria")]
        public async Task<IActionResult> AddCriteria([FromBody]Criteria criteria)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                criteria = await CriteriaAdapter.Add(criteria);
                res.GetObjectForJson(criteria);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Ajoute un SelectorConfig en Previous Propagate (PrevPropagate) d'un autre SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="previousSelectorConf">SelectorConfig à ajouter en Previous Propagate</param>
        /// <returns>Message de resultat</returns>
        /// <remarks>Il faut que les id donnés en paramètre existent en base de donnée. Si le SelectorConfig cible
        /// posséde déjà le SelectorConfig en PrevPropagate, il ne se passe rien.</remarks>
        [HttpPut("AddPreviousPropagate")]
        public async Task<IActionResult> AddPreviousPropagate(long idSelectorConfig, [FromBody]SelectorConfig previousSelectorConf)
        {
            try
            {
                HttpResponseMessageResult res = await Adapter.AddPreviousPropagate(idSelectorConfig, previousSelectorConf);

                if (res.IsSuccess)
                {
                    Logger.LogInformation($"Add PrevPropagate {previousSelectorConf.Id} to SelectorConfig {idSelectorConfig} with success.");
                    return Ok();
                }
                else
                {
                    Logger.LogWarning(LoggingEvents.WARNING_ERROR, res.Message);
                    return StatusCode(500, res.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Ajoute un SelectorConfig en Propagate d'un autre SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="propagateSelectorConf">SelectorConfig à ajouter en Propagate</param>
        /// <returns>Message de resultat</returns>
        /// <remarks>Il faut que les id donnés en paramètre existent en base de donnée. Si le SelectorConfig cible
        /// posséde déjà le SelectorConfig en Propagate, il ne se passe rien.</remarks>
        [HttpPut("AddPropagate")]
        public async Task<IActionResult> AddPropagate(long idSelectorConfig, [FromBody]SelectorConfig propagateSelectorConf)
        {
            try
            {
                HttpResponseMessageResult res = await Adapter.AddPropagate(idSelectorConfig, propagateSelectorConf);

                if (res.IsSuccess)
                {
                    Logger.LogInformation($"Add Propagate {propagateSelectorConf.Id} to SelectorConfig {idSelectorConfig} with success.");
                    return Ok();
                }
                else
                {
                    Logger.LogWarning(LoggingEvents.WARNING_ERROR, res.Message);
                    return StatusCode(500, res.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Ajoute un Criteria à un SelectorConfig pour cibler les données à modifier
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria définissant les valeurs à modifier</param>
        /// <returns>Message de résultat</returns>
        /// <remarks>Ajoute juste l'objet Criteria au SelectorConfig. Vérifie l'existance du SelectorConfig mais ne controle pas si le résultat 
        /// du Criteria sur les potentiels subset du SelectorConfig contient bel et bien des données.</remarks>
        [HttpPut("AddCriteriaToModifyValue")]
        public async Task<IActionResult> AddCriteriaToModifyValue(long idSelectorConfig, [FromBody]Criteria criteria)
        {
            try
            {
                HttpResponseMessageResult res = await Adapter.AddCriteriaToModifyValue(idSelectorConfig, criteria);

                if (res.IsSuccess)
                {
                    Logger.LogInformation($"Add criteria for modify values to SelectorConfig {idSelectorConfig} with success.");
                    return Ok();
                }
                else
                {
                    Logger.LogWarning(LoggingEvents.WARNING_ERROR, res.Message);
                    return StatusCode(500, res.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Ajoute un criteria dans la liste ordonnée des criterias de modificateurs d'un SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria destiné à la liste des modificateurs</param>
        /// <returns>Message de retour</returns>
        [HttpPut("AddModifiersCriteria")]
        public async Task<IActionResult> AddModifiersCriteria(long idSelectorConfig, [FromBody]Criteria criteria)
        {
            try
            {
                HttpResponseMessageResult res = await Adapter.AddModifiersCriteria(idSelectorConfig, criteria);

                if (res.IsSuccess)
                {
                    Logger.LogInformation($"Add modifiers criteria to SelectorConfig {idSelectorConfig} with success.");
                    return Ok();
                }
                else
                {
                    Logger.LogWarning(LoggingEvents.WARNING_ERROR, res.Message);
                    return StatusCode(500, res.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Ajoute un criteria dans la liste ordonnée des criterias de validateurs d'un SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria destiné à la liste des validateurs</param>
        /// <returns>Message de retour</returns>
        [HttpPut("AddValidatorsCriteria")]
        public async Task<IActionResult> AddValidatorsCriteria(long idSelectorConfig, [FromBody]Criteria criteria)
        {
            try
            {
                HttpResponseMessageResult res = await Adapter.AddValidatorsCriteria(idSelectorConfig, criteria);

                if (res.IsSuccess)
                {
                    Logger.LogInformation($"Add validator criteria to SelectorConfig {idSelectorConfig} with success.");
                    return Ok();
                }
                else
                {
                    Logger.LogWarning(LoggingEvents.WARNING_ERROR, res.Message);
                    return StatusCode(500, res.Message);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Récupere la liste des SelectorConfig et de leurs SelectorInstance lancés d'un WorkflowInstance.
        /// </summary>
        /// <param name="workflowInstanceId">Id du workflowInstance</param>
        /// <returns>Liste de SelectorConfig (et donc de leur SelectorInstance)</returns>
        [HttpGet("Selectors")]
        public async Task<IActionResult> Selectors(long workflowInstanceId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                IEnumerable<SelectorConfig> lstSC = await Adapter.GetSelectors(workflowInstanceId);
                if (lstSC != null)
                    res.GetObjectForJson(lstSC);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }
}
