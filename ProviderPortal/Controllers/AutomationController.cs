using SFA.Roatp.Api.Client;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;
using TribalTechnology.InformationManagement.Net.Mail;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public class AutomationController : BaseController
    {
        private const Int32 languageId = 1;  // Automated tasks don't have a language Id so we need to pass one

        public enum AutomatedTaskName
        {
            LARSImport,
            RoATPAPI
        }

        [NonAction]
        public static Boolean CanRunAutomatedTask(AutomatedTaskName taskName)
        {
            ProviderPortalEntities db = new ProviderPortalEntities();
            return db.up_CanRunAutomatedTask(taskName.ToString()) == 1;
        }

        [NonAction]
        public static void CompleteAutomatedTask(AutomatedTaskName taskName)
        {
            String name = taskName.ToString();
            ProviderPortalEntities db = new ProviderPortalEntities();
            AutomatedTask at = db.AutomatedTasks.Where(x => x.TaskName == name).FirstOrDefault();
            if (at != null)
            {
                at.InProgress = false;
                db.Entry(at).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        #region ProviderTrafficLightStatus

        // Number of months before a new provider receives their first email
        const int NewProviderGracePeriod = 3;
        
        //
        // GET: /Automation/ProviderTrafficLightStatus/{days offset}
        // This needs a scheduled task to call it periodically
        [AllowAnonymous]
        public ActionResult ProviderTrafficLightStatus(int id = 0, bool forceStop = false)
        {
            const string mutexKey = "ProviderTrafficLightStatusMutex";
            var mutex = (bool?)CacheManagement.CacheHandler.Get(mutexKey);
            if (mutex.HasValue && mutex.Value)
            {
                if (forceStop)
                {
                    CacheManagement.CacheHandler.Invalidate(mutexKey);
                }
                Response.Write(forceStop
                    ? "Job in progress, stopped..."
                    : "Job in progress, skipping...");
                return null;
            }
            if (forceStop)
            {
                Response.Write("Not in progress...");
                return null;
            }

            CacheManagement.CacheHandler.Add(mutexKey, true, new TimeSpan(12, 0, 0));

            var model = new ProviderTrafficLightStatusViewModel();
            var providers = GetProviderTrafficLightStatusInfo();
            var sendToList = new Dictionary<int, ProviderTrafficLightStatusViewModel.ProviderTrafficLightEmail>();
            var today = Request.IsAuthenticated && userContext.IsAdministration()
                ? DateTime.UtcNow.AddDays(id).Date 
                : DateTime.UtcNow.Date;
            model.Today = today;
            foreach (var item in providers)
            {
                var lastUpdate = item.ModifiedDateTimeUtc == null ? (DateTime?)null : item.ModifiedDateTimeUtc.Value.Date;
                var lastEmail = item.LastEmailDateTimeUtc;
                var action = model.Skipped;

                if (item.SFAFunded /* and it doesn't matter if they are also DfE funded, SFA emails trump the DfE ones */)
                {
                    // Number of months the provider stays green after updating
                    var greenPeriod = lastUpdate == null
                        ? 0
                        : QualityIndicator.GetGreenDuration(lastUpdate.Value.Month);
                    // Offset for when they go amber (alwas one month before going red)
                    var amberPeriod = greenPeriod + 1;

                    if (lastEmail == today)
                    {
                        action = model.EmailAlreadySent;
                    }
                    else if (lastUpdate == null)
                    {
                        action = model.Skipped;
                        item.EmailTemplateId = null;
                        item.NextEmailTemplateId = Constants.EmailTemplates.SfaProviderTrafficLightIsNowRed;
                        item.NextEmailDateTimeUtc = item.ProviderCreatedDateTimeUtc.AddMonths(NewProviderGracePeriod).Date;
                        item.TrafficLightStatusId = QualityIndicator.TrafficLight.Red;
                    }
                    else if (today < lastUpdate.Value.AddMonths(greenPeriod))
                    {
                        action = model.Skipped;
                        item.NextEmailTemplateId = Constants.EmailTemplates.SfaProviderTrafficLightIsNowAmber;
                        item.NextEmailDateTimeUtc = lastUpdate.Value.AddMonths(greenPeriod);
                        item.TrafficLightStatusId = QualityIndicator.TrafficLight.Green;
                    }
                    else if (today == lastUpdate.Value.AddMonths(greenPeriod))
                    {
                        action = model.AmberToday;
                        item.EmailTemplateId = Constants.EmailTemplates.SfaProviderTrafficLightIsNowAmber;
                        item.NextEmailTemplateId = Constants.EmailTemplates.SfaProviderTrafficLightIsAmberWeek1;
                        item.NextEmailDateTimeUtc = lastUpdate.Value.AddMonths(greenPeriod).AddDays(7);
                        item.TrafficLightStatusId = QualityIndicator.TrafficLight.Amber;
                    }
                    else if (today == lastUpdate.Value.AddMonths(greenPeriod).AddDays(7))
                    {
                        action = model.AmberForOneWeek;
                        item.EmailTemplateId = Constants.EmailTemplates.SfaProviderTrafficLightIsAmberWeek1;
                        item.NextEmailTemplateId = Constants.EmailTemplates.SfaProviderStatusRedInOneWeek;
                        item.NextEmailDateTimeUtc = lastUpdate.Value.AddMonths(amberPeriod).AddDays(-7);
                        item.TrafficLightStatusId = QualityIndicator.TrafficLight.Amber;
                    }
                    else if (today == lastUpdate.Value.AddMonths(amberPeriod).AddDays(-7))
                    {
                        action = model.RedInOneWeek;
                        item.EmailTemplateId = Constants.EmailTemplates.SfaProviderStatusRedInOneWeek;
                        item.NextEmailTemplateId = Constants.EmailTemplates.SfaProviderTrafficLightIsNowRed;
                        item.NextEmailDateTimeUtc = lastUpdate.Value.AddMonths(amberPeriod);
                        item.TrafficLightStatusId = QualityIndicator.TrafficLight.Amber;
                    }
                    else if (today == lastUpdate.Value.AddMonths(amberPeriod))
                    {
                        action = model.RedToday;
                        item.EmailTemplateId = Constants.EmailTemplates.SfaProviderTrafficLightIsNowRed;
                        item.NextEmailTemplateId = Constants.EmailTemplates.SfaProviderTrafficLightIsRedWeeklyReminder;
                        item.NextEmailDateTimeUtc = lastUpdate.Value.AddMonths(amberPeriod).AddDays(7);
                        item.TrafficLightStatusId = QualityIndicator.TrafficLight.Red;
                    }
                    else if (today > lastUpdate.Value.AddMonths(amberPeriod) &&
                             lastUpdate.Value.AddMonths(amberPeriod).DayOfWeek == today.DayOfWeek)
                    {
                        action = model.StillRed;
                        item.EmailTemplateId = Constants.EmailTemplates.SfaProviderTrafficLightIsRedWeeklyReminder;
                        item.NextEmailTemplateId = Constants.EmailTemplates.SfaProviderTrafficLightIsRedWeeklyReminder;
                        item.NextEmailDateTimeUtc = today.AddDays(7);
                        item.TrafficLightStatusId = QualityIndicator.TrafficLight.Red;
                    }
                }
                else if (item.DFE1619Funded && lastUpdate == null)
                {
                    var notifyDate =
                        item.ProviderCreatedDateTimeUtc.Month < 11
                            ? new DateTime(today.Year, 11, 1)
                            : new DateTime(today.AddYears(1).Year, 11, 1);
                    action = model.Skipped;
                    item.EmailTemplateId = null;
                    item.NextEmailTemplateId = Constants.EmailTemplates.Dfe1619ProviderTrafficLightIsNowRed;
                    item.NextEmailDateTimeUtc = notifyDate;
                    item.TrafficLightStatusId = QualityIndicator.TrafficLight.Red;
                }
                else if (item.DFE1619Funded && lastUpdate.HasValue)
                {
                    item.TrafficLightStatusId = QualityIndicator.DfeProviderDateToIndex(lastUpdate.Value);
                    var greenEndDate = QualityIndicator.GetDfeProviderGreenEndDate(lastUpdate.Value).Date;
                    switch (item.TrafficLightStatusId)
                    {
                        case QualityIndicator.TrafficLight.Green:

                            action = model.Skipped;
                            item.NextEmailTemplateId = Constants.EmailTemplates.Dfe1619ProviderTrafficLightIsNowAmber;
                            item.NextEmailDateTimeUtc = greenEndDate.AddDays(1);
                            break;

                        case QualityIndicator.TrafficLight.Amber:

                            if (today.Month == 10 && today.Day == 1)
                            {
                                action = model.AmberToday;
                                item.EmailTemplateId = Constants.EmailTemplates.Dfe1619ProviderTrafficLightIsNowAmber;
                                item.NextEmailTemplateId = Constants.EmailTemplates.Dfe1619ProviderTrafficLightIsAmberWeek1;
                                item.NextEmailDateTimeUtc = today.AddDays(7);
                            }
                            else if (today.Month == 10 && today.Day == 8)
                            {
                                action = model.AmberForOneWeek;
                                item.EmailTemplateId = Constants.EmailTemplates.Dfe1619ProviderTrafficLightIsAmberWeek1;
                                item.NextEmailTemplateId = Constants.EmailTemplates.Dfe1619ProviderStatusRedInOneWeek;
                                item.NextEmailDateTimeUtc = today.AddDays(17);
                            }
                            break;

                        case QualityIndicator.TrafficLight.Red:

                            if (today.Month == 10 && today.Day == 25 && today.Year == greenEndDate.Year)
                            {
                                action = model.RedInOneWeek;
                                item.EmailTemplateId = Constants.EmailTemplates.Dfe1619ProviderStatusRedInOneWeek;
                                item.NextEmailTemplateId = Constants.EmailTemplates.Dfe1619ProviderTrafficLightIsNowRed;
                                item.NextEmailDateTimeUtc = today.AddDays(7);
                            }
                            else if (today.Month == 11 && today.Day == 1 && today.Year == greenEndDate.Year)
                            {
                                action = model.RedToday;
                                item.EmailTemplateId = Constants.EmailTemplates.Dfe1619ProviderTrafficLightIsNowRed;
                                item.NextEmailTemplateId =
                                    Constants.EmailTemplates.Dfe1619ProviderTrafficLightIsRedWeeklyReminder;
                                item.NextEmailDateTimeUtc = today.AddDays(7);
                            }
                            else if ((int)(today - lastUpdate.Value).TotalDays % 7 == 0)
                            {
                                action = model.StillRed;
                                item.EmailTemplateId = Constants.EmailTemplates.Dfe1619ProviderTrafficLightIsRedWeeklyReminder;
                                item.NextEmailTemplateId =
                                    Constants.EmailTemplates.Dfe1619ProviderTrafficLightIsRedWeeklyReminder;
                                item.NextEmailDateTimeUtc = today.AddDays(7);
                            }
                            break;
                    }
                }

                //if (item.EmailTemplateId != null || item.NextEmailTemplateId != null)
                //{
                    item.EmailDateTimeUtc = item.EmailTemplateId == null ? (DateTime?)null : today;
                    item.HasValidRecipients = false;
                    sendToList.Add(item.ProviderId, item);
                //}

                if (item.TrafficLightStatusId == 0)
                {
                    item.TrafficLightStatusId = QualityIndicator.GetTrafficStatus(item.ModifiedDateTimeUtc, item.SFAFunded, item.DFE1619Funded);
                }

                model.Log[action].Add(item.ProviderName);
            }

            // Send the emails - this updates the sendToList
            SendProviderTrafficLightStatusEmails(sendToList, today);       

            // Log the results
            SaveHistoryLog(sendToList, today);

            // Delete mutex
            CacheManagement.CacheHandler.Invalidate(mutexKey);

            return View(model);
        }

        [NonAction]
        private void SendProviderTrafficLightStatusEmails(Dictionary<int, ProviderTrafficLightStatusViewModel.ProviderTrafficLightEmail> providers, DateTime today)
        {
            if (!providers.Any()) return;

            var sendToProviderIds = providers.Values
                .Where(x => x.EmailTemplateId != null)
                .Where(x => !x.QualityEmailsPaused)
                .Select(x => x.ProviderId)
                .ToList();
            var superUsers = ProvisionUtilities.GetProviderUsers(db, sendToProviderIds, false, true);
            
            // Send the emails out
            foreach (var user in superUsers)
            {
                if (!providers.ContainsKey(user.ProviderId)) continue;
                var provider = providers[user.ProviderId];
                if (provider.EmailTemplateId == null) continue;
                provider.HasValidRecipients = true;

                //AppGlobal.EmailQueue.AddToSendQueue(
                //    TemplatedEmail.EmailMessage(
                //        new MailAddress(user.Email, user.Name),
                //        null,
                //        null,
                //        provider.EmailTemplateId.Value,
                //        new List<EmailParameter>
                //        {
                //            new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                //            new EmailParameter("%LASTUPDATEDATE%",
                //                provider.ModifiedDateTimeUtc.HasValue
                //                    ? provider.ModifiedDateTimeUtc.Value.ToString(
                //                        Constants.ConfigSettings.ShortDateFormat)
                //                    : AppGlobal.Language.GetText(this, "NeverUpdated", "never")),
                //            new EmailParameter("%MONTHSSINCEUPDATE%",
                //                provider.ModifiedDateTimeUtc.HasValue
                //                ? QualityIndicator.GetMonthsBetween(provider.ModifiedDateTimeUtc.Value, DateTime.UtcNow).ToString()
                //                : NewProviderGracePeriod.ToString(CultureInfo.InvariantCulture))
                //        }));

                var emailMessage = TemplatedEmail.EmailMessage(
                        new MailAddress(user.Email, user.Name),
                        null,
                        null,
                        provider.EmailTemplateId.Value,
                        new List<EmailParameter>
                        {
                            new EmailParameter("%PROVIDERNAME%", provider.ProviderName),
                            new EmailParameter("%LASTUPDATEDATE%",
                                provider.ModifiedDateTimeUtc.HasValue
                                    ? provider.ModifiedDateTimeUtc.Value.ToString(
                                        Constants.ConfigSettings.ShortDateFormat)
                                    : AppGlobal.Language.GetText(this, "NeverUpdated", "never")),
                            new EmailParameter("%MONTHSSINCEUPDATE%",
                                provider.ModifiedDateTimeUtc.HasValue
                                ? QualityIndicator.GetMonthsBetween(provider.ModifiedDateTimeUtc.Value, DateTime.UtcNow).ToString()
                                : NewProviderGracePeriod.ToString(CultureInfo.InvariantCulture))
                        });

                var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null);
            }

            // Update the providers
            foreach (var providerId in sendToProviderIds)
            {
                var provider = new Provider
                {
                    ProviderId = providerId,
                    TrafficLightEmailDateTimeUtc = today
                };

                db.Providers.Attach(provider);
                db.Entry(provider).Property(x => x.TrafficLightEmailDateTimeUtc).IsModified = true;

                providers[providerId].EmailDateTimeUtc = today;
            }

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        /// <summary>
        /// Gets the current provider traffic light status information.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private IEnumerable<ProviderTrafficLightStatusViewModel.ProviderTrafficLightEmail> GetProviderTrafficLightStatusInfo()
        {
            var providers = db.Providers
                .Where(x =>
                    x.RecordStatu.IsPublished)
                .Select(x =>
                    new ProviderTrafficLightStatusViewModel.ProviderTrafficLightEmail
                    {
                        ProviderId = x.ProviderId,
                        ProviderName = x.ProviderName,
                        ModifiedDateTimeUtc = x.QualityScore.LastActivity,
                        LastEmailDateTimeUtc = x.TrafficLightEmailDateTimeUtc,
                        ProviderCreatedDateTimeUtc = x.CreatedDateTimeUtc,
                        SFAFunded = x.SFAFunded,
                        DFE1619Funded = x.DFE1619Funded,
                        EmailTemplateId = (Constants.EmailTemplates?) null,
                        QualityEmailsPaused = x.QualityEmailsPaused,
                    }).ToList();
            return providers;
        }

        /// <summary>
        /// Saves the quality email status history log.
        /// </summary>
        /// <param name="trafficLightEmails">The traffic light emails.</param>
        /// <param name="today"></param>
        private void SaveHistoryLog(Dictionary<int, ProviderTrafficLightStatusViewModel.ProviderTrafficLightEmail> trafficLightEmails, DateTime today)
        {
            db.QualityEmailLogs.AddRange(
                trafficLightEmails.Values
                .Where(x => x.EmailTemplateId != null)
                .Select(x => new QualityEmailLog
                {
                    ProviderId = x.ProviderId,
                    ModifiedDateTimeUtc = x.ModifiedDateTimeUtc,
                    SFAFunded = x.SFAFunded,
                    DFE1619Funded = x.DFE1619Funded,
                    QualityEmailsPaused = x.QualityEmailsPaused,
                    EmailDateTimeUtc = x.EmailDateTimeUtc,
                    EmailTemplateId = (int?)x.EmailTemplateId,
                    NextEmailTemplateId = (int?)x.NextEmailTemplateId,
                    NextEmailDateTimeUtc = x.NextEmailDateTimeUtc,
                    HasValidRecipients = x.HasValidRecipients,
                    TrafficLightStatusId = (int)x.TrafficLightStatusId,
                    CreatedDateTimeUtc = today,
                }));
            db.SaveChanges();

            db.up_QualityEmailLogDeleteDuplicateRows();
        }

        #endregion

        #region LARS Download and Import

        private static String LARSFilename = String.Empty;
        private static String LARSFolder = String.Empty;
        [NonAction]
        public static void CheckLARSDownload(Object stateInfo)
        {
            Boolean automatedTaskStarted = false;
            try
            {
                if (String.IsNullOrEmpty(Constants.ConfigSettings.LARSImportTime))
                {
                    // Log Warning about LARSImportTime not being set
                    AppGlobal.Log.WriteError(AppGlobal.Language.GetText("Automation_LARSImport_LARSImportTimeNotConfigured", "Error Importing LARS File.  LARSImportTime Not Configured", false, languageId));
                    return;
                }

                LARSFolder = Constants.ConfigSettings.LARSUploadVirtualDirectoryName;
                if (LARSFolder.EndsWith(@"\"))
                {
                    LARSFolder = LARSFolder.Substring(0, LARSFolder.Length - 1);
                }

                // Check if config setting is valid
                if (String.IsNullOrEmpty(LARSFolder) || !Directory.Exists(LARSFolder))
                {
                    AppGlobal.Log.WriteError(String.Format(AppGlobal.Language.GetText("Automation_LARSImport_LARSFolderNotConfigured", "Error Importing LARS File.  LARSFolderNotConfigured Not Configured Correctly: {0}", false, languageId), LARSFolder));
                    return;
                }

                if (String.IsNullOrEmpty(Constants.ConfigSettings.LARSImportUserId))
                {
                    // Log Warning about LARSImportUserId not being set
                    AppGlobal.Log.WriteError(AppGlobal.Language.GetText("Automation_LARSImport_UserIdNotConfigured", "Error Importing LARS File.  LARSImportUserId Not Configured", false, languageId));
                    return;
                }

                AspNetUser aspNetUser = new ProviderPortalEntities().AspNetUsers.Find(Constants.ConfigSettings["LARSImportUserId"].ToString());
                if (aspNetUser == null)
                {
                    // Log Error
                    AppGlobal.Log.WriteError(String.Format(AppGlobal.Language.GetText("Automation_LARSImport_UserIdNotFound", "Error Importing LARS File.  Cannot find user Id {0}", false, languageId), Constants.ConfigSettings["LARSImportUserId"]));
                    return;
                }

                if (DateTime.Now.ToString("HH:mm") == Constants.ConfigSettings.LARSImportTime)
                {
                    // Ensure that another server hasn't picked this up
                    if (!CanRunAutomatedTask(AutomatedTaskName.LARSImport))
                    {
                        AppGlobal.Log.WriteLog(AppGlobal.Language.GetText("Automation_LARSImport_Running", "Automated LARS import running on a different server", false, languageId));
                        return;
                    }

                    automatedTaskStarted = true;

                    DateTime date = DateTime.Today;
                    DateTime lastImport = DateTime.MinValue.Date;

                    MetadataUpload mtu = new ProviderPortalEntities().MetadataUploads.Where(x => x.MetadataUploadTypeId == (Int32) Constants.MetadataUploadType.LearningAim).OrderByDescending(x => x.CreatedDateTimeUtc).FirstOrDefault();
                    if (mtu != null)
                    {
                        lastImport = mtu.CreatedDateTimeUtc.Date;
                    }

                    Boolean fileFound = false;
                    String fileName = String.Empty;
                    while (date > lastImport)
                    {
                        fileName = Constants.ConfigSettings.LARSUrlAndFileName.Replace("{date}", date.ToString("yyyyMMdd")).Replace("{year}", GetLastYear());
                        if (DoesFileExistAtUrl(fileName))
                        {
                            fileFound = true;
                            break;
                        }
                        fileName = Constants.ConfigSettings.LARSUrlAndFileName.Replace("{date}", date.ToString("yyyyMMdd")).Replace("{year}", GetThisYear());
                        if (DoesFileExistAtUrl(fileName))
                        {
                            fileFound = true;
                            break;
                        }

                        date = date.AddDays(-1);
                    }

                    if (fileFound)
                    {
                        AppGlobal.Log.WriteLog(String.Format(AppGlobal.Language.GetText("Automation_LARSImport_FileFound", "Found LARS File {0}.  Importing...", false, languageId), fileName));

                        // Download the file
                        using (WebClient webClient = new WebClient())
                        {
                            LARSFilename = LARSFolder + @"\" + GetFileName(fileName);
                            webClient.DownloadFileCompleted += webClient_LARSDownloadFileCompleted;
                            webClient.DownloadFileAsync(new Uri(fileName), LARSFilename);
                        }
                    }
                    else
                    {
                        CompleteAutomatedTask(AutomatedTaskName.LARSImport);

                        AppGlobal.Log.WriteLog(AppGlobal.Language.GetText("Automation_LARSImport_NoFileFound", "No Updated LARS File Found", false, languageId));
                        AppGlobal.Log.WriteLog(String.Format(AppGlobal.Language.GetText("Automation_LARSImport_NoOfDaysSinceLastImport", "{0} day(s) since last LARS import.", false, languageId), (DateTime.Now - lastImport).Days));

                        // Check if we need to send an email
                        Int32 daysBeforeSendingEmail = Constants.ConfigSettings.LARSDaysSinceLastImportBeforeSendingEmail;
                        if (daysBeforeSendingEmail > 0)
                        {
                            TimeSpan ts = TimeSpan.FromDays(daysBeforeSendingEmail);
                            if (DateTime.Today - lastImport >= ts)
                            {
                                // Send email once per week
                                if ((DateTime.Today - lastImport).Subtract(ts).Days%7 == 0)
                                {
                                    AppGlobal.Log.WriteLog(AppGlobal.Language.GetText("Automation_LARSImport_SendingWarningEmail", "LARS Importer: Sending warning email.", false, languageId));

                                    if (String.IsNullOrWhiteSpace(Constants.ConfigSettings.LARSLongTimeSinceImportEmailAddress))
                                    {
                                        AppGlobal.Log.WriteLog(AppGlobal.Language.GetText("Automation_LARSImport_LARSLongTimeSinceImportEmailAddressNotConfigured", "LARSLongTimeSinceImportEmailAddress Not Configured", false, languageId));
                                    }
                                    else if (!AppGlobal.IsValidEmail(Constants.ConfigSettings.LARSLongTimeSinceImportEmailAddress))
                                    {
                                        AppGlobal.Log.WriteError(String.Format(AppGlobal.Language.GetText("Automation_LARSImport_LARSLongTimeSinceImportEmailAddressInvalid", "LARSLongTimeSinceImportEmailAddress is Invalid: {0}", false, languageId), Constants.ConfigSettings.LARSLongTimeSinceImportEmailAddress));
                                    }
                                    else
                                    {
                                        // Send email(s)
                                        foreach (String address in Constants.ConfigSettings.LARSLongTimeSinceImportEmailAddress.Split(';'))
                                        {
                                            //AppGlobal.EmailQueue.AddToSendQueue(
                                            //    TemplatedEmail.EmailMessage(
                                            //        new MailAddress(address),
                                            //        null,
                                            //        new MailAddress(Constants.ConfigSettings.AutomatedFromEmailAddress, Constants.ConfigSettings.AutomatedFromEmailName),
                                            //        Constants.EmailTemplates.LARSFileNotImportSinceXDaysAgo,
                                            //        new List<EmailParameter>
                                            //        {
                                            //            new EmailParameter("%LASTIMPORT%", lastImport.ToString("dd MMM yyyy")),
                                            //            new EmailParameter("%NUMBEROFDAYSSINCELASTIMPORT%", (DateTime.Today - lastImport).Days.ToString("N0")),
                                            //            new EmailParameter("%CONFIGUREDNUMBEROFDAYS%", daysBeforeSendingEmail.ToString("N0"))
                                            //        },
                                            //        AppGlobal.Language.GetText("TemplatedEmail_EmailOverride_FormatString", "<p>This email was originally sent to {0}:<p>{1}", false, languageId)));

                                            var emailMessage = TemplatedEmail.EmailMessage(
                                                    new MailAddress(address),
                                                    null,
                                                    new MailAddress(Constants.ConfigSettings.AutomatedFromEmailAddress, Constants.ConfigSettings.AutomatedFromEmailName),
                                                    Constants.EmailTemplates.LARSFileNotImportSinceXDaysAgo,
                                                    new List<EmailParameter>
                                                    {
                                                        new EmailParameter("%LASTIMPORT%", lastImport.ToString("dd MMM yyyy")),
                                                        new EmailParameter("%NUMBEROFDAYSSINCELASTIMPORT%", (DateTime.Today - lastImport).Days.ToString("N0")),
                                                        new EmailParameter("%CONFIGUREDNUMBEROFDAYS%", daysBeforeSendingEmail.ToString("N0"))
                                                    },
                                                    AppGlobal.Language.GetText("TemplatedEmail_EmailOverride_FormatString", "<p>This email was originally sent to {0}:<p>{1}", false, languageId));

                                            var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (automatedTaskStarted)
                {
                    CompleteAutomatedTask(AutomatedTaskName.LARSImport);
                }
                AppGlobal.Log.WriteError(String.Format(AppGlobal.Language.GetText("Automation_LARSImport_GenericError", "Automated LARS Importer Failed With Error: {0}", false, languageId), ex.Message));
            }
        }

        static void webClient_LARSDownloadFileCompleted(Object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    EmptyLARSFolder();

                    // Log Error Downloading File
                    AppGlobal.Log.WriteError(String.Format(AppGlobal.Language.GetText("Automation_LARSImport_Error", "Error Importing LARS File: {0}", false, languageId), e.Error.Message));
                    return;
                }

                if (String.IsNullOrEmpty(Constants.ConfigSettings.LARSImportUserId))
                {
                    // Already checked so unlikely but if we do get here then delete the downloaded file
                    EmptyLARSFolder();

                    // Log Warning about LARSImportUserId not being set
                    AppGlobal.Log.WriteError(AppGlobal.Language.GetText("Automation_LARSImport_UserIdNotConfigured", "Error Importing LARS File.  LARSImportUserId Not Configured", false, languageId));
                    return;
                }

                AspNetUser aspNetUser = new ProviderPortalEntities().AspNetUsers.Find(Constants.ConfigSettings.LARSImportUserId);
                if (aspNetUser == null)
                {
                    // Already checked so unlikely but if we do get here then delete the downloaded file
                    EmptyLARSFolder();

                    // Log Error
                    AppGlobal.Log.WriteError(String.Format(AppGlobal.Language.GetText("Automation_LARSImport_UserIdNotFound", "Error Importing LARS File.  Cannot find user Id {0}", false, languageId), Constants.ConfigSettings.LARSImportUserId));
                    return;
                }

                // Import the data
                LARSController.ImportLARSFile(aspNetUser.Id, languageId);

                // Log Success!!
                AppGlobal.Log.WriteLog(String.Format(AppGlobal.Language.GetText("Automation_LARSImport_Success", "Succesfully Imported LARS File: {0}", false, languageId), GetFileName(LARSFilename)));
            }
            catch (Exception ex)
            {
                // Log Error
                AppGlobal.Log.WriteError(String.Format(AppGlobal.Language.GetText("Automation_LARSImport_Error", "Error Importing LARS File: {0}", false, languageId), ex.Message));

                // Send Email
                foreach (String address in Constants.ConfigSettings.LARSImportErrorEmailAddress.Split(';'))
                {
                    //AppGlobal.EmailQueue.AddToSendQueue(
                    //    TemplatedEmail.EmailMessage(
                    //        new MailAddress(address),
                    //        null,
                    //        new MailAddress(Constants.ConfigSettings.AutomatedFromEmailAddress, Constants.ConfigSettings.AutomatedFromEmailName),
                    //        Constants.EmailTemplates.LARSImportError,
                    //        new List<EmailParameter>
                    //        {
                    //            new EmailParameter("%EXCEPTION%", ex.Message),
                    //            new EmailParameter("%STACKTRACE%", ex.StackTrace),
                    //            new EmailParameter("%FILENAME%", GetFileName(LARSFilename))
                    //        },
                    //        AppGlobal.Language.GetText("TemplatedEmail_EmailOverride_FormatString", "<p>This email was originally sent to {0}:<p>{1}", false, languageId)));

                    var emailMessage = TemplatedEmail.EmailMessage(
                            new MailAddress(address),
                            null,
                            new MailAddress(Constants.ConfigSettings.AutomatedFromEmailAddress, Constants.ConfigSettings.AutomatedFromEmailName),
                            Constants.EmailTemplates.LARSImportError,
                            new List<EmailParameter>
                            {
                                new EmailParameter("%EXCEPTION%", ex.Message),
                                new EmailParameter("%STACKTRACE%", ex.StackTrace),
                                new EmailParameter("%FILENAME%", GetFileName(LARSFilename))
                            },
                            AppGlobal.Language.GetText("TemplatedEmail_EmailOverride_FormatString", "<p>This email was originally sent to {0}:<p>{1}", false, languageId));

                    var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null);
                }
            }
            finally
            {
                EmptyLARSFolder();
            }
        }

        [NonAction]
        private static String GetFileName(String fileName)
        {
            char separator = '\\';
            if (fileName.IndexOf(@"\") == -1)
            {
                separator = '/';
            }
            String[] fileParts = fileName.Split(separator);
            return fileParts[fileParts.Length - 1];
        }

        [NonAction]
        private static void EmptyLARSFolder(Boolean clearDatabaseFlag = true)
        {
            foreach (FileInfo file in new DirectoryInfo(LARSFolder).GetFiles())
            {
                file.Delete();
            }

            if (clearDatabaseFlag)
            {
                CompleteAutomatedTask(AutomatedTaskName.LARSImport);
            }
        }

        [NonAction]
        private static String GetLastYear()
        {
            return String.Format("{0}{1}", DateTime.Today.AddYears(-1).ToString("yy"), DateTime.Today.ToString("yy"));
        }

        [NonAction]
        private static String GetThisYear()
        {
            return String.Format("{0}{1}", DateTime.Today.ToString("yy"), DateTime.Today.AddYears(1).ToString("yy"));
        }

        [NonAction]
        private static Boolean DoesFileExistAtUrl(String url)
        {
            Boolean result = false;

            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Timeout = 1200; // miliseconds
            webRequest.Method = "HEAD";

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)webRequest.GetResponse();
                result = true;
            }
            catch (WebException) {}
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }

        #endregion

        #region RoATP API

        [NonAction]
        public static void ImportRoATPData(Object stateInfo)
        {
            Boolean automatedTaskStarted = false;
            try
            {
                if (String.IsNullOrEmpty(Constants.ConfigSettings.RoATPAPIImportTime))
                {
                    // Log Warning about RoATPAPIImportTime not being set
                    AppGlobal.Log.WriteError(AppGlobal.Language.GetText("Automation_RoATPAPIImport_ImportTimeNotConfigured", "Error Importing RoATP Data.  RoATPAPIImportTime Not Configured", false, languageId));
                    return;
                }

                if (DateTime.Now.ToString("HH:mm") == Constants.ConfigSettings.RoATPAPIImportTime)
                {
                    // Ensure that another server hasn't picked this up
                    if (!CanRunAutomatedTask(AutomatedTaskName.RoATPAPI))
                    {
                        AppGlobal.Log.WriteLog(AppGlobal.Language.GetText("Automation_RoATPImport_Running", "Automated RoATP API data import running on a different server", false, languageId));
                        return;
                    }

                    automatedTaskStarted = true;

                    // Do the import
                    new Thread(() =>
                    {
                        try
                        {
                            using (ProviderPortalEntities db = new ProviderPortalEntities())
                            {
                                List<Int32> ukprns = new List<Int32>();
                                using (RoatpApiClient client = new RoatpApiClient())
                                {
                                    foreach (SFA.Roatp.Api.Types.Provider apiProvider in client.FindAll())
                                    {
                                        if (apiProvider.ProviderType == SFA.Roatp.Api.Types.ProviderType.MainProvider)
                                        {
                                            if (apiProvider.Ukprn <= Int32.MaxValue) // Should always be but just incase
                                            {
                                                ukprns.Add(Convert.ToInt32(apiProvider.Ukprn));
                                            }
                                        }

                                        List<Provider>providers = db.Providers.Where(x => x.Ukprn == apiProvider.Ukprn && x.RecordStatusId == (Int32)Constants.RecordStatus.Live).ToList();
                                        if (providers.Count > 0)
                                        {
                                            foreach (Provider provider in providers)
                                            {
                                                RoATPProviderType providerType = db.RoATPProviderTypes.Find((Int32)apiProvider.ProviderType);
                                                if (providerType != null)
                                                {
                                                    if (provider.RoATPProviderType != providerType || provider.RoATPStartDate != apiProvider.StartDate || provider.ApprenticeshipContract != (apiProvider.ProviderType == SFA.Roatp.Api.Types.ProviderType.MainProvider))
                                                    {
                                                        // Provider found and updated - log it.
                                                        AppGlobal.Log.WriteLog(String.Format(AppGlobal.Language.GetText("Automation_RoATPImport_ProviderUpdated", "Automated RoATP API data import updated provider {0} for ukprn {1}", false, languageId), provider.ProviderId, provider.Ukprn));

                                                        provider.RoATPProviderType = providerType;
                                                        provider.RoATPStartDate = apiProvider.StartDate;
                                                        provider.ApprenticeshipContract = apiProvider.ProviderType == SFA.Roatp.Api.Types.ProviderType.MainProvider;
                                                        db.Entry(provider).State = EntityState.Modified;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // Provider not found - log it
                                            AppGlobal.Log.WriteLog(String.Format(AppGlobal.Language.GetText("Automation_RoATPImport_ProviderNotFound", "Automated RoATP API data import no provider found for ukprn {0}", false, languageId), apiProvider.Ukprn));
                                        }
                                    }
                                }

                                // Get current apprenticeship provers who are not in the API
                                foreach (Provider provider in db.Providers.Where(x => x.ApprenticeshipContract == true && !ukprns.Contains(x.Ukprn)))
                                {
                                    // Log it
                                    AppGlobal.Log.WriteLog(String.Format(AppGlobal.Language.GetText("Automation_RoATPImport_RemovedProviderFromRoATP", "Automated RoATP API data import removed provider {0} from RoATP, ukprn {1}", false, languageId), provider.ProviderId, provider.Ukprn));

                                    provider.ApprenticeshipContract = false;
                                    db.Entry(provider).State = EntityState.Modified;
                                }

                                // Save the changes
                                db.SaveChanges();
                            }

                            // Complete the task
                            CompleteAutomatedTask(AutomatedTaskName.RoATPAPI);
                        }
                        catch (Exception ex)
                        {
                            // Send Email
                            foreach (String address in Constants.ConfigSettings.RoATPImportErrorEmailAddress.Split(';'))
                            {
                                //AppGlobal.EmailQueue.AddToSendQueue(
                                //    TemplatedEmail.EmailMessage(
                                //        new MailAddress(address),
                                //        null,
                                //        new MailAddress(Constants.ConfigSettings.AutomatedFromEmailAddress, Constants.ConfigSettings.AutomatedFromEmailName),
                                //        Constants.EmailTemplates.RoATPImportError,
                                //        new List<EmailParameter>
                                //        {
                                //            new EmailParameter("%EXCEPTION%", ex.Message),
                                //            new EmailParameter("%STACKTRACE%", ex.StackTrace)
                                //        },
                                //        AppGlobal.Language.GetText("TemplatedEmail_EmailOverride_FormatString", "<p>This email was originally sent to {0}:<p>{1}", false, languageId)));

                                var emailMessage = TemplatedEmail.EmailMessage(
                                        new MailAddress(address),
                                        null,
                                        new MailAddress(Constants.ConfigSettings.AutomatedFromEmailAddress, Constants.ConfigSettings.AutomatedFromEmailName),
                                        Constants.EmailTemplates.RoATPImportError,
                                        new List<EmailParameter>
                                        {
                                            new EmailParameter("%EXCEPTION%", ex.Message),
                                            new EmailParameter("%STACKTRACE%", ex.StackTrace)
                                        },
                                        AppGlobal.Language.GetText("TemplatedEmail_EmailOverride_FormatString", "<p>This email was originally sent to {0}:<p>{1}", false, languageId));

                                var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, null);
                            }

                            AppGlobal.Log.WriteError(String.Format(AppGlobal.Language.GetText("Automation_RoATPImport_GenericError", "Automated RoATP API Data Importer Failed With Error: {0}", false, languageId), ex.Message));
                            if (automatedTaskStarted)
                            {
                                CompleteAutomatedTask(AutomatedTaskName.RoATPAPI);
                            }
                        }
                    }).Start();
                }
            }
            catch (Exception ex)
            {
                AppGlobal.Log.WriteError(String.Format(AppGlobal.Language.GetText("Automation_RoATPImport_GenericError", "Automated RoATP API Data Importer Failed With Error: {0}", false, languageId), ex.Message));
                if (automatedTaskStarted)
                {
                    CompleteAutomatedTask(AutomatedTaskName.RoATPAPI);
                }
            }
        }

        #endregion
    }
}