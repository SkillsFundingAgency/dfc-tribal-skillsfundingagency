// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Classes inheriting from this interface can handle events when the session state changes.
    /// </summary>
    interface ISession
    {
        /// <summary>
        /// Called when a new session is created.
        /// </summary>
        void Session_Start();

        /// <summary>
        /// Called when a session expires.
        /// </summary>
        void Session_End();
    }
}
