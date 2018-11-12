using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.DataWarehouse.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;
using WebSupergoo.ABCpdf9;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public class ReportController : BaseController
    {

        #region Index (Routing)

        //
        // GET: /Report/
        [ContextAuthorize(UserContext.UserContextName.AdministrationProviderOrganisation)]
        [PermissionAuthorize(new [] {Permission.PermissionName.CanViewAdminReports, Permission.PermissionName.CanViewOrganisationReports, Permission.PermissionName.CanViewProviderReports})]
        public ActionResult Index()
        {
            switch (userContext.ContextName)
            {
                case UserContext.UserContextName.Organisation:

                    return RedirectToAction("TrafficLight");

                case UserContext.UserContextName.Provider:

                    return RedirectToAction("Courses");

                case UserContext.UserContextName.Administration:

                    return RedirectToAction("ContractingBodies");

                default:
                    return HttpNotFound();
            }
        }

        #endregion

        #region Available to all users

        //
        // GET: /Report/UsageStatistics
        public ActionResult UsageStatistics()
        {
            var model = new UsageStatisticsReportViewModel();
            model.Populate();
            return View(model);
        }

        #endregion

        #region Admin Reports

        // GET /Report/ContractingBodies
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult ContractingBodies()
        {
            var model = new AdminReportMasterViewModel(true, true, true, true, false);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/ContractingBodiesJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult ContractingBodiesJson()
        {
            var model = new AdminReportMasterViewModel(true, true, true, true, false);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/ContractingBodies
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult DFEContractingBodies()
        {
            var model = new AdminReportMasterViewModel(true, true, true, false, true);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/ContractingBodiesJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult DFEContractingBodiesJson()
        {
            var model = new AdminReportMasterViewModel(true, true, true, false, true);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/Providers
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult Providers()
        {
            var model = new AdminReportMasterViewModel(true, false, false, true, false);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/ProvidersJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult ProvidersJson()
        {
            var model = new AdminReportMasterViewModel(true, false, false, true, false);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/Providers
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult DFEProviders()
        {
            var model = new AdminReportMasterViewModel(true, false, false, false, true);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/ProvidersJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult DFEProvidersJson()
        {
            var model = new AdminReportMasterViewModel(true, false, false, false, true);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/Organisations
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult Organisations()
        {
            var model = new AdminReportMasterViewModel(false, true, false, true, false);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/OrganisationsJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult OrganisationsJson()
        {
            var model = new AdminReportMasterViewModel(false, true, false, true, false);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/Organisations
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult DFEOrganisations()
        {
            var model = new AdminReportMasterViewModel(false, true, false, false, true);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/OrganisationsJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult DFEOrganisationsJson()
        {
            var model = new AdminReportMasterViewModel(false, true, false, false, true);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/ProvidersAndOrganisations
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult ProvidersAndOrganisations()
        {
            var model = new AdminReportMasterViewModel(true, true, false, true, false);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/ProvidersAndOrganisationsJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult ProvidersAndOrganisationsJson()
        {
            var model = new AdminReportMasterViewModel(true, true, false, true, false);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/ProvidersAndOrganisations
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult DFEProvidersAndOrganisations()
        {
            var model = new AdminReportMasterViewModel(true, true, false, false, true);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/ProvidersAndOrganisationsJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult DFEProvidersAndOrganisationsJson()
        {
            var model = new AdminReportMasterViewModel(true, true, false, false, true);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/DailyReport
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult DailyReport()
        {            
            var model = new DailyReportViewModel(true, false);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/DailyReportJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult DailyReportJson()
        {
            var model = new DailyReportViewModel(true, false);
            model.Populate(db, false);
            return model.ToSFAJsonResult();
        }

        // GET /Report/DFEDailyReport
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult DFEDailyReport()
        {
            var model = new DailyReportViewModel(false, true);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/DFEDailyReportJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult DFEDailyReportJson()
        {
            var model = new DailyReportViewModel(false, true);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/WeeklyReport
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult WeeklyReport()
        {
            var model = new WeeklyReportViewModel(true, false);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/WeeklyReportJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult WeeklyReportJson()
        {
            var model = new WeeklyReportViewModel(true, false);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/DFEWeeklyReport
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult DFEWeeklyReport()
        {
            var model = new WeeklyReportViewModel(false, true);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/DFEWeeklyReportJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult DFEWeeklyReportJson()
        {
            var model = new WeeklyReportViewModel(false, true);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/SFAWeeklyReport
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult SFAWeeklyReport()
        {
            var model = new SFAWeeklyReportViewModel(true, false);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/SFAWeeklyReportJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult SFAWeeklyReportJson()
        {
            var model = new SFAWeeklyReportViewModel(true, false);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/SFAWeeklyReport
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult DFESFAWeeklyReport()
        {
            var model = new SFAWeeklyReportViewModel(false, true);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/SFAWeeklyReportJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult DFESFAWeeklyReportJson()
        {
            var model = new SFAWeeklyReportViewModel(false, true);
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        [HttpGet]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult MonthlyReport()
        {
            ProviderPortalDataWarehouseEntities dataWarehouse = new ProviderPortalDataWarehouseEntities();

            MonthlyReportModel model = new MonthlyReportModel
            {
                PeriodType = "M"
            };

            model.Populate(dataWarehouse);

            Byte[] fileData;

            // Open the Excel Workbook
            using (ExcelPackage pck = new ExcelPackage())
            {
                StreamReader sr = new StreamReader(Constants.ConfigSettings.MonthlyReportExcelTemplateFilename);
                pck.Load(sr.BaseStream);
                sr.Close();

                String startDate = String.Empty;
                String endDate = String.Empty;
                if (model.Usage.Count() > 0)
                {
                    startDate = model.Usage.First().PeriodHeading;
                    endDate = model.Usage.Last().PeriodHeading;
                }

                ExcelWorksheet wsContents = pck.Workbook.Worksheets[1];
                wsContents.Cells["A1"].Value = String.Format(AppGlobal.Language.GetText(this, "ReportHeading", "Provider Portal Course Directory: {0} - {1}"), startDate, endDate);
                wsContents.Cells["K25"].Value = model.AverageQualityScore == null ? 0 : model.AverageQualityScore.Value / Convert.ToDecimal(100.0);
                wsContents.Cells["K25"].Style.Numberformat.Format = "#,##0.0%;-#,##0.0%;0%";

                ExcelWorksheet wsUsageData = pck.Workbook.Worksheets[8];
                Int32 column = 2;
                foreach (MonthlyReport_UsageModel usage in model.Usage)
                {
                    wsUsageData.Cells[1, column].Value = usage.PeriodHeading;
                    wsUsageData.Cells[2, column].Value = usage.NumberOfProvidersWithValidSuperUser;
                    wsUsageData.Cells[3, column].Value = usage.NumberOfProviders;
                    wsUsageData.Cells[4, column].Value = usage.NumberOfManuallyUpdatedOpportunitiesInPeriod;
                    wsUsageData.Cells[5, column].Value = usage.TotalBulkUploadOpportunities;
                    wsUsageData.Cells[6, column].Value = usage.NumberOfBulkUploadOpportunitiesInPeriod;
                    wsUsageData.Cells[7, column].Value = usage.TotalManuallyUpdatedOpportunities;
                    wsUsageData.Cells[8, column].Value = usage.NumberOfManuallyUpdatedOpportunitiesInPeriod;
                    wsUsageData.Cells[9, column].Value = usage.NumberOfProvidersNotUpdatedOpportunityInPastYear;
                    wsUsageData.Cells[11, column].Value = usage.NumberOfProvidersUpdatedDuringPeriod;
                    wsUsageData.Cells[12, column].Value = usage.NumberOfProvidersUpdated1to2PeriodsAgo;
                    wsUsageData.Cells[13, column].Value = usage.NumberOfProvidersUpdated2to3PeriodsAgo;
                    wsUsageData.Cells[14, column].Value = usage.NumberOfProvidersUpdatedMoreThan3PeriodsAgo;

                    column++;
                }

                ExcelWorksheet wsProvisionData = pck.Workbook.Worksheets[9];
                column = 2;
                foreach (MonthlyReport_ProvisionModel provision in model.Provision)
                {
                    wsProvisionData.Cells[2, column].Value = provision.NumberOfCourses;
                    wsProvisionData.Cells[3, column].Value = provision.NumberOfLiveCourses;
                    wsProvisionData.Cells[4, column].Value = provision.NumberOfOpportunities;
                    wsProvisionData.Cells[5, column].Value = provision.NumberOfLiveOpportunities;
                    wsProvisionData.Cells[6, column].Value = provision.ProvidersWithNoCourses;

                    wsProvisionData.Cells[9, column].Value = provision.SM_Flexible;
                    wsProvisionData.Cells[10, column].Value = provision.SM_FullTime;
                    wsProvisionData.Cells[11, column].Value = provision.SM_NotKnown;
                    wsProvisionData.Cells[12, column].Value = provision.SM_PartOfFullTimeProgramme;
                    wsProvisionData.Cells[13, column].Value = provision.SM_PartTime;

                    wsProvisionData.Cells[16, column].Value = provision.AM_DistanceWithAttendance;
                    wsProvisionData.Cells[17, column].Value = provision.AM_DistanceWithoutAttendance;
                    wsProvisionData.Cells[18, column].Value = provision.AM_FaceToFace;
                    wsProvisionData.Cells[19, column].Value = provision.AM_LocationCampus;
                    wsProvisionData.Cells[20, column].Value = provision.AM_MixedMode;
                    wsProvisionData.Cells[21, column].Value = provision.AM_NotKnown;
                    wsProvisionData.Cells[22, column].Value = provision.AM_OnlineWithAttendance;
                    wsProvisionData.Cells[23, column].Value = provision.AM_OnlineWithoutAttendance;
                    wsProvisionData.Cells[24, column].Value = provision.AM_WorkBased;

                    wsProvisionData.Cells[27, column].Value = provision.AP_Customised;
                    wsProvisionData.Cells[28, column].Value = provision.AP_DayBlockRelease;
                    wsProvisionData.Cells[29, column].Value = provision.AP_Daytime;
                    wsProvisionData.Cells[30, column].Value = provision.AP_Evening;
                    wsProvisionData.Cells[31, column].Value = provision.AP_NotApplicable;
                    wsProvisionData.Cells[32, column].Value = provision.AP_NotKnown;
                    wsProvisionData.Cells[33, column].Value = provision.AP_Twilight;
                    wsProvisionData.Cells[34, column].Value = provision.AP_Weekend;

                    wsProvisionData.Cells[37, column].Value = provision.DU_1WeekOrLess;
                    wsProvisionData.Cells[38, column].Value = provision.DU_1To4Weeks;
                    wsProvisionData.Cells[39, column].Value = provision.DU_1To3Months;
                    wsProvisionData.Cells[40, column].Value = provision.DU_3To6Months;
                    wsProvisionData.Cells[41, column].Value = provision.DU_6To12Months;
                    wsProvisionData.Cells[42, column].Value = provision.DU_1To2Years;
                    wsProvisionData.Cells[43, column].Value = provision.DU_NotKnown;

                    wsProvisionData.Cells[46, column].Value = provision.QT_14To19Diploma;
                    wsProvisionData.Cells[47, column].Value = provision.QT_AccessToHigherEducation;
                    wsProvisionData.Cells[48, column].Value = provision.QT_Apprenticeship;
                    wsProvisionData.Cells[49, column].Value = provision.QT_BasicKeySkill;
                    wsProvisionData.Cells[50, column].Value = provision.QT_CertificateOfAttendance;
                    wsProvisionData.Cells[51, column].Value = provision.QT_CourseProviderCertificate;
                    wsProvisionData.Cells[52, column].Value = provision.QT_ExternalAwardedQualification;
                    wsProvisionData.Cells[53, column].Value = provision.QT_FoundationalDegree;
                    wsProvisionData.Cells[54, column].Value = provision.QT_FunctionalSkill;
                    wsProvisionData.Cells[55, column].Value = provision.QT_GCEOrEquivalent;
                    wsProvisionData.Cells[56, column].Value = provision.QT_GCSEOrEquivalent;
                    wsProvisionData.Cells[57, column].Value = provision.QT_HncHnd;
                    wsProvisionData.Cells[58, column].Value = provision.QT_InternationalBacculaureate;
                    wsProvisionData.Cells[59, column].Value = provision.QT_NoQualification;
                    wsProvisionData.Cells[60, column].Value = provision.QT_NVQ;
                    wsProvisionData.Cells[61, column].Value = provision.QT_OtherAccreditedQualification;
                    wsProvisionData.Cells[62, column].Value = provision.QT_Postgraduate;
                    wsProvisionData.Cells[63, column].Value = provision.QT_Undergraduate;
                    wsProvisionData.Cells[64, column].Value = provision.QT_IndustrySpecificQualification;

                    wsProvisionData.Cells[67, column].Value = provision.QL_EntryLevel;
                    wsProvisionData.Cells[68, column].Value = provision.QL_HigherLevel;
                    wsProvisionData.Cells[69, column].Value = provision.QL_Level1;
                    wsProvisionData.Cells[70, column].Value = provision.QL_Level2;
                    wsProvisionData.Cells[71, column].Value = provision.QL_Level3;
                    wsProvisionData.Cells[72, column].Value = provision.QL_Level4;
                    wsProvisionData.Cells[73, column].Value = provision.QL_Level5;
                    wsProvisionData.Cells[74, column].Value = provision.QL_Level6;
                    wsProvisionData.Cells[75, column].Value = provision.QL_Level7;
                    wsProvisionData.Cells[76, column].Value = provision.QL_Level8;
                    wsProvisionData.Cells[77, column].Value = provision.QL_NotKnown;

                    column++;
                }

                ExcelWorksheet wsQualityData = pck.Workbook.Worksheets[10];
                column = 2;
                foreach (MonthlyReport_QualityModel quality in model.Quality)
                {
                    wsQualityData.Cells[2, column].Value = quality.Poor;
                    wsQualityData.Cells[3, column].Value = quality.Average;
                    wsQualityData.Cells[4, column].Value = quality.Good;
                    wsQualityData.Cells[5, column].Value = quality.VeryGood;

                    column++;
                }

                // Set "Best Fit" for all data labels in all Pie Charts
                foreach (ExcelWorksheet ws in pck.Workbook.Worksheets)
                {
                    foreach (ExcelDrawing drawing in ws.Drawings)
                    {
                        if (drawing is ExcelPieChart chart)
                        {
                            foreach (ExcelPieChartSerie series in chart.Series)
                            {
                                series.DataLabel.Position = eLabelPosition.BestFit;
                            }
                        }
                    }
                }

                fileData = pck.GetAsByteArray();
            }

            return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MonthlyReport.xlsx");
        }

        [HttpGet]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult DFEStartDateReport()
        {
            ProviderPortalDataWarehouseEntities dataWarehouse = new ProviderPortalDataWarehouseEntities();

            DFEStartDateReportModel model = new DFEStartDateReportModel();
            

            model.Populate(dataWarehouse);

            Byte[] fileData;

            // Open the Excel Workbook
            using (ExcelPackage pck = new ExcelPackage())
            {
                StreamReader sr = new StreamReader(Constants.ConfigSettings.DFEStartDateReportExcelTemplateFilename);
                pck.Load(sr.BaseStream);
                sr.Close();

                
                ExcelWorksheet wsData = pck.Workbook.Worksheets[2];
                Int32 column = 2;
                foreach (DFEStartDateReport_COModel usage in model.Usage)
                {
                    wsData.Cells[10, column].Value = usage.Period;
                    wsData.Cells[11, column].Value = usage.NumberOfLiveOpportunities;
                    wsData.Cells[12, column].Value = usage.NumberOfLiveCourses;
                    
                    column++;
                }

                column = 2;
                foreach (MonthlyReport_QualityModel quality in model.Quality.Reverse<MonthlyReport_QualityModel>())
                {
                    wsData.Cells[3, column].Value = quality.Poor;
                    wsData.Cells[4, column].Value = quality.Average;
                    wsData.Cells[5, column].Value = quality.Good;
                    wsData.Cells[6, column].Value = quality.VeryGood;

                    column++;
                }

                // Average Quality score

                wsData.Cells[25, 2].Value = model.AverageQualityScore;

                // Total providers

                wsData.Cells[18, 2].Value = model.TotalProviders;

                // Total providers With Zero Courses

                wsData.Cells[20, 2].Value = model.TotalProvidersWithZeroCourses;

                column = 2;
                foreach (MonthlyReport_QualityModel dfeQuality in model.DFEQuality)
                {
                    wsData.Cells[21, column].Value = dfeQuality.Poor;
                    wsData.Cells[22, column].Value = dfeQuality.Average;
                    wsData.Cells[23, column].Value = dfeQuality.Good;
                    wsData.Cells[24, column].Value = dfeQuality.VeryGood;

                    column++;
                }

                column = 3;
                foreach (MonthlyReport_QualityModel dfeSfaQuality in model.DFESFAQuality)
                {
                    wsData.Cells[21, column].Value = dfeSfaQuality.Poor;
                    wsData.Cells[22, column].Value = dfeSfaQuality.Average;
                    wsData.Cells[23, column].Value = dfeSfaQuality.Good;
                    wsData.Cells[24, column].Value = dfeSfaQuality.VeryGood;

                    column++;
                }

                wsData.Cells[19, 2].Value = model.TotalProvidersUpdated;

                wsData.Cells[16, 2].Value = model.FundedCoursesUploaded;

                wsData.Cells[17, 2].Value = model.FundedCoursesUploadedPostSept;

                wsData.Cells[29, 2].Value = model.AcademicYearNext.Date;

                wsData.Cells[30, 2].Value = DateTime.Now.Date;


                fileData = pck.GetAsByteArray();
            }

            return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DFEStartDateReport.xlsx");
        }

        // GET /Report/SalesForceAccounts
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult SalesForceAccounts()
        {
            var model = new SalesForceAccountsViewModel();
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/SalesForceAccountsJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult SalesForceAccountsJson()
        {
            var model = new SalesForceAccountsViewModel();
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/SalesForceContacts
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult SalesForceContacts()
        {
            var model = new SalesForceContactsViewModel();
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/SalesForceContactsJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult SalesForceContactsJson()
        {
            var model = new SalesForceContactsViewModel();
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/BulkUploadHistory
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult BulkUploadHistory()
        {
            var model = new BulkUploadHistoryReportViewModel()
            {
                StartDate = DateTime.UtcNow.AddDays(-7).Date,
                EndDate = DateTime.UtcNow.Date
            };
            model.Populate(db, true);
            return View(model);
        }

        // POST /Report/BulkUploadHistory
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult BulkUploadHistory(BulkUploadHistoryReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Populate(db, true);
            }
            if (model.Items == null)
            {
                model.Items = new List<BulkUploadHistoryReportViewModelItem>();
            }
            return View(model);
        }

        // GET /Report/BulkUploadHistoryJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult BulkUploadHistoryJson(BulkUploadHistoryReportViewModel model = null)
        {
            if (model == null || model.StartDate == null || model.EndDate == null)
            {
                model = new BulkUploadHistoryReportViewModel
                {
                    StartDate = DateTime.UtcNow.AddDays(-7).Date,
                    EndDate = DateTime.UtcNow.Date
                };
            }
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/BulkUploadHistoryApprentice
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult BulkUploadHistoryApprentice()
        {
            var model = new BulkUploadHistoryApprenticeReportViewModel()
            {
                StartDate = DateTime.UtcNow.AddDays(-7).Date,
                EndDate = DateTime.UtcNow.Date
            };
            model.Populate(db, true);
            return View(model);
        }

        // POST /Report/BulkUploadHistory
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult BulkUploadHistoryApprentice(BulkUploadHistoryApprenticeReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Populate(db, true);
            }
            if (model.Items == null)
            {
                model.Items = new List<BulkUploadHistoryApprenticeReportViewModelItem>();
            }
            return View(model);
        }

        // GET /Report/BulkUploadHistoryJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult BulkUploadHistoryApprenticeJson(BulkUploadHistoryApprenticeReportViewModel model = null)
        {
            if (model == null || model.StartDate == null || model.EndDate == null)
            {
                model = new BulkUploadHistoryApprenticeReportViewModel
                {
                    StartDate = DateTime.UtcNow.AddDays(-7).Date,
                    EndDate = DateTime.UtcNow.Date
                };
            }
            model.Populate(db, false);
            return model.ToJsonResult();
        }



        // GET /Report/BulkUploadHistory
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult QualityEmailHistory()
        {
            var model = new QualityEmailHistoryReportViewModel
            {
                StartDate = DateTime.UtcNow.AddDays(-7).Date,
                EndDate = DateTime.UtcNow.Date
            };
            model.Populate(db, true);
            return View(model);
        }

        // POST /Report/BulkUploadHistory
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult QualityEmailHistory(QualityEmailHistoryReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Populate(db, true);
            }
            if (model.Items == null)
            {
                model.Items = new List<QualityEmailHistoryReportViewModelItem>();
            }
            return View(model);
        }

        // GET /Report/BulkUploadHistoryJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult QualityEmailHistoryJson(QualityEmailHistoryReportViewModel model = null)
        {
            if (model == null || model.StartDate == null || model.EndDate == null)
            {
                model = new QualityEmailHistoryReportViewModel
                {
                    StartDate = DateTime.UtcNow.AddDays(-7).Date,
                    EndDate = DateTime.UtcNow.Date
                };
            }
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/MetadataUploadHistory
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult MetadataUploadHistory()
        {
            var model = new MetadataUploadHistoryReportViewModel();
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/MetadataUploadHistoryJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public NewtonsoftJsonResult MetadataUploadHistoryJson()
        {
            var model = new MetadataUploadHistoryReportViewModel();
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        #endregion

        #region Apprenticeship (RoATP) Reports

        // GET /Report/ApprenticeshipOverallCountReport
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult ApprenticeshipOverallCountReport()
        {
            var model = new ApprenticeshipOverallCountReportViewModel();
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/ApprenticeshipOverallCountReportJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public NewtonsoftJsonResult ApprenticeshipOverallCountReportJson()
        {
            var model = new ApprenticeshipOverallCountReportViewModel();
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/ApprenticeshipDetailedListUploadedReport
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult ApprenticeshipDetailedListUploadedReport()
        {
            var model = new ApprenticeshipDetailedListUploadedReportViewModel();
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/ApprenticeshipDetailedListUploadedReportJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public NewtonsoftJsonResult ApprenticeshipDetailedListUploadedReportJson()
        {
            var model = new ApprenticeshipDetailedListUploadedReportViewModel();
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/ProvidersWithOver10ApprenticeshipsReport
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult ProvidersWithOver10ApprenticeshipsReport()
        {
            var model = new ProvidersWithOver10ApprenticeshipsReportViewModel();
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/ProvidersWithOver10ApprenticeshipsReportJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public NewtonsoftJsonResult ProvidersWithOver10ApprenticeshipsReportJson()
        {
            var model = new ProvidersWithOver10ApprenticeshipsReportViewModel();
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/QAdProvidersReport
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult QAdProvidersReport()
        {
            var model = new QAdProvidersReportViewModel();
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/QAdProvidersReportJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public NewtonsoftJsonResult QAdProvidersReportJson()
        {
            var model = new QAdProvidersReportViewModel();
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/ProvidersSubmittedDataForQAReport
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult ProvidersSubmittedDataForQAReport()
        {
            var model = new ProvidersSubmittedDataForQAReportViewModel();
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/ProvidersSubmittedDataForQAReportJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public NewtonsoftJsonResult ProvidersSubmittedDataForQAReportJson()
        {
            var model = new ProvidersSubmittedDataForQAReportViewModel();
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/RegisterOpening
        [HttpGet]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult RegisterOpening()
        {
            ImportBatch importBatch = db.ImportBatches.Where(x => x.Current == true).FirstOrDefault();
            if (importBatch == null)
            {
                importBatch = db.ImportBatches.OrderByDescending(x => x.ImportBatchId).FirstOrDefault();
            }
            if (importBatch == null)
            {
                return HttpNotFound();
            }

            RegisterOpeningReportViewModel model = new RegisterOpeningReportViewModel
            {
                ImportBatchId = importBatch.ImportBatchId
            };

            ViewBag.ShowResults = false;
            GetRegisterOpeningLookups(model);
            model.Populate(db, true);
            return View(model);
        }

        // POST /Report/RegisterOpening
        [HttpPost]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult RegisterOpening(RegisterOpeningReportViewModel model)
        {
            ViewBag.ShowResults = true;
            GetRegisterOpeningLookups(model);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/RegisterOpeningJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public NewtonsoftJsonResult RegisterOpeningJson(Int32? importBatchId)
        {
            RegisterOpeningReportViewModel model = new RegisterOpeningReportViewModel
            {
                ImportBatchId = importBatchId
            };
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        [NonAction]
        private void GetRegisterOpeningLookups(RegisterOpeningReportViewModel model)
        {
            ViewBag.ImportBatches = new SelectList(db.ImportBatches, "ImportBatchId", "ImportBatchName", model.ImportBatchId);
        }

        // GET /Report/RegisterOpeningDetail
        [HttpGet]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult RegisterOpeningDetail(Int32? importBatchId)
        {
            Int32 batchId;
            if (importBatchId.HasValue)
            {
                batchId = importBatchId.Value;
                if (batchId != -1)
                {
                    ImportBatch importBatch = db.ImportBatches.Find(batchId);
                    if (importBatch == null)
                    {
                        return HttpNotFound();
                    }
                }
            }
            else
            {
                ImportBatch importBatch = db.ImportBatches.Where(x => x.Current == true).FirstOrDefault();
                if (importBatch == null)
                {
                    importBatch = db.ImportBatches.OrderByDescending(x => x.ImportBatchId).FirstOrDefault();
                }
                if (importBatch == null)
                {
                    return HttpNotFound();
                }
                batchId = importBatch.ImportBatchId;
            }
            RegisterOpeningDetailReportViewModel model = new RegisterOpeningDetailReportViewModel
            {
                ImportBatchId = batchId
            };

            ViewBag.ShowResults = importBatchId.HasValue;
            GetRegisterOpeningDetailLookups(model);
            model.Populate(db, true);
            return View(model);
        }

        // POST /Report/RegisterOpeningDetail
        [HttpPost]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult RegisterOpeningDetail(RegisterOpeningDetailReportViewModel model)
        {
            ViewBag.ShowResults = true;
            GetRegisterOpeningDetailLookups(model);
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/RegisterOpeningDetailJson
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public NewtonsoftJsonResult RegisterOpeningDetailJson(Int32? importBatchId)
        {
            RegisterOpeningDetailReportViewModel model = new RegisterOpeningDetailReportViewModel
            {
                ImportBatchId = importBatchId
            };
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        [NonAction]
        private void GetRegisterOpeningDetailLookups(RegisterOpeningDetailReportViewModel model)
        {
            ViewBag.ImportBatches = new SelectList(db.ImportBatches, "ImportBatchId", "ImportBatchName", model.ImportBatchId);
        }

        #endregion

        #region Provider Reports

        // GET /Report/Courses
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderReports)]
        public ActionResult Courses()
        {
            if (!userContext.ItemId.HasValue)
            {
                return HttpNotFound();
            }
            var model = new ProviderCourseReportViewModel(userContext.ItemId.Value);
            model.Populate(db);
            return View(model);
        }

        // GET /Report/Venues
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderReports)]
        public ActionResult Venues()
        {
            if (!userContext.ItemId.HasValue)
            {
                return HttpNotFound();
            }
            var model = new ProviderVenueReportViewModel(userContext.ItemId.Value);
            model.Populate(db);
            return View(model);
        }

        // GET /Report/Opportunities
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderReports)]
        public ActionResult Opportunities()
        {
            if (!userContext.ItemId.HasValue)
            {
                return HttpNotFound();
            }
            var model = new ProviderOpportunityReportViewModel(userContext.ItemId.Value);
            model.Populate(db);
            return View(model);
        }

        // GET /Report/Dashboard
        // 2.1.9.0 Dashboard is now to be used as the home page.
        [ContextAuthorize(UserContext.UserContextName.AdministrationProvider)]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderHomePage)]
        public ActionResult Dashboard(int? id = null, bool recalculate = false)
        {
            var providerId = GetProviderId(id);
            if (!providerId.HasValue)
            {
                return HttpNotFound();
            }
            if (recalculate &&
                Permission.HasPermission(false, true, Permission.PermissionName.CanRecalculateQualityScores))
            {
                // TODO move to POST action
                var oldTimeout = ((IObjectContextAdapter) db).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter) db).ObjectContext.CommandTimeout = 540;
                db.up_ProviderUpdateQualityScore(providerId, true);
                ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = oldTimeout;
                return BounceToDashboard(providerId);
            }
            var model = new ProviderDashboardReportViewModel(providerId.Value);
            model = model.Populate(db);
            QualityIndicator.UpdateSessionQualityInformation(model.LastActivity, model.LastUpdatingDateTimeUtc, model.AutoAggregateQualityRating.Value);
            return View(model);
        }

        // GET /Report/DashboardRecalculate
        [ContextAuthorize(UserContext.UserContextName.AdministrationProvider)]
        [PermissionAuthorize(Permission.PermissionName.CanRecalculateQualityScores)]
        public ActionResult DashboardRecalculate(int? id = null)
        {
            var providerId = GetProviderId(id);
            if (!providerId.HasValue)
            {
                return HttpNotFound();
            }
            var oldTimeout = ((IObjectContextAdapter) db).ObjectContext.CommandTimeout;
            ((IObjectContextAdapter) db).ObjectContext.CommandTimeout = 540;
            db.up_ProviderUpdateQualityScore(providerId.Value, true);
            ((IObjectContextAdapter) db).ObjectContext.CommandTimeout = oldTimeout;
            return BounceToDashboard(providerId);
        }

        // GET /Report/DashboardPdf
        [ContextAuthorize(UserContext.UserContextName.AdministrationProvider)]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderReports)]
        public ActionResult DashboardPdf(int? id = null)
        {
            var providerId = GetProviderId(id);
            if (!providerId.HasValue)
            {
                return HttpNotFound();
            }
            XSettings.InstallLicense("[X/VKS08wmMhAtn4jNv3DOcikae8bCdcYlqznwb2"
                                     + "zWBhjkArDfDm+CqofKHk/hAUJDskx+yNGp0QVyqg"
                                     + "hT758yE0kam2dBHlV42ERQ8qoKE7g5QnEoIE6faK"
                                     + "xXECzZmgzRqd0V89ZskpwdxB7oTWBPk1TZzqRSWX"
                                     + "q5eQ37UYvXPvVwaEo8hjk53PKsLYo4xpRYkwu7mB"
                                     + "fwH33eSQrDutd839tn1zTNPdAGcGYM1aRXqOCdgY"
                                     + "S27M=]");
            var guid = Guid.NewGuid().ToString();

            string key = "Report_DashboardPdf_" + guid;
            string nameKey = key + "_Name";
            CacheManagement.CacheHandler.Add(key, providerId);
            
            string url = Url.Action("DashboardPdfInternal", "Report",
                new {id = guid},
                Request.Url.Scheme);
            byte[] theData;
            string providerName;
            using (Doc theDoc = new Doc())
            {
                theDoc.MediaBox.String = "A4";
                theDoc.Rect.String = "A4";
                theDoc.Font = theDoc.AddFont("Arial", LanguageType.Unicode);
                theDoc.HtmlOptions.UseScript = true;
                theDoc.HtmlOptions.OnLoadScript = "(function(){ window.external.ABCpdf_RenderWait(); setInterval(function(){if($('html').hasClass('rendered')){ window.external.ABCpdf_RenderComplete();}}, 500); })();";
                theDoc.HtmlOptions.Engine = EngineType.MSHtml;

                var theId = theDoc.AddImageUrl(url, true/*paged*/, 1250/*width*/, true/*disableCache*/);
                while (true)
                {
                    //theDoc.FrameRect();
                    if (!theDoc.Chainable(theId))
                        break;
                    theDoc.Page = theDoc.AddPage();
                    theId = theDoc.AddImageToChain(theId);
                }
                
                for (int i = 1; i <= theDoc.PageCount; i++)
                {
                    theDoc.PageNumber = i;
                    theDoc.Flatten();
                }

                providerName = (String) CacheManagement.CacheHandler.Get(nameKey);

                int theID = theDoc.AddObject("<< >>");
                theDoc.SetInfo(-1, "/Info:Ref", theID.ToString());
                theDoc.SetInfo(theID, "/Title:Text", String.Format("{0} - Quality Dashboard", providerName));
                theDoc.SetInfo(theID, "/Author:Text", "National Careers Service Course Directory Provider Portal");
                theDoc.SetInfo(theID, "/Subject:Text", String.Format("National Careers Service Provider Portal Quality Dashboard for {0}", providerName));
                theDoc.SetInfo(theID, "/Keywords:Text", "Quality Dashboard");
                theDoc.SetInfo(theID, "/Creator:Text", "National Careers Service Course Directory Provider Portal");
                var theDate = String.Format("D:{0}", DateTime.UtcNow.ToString("yyyyMMddHHmmssZ"));
                theDoc.SetInfo(theID, "/CreationDate:Text", theDate);
                theDoc.SetInfo(theID, "/ModDate:Text", theDate);
                theData = theDoc.GetData();
            }

            CacheManagement.CacheHandler.Invalidate(key);
            var fileName = providerName + " - Quality Dashboard.pdf";
            foreach (var c in Path.GetInvalidFileNameChars()) { fileName = fileName.Replace(c, '-'); }
            Response.AddHeader("content-disposition", "attachment; filename=\""+fileName+"\"");
            return new FileContentResult(theData, "application/pdf");
        }

        // GET /Report/DashboardPdf
        [AllowAnonymous]
        public ActionResult DashboardPdfInternal(string id)
        {
            if (id.Length > 10)
            {
                Guid temp;
                if (String.IsNullOrEmpty(id) || !Guid.TryParse(id, out temp))
                {
                    return HttpNotFound();
                }

                string key = "Report_DashboardPdf_" + id;
                object obj = CacheManagement.CacheHandler.Get(key);
                if (obj == null)
                {
                    return HttpNotFound();
                }

                var model = new ProviderDashboardReportViewModel((int) obj);
                model = model.Populate(db);
                model.RenderForPdf = true;
                string nameKey = key + "_Name";
                CacheManagement.CacheHandler.Add(nameKey, model.ProviderName);
                return View("Dashboard", model);
            }

            int entityId;
            if (Debugger.IsAttached && int.TryParse(id, out entityId))
            {
                var model = new ProviderDashboardReportViewModel(entityId);
                model = model.Populate(db);
                model.RenderForPdf = true;
                return View("Dashboard", model);
            }

            return HttpNotFound();
        }


        [HttpGet]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult ProvidersWithArchivedApprenticeships()
        {
            ProvidersWithArchivedApprenticeshipsReportViewModel model = new ProvidersWithArchivedApprenticeshipsReportViewModel
            {
                StartDate = DateTime.Today.AddMonths(-1),
                EndDate = DateTime.Today
            };

            ViewBag.ShowResults = true;
            model.Populate(db, true);

            return View(model);
        }

        [HttpPost]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        [ValidateAntiForgeryToken]
        public ActionResult ProvidersWithArchivedApprenticeships(ProvidersWithArchivedApprenticeshipsReportViewModel model)
        {
            if (model.StartDate == DateTime.MinValue)
            {
                model.StartDate = DateTime.Today.AddMonths(-1);
            }
            if (model.EndDate == DateTime.MinValue)
            {
                model.EndDate = DateTime.Today;
            }

            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateAfterEndDate", "Start Date cannot be after End Date"));
            }

            if (ModelState.IsValid)
            {
                // Get the data
                model.Populate(db, true);
            }

            ViewBag.ShowResults = ModelState.IsValid;

            return View(model);
        }

        [HttpGet]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult ProvidersWithArchivedApprenticeshipsJson(DateTime startDate, DateTime endDate)
        {
            ProvidersWithArchivedApprenticeshipsReportViewModel model = new ProvidersWithArchivedApprenticeshipsReportViewModel
            {
                StartDate = startDate,
                EndDate = endDate
            };

            if (model.StartDate == DateTime.MinValue)
            {
                model.StartDate = DateTime.Today.AddMonths(-1);
            }
            if (model.EndDate == DateTime.MinValue)
            {
                model.EndDate = DateTime.Today;
            }

            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateAfterEndDate", "Start Date cannot be after End Date"));
            }

            if (ModelState.IsValid)
            {
                // Get the data
                model.Populate(db, false);
                return model.ToJsonResult();
            }

            return HttpNotFound();
        }

        [HttpGet]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult ProvidersWithArchivedApprenticeshipsDetailed()
        {
            ProvidersWithArchivedApprenticeshipsDetailedReportViewModel model = new ProvidersWithArchivedApprenticeshipsDetailedReportViewModel
            {
                StartDate = DateTime.Today.AddMonths(-1),
                EndDate = DateTime.Today
            };

            ViewBag.ShowResults = true;
            model.Populate(db, true);

            return View(model);
        }

        [HttpPost]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        [ValidateAntiForgeryToken]
        public ActionResult ProvidersWithArchivedApprenticeshipsDetailed(ProvidersWithArchivedApprenticeshipsDetailedReportViewModel model)
        {
            if (model.StartDate == DateTime.MinValue)
            {
                model.StartDate = DateTime.Today.AddMonths(-1);
            }
            if (model.EndDate == DateTime.MinValue)
            {
                model.EndDate = DateTime.Today;
            }

            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateAfterEndDate", "Start Date cannot be after End Date"));
            }

            if (ModelState.IsValid)
            {
                // Get the data
                model.Populate(db, true);
            }

            ViewBag.ShowResults = ModelState.IsValid;

            return View(model);
        }

        [HttpGet]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult ProvidersWithArchivedApprenticeshipsDetailedJson(DateTime startDate, DateTime endDate)
        {
            ProvidersWithArchivedApprenticeshipsDetailedReportViewModel model = new ProvidersWithArchivedApprenticeshipsDetailedReportViewModel
            {
                StartDate = startDate,
                EndDate = endDate
            };

            if (model.StartDate == DateTime.MinValue)
            {
                model.StartDate = DateTime.Today.AddMonths(-1);
            }
            if (model.EndDate == DateTime.MinValue)
            {
                model.EndDate = DateTime.Today;
            }

            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateAfterEndDate", "Start Date cannot be after End Date"));
            }

            if (ModelState.IsValid)
            {
                // Get the data
                model.Populate(db, false);
                return model.ToJsonResult();
            }

            return HttpNotFound();
        }



        [HttpGet]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult RoATPProvidersRefreshed()
        {
            RoATPProvidersRefreshedReportViewModel model = new RoATPProvidersRefreshedReportViewModel
            {
                StartDate = Constants.ConfigSettings.RoATPRefreshStartDate,
                EndDate = Constants.ConfigSettings.RoATPRefreshEndDate
            };

            ViewBag.ShowResults = true;
            model.Populate(db, true);

            return View(model);
        }

        [HttpPost]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        [ValidateAntiForgeryToken]
        public ActionResult RoATPProvidersRefreshed(RoATPProvidersRefreshedReportViewModel model)
        {
            if (model.StartDate == DateTime.MinValue)
            {
                model.StartDate = Constants.ConfigSettings.RoATPRefreshStartDate;
            }
            if (model.EndDate == DateTime.MinValue)
            {
                model.EndDate = Constants.ConfigSettings.RoATPRefreshEndDate;
            }

            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateAfterEndDate", "Start Date cannot be after End Date"));
            }

            if (ModelState.IsValid)
            {
                // Get the data
                model.Populate(db, true);
            }

            ViewBag.ShowResults = ModelState.IsValid;

            return View(model);
        }

        [HttpGet]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult RoATPProvidersRefreshedJson(DateTime startDate, DateTime endDate)
        {
            RoATPProvidersRefreshedReportViewModel model = new RoATPProvidersRefreshedReportViewModel
            {
                StartDate = startDate,
                EndDate = endDate
            };

            if (model.StartDate == DateTime.MinValue)
            {
                model.StartDate = Constants.ConfigSettings.RoATPRefreshStartDate;
            }
            if (model.EndDate == DateTime.MinValue)
            {
                model.EndDate = Constants.ConfigSettings.RoATPRefreshEndDate;
            }

            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateAfterEndDate", "Start Date cannot be after End Date"));
            }

            if (ModelState.IsValid)
            {
                // Get the data
                model.Populate(db, false);
                return model.ToJsonResult();
            }

            return HttpNotFound();
        }




        [HttpGet]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult RoATPProvidersNotRefreshed()
        {
            RoATPProvidersNotRefreshedReportViewModel model = new RoATPProvidersNotRefreshedReportViewModel
            {
                StartDate = Constants.ConfigSettings.RoATPRefreshStartDate,
                EndDate = Constants.ConfigSettings.RoATPRefreshEndDate
            };

            ViewBag.ShowResults = true;
            model.Populate(db, true);

            return View(model);
        }

        [HttpPost]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        [ValidateAntiForgeryToken]
        public ActionResult RoATPProvidersNotRefreshed(RoATPProvidersNotRefreshedReportViewModel model)
        {
            if (model.StartDate == DateTime.MinValue)
            {
                model.StartDate = Constants.ConfigSettings.RoATPRefreshStartDate;
            }
            if (model.EndDate == DateTime.MinValue)
            {
                model.EndDate = Constants.ConfigSettings.RoATPRefreshEndDate;
            }

            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateAfterEndDate", "Start Date cannot be after End Date"));
            }

            if (ModelState.IsValid)
            {
                // Get the data
                model.Populate(db, true);
            }

            ViewBag.ShowResults = ModelState.IsValid;

            return View(model);
        }

        [HttpGet]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult RoATPProvidersNotRefreshedJson(DateTime startDate, DateTime endDate)
        {
            RoATPProvidersNotRefreshedReportViewModel model = new RoATPProvidersNotRefreshedReportViewModel
            {
                StartDate = startDate,
                EndDate = endDate
            };

            if (model.StartDate == DateTime.MinValue)
            {
                model.StartDate = Constants.ConfigSettings.RoATPRefreshStartDate;
            }
            if (model.EndDate == DateTime.MinValue)
            {
                model.EndDate = Constants.ConfigSettings.RoATPRefreshEndDate;
            }

            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateAfterEndDate", "Start Date cannot be after End Date"));
            }

            if (ModelState.IsValid)
            {
                // Get the data
                model.Populate(db, false);
                return model.ToJsonResult();
            }

            return HttpNotFound();
        }





        [NonAction]
        private int? GetProviderId(int? id)
        {
            return userContext.IsAdministration()
                ? (id.HasValue && db.Providers.Any(p => p.ProviderId == id.Value)
                    ? id
                    : null)
                : userContext.ItemId;
        }

        [NonAction]
        private ActionResult BounceToDashboard(int? providerId)
        {
            return RedirectToAction("Dashboard", new {id = userContext.IsAdministration() ? providerId : null});
        }

        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports, Permission.PermissionName.CanViewProviderQA)]
        [ContextAuthorize(UserContext.UserContextName.AdministrationProvider)]
        public ActionResult ProviderQAHistory()
        {
            ProviderQAHistoryReportViewModel model;
            if (userContext.IsAdministration())
            {
                model = new ProviderQAHistoryReportViewModel();
                model.Populate(db);
            }
            else if (userContext.IsProvider() && userContext.ItemId.HasValue)
            {
                model = new ProviderQAHistoryReportViewModel(userContext.ItemId.Value);
                model.Populate(db, userContext.ItemId.Value);
            }
            else
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET /Report/UnableToComplete
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult UnableToComplete()
        {
            var model = new ProviderUnableToCompleteHistoryReportViewModel();
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/UnableToComplete
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewApprenticeshipReports)]
        public ActionResult UnableToCompleteJson()
        {

            var model = new ProviderUnableToCompleteHistoryReportViewModel();
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        // GET /Report/UnableToComplete
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult HeadlineStats()
        {
            var model = new HeadlineStatsReportViewModel();
            model.Populate(db, true);
            return View(model);
        }

        // GET /Report/UnableToComplete
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        [PermissionAuthorize(Permission.PermissionName.CanViewAdminReports)]
        public ActionResult HeadlineStatsJson()
        {

            var model = new HeadlineStatsReportViewModel();
            model.Populate(db, false);
            return model.ToJsonResult();
        }

        #endregion

        #region Organisation Reports

        // GET /Report/TrafficLight
        [ContextAuthorize(UserContext.UserContextName.Organisation)]
        [PermissionAuthorize(Permission.PermissionName.CanViewOrganisationReports)]
        public ActionResult TrafficLight()
        {
            if (!userContext.ItemId.HasValue)
            {
                return HttpNotFound();
            }
            var model = new OrganisationTrafficLightReportViewModel(userContext.ItemId.Value);
            model.Populate(db);
            return View(model);
        }

        // GET /Report/DashboardRecalculate
        [ContextAuthorize(UserContext.UserContextName.Organisation)]
        [PermissionAuthorize(Permission.PermissionName.CanRecalculateQualityScores)]
        public ActionResult TrafficLightRecalculate()
        {
            if (!userContext.ItemId.HasValue)
            {
                return HttpNotFound();
            }
            var organisation = db.Organisations
                .FirstOrDefault(x => x.OrganisationId == userContext.ItemId.Value);
            if (organisation == null)
            {
                return HttpNotFound();
            }
            var model = new OrganisationDashboardViewModel(organisation);
            foreach (var item in model.Providers.Where(x => x.IsAccepted && !x.IsRejected && !x.IsProviderDeleted))
            {
                db.up_ProviderUpdateQualityScore(item.ProviderId, true);
            }
            db.up_OrganisationUpdateQualityScore(organisation.OrganisationId);
            QualityIndicator.SetSessionInformation(userContext);
            return RedirectToAction("TrafficLight");
        }

        #endregion
    }
}