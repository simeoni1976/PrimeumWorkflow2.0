using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.Frontal.BusinessLogic.Models;
using Workflow.Frontal.BusinessLogic.Services;
using Workflow.Transverse.DTO;
using Workflow.Transverse.Helpers;

namespace Workflow.Frontal.BusinessLogic.Grid
{
    /// <summary>
    /// DashBoard configuration
    /// </summary>
    public class DashBoard
    {
 
        private ServiceFacade _serviceFacade;

        /// <summary>
        /// DashBoardWorkflows property.
        /// </summary>
        /// <value>
        /// Gets or sets the DashBoardWorkflows value.
        /// </value>
        public List<DashBoardWorkflowModel> DashBoardWorkflows { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public DashBoard(ServiceFacade serviceFacade)
        {
            DashBoardWorkflows = new List<DashBoardWorkflowModel>();
            _serviceFacade = serviceFacade;
        }
        
        /// <summary>
        /// Build the dashBoard grid
        /// </summary>
        /// <param name="userId">Id from User</param>
        public async Task BuildStructureGrid(long userId)
        {
            List<DashBoardWorkflowModel> _dashBoardWorkflows = new List<DashBoardWorkflowModel>();

            AbstractServiceFactory service = _serviceFacade[ServiceFacade.SERVICENAME_USER];
            HttpResponseMessageResult result = await service.Get("GetWorkflowInstance", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("userId", userId.ToString()) });

            if(result != null && !string.IsNullOrWhiteSpace(result.Json))
            {
                IEnumerable<WorkflowInstance> workflowInstances = JsonConvert.DeserializeObject<List<WorkflowInstance>>(result.Json);
                foreach(WorkflowInstance workflowInstance in workflowInstances)
                {
                    _dashBoardWorkflows.Add(await SetDashBoardWorkflowModel(workflowInstance));
                }
            } 
                      

            DashBoardWorkflows = _dashBoardWorkflows;
        }

        /// <summary>
        /// This method permits to initialize the workflow model.
        /// </summary>
        /// <param name="workflowInstance"></param>
        /// <returns></returns>
        private async Task<DashBoardWorkflowModel> SetDashBoardWorkflowModel(WorkflowInstance workflowInstance)
        {
            DashBoardWorkflowModel workflow = new DashBoardWorkflowModel();
            workflow.WorkflowInstanceId = workflowInstance.Id;
            workflow.Title = string.IsNullOrWhiteSpace(workflowInstance.Name) ? $"WorkflowInstance{workflowInstance.Id}" : workflowInstance.Name;
            long workflowModifyTotal = 0,
                workflowValidatedTotal = 0;

            List<DashBoardSectionModel> sections = new List<DashBoardSectionModel>();

            AbstractServiceFactory service = _serviceFacade[ServiceFacade.SERVICENAME_SELECTORCONFIG];
            HttpResponseMessageResult result = await service.Get("Selectors", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("WorkflowInstanceId", workflowInstance.Id.ToString()) });

            if (result != null && !string.IsNullOrWhiteSpace(result.Json))
            {
                IEnumerable<SelectorConfig> selectorConfigs = JsonConvert.DeserializeObject<List<SelectorConfig>>(result.Json);
                if (selectorConfigs != null)
                    foreach (SelectorConfig selectorConfig in selectorConfigs)
                    {
                        DashBoardSectionModel section = new DashBoardSectionModel();
                        long toDoTotal = 0, 
                            pendingValidationTotal = 0, 
                            validatedModifTotal = 0,
                            validatedValidTotal = 0,
                            toBeValidatedTotal = 0,
                            notSubmitedTotal = 0;

                        section.Modification = new DashBoardModificationActionModel();
                        section.Modification.Activities = SetModifications(selectorConfig, 
                            ref workflowModifyTotal, 
                            ref toDoTotal, 
                            ref pendingValidationTotal, 
                            ref validatedModifTotal);
                        section.Modification.ToDoTotal = toDoTotal;
                        section.Modification.PendingValidationTotal = pendingValidationTotal;
                        section.Modification.ValidatedTotal = validatedModifTotal;

                        section.Validation = new DashBoardValidationActionModel ();
                        section.Validation.Activities = SetValidations(selectorConfig,
                            ref workflowValidatedTotal,
                            ref notSubmitedTotal,
                            ref toBeValidatedTotal,
                            ref validatedValidTotal);
                        section.Validation.NotSubmitedTotal = notSubmitedTotal;
                        section.Validation.ToBeValidatedTotal = toBeValidatedTotal;
                        section.Validation.ValidatedTotal = validatedValidTotal;

                        section.Description = selectorConfig.Description;
                        section.DeadLine = null;

                        sections.Add(section);
                    }
            }

