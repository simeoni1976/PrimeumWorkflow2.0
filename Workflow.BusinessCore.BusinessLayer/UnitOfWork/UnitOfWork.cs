using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Workflow.BusinessCore.DataLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Workflow.BusinessCore.DataLayer.Repositories.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.Helpers;
using Workflow.Transverse.Environment;

namespace Workflow.BusinessCore.BusinessLayer.UnitOfWork
{
    /// <summary>
    /// UnitOfWork class.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// On va chercher le context dans la session courte. Cela permet de limiter une connexion par hit http (et par utilisateur).
        /// </summary>
        private ApplicationContext AppContext
        {
            get
            {
                ApplicationContext appDb = SessionStatsHelper.HttpHitGetItem<ApplicationContext>(Constant.S_DBCONTEXT, _serviceProvider);
                if (appDb == null)
                {
                    appDb = _serviceProvider.GetService<ApplicationContext>();
                    SessionStatsHelper.HttpHitSaveItem(Constant.S_DBCONTEXT, appDb, _serviceProvider);
                }
                return appDb;
            }
        }
        private readonly IServiceProvider _serviceProvider = null;


        private IRepository<ValueObject> _valueObjectRepository;
        private IRepository<WorkflowInstance> _workflowInstanceRepository;
        private IRepository<WorkflowConfig> _workflowConfigRepository;
        private IRepository<SelectorInstance> _selectorInstanceRepository;
        private IRepository<SelectorConfig> _selectorConfigRepository;
        private IRepository<DataSet> _dataSetRepository;
        private IRepository<Dimension> _dimensionRepository;
        private IRepository<WorkflowDimension> _workflowDimensionRepository;
        private IRepository<Criteria> _criteriaRepository;
        private IRepository<DataSetDimension> _dataSetDimensionRepository;
        private IRepository<CriteriaValues> _criteriaValuesRepository;
        private IRepository<User> _userRepository;
        private IRepository<Comment> _commentRepository;
        private IRepository<DimensionGroupData> _dimensionGroupDataRepository;
        private IRepository<DimensionPeriodData> _dimensionPeriodDataRepository;
        private IRepository<DimensionScalarData> _dimensionScalarDataRepository;
        private IRepository<DimensionTreeData> _dimensionTreeDataRepository;
        private IRepository<DimensionValues> _dimensionValuesRepository;
        private IRepository<UserSet> _userSetRepository;
        private IRepository<UserSetUser> _userSetUserRepository;
        private IRepository<ConditionnedCriteria> _conditionnedCriteriaRepository;
        private IRepository<ConditionnedCriteriaValues> _conditionnedCriteriaValuesRepository;
        private IRepository<DistinctValue> _distinctValueRepository;
        private IRepository<GridConfig> _gridConfigRepository;
        private IRepository<GridDimensionConfig> _gridDimensionConfigRepository;
        private IRepository<GridValueConfig> _gridValueConfigRepository;
        private IRepository<ConfigVariable> _configVariableRepository;
        private IRepository<ENT.Action> _actionRepository;
        private IRepository<ActionSequence> _actionSequenceRepositry;
        private IRepository<ActionParameter> _actionParameterRepository;
        private IRepository<Constraint> _constraintRepository;
        private IRepository<ConstraintSequence> _constraintSequenceRepositry;
        private IRepository<ConstraintParameter> _constraintParameterRepository;
        private IRepository<SelectorInstanceUser> _selectorInstanceUserRepository;


        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// This function permits to get the DbContext
        /// </summary>
        /// <returns>ApplicationContext</returns>
        public ApplicationContext GetDbContext()
        {
            return AppContext;
        }

        /// <summary>
        /// CriteriaRepository property.
        /// </summary>
        /// <value>
        /// Gets the CriteriaRepository value.
        /// </value>
        public IRepository<Criteria> CriteriaRepository
        {
            get
            {
                return _criteriaRepository = _criteriaRepository ?? new Repository<Criteria>(AppContext);
            }
        }

