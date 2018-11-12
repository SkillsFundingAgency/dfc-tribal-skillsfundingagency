using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace IMS.NCS.CourseSearchService.Common.Unity
{
    /// <summary>
    /// Configuration section for the UnityEndpointBehavior. 
    /// </summary>
    public class UnityEndpointBehaviorSection : BehaviorExtensionElement
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityEndpointBehaviorSection"/> class.
        /// </summary>
        public UnityEndpointBehaviorSection()
        : base()
        {
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets the name of the container.
        /// </summary>
        /// <value>The name of the container.</value>
        [ConfigurationProperty("containerName")]
        public string ContainerName
        {
            get
            {
                return (string)base["containerName"];
            }
        }

        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/>.</returns>
        public override Type BehaviorType
        {
            get
            {
                return typeof(UnityEndpointBehaviorAttribute);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>The behavior extension.</returns>
        protected override object CreateBehavior()
        {
            return new UnityEndpointBehaviorAttribute(this.ContainerName);
        }
        #endregion Methods
    }
}