            workflow.ModificationTotal = workflowModifyTotal;
            workflow.ValidationTotal = workflowValidatedTotal;
            workflow.Sections = sections;

            return workflow;
        }

        /// <summary>
        /// This method permits to initialize the modifcation part.
        /// </summary>
        /// <param name="selectorConfig"></param>
        /// <param name="workflowModifyTotal"></param>
        /// <param name="toDoTotal"></param>
        /// <param name="pendingValidationTotal"></param>
        /// <param name="validatedTotal"></param>
        /// <returns></returns>
        private List<DashBoardModificationActivityModel> SetModifications(SelectorConfig selectorConfig,
            ref long workflowModifyTotal,
            ref long toDoTotal,
            ref long pendingValidationTotal, 
            ref long validatedTotal)
        {
            List<DashBoardModificationActivityModel> activities = new List<DashBoardModificationActivityModel>();
            foreach(SelectorInstance instance in selectorConfig.SelectorInstance)
            {
                ModificationStatusEnum activityStatus = ModificationStatusEnum.None;
                bool isRegistred = false;

                if (instance.Status == SelectorStateEnum.Init 
                    || instance.Status == SelectorStateEnum.Create 
                    || instance.Status == SelectorStateEnum.PrevPropagate)
                {
                    toDoTotal =+1;
                    activityStatus = ModificationStatusEnum.ToDo;
                    isRegistred = true;
                }

                if (instance.Status == SelectorStateEnum.Modify)
                {
                    workflowModifyTotal++;
                    pendingValidationTotal++;
                    activityStatus = ModificationStatusEnum.PendingValidation;
                    isRegistred = true;
                }

                if (instance.Status == SelectorStateEnum.Act
                    || instance.Status == SelectorStateEnum.Constraint)
                {
                    validatedTotal++;
                    activityStatus = ModificationStatusEnum.Validated;
                    isRegistred = true;
                }

                if (isRegistred != false)
                {
                    activities.Add(new DashBoardModificationActivityModel()
                    {
                        Affected = instance.ModificatorName,
                        LastAction = instance.ModifiedDate.TimeAgo(),
                        Node = instance.DimensionValueImportant,
                        Status = activityStatus,
                        SelectorInstanceId = instance.Id
                    });
                }
            }

            return activities;
        }

        /// <summary>
        /// This method permits to initialize the validation part.
        /// </summary>
        /// <param name="selectorConfig"></param>
        /// <param name="workflowValidatedTotal"></param>
        /// <param name="notSubmitedTotal"></param>
        /// <param name="toBeValidated"></param>
        /// <param name="validatedTotal"></param>
        /// <returns></returns>
        private List<DashBoardValidationActivityModel> SetValidations(SelectorConfig selectorConfig,
        ref long workflowValidatedTotal,
        ref long notSubmitedTotal,
        ref long toBeValidated,
        ref long validatedTotal)
        {
            List<DashBoardValidationActivityModel> activities = new List<DashBoardValidationActivityModel>();
            foreach (SelectorInstance instance in selectorConfig.SelectorInstance)
            {
                ValidationStatusEnum activityStatus = ValidationStatusEnum.None;
                bool isRegistred = false;

                if (instance.Status == SelectorStateEnum.Act 
                    || instance.Status == SelectorStateEnum.Constraint)
                {
                    toBeValidated++;
                    notSubmitedTotal++;
                    activityStatus = ValidationStatusEnum.ToBeValidated;
                    isRegistred = true;
                }

                if (instance.Status == SelectorStateEnum.Validate)
                {
                    workflowValidatedTotal++;
                    validatedTotal++;
                    notSubmitedTotal++;
                    activityStatus = ValidationStatusEnum.Validated;
                    isRegistred = true;
                }

                if (isRegistred != false)
                {
                    activities.Add(new DashBoardValidationActivityModel()
                    {
                        Affected = instance.ValidatorsNames,
                        LastAction = instance.ModifiedDate.TimeAgo(),
                        Node = instance.DimensionValueImportant,
                        Status = activityStatus,
                        SelectorInstanceId = instance.Id
                    });
                }
            }

            return activities;
        }
    }
}