        /// <summary>
        /// ValueObjectRepository property.
        /// </summary>
        /// <value>
        /// Gets the ValueObjectRepository value.
        /// </value>
        public IRepository<ValueObject> ValueObjectRepository
        {
            get
            {
                return _valueObjectRepository = _valueObjectRepository ?? new Repository<ValueObject>(AppContext);
            }
        }

        /// <summary>
        /// WorkflowInstanceRepository property.
        /// </summary>
        /// <value>
        /// Gets the WorkflowInstanceRepository value.
        /// </value>
        public IRepository<WorkflowInstance> WorkflowInstanceRepository
        {
            get
            {
                return _workflowInstanceRepository = _workflowInstanceRepository ?? new Repository<WorkflowInstance>(AppContext);
            }
        }

        /// <summary>
        /// SelectorInstanceRepository property.
        /// </summary>
        /// <value>
        /// Gets the SelectorInstanceRepository value.
        /// </value>
        public IRepository<SelectorInstance> SelectorInstanceRepository
        {
            get
            {
                return _selectorInstanceRepository = _selectorInstanceRepository ?? new Repository<SelectorInstance>(AppContext);
            }
        }

        /// <summary>
        /// SelectorConfigRepository property.
        /// </summary>
        /// <value>
        /// Gets the SelectorConfigRepository value.
        /// </value>
        public IRepository<SelectorConfig> SelectorConfigRepository
        {
            get
            {
                return _selectorConfigRepository = _selectorConfigRepository ?? new Repository<SelectorConfig>(AppContext);
            }
        }

        /// <summary>
        /// WorkflowConfigRepository property.
        /// </summary>
        /// <value>
        /// Gets the WorkflowConfigRepository value.
        /// </value>
        public IRepository<WorkflowConfig> WorkflowConfigRepository
        {
            get
            {
                return _workflowConfigRepository = _workflowConfigRepository ?? new Repository<WorkflowConfig>(AppContext);
            }
        }

        /// <summary>
        /// DataSetRepository property.
        /// </summary>
        /// <value>
        /// Gets the DataSetRepository value.
        /// </value>
        public IRepository<DataSet> DataSetRepository
        {
            get
            {
                return _dataSetRepository = _dataSetRepository ?? new Repository<DataSet>(AppContext);
            }
        }

        /// <summary>
        /// DimensionRepository property.
        /// </summary>
        /// <value>
        /// Gets the DimensionRepository value.
        /// </value>
        public IRepository<Dimension> DimensionRepository
        {
            get
            {
                return _dimensionRepository = _dimensionRepository ?? new Repository<Dimension>(AppContext);
            }
        }

        /// <summary>
        /// WorkflowDimensionRepository property.
        /// </summary>
        /// <value>
        /// Gets the WorkflowDimensionRepository value.
        /// </value>
        public IRepository<WorkflowDimension> WorkflowDimensionRepository
        {
            get
            {
                return _workflowDimensionRepository = _workflowDimensionRepository ?? new Repository<WorkflowDimension>(AppContext);
            }
        }

        /// <summary>
        /// DataSetDimensionRepository property.
        /// </summary>
        /// <value>
        /// Gets the DataSetDimensionRepository value.
        /// </value>
        public IRepository<DataSetDimension> DataSetDimensionRepository
        {
            get
            {
                return _dataSetDimensionRepository = _dataSetDimensionRepository ?? new Repository<DataSetDimension>(AppContext);
            }
        }

        /// <summary>
        /// CriteriaValuesRepository property.
        /// </summary>
        /// <value>
        /// Gets the CriteriaValuesRepository value.
        /// </value>
        public IRepository<CriteriaValues> CriteriaValuesRepository
        {
            get
            {
                return _criteriaValuesRepository = _criteriaValuesRepository ?? new Repository<CriteriaValues>(AppContext);
            }
        }

