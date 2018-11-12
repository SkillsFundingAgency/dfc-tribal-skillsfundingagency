using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IMS.NCS.CourseSearchService.Common;
using IMS.NCS.CourseSearchService.Exceptions;
using IMS.NCS.CourseSearchService.Gateways;

namespace IMS.NCS.CourseSearchService.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public class UserQuery : IUserQuery
    {
        #region Variables

        private IUserGateway _userGateway;

        #endregion Variables

        #region Properties

        public IUserGateway UserGateway
        {
            get
            {
                return _userGateway;
            }
            set
            {
                _userGateway = value;
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
            UserGateway = new UserGateway();
            return UserGateway.GetUserPassword(username);
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
            bool isUserValid = true;
            UserGateway = new UserGateway();
            
            string clearPassword = UserGateway.GetUserPassword(username);
            string digestPassword = Utilities.GetDigestedPassword(clearPassword, createdDate);

            if (!digestPassword.Equals(password))
            {
                throw new InvalidUserException("Invalid digested password");
            }

            return isUserValid;
        }
    }
}
