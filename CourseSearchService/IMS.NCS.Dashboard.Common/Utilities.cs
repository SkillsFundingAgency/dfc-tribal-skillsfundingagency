using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

/// Mark the assembly as CLS Compliant
[assembly: CLSCompliant(true)]

namespace IMS.NCS.Dashboard.Common
{
    /// <summary>
    /// Set of utility methods for use within the IMS.NCS.CourseDataFileRetriever solution.
    /// </summary>
    [CLSCompliant(true)]
    public static class Utilities
    {

        /// <summary>
        /// Gets the database connection string from the config file.
        /// </summary>
        /// <returns>The database connection string.</returns>
        public static string GetDatabaseConnection()
        {
            string databaseConnection = ConfigurationManager.ConnectionStrings[Constants.Database.NCSImportControlDatabase].ConnectionString;

            if (databaseConnection == null)
            {
                throw new ConfigurationErrorsException("Database connection '" + Constants.Database.NCSImportControlDatabase + "' was not found in application settings");
            }

            return databaseConnection;
        }
    }
}
