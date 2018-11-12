using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web;
using Microsoft.SqlServer.Server;

// ReSharper disable once CheckNamespace
public partial class UserDefinedFunctions
{
    /// <summary>
    /// The apprenticeship QA bands.
    /// </summary>
    private static readonly SortedDictionary<Int32, Int32> ApprenticeshipQaBands = new SortedDictionary<Int32, Int32>();

    private static String ApprenticeshipQaBandsConfiguration { get; set; }

    /// <summary>
    /// Gets the number of apprenticeships to QA.
    /// </summary>
    /// <param name="numberOfApprenticeships">The number of apprenticeships.</param>
    /// <returns>The number required to QA</returns>
    [SqlFunction(
        IsDeterministic = true,
        DataAccess = DataAccessKind.None,
        IsPrecise = true)]
    public static Int32 GetNumberOfApprenticeshipsToQa(Int32 numberOfApprenticeships, string qaBands)
    {
        if (numberOfApprenticeships == 0)
        {
            return 0;
        }

        // We pass the QA bands in each time as getting it ourselves from the DB
        // would significantly slow this function down
        if (ApprenticeshipQaBands.Count == 0 || ApprenticeshipQaBandsConfiguration !=  qaBands)
        {
            ApprenticeshipQaBandsConfiguration = qaBands;
            foreach (String setting in ApprenticeshipQaBandsConfiguration.Split(','))
            {
                try
                {
                    String[] s = setting.Split('~');
                    ApprenticeshipQaBands.Add(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]));
                }
                catch { }
            }
        }

        Int32 retValue = 0;
        foreach (KeyValuePair<Int32, Int32> kvp in ApprenticeshipQaBands)
        {
            if (kvp.Key >= numberOfApprenticeships)
            {
                retValue = kvp.Value;
                break;
            }
        }
        if (retValue > numberOfApprenticeships)
        {
            retValue = numberOfApprenticeships;
        }

        return retValue;
    }
}
