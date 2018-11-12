using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.DataWarehouse.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class ProviderCourseReportViewModelExtensions
    {
        public static ProviderCourseReportViewModel Populate(this ProviderCourseReportViewModel model,
            ProviderPortalEntities db)
        {
            model.Items = db.up_ReportProviderCourses(model.ProviderId)
                .Select(x => new ProviderCourseReportViewModelItem
                {
                    RecordStatusName = x.RecordStatusName,
                    ProviderOwnCourseRef = x.ProviderOwnCourseRef,
                    CourseTitle = x.CourseTitle,
                    QualificationTypeName = x.QualificationTypeName,
                    QualificationTitle = x.QualificationTitle,
                    AwardOrgName = x.AwardOrgName
                })
                .OrderBy(x => x.ProviderOwnCourseRef)
                .ToList();
            return model;
        }
    }

    public static class ProviderOpportunityReportViewModelExtensions
    {
        public static ProviderOpportunityReportViewModel Populate(this ProviderOpportunityReportViewModel model,
            ProviderPortalEntities db)
        {
            model.Items = db.up_ReportProviderOpportunities(model.ProviderId)
                .Select(x => new ProviderOpportunityReportViewModelItem
                {
                    RecordStatusName = x.RecordStatusName,
                    ProviderOwnCourseInstanceRef = x.ProviderOwnCourseInstanceRef,
                    StudyModeName = x.StudyModeName,
                    AttendanceTypeName = x.AttendanceTypeName,
                    AttendancePatternName = x.AttendancePatternName,
                    DurationUnit = x.DurationUnit,
                    DurationUnitName = x.DurationUnitName,
                    DurationAsText = x.DurationAsText,
                    StartDates = x.StartDates,
                    StartDateDescription = x.StartDateDescription,
                    EndDate = x.EndDate,
                    Price = x.Price,
                    PriceAsText = x.Price.ToString(),
                    Region = x.Region,
                    Venues = x.Venues
                })
                .OrderBy(x => x.ProviderOwnCourseInstanceRef)
                .ToList();
            return model;
        }
    }

    public static class ProviderVenueReportViewModelExtensions
    {
        public static ProviderVenueReportViewModel Populate(this ProviderVenueReportViewModel model,
            ProviderPortalEntities db)
        {
            model.Items = db.up_ReportProviderVenues(model.ProviderId)
                .Select(x => new ProviderVenueReportViewModelItem
                {
                    RecordStatusName = x.RecordStatusName,
                    ProviderOwnVenueRef = x.ProviderOwnVenueRef,
                    Address = new AddressViewModel
                    {
                        AddressLine1 = x.AddressLine1,
                        AddressLine2 = x.AddressLine2,
                        Town = x.Town,
                        County = x.County,
                        Postcode = x.Postcode
                    }
                })
                .OrderBy(x => x.ProviderOwnVenueRef)
                .ToList();
            return model;
        }
    }

    public static class ProviderDashboardReportViewModelExtensions
    {
        public static ProviderDashboardReportViewModel Populate(this ProviderDashboardReportViewModel model,
            ProviderPortalEntities db)
        {
            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportProviderDashboard] @ProviderId";
            cmd.Parameters.Add(new SqlParameter("ProviderId", model.ProviderId));
            cmd.CommandTimeout = 120;
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                model = ((IObjectContextAdapter) db)
                    .ObjectContext
                    .Translate<ProviderDashboardReportViewModel>(reader).First();

                model.ParentOrganisations = ReadProviderOrganisationsObject(reader);
                model.OpportunitiesChart = ReadChartObject(reader);
                model.CoursesChart = ReadChartObject(reader);
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }


        public static string EncodeChart(List<ProviderDashboardChartViewModel> items)
        {
            List<string> components = new List<string>();
            components.Add("['Course','Count',{ role:'style'},'Filter']");
            foreach (var item in items)
            {
                components.Add(string.Format("['{0}',{1},'{2}','{3}']", item.CourseStatusName, item.CourseCount, item.BarColour, item.Link));
            }
            string chartData =  string.Format("[{0}]", string.Join(",", components));
            return chartData;
        }

        private static List<ProviderDashboardChartViewModel> ReadChartObject(DbDataReader reader)
        {
            var dataList = new List<ProviderDashboardChartViewModel>();
            reader.NextResult();
            while (reader.Read())
            {
                var data = new ProviderDashboardChartViewModel();
                data.CourseStatusName = reader.IsDBNull(0) ? String.Empty : reader.GetString(0);
                data.CourseCount = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                data.BarColour = reader.IsDBNull(0) ? String.Empty : reader.GetString(2);
                data.Link = reader.IsDBNull(0) ? String.Empty : reader.GetString(3);
                dataList.Add(data);
            }
            return dataList;
        }


        private static List<ProviderDashboardOrganisationViewModel> ReadProviderOrganisationsObject(DbDataReader reader)
        {
            var dataList = new List<ProviderDashboardOrganisationViewModel>();
            reader.NextResult();
            while (reader.Read())
            {
                var data = new ProviderDashboardOrganisationViewModel();
                data.OrganisationId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                data.OrganisationName = reader.IsDBNull(1) ? String.Empty : reader.GetString(1);
                dataList.Add(data);
            }
            return dataList;
        }


    }

    public static class ProviderQAHistoryReportViewModelExtensions
    {
        /// <summary>
        /// Populates view model for a specified provider.
        /// </summary>
        /// <param name="providerId">The provider.</param>
        /// <returns></returns>
        public static ProviderQAHistoryReportViewModel Populate(this ProviderQAHistoryReportViewModel model,
            ProviderPortalEntities db, int providerId)
        {
            model.Items = GetDataInternal(db, false, providerId);
            return model;
        }

        /// <summary>
        /// Populates view model for all providers.
        /// </summary>
        /// <returns></returns>
        public static ProviderQAHistoryReportViewModel Populate(this ProviderQAHistoryReportViewModel model,
            ProviderPortalEntities db)
        {
            model.Items = GetDataInternal(db, true, -1);
            return model;
        }

        private static List<ProviderQAHistoryReportViewModelItem> GetDataInternal(ProviderPortalEntities db,
            bool showAllProviders, int providerId)
        {
            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportProviderQAHistory] @ShowAllProviders, @ProviderId";
            cmd.Parameters.Add(new SqlParameter("ShowAllProviders", showAllProviders));
            cmd.Parameters.Add(new SqlParameter("ProviderId", providerId));

            var list = new List<ProviderQAHistoryReportViewModelItem>();
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                list = ((IObjectContextAdapter) db)
                    .ObjectContext
                    .Translate<ProviderQAHistoryReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return list;
        }
    }

    public static class OrganisationTrafficLightReportViewModelExtensions
    {
        public static OrganisationTrafficLightReportViewModel Populate(
            this OrganisationTrafficLightReportViewModel model, ProviderPortalEntities db)
        {
            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportOrganisationTrafficLight] @OrganisationId";
            cmd.Parameters.Add(new SqlParameter("OrganisationId", model.OrganisationId));

            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                var items = ((IObjectContextAdapter) db)
                    .ObjectContext
                    .Translate<up_ReportOrganisationTrafficLight_Result>(reader).ToList();

                // Read the contacts
                var contacts = new Dictionary<int, List<MailAddressPhoneNumber>>();
                reader.NextResult();
                while (reader.Read())
                {
                    var providerId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    var name = reader.IsDBNull(1) ? String.Empty : reader.GetString(1);
                    var email = reader.IsDBNull(2) ? String.Empty : reader.GetString(2);
                    var phoneNumber = reader.IsDBNull(3) ? String.Empty : reader.GetString(3);
                    if (!contacts.ContainsKey(providerId))
                        contacts[providerId] = new List<MailAddressPhoneNumber>();
                    contacts[providerId].Add(new MailAddressPhoneNumber(email, name, phoneNumber));
                }

                // Create the view model
                model.Items = items.Select(x => new OrganisationTrafficLightReportViewModelItem
                {
                    ProviderId = x.ProviderId,
                    Ukprn = x.Ukprn,
                    ProviderTypeName = x.ProviderTypeName,
                    ProviderName = x.ProviderName,
                    ProviderNameAlias = x.ProviderNameAlias,
                    UkrlpName = x.UkrlpName,
                    ModifiedDateTimeUtc = x.ModifiedDateTimeUtc,
                    LastUpdateMethod = x.ApplicationName,
                    SFAFunded = x.SFAFunded,
                    DFE1619Funded = x.DFE1619Funded,
                    PrimaryContacts =
                        contacts.ContainsKey(x.ProviderId) ? contacts[x.ProviderId] : new List<MailAddressPhoneNumber>()
                }).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }
    }

    public static class AdminReportMasterViewModelExtensions
    {
        public static AdminReportMasterViewModel Populate(this AdminReportMasterViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<AdminReportMasterViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText =
                "exec dbo.[up_ReportAdminReport] @IncludeProviders, @IncludeOrganisations, @ContractingBodiesOnly, @SFAFunded, @DFEFunded";
            cmd.Parameters.Add(new SqlParameter("IncludeProviders", model.IncludeProviders));
            cmd.Parameters.Add(new SqlParameter("IncludeOrganisations", model.IncludeOrganisations));
            cmd.Parameters.Add(new SqlParameter("ContractingBodiesOnly", model.ContractingBodiesOnly));
            cmd.Parameters.Add(new SqlParameter("SFAFunded", model.SFAFunded));
            cmd.Parameters.Add(new SqlParameter("DFEFunded", model.DFEFunded));

            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                model.Items = ((IObjectContextAdapter) db)
                    .ObjectContext
                    .Translate<AdminReportMasterViewModelItem>(reader).ToList();

                // Read the contacts
                var contacts = new Dictionary<string, List<MailAddressPhoneNumber>>();
                reader.NextResult();
                while (reader.Read())
                {
                    var key = reader.IsDBNull(0) ? "O" + reader.GetInt32(1) : "P" + reader.GetInt32(0);
                    var name = reader.IsDBNull(2) ? String.Empty : reader.GetString(2);
                    var email = reader.IsDBNull(3) ? String.Empty : reader.GetString(3);
                    var phoneNumber = reader.IsDBNull(4) ? String.Empty : reader.GetString(4);
                    if (!contacts.ContainsKey(key))
                        contacts[key] = new List<MailAddressPhoneNumber>();
                    contacts[key].Add(new MailAddressPhoneNumber(email, name, phoneNumber));
                }

                foreach (var item in model.Items)
                {
                    var key = String.Format("{0}{1}", item.IsProvider ? "P" : "O", item.ProviderId);
                    item.PrimaryContacts = contacts.ContainsKey(key)
                        ? contacts[key]
                        : new List<MailAddressPhoneNumber>();
                }
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this AdminReportMasterViewModel model)
        {
            var provider = AppGlobal.Language.GetText("Report_AdminReports_Provider", "Provider");
            var organisation = AppGlobal.Language.GetText("Report_AdminReports_Organisation", "Organisation");
            var yes = AppGlobal.Language.GetText("Report_AdminReports_IsContractingBody", "Yes");
            var no = AppGlobal.Language.GetText("Report_AdminReports_IsNotContractingBody", "No");
            var na = AppGlobal.Language.GetText("Report_AdminReports_NotApplicable", "N/A");
            var portal = AppGlobal.Language.GetText("Report_AdminReports_PortalApplication", "Portal");

            string[][] data = null;
            if (model.ContractingBodiesOnly && model.IncludeProviders && model.IncludeOrganisations)
            {
                // Contracting bodies only
                data = model.Items.Select(x => new[]
                {
                    x.Status,
                    x.ProviderId.ToString(CultureInfo.InvariantCulture),
                    x.Ukprn.HasValue ? x.Ukprn.ToString() : String.Empty,
                    x.IsProvider ? provider : organisation,
                    !x.IsTASOnly.HasValue ? String.Empty : x.IsTASOnly.Value ? yes : no,
                    x.ProviderTypeName,
                    x.ProviderName ?? String.Empty,
                    x.ProviderNameAlias ?? String.Empty,
                    x.LegalName ?? String.Empty,
                    x.LastActivity.HasValue
                        ? x.LastActivity.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                        : String.Empty,
                    x.LastProvisionUpdate.HasValue ? x.LastProvisionUpdate.Value.ToString(Constants.ConfigSettings.ShortDateFormat) : String.Empty,
                    x.UpToDateConfirmations.HasValue
                        ? x.UpToDateConfirmations.Value.ToString("N0")
                        : String.Empty,
                    x.ApplicationName ?? String.Empty,
                    x.ExpiredLARs.HasValue
                        ? x.ExpiredLARs.Value.ToString("N0")
                        : String.Empty,
                    !x.PublishData.HasValue ? na : x.PublishData.Value ? yes : no,
                    x.PrimaryContacts.ToHtml() ?? String.Empty,
                    x.AutoAggregateQualityRating.HasValue
                        ? (x.AutoAggregateQualityRating.Value/100m).ToString("0.#%")
                        : x.IsProvider ? "0%" : String.Empty,
                    x.Rating,
                    x.IsProvider ? "P" + x.ProviderId : "O" + x.ProviderId
                }).ToArray();
            }
            else if (!model.ContractingBodiesOnly && model.IncludeProviders && !model.IncludeOrganisations)
            {
                // Providers only
                data = model.Items.Select(x => new[]
                {
                    x.Status,
                    x.ProviderId.ToString(CultureInfo.InvariantCulture),
                    x.Ukprn.HasValue ? x.Ukprn.ToString() : String.Empty,
                    x.IsContractingBody ? yes : no,
                    !x.IsTASOnly.HasValue ? String.Empty : x.IsTASOnly.Value ? yes : no,
                    x.ProviderTypeName ?? String.Empty,
                    x.ProviderName ?? String.Empty,
                    x.ProviderNameAlias ?? String.Empty,
                    x.LegalName ?? String.Empty,
                    x.LastActivity.HasValue
                        ? x.LastActivity.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                        : String.Empty,
                    x.LastProvisionUpdate.HasValue ? x.LastProvisionUpdate.Value.ToString(Constants.ConfigSettings.ShortDateFormat) : String.Empty,
                    x.UpToDateConfirmations.HasValue
                        ? x.UpToDateConfirmations.Value.ToString("N0")
                        : String.Empty,
                    x.ApplicationName ?? String.Empty,
                    x.ExpiredLARs.HasValue
                        ? x.ExpiredLARs.Value.ToString("N0")
                        : String.Empty,
                    !x.PublishData.HasValue ? na : x.PublishData.Value ? yes : no,
                    x.PrimaryContacts.ToHtml() ?? String.Empty,
                    x.AutoAggregateQualityRating.HasValue
                        ? (x.AutoAggregateQualityRating.Value/100m).ToString("0.#%")
                        : x.IsProvider ? "0%" : String.Empty,
                    x.Rating,
                    "P" + x.ProviderId
                }).ToArray();
            }
            else if (!model.ContractingBodiesOnly && !model.IncludeProviders && model.IncludeOrganisations)
            {
                // Organisations only
                data = model.Items.Select(x => new[]
                {
                    x.Status,
                    x.ProviderId.ToString(CultureInfo.InvariantCulture),
                    x.Ukprn.HasValue ? x.Ukprn.ToString() : String.Empty,
                    x.IsContractingBody ? yes : no,
                    x.ProviderTypeName,
                    x.ProviderName ?? String.Empty,
                    x.ProviderNameAlias ?? String.Empty,
                    x.LegalName ?? String.Empty,
                    na,
                    portal,
                    x.PrimaryContacts.ToHtml(),
                    "O" + x.ProviderId
                }).ToArray();
            }
            else if (!model.ContractingBodiesOnly && model.IncludeProviders && model.IncludeOrganisations)
            {
                // Providers & organisations
                data = model.Items.Select(x => new[]
                {
                    x.Status,
                    x.ProviderId.ToString(CultureInfo.InvariantCulture),
                    x.Ukprn.HasValue ? x.Ukprn.ToString() : String.Empty,
                    x.IsContractingBody ? yes : no,
                    x.IsProvider ? provider : organisation,
                    !x.IsTASOnly.HasValue ? String.Empty : x.IsTASOnly.Value ? yes : no,
                    x.ProviderTypeName,
                    x.ProviderName ?? String.Empty,
                    x.ProviderNameAlias ?? String.Empty,
                    x.LegalName ?? String.Empty,
                    x.LastActivity.HasValue
                        ? x.LastActivity.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                        : String.Empty,
                    x.LastProvisionUpdate.HasValue ? x.LastProvisionUpdate.Value.ToString(Constants.ConfigSettings.ShortDateFormat) : String.Empty,
                    x.UpToDateConfirmations.HasValue
                        ? x.UpToDateConfirmations.Value.ToString("N0")
                        : String.Empty,
                    x.ApplicationName ?? String.Empty,
                    x.ExpiredLARs.HasValue
                        ? x.ExpiredLARs.Value.ToString("N0")
                        : String.Empty,
                    !x.PublishData.HasValue ? na : x.PublishData.Value ? yes : no,
                    x.PrimaryContacts.ToHtml() ?? String.Empty,
                    x.AutoAggregateQualityRating.HasValue
                        ? (x.AutoAggregateQualityRating.Value/100m).ToString("0.#%")
                        : x.IsProvider ? "0%" : String.Empty,
                    x.Rating,
                    x.IsProvider ? "P" + x.ProviderId : "O" + x.ProviderId
                }).ToArray();
            }
            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class DailyReportViewModelExtensions
    {
        public static DailyReportViewModel Populate(this DailyReportViewModel model, ProviderPortalEntities db,
            bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<DailyReportViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_DailyReport] @DecimalPlaces, @SFAProvider, @DFEProvider";
            cmd.Parameters.Add(new SqlParameter("DecimalPlaces", Constants.ConfigSettings.DailyReportDecimalPlaces));
            cmd.Parameters.Add(new SqlParameter("SFAProvider", model.SFAFunded));
            cmd.Parameters.Add(new SqlParameter("DFEProvider", model.DFEFunded));

            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                model.Items =
                    ((IObjectContextAdapter) db).ObjectContext.Translate<DailyReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this DailyReportViewModel model)
        {
            String Yes = AppGlobal.Language.GetText("Report_AdminReports_Yes", "Yes");
            String No = AppGlobal.Language.GetText("Report_AdminReports_No", "No");
            String numberFormat = "N" + Constants.ConfigSettings.DailyReportDecimalPlaces;

            String[][] data = model.Items.Select(x => new[]
            {
                x.ProviderId.ToString(CultureInfo.InvariantCulture),
                x.Ukprn.HasValue ? x.Ukprn.ToString() : String.Empty,
                //x.IsTASOnly.HasValue
                //    ? x.IsTASOnly.Value ? Yes : No
                //    : String.Empty,
                x.ProviderName,
                x.Courses.ToString("N0").Replace(",", ""),
                x.Opportunities.ToString("N0").Replace(",", ""),
                x.OpportunitiesPerCourse.ToString(numberFormat),
                x.Summaries.ToString(numberFormat),
                x.DistinctSummaries.ToString(numberFormat),
                x.Aims.ToString(numberFormat),
                x.DistinctAims.ToString(numberFormat),
                x.Url.ToString(numberFormat),
                x.DistinctUrl.ToString(numberFormat),
                x.BookingUrl.ToString(numberFormat),
                x.DistinctBookingUrl.ToString(numberFormat),
                x.SpecificStarts.ToString(numberFormat),
                x.FutureStarts.ToString(numberFormat),
                x.EntryRequirements.ToString(numberFormat),
                x.Prices.ToString(numberFormat),
                x.LastActivity.HasValue
                    ? x.LastActivity.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                    : String.Empty,
                x.LastUpdated.HasValue
                    ? x.LastUpdated.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                    : String.Empty,
                x.Autoscore.HasValue ? Convert.ToDouble(x.Autoscore).ToString(numberFormat) : String.Empty,
                x.Rating,
                x.LiveSuperuser.ToString(),
                x.Region,
                x.DfERegion,
                x.ProviderType,
                x.DfEProviderType,
                x.DfeProviderStatus,
                x.DfeLocalAuthority,
                x.DfeEstablishmentType,
                x.RoATP ? Yes : No
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }

        public static NewtonsoftJsonResult ToSFAJsonResult(this DailyReportViewModel model)
        {
            String Yes = AppGlobal.Language.GetText("Report_AdminReports_Yes", "Yes");
            String No = AppGlobal.Language.GetText("Report_AdminReports_No", "No");
            String numberFormat = "N" + Constants.ConfigSettings.DailyReportDecimalPlaces;

            String[][] data = model.Items.Select(x => new[]
            {
                x.ProviderId.ToString(CultureInfo.InvariantCulture),
                x.Ukprn.HasValue ? x.Ukprn.ToString() : String.Empty,
                x.IsTASOnly.HasValue
                    ? x.IsTASOnly.Value ? Yes : No
                    : String.Empty,
                x.ProviderName,
                x.Courses.ToString("N0").Replace(",", ""),
                x.Opportunities.ToString("N0").Replace(",", ""),
                x.OpportunitiesPerCourse.ToString(numberFormat),
                x.Summaries.ToString(numberFormat),
                x.DistinctSummaries.ToString(numberFormat),
                x.Aims.ToString(numberFormat),
                x.DistinctAims.ToString(numberFormat),
                x.Url.ToString(numberFormat),
                x.DistinctUrl.ToString(numberFormat),
                x.BookingUrl.ToString(numberFormat),
                x.DistinctBookingUrl.ToString(numberFormat),
                x.SpecificStarts.ToString(numberFormat),
                x.FutureStarts.ToString(numberFormat),
                x.EntryRequirements.ToString(numberFormat),
                x.Prices.ToString(numberFormat),
                x.LastActivity.HasValue
                    ? x.LastActivity.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                    : String.Empty,
                x.LastUpdated.HasValue
                    ? x.LastUpdated.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                    : String.Empty,
                x.Autoscore.HasValue ? Convert.ToDouble(x.Autoscore).ToString(numberFormat) : String.Empty,
                x.Rating,
                x.LiveSuperuser.ToString(),
                x.Region,
                //x.DfERegion,
                x.ProviderType,
                //x.DfEProviderType,
                //x.DfeProviderStatus,
                //x.DfeLocalAuthority,
                //x.DfeEstablishmentType,
                x.RoATP ? Yes : No
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class WeeklyReportViewModelExtensions
    {
        public static WeeklyReportViewModel Populate(this WeeklyReportViewModel model, ProviderPortalEntities db,
            bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<WeeklyReportViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_WeeklyReport] @SFAProvider, @DFEProvider";
            cmd.Parameters.Add(new SqlParameter("SFAProvider", model.SFAFunded));
            cmd.Parameters.Add(new SqlParameter("DFEProvider", model.DFEFunded));

            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                model.Items =
                    ((IObjectContextAdapter) db).ObjectContext.Translate<WeeklyReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this WeeklyReportViewModel model)
        {
            String Yes = AppGlobal.Language.GetText("Report_AdminReports_Yes", "Yes");
            String No = AppGlobal.Language.GetText("Report_AdminReports_No", "No");
            const String numberFormat = "N0";

            String[][] data = model.Items.Select(x => new[]
            {
                x.ProviderId.ToString(CultureInfo.InvariantCulture),
                x.Ukprn.HasValue ? x.Ukprn.ToString() : String.Empty,
                x.IsTASOnly.HasValue
                ? x.IsTASOnly.Value ? Yes : No
                    : String.Empty,
                x.ProviderName,
                x.LastActivity.HasValue
                    ? x.LastActivity.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                    : String.Empty,
                x.LastUpdate.HasValue
                    ? x.LastUpdate.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                    : String.Empty,
                x.LastOpportunityUpdate.HasValue
                    ? x.LastOpportunityUpdate.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                    : String.Empty,
                x.TrafficLightStatus,
                x.Rating,
                x.LastUpdateMethod,
                x.BulkUploadSuccess.ToString(numberFormat),
                x.BulkUploads.ToString(numberFormat),
                x.NumberOfSuperUsers.ToString(numberFormat),
                x.NumberOfUsers.ToString(numberFormat),
                x.Courses.ToString(numberFormat),
                x.BulkUploadCourses.ToString(numberFormat),
                x.ProviderPortalCourses.ToString(numberFormat),
                x.PlanBCourses.ToString(numberFormat),
                x.CoursesWithLearningAims.ToString(numberFormat),
                x.CoursesWithoutLearningAims.ToString(numberFormat),
                x.Opportunities.ToString(numberFormat),
                x.InScopeOpportunities.ToString(numberFormat),
                x.OutOfScopeOpportunities.ToString(numberFormat),
                x.A1010.ToString(numberFormat),
                x.A1022.ToString(numberFormat),
                x.A1025.ToString(numberFormat),
                x.A1035.ToString(numberFormat),
                x.A1045.ToString(numberFormat),
                x.A1046.ToString(numberFormat),
                x.A1070.ToString(numberFormat),
                x.A1080.ToString(numberFormat),
                x.A1081.ToString(numberFormat),
                x.A1021.ToString(numberFormat),
                x.A1082.ToString(numberFormat),
                x.A1099.ToString(numberFormat),
                x.A10NA.ToString(numberFormat),
                x.RoATP ? Yes : No
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class HeadlineStatsReportViewModelExtensions
    {
        public static HeadlineStatsReportViewModel Populate(
            this HeadlineStatsReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<HeadlineStatsReportViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportHeadlineStats]";
           

            List<HeadlineStatsReportViewModelItem> list = new List<HeadlineStatsReportViewModelItem>();
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                list = ((IObjectContextAdapter)db)
                    .ObjectContext
                    .Translate<HeadlineStatsReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            model.Items = list;

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this HeadlineStatsReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.NumberOfCourses.ToString("N0"),
                x.NumberOfOpportunities.ToString("N0"),
                x.NumberOfPoorProviders.ToString("N0"),
                x.NumberOfAverageProviders.ToString("N0"),
                x.NumberOfGoodProviders.ToString("N0"),
                x.NumberOfVeryGoodProviders.ToString("N0"),
                x.AverageQualityScorePercent.ToString("N2") + "%",
                x.AverageQualityScoreText,
                x.ZeroCourses.ToString("N0")

            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class SFAWeeklyReportViewModelExtensions
    {
        public static SFAWeeklyReportViewModel Populate(this SFAWeeklyReportViewModel model, ProviderPortalEntities db,
            bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<SFAWeeklyReportViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_SFAWeeklyReport] @SFAProvider, @DFEProvider";
            cmd.Parameters.Add(new SqlParameter("SFAProvider", model.SFAFunded));
            cmd.Parameters.Add(new SqlParameter("DFEProvider", model.DFEFunded));

            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                model.Items =
                    ((IObjectContextAdapter) db).ObjectContext.Translate<SFAWeeklyReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this SFAWeeklyReportViewModel model)
        {
            const String numberFormat = "N0";
            String[][] data = model.Items.Select(x => new[]
            {
                x.NumberOfProvidersWithSuperusers.ToString(numberFormat),
                x.UpdatedAtLeast1Opportunity.ToString(numberFormat),
                x.BulkUploadOpportunities.ToString(numberFormat),
                x.BulkUploadOpportunitiesLast7Days.ToString(numberFormat),
                x.ManualOpportunities.ToString(numberFormat),
                x.ManualOpportunitiesLast7Days.ToString(numberFormat),
                x.NotUpdatedInLastYear.ToString(numberFormat),
                x.UpdatedInLast7Days.ToString(numberFormat),
                x.UpdatedBetween8and30Days.ToString(numberFormat),
                x.UpdatedBetween31and60Days.ToString(numberFormat),
                x.UpdatedBetween61and90Days.ToString(numberFormat),
                x.UpdatedMoreThan90Days.ToString(numberFormat),
                x.NumberOfCourses.ToString(numberFormat),
                x.NumberOfLiveCourses.ToString(numberFormat),
                x.NumberOfOpportunities.ToString(numberFormat),
                x.NumberOfLiveOpportunities.ToString(numberFormat),
                x.NumberOfInScopeCourses.ToString(numberFormat),
                x.NumberOfOutOfScopeCourses.ToString(numberFormat),
                x.NumberOfInScopeOpportunities.ToString(numberFormat),
                x.NumberOfOutOfScopeOpportunities.ToString(numberFormat),
                x.NumberOfProvidersWithNoCourses.ToString(numberFormat),
                x.NumberOfType1Courses.ToString(numberFormat),
                x.NumberOfType2Courses.ToString(numberFormat),
                x.NumberOfType3Courses.ToString(numberFormat),
                x.A1010.ToString(numberFormat),
                x.A1021.ToString(numberFormat),
                x.A1022.ToString(numberFormat),
                x.A1025.ToString(numberFormat),
                x.A1035.ToString(numberFormat),
                x.A1045.ToString(numberFormat),
                x.A1046.ToString(numberFormat),
                x.A1070.ToString(numberFormat),
                x.A1080.ToString(numberFormat),
                x.A1081.ToString(numberFormat),
                x.A1082.ToString(numberFormat),
                x.A1099.ToString(numberFormat),
                x.A10NA.ToString(numberFormat),
                x.StudyModeFlexible.ToString(numberFormat),
                x.StudyModeFullTime.ToString(numberFormat),
                x.StudyModeNotKnown.ToString(numberFormat),
                x.StudyModePartOfFullTimeProgramme.ToString(numberFormat),
                x.StudyModePartTime.ToString(numberFormat),
                x.AttendanceModeDistanceWithAttendance.ToString(numberFormat),
                x.AttendanceModeDistanceWithoutAttendance.ToString(numberFormat),
                x.AttendanceModeFaceToFace.ToString(numberFormat),
                x.AttendanceModeLocation.ToString(numberFormat),
                x.AttendanceModeMixed.ToString(numberFormat),
                x.AttendanceModeNotKnown.ToString(numberFormat),
                x.AttendanceModeOnlineWithAttendance.ToString(numberFormat),
                x.AttendanceModeOnlineWithoutAttendance.ToString(numberFormat),
                x.AttendanceModeWorkBased.ToString(numberFormat),
                x.AttendancePatternCustomised.ToString(numberFormat),
                x.AttendancePatternDayRelease.ToString(numberFormat),
                x.AttendancePatternDaytime.ToString(numberFormat),
                x.AttendancePatternEvening.ToString(numberFormat),
                x.AttendancePatternNotApplicable.ToString(numberFormat),
                x.AttendancePatternNotKnown.ToString(numberFormat),
                x.AttendancePatternTwilight.ToString(numberFormat),
                x.AttendancePatternWeekend.ToString(numberFormat),
                x.Duration1Week.ToString(numberFormat),
                x.Duration1To4Weeks.ToString(numberFormat),
                x.Duration1To3Months.ToString(numberFormat),
                x.Duration3To6Months.ToString(numberFormat),
                x.Duration6To12Months.ToString(numberFormat),
                x.Duration1To2Years.ToString(numberFormat),
                x.DurationNotKnown.ToString(numberFormat),
                x.QualType14To19Diploma.ToString(numberFormat),
                x.QualTypeAccessToHigher.ToString(numberFormat),
                x.QualTypeApprenticeship.ToString(numberFormat),
                x.QualTypeBasicSkill.ToString(numberFormat),
                x.QualTypeCertificateOfAttendance.ToString(numberFormat),
                x.QualTypeCourseProviderCertificate.ToString(numberFormat),
                x.QualTypeExternalAward.ToString(numberFormat),
                x.QualTypeFoundationDegree.ToString(numberFormat),
                x.QualTypeFunctionalSkills.ToString(numberFormat),
                x.QualTypeGCE.ToString(numberFormat),
                x.QualTypeGSCE.ToString(numberFormat),
                x.QualTypeHND.ToString(numberFormat),
                x.QualTypeBaccalaureate.ToString(numberFormat),
                x.QualTypeNoQualification.ToString(numberFormat),
                x.QualTypeNVQ.ToString(numberFormat),
                x.QualTypeOtherAccredited.ToString(numberFormat),
                x.QualTypePostgraduate.ToString(numberFormat),
                x.QualTypeProfessionalOrIndustrySpecific.ToString(numberFormat),
                x.QualTypeUndergraduate.ToString(numberFormat),
                x.QualLevelEntry.ToString(numberFormat),
                x.QualLevelHigher.ToString(numberFormat),
                x.QualLevel1.ToString(numberFormat),
                x.QualLevel2.ToString(numberFormat),
                x.QualLevel3.ToString(numberFormat),
                x.QualLevel4.ToString(numberFormat),
                x.QualLevel5.ToString(numberFormat),
                x.QualLevel6.ToString(numberFormat),
                x.QualLevel7.ToString(numberFormat),
                x.QualLevel8.ToString(numberFormat),
                x.QualLevelUnknown.ToString(numberFormat),
                x.ProvidersWithSuperusersUpdatedInLastMonth.ToString(numberFormat),
                x.ProvidersWithSuperusersUpdatedBetween2and3Months.ToString(numberFormat),
                x.ProvidersWithSuperusersUpdatedMoreThan3Months.ToString(numberFormat),
                x.ProvidersWithLoggedInUsersNotUpdatedSince01Aug2010.ToString(numberFormat),
                x.TrafficLightGreen.ToString(numberFormat),
                x.TrafficLightAmber.ToString(numberFormat),
                x.TrafficLightRed.ToString(numberFormat),
                x.QualityVeryGood.ToString(numberFormat),
                x.QualityGood.ToString(numberFormat),
                x.QualityAverage.ToString(numberFormat),
                x.QualityPoor.ToString(numberFormat),
                x.QualityPoorAndAverage.ToString(numberFormat),
                x.QualityGoodAndVeryGood.ToString(numberFormat),
                x.AverageScore.ToString("N2"),
                x.TotalUserSessions.ToString(numberFormat),
                x.ProviderPortalUserSessions.ToString(numberFormat),
                x.SecureAccessUserSessions.ToString(numberFormat),
                x.PublishedProviders.ToString(numberFormat),
                x.UnpublishedProviders.ToString(numberFormat)
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class MonthlyReportModelExtensions
    {
        public static MonthlyReportModel Populate(this MonthlyReportModel model, ProviderPortalDataWarehouseEntities db)
        {
            DbCommand cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[usp_ReportMonthlyReport] @PeriodToRun, @PeriodType";
            cmd.Parameters.Add(new SqlParameter("PeriodToRun", String.IsNullOrWhiteSpace(model.PeriodToRun) ? DBNull.Value : (Object)model.PeriodToRun));
            cmd.Parameters.Add(new SqlParameter("PeriodType", String.IsNullOrWhiteSpace(model.PeriodType) ? DBNull.Value : (Object)model.PeriodType));

            //try
            {
                db.Database.Connection.Open();

                // Get the Usage data
                DbDataReader reader = cmd.ExecuteReader();
                model.Usage = ((IObjectContextAdapter)db).ObjectContext.Translate<MonthlyReport_UsageModel>(reader).ToList();

                // Get the Provision data
                reader.NextResult();
                model.Provision = ((IObjectContextAdapter)db).ObjectContext.Translate<MonthlyReport_ProvisionModel>(reader).ToList();

                // Get the Quality data
                reader.NextResult();
                model.Quality = ((IObjectContextAdapter)db).ObjectContext.Translate<MonthlyReport_QualityModel>(reader).ToList();

                // Get the Average Quality Score
                reader.NextResult();
                if (reader.Read())
                {
                    model.AverageQualityScore = reader.IsDBNull(0) ? (Decimal?)null : reader.GetDecimal(0);
                }
            }
            //catch { }

            return model;
        }
    }

    public static class DFEStartDateReportModelExtensions
    {
        public static DFEStartDateReportModel Populate(this DFEStartDateReportModel model, ProviderPortalDataWarehouseEntities db)
        {

            // Calculate current academic year

            DateTime currentDate = DateTime.Now;
            
            if (currentDate.Month>9)
            {
                model.AcademicYearStart = new DateTime(currentDate.Year, 9, 1);
                model.AcademicYearNext = new DateTime(currentDate.Year + 1, 9, 1);
            } else
            {
                model.AcademicYearStart = new DateTime(currentDate.Year-1, 9, 1);
                model.AcademicYearNext = new DateTime(currentDate.Year, 9, 1);
            }

            DbCommand cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[usp_ReportDFEStartDateReport] @PeriodToRun, @LastActive, @YearStart";
            cmd.Parameters.Add(new SqlParameter("PeriodToRun", String.IsNullOrWhiteSpace(model.PeriodToRun) ? DBNull.Value : (Object)model.PeriodToRun));
            cmd.Parameters.Add(new SqlParameter("LastActive", model.AcademicYearStart));
            cmd.Parameters.Add(new SqlParameter("YearStart", model.AcademicYearNext));

            //try
            {
                db.Database.Connection.Open();

                // Get the Course Opportunity data
                DbDataReader reader = cmd.ExecuteReader();
                model.Usage = ((IObjectContextAdapter)db).ObjectContext.Translate<DFEStartDateReport_COModel>(reader).ToList();

                

                // Get the Quality data
                reader.NextResult();
                model.Quality = ((IObjectContextAdapter)db).ObjectContext.Translate<MonthlyReport_QualityModel>(reader).ToList();

                // Get the Average Quality Score
                reader.NextResult();
                if (reader.Read())
                {
                    model.AverageQualityScore = reader.GetString(0);
                }

                // Providers with no courses

                reader.NextResult();
                if (reader.Read())
                {
                    model.TotalProvidersWithZeroCourses =  reader.GetInt32(0);
                }

                // Total Providers

                reader.NextResult();
                if (reader.Read())
                {
                    model.TotalProviders = reader.GetInt32(0);
                }

                // Get the DFE Only Quality data
                reader.NextResult();
                model.DFEQuality = ((IObjectContextAdapter)db).ObjectContext.Translate<MonthlyReport_QualityModel>(reader).ToList();

                // Get the DFE and SFA Quality data
                reader.NextResult();
                model.DFESFAQuality = ((IObjectContextAdapter)db).ObjectContext.Translate<MonthlyReport_QualityModel>(reader).ToList();

                // Total Providers Updated Since

                reader.NextResult();
                if (reader.Read())
                {
                    model.TotalProvidersUpdated = reader.GetInt32(0);
                }

                // Total Courses uploaded

                reader.NextResult();
                if (reader.Read())
                {
                    model.FundedCoursesUploaded = reader.GetInt32(0);
                }

                // Total Courses uploaded with start date in the next academic year

                reader.NextResult();
                if (reader.Read())
                {
                    model.FundedCoursesUploadedPostSept = reader.GetInt32(0);
                }

            }
            //catch { }

            return model;
        }
    }

    public static class SalesForceAccountsViewModelExtensions
    {
        public static SalesForceAccountsViewModel Populate(this SalesForceAccountsViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<SalesForceAccountsViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportSalesForceAccounts]";

            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                model.Items = ((IObjectContextAdapter) db).ObjectContext
                    .Translate<up_ReportSalesForceAccounts_Result>(reader)
                    .Select(x => new SalesForceAccountsViewModelItem
                    {
                        Name = x.Name,
                        DFE1619Funded = x.DFE1619Funded == 1,
                        SFAFunded = x.SFAFunded == 1,
                        Type = x.Type,
                        BillingStreet = x.BillingStreet,
                        BillingCity = x.BillingCity,
                        BillingState = x.BillingState,
                        BillingPostalCode = x.BillingPostalCode,
                        Phone = x.Phone,
                        Fax = x.Fax,
                        Website = x.Website,
                        UKPRN = x.UKPRN,
                        Region = x.Region,
                        ExternalId = x.ExternalId,
                        QualityStatus = x.QualityStatus,
                        PrimaryContact = x.PrimaryContact,
                        PrimaryContactEmail = x.PrimaryContactEmail,
                        DatePortalLastUpdated = x.DatePortalLastUpdated,
                        DfEProviderType = x.DfEProviderType,
                        DfEProviderStatus = x.DfEProviderStatus,
                        DfELocalAuthority = x.DfELocalAuthority,
                        DfERegion = x.DfeRegion,
                        DfEEstablishmentType = x.DfEEstablishmentType,
                        LastQualityEmailName = x.LastQualityEmailName,
                        LastQualityEmailDate = x.LastQualityEmailDate,
                        NextQualityEmailName = x.NextQualityEmailName,
                        NextQualityEmailDate = x.NextQualityEmailDate,
                        QualityEmailSent = x.QualityEmailSent,
                        QualityEmailsPaused = x.QualityEmailsPaused,
                        AutoAggregateQualityRating = x.AutoAggregateQualityRating,
                        TrafficLightStatus = x.TrafficLightStatus
                    })
                    .ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this SalesForceAccountsViewModel model)
        {
            var yes = AppGlobal.Language.GetText("Report_AdminReports_True", "True");
            var no = AppGlobal.Language.GetText("Report_AdminReports_False", "False");

            String[][] data = model.Items.Select(x => new[]
            {
                x.Name,
                x.Type,
                x.BillingStreet,
                x.BillingCity,
                x.BillingState,
                x.BillingPostalCode,
                x.Phone,
                x.Fax,
                x.Website,
                x.UKPRN.HasValue ? x.UKPRN.Value.ToString(CultureInfo.InvariantCulture) : "",
                x.Region,
                x.ExternalId,
                x.QualityStatus,
                x.PrimaryContact,
                x.PrimaryContactEmail,
                x.DatePortalLastUpdated.HasValue ? x.DatePortalLastUpdated.Value.ToString("yyyy-MM-dd") : "",
                x.ExternalId.StartsWith("P") ? x.SFAFunded.ToString() : "",
                x.ExternalId.StartsWith("P") ? x.DFE1619Funded.ToString() : "",
                string.IsNullOrEmpty(x.DfEProviderType) ? string.Empty : x.DfEProviderType,
                string.IsNullOrEmpty(x.DfEProviderStatus) ? string.Empty : x.DfEProviderStatus,
                string.IsNullOrEmpty(x.DfELocalAuthority) ? string.Empty : x.DfELocalAuthority,
                string.IsNullOrEmpty(x.DfERegion) ? string.Empty : x.DfERegion,
                string.IsNullOrEmpty(x.DfEEstablishmentType) ? string.Empty : x.DfEEstablishmentType,
                x.LastQualityEmailName,
                x.LastQualityEmailDate.HasValue ? x.LastQualityEmailDate.Value.ToString("yyyy-MM-dd") : "",
                x.NextQualityEmailName,
                x.NextQualityEmailDate.HasValue ? x.NextQualityEmailDate.Value.ToString("yyyy-MM-dd") : "",
                x.QualityEmailSent == null || !x.LastQualityEmailDate.HasValue
                    ? ""
                    : x.QualityEmailSent.Value ? yes : no,
                x.ExternalId.StartsWith("O") || !x.QualityEmailsPaused.HasValue
                    ? ""
                    : x.QualityEmailsPaused.Value ? yes : no,
                x.AutoAggregateQualityRating.HasValue ? (x.AutoAggregateQualityRating.Value/100m).ToString("0.#%") : "",
                x.TrafficLightStatus
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class SalesForceContactsViewModelExtensions
    {
        public static SalesForceContactsViewModel Populate(this SalesForceContactsViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<SalesForceContactsViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportSalesForceContacts]";

            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                model.Items =
                    ((IObjectContextAdapter) db).ObjectContext.Translate<SalesForceContactsViewModelItem>(reader)
                        .ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this SalesForceContactsViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.AccountName,
                x.FirstName,
                x.LastName,
                x.Email,
                x.MailingStreet,
                x.MailingCity,
                x.MailingState,
                x.MailingPostalCode,
                x.Phone,
                x.IsPrimaryContact,
                x.UserExternalId,
                x.SFAFunded.ToString(),
                x.DFE1619Funded.ToString(),
                x.ExternalId
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class BulkUploadHistoryReportViewModelExtensions
    {
        public static BulkUploadHistoryReportViewModel Populate(this BulkUploadHistoryReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<BulkUploadHistoryReportViewModelItem>();
                return model;
            }

            if (model.StartDate > model.EndDate)
            {
                var temp = model.StartDate;
                model.StartDate = model.EndDate;
                model.EndDate = temp;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportBulkUploadHistory] @StartDate, @EndDate";
            cmd.Parameters.Add(model.StartDate.HasValue
                ? new SqlParameter("StartDate", model.StartDate.Value.Date)
                : new SqlParameter("StartDate", DBNull.Value));
            cmd.Parameters.Add(model.EndDate.HasValue
                ? new SqlParameter("EndDate", model.EndDate.Value.Date.AddDays(1).AddSeconds(-1))
                : new SqlParameter("EndDate", DBNull.Value));

            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                model.Items =
                    ((IObjectContextAdapter) db).ObjectContext.Translate<BulkUploadHistoryReportViewModelItem>(reader)
                        .ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this BulkUploadHistoryReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.BulkUploadId.ToString(CultureInfo.InvariantCulture),
                String.Format("{0:" + Constants.ConfigSettings.ShortDateTimeFormat + "}",
                    x.CreatedDateTimeUtc.ToLocalTime()),
                x.OrganisationName,
                x.ProviderName,
                x.UserName,
                x.BulkUploadStatusText,
                x.FileName,
                x.ExistingCourses.ToString(CultureInfo.InvariantCulture),
                x.NewCourses.ToString(CultureInfo.InvariantCulture),
                x.InvalidCourses.ToString(CultureInfo.InvariantCulture),
                x.ExistingOpportunities.ToString(CultureInfo.InvariantCulture),
                x.NewOpportunities.ToString(CultureInfo.InvariantCulture),
                x.InvalidOpportunities.ToString(CultureInfo.InvariantCulture),
                x.ExistingVenues.ToString(CultureInfo.InvariantCulture),
                x.NewVenues.ToString(CultureInfo.InvariantCulture),
                x.InvalidVenues.ToString(CultureInfo.InvariantCulture),
                x.Errors.ToString(CultureInfo.InvariantCulture),
                x.Warnings.ToString(CultureInfo.InvariantCulture),
                x.SystemExceptions.ToString(CultureInfo.InvariantCulture),
                x.Successes.ToString(CultureInfo.InvariantCulture),
                x.Notices.ToString(CultureInfo.InvariantCulture)
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }






    public static class BulkUploadHistoryApprenticeReportViewModelExtensions
    {
        public static BulkUploadHistoryApprenticeReportViewModel Populate(this BulkUploadHistoryApprenticeReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<BulkUploadHistoryApprenticeReportViewModelItem>();
                return model;
            }

            if (model.StartDate > model.EndDate)
            {
                var temp = model.StartDate;
                model.StartDate = model.EndDate;
                model.EndDate = temp;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportBulkUploadHistoryApprentice] @StartDate, @EndDate";
            cmd.Parameters.Add(model.StartDate.HasValue
                ? new SqlParameter("StartDate", model.StartDate.Value.Date)
                : new SqlParameter("StartDate", DBNull.Value));
            cmd.Parameters.Add(model.EndDate.HasValue
                ? new SqlParameter("EndDate", model.EndDate.Value.Date.AddDays(1).AddSeconds(-1))
                : new SqlParameter("EndDate", DBNull.Value));

            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                model.Items =
                    ((IObjectContextAdapter)db).ObjectContext.Translate<BulkUploadHistoryApprenticeReportViewModelItem>(reader)
                        .ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this BulkUploadHistoryApprenticeReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.BulkUploadId.ToString(CultureInfo.InvariantCulture),
                String.Format("{0:" + Constants.ConfigSettings.ShortDateTimeFormat + "}",
                    x.CreatedDateTimeUtc.ToLocalTime()),
                x.OrganisationName,
                x.ProviderName,
                x.UserName,
                x.BulkUploadStatusText,
                x.FileName,
                x.ExistingApprenticeships.ToString(CultureInfo.InvariantCulture),
                x.NewApprenticeships.ToString(CultureInfo.InvariantCulture),
                x.InvalidApprenticeships.ToString(CultureInfo.InvariantCulture),
                x.ExistingDelivLocations.ToString(CultureInfo.InvariantCulture),
                x.NewDelivLocations.ToString(CultureInfo.InvariantCulture),
                x.InvalidDelivLocations.ToString(CultureInfo.InvariantCulture),
                x.Errors.ToString(CultureInfo.InvariantCulture),
                x.Warnings.ToString(CultureInfo.InvariantCulture),
                x.SystemExceptions.ToString(CultureInfo.InvariantCulture),
                x.Successes.ToString(CultureInfo.InvariantCulture),
                x.Notices.ToString(CultureInfo.InvariantCulture)
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }






    public static class QualityEmailHistoryReportViewModelExtensions
    {
        public static QualityEmailHistoryReportViewModel Populate(this QualityEmailHistoryReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<QualityEmailHistoryReportViewModelItem>();
                return model;
            }

            if (model.StartDate > model.EndDate)
            {
                var temp = model.StartDate;
                model.StartDate = model.EndDate;
                model.EndDate = temp;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportProviderQualityEmailHistory] @ProviderId, @StartDate, @EndDate";
            cmd.Parameters.Add(model.ProviderId.HasValue
                ? new SqlParameter("ProviderId", model.ProviderId)
                : new SqlParameter("ProviderId", DBNull.Value));
            cmd.Parameters.Add(model.StartDate.HasValue
                ? new SqlParameter("StartDate", model.StartDate.Value.Date)
                : new SqlParameter("StartDate", DBNull.Value));
            cmd.Parameters.Add(model.EndDate.HasValue
                ? new SqlParameter("EndDate", model.EndDate.Value.Date.AddDays(1).AddSeconds(-1))
                : new SqlParameter("EndDate", DBNull.Value));
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                model.Items =
                    ((IObjectContextAdapter) db).ObjectContext.Translate<QualityEmailHistoryReportViewModelItem>(reader)
                        .ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this QualityEmailHistoryReportViewModel model)
        {
            var yes = AppGlobal.Language.GetText("Report_AdminReports_Yes", "Yes");
            var no = AppGlobal.Language.GetText("Report_AdminReports_No", "No");

            String[][] data = model.Items.Select(x => new[]
            {
                QualityIndicator.GetTrafficText(x.TrafficLightStatusId),
                x.Ukprn.ToString(CultureInfo.InvariantCulture),
                x.ProviderId.ToString(CultureInfo.InvariantCulture),
                x.ProviderName,
                x.ProviderTypeName,
                x.ModifiedDateTimeUtc.ToShortLocalDateString(),
                x.SFAFunded ? yes : no,
                x.DfE1619Funded ? yes : no,
                x.QualityEmailsPaused ? yes : no,
                x.HasValidRecipients ? yes : no,
                x.EmailDateTimeUtc.ToShortLocalDateString(),
                x.EmailTemplateName,
                x.NextEmailDateTimeUtc.ToShortLocalDateString(),
                x.NextEmailTemplateName
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class MetadataUploadHistoryReportViewModelExtensions
    {
        public static MetadataUploadHistoryReportViewModel Populate(this MetadataUploadHistoryReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<MetadataUploadHistoryReportViewModelItem>();
                return model;
            }

            model.Items = db.MetadataUploads
                .OrderByDescending(x => x.CreatedDateTimeUtc)
                .Select(x => new MetadataUploadHistoryReportViewModelItem
                {
                    MetadataUploadId = x.MetadataUploadId,
                    MetadataUploadTypeName = x.MetadataUploadType.MetadataUploadTypeName,
                    MetadataUploadTypeDescription = x.MetadataUploadType.MetadataUploadTypeDescription,
                    RowsBefore = x.RowsBefore,
                    RowsAfter = x.RowsAfter,
                    CreatedByUser = x.AspNetUser.Name,
                    CreatedDateTimeUtc = x.CreatedDateTimeUtc,
                    DurationInMilliseconds = x.DurationInMilliseconds,
                    FileName = x.FileName,
                    FileSizeInBytes = x.FileSizeInBytes
                }).ToList();

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this MetadataUploadHistoryReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                //x.MetadataUploadId.ToString(CultureInfo.InvariantCulture),
                x.CreatedDateTimeUtc.ToShortLocalDateTimeString(),
                //x.MetadataUploadTypeName,
                x.MetadataUploadTypeDescription,
                x.RowsBefore.ToString(CultureInfo.InvariantCulture),
                x.RowsAfter.ToString(CultureInfo.InvariantCulture),
                x.CreatedByUser,
                new TimeSpan(10000*x.DurationInMilliseconds).ToString("g"),
                x.FileName,
                x.FileSizeInBytes.HasValue ? x.FileSizeInBytes.Value.ToFileSizeString() : String.Empty,
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    #region Apprenticeship (RoATP) Reports

    public static class ApprenticeshipOverallCountReportViewModelExtensions
    {
        public static ApprenticeshipOverallCountReportViewModel Populate(
            this ApprenticeshipOverallCountReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<ApprenticeshipOverallCountReportViewModelItem>();
                return model;
            }

            Int32 providerCount =
                db.Providers.Count(
                    x =>
                        x.Apprenticeships.Count(a => a.RecordStatusId == (Int32) Constants.RecordStatus.Live) > 0 &&
                        x.RecordStatusId == (Int32) Constants.RecordStatus.Live);
            Int32 apprenticeshipCount =
                db.Apprenticeships.Count(
                    x =>
                        x.Provider.RecordStatusId == (Int32) Constants.RecordStatus.Live &&
                        x.RecordStatusId == (Int32) Constants.RecordStatus.Live);

            model.Items.Add(new ApprenticeshipOverallCountReportViewModelItem(providerCount, apprenticeshipCount));

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this ApprenticeshipOverallCountReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.ProviderCount.ToString("N0"),
                x.ApprenticeshipCount.ToString("N0")
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class ApprenticeshipDetailedListUploadedReportViewModelExtensions
    {
        public static ApprenticeshipDetailedListUploadedReportViewModel Populate(
            this ApprenticeshipDetailedListUploadedReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<ApprenticeshipDetailedListUploadedReportViewModelItem>();
                return model;
            }

            model.Items = db.Providers
                .Where(x => x.Apprenticeships.Count(a => a.RecordStatusId == (Int32) Constants.RecordStatus.Live) > 0)
                .OrderBy(x => x.Ukprn)
                .Select(x => new ApprenticeshipDetailedListUploadedReportViewModelItem
                {
                    UKPRN = x.Ukprn.ToString(),
                    ProviderName = x.ProviderName,
                    ApprenticeshipCount =
                        x.Apprenticeships.Count(a => a.RecordStatusId == (Int32) Constants.RecordStatus.Live),
                    FrameworkCount =
                        x.Apprenticeships.Where(
                            a => a.RecordStatusId == (Int32) Constants.RecordStatus.Live && a.FrameworkCode != null)
                            .Select(s => new {s.FrameworkCode, s.ProgType, s.PathwayCode})
                            .Distinct()
                            .Count(),
                    // x.Apprenticeships.Count(a => a.RecordStatusId == (Int32)Constants.RecordStatus.Live && a.FrameworkCode != null),
                    StandardCount =
                        x.Apprenticeships.Where(
                            a => a.RecordStatusId == (Int32) Constants.RecordStatus.Live && a.StandardCode != null)
                            .Select(a => new {a.StandardCode, a.Version})
                            .Distinct()
                            .Count(),
                    ProviderId = x.ProviderId
                }).ToList();

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this ApprenticeshipDetailedListUploadedReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.UKPRN,
                x.ProviderName,
                x.ApprenticeshipCount.ToString("N0"),
                x.FrameworkCount.ToString("N0"),
                x.StandardCount.ToString("N0"),
                x.ProviderId.ToString()
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class ProvidersWithOver10ApprenticeshipsReportViewModelExtensions
    {
        public static ProvidersWithOver10ApprenticeshipsReportViewModel Populate(
            this ProvidersWithOver10ApprenticeshipsReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<ProvidersWithOver10ApprenticeshipsReportViewModelItem>();
                return model;
            }

            model.Items = db.Providers
                .Where(
                    x =>
                        x.RecordStatusId == (Int32) Constants.RecordStatus.Live &&
                        x.Apprenticeships.Count(a => a.RecordStatusId == (Int32) Constants.RecordStatus.Live) > 10)
                .OrderByDescending(
                    x => x.Apprenticeships.Count(a => a.RecordStatusId == (Int32) Constants.RecordStatus.Live))
                .Select(x => new ProvidersWithOver10ApprenticeshipsReportViewModelItem
                {
                    UKPRN = x.Ukprn.ToString(),
                    ProviderName = x.ProviderName,
                    NumberOfApprenticeships =
                        x.Apprenticeships.Count(a => a.RecordStatusId == (Int32) Constants.RecordStatus.Live),
                    ProviderId = x.ProviderId
                }).ToList();

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this ProvidersWithOver10ApprenticeshipsReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.UKPRN,
                x.ProviderName,
                x.NumberOfApprenticeships.ToString("N0"),
                x.ProviderId.ToString()
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class QAdProvidersReportViewModelExtensions
    {
        public static QAdProvidersReportViewModel Populate(this QAdProvidersReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<QAdProvidersReportViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportQualityAssuredProviders]";
  
            var list = new List<QAdProvidersReportViewModelItem>();
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                list = ((IObjectContextAdapter)db)
                    .ObjectContext
                    .Translate<QAdProvidersReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            model.Items = list;

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this QAdProvidersReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.UKPRN.ToString(),
                x.ProviderName,
                x.NumberOfApprenticeships.ToString("N0"),
                x.Passed,
                x.NumberRequiredToQA.ToString("N0"),
                x.PassedCompliance,
                x.NumberQAdForCompliance.ToString("N0"),
                x.PassedStyle,
                x.NumberQAdForStyle.ToString("N0"),
                x.ImportBatchNames,
                x.ProviderId.ToString()
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class ProvidersSubmittedDataForQAReportViewModelExtensions
    {
        public static ProvidersSubmittedDataForQAReportViewModel Populate(
            this ProvidersSubmittedDataForQAReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<ProvidersSubmittedDataForQAReportViewModelItem>();
                return model;
            }

            model.Items = db.Providers
                .Where(x => x.RecordStatusId == (Int32) Constants.RecordStatus.Live && x.DataReadyToQA)
                .OrderBy(x => x.ProviderName)
                .Select(x => new ProvidersSubmittedDataForQAReportViewModelItem
                {
                    UKPRN = x.Ukprn.ToString(),
                    ProviderName = x.ProviderName,
                    NumberOfApprenticeships =
                        x.Apprenticeships.Count(a => a.RecordStatusId == (Int32) Constants.RecordStatus.Live),
                    ProviderId = x.ProviderId,
                    ImportBatchList = x.ImportBatches.OrderByDescending(b => b.ImportBatch.ImportBatchId).Select(b => b.ImportBatch.ImportBatchName).ToList(),
                }).ToList();

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this ProvidersSubmittedDataForQAReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.UKPRN,
                x.ProviderName,
                x.NumberOfApprenticeships.ToString("N0"),
                x.ImportBatchNames,
                x.ProviderId.ToString()
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class RegisterOpeningReportViewModelExtensions
    {
        public static RegisterOpeningReportViewModel Populate(this RegisterOpeningReportViewModel model, ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<RegisterOpeningReportViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportRegisterOpening] @ImportBatchId";
            cmd.Parameters.Add(new SqlParameter("ImportBatchId", model.ImportBatchId ?? -1));

            List<RegisterOpeningReportViewModelItem> list = new List<RegisterOpeningReportViewModelItem>();
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                list = ((IObjectContextAdapter)db)
                    .ObjectContext
                    .Translate<RegisterOpeningReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            model.Items = list;

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this RegisterOpeningReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.NumberOfProvidersWithProviderLevelData.ToString("N0"),
                x.NumberOfProvidersWithApprenticeshipLevelData.ToString("N0"),
                x.NumberOfApprenticeshipOffers.ToString("N0"),
                x.NumberOfProvidersWhoHaveAppliedInRound.ToString("N0"),
                x.NumberOfProvidersWithoutApprenticeshipLevelData.ToString("N0"),
                x.NumberOfProvidersWhoHaveBeenOverallQAd.ToString("N0"),
                x.NumberOfProvidersWhoHavePassedOverallQA.ToString("N0"),
                x.NumberOfProvidersWhoHaveFailedOverallQA.ToString("N0"),
                x.SpecificEmployerNamed.ToString("N0"),
                x.UnverifiableClaim.ToString("N0"),
                x.IncorrectOfstedGrade.ToString("N0"),
                x.InsufficientDetail.ToString("N0"),
                x.NotAimedAtEmployer.ToString("N0"),
                x.NumberOfApprenticeshipOffersQAd.ToString("N0"),
                x.NumberOfApprenticeshipsOffersFailed.ToString("N0")
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class RegisterOpeningDetailReportViewModelExtensions
    {
        public static RegisterOpeningDetailReportViewModel Populate(this RegisterOpeningDetailReportViewModel model, ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<RegisterOpeningDetailReportViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportRegisterOpeningDetail] @ImportBatchId";
            cmd.Parameters.Add(new SqlParameter("ImportBatchId", model.ImportBatchId ?? -1));

            List<RegisterOpeningDetailReportViewModelItem> list = new List<RegisterOpeningDetailReportViewModelItem>();
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                list = ((IObjectContextAdapter)db)
                    .ObjectContext
                    .Translate<RegisterOpeningDetailReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            model.Items = list;

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this RegisterOpeningDetailReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.ProviderId.ToString(),
                x.ProviderName,
                x.UKPRN.ToString(),
                x.HasProviderLevelData,
                x.HasApprenticeshipLevelData,
                x.NumberOfApprenticeshipOffers.ToString("N0"),
                x.HadApprenticeshipLevelData,
                x.HasBeenOverallQAd,
                x.HasPassedOverallQA,
                x.HasFailedOverallQA,
                x.SpecificEmployerNamed,
                x.UnverifiableClaim,
                x.IncorrectOfstedGrade,
                x.InsufficientDetail,
                x.NotAimedAtEmployer,
                x.NumberOfApprenticeshipOffersQAd.ToString("N0"),
                x.NumberOfApprenticeshipsOffersFailed.ToString("N0"),
                x.ImportBatches
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class ProvidersWithArchivedApprenticeshipsReportViewModelExtensions
    {
        public static ProvidersWithArchivedApprenticeshipsReportViewModel Populate(this ProvidersWithArchivedApprenticeshipsReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<ProvidersWithArchivedApprenticeshipsReportViewModelItem>();
                return model;
            }

            DbCommand cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportProvidersWithArchivedApprenticeships] @StartDate, @EndDate";
            cmd.Parameters.Add(new SqlParameter("StartDate", model.StartDate.Date));
            cmd.Parameters.Add(new SqlParameter("EndDate", model.EndDate.Date));

            List<ProvidersWithArchivedApprenticeshipsReportViewModelItem> list = new List<ProvidersWithArchivedApprenticeshipsReportViewModelItem>();
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                list = ((IObjectContextAdapter)db)
                    .ObjectContext
                    .Translate<ProvidersWithArchivedApprenticeshipsReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            model.Items = list;

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this ProvidersWithArchivedApprenticeshipsReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.ProviderId.ToString(),
                x.ProviderName,
                x.UKPRN.ToString(),
                x.NumberOfArchivedApprenticeships.ToString("N0"),
                x.NumberOfUnarchivedApprenticeships.ToString("N0"),
                x.NumberOfCurrentArchivedApprenticeships.ToString("N0"),
                x.NumberOfCurrentLiveApprenticeships.ToString("N0")
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class ProvidersWithArchivedApprenticeshipsDetailedReportViewModelExtensions
    {
        public static ProvidersWithArchivedApprenticeshipsDetailedReportViewModel Populate(this ProvidersWithArchivedApprenticeshipsDetailedReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<ProvidersWithArchivedApprenticeshipsDetailedReportViewModelItem>();
                return model;
            }

            DbCommand cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportProvidersWithArchivedApprenticeshipsDetailed] @StartDate, @EndDate";
            cmd.Parameters.Add(new SqlParameter("StartDate", model.StartDate.Date));
            cmd.Parameters.Add(new SqlParameter("EndDate", model.EndDate.Date));

            List<ProvidersWithArchivedApprenticeshipsDetailedReportViewModelItem> list = new List<ProvidersWithArchivedApprenticeshipsDetailedReportViewModelItem>();
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                list = ((IObjectContextAdapter)db)
                    .ObjectContext
                    .Translate<ProvidersWithArchivedApprenticeshipsDetailedReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            model.Items = list;

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this ProvidersWithArchivedApprenticeshipsDetailedReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.ProviderId.ToString(),
                x.ProviderName,
                x.UKPRN.ToString(),
                x.StandardOrFramework,
                x.ApprenticeshipDetails,
                x.NumberOfTimesArchived.ToString("N0"),
                x.NumberOfTimesUnarchived.ToString("N0"),
                x.CurrentStatus
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }

    public static class ProviderUnableToCompleteHistoryReportViewModelExtensions
    {
        /// <summary>
        /// Populates view model for all providers.
        /// </summary>
        /// <returns></returns>
        public static ProviderUnableToCompleteHistoryReportViewModel Populate(this ProviderUnableToCompleteHistoryReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<ProviderUnableToCompleteHistoryReportViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportProviderUnableToCompleteHistory] @ShowAllProviders, @ProviderId";
            cmd.Parameters.Add(new SqlParameter("ShowAllProviders", model.ShowAllProviders));
            cmd.Parameters.Add(new SqlParameter("ProviderId", model.ProviderId));

            List<ProviderUnableToCompleteHistoryReportViewModelItem> list = new List<ProviderUnableToCompleteHistoryReportViewModelItem>();
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                list = ((IObjectContextAdapter)db)
                    .ObjectContext
                    .Translate<ProviderUnableToCompleteHistoryReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            model.Items = list;

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this ProviderUnableToCompleteHistoryReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.ProviderId.ToString(),
                x.Ukprn.HasValue ? x.Ukprn.ToString() : String.Empty,
                x.ProviderName,
                x.UnableToCompleteReasonChecks.ToString().Replace(",",", "),
                x.UnableToCompleteText != null ? x.UnableToCompleteText.ToString() : String.Empty,
                x.UnableToCompleteDateCreated.HasValue
                    ? x.UnableToCompleteDateCreated.Value.ToString()
                    : String.Empty,
                x.UnableToCompleteUsername.ToString()
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }



    public static class RoATPProvidersRefreshedReportViewModelExtensions
    {
        /// <summary>
        /// Populates view model for all providers.
        /// </summary>
        /// <returns></returns>
        public static RoATPProvidersRefreshedReportViewModel Populate(this RoATPProvidersRefreshedReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<RoATPProvidersRefreshedReportViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportRoATPProviderRefreshed] @StartDate, @EndDate";
            cmd.Parameters.Add(new SqlParameter("StartDate", model.StartDate));
            cmd.Parameters.Add(new SqlParameter("EndDate", model.EndDate));

            List<RoATPProvidersRefreshedReportViewModelItem> list = new List<RoATPProvidersRefreshedReportViewModelItem>();
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                list = ((IObjectContextAdapter)db)
                    .ObjectContext
                    .Translate<RoATPProvidersRefreshedReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            model.Items = list;

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this RoATPProvidersRefreshedReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.ProviderId.ToString(),
                x.ProviderName,
                x.UKPRN.ToString(),
                x.RoATPStartDate.HasValue
                    ? x.RoATPStartDate.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                    : String.Empty,
                x.RoATPProviderType,
                x.ImportBatchName,
                x.RefreshTimeUtc.HasValue
                    ? x.RefreshTimeUtc.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                    : String.Empty,
                x.ConfirmedBy.ToString()
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }



    public static class RoATPProvidersNotRefreshedReportViewModelExtensions
    {
        /// <summary>
        /// Populates view model for all providers.
        /// </summary>
        /// <returns></returns>
        public static RoATPProvidersNotRefreshedReportViewModel Populate(this RoATPProvidersNotRefreshedReportViewModel model,
            ProviderPortalEntities db, bool deferredLoad)
        {
            if (deferredLoad)
            {
                model.Items = new List<RoATPProvidersNotRefreshedReportViewModelItem>();
                return model;
            }

            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = "exec dbo.[up_ReportRoATPProviderNotRefreshed] @StartDate, @EndDate";
            cmd.Parameters.Add(new SqlParameter("StartDate", model.StartDate));
            cmd.Parameters.Add(new SqlParameter("EndDate", model.EndDate));

            List<RoATPProvidersNotRefreshedReportViewModelItem> list = new List<RoATPProvidersNotRefreshedReportViewModelItem>();
            try
            {
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                list = ((IObjectContextAdapter)db)
                    .ObjectContext
                    .Translate<RoATPProvidersNotRefreshedReportViewModelItem>(reader).ToList();
            }
            finally
            {
                db.Database.Connection.Close();
            }

            model.Items = list;

            return model;
        }

        public static NewtonsoftJsonResult ToJsonResult(this RoATPProvidersNotRefreshedReportViewModel model)
        {
            String[][] data = model.Items.Select(x => new[]
            {
                x.ProviderId.ToString(),
                x.ProviderName,
                x.UKPRN.ToString(),
                x.RoATPStartDate.HasValue
                    ? x.RoATPStartDate.Value.ToString(Constants.ConfigSettings.ShortDateFormat)
                    : String.Empty,
                x.RoATPProviderType,
                x.ImportBatchName,
                x.TASRefreshOverride.HasValue ? x.TASRefreshOverride.Value.ToString() : "False"
            }).ToArray();

            return new NewtonsoftJsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    data = data
                }
            };
        }
    }


    #endregion

    #region Misc reports

    public static class UsageStatisticsReportViewModelExtensions
    {
        public static UsageStatisticsReportViewModel Populate(this UsageStatisticsReportViewModel model)
        {
            model.BaseUrl = Constants.ConfigSettings.UsageStatisticsVirtualDirectory;
            model.Items = UsageStatistics.GetAll();
            return model;
        }
    }

    #endregion

    #region Helpers

    public static class ReportViewModelHelpers
    {
        public static string ToShortLocalDateString(this DateTime? dateTime)
        {
            return dateTime == null
                ? String.Empty
                : dateTime.Value.ToLocalTime().ToString(Constants.ConfigSettings.ShortDateFormat);
        }

        public static string ToShortLocalDateString(this DateTime dateTime)
        {
            return dateTime.ToLocalTime().ToString(Constants.ConfigSettings.ShortDateFormat);
        }

        public static string ToShortLocalDateTimeString(this DateTime? dateTime)
        {
            return dateTime == null
                ? String.Empty
                : dateTime.Value.ToLocalTime().ToString(Constants.ConfigSettings.ShortDateTimeFormat);
        }

        public static string ToShortLocalDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToLocalTime().ToString(Constants.ConfigSettings.ShortDateTimeFormat);
        }

        public static string ToLongLocalDateString(this DateTime? dateTime)
        {
            return dateTime == null
                ? String.Empty
                : dateTime.Value.ToLocalTime().ToString(Constants.ConfigSettings.LongDateFormat);
        }

        public static string ToLongLocalDateString(this DateTime dateTime)
        {
            return dateTime.ToLocalTime().ToString(Constants.ConfigSettings.LongDateFormat);
        }

        public static string ToLongLocalDateTimeString(this DateTime? dateTime)
        {
            return dateTime == null
                ? String.Empty
                : dateTime.Value.ToLocalTime().ToString(Constants.ConfigSettings.LongDateTimeFormat);
        }

        public static string ToLongLocalDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToLocalTime().ToString(Constants.ConfigSettings.LongDateTimeFormat);
        }

        public static string ToFileSizeString(this int sizeInBytes)
        {
            var size = (decimal) sizeInBytes;
            var units = "bytes";
            if (size >= 1024)
            {
                size /= 1024m;
                units = "KB";
            }
            if (size > 1024)
            {
                size /= 1024m;
                units = "MB";
            }
            if (size > 1024)
            {
                size /= 1024m;
                units = "GB";
            }
            if (size > 1024)
            {
                size /= 1024m;
                units = "TB";
            }
            return String.Format("{0:0.##} {1}", size, units);
        }
    }

    #endregion
}