using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public class OpportunityController : BaseController
    {
        public const Int32 CreateNewVenueBasedOnProviderAddressId = -1;
        public const String StartMonthFormat = "MMM-yyyy";

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Create(Int32 id)
        {
            Course course = db.Courses.Find(id);
            if (course == null || course.ProviderId != userContext.ItemId || course.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            AddEditOpportunityModel model = new AddEditOpportunityModel
            {
                CourseId = id,
                A10FundingCodes = db.A10FundingCode.Where(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live || x.RecordStatusId == (Int32)Constants.RecordStatus.Deleted),
                SelectedA10FundingCodes = new List<Int32>(),
                ProviderId = course.ProviderId,
                ProviderName = course.Provider.ProviderName,
                IsInOrganisation = course.Provider.OrganisationProviders.Count > 0
            };

            // Populate drop downs
            GetLookups(model, course.ProviderId);

            ViewBag.DateFormat = Constants.ConfigSettings.ShortDateFormat;
            ViewBag.StartMonthFormat = StartMonthFormat;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public async Task<ActionResult> Create(Int32 id, AddEditOpportunityModel model)
        {
            Course course = db.Courses.Find(id);
            if (course == null || course.ProviderId != userContext.ItemId)
            {
                return HttpNotFound();
            }

            model.CourseId = course.CourseId;

            // Validate the model
            CheckModel(model);

            if (ModelState.IsValid)
            {
                CourseInstance courseInstance = model.ToEntity(db);

                courseInstance.RecordStatusId = (Int32)Constants.RecordStatus.Live;
                courseInstance.AddedByApplicationId = (Int32)Constants.Application.Portal;

                // Add the Start Dates
                if (!String.IsNullOrEmpty(model.StartDate))
                {
                    foreach (String strStart in model.StartDate.Split(','))
                    {
                        DateTime dtStart;
                        if (DateTime.TryParseExact(strStart.Trim(), Constants.ConfigSettings.ShortDateFormat, null, System.Globalization.DateTimeStyles.None, out dtStart))
                        {
                            CourseInstanceStartDate startDate = new CourseInstanceStartDate
                            {
                                StartDate = dtStart,
                                IsMonthOnlyStartDate = false
                            };
                            courseInstance.CourseInstanceStartDates.Add(startDate);
                        }
                    }
                }

                // Add the Start Dates by Month
                if (!String.IsNullOrEmpty(model.StartMonth))
                {
                    foreach (String strStart in model.StartMonth.Split(','))
                    {
                        DateTime dtStart;
                        if (DateTime.TryParseExact(strStart.Trim(), StartMonthFormat, null, System.Globalization.DateTimeStyles.None, out dtStart))
                        {
                            CourseInstanceStartDate startDate = new CourseInstanceStartDate
                            {
                                StartDate = dtStart,
                                IsMonthOnlyStartDate = true
                            };
                            courseInstance.CourseInstanceStartDates.Add(startDate);
                        }
                    }
                }

                // Add the funding codes
                foreach (Int32 fcId in model.SelectedA10FundingCodes)
                {
                    A10FundingCode fc = db.A10FundingCode.Find(fcId);
                    if (fc != null)
                    {
                        courseInstance.A10FundingCode.Add(fc);
                    }
                }

                // Add the Venue(s)?
                if (model.VenueId.HasValue)
                {
                    if (model.VenueId == CreateNewVenueBasedOnProviderAddressId)
                    {
                        Provider provider = db.Providers.Find(course.ProviderId);
                        if (provider == null)
                        {
                            return HttpNotFound();
                        }
                        Venue venue = CreateVenueBasedOnProvider(provider);
                        db.Entry(venue).State = EntityState.Added;
                        courseInstance.Venues.Add(venue);
                    }
                    else
                    {
                        Venue venue = db.Venues.Find(model.VenueId);
                        if (venue != null)
                        {
                            courseInstance.Venues.Add(venue);
                        }
                    }
                }

                // Set Location
                if (model.RegionId.HasValue)
                {
                    VenueLocation location = db.VenueLocations.Find(model.RegionId);
                    if (location != null)
                    {
                        courseInstance.VenueLocation = location;
                    }
                }
                else
                {
                    courseInstance.VenueLocation = null;
                }

                // Update course properties
                course.RecordStatusId = (Int32)Constants.RecordStatus.Live;
                course.ModifiedDateTimeUtc = DateTime.UtcNow;
                course.ModifiedByUserId = Permission.GetCurrentUserId();
                db.Entry(course).State = EntityState.Modified;

                db.CourseInstances.Add(courseInstance);

                // Archive the old opportunity if required
                if (model.DuplicatingOpportunityId.HasValue && model.ArchiveOldOpportunity.HasValue && model.ArchiveOldOpportunity.Value == 1)
                {
                    CourseInstance oldCourseInstance = db.CourseInstances.Find(model.DuplicatingOpportunityId);
                    if (oldCourseInstance != null && oldCourseInstance.CourseId == course.CourseId)
                    {
                        oldCourseInstance.Archive(db);
                    }
                }

                await db.SaveChangesAsync();

                List<String> messages = model.GetWarningMessages();
                if (messages.Count == 0)
                {
                    ShowGenericSavedMessage();
                }
                else
                {
                    // Add a blank entry at the beginning so the String.Join starts with <br /><br />
                    messages.Insert(0, "");
                    SessionMessage.SetMessage(AppGlobal.Language.GetText(this, "SaveSuccessfulWithWarnings", "Your changes were saved successfully with the following warnings:") + String.Join("<br /><br />", messages), SessionMessageType.Success);
                }

                return RedirectToAction("Edit", "Course", new { Id = courseInstance.CourseId });
            }

            // Populate Required Model Properties
            model.A10FundingCodes = db.A10FundingCode.Where(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live || x.RecordStatusId == (Int32)Constants.RecordStatus.Deleted);
            model.ProviderId = course.ProviderId;
            model.IsInOrganisation = course.Provider.OrganisationProviders.Count > 0;
            model.ProviderName = course.Provider.ProviderName;

            // Populate drop downs
            GetLookups(model, course.ProviderId);

            ViewBag.DateFormat = Constants.ConfigSettings.ShortDateFormat;
            ViewBag.StartMonthFormat = StartMonthFormat;

            return View(model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult View(Int32 id)
        {
            CourseInstance courseInstance = db.CourseInstances.Find(id);
            if (courseInstance == null || courseInstance.Course.ProviderId != userContext.ItemId || courseInstance.Course.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            ViewOpportunityModel model = new ViewOpportunityModel(courseInstance)
            {
                A10FundingCodes = db.A10FundingCode.Where(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live)
            };

            ViewBag.DateFormat = Constants.ConfigSettings.ShortDateFormat;
            ViewBag.StartMonthFormat = StartMonthFormat;

            return View(model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Duplicate(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            CourseInstance courseInstance = db.CourseInstances.Find(id);
            if (courseInstance == null || courseInstance.Course.ProviderId != userContext.ItemId || courseInstance.Course.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            AddEditOpportunityModel model = new AddEditOpportunityModel(courseInstance)
            {
                A10FundingCodes = db.A10FundingCode.Where(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live)
            };

            model.DuplicatingOpportunityId = id;

            // Populate drop downs
            GetLookups(model, courseInstance.Course.ProviderId);

            ViewBag.DateFormat = Constants.ConfigSettings.ShortDateFormat;
            ViewBag.StartMonthFormat = StartMonthFormat;

            return View("Create", model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Edit(Int32? id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            CourseInstance courseInstance = db.CourseInstances.Find(id);
            if (courseInstance == null || courseInstance.Course.ProviderId != userContext.ItemId || courseInstance.Course.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            AddEditOpportunityModel model = new AddEditOpportunityModel(courseInstance)
            {
                A10FundingCodes = db.A10FundingCode.Where(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live || x.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            };

            // If there are no selected A10Funding Codes then ensure that the N/A option is selected
            if (model.SelectedA10FundingCodes.Count == 0)
            {
                model.SelectedA10FundingCodes.Add(-1);
            }

            // Populate drop downs
            GetLookups(model, courseInstance.Course.ProviderId);

            ViewBag.DateFormat = Constants.ConfigSettings.ShortDateFormat;
            ViewBag.StartMonthFormat = StartMonthFormat;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public async Task<ActionResult> Edit(Int32 id, AddEditOpportunityModel model)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (model.OpportunityId != id)
            {
                return HttpNotFound();
            }

            CourseInstance courseInstance = db.CourseInstances.Find(id);
            if (courseInstance == null)
            {
                return HttpNotFound();
            }

            // Validate the model
            CheckModel(model);

            if (ModelState.IsValid)
            {
                courseInstance = model.ToEntity(db);
                if (courseInstance.Course.ProviderId != userContext.ItemId)
                {
                    // User is trying to change the OpportunityId (or the context has changed)
                    return HttpNotFound();
                }

                courseInstance.ModifiedByUserId = Permission.GetCurrentUserId();
                courseInstance.ModifiedDateTimeUtc = DateTime.UtcNow;

                // Remove Existing Funding Codes
                List<A10FundingCode> existingA10FundingCodes = courseInstance.A10FundingCode.ToList();
                foreach (A10FundingCode a10 in existingA10FundingCodes.Where(a10 => !model.SelectedA10FundingCodes.Contains(a10.A10FundingCodeId)))
                {
                    courseInstance.A10FundingCode.Remove(a10);
                }

                // Add the funding codes
                foreach (Int32 fcId in model.SelectedA10FundingCodes)
                {
                    A10FundingCode fc = courseInstance.A10FundingCode.FirstOrDefault(x => x.A10FundingCodeId == fcId);
                    if (fc == null)
                    {
                        fc = db.A10FundingCode.Find(fcId);
                        if (fc != null)
                        {
                            courseInstance.A10FundingCode.Add(fc);
                        }
                    }
                }

                // Remove Current Start Dates
                List<CourseInstanceStartDate> existingStartDates = courseInstance.CourseInstanceStartDates.ToList();
                String[] strDates = new String[0];
                if (!String.IsNullOrEmpty(model.StartDate))
                {
                    strDates = model.StartDate.Split(',');
                }
                if (!String.IsNullOrEmpty(model.StartMonth))
                {
                    strDates = model.StartMonth.Split(',');
                }
                foreach (CourseInstanceStartDate sd in existingStartDates.Where(sd => !strDates.Contains(sd.ToFormattedString())))
                {
                    courseInstance.CourseInstanceStartDates.Remove(sd);
                    db.CourseInstanceStartDates.Remove(sd);
                }

                // Add the Start Dates
                if (!String.IsNullOrEmpty(model.StartDate))
                {
                    foreach (String strStart in model.StartDate.Split(','))
                    {
                        CourseInstanceStartDate cisd = courseInstance.CourseInstanceStartDates.FirstOrDefault(x => x.ToFormattedString() == strStart);
                        if (cisd == null)
                        {
                            DateTime dtStart;
                            if (DateTime.TryParseExact(strStart.Trim(), Constants.ConfigSettings.ShortDateFormat, null, System.Globalization.DateTimeStyles.None, out dtStart))
                            {
                                CourseInstanceStartDate startDate = new CourseInstanceStartDate
                                {
                                    StartDate = dtStart,
                                    IsMonthOnlyStartDate = false
                                };
                                courseInstance.CourseInstanceStartDates.Add(startDate);
                            }
                        }
                    }
                }

                // Add the Start Dates by Month
                if (!String.IsNullOrEmpty(model.StartMonth))
                {
                    foreach (String strStart in model.StartMonth.Split(','))
                    {
                        CourseInstanceStartDate cisd = courseInstance.CourseInstanceStartDates.FirstOrDefault(x => x.ToFormattedString() == strStart);
                        if (cisd == null)
                        {
                            DateTime dtStart;
                            if (DateTime.TryParseExact(strStart.Trim(), StartMonthFormat, null, System.Globalization.DateTimeStyles.None, out dtStart))
                            {
                                CourseInstanceStartDate startDate = new CourseInstanceStartDate
                                {
                                    StartDate = dtStart,
                                    IsMonthOnlyStartDate = true
                                };
                                courseInstance.CourseInstanceStartDates.Add(startDate);
                            }
                        }
                    }
                }

                // Delete Existing Venue(s)?
                foreach (Venue venue in courseInstance.Venues.Where(venue => venue.VenueId != (model.VenueId ?? -1)).ToList())
                {
                    courseInstance.Venues.Remove(venue);
                }

                // Add the Venue(s)?
                if (model.VenueId.HasValue)
                {
                    if (model.VenueId == CreateNewVenueBasedOnProviderAddressId)
                    {
                        Venue venue = CreateVenueBasedOnProvider(provider);
                        db.Entry(venue).State = EntityState.Added;
                        courseInstance.Venues.Add(venue);
                    }
                    else
                    {
                        Venue venue = db.Venues.Find(model.VenueId);
                        if (venue != null)
                        {
                            if (!courseInstance.Venues.Contains(venue))
                            {
                                courseInstance.Venues.Add(venue);
                            }
                        }
                    }
                }

                // Set Location
                if (model.RegionId.HasValue)
                {
                    VenueLocation location = db.VenueLocations.Find(model.RegionId);
                    if (location != null)
                    {
                        courseInstance.VenueLocation = location;
                    }
                }
                else
                {
                    courseInstance.VenueLocation = null;
                }

                db.Entry(courseInstance).State = EntityState.Modified;

                // Update course properties
                if (courseInstance.RecordStatusId == (Int32) Constants.RecordStatus.Live)
                {
                    courseInstance.Course.RecordStatusId = (Int32) Constants.RecordStatus.Live;
                }
                courseInstance.Course.ModifiedDateTimeUtc = DateTime.UtcNow;
                courseInstance.Course.ModifiedByUserId = Permission.GetCurrentUserId();
                courseInstance.AddedByApplicationId = (Int32)Constants.Application.Portal;
                db.Entry(courseInstance.Course).State = EntityState.Modified;

                await db.SaveChangesAsync();

                List<String> messages = model.GetWarningMessages();
                if (messages.Count == 0)
                {
                    ShowGenericSavedMessage();
                }
                else
                {
                    // Add a blank entry at the beginning so the String.Join starts with <br /><br />
                    messages.Insert(0, "");
                    SessionMessage.SetMessage(AppGlobal.Language.GetText(this, "SaveSuccessfulWithWarnings", "Your changes were saved successfully with the following warnings:") + String.Join("<br /><br />", messages), SessionMessageType.Success);
                }

                return RedirectToAction("Edit", "Course", new { Id = courseInstance.CourseId });
            }

            // Populate Required Model Properties
            model.A10FundingCodes = db.A10FundingCode.Where(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live || x.RecordStatusId == (Int32)Constants.RecordStatus.Deleted);
            model.ProviderId = courseInstance.Course.ProviderId;
            model.IsInOrganisation = courseInstance.Course.Provider.OrganisationProviders.Count > 0;
            model.ProviderName = courseInstance.Course.Provider.ProviderName;

            // Populate drop downs
            GetLookups(model, provider.ProviderId);

            ViewBag.DateFormat = Constants.ConfigSettings.ShortDateFormat;
            ViewBag.StartMonthFormat = StartMonthFormat;

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Archive(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            CourseInstance courseInstance = db.CourseInstances.Find(id);
            if (courseInstance == null || courseInstance.Course.ProviderId != userContext.ItemId || courseInstance.Course.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            courseInstance.Archive(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Edit", "Course", new { Id = courseInstance.CourseId });
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Unarchive(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            CourseInstance courseInstance = db.CourseInstances.Find(id);
            if (courseInstance == null || courseInstance.Course.ProviderId != userContext.ItemId || courseInstance.Course.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            courseInstance.Unarchive(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Edit", "Course", new { Id = courseInstance.CourseId });
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanDeleteProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Delete(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            CourseInstance courseInstance = db.CourseInstances.Find(id);
            if (courseInstance == null || courseInstance.Course.ProviderId != userContext.ItemId || courseInstance.Course.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            courseInstance.Course.ModifiedByUserId = Permission.GetCurrentUserId();
            courseInstance.Course.ModifiedDateTimeUtc = DateTime.UtcNow;
            courseInstance.Delete(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Edit", "Course", new { Id = courseInstance.CourseId });
        }



        [PermissionAuthorize(Permission.PermissionName.CanViewProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult List(Int32? venueId, Constants.OpportunitySearchQAFilter? qualitySearchMode, OpportunitySearchModel model)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (model == null)
            {
                model = new OpportunitySearchModel();
            }

            if (venueId.HasValue && Request.HttpMethod == "GET")
            {
                model.VenueId = venueId;
            }

            if (qualitySearchMode.HasValue && Request.HttpMethod == "GET")
            {
                model.QualitySearchMode = qualitySearchMode;
            }

            IQueryable<CourseInstance> opportunities = db.CourseInstances.Include("Course").Where(x => x.Course.ProviderId == userContext.ItemId);

            if (!model.QualitySearchMode.HasValue)
            {
                // Get Opportunities Based on Search Criteria
                if (!String.IsNullOrEmpty(model.ProviderCourseId))
                {
                    opportunities = opportunities.Where(x => x.Course.ProviderOwnCourseRef.Contains(model.ProviderCourseId));
                }
                if (!String.IsNullOrEmpty(model.ProviderCourseTitle))
                {
                    opportunities = opportunities.Where(x => x.Course.CourseTitle.Contains(model.ProviderCourseTitle));
                }
                if (!String.IsNullOrEmpty(model.LearningAimReference))
                {
                    opportunities = opportunities.Where(x => x.Course.LearningAimRefId.Contains(model.LearningAimReference));
                }
                if (model.LastUpdated.HasValue)
                {
                    //Bug fix, cannot use AddDays within a linq expression. Calculate outside of linq, also cater for null ModifiedDateTime
                    DateTime dateTo = model.LastUpdated.Value.AddDays(1);
                    opportunities = opportunities.Where(x => (x.Course.ModifiedDateTimeUtc ?? x.Course.CreatedDateTimeUtc) >= model.LastUpdated && (x.Course.ModifiedDateTimeUtc ?? x.Course.CreatedDateTimeUtc) < dateTo);
                }
                if (model.CourseStatus.HasValue)
                {
                    opportunities = opportunities.Where(x => x.Course.RecordStatusId == model.CourseStatus);
                }
                if (!String.IsNullOrEmpty(model.ProviderOpportunityId))
                {
                    opportunities = opportunities.Where(x => x.ProviderOwnCourseInstanceRef == model.ProviderOpportunityId);
                }
                if (model.StudyModeId.HasValue)
                {
                    opportunities = opportunities.Where(x => x.StudyModeId == model.StudyModeId);
                }
                if (model.AttendanceModeId.HasValue)
                {
                    opportunities = opportunities.Where(x => x.AttendanceTypeId == model.AttendanceModeId);
                }
                if (model.AttendancePatternId.HasValue)
                {
                    opportunities = opportunities.Where(x => x.AttendancePatternId == model.AttendancePatternId);
                }
                if (model.VenueId.HasValue)
                {
                    opportunities = opportunities.Where(x => x.Venues.Any(v => v.VenueId == model.VenueId));
                }
                if (model.StartDateId.HasValue && model.StartDate.HasValue)
                {
                    if (model.StartDateId == 1)
                    {
                        /* Before */
                        opportunities = opportunities.Where(x => x.CourseInstanceStartDates.Any(sd => sd.StartDate < model.StartDate));
                    }
                    else
                    {
                        /* After */
                        opportunities = opportunities.Where(x => x.CourseInstanceStartDates.Any(sd => sd.StartDate >= model.StartDate));
                    }
                }
                if (!String.IsNullOrEmpty(model.StartDateDescription))
                {
                    opportunities = opportunities.Where(x => x.StartDateDescription.Contains(model.StartDateDescription));
                }
                if (model.OpportunityStatus.HasValue)
                {
                    opportunities = opportunities.Where(x => x.RecordStatusId == model.OpportunityStatus);
                }
            }


            if (model.QualitySearchMode.HasValue)
            {
                //New quality score filtering option
                if (model.QualitySearchMode == Constants.OpportunitySearchQAFilter.OpportunitiesPending)
                {
                    opportunities = opportunities.Where(x => x.RecordStatusId == (int)Constants.RecordStatus.Pending);
                }
            }

            OpportunityDateStatusModel opportunityDateStatuses = new OpportunityDateStatusModel
            {
                ProviderId = provider.ProviderId
            };
            opportunityDateStatuses.Populate(db);

            foreach (CourseInstance c in opportunities)
            {
                OpportunityDateStatusModelItem item = opportunityDateStatuses.Items.Where(x => x.OpportunityId == c.CourseInstanceId).FirstOrDefault();
                if (item != null)
                {
                    model.Opportunities.Add(new FullOpportunityListModel(c, item.MaxStartDate, item.DateStatus));
                }
                else
                {
                    model.Opportunities.Add(new FullOpportunityListModel(c, (DateTime?)null, null));
                }
            }

            if (model.QualitySearchMode.HasValue)
            {
                //New quality score filtering options
                if (model.QualitySearchMode == Constants.OpportunitySearchQAFilter.OpportunitiesOutOfDate)
                {
                    model.Opportunities = model.Opportunities.Where(x => x.DateStatus == Constants.OpportunityFilterDateStatus.OutOfDate).ToList();
                }
                if (model.QualitySearchMode == Constants.OpportunitySearchQAFilter.OpportunitiesExpiring)
                {
                    model.Opportunities = model.Opportunities.Where(x => x.DateStatus == Constants.OpportunityFilterDateStatus.Expiring).ToList();
                }
                if (model.QualitySearchMode == Constants.OpportunitySearchQAFilter.OpportunitiesUpToDate)
                {
                    model.Opportunities = model.Opportunities.Where(x => x.DateStatus == Constants.OpportunityFilterDateStatus.UpToDate).ToList();
                }
            }


            // Populate drop downs
            GetLookups(model, userContext.ItemId ?? -1);

            model.NumberOfPendingOpportunities = opportunities.Count(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Pending);

            return View("List", model);
        }



        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult AdvanceStartDates(String opportunityIds)
        {
            if (userContext.ContextName != UserContext.UserContextName.Provider)
            {
                return HttpNotFound();
            }

            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            AdvanceStartDatesModel model = new AdvanceStartDatesModel();
            model.OpportunityIdsToUpdate = opportunityIds;
            model.CountUpdates = opportunityIds.Split(',').Count();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderOpportunity)]
        [ContextAuthorize(new[] { UserContext.UserContextName.Provider })]
        public async Task<ActionResult> AdvanceStartDates(AdvanceStartDatesModel model)
        {
            int updatedCount = 0;
            model.OpportunitiesNotUpdated = new List<AdvanceStartDatesNotUpdated>();

            if (userContext.ContextName != UserContext.UserContextName.Provider)
            {
                return HttpNotFound();
            }

            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (model.NewStartDate < DateTime.Today)
            {
                ModelState.AddModelError("StartDateInvalid", "Start date must be in the future");
            }

            if (model.NewStartDate > DateTime.Today.AddYears(2))
            {
                ModelState.AddModelError("StartDateInvalid", "Start date must be within the next 2 years");
            }

            if (model.NewEndDate < model.NewStartDate)
            {
                ModelState.AddModelError("EndDateInvalid", "End date must be later than start date");
            }

            if (ModelState.IsValid)
            {
                //Loop around all opportunities increasing their start and end dates.
                foreach (String id in model.OpportunityIdsToUpdate.Split(','))
                {
                    Int32 opportunityId;
                    if (Int32.TryParse(id, out opportunityId))
                    {
                        CourseInstance opportunity = db.CourseInstances.Find(opportunityId);
                        //Course will always become live since updated / created opportunity will be live
                        opportunity.Course.RecordStatusId = (Int32)Constants.RecordStatus.Live;
                        opportunity.Course.ModifiedDateTimeUtc = DateTime.UtcNow;
                        opportunity.Course.ModifiedByUserId = Permission.GetCurrentUserId();

                        //Advance start dates unless the opportunity has multiple start dates or the end date is too early for the duration
                        if (opportunity.CourseInstanceStartDates.Count <= 1 &&
                            IsEndDateValidForDuration(opportunity, model.NewStartDate, model.NewEndDate))  
                        {
                            if (model.CreateOrUpdate == AdvanceStartDateOptions.Update)
                            {
                                //Update the existing opportunity dates 
                                CourseInstanceStartDate opportunityStartDate = opportunity.CourseInstanceStartDates.FirstOrDefault();
                                if (opportunityStartDate != null)
                                {
                                    opportunityStartDate.StartDate = model.NewStartDate;
                                    opportunityStartDate.IsMonthOnlyStartDate = false;
                                    opportunityStartDate.PlacesAvailable = null;   //Field not in use, reset to null in case used in future
                                }
                                else
                                {
                                    CourseInstanceStartDate startDate = new CourseInstanceStartDate
                                    {
                                        StartDate = model.NewStartDate,
                                        IsMonthOnlyStartDate = false
                                    };
                                    opportunity.CourseInstanceStartDates.Add(startDate);
                                }
                                opportunity.EndDate = model.NewEndDate;
                                opportunity.ModifiedByUserId = Permission.GetCurrentUserId();
                                opportunity.ModifiedDateTimeUtc = DateTime.UtcNow;
                                opportunity.RecordStatusId = (Int32)Constants.RecordStatus.Live;
                            }
                            else
                            {
                                //Create a new opportunity
                                var newOpportunity = db.CourseInstances.AsNoTracking()
                                     .FirstOrDefault(e => e.CourseInstanceId == opportunityId);
                                newOpportunity.CreatedByUserId = Permission.GetCurrentUserId();
                                newOpportunity.CreatedDateTimeUtc = DateTime.UtcNow;
                                newOpportunity.ModifiedByUserId = newOpportunity.CreatedByUserId;
                                newOpportunity.ModifiedDateTimeUtc = newOpportunity.CreatedDateTimeUtc;
                                newOpportunity.AddedByApplicationId = (Int32)Constants.Application.Portal;

                                newOpportunity.EndDate = model.NewEndDate;
                                newOpportunity.RecordStatusId = (Int32)Constants.RecordStatus.Live;
                                db.CourseInstances.Add(newOpportunity);

                                foreach (Venue v in opportunity.Venues)
                                {
                                    newOpportunity.Venues.Add(v);
                                }

                                foreach (A10FundingCode f in opportunity.A10FundingCode)
                                {
                                    newOpportunity.A10FundingCode.Add(f);
                                }

                                CourseInstanceStartDate startDate = new CourseInstanceStartDate
                                {
                                    StartDate = model.NewStartDate,
                                    IsMonthOnlyStartDate = false
                                };
                                newOpportunity.CourseInstanceStartDates.Add(startDate);

                                if (model.CreateOrUpdate == AdvanceStartDateOptions.CreateAndArchive)
                                {
                                    //archive the original opportunity, 
                                    //Don't call the archive extension method as it won't pick up the newly created opportunity and may set the course to pending
                                    opportunity.RecordStatusId = (Int32)Constants.RecordStatus.Archived;
                                    opportunity.AddedByApplicationId = (Int32)Constants.Application.Portal;
                                    opportunity.ModifiedByUserId = Permission.GetCurrentUserId();
                                    opportunity.ModifiedDateTimeUtc = DateTime.UtcNow;
                                }
                            }
                            updatedCount++;
                        }
                        else
                        {
                            model.OpportunitiesNotUpdated.Add(new AdvanceStartDatesNotUpdated(opportunity));
                        }
                    }
                }
                await db.SaveChangesAsync();

                //Keep dialog open and display number of opportunities updated and number of errors
                model.CountErrors = model.OpportunitiesNotUpdated.Count;
                model.CountUpdated = updatedCount;
                model.CountUpdates = 0;
            }
            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public NewtonsoftJsonResult ArchiveSelected(String opportunityIds)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                throw new Exception("");
            }

            List<Int32> archivedIds = new List<Int32>();
            foreach (String id in opportunityIds.Split(','))
            {
                Int32 opportunityId;
                if (Int32.TryParse(id, out opportunityId))
                {
                    CourseInstance opportunity = db.CourseInstances.Find(opportunityId);
                    if (opportunity.Course.ProviderId == userContext.ItemId && opportunity.RecordStatusId == (Int32)Constants.RecordStatus.Live)
                    {
                        opportunity.Archive(db);
                        archivedIds.Add(opportunityId);
                    }
                }
            }
            if (archivedIds.Count > 0)
            {
                db.SaveChanges();
            }

            return Json(archivedIds.ToArray());
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public NewtonsoftJsonResult UnarchiveSelected(String opportunityIds)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                throw new Exception("");
            }

            List<Int32> unarchivedIds = new List<Int32>();
            foreach (String id in opportunityIds.Split(','))
            {
                Int32 opportunityId;
                if (Int32.TryParse(id, out opportunityId))
                {
                    CourseInstance opportunity = db.CourseInstances.Find(opportunityId);
                    if (opportunity.Course.ProviderId == userContext.ItemId && opportunity.RecordStatusId == (Int32)Constants.RecordStatus.Archived)
                    {
                        opportunity.Unarchive(db);
                        unarchivedIds.Add(opportunityId);
                    }
                }
            }
            if (unarchivedIds.Count > 0)
            {
                db.SaveChanges();
            }

            return Json(unarchivedIds.ToArray());
        }




        [NonAction]
        private bool IsEndDateValidForDuration(CourseInstance opportunity, DateTime newStartDate, DateTime newEndDate)
        {

            DurationUnit du = null;
            du = db.DurationUnits.Find(opportunity.DurationUnitId);
            DateTime dtMinEndDate = DateTime.MaxValue;
            if (du != null)
            {
                if (du.BulkUploadRef == "DU2")
                {
                    // DU2 is Days
                    // For other reasons the WeekEquivalent for days is 0.2 so we can't use that here
                    dtMinEndDate = newStartDate.AddDays(opportunity.DurationUnit.Value - 1); // Subtract 1 from min date because start date is inclusive
                }
                else
                {
                    dtMinEndDate = newStartDate.AddDays(Convert.ToInt32(7 * du.WeekEquivalent * opportunity.DurationUnit.Value) - 1); // Subtract 1 from min date because start date is inclusive

                    // If MinEndDate falls on a weekend then move back to the Friday (so for example you can have a 1 week course that starts on Monday and ends on Friday)
                    while (dtMinEndDate.DayOfWeek == DayOfWeek.Saturday || dtMinEndDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        dtMinEndDate = dtMinEndDate.AddDays(-1);
                    }
                }
            }
            if (newEndDate < dtMinEndDate)
            {
                return false;
            }
            return true;

        }



        [NonAction]
        private void CheckModel(AddEditOpportunityModel model)
        {
            if (model.SelectedA10FundingCodes.Count == 0)
            {
                ModelState.AddModelError("A10FundingCodes", AppGlobal.Language.GetText(this, "A10FundingCodesMandatory", "The A10 Funding Code field is required"));
            }
            if (!String.IsNullOrEmpty(model.StartDate) && !String.IsNullOrEmpty(model.StartMonth))
            {
                ModelState.AddModelError("StartMonth", AppGlobal.Language.GetText(this, "StartDateAndStartMonthProvided", "You cannot enter both Start Date and Start Month"));
            }

            if (((!String.IsNullOrEmpty(model.StartDate) && model.StartDate.Contains(',')) || (!String.IsNullOrEmpty(model.StartMonth) && model.StartMonth.Contains(','))) && model.EndDate != null)
            {
                ModelState.AddModelError("EndDate", AppGlobal.Language.GetText(this, "EndDateNotSupportedForMultipleStartDates", "End date is not supported for multiple start dates"));
            }

            if ((String.IsNullOrEmpty(model.StartDate) && String.IsNullOrEmpty(model.StartMonth)) || model.EndDate == null)
            {
                if ((model.Duration == null || model.DurationUnitId == null) && String.IsNullOrEmpty(model.DurationDescription))
                {
                    ModelState.AddModelError("Duration", AppGlobal.Language.GetText(this, "DurationMustBeSpecified", "You must specify a duration unless you specify both a start date and an end date"));
                }
            }
            
            if ((model.Duration == null && model.DurationUnitId != null)
                || (model.Duration != null && model.DurationUnitId == null))
            {
                ModelState.AddModelError("Duration", AppGlobal.Language.GetText(this, "PartialDurationSpecified", "You must specify both part of the duration"));
            }

            if (String.IsNullOrEmpty(model.StartDate) && String.IsNullOrEmpty(model.StartMonth) && String.IsNullOrEmpty(model.StartDateDescription))
            {
                ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateMandatory", "You must enter at least one of Start Date, Start Month and Start Date Description"));
            }

            if (model.Price == null && String.IsNullOrEmpty(model.PriceDescription))
            {
                ModelState.AddModelError("Price", AppGlobal.Language.GetText(this, "PriceMustBeSpecified", "You must enter either a price or a price description or both"));
            }

            // Get Earliest StartDate - If course is longer than 12 weeks then the start date can be up to 4 weeks ago.  Otherwise the course start date has to be no earlier than today.
            DateTime dtMinStartDate = DateTime.Today;
            DurationUnit du = null;
            const Int32 minCourseLengthToAllowOlderStartDate = 12;
            const Int32 maxNumberOfDaysBeforeToday = -28;
            if (model.Duration.HasValue && model.DurationUnitId.HasValue)
            {
                du = db.DurationUnits.Find(model.DurationUnitId);
                if (du != null)
                {
                    if (du.WeekEquivalent * model.Duration.Value >= minCourseLengthToAllowOlderStartDate)
                    {
                        dtMinStartDate = dtMinStartDate.AddDays(maxNumberOfDaysBeforeToday);
                    }
                }
            }
            else if ((!String.IsNullOrEmpty(model.StartDate) || !String.IsNullOrEmpty(model.StartMonth)) && model.EndDate.HasValue)
            {
                // Start Date and End Date supplied - try to work out duration
                DateTime actualStartDate;
                if (!DateTime.TryParseExact(model.StartDate.Trim(), Constants.ConfigSettings.ShortDateFormat, null, System.Globalization.DateTimeStyles.None, out actualStartDate))
                {
                    DateTime.TryParseExact(model.StartMonth.Trim(), StartMonthFormat, null, System.Globalization.DateTimeStyles.None, out actualStartDate);
                }
                if (actualStartDate != DateTime.MinValue)
                {
                    if (model.EndDate >= actualStartDate.AddDays(minCourseLengthToAllowOlderStartDate * 7))
                    {
                        dtMinStartDate = dtMinStartDate.AddDays(maxNumberOfDaysBeforeToday);
                    }
                }
            }

            DateTime dtEndDate = model.EndDate ?? DateTime.MaxValue;

            // Validate Start Date(s)
            DateTime dtMinEndDate = DateTime.MaxValue;
            Boolean startDateTooEarlyErrorAdded = false;
            Boolean startDateAfterEndDateErrorAdded = false;
            if (!String.IsNullOrEmpty(model.StartDate))
            {
                foreach (String strStart in model.StartDate.Split(','))
                {
                    DateTime dtStart;
                    if (!DateTime.TryParseExact(strStart.Trim(), Constants.ConfigSettings.ShortDateFormat, null, System.Globalization.DateTimeStyles.None, out dtStart))
                    {
                        ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateFormatError", "Start Date is not in the correct format"));
                        break;
                    }
                    if (dtStart < dtMinEndDate)
                    {
                        dtMinEndDate = dtStart;
                    }
                    if (!startDateTooEarlyErrorAdded)
                    {
                        if (dtStart < dtMinStartDate)
                        {
                            ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateTooEarly", "The start dates entered have already passed. For courses that last less than 12 weeks start dates should always be in the future. For courses with longer durations, the system allows past start dates to be entered providing that the course didn't start more than 4 weeks ago"));
                            startDateTooEarlyErrorAdded = true;
                        }
                    }
                    if (!startDateAfterEndDateErrorAdded)
                    {
                        if (dtStart > dtEndDate)
                        {
                            ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateAfterEndDate", "The 'start date' you have entered for this course opportunity is later than the 'end date'. Please re-check the dates you have selected for these two fields"));
                            startDateAfterEndDateErrorAdded = true;
                        }
                    }
                    if (dtStart > DateTime.UtcNow.AddYears(2))
                    {
                        ModelState.AddModelError("StartDate", AppGlobal.Language.GetText(this, "StartDateTooLate", "The start dates entered are more than two years in the future. The start date must be within two years of today's date."));
                        break;
                    }
                }
            }

            // Validate Start Month(s)
            if (!String.IsNullOrEmpty(model.StartMonth))
            {
                foreach (String strStart in model.StartMonth.Split(','))
                {
                    DateTime dtStart;
                    if (!DateTime.TryParseExact(strStart.Trim(), StartMonthFormat, null, System.Globalization.DateTimeStyles.None, out dtStart))
                    {
                        ModelState.AddModelError("StartMonth", AppGlobal.Language.GetText(this, "StartMonthFormatError", "Start Month is not in the correct format"));
                        break;
                    }
                    if (dtStart < dtMinEndDate)
                    {
                        dtMinEndDate = dtStart;
                    }
                    if (!startDateTooEarlyErrorAdded)
                    {
                        if (dtStart < dtMinStartDate)
                        {
                            ModelState.AddModelError("StartMonth", AppGlobal.Language.GetText(this, "StartDateTooEarly", "The start dates entered have already passed. For courses that last less than 12 weeks start dates should always be in the future. For courses with longer durations, the system allows past start dates to be entered providing that the course didn't start more than 4 weeks ago"));
                            startDateTooEarlyErrorAdded = true;
                        }
                    }
                    if (!startDateAfterEndDateErrorAdded)
                    {
                        if (dtStart.AddMonths(1).AddDays(-1) > dtEndDate)
                        {
                            ModelState.AddModelError("StartMonth", AppGlobal.Language.GetText(this, "StartDateAfterEndDate", "The 'start date' you have entered for this course opportunity is later than the 'end date'. Please re-check the dates you have selected for these two fields"));
                            startDateAfterEndDateErrorAdded = true;
                        }
                    }
                }
            }

            if (model.EndDate.HasValue)
            {
                if (du != null)
                {
                    if (dtMinEndDate == DateTime.MaxValue)
                    {
                        dtMinEndDate = DateTime.Today;
                    }
                     
                    if (du.BulkUploadRef == "DU2")
                    {
                        // DU2 is Days
                        // For other reasons the WeekEquivalent for days is 0.2 so we can't use that here
                        dtMinEndDate = dtMinEndDate.AddDays(model.Duration.Value - 1); // Subtract 1 from min date because start date is inclusive
                    }
                    else
                    {
                        dtMinEndDate = dtMinEndDate.AddDays(Convert.ToInt32(7 * du.WeekEquivalent * model.Duration.Value) - 1); // Subtract 1 from min date because start date is inclusive

                        // If MinEndDate falls on a weekend then move back to the Friday (so for example you can have a 1 week course that starts on Monday and ends on Friday)
                        while (dtMinEndDate.DayOfWeek == DayOfWeek.Saturday || dtMinEndDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            dtMinEndDate = dtMinEndDate.AddDays(-1);
                        }
                    }
                }
                if (model.EndDate < dtMinEndDate)
                {
                    ModelState.AddModelError("EndDate", AppGlobal.Language.GetText(this, "EndDateTooEarly", "The end date you have entered for this course opportunity is too early based on the start date and the duration entered"));
                }
            }

            if (model.RegionId.HasValue)
            {
                VenueLocation location = db.VenueLocations.Find(model.RegionId);
                if (location == null)
                {
                    ModelState.AddModelError("RegionId", AppGlobal.Language.GetText(this, "InvalidRegion", "Region Not Found"));
                }
            }

            if (ModelState.IsValid)
            {
                if (model.VenueId.HasValue && model.RegionId.HasValue)
                {
                    ModelState.AddModelError("VenueId", AppGlobal.Language.GetText(this, "InvalidRegion", "Please select either a region or a venue"));
                }
                if (model.VenueId == null && model.RegionId == null)
                {
                    // Get "United Kingdom" Location
                    VenueLocation venueLocation = db.VenueLocations.FirstOrDefault(x => x.LocationName == @"United Kingdom");
                    if (venueLocation != null)
                    {
                        model.RegionId = venueLocation.VenueLocationId;
                    }
                }
            }
        }

        [NonAction]
        private void GetLookups(AddEditOpportunityModel model, Int32 providerId)
        {
            ViewBag.StudyModes = new SelectList(
                db.StudyModes,
                "StudyModeId",
                "StudyModeName",
                model.StudyModeId);
            ViewBag.AttendanceTypes = new SelectList(
                db.AttendanceTypes,
                "AttendanceTypeId",
                "AttendanceTypeName",
                model.AttendanceModeId);
            ViewBag.AttendancePatterns = new SelectList(
                db.AttendancePatterns,
                "AttendancePatternId",
                "AttendancePatternName",
                model.AttendancePatternId);
            ViewBag.DurationUnits = new SelectList(
                db.DurationUnits,
                "DurationUnitId",
                "DurationUnitName",
                model.DurationUnitId);

            // Add Use Provider's Address if required
            Provider provider = db.Providers.Find(providerId);
            List<SelectListItem> Venues = new List<SelectListItem>();
            Int32? defaultVenueId = model.VenueId;
            if (provider != null)
            {
                Venue venue = db.Venues.Where(x => x.ProviderId == providerId && x.Address.Postcode == provider.Address.Postcode).FirstOrDefault();
                if (venue == null)
                {
                    Venues.Add(new SelectListItem() { Value = CreateNewVenueBasedOnProviderAddressId.ToString(), Text = AppGlobal.Language.GetText(this, "CreateVenueForProviderAddress", "Create a new Venue using Provider's Address") });
                    defaultVenueId = -1;
                }
            }
            foreach (Venue venue in db.Venues.Where(x => x.ProviderId == providerId && x.RecordStatusId == (Int32)Constants.RecordStatus.Live))
            {
                Venues.Add(new SelectListItem() { Value = venue.VenueId.ToString(), Text = venue.VenueName });
            }
            ViewBag.Venues = new SelectList(Venues, "Value", "Text", defaultVenueId);

            ViewBag.OfferedByOptions = new SelectList(
                db.Organisations.Where(x => x.OrganisationProviders.Any(p => p.ProviderId == providerId)),
                "OrganisationId",
                "OrganisationName",
                model.OfferedById);
            ViewBag.DisplayByOptions = new SelectList(
                db.Organisations.Where(x => x.OrganisationProviders.Any(p => p.ProviderId == providerId)),
                "OrganisationId",
                "OrganisationName",
                model.DisplayId);
        }

        [NonAction]
        public Int32? GetLatestOpportunityIdForCourse(Int32 courseId)
        {
            Course course = db.Courses.Find(courseId);
            if (course == null)
            {
                return null;
            }

            if (course.ProviderId != userContext.ItemId)
            {
                return null;
            }

            if (!course.CourseInstances.Any())
            {
                return null;
            }

            Int32 opportunityId = 0;
            DateTime maxStartDate = DateTime.MinValue;
            foreach (CourseInstance courseInstance in course.CourseInstances)
            {
                foreach (CourseInstanceStartDate sd in courseInstance.CourseInstanceStartDates)
                {
                    if (sd.StartDate > maxStartDate)
                    {
                        maxStartDate = sd.StartDate;
                        opportunityId = courseInstance.CourseInstanceId;
                    }
                }
            }

            // If none of the opprtunities have a start date then choose the last one created
            if (opportunityId == 0)
            {
                CourseInstance ci = course.CourseInstances.OrderByDescending(x => x.CreatedDateTimeUtc).FirstOrDefault();
                if (ci != null)
                {
                    opportunityId = ci.CourseInstanceId;
                }
            }

            return opportunityId == 0 ? (Int32?) null : opportunityId;
        }

        [NonAction]
        private void GetLookups(OpportunitySearchModel model, Int32 providerId)
        {
            ViewBag.RecordStatuses = new SelectList(
                db.RecordStatus.Where(s => !s.IsDeleted),
                "RecordStatusId",
                "RecordStatusName",
                model.CourseStatus);
            ViewBag.StudyModes = new SelectList(
                db.StudyModes,
                "StudyModeId",
                "StudyModeName",
                model.StudyModeId);
            ViewBag.AttendanceTypes = new SelectList(
                db.AttendanceTypes,
                "AttendanceTypeId",
                "AttendanceTypeName",
                model.AttendanceModeId);
            ViewBag.AttendancePatterns = new SelectList(
                db.AttendancePatterns,
                "AttendancePatternId",
                "AttendancePatternName",
                model.AttendancePatternId);
            ViewBag.Venues = new SelectList(
                db.Venues.Where(x => x.ProviderId == providerId),
                "VenueId",
                "VenueName",
                model.VenueId);
            ViewBag.OpportunityStatuses = new SelectList(
                db.RecordStatus.Where(s => !s.IsDeleted && s.RecordStatusId != (int)Constants.RecordStatus.Pending),
                "RecordStatusId",
                "RecordStatusName",
                model.OpportunityStatus);
            ViewBag.StartDateOptions = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Text = AppGlobal.Language.GetText(this, "Before", "Before"), Value = "1"},
                    new SelectListItem { Text = AppGlobal.Language.GetText(this, "After", "After"), Value = "2"}
                }, "Value", "Text", model.StartDateId);
        }

        [NonAction]
        private Venue CreateVenueBasedOnProvider(Provider provider)
        {
            Venue venue = new Venue
            {
                ProviderId = provider.ProviderId,
                VenueName = provider.ProviderName,
                Address = provider.Address.Clone(),
                CreatedByUserId = Permission.GetCurrentUserId(),
                CreatedDateTimeUtc = DateTime.UtcNow,
                RecordStatusId = (Int32)Constants.RecordStatus.Live
            };
            return venue;
        }
    }
}