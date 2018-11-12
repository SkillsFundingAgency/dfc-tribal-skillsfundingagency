using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace IMS.NCS.CourseSearchService.Common
{
    /// <summary>
    /// Provides Common utility methods.
    /// </summary>
    public static class Utilities
    {
        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_PROVIDER_DEFAULT = 0;

        // windows api calls for impersonation
        [DllImport("advapi32.dll")]
        private static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(IntPtr handle);
        
        
        /// <summary>
        /// Gets Database Connection string.
        /// </summary>
        /// <returns>Database connection string</returns>
        public static string GetDatabaseConnection()
        {
            //string setting = "Data Source=SCOTDEV2 ;User Id=LSC_SOA; Password=SCOTDEV2;";//ConfigurationManager.ConnectionStrings["PBDatabase"].ConnectionString;
            string databaseConnection = ConfigurationManager.ConnectionStrings[Constants.DATABASE_CONNECTION].ConnectionString;

            if (databaseConnection == null)
            {
                throw new ConfigurationErrorsException("Database connection '" + Constants.DATABASE_CONNECTION + "' was not found in application settings");
            }

            return databaseConnection;            
        }


        /// <summary>
        /// Converts an array of string values to a delimited string using the delimiter provided.
        /// </summary>
        /// <param name="list">Array of string values to convert.</param>
        /// <param name="delimiter">Delimiter.</param>
        /// <returns>
        /// A concatenated string of the values in the original string array delimited by the delimiter priovided.
        /// </returns>
        public static string ConvertToDelimitedString(string[] list, string delimiter)
        {
            StringBuilder sb = new StringBuilder();
            int i = 1;

            foreach (string s in list)
            {
                sb.Append(s);

                if (i < list.Length)
                {
                    sb.Append(delimiter);
                }

                i++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts an List of long values to a delimited string using the delimiter provided.
        /// </summary>
        /// <param name="list">List of long values to convert.</param>
        /// <param name="delimiter">Delimiter.</param>
        /// <returns>
        /// A concatenated string of the values in the original long List delimited by the delimiter priovided.
        /// </returns>
        public static string ConvertToDelimitedString(List<long> list, string delimiter)
        {
            StringBuilder sb = new StringBuilder();
            int i = 1;

            foreach (long l in list)
            {
                sb.Append(l);

                if (i < list.Count)
                {
                    sb.Append(delimiter);
                }

                i++;
            }

            return sb.ToString();
        }


        /// <summary>
        /// Converts a delimited string to a string array.
        /// </summary>
        /// <param name="listToConvert">Delimited string to convert.</param>
        /// <param name="delimiters">Array of delimiters.</param>
        /// <returns>Converted string array.</returns>
        public static string[] ConvertDelimitedStringToArray(string listToConvert, string[] delimiters)
        {
            return listToConvert.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }


        /// <summary>
        /// Impersonates a specific user
        /// </summary>
        /// <param name="userName">The name of the user to impersonate.</param>
        /// <param name="domain">The domain of the user to impersonate.</param>
        /// <param name="password">The password od the user oto impersonate.</param>
        /// <returns>The impersonated Windows Identity</returns>
        public static WindowsImpersonationContext impersonateValidUser(String userName, String domain, String password)
        {
            WindowsIdentity tempWindowsIdentity;
            WindowsImpersonationContext context = null;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        context = tempWindowsIdentity.Impersonate();
                        if (context != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return context;
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return context;
        }


        /// <summary>
        /// Undos the impersonation of the context passed in.
        /// </summary>
        /// <param name="context">The Windows Idenity of the impersonated user to undo.</param>
        public static void UndoImpersonation(WindowsImpersonationContext context)
        {
            context.Undo();
        }
    }
}
