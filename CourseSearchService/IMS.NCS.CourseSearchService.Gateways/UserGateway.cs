using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;

using IMS.NCS.CourseSearchService.Common;
using IMS.NCS.CourseSearchService.Exceptions;

namespace IMS.NCS.CourseSearchService.Gateways
{
    /// <summary>
    /// UserGateway implementation, manages all User type calls to the database.
    /// </summary>
    public class UserGateway : IUserGateway
    {
        /// <summary>
        /// Retrieves user password for specified username.
        /// </summary>
        /// <param name="username">Username to retreive password for.</param>
        /// <returns>User's password.</returns>
        public string GetUserPassword(string username)
        {
            string commandText = Constants.USER_DETAILS_SP;
            DataSet userDataSet = new DataSet();

            OracleParameter[] parameters = new OracleParameter[4];
            parameters[0] = new OracleParameter("PV_USER_NAME", OracleDbType.Varchar2, username, ParameterDirection.Input);
            parameters[1] = new OracleParameter("USER_PASSWORD", OracleDbType.Varchar2, ParameterDirection.Output);
            parameters[2] = new OracleParameter("p_status", OracleDbType.Decimal);
            parameters[2].Direction = ParameterDirection.Output;
            parameters[2].Value = 0;
            parameters[3] = new OracleParameter("p_err_msg", OracleDbType.Varchar2, 1500);
            parameters[3].Direction = ParameterDirection.Output;
            parameters[3].Value = DBNull.Value;

            userDataSet = OracleHelper.ExecuteDataset(Utilities.GetDatabaseConnection(), CommandType.StoredProcedure, commandText, parameters);

            // Check no errors in response
            if (Convert.ToString(parameters[2].Value) != "0")
            {
                // TODO: do we want to throw an exception here or do we want to do something else?
                throw new Exception(Convert.ToString(parameters[3].Value));
            }

            if (parameters[1].Value == null)
            {
                // TODO: this was an InvalidUserException in the java code - do we want to replicate this?
                throw new InvalidUserException("Invalid username");
            }

            return Convert.ToString(parameters[1].Value);
        }
    }
}
