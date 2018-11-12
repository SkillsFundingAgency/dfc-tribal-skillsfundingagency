using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using Tribal.SkillsFundingAgency.ProviderPortal.Controllers;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class OpportunityModelExtensions
    {
        /// <summary>
        /// Convert an <see cref="AddEditOpportunityModel"/> to an <see cref="CourseInstance"/>.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <returns>
        /// The <see cref="CourseInstance"/>.
        /// </returns>        
        public static CourseInstance ToEntity(this AddEditOpportunityModel model, ProviderPortalEntities db)
        {
            CourseInstance courseInstance;

            if (model.OpportunityId == null)
            {
                courseInstance = new CourseInstance                
                {
                    CourseId = model.CourseId,
                    CreatedByUserId = Permission.GetCurrentUserId(),
                    CreatedDateTimeUtc = DateTime.UtcNow
                };
            }
            else
            {
                courseInstance = db.CourseInstances.Find(model.OpportunityId);
                if (courseInstance == null)
                {
                    return null;
                }
            }

            courseInstance.DisplayedByOrganisationId = model.DisplayId;
            courseInstance.OfferedByOrganisationId = model.OfferedById;
            courseInstance.BothOfferedByDisplayBySearched = model.DisplayId != model.OfferedById && model.BothOfferedByDisplayBySearched;

            courseInstance.ProviderOwnCourseInstanceRef = model.ProviderOwnOpportunityRef;
            courseInstance.StudyModeId = model.StudyModeId;
            courseInstance.AttendanceTypeId = model.AttendanceModeId;
            courseInstance.AttendancePatternId = model.AttendancePatternId;
            courseInstance.DurationUnit = model.Duration;
            courseInstance.DurationUnitId = model.DurationUnitId;
            courseInstance.DurationAsText = model.DurationDescription;
            courseInstance.StartDateDescription = model.StartDateDescription;
            courseInstance.EndDate = model.EndDate;
            courseInstance.TimeTable = model.Timetable;
            courseInstance.Price = model.Price;
            courseInstance.PriceAsText = model.PriceDescription;
            courseInstance.LanguageOfInstruction = model.LanguageOfInstruction;
            courseInstance.LanguageOfAssessment = model.LanguageOfAssessment;
            courseInstance.ApplyFromDate = model.ApplyFrom;
            courseInstance.ApplyUntilDate = model.ApplyUntil;
            courseInstance.ApplyUntilText = model.ApplyUntilDescription;
            courseInstance.EnquiryTo = model.EnquireTo;
            courseInstance.ApplyTo = model.ApplyTo;
            courseInstance.Url = UrlHelper.GetFullUrl(model.Url);
            courseInstance.CanApplyAllYear = model.AcceptedThroughoutYear;
            courseInstance.VenueLocationId = model.RegionId;

            return courseInstance;
        }

        /// <summary>
        /// Gets the opportunity details
        /// </summary>
        /// <param name="courseInstance">The <see cref="courseInstance"/> object</param>
        /// <returns>The opportunity details in 1 line separated by pipes</returns>
        public static String GetOpportunityDetails(this CourseInstance courseInstance, bool includePrice = true, bool includeStartDates = true)
        {
            String details = courseInstance.DurationUnit.HasValue ?
                             courseInstance.DurationUnit + " " + (courseInstance.DurationUnit1!=null? courseInstance.DurationUnit1.DurationUnitName: string.Empty) : 
                             courseInstance.DurationAsText;

            details += " | " +
                       (courseInstance.StudyModeId.HasValue ? courseInstance.StudyMode.StudyModeName : String.Empty) +
                       " | ";

            if (includePrice)
            {
                details += courseInstance.Price.HasValue ? "£" + courseInstance.Price.Value.ToString("N0") : courseInstance.PriceAsText;
            }

            if (includeStartDates)
            {
                String startDates = "";
                foreach (CourseInstanceStartDate sd in courseInstance.CourseInstanceStartDates)
                {
                    if (startDates != "")
                    {
                        startDates += ",";
                    }
                    startDates += sd.ToFormattedString();
                }

                details += " | " + startDates + " | ";
            }

            if (courseInstance.VenueLocation != null)
            {
                details += courseInstance.VenueLocation.LocationName + (courseInstance.VenueLocation.ParentVenueLocation != null ? " (" + courseInstance.VenueLocation.ParentVenueLocation.LocationName + ")" : "");
            }
            else if (courseInstance.Venues != null)
            {
                String venues = "";
                foreach (Venue venue in courseInstance.Venues)
                {
                    if (venues != "")
                    {
                        venues += ",";
                    }
                    venues += venue.VenueName;
                }
                details += venues;
            }

            return details;
        }

        public static List<String> GetWarningMessages(this AddEditOpportunityModel model)
        {
            List<String> messages = new List<String>();

            if (!String.IsNullOrWhiteSpace(model.Url) && !UrlHelper.UrlIsReachable(model.Url))
            {
                messages.Add(String.Format(AppGlobal.Language.GetText("AddEditOpportunityModel_Edit_UrlNotReachable", "The web address for {0} returns a response that suggests this page may not exist. Please check that the web address entered is correct."), AppGlobal.Language.GetText("AddEditOpportunityModel_DisplayName_Url", "URL")));
            }

            return messages;
        }
    }


    public static class OpportunityDateStatusModelExtensions
    {
        public static OpportunityDateStatusModel Populate(this OpportunityDateStatusModel model, ProviderPortalEntities db)
        {
            DbCommand cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec [dbo].[up_GetProviderOpportunitiesDateStatus] @ProviderId";
            cmd.Parameters.Add(new SqlParameter("ProviderId", model.ProviderId));
            cmd.CommandTimeout = 120;
            try
            {
                db.Database.Connection.Open();
                DbDataReader reader = cmd.ExecuteReader();
                model.Items = ((IObjectContextAdapter)db)
                    .ObjectContext
                    .Translate<OpportunityDateStatusModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }
    }

}