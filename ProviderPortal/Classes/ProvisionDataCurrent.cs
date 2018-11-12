using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    public class ProvisionDataCurrent
    {
        public static Boolean ShowSecondRowOfHeader
        {
            get
            {
                ProviderPortalEntities db = new ProviderPortalEntities();
                UserContext.UserContextInfo context = UserContext.GetUserContext();

                if (!context.IsProvider())
                {
                    return false;
                }

                Provider provider = db.Providers.Find(context.ItemId);
                if (provider != null)
                {
                    return provider.Courses.Count() > 0;
                }

                return false;
            }
        }

        public static Boolean IsTASOnly
        {
            get
            {
                ProviderPortalEntities db = new ProviderPortalEntities();
                UserContext.UserContextInfo context = UserContext.GetUserContext();

                if (!context.IsProvider())
                {
                    return false;
                }

                Provider provider = db.Providers.Find(context.ItemId);
                if (provider != null)
                {
                    return provider.IsTASOnly ?? false;
                }

                return false;
            }
        }

        public static DateTime? GetProvisionUpdateDate()
        {
            return DateTime.UtcNow; // new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        public static Int32 GetCountOfCoursesOutOfDate(Int32? providerId = null)
        {
            return GetOutOfDateCourses(providerId).Rows.Count;
        }

        public static DataTable GetOutOfDateCourses(Int32? providerId = null)
        {
            ProviderPortalEntities db = new ProviderPortalEntities();
            UserContext.UserContextInfo context = UserContext.GetUserContext();
            DataTable dt = new DataTable();

            DbCommand cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec [dbo].[up_GetProviderCoursesOutOfDate] @ProviderId, @LongCourseMinDurationWeeks, @LongCourseMaxStartDateInPastDays";
            cmd.Parameters.Add(new SqlParameter("@ProviderId", providerId ?? context.ItemId));
            cmd.Parameters.Add(new SqlParameter("@LongCourseMinDurationWeeks", Constants.ConfigSettings.LongCourseMinDurationWeeks));
            cmd.Parameters.Add(new SqlParameter("@LongCourseMaxStartDateInPastDays", Constants.ConfigSettings.LongCourseMaxStartDateInPastDays));

            try
            {
                db.Database.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return dt;
        }

        public static Int32 GetCountOfCoursesWithExpiredLAR(Int32? providerId = null)
        {
            return GetCoursesWithExpiredLAR(providerId).Rows.Count;
        }

        public static DataTable GetCoursesWithExpiredLAR(Int32? providerId = null)
        {
            //If there are courses which are linked to a no-longer-valid (expired) LAR (learning aim reference) code, 
            //then the number of courses like this should be indicated.   
            ProviderPortalEntities db = new ProviderPortalEntities();
            UserContext.UserContextInfo context = UserContext.GetUserContext();
            DataTable dt = new DataTable();

            DbCommand cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec [dbo].[up_GetProviderCoursesWithExpiredLAR] @ProviderId";
            cmd.Parameters.Add(new SqlParameter("@ProviderId", providerId ?? context.ItemId));

            try
            {
                db.Database.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return dt;
        }

        public static DateTime? GetLatestDate(DateTime? date1, DateTime? date2)
        {
            if (date1 == null)
            {
                return date2;
            }

            if (date2 == null)
            {
                return date1;
            }

            return date1 > date2 ? date1 : date2;
        }

    }
}