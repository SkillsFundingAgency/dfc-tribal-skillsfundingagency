using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IMS.NCS.CourseSearchService.Queries;

namespace IMS.NCS.CourseSearchService.BusinessServices
{
    /// <summary>
    /// Service Implementation providing all user functions.
    /// </summary>
    public class UserService : IUserService
    {
        #region Variables

        private IUserQuery _userQuery;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Handle for UserQuery.
        /// </summary>
        public IUserQuery UserQuery
        {
            get
            {
                return _userQuery;
            }
            set
            {
                _userQuery = value;
            }
        }

        #endregion Properties

        /// <summary>
        /// Retrieves password for username provided.
        /// </summary>
        /// <param name="username">User name to retrieve password for.</param>
        /// <returns>Password for user name.</returns>
        public string GetPassword(string username)
        {
            UserQuery = new UserQuery();
            return UserQuery.GetPassword(username);
        }

        /// <summary>
        /// Validates username and password provided.
        /// </summary>
        /// <param name="password">Password to validate.</param>
        /// <param name="username">Username to validate.</param>
        /// <param name="createdDate">Date created.</param>
        /// <returns>True if username / password are valid.</returns>
        public bool ValidateUser(string password, string username, string createdDate)
        {
            UserQuery = new UserQuery();
            return UserQuery.ValidateUser(password, username, createdDate);
        }
    }
}