        /// <summary>
        /// UserRepository property.
        /// </summary>
        /// <value>
        /// Gets the UserRepository value.
        /// </value>
        public IRepository<User> UserRepository
        {
            get
            {
                return _userRepository = _userRepository ?? new Repository<User>(AppContext);
            }
        }

        /// <summary>
        /// CommentRepository property.
        /// </summary>
        /// <value>
        /// Gets the CommentRepository value.
        /// </value>
        public IRepository<Comment> CommentRepository
        {
            get
            {
                return _commentRepository = _commentRepository ?? new Repository<Comment>(AppContext);
            }
        }

        /// <summary>
        /// CommentRepository property.
        /// </summary>
        /// <value>
        /// Gets the CommentRepository value.
        /// </value>
        public IRepository<DimensionPeriodData> DimensionPeriodDataRepository
        {
            get
            {
                return _dimensionPeriodDataRepository = _dimensionPeriodDataRepository ?? new Repository<DimensionPeriodData>(AppContext);
            }
        }

        /// <summary>
        /// DimensionGroupDataRepository property.
        /// </summary>
        /// <value>
        /// Gets the DimensionGroupDataRepository value.
        /// </value>
        public IRepository<DimensionGroupData> DimensionGroupDataRepository
        {
            get
            {
                return _dimensionGroupDataRepository = _dimensionGroupDataRepository ?? new Repository<DimensionGroupData>(AppContext);
            }
        }

        /// <summary>
        /// DimensionScalarDataRepository property.
        /// </summary>
        /// <value>
        /// Gets the DimensionScalarDataRepository value.
        /// </value>
        public IRepository<DimensionScalarData> DimensionScalarDataRepository
        {
            get
            {
                return _dimensionScalarDataRepository = _dimensionScalarDataRepository ?? new Repository<DimensionScalarData>(AppContext);
            }
        }

        /// <summary>
        /// DimensionTreeDataRepository property.
        /// </summary>
        /// <value>
        /// Gets the DimensionTreeDataRepository value.
        /// </value>
        public IRepository<DimensionTreeData> DimensionTreeDataRepository
        {
            get
            {
                return _dimensionTreeDataRepository = _dimensionTreeDataRepository ?? new Repository<DimensionTreeData>(AppContext);
            }
        }

        /// <summary>
        /// DimensionValuesRepository property.
        /// </summary>
        /// <value>
        /// Gets the DimensionValuesRepository value.
        /// </value>
        public IRepository<DimensionValues> DimensionValuesRepository
        {
            get
            {
                return _dimensionValuesRepository = _dimensionValuesRepository ?? new Repository<DimensionValues>(AppContext);
            }
        }

        /// <summary>
        /// UserSetRepository property.
        /// </summary>
        /// <value>
        /// Gets the UserSetRepository value.
        /// </value>
        public IRepository<UserSet> UserSetRepository
        {
            get
            {
                return _userSetRepository = _userSetRepository ?? new Repository<UserSet>(AppContext);
            }
        }

        /// <summary>
        /// UserSetSetRepository property.
        /// </summary>
        /// <value>
        /// Gets the UserSetSetRepository value.
        /// </value>
        public IRepository<UserSetUser> UserSetUserRepository
        {
            get
            {
                return _userSetUserRepository = _userSetUserRepository ?? new Repository<UserSetUser>(AppContext);
            }
        }

        /// <summary>
        /// Repo des ConditionnedCriteria
        /// </summary>
        public IRepository<ConditionnedCriteria> ConditionnedCriteriaRepository
        {
            get
            {
                return _conditionnedCriteriaRepository = _conditionnedCriteriaRepository ?? new Repository<ConditionnedCriteria>(AppContext);
            }
        }

        /// <summary>
        /// Repo des ConditionnedCriteriaValues
        /// </summary>
        public IRepository<ConditionnedCriteriaValues> ConditionnedCriteriaValuesRepository
        {
            get
            {
                return _conditionnedCriteriaValuesRepository = _conditionnedCriteriaValuesRepository ?? new Repository<ConditionnedCriteriaValues>(AppContext);
            }
        }

