using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace IMS.NCS.CourseSearchService.Common.Unity
{
    /// <summary>
    /// Dependency injection instance provider (using Unity application block). 
    /// This kind of behavior controls the lifecycle of a WCF service instance, so it is the best place
    /// to inject the service dependencies.
    /// </summary>
    public class UnityInstanceProvider : IInstanceProvider
    {
        #region Member Variables
        private IUnityContainer _container = null;
        private string _containerName;
        private Type _contractType;
		private string _lockingKey = "LockingKey";
        #endregion Member Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityInstanceProvider"/> class.
        /// </summary>
        /// <param name="contractType">The service contract type.</param>
        /// <param name="containerName">Name of the unity container.</param>
        public UnityInstanceProvider(Type contractType, string containerName) 
        {
            _contractType = contractType;
            _containerName = containerName;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets a fresh service instance
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <returns>A user-defined service object.</returns>
        public object GetInstance(InstanceContext instanceContext) 
        { 
            return GetInstance(instanceContext, null); 
        }

        /// <summary>
        /// Gets a fresh service instance
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <param name="message">The message that triggered the creation of a service object.</param>
        /// <returns>The service object.</returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
			lock (_lockingKey)
			{
				if (_container == null)
				{
					// Instantiate our Unity Container
					_container = new UnityContainer();
					_container.LoadConfiguration(_containerName);
					//// Pull the "unity" section from config file
					//UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
					//// If configuration information is available apply it
					//if (section != null)
					//    section.Configure(_container, _containerName);
				}
			}
            return _container.Resolve(_contractType);
        }

        //public object GetInstance(InstanceContext instanceContext, Message message)
        //{
        //    if (_container == null)
        //    {
        //        // Instantiate our Unity Container
        //        _container = new UnityContainer();

        //        // Pull the "unity" section from config file
        //        UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");

        //        // If configuration information is available apply it
        //        if (section != null)
        //            section.Containers[_containerName].Configure(_container);
        //    }
        //    return _container.Resolve(_contractType);
        //}
        /// <summary>
        /// Releases the specified service instance
        /// </summary>
        /// <param name="instanceContext">The service's instance context.</param>
        /// <param name="instance">The service object to be recycled.</param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance) 
        {
            IDisposable disposable = instance as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }
        #endregion Methods
    }
}