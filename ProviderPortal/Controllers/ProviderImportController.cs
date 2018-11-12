using System.Text.RegularExpressions;
using System.Threading;
using CsvHelper;
using System;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Web;

    using Microsoft.AspNet.Identity.Owin;

    using Models;
    using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
    using TribalTechnology.InformationManagement.Net.Mail;

    public class ProviderImportController : BaseController
    {
        private const String MessageArea = "ProvImport";
        private readonly String cancelImportMessage;

        // ReSharper disable once RedundantBaseConstructorCall
        public ProviderImportController() : base()
        {
            cancelImportMessage = AppGlobal.Language.GetText(this, "CancellingImport", "Cancelling import...");
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadProviderData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult Import()
        {
            GetViewData();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(new[] { Permission.PermissionName.CanUploadProviderData })]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult Import(ProviderImportModel model)
        {
            String ProviderImportFolder = String.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    ProviderImportFolder = Constants.ConfigSettings.ProviderImportFilesVirtualDirectoryName;
                    if (ProviderImportFolder.EndsWith(@"\"))
                    {
                        ProviderImportFolder = ProviderImportFolder.Substring(0, ProviderImportFolder.Length - 1);
                    }

                    ProviderImportFolder += "\\" + Guid.NewGuid();
                    if (!Directory.Exists(ProviderImportFolder))
                    {
                        Directory.CreateDirectory(ProviderImportFolder);
                    }
                }
                catch
                {
                    ModelState.AddModelError("ProviderFolder", AppGlobal.Language.GetText(this, "ProviderFolderCreareFailure", "Failed to create required folder."));
                }


                if (ModelState.IsValid && model.FileUpload != null && model.FileUpload.ContentLength > 0)
                {
                    String[] validFileTypes = { ".zip" };
                    Boolean validFileType = false;

                    foreach (String fileType in validFileTypes)
                    {
                        if (model.FileUpload.FileName.ToLower().EndsWith(fileType))
                        {
                            validFileType = true;
                            break;
                        }
                    }
                    if (!validFileType)
                    {
                        ModelState.AddModelError("File", AppGlobal.Language.GetText(this, "ZIPFilesOnly", "Please upload a ZIP file"));
                    }
                    else
                    {
                        String ZIPFile = String.Format(@"{0}\{1}", ProviderImportFolder, new FileInfo(model.FileUpload.FileName).Name);
                        model.FileUpload.SaveAs(ZIPFile);

                        FileInfo fileI = new FileInfo(ZIPFile);
                        // Unzip the file
                        System.IO.Compression.ZipFile.ExtractToDirectory(fileI.FullName, ProviderImportFolder);

                        // Delete the zip file
                        fileI.Delete();
                        int providersUpdated = 0;
                        String userId = Permission.GetCurrentUserId();

                        AddOrReplaceProgressMessage(AppGlobal.Language.GetText(this, "Importing", "Importing..."));
                        Boolean cancellingImport = false;
                        String importingProvidersMessage = AppGlobal.Language.GetText(this, "ImportingProviders", "Imported {0} Provider(s).  {1} Provider(s) Failed to Import");
                        String importCancelledMessageText = AppGlobal.Language.GetText(this, "ImportCancelled", "Provider Data Import Cancelled");
                        String importErrorMessageText = AppGlobal.Language.GetText(this, "ImportErrored", "Provider Data Import Stopped due to an Error: {0}");
                        String importErrorSendingWelcomeEmailsMessageText = AppGlobal.Language.GetText(this, "ErrorSendingWelcomeEmails", "Error Sending Welcome Emails: {0}");
                        String importSendingWelcomeEmailsMessageText = AppGlobal.Language.GetText(this, "ErrorSendingWelcomeEmails", "Error Sending Welcome Emails: {0}");
                        String emailOverrideRecipientText = AppGlobal.Language.GetText("TemplatedEmail_EmailOverride_FormatString", "<p>This email was originally sent to {0}:<p>{1}");
                        ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        String CallbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = "UserIdGoesHere", code = "CodeGoesHere" }, Request.Url == null ? "https" : Request.Url.Scheme);
                        new Thread(() =>
                        {
                            using (ProviderPortalEntities databaseContext = new ProviderPortalEntities())
                            {
                                try
                                {
                                    MetadataUpload metadataUpload = new MetadataUpload
                                    {
                                        MetadataUploadTypeId = (int) Constants.MetadataUploadType.ProviderImport,
                                        CreatedByUserId = userId,
                                        CreatedDateTimeUtc = DateTime.UtcNow,
                                        FileName = model.FileUpload.FileName,
                                        FileSizeInBytes = model.FileUpload.ContentLength,
                                        RowsBefore = databaseContext.Providers.Count()
                                    };

                                    Stopwatch sw = new Stopwatch();
                                    sw.Start();

                                    List<String> failedProviders = new List<String>();
                                    List<String> succeededProviders = new List<String>();
                                    List<String> usersToEmail = new List<String>();
                                    AspNetRole providerSuperuserRole = databaseContext.AspNetRoles.Find("5394B20B-1668-4D4C-AEE4-0FA057AC12B8");

                                    ImportBatch selectedBatch = databaseContext.ImportBatches.Find(model.ImportBatchId);
                                    if (selectedBatch == null)
                                    {
                                        throw new Exception("Unable to open selected import batch");
                                    }
                                    
                                    foreach (FileInfo csvFileInfo in new DirectoryInfo(ProviderImportFolder).GetFiles("*.csv"))
                                    {
                                        using (ProviderCsvReader csv = ProviderCsvReader.OpenCsvFile(csvFileInfo.FullName))
                                        {
                                            while (csv.Read())
                                            {
                                                String UKPRN = csv["UKPRN"].Trim();
                                                String providerName = csv["Provider Name"].Trim();
                                                String providerType = csv["Provider Type"].Trim();
                                                String UPIN = csv["UPIN"].Trim();
                                                String providersEmailAddress = csv["Providers Email Address"].Trim();
                                                String website = UrlHelper.GetFullUrl(csv["Website"].Trim());
                                                String telephone = csv["Telephone"].Trim();
                                                String usersEmailAddress = csv["Users Email Address"].Trim();
                                                String usersName = csv["Users Name"].Trim();
                                                String roATP = csv["RoATPFFlag"].Trim();

                                                #region Validate Record
                                                // Validate import data
                                                if (String.IsNullOrWhiteSpace(UKPRN))
                                                {
                                                    failedProviders.Add(providerName + ": No valid UKPRN number");
                                                    continue;
                                                }

                                                Int32 _UKPRN;
                                                if (!Int32.TryParse(UKPRN, out _UKPRN))
                                                {
                                                    failedProviders.Add(providerName + ": Invalid UKPRN - Not a number");
                                                    continue;
                                                }

                                                Int32 _UPIN = 0;
                                                if (!String.IsNullOrWhiteSpace(UPIN))
                                                {
                                                    if (UPIN.Length > 8)
                                                    {
                                                        failedProviders.Add(providerName + ": UPIN longer than the maximum 8 characters.");
                                                        continue;
                                                    }

                                                    if (!Int32.TryParse(UPIN, out _UPIN))
                                                    {
                                                        failedProviders.Add(providerName + ": Invalid UPIN - Not a number");
                                                        continue;
                                                    }
                                                }

                                                Int32 _ProviderType;
                                                if (!Int32.TryParse(providerType, out _ProviderType))
                                                {
                                                    failedProviders.Add(providerName + ": Invalid Provider Type - Not a number");
                                                    continue;
                                                }

                                                if (!IsValidEmail(usersEmailAddress))
                                                {
                                                    failedProviders.Add(providerName + ": Invalid user's email address or no user's email address supplied");
                                                    continue;
                                                }

                                                if (!IsValidEmail(providersEmailAddress, true))
                                                {
                                                    failedProviders.Add(providerName + ": Invalid provider email address.");
                                                    continue;
                                                }

                                                if (!IsValidUrl(website, true))
                                                {
                                                    failedProviders.Add(providerName + ": Invalid website address.");
                                                    continue;
                                                }

                                                #region Check Field Lengths
                                                // Check Field Lengths
                                                if (providerName.Length > 200)
                                                {
                                                    failedProviders.Add(providerName + ": Provider name longer than the maximum 200 characters.");
                                                    continue;
                                                }

                                                if (providersEmailAddress.Length > 255)
                                                {
                                                    failedProviders.Add(providerName + ": Provider's email address longer than the maximum 255 characters.");
                                                    continue;
                                                }

                                                if (website.Length > 255)
                                                {
                                                    failedProviders.Add(providerName + ": Website longer than the maximum 255 characters.");
                                                    continue;
                                                }

                                                if (telephone.Length > 30)
                                                {
                                                    failedProviders.Add(providerName + ": Telephone longer than the maximum 30 characters.");
                                                    continue;
                                                }

                                                if (usersEmailAddress.Length > 256)
                                                {
                                                    failedProviders.Add(providerName + ": User's email address longer than the maximum 256 characters.");
                                                    continue;
                                                }

                                                if (String.IsNullOrWhiteSpace(usersName))
                                                {
                                                    failedProviders.Add(providerName + ": User's name not supplied.");
                                                    continue;
                                                }

                                                if (usersName.Length > 4000)
                                                {
                                                    failedProviders.Add(providerName + ": User's name longer than the maximum 4000 characters.");
                                                    continue;
                                                }

                                                #endregion

                                                #endregion

                                                //Does this provider already exist in the database (or in the unsaved data set)
                                                Provider existingProvider = databaseContext.Providers.FirstOrDefault(x => x.Ukprn == _UKPRN) ?? databaseContext.Providers.Local.FirstOrDefault(x => x.Ukprn == _UKPRN);
                                                if (existingProvider == null)
                                                {
                                                    //Get Address details
                                                    Ukrlp ukrlp = databaseContext.Ukrlps.Find(_UKPRN);
                                                    if (ukrlp == null)
                                                    {
                                                        failedProviders.Add(providerName + ": UKPRN not found in UKRLP.");
                                                        continue;
                                                    }
                                                    if (ukrlp.LegalAddress == null)
                                                    {
                                                        failedProviders.Add(providerName + ": No valid ukrlp address.");
                                                        continue;
                                                    }

                                                    Address address = ukrlp.LegalAddress.Clone();
                                                    // Use postcode to get lat / long 
                                                    // we may already have this but get it again just incase we have an updated lat/lng
                                                    GeoLocation geo = databaseContext.GeoLocations.Find(address.Postcode);
                                                    if (geo != null)
                                                    {
                                                        address.Latitude = geo.Lat;
                                                        address.Longitude = geo.Lng;
                                                    }

                                                    try
                                                    {
                                                        //Now get provider details
                                                        Provider provider = new Provider();
                                                        if (_ProviderType <= 0 || _ProviderType > 9)
                                                        {
                                                            failedProviders.Add(providerName + ": Failed to create provider. Provider type not in permitted range.");
                                                            continue;
                                                        }
                                                        provider.Ukprn = _UKPRN;
                                                        provider.ProviderName = String.IsNullOrWhiteSpace(providerName) ? ukrlp.LegalName : providerName;
                                                        provider.ProviderTypeId = _ProviderType;
                                                        provider.UPIN = _UPIN == 0 ? (Int32?)null : _UPIN;
                                                        provider.Email = providersEmailAddress;
                                                        provider.Website = website;
                                                        provider.Telephone = telephone;
                                                        provider.CreatedDateTimeUtc = DateTime.UtcNow;
                                                        provider.CreatedByUserId = userId;
                                                        provider.RoATPFFlag = roATP == "1";
                                                        //These can be null
                                                        provider.ModifiedDateTimeUtc = null;
                                                        provider.ModifiedByUserId = null;
                                                        provider.ProviderRegionId = null;
                                                        provider.ProviderTrackingUrl = null;
                                                        provider.VenueTrackingUrl = null;
                                                        provider.CourseTrackingUrl = null;
                                                        provider.BookingTrackingUrl = null;
                                                        provider.RelationshipManagerUserId = null;
                                                        provider.InformationOfficerUserId = null;
                                                        provider.CourseTrackingUrl = null;
                                                        provider.ProviderNameAlias = null;
                                                        provider.Fax = null;
                                                        provider.FeChoicesLearner = null;
                                                        provider.FeChoicesEmployer = null;
                                                        provider.FeChoicesDestination = null;
                                                        provider.FeChoicesUpdatedDateTimeUtc = null;
                                                        provider.QualityEmailStatusId = null;
                                                        provider.TrafficLightEmailDateTimeUtc = null;
                                                        provider.DfENumber = null;
                                                        provider.DfEUrn = null;
                                                        provider.DfEProviderTypeId = null;
                                                        provider.DfEProviderStatusId = null;
                                                        provider.DfELocalAuthorityId = null;
                                                        provider.DfERegionId = null;
                                                        provider.DfEEstablishmentTypeId = null;
                                                        provider.DfEEstablishmentNumber = null;
                                                        provider.StatutoryLowestAge = null;
                                                        provider.StatutoryHighestAge = null;
                                                        provider.AgeRange = null;
                                                        provider.AnnualSchoolCensusLowestAge = null;
                                                        provider.AnnualSchoolCensusHighestAge = null;
                                                        provider.CompanyRegistrationNumber = null;
                                                        provider.Uid = null;
                                                        provider.SecureAccessId = null;
                                                        provider.MarketingInformation = null;

                                                        //These are set to the default value
                                                        provider.Loans24Plus = false;
                                                        provider.RecordStatusId = (int) Constants.RecordStatus.Live;
                                                        provider.IsContractingBody = false;
                                                        provider.QualityEmailsPaused = false;
                                                        provider.DFE1619Funded = false;
                                                        provider.SFAFunded = true;
                                                        provider.BulkUploadPending = false;
                                                        provider.PublishData = true;
                                                        provider.NationalApprenticeshipProvider = false;
                                                        provider.ApprenticeshipContract = false;

                                                        Address providerAddress = address.Clone();
                                                        databaseContext.Addresses.Add(address);  //Users address
                                                        databaseContext.Addresses.Add(providerAddress);
                                                        provider.Address = providerAddress;

                                                        try
                                                        {
                                                            String errMessage = String.Empty;
                                                            AspNetUser aspNetUser = GetOrCreateUser(databaseContext, usersEmailAddress, usersName, address, userId, providerSuperuserRole, 0, out errMessage);
                                                            if (aspNetUser == null)
                                                            {
                                                                failedProviders.Add(providerName + " not created. " + errMessage);
                                                                continue;
                                                            }
                                                            provider.AspNetUsers.Add(aspNetUser);
                                                            usersToEmail.Add(aspNetUser.Id);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            failedProviders.Add(String.Format(providerName + " not created. Error creating user account - {0}.", ex.Message));
                                                            continue;
                                                        }

                                                        ImportBatchProvider importBatchProvider = new ImportBatchProvider()
                                                        {
                                                            ImportBatch = selectedBatch,
                                                            HasProviderLevelData = false,
                                                            HasApprenticeshipLevelData = false,
                                                            ExistingProvider = false,
                                                            ImportDateTimeUtc = DateTime.UtcNow
                                                        };
                                                        databaseContext.Entry(importBatchProvider).State = EntityState.Added;
                                                        provider.ImportBatches.Add(importBatchProvider);

                                                        databaseContext.Providers.Add(provider);

                                                        succeededProviders.Add(String.Format("{0} ({1})", provider.ProviderName, usersEmailAddress));
                                                        providersUpdated += 1;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        failedProviders.Add(String.Format(providerName + " not created. Error creating provider - {0}.", ex.Message));
                                                    }
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        Provider provider = existingProvider;
                                                        if (roATP == "1")
                                                        {
                                                            provider.RoATPFFlag = true;
                                                        }
                                                        if (provider.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
                                                        {
                                                            provider.ProviderName = providerName;
                                                            provider.Telephone = telephone;
                                                            provider.Website = website;
                                                            provider.Email = providersEmailAddress;
                                                            provider.RecordStatusId = (Int32)Constants.RecordStatus.Live;                                                            
                                                        }
                                                        provider.ModifiedByUserId = userId;
                                                        provider.ModifiedDateTimeUtc = DateTime.UtcNow;

                                                        Address address = provider.Address.Clone();

                                                        Boolean error = false;
                                                        try
                                                        {
                                                            String errMessage = String.Empty;
                                                            AspNetUser aspNetUser = GetOrCreateUser(databaseContext, usersEmailAddress, usersName, address, userId, providerSuperuserRole, provider.ProviderId, out errMessage);
                                                            if (aspNetUser == null)
                                                            {
                                                                failedProviders.Add(providerName + " already exists on portal. " + errMessage);
                                                                error = true;
                                                            }
                                                            else
                                                            {
                                                                provider.AspNetUsers.Add(aspNetUser);
                                                                usersToEmail.Add(aspNetUser.Id);
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            failedProviders.Add(String.Format(providerName + " already exists on the portal. Error creating user account - {0}.", ex.Message));
                                                            error = true;
                                                        }

                                                        ImportBatchProvider importBatchProvider = existingProvider.ImportBatches.FirstOrDefault(x => x.ImportBatch.ImportBatchId == selectedBatch.ImportBatchId);
                                                        if (importBatchProvider is null)
                                                        {
                                                            importBatchProvider = new ImportBatchProvider()
                                                            {
                                                                ImportBatch = selectedBatch,
                                                                HasProviderLevelData = !String.IsNullOrWhiteSpace(existingProvider.MarketingInformation),
                                                                HasApprenticeshipLevelData = existingProvider.Apprenticeships.Count > 0,
                                                                ExistingProvider = true,
                                                                ImportDateTimeUtc = DateTime.UtcNow
                                                            };
                                                            databaseContext.Entry(importBatchProvider).State = EntityState.Added;
                                                            provider.ImportBatches.Add(importBatchProvider);
                                                        }

                                                        if (provider.ProviderId != 0)
                                                        {
                                                            databaseContext.Providers.Attach(provider);
                                                            databaseContext.Entry(provider).State = EntityState.Modified;
                                                            if (!error)
                                                            {
                                                                succeededProviders.Add(String.Format("{0} ({1})", provider.ProviderName, usersEmailAddress));
                                                                providersUpdated += 1;
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        failedProviders.Add(String.Format(providerName + " already exists on the portal. Error updating provider = {0}.", ex.Message));
                                                    }
                                                }

                                                // Every 10 rows, check whether we are cancelling the import
                                                if ((csv.Row - 1)%10 == 0)
                                                {
                                                    if (IsCancellingImport())
                                                    {
                                                        cancellingImport = true;
                                                        break;
                                                    }

                                                    AddOrReplaceProgressMessage(String.Format(importingProvidersMessage, providersUpdated.ToString("N0"), failedProviders.Count.ToString("N0")));
                                                }
                                            }
                                        }

                                        csvFileInfo.Delete();
                                    }

                                    if (ModelState.IsValid && !cancellingImport)
                                    {
                                        sw.Stop();
                                        metadataUpload.DurationInMilliseconds = (int) sw.ElapsedMilliseconds;
                                        metadataUpload.RowsAfter = databaseContext.Providers.Count();
                                        databaseContext.MetadataUploads.Add(metadataUpload);

                                        Directory.Delete(ProviderImportFolder);

                                        // Write database changes
                                        databaseContext.SaveChanges();

                                        try
                                        {
                                            AddOrReplaceProgressMessage(importSendingWelcomeEmailsMessageText);

                                            // Send the welcome emails
                                            foreach (String uId in usersToEmail)
                                            {
                                                String code = HttpUtility.UrlEncode(userManager.GenerateEmailConfirmationTokenAsync(uId).Result);
                                                String callbackUrl = CallbackUrl.Replace("UserIdGoesHere", uId).Replace("CodeGoesHere", code);

                                                //AppGlobal.EmailQueue.AddToSendQueue(TemplatedEmail.EmailMessage(
                                                //    uId,
                                                //    Constants.EmailTemplates.NewUserWelcome,
                                                //    new List<EmailParameter>
                                                //    {
                                                //        new EmailParameter("%URL%", callbackUrl),
                                                //        new EmailParameter("%RESENT%", "")
                                                //    },
                                                //    emailOverrideRecipientText)
                                                //    );

                                                var emailMessage = TemplatedEmail.EmailMessage(
                                                    uId,
                                                    Constants.EmailTemplates.NewUserWelcome,
                                                    new List<EmailParameter>
                                                    {
                                                        new EmailParameter("%URL%", callbackUrl),
                                                        new EmailParameter("%RESENT%", "")
                                                    },
                                                    emailOverrideRecipientText);

                                                var response = SfaSendGridClient.SendGridEmailMessage(emailMessage, uId);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            AddOrReplaceProgressMessage(String.Format(importErrorSendingWelcomeEmailsMessageText, ex.Message), true);
                                        }

                                        String strMessage = String.Format("Provider Data: {0} provider{1} successfully imported.", providersUpdated.ToString("N0"), providersUpdated == 1 ? "" : "s");
                                        if (failedProviders.Count > 0)
                                        {
                                            strMessage += "<br /><br /> The following providers were <strong>not</strong> imported: <br /><br /> ";
                                            foreach (String failure in failedProviders)
                                            {
                                                strMessage += failure + " <br/>";
                                            }
                                        }
                                        if (succeededProviders.Count > 0)
                                        {
                                            strMessage += "<br /><br /> The following providers were imported successfully: <br /><br /> ";
                                            foreach (String success in succeededProviders)
                                            {
                                                strMessage += success + " <br/>";
                                            }
                                        }

                                        AddOrReplaceProgressMessage(strMessage, true);
                                    }
                                    else
                                    {
                                        sw.Stop();
                                        AddOrReplaceProgressMessage(importCancelledMessageText, true);
                                    }
                                }
                                catch (DbEntityValidationException ex)
                                {
                                    AddOrReplaceProgressMessage(String.Format(importErrorMessageText, ex.Message), true);
                                }
                                catch (Exception ex)
                                {
                                    AddOrReplaceProgressMessage(String.Format(importErrorMessageText, ex.Message), true);
                                }
                            }
                        }).Start();
                    }
                }
            }

            GetViewData();

            return this.View();
        }

        [NonAction]
        private static AspNetUser GetOrCreateUser(ProviderPortalEntities databaseContext, String usersEmailAddress, String usersName, Address address, String createdByUserId, AspNetRole providerSuperuserRole, Int32 providerId, out String errMessage)
        {
            Address oldExistingUserAddress = null;
            AspNetUser aspNetUser = null;

            errMessage = String.Empty;

            // Check if the user exists in the database (or in the unsaved data set)
            AspNetUser existingUser = databaseContext.AspNetUsers.FirstOrDefault(x => x.UserName == usersEmailAddress) ?? databaseContext.AspNetUsers.Local.FirstOrDefault(x => x.UserName == usersEmailAddress); 
            if (existingUser == null)
            {
                aspNetUser = new AspNetUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = usersEmailAddress,
                    Email = usersEmailAddress,
                    Address = address,
                    Name = usersName,
                    PhoneNumber = null,
                    PasswordResetRequired = true,
                    CreatedDateTimeUtc = DateTime.UtcNow,
                    CreatedByUserId = createdByUserId,
                    IsSecureAccessUser = false,
                    SecureAccessUserId = null,
                    ProviderUserTypeId = (int)Constants.ProviderUserTypes.NormalUser,
                    EmailConfirmed = false,
                    IsDeleted = false,
                    ModifiedDateTimeUtc = null,
                    ModifiedByUserId = null,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
            }
            else if (!existingUser.IsSecureAccessUser && existingUser.Providers2.All(p => p.RecordStatu.IsDeleted) && existingUser.Providers2.Any())
            {
                //June 2017 If old provider has been deleted move user from old provider to new provider and set user as new
                oldExistingUserAddress = existingUser.Address;
                existingUser.Name = usersName;
                existingUser.PasswordResetRequired = true;
                existingUser.Address = address;
                existingUser.EmailConfirmed = false;
                existingUser.IsDeleted = false;
                existingUser.ModifiedDateTimeUtc = DateTime.UtcNow;
                existingUser.ModifiedByUserId = createdByUserId;

                List<Provider> existingProviders = existingUser.Providers2.ToList();
                foreach (Provider p in existingProviders)
                {
                    existingUser.Providers2.Remove(p);
                }
            }
            else
            {
                if (!existingUser.Providers2.Any())
                {
                    errMessage = String.Format("User {0} already exists but is not linked to any providers (May be an administrator user or an Organisation user).", usersEmailAddress);
                }
                else if (!existingUser.Providers2.All(p => p.RecordStatu.IsDeleted))
                {
                    if (existingUser.Providers2.Any(x => x.ProviderId == providerId))
                    {
                        errMessage = String.Format("User {0} already exists at this provider.", usersEmailAddress);
                    }
                    else
                    {
                        errMessage = String.Format("User {0} already exists at another provider which is not deleted.", usersEmailAddress);
                    }
                }
                else if (existingUser.IsSecureAccessUser)
                {
                    errMessage = String.Format("User {0} already exists at a deleted provider but is a Secure Access user and therefore cannot be moved to the new provider.", usersEmailAddress);
                }
                existingUser = null;
            }

            if (aspNetUser != null)
            {
                //Original code to create new user and assign to provider
                databaseContext.AspNetUsers.Add(aspNetUser);
                aspNetUser.AspNetRoles.Add(providerSuperuserRole);
            }
            else if (existingUser != null)
            {
                //June 2017 Update, assign user to new provider
                databaseContext.Addresses.Remove(oldExistingUserAddress);
                if (!existingUser.AspNetRoles.Contains(providerSuperuserRole))
                {
                    existingUser.AspNetRoles.Add(providerSuperuserRole);
                }
            }

            return existingUser ?? aspNetUser;
        }

        /// <summary>
        /// Is the string a valid email address
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="isOptional">Indicates whether the value can be blank</param>
        /// <returns></returns>
        [NonAction]
        private static Boolean IsValidEmail(String value, Boolean isOptional = false)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return isOptional;
            }

            return TestRegex(value, @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
        }

        /// <summary>
        /// Is the string a valid URL
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isOptional"></param>
        /// <returns></returns>
        [NonAction]
        private static Boolean IsValidUrl(String value, Boolean isOptional = false)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return isOptional;
            }

            return TestRegex(value, @"^http(s)?://(((?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)");
        }
        
        /// <summary>
        /// Test a value against a regular expression, failing out if it takes longer than a second to evaluate
        /// </summary>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        [NonAction]
        private static Boolean TestRegex(String value, String expression)
        {
            try
            {
                var matchTimeout = TimeSpan.FromSeconds(1);
                var regex = new Regex(expression);
                return Regex.IsMatch(value.Trim(), regex.ToString(), RegexOptions.IgnoreCase, matchTimeout);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        [NonAction]
        private void GetViewData()
        {
            ViewBag.IsComplete = true;
            ViewBag.IsCancelling = false;
            PopulateBatchNames();

            ProgressMessage pm = db.ProgressMessages.Find(MessageArea);
            if (pm != null)
            {
                ViewBag.Message = pm.MessageText;
                ViewBag.IsComplete = pm.IsComplete;
                ViewBag.IsCancelling = pm.MessageText.Equals(cancelImportMessage);

                if (pm.IsComplete)
                {
                    db.Entry(pm).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }

            String ProviderImportFolder = Constants.ConfigSettings.ProviderImportFilesVirtualDirectoryName;
            if (ProviderImportFolder.EndsWith(@"\"))
            {
                ProviderImportFolder = ProviderImportFolder.Substring(0, ProviderImportFolder.Length - 1);
            }

            // Check if config setting is valid
            if (String.IsNullOrEmpty(ProviderImportFolder) || !Directory.Exists(ProviderImportFolder))
            {
                ModelState.AddModelError("", AppGlobal.Language.GetText(this, "ProviderImportFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringProviderImportFiles is not set or is incorrect"));
                DeleteProgressMessage();
            }
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadUCASData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult CancelImport()
        {
            ProgressMessage pm = db.ProgressMessages.Find(MessageArea);
            if (pm != null && !pm.IsComplete)
            {
                AddOrReplaceProgressMessage(cancelImportMessage);
            }

            return RedirectToAction("Import");
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanUploadUCASData)]
        [ContextAuthorize(UserContext.UserContextName.Administration)]
        public ActionResult ForceCancelImport()
        {
            DeleteProgressMessage();

            String ProviderImportDataFolder = Constants.ConfigSettings.ProviderImportFilesVirtualDirectoryName;
            if (ProviderImportDataFolder.EndsWith(@"\"))
            {
                ProviderImportDataFolder = ProviderImportDataFolder.Substring(0, ProviderImportDataFolder.Length - 1);
            }

            // Check if config setting is valid
            if (String.IsNullOrEmpty(ProviderImportDataFolder) || !Directory.Exists(ProviderImportDataFolder))
            {
                ModelState.AddModelError("", AppGlobal.Language.GetText(this, "ProviderImportFolderNotConfigured", "Configuration setting VirtualDirectoryNameForStoringProviderImportFiles is not set or is incorrect"));
                DeleteProgressMessage();
            }

            foreach (FileInfo file in new DirectoryInfo(ProviderImportDataFolder).GetFiles())
            {
                file.Delete();
            }

            return RedirectToAction("Import");
        }

        [NonAction]
        private Boolean IsCancellingImport()
        {
            // Create a new ProviderPortalEntities object so that it doesn't get messed up with the transaction
            using (ProviderPortalEntities _db = new ProviderPortalEntities())
            {
                // Check if we have stopped the import
                ProgressMessage pm = _db.ProgressMessages.FirstOrDefault(x => x.MessageArea == MessageArea);
                return pm == null || pm.MessageText == cancelImportMessage;
            }
        }

        [NonAction]
        private static void DeleteProgressMessage()
        {
            using (ProviderPortalEntities _db = new ProviderPortalEntities())
            {
                ProgressMessage pm = _db.ProgressMessages.Find(MessageArea);
                if (pm != null)
                {
                    _db.Entry(pm).State = EntityState.Deleted;
                    _db.SaveChanges();
                }
            }
        }

        [NonAction]
        private static void AddOrReplaceProgressMessage(String message, Boolean isComplete = false)
        {
            using (ProviderPortalEntities _db = new ProviderPortalEntities())
            {
                ProgressMessage pm = _db.ProgressMessages.Find(MessageArea);
                if (pm != null)
                {
                    _db.Entry(pm).State = EntityState.Modified;
                }
                else
                {
                    pm = new ProgressMessage
                    {
                        MessageArea = MessageArea
                    };
                    _db.Entry(pm).State = EntityState.Added;
                }
                pm.MessageText = message;
                pm.IsComplete = isComplete;

                _db.SaveChanges();
            }
        }

        private void PopulateBatchNames()
        {
            ViewBag.BatchNames = db.ImportBatches
                .OrderByDescending(x => x.Current)
                .ThenByDescending(x => x.ImportBatchName)
                .Select(x => new SelectListItem
                {
                    Value = x.ImportBatchId.ToString(),
                    Text = x.ImportBatchName + (x.Current ? " (current)" : "")
                }).ToList();
        }
    }
   
    sealed class ProviderCsvReader : CsvReader
    {
        private static readonly System.Text.Encoding encoding = System.Text.Encoding.UTF7;

        public static ProviderCsvReader OpenCsvFile(String filename)
        {
            StreamReader sr = new StreamReader(filename, encoding);
            return new ProviderCsvReader(sr);
        }

        private ProviderCsvReader(TextReader sr)
            : base(sr)
        {
            Configuration.Encoding = encoding;
            Configuration.IgnoreBlankLines = true;
            Configuration.IsHeaderCaseSensitive = false;
        }
    }
}