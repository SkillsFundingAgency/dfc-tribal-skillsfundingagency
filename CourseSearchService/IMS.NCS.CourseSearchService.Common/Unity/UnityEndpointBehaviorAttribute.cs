using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace IMS.NCS.CourseSearchService.Common.Unity
{
    /// <summary>
    /// Dependency injection service behaviour (using Unity application block).
    /// This behaviour is only used to hook up the instance provider in the WCF dispatcher at runtime.
    /// </summary>
    /// <remarks>
    /// This code has mostly been lifted from an article at http://weblogs.asp.net/cibrax/archive/2007/12/13/wcf-dependency-injection-behavior.aspx
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UnityEndpointBehaviorAttribute : Attribute, IEndpointBehavior
    {
        #region Member Variables
        string _containerName = string.Empty;

        #endregion Member Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityEndpointBehaviorAttribute"/> class.
        /// </summary>
        public UnityEndpointBehaviorAttribute()
        : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityEndpointBehaviorAttribute"/> class.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        public UnityEndpointBehaviorAttribute(string containerName)
        : base()
        {
            _containerName = containerName;
        }

        #endregion Constructors

        #region IEndpointBehavior Members

        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="endpoint">The endpoint to modify.</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that is to be customized.</param>
        /// <param name="clientRuntime">The client runtime to be customized.</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the service across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that exposes the contract.</param>
        /// <param name="endpointDispatcher">The endpoint dispatcher to be modified or extended.</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            Type contractType = endpoint.Contract.ContractType;
            endpointDispatcher.DispatchRuntime.InstanceProvider = new UnityInstanceProvider(contractType, _containerName);
        }

        /// <summary>
        /// Implement to confirm that the endpoint meets some intended criteria.
        /// </summary>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }
        #endregion IEndpointBehavior Members
    }
}