using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.CourseSearchService.Queries
{
    /// <summary>
    /// Interface providing User Query functions.
    /// </summary>
    public interface IUserQuery
    {
        /// <summary>
        /// Retrieves password for username provided.
        /// </summary>
        /// <param name="username">User name to retrieve password for.</param>
        /// <returns>Password for user name.</returns>
        string GetPassword(string username);

        /// <summary>
        /// Validates username and password provided.
        /// </summary>
        /// <param name="password">Password to validate.</param>
        /// <param name="username">Username to validate.</param>
        /// <param name="createdDate">Date created.</param>
        /// <returns>True if username / password are valid.</returns>
        bool ValidateUser(string password, string username, string createdDate);
    }
}
