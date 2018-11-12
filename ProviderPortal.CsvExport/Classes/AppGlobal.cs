// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppGlobal.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Defines the AppGlobal type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace


namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    /// <summary>
    /// Objects that are global to the site
    /// </summary>
    public class AppGlobal
    {
        /// <summary>
        /// Initializes static members of the <see cref="AppGlobal"/> class.
        /// </summary>
        static AppGlobal()
        {
           
            EmailQueue = null;
        }

        /// <summary>
        /// Gets or sets the email queue to add emails for immediate sending
        /// </summary>
        public static WebEmailSendQueue EmailQueue { get; set; }


        /// <summary>
        /// Gets or sets the application version
        /// </summary>
        public static string Version { get; set; }
 
      
    }
}