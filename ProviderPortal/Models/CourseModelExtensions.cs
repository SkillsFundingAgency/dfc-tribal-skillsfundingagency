using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class CourseModelExtensions
    {
        /// <summary>
        /// Convert an <see cref="AddEditCourseModel"/> to an <see cref="Course"/>.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <returns>
        /// The <see cref="Course"/>.
        /// </returns>        
        public static Course ToEntity(this AddEditCourseModel model, ProviderPortalEntities db)
        {
            Course course;

            if (model.CourseId == null)
            {
                course = new Course
                {
                    CreatedByUserId = Permission.GetCurrentUserId(),
                    CreatedDateTimeUtc = DateTime.UtcNow
                };
            }
            else
            {
                course = db.Courses.Find(model.CourseId);
                if (course == null)
                {
                    return null;
                }
            }

            // Get the Learning Aim if one has been specified
            LearningAim la = null;
            if (!String.IsNullOrWhiteSpace(model.LearningAimId))
            {
                la = db.LearningAims.Find(model.LearningAimId);
            }

            course.LearningAimRefId = model.LearningAimId;
            course.WhenNoLarQualificationTypeId = la == null || la.QualificationTypeId == null ? model.QualificationTypeId : null;
            course.WhenNoLarQualificationTitle = la == null ? model.WhenNoLarQualificationTitle : null;
            course.CourseTitle = model.CourseTitle;
            course.ProviderOwnCourseRef = model.ProviderOwnCourseRef;
            course.CourseSummary = model.CourseSummary;
            course.EntryRequirements = model.EntryRequirements;
            course.QualificationLevelId = model.QualificationLevelId;
            course.Url = UrlHelper.GetFullUrl(model.Url);
            course.BookingUrl = UrlHelper.GetFullUrl(model.BookingUrl);
            course.AssessmentMethod = model.AssessmentMethod;
            course.EquipmentRequired = model.EquipmentRequired;
            course.QualificationLevelId = model.QualificationLevelId;
            course.AwardingOrganisationName = model.AwardingOrganisation;
            course.UcasTariffPoints = model.UcasTariffPoints;

            return course;
        }

        public static List<String> GetWarningMessages(this AddEditCourseModel model)
        {
            List<String> messages = new List<String>();

            if (!String.IsNullOrWhiteSpace(model.Url) && !UrlHelper.UrlIsReachable(model.Url))
            {
                messages.Add(String.Format(AppGlobal.Language.GetText("AddEditCourseModel_Edit_UrlNotReachable", "The web address for {0} returns a response that suggests this page may not exist. Please check that the web address entered is correct."), AppGlobal.Language.GetText("AddEditCourseModel_DisplayName_Url", "URL")));
            }
            if (!String.IsNullOrWhiteSpace(model.BookingUrl) && !UrlHelper.UrlIsReachable(model.BookingUrl))
            {
                messages.Add(String.Format(AppGlobal.Language.GetText("AddEditCourseModel_Edit_UrlNotReachable", "The web address for {0} returns a response that suggests this page may not exist. Please check that the web address entered is correct."), AppGlobal.Language.GetText("AddEditCourseModel_DisplayName_BookingUrl", "Booking URL")));
            }

            return messages;
        }
    }

    public static class OutOfDateCourseModelExtensions
    {
        public static OutOfDateCourseModel Populate(this OutOfDateCourseModel model, ProviderPortalEntities db)
        {
            DbCommand cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec [dbo].[up_GetProviderCoursesOutOfDate] @ProviderId, @LongCourseMinDurationWeeks, @LongCourseMaxStartDateInPastDays";
            cmd.Parameters.Add(new SqlParameter("ProviderId", model.ProviderId));
            cmd.Parameters.Add(new SqlParameter("@LongCourseMinDurationWeeks", Constants.ConfigSettings.LongCourseMinDurationWeeks));
            cmd.Parameters.Add(new SqlParameter("@LongCourseMaxStartDateInPastDays", Constants.ConfigSettings.LongCourseMaxStartDateInPastDays));
            cmd.CommandTimeout = 120;
            try
            {
                db.Database.Connection.Open();
                DbDataReader reader = cmd.ExecuteReader();
                model.Items = ((IObjectContextAdapter)db)
                    .ObjectContext
                    .Translate<OutOfDateCourseModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }
    }


    public static class CourseDateStatusModelExtensions
    {
        public static CourseDateStatusModel Populate(this CourseDateStatusModel model, ProviderPortalEntities db)
        {
            DbCommand cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec [dbo].[up_GetProviderCoursesDateStatus] @ProviderId";
            cmd.Parameters.Add(new SqlParameter("ProviderId", model.ProviderId));
            cmd.CommandTimeout = 120;
            try
            {
                db.Database.Connection.Open();
                DbDataReader reader = cmd.ExecuteReader();
                model.Items = ((IObjectContextAdapter)db)
                    .ObjectContext
                    .Translate<CourseDateStatusModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }
    }


}