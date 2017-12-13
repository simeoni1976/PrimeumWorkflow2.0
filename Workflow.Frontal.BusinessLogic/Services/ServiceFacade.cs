using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Workflow.Frontal.BusinessLogic.Services
{
    /// <summary>
    /// ServiceProvider class.
    /// </summary>
    public class ServiceFacade
    {
        public const string SERVICENAME_ACTION = "Action";
        public const string SERVICENAME_ACTIONSEQUENCE = "ActionSequence";
        public const string SERVICENAME_ACTIONPARAMETER = "ActionParameter";
        public const string SERVICENAME_CONSTRAINT = "Constraint";
        public const string SERVICENAME_CONSTRAINTSEQUENCE = "ConstraintSequence";
        public const string SERVICENAME_CONSTRAINTPARAMETER = "ConstraintParameter";
        public const string SERVICENAME_WORKFLOWCONFIG = "WorkflowConfig";
        public const string SERVICENAME_SELECTORCONFIG = "SelectorConfig";
        public const string SERVICENAME_DATASET = "DataSet";
        public const string SERVICENAME_VALUEOBJECT = "ValueObject";
        public const string SERVICENAME_GRIDCONFIGURATION = "GridConfiguration";
        public const string SERVICENAME_USER = "User";
        public const string SERVICENAME_USERSET = "UserSet";


        private const string KEYSERVICE_ACTION = "Action";
        private const string KEYSERVICE_ACTIONSEQUENCE = "ActionSequence";
        private const string KEYSERVICE_ACTIONPARAMETER = "ActionParameter";
        private const string KEYSERVICE_CONSTRAINT = "Constraint";
        private const string KEYSERVICE_CONSTRAINTSEQUENCE = "ConstraintSequence";
        private const string KEYSERVICE_CONSTRAINTPARAMETER = "ConstraintParameter";
        private const string KEYSERVICE_WORKFLOWCONFIG = "WorkflowConfig";
        private const string KEYSERVICE_SELECTOR = "Selector";
        private const string KEYSERVICE_SELECTORCONFIG = "SelectorConfig";
        private const string KEYSERVICE_VALUEOBJECT_FILTER = "ValueObject/Filter";
        private const string KEYSERVICE_COMMENT = "Comment";
        private const string KEYSERVICE_USER = "User";
        private const string KEYSERVICE_USER_LOGIN = "User/Login";
        private const string KEYSERVICE_USER_ROLE = "User/Role";
        private const string KEYSERVICE_USERSET = "UserSet";
        private const string KEYSERVICE_DATASET = "DataSet";
        private const string KEYSERVICE_GRIDCONFIGURATION = "GridConfiguration";
        private const string KEYSERVICE_VALUEOBJECT = "ValueObject";

        private Dictionary<string, Tuple<Type, string>> _linkKeysNames = new Dictionary<string, Tuple<Type, string>>()
        {
            { SERVICENAME_WORKFLOWCONFIG, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_WORKFLOWCONFIG) },
            { SERVICENAME_SELECTORCONFIG, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_SELECTORCONFIG) },
            { SERVICENAME_ACTION, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_ACTION) },
            { SERVICENAME_ACTIONSEQUENCE, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_ACTIONSEQUENCE) },
            { SERVICENAME_ACTIONPARAMETER, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_ACTIONPARAMETER) },
            { SERVICENAME_CONSTRAINT, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_CONSTRAINT) },
            { SERVICENAME_CONSTRAINTSEQUENCE, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_CONSTRAINTSEQUENCE) },
            { SERVICENAME_CONSTRAINTPARAMETER, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_CONSTRAINTPARAMETER) },
            { SERVICENAME_DATASET, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_DATASET) },
            { SERVICENAME_GRIDCONFIGURATION, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_GRIDCONFIGURATION) },
            { SERVICENAME_VALUEOBJECT, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_VALUEOBJECT) },
            { SERVICENAME_USER, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_USER) },
            { SERVICENAME_USERSET, Tuple.Create<Type, string>(typeof(ServiceCommonFactory), KEYSERVICE_USERSET) }
        };



        private string _baseAdress;
        private Dictionary<string, AbstractServiceFactory> _cacheServices;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="baseAdress"></param>
        public ServiceFacade(string baseAdress)
        {
            _baseAdress = baseAdress;
            _cacheServices = new Dictionary<string, AbstractServiceFactory>();
        }

        /// <summary>
        /// This function permits to format the action with base adress
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private string FormatUrl(string action)
        {
            return string.Format("{0}/{1}", _baseAdress, action);
        }


        public AbstractServiceFactory this[string serviceName]
        {
            get
            {
                if (_linkKeysNames.ContainsKey(serviceName))
                {
                    Type t = _linkKeysNames[serviceName].Item1;
                    string urlKey = _linkKeysNames[serviceName].Item2;
                    return this?.GetType()
                        ?.GetMethod("GetOrCreateService", BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.MakeGenericMethod(t)
                        ?.Invoke(this, new object[] { urlKey }) as AbstractServiceFactory;
                }

                //if (serviceName == SERVICENAME_WORKFLOWCONFIG)
                //    return Workflow;
                //if (serviceName == SERVICENAME_SELECTORCONFIG)
                //    return SelectorConfig;


                    return null;
            }
        }

        /// <summary>
        /// Workflow property.
        /// </summary>
        /// <value>
        /// Gets or sets the Workflow value
        /// </value>
        public ServiceCommonFactory Workflow { get { return GetOrCreateService<ServiceCommonFactory>(KEYSERVICE_WORKFLOWCONFIG); } }

        /// <summary>
        /// Selector property.
        /// </summary>
        /// <value>
        /// Gets or sets the Selector value
        /// </value>
        public ServiceCommonFactory Selector { get { return GetOrCreateService<ServiceCommonFactory>(KEYSERVICE_SELECTOR); } }

        /// <summary>
        /// SelectorConfig.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorConfig value
        /// </value>
        public ServiceCommonFactory SelectorConfig { get { return GetOrCreateService<ServiceCommonFactory>(KEYSERVICE_SELECTORCONFIG); } }


        ///// <summary>
        ///// ValueObject property.
        ///// </summary>
        ///// <value>
        ///// Gets or sets the ValueObject value
        ///// </value>
        //public ServiceCommonFactory ValueObject { get { return new ServiceCommonFactory(FormatUrl("ValueObject")); } }

        /// <summary>
        /// ValueObjectByFilter property.
        /// </summary>
        /// <value>
        /// Gets or sets the ValueObjectByFilter value
        /// </value>
        public ServiceCommonFactory ValueObjectByFilter { get { return GetOrCreateService<ServiceCommonFactory>(KEYSERVICE_VALUEOBJECT_FILTER); } }

        /// <summary>
        /// Comment property.
        /// </summary>
        /// <value>
        /// Gets or sets the Comment value
        /// </value>
        public ServiceCommonFactory Comment { get { return GetOrCreateService<ServiceCommonFactory>(KEYSERVICE_COMMENT); } }

        /// <summary>
        /// User property.
        /// </summary>
        /// <value>
        /// Gets or sets the User value
        /// </value>
        public ServiceCommonFactory User { get { return GetOrCreateService<ServiceCommonFactory>(KEYSERVICE_USER); } }

        /// <summary>
        /// UserLogin property.
        /// </summary>
        /// <value>
        /// Gets or sets the UserLogin value
        /// </value>
        public ServiceCommonFactory UserLogin { get { return GetOrCreateService<ServiceCommonFactory>(KEYSERVICE_USER_LOGIN); } }

        /// <summary>
        /// UserRole property.
        /// </summary>
        /// <value>
        /// Gets or sets the UserRole value
        /// </value>
        public ServiceCommonFactory UserRole { get { return GetOrCreateService<ServiceCommonFactory>(KEYSERVICE_USER_ROLE); } }

        /// <summary>
        /// UserSet property.
        /// </summary>
        /// <value>
        /// Gets or sets the UserSet value
        /// </value>
        public ServiceCommonFactory UserSet { get { return GetOrCreateService<ServiceCommonFactory>(KEYSERVICE_USERSET); } }

        /// <summary>
        /// DataSet property.
        /// </summary>
        /// <value>
        /// Gets or sets the DataSet value
        /// </value>
        public ServiceCommonFactory DataSet { get { return GetOrCreateService<ServiceCommonFactory>(KEYSERVICE_DATASET); } }

        /// <summary>
        /// Donne le service pour récupérer les configurations des grid.
        /// </summary>
        public GridConfigurationServiceFactory GridConfiguration { get { return GetOrCreateService<GridConfigurationServiceFactory>(KEYSERVICE_GRIDCONFIGURATION); } }

        /// <summary>
        /// Donne le servicce sur les ValueObject
        /// </summary>
        public ValueObjectServiceFactory ValueObject { get { return GetOrCreateService<ValueObjectServiceFactory>(KEYSERVICE_VALUEOBJECT); } }



        private T GetOrCreateService<T>(string urlKey) where T : AbstractServiceFactory, new()
        {
            if (string.IsNullOrWhiteSpace(urlKey))
                return null;

            if (!_cacheServices.ContainsKey(urlKey))
            {
                T service = new T();
                service.InitializeServiceUrl(FormatUrl(urlKey));
                _cacheServices.Add(urlKey, service);
            }

            return _cacheServices[urlKey] as T;
        }

    }
}