        /// <summary>
        /// Repo des DistinctValue
        /// </summary>
        public IRepository<DistinctValue> DistinctValueRepository
        {
            get
            {
                return _distinctValueRepository = _distinctValueRepository ?? new Repository<DistinctValue>(AppContext);
            }
        }

        /// <summary>
        /// Repo des configuration de grid
        /// </summary>
        public IRepository<GridConfig> GridConfigRepository
        {
            get
            {
                return _gridConfigRepository = _gridConfigRepository ?? new Repository<GridConfig>(AppContext);
            }
        }

        /// <summary>
        /// Repo des configuration de dimension de grid
        /// </summary>
        public IRepository<GridDimensionConfig> GridDimensionConfigRepository
        {
            get
            {
                return _gridDimensionConfigRepository = _gridDimensionConfigRepository ?? new Repository<GridDimensionConfig>(AppContext);
            }
        }

        /// <summary>
        /// Repo de configuration de valeur de dimension de grid.
        /// </summary>
        public IRepository<GridValueConfig> GridValueConfigRepository
        {
            get
            {
                return _gridValueConfigRepository = _gridValueConfigRepository ?? new Repository<GridValueConfig>(AppContext);
            }
        }

        /// <summary>
        /// Repo des variables de configuration
        /// </summary>
        public IRepository<ConfigVariable> ConfigVariableRepository
        {
            get
            {
                return _configVariableRepository = _configVariableRepository ?? new Repository<ConfigVariable>(AppContext);
            }
        }

        /// <summary>
        /// Repo des actions
        /// </summary>
        public IRepository<ENT.Action> ActionRepository
        {
            get
            {
                return _actionRepository = _actionRepository ?? new Repository<ENT.Action>(AppContext);
            }
        }

        /// <summary>
        /// Repo des séquences d'actions.
        /// </summary>
        public IRepository<ActionSequence> ActionSequenceRepository
        {
            get
            {
                return _actionSequenceRepositry = _actionSequenceRepositry ?? new Repository<ActionSequence>(AppContext);
            }
        }

        /// <summary>
        /// Repo des paramètres d'actions.
        /// </summary>
        public IRepository<ActionParameter> ActionParameterRepository
        {
            get
            {
                return _actionParameterRepository = _actionParameterRepository ?? new Repository<ActionParameter>(AppContext);
            }
        }

        /// <summary>
        /// Repo des contraintes globales
        /// </summary>
        public IRepository<Constraint> ConstraintRepository
        {
            get
            {
                return _constraintRepository = _constraintRepository ?? new Repository<Constraint>(AppContext);
            }
        }

        /// <summary>
        /// Repo des séquences de contraintes
        /// </summary>
        public IRepository<ConstraintSequence> ConstraintSequenceRepository
        {
            get
            {
                return _constraintSequenceRepositry = _constraintSequenceRepositry ?? new Repository<ConstraintSequence>(AppContext);
            }
        }

        /// <summary>
        /// Repo des paramétres de contraintes
        /// </summary>
        public IRepository<ConstraintParameter> ConstraintParameterRepository
        {
            get
            {
                return _constraintParameterRepository = _constraintParameterRepository ?? new Repository<ConstraintParameter>(AppContext);
            }
        }

        /// <summary>
        /// Repo des SelectorInstanceUser
        /// </summary>
        public IRepository<SelectorInstanceUser> SelectorInstanceUserRepository
        {
            get
            {
                return _selectorInstanceUserRepository = _selectorInstanceUserRepository ?? new Repository<SelectorInstanceUser>(AppContext);
            }
        }

        


        /// <summary>
        /// This method permits to dispose.
        /// </summary>
        public void Dispose()
        {
            AppContext.Dispose();
        }
    }
}
