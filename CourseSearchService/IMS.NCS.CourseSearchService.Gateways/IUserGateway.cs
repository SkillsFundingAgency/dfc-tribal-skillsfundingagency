using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.CourseSearchService.Gateways
{
    /// <summary>
    /// User Gateway interface.
    /// </summary>
    public interface IUserGateway
    {
        /// <summary>
        /// Retrieves user password for specified username.
        /// </summary>
        /// <param name="username">Username to retreive password for.</param>
        /// <returns>User's password.</returns>
        string GetUserPassword(string username);
    }
}
