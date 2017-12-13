using Workflow.BusinessCore.DataLayer.Entities;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.DataLayer.Repositories;
using Workflow.BusinessCore.DataLayer.Repositories.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces
{
    /// <summary>
    /// UnitOfWork interface.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// This function permits to get the DbContext
        /// </summary>
        /// <returns>ApplicationContext</returns>
        ApplicationContext GetDbContext();

        /// <summary>
        /// CriteriaRepository property.
        /// </summary>
        /// <value>
        /// Gets the CriteriaRepository value.
        /// </value>
        IRepository<Criteria> CriteriaRepository { get; }

        /// <summary>
        /// SelectorConfigRepository property.
        /// </summary>
        /// <value>
        /// Gets the SelectorConfigRepository value.
        /// </value>
        IRepository<SelectorConfig> SelectorConfigRepository { get; }

        /// <summary>
        /// SelectorInstanceRepository property.
        /// </summary>
        /// <value>
        /// Gets the SelectorInstanceRepository value.
        /// </value>
        IRepository<SelectorInstance> SelectorInstanceRepository { get; }

        /// <summary>
        /// ValueObjectRepository property.
        /// </summary>
        /// <value>
        /// Gets the ValueObjectRepository value.
        /// </value>
        IRepository<ValueObject> ValueObjectRepository { get; }

        /// <summary>
        /// WorkflowConfigRepository property.
        /// </summary>
        /// <value>
        /// Gets the WorkflowConfigRepository value.
        /// </value>
        IRepository<WorkflowConfig> WorkflowConfigRepository { get; }

        /// <summary>
        /// VWorkflowInstanceRepository property.
        /// </summary>
        /// <value>
        /// Gets the VWorkflowInstanceRepository value.
        /// </value>
        IRepository<WorkflowInstance> WorkflowInstanceRepository { get; }

        /// <summary>
        /// DataSetRepository property.
        /// </summary>
        /// <value>
        /// Gets the DataSetRepository value.
        /// </value>
        IRepository<DataSet> DataSetRepository { get; }

        /// <summary>
        /// DimensionRepository property.
        /// </summary>
        /// <value>
        /// Gets the DimensionRepository value.
        /// </value>
        IRepository<Dimension> DimensionRepository { get; }

        /// <summary>
        /// WorkflowDimensionRepository property.
        /// </summary>
        /// <value>
        /// Gets the WorkflowDimensionRepository value.
        /// </value>
        IRepository<WorkflowDimension> WorkflowDimensionRepository { get; }

        /// <summary>
        /// DataSetDimensionRepository property.
        /// </summary>
        /// <value>
        /// Gets the DataSetDimensionRepository value.
        /// </value>
        IRepository<DataSetDimension> DataSetDimensionRepository { get; }

        /// <summary>
        /// CriteriaValuesRepository property.
        /// </summary>
        /// <value>
        /// Gets the CriteriaValuesRepository value.
        /// </value>
        IRepository<CriteriaValues> CriteriaValuesRepository { get; }

        /// <summary>
        /// UserRepository property.
        /// </summary>
        /// <value>
        /// Gets the UserRepository value.
        /// </value>
        IRepository<User> UserRepository { get; }

        /// <summary>
        /// CommentRepository property.
        /// </summary>
        /// <value>
        /// Gets the CommentRepository value.
        /// </value>
        IRepository<Comment> CommentRepository { get; }

        /// <summary>
        /// DimensionPeriodDataRepository property.
        /// </summary>
        /// <value>
        /// Gets the DimensionPeriodDataRepository value.
        /// </value>
        IRepository<DimensionPeriodData> DimensionPeriodDataRepository { get; }

        /// <summary>
        /// DimensionGroupDataRepository property.
        /// </summary>
        /// <value>
        /// Gets the DimensionGroupDataRepository value.
        /// </value>
        IRepository<DimensionGroupData> DimensionGroupDataRepository { get; }

        /// <summary>
        /// DimensionScalarDataRepository property.
        /// </summary>
        /// <value>
        /// Gets the DimensionScalarDataRepository value.
        /// </value>
        IRepository<DimensionScalarData> DimensionScalarDataRepository { get; }

        /// <summary>
        /// DimensionTreeDataRepository property.
        /// </summary>
        /// <value>
        /// Gets the DimensionTreeDataRepository value.
        /// </value>
        IRepository<DimensionTreeData> DimensionTreeDataRepository { get; }

        /// <summary>
        /// DimensionValuesRepository property.
        /// </summary>
        /// <value>
        /// Gets the DimensionValuesRepository value.
        /// </value>
        IRepository<DimensionValues> DimensionValuesRepository { get; }

        /// <summary>
        /// UserSetRepository property.
        /// </summary>
        /// <value>
        /// Gets the UserSetRepository value.
        /// </value>
        IRepository<UserSet> UserSetRepository { get; }

        /// <summary>
        /// UserSetUserRepository property.
        /// </summary>
        /// <value>
        /// Gets the UserSetUserRepository value.
        /// </value>
        IRepository<UserSetUser> UserSetUserRepository { get; }

        /// <summary>
        /// Repo des ConditionnedCriteria
        /// </summary>
        IRepository<ConditionnedCriteria> ConditionnedCriteriaRepository { get; }

        /// <summary>
        /// Repo des ConditionnedCriteriaValues
        /// </summary>
        IRepository<ConditionnedCriteriaValues> ConditionnedCriteriaValuesRepository { get; }

        /// <summary>
        /// Repo des DistinctValue
        /// </summary>
        IRepository<DistinctValue> DistinctValueRepository { get; }

        /// <summary>
        /// Repo des configuration de grid
        /// </summary>
        IRepository<GridConfig> GridConfigRepository { get; }

        /// <summary>
        /// Repo des configuration de dimension de grid
        /// </summary>
        IRepository<GridDimensionConfig> GridDimensionConfigRepository { get; }

        /// <summary>
        /// Repo de configuration de valeur de dimension de grid.
        /// </summary>
        IRepository<GridValueConfig> GridValueConfigRepository { get; }

        /// <summary>
        /// Repo des variables de configuration
        /// </summary>
        IRepository<ConfigVariable> ConfigVariableRepository { get; }

        /// <summary>
        /// Repo des actions
        /// </summary>
        IRepository<ENT.Action> ActionRepository { get; }

        /// <summary>
        /// Repo des séquences d'actions.
        /// </summary>
        IRepository<ActionSequence> ActionSequenceRepository { get; }

        /// <summary>
        /// Repo des paramètres d'actions
        /// </summary>
        IRepository<ActionParameter> ActionParameterRepository { get; }

        /// <summary>
        /// Repo des contraintes globales
        /// </summary>
        IRepository<Constraint> ConstraintRepository { get; }

        /// <summary>
        /// Repo des séquences de contraintes
        /// </summary>
        IRepository<ConstraintSequence> ConstraintSequenceRepository { get; }

        /// <summary>
        /// Repo des paramétres de contraintes
        /// </summary>
        IRepository<ConstraintParameter> ConstraintParameterRepository { get; }

        /// <summary>
        /// Repo des SelectorInstanceUser.
        /// </summary>
        IRepository<SelectorInstanceUser> SelectorInstanceUserRepository { get; }
    }
}
