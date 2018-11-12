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
    public class CourseController : BaseController
    {
        //
        // GET: /Course
        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Create(String id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            AddEditCourseModel model = new AddEditCourseModel
            {
                LearningAimId = id
            };

            if (id != null)
            {
                LearningAim learningAim = db.LearningAims.Find(id);
                if (learningAim == null)
                {
                    return HttpNotFound();
                }
                model.Qualification = learningAim.LearningAimTitle;                
                model.QualificationTypeId = learningAim.QualificationTypeId;
                model.LearningAimQualificationTypeId = learningAim.QualificationTypeId;
                model.AwardingOrganisation = learningAim.LearningAimAwardOrg.AwardOrgName;
                model.QualificationLevelId = learningAim.QualificationLevelId;
            }

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public async Task<ActionResult> Create(AddEditCourseModel model)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            CheckModel(model);

            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(model.LearningAimId))
                {
                    LearningAim learningAim = db.LearningAims.Find(model.LearningAimId);
                    if (learningAim == null)
                    {
                        return HttpNotFound();
                    }
                }
                Course course = model.ToEntity(db);
                course.ProviderId = provider.ProviderId;
                course.RecordStatusId = (Int32)Constants.RecordStatus.Pending;
                course.AddedByApplicationId = (Int32)Constants.Application.Portal;

                if (String.IsNullOrEmpty(model.LearningAimId))
                {
                    List<String> LearnDirectClassificationCodes = new List<String>();
                    if (!String.IsNullOrEmpty(model.LearnDirectClassificationId1))
                    {
                        LearnDirectClassificationCodes.Add(model.LearnDirectClassificationId1);
                    }
                    if (!String.IsNullOrEmpty(model.LearnDirectClassificationId2))
                    {
                        LearnDirectClassificationCodes.Add(model.LearnDirectClassificationId2);
                    }
                    if (!String.IsNullOrEmpty(model.LearnDirectClassificationId3))
                    {
                        LearnDirectClassificationCodes.Add(model.LearnDirectClassificationId3);
                    }
                    if (!String.IsNullOrEmpty(model.LearnDirectClassificationId4))
                    {
                        LearnDirectClassificationCodes.Add(model.LearnDirectClassificationId4);
                    }
                    if (!String.IsNullOrEmpty(model.LearnDirectClassificationId5))
                    {
                        LearnDirectClassificationCodes.Add(model.LearnDirectClassificationId5);
                    }
                    Int32 i = 1;
                    foreach (LearnDirectClassification classification in LearnDirectClassificationCodes.Select(ld => db.LearnDirectClassifications.Find(ld)))
                    {
                        CourseLearnDirectClassification cld = new CourseLearnDirectClassification
                        {
                            LearnDirectClassification = classification,
                            ClassificationOrder = i
                        };
                        course.CourseLearnDirectClassifications.Add(cld);
                        i++;
                    }
                }

                db.Courses.Add(course);
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

                // If user clicked "Create" then take them to Course List
                if (Request.Form["Create"] != null)
                {
                    return RedirectToAction("List");
                }

                // User clicked "Create and Add Opportunity" - Take them to Create Opportunity page
                return RedirectToAction("Create", "Opportunity", new { id = course.CourseId });
            }

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Edit(Int32? id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Course course = db.Courses.Include("LearningAim").FirstOrDefault(x => x.CourseId == id);
            if (course == null || course.ProviderId != userContext.ItemId)
            {
                return HttpNotFound();
            }

            AddEditCourseModel model = new AddEditCourseModel(course);

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public async Task<ActionResult> Edit(Int32 id, AddEditCourseModel model)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (model.CourseId != id)
            {
                return HttpNotFound();
            }

            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            CheckModel(model);

            if (ModelState.IsValid)
            {
                course = model.ToEntity(db);
                if (course.ProviderId != userContext.ItemId)
                {
                    // User is trying to change the VenueId (or the context has changed)
                    return HttpNotFound();
                }

                // Delete Existing CourseLearnDirectClassifications
                // Normally I would work out which already exist and just write the changes but because of the ordering it's easier just to delete them all and re-write them all
                foreach (CourseLearnDirectClassification ld in course.CourseLearnDirectClassifications.ToList())
                {
                    course.CourseLearnDirectClassifications.Remove(ld);
                }

                // Add any new Learn Direct classifications
                if (String.IsNullOrEmpty(model.LearningAimId))
                {
                    List<String> LearnDirectClassificationCodes = new List<String>();
                    if (!String.IsNullOrEmpty(model.LearnDirectClassificationId1))
                    {
                        LearnDirectClassificationCodes.Add(model.LearnDirectClassificationId1);
                    }
                    if (!String.IsNullOrEmpty(model.LearnDirectClassificationId2))
                    {
                        LearnDirectClassificationCodes.Add(model.LearnDirectClassificationId2);
                    }
                    if (!String.IsNullOrEmpty(model.LearnDirectClassificationId3))
                    {
                        LearnDirectClassificationCodes.Add(model.LearnDirectClassificationId3);
                    }
                    if (!String.IsNullOrEmpty(model.LearnDirectClassificationId4))
                    {
                        LearnDirectClassificationCodes.Add(model.LearnDirectClassificationId4);
                    }
                    if (!String.IsNullOrEmpty(model.LearnDirectClassificationId5))
                    {
                        LearnDirectClassificationCodes.Add(model.LearnDirectClassificationId5);
                    }
                    Int32 i = 1;
                    foreach (LearnDirectClassification classification in LearnDirectClassificationCodes.Select(ld => db.LearnDirectClassifications.Find(ld)))
                    {
                        CourseLearnDirectClassification cld = new CourseLearnDirectClassification
                        {
                            LearnDirectClassification = classification,
                            ClassificationOrder = i
                        };
                        course.CourseLearnDirectClassifications.Add(cld);
                        i++;
                    }
                }

                course.ModifiedByUserId = Permission.GetCurrentUserId();
                course.ModifiedDateTimeUtc = DateTime.UtcNow;
                course.AddedByApplicationId = (Int32)Constants.Application.Portal;

                db.Entry(course).State = EntityState.Modified;
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

                return RedirectToAction("List");
            }

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult EditLatestOpportunity(Int32 id)
        {
            // Get latest Opportunity and redirect to Opportunity/Edit
            Int32? opportunityId = new OpportunityController().GetLatestOpportunityIdForCourse(id);
            if (!opportunityId.HasValue)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Edit", "Opportunity", new {id = opportunityId.Value});
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderOpportunity)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult DuplicateLatestOpportunity(Int32 id)
        {
            // Get latest Opportunity and redirect to Opportunity/Edit
            Int32? opportunityId = new OpportunityController().GetLatestOpportunityIdForCourse(id);
            if (!opportunityId.HasValue)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Duplicate", "Opportunity", new { id = opportunityId.Value });
        }


        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Archive(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Course course = db.Courses.Find(id);
            if (course == null || course.ProviderId != userContext.ItemId || course.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            course.Archive(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Edit", "Course", new { Id = course.CourseId });
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Unarchive(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Course course = db.Courses.Find(id);
            if (course == null || course.ProviderId != userContext.ItemId || course.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            course.Unarchive(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Edit", "Course", new { Id = course.CourseId });
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanDeleteProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Delete(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Course course = db.Courses.Find(id);
            if (course == null || course.ProviderId != userContext.ItemId || course.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            course.Delete(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);
            
            return RedirectToAction("List");
        }

        [PermissionAuthorize(Permission.PermissionName.CanViewProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult List(Int32? venueId, Constants.CourseSearchQAFilter? qualitySearchMode, CourseSearchModel model)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (model == null)
            {
                model = new CourseSearchModel();
            }

            if (venueId.HasValue && Request.HttpMethod == "GET")
            {
                model.VenueId = venueId;
            }

            if (qualitySearchMode.HasValue && Request.HttpMethod == "GET")
            {
                model.QualitySearchMode = qualitySearchMode;
            }

            IQueryable<Course> courses = db.Courses.Include("LearningAim").Where(x => x.ProviderId == userContext.ItemId);

            if (!model.QualitySearchMode.HasValue)
            {
                // Get Courses Based on Search Criteria
                if (!String.IsNullOrEmpty(model.ProviderCourseId))
                {
                    courses = courses.Where(x => x.ProviderOwnCourseRef.Contains(model.ProviderCourseId));
                }
                if (!String.IsNullOrEmpty(model.ProviderCourseTitle))
                {
                    courses = courses.Where(x => x.CourseTitle.Contains(model.ProviderCourseTitle));
                }
                if (!String.IsNullOrEmpty(model.LearningAimReference))
                {
                    courses = courses.Where(x => x.LearningAimRefId.Contains(model.LearningAimReference));
                }
                if (model.LastUpdated.HasValue)
                {
                    //Bug fix, cannot use AddDays within a linq expression. Calculate outside of linq, also cater for null ModifiedDateTime
                    DateTime dateTo = model.LastUpdated.Value.AddDays(1);
                    courses = courses.Where(x => (x.ModifiedDateTimeUtc ?? x.CreatedDateTimeUtc) >= model.LastUpdated && (x.ModifiedDateTimeUtc ?? x.CreatedDateTimeUtc) < dateTo);
                }
                if (model.CourseStatus.HasValue)
                {
                    courses = courses.Where(x => x.RecordStatusId == model.CourseStatus);
                }
                if (!String.IsNullOrEmpty(model.ProviderOpportunityId))
                {
                    courses = courses.Where(x => x.CourseInstances.Any(i => i.ProviderOwnCourseInstanceRef == model.ProviderOpportunityId));
                }
                if (model.StudyModeId.HasValue)
                {
                    courses = courses.Where(x => x.CourseInstances.Any(i => i.StudyModeId == model.StudyModeId));
                }
                if (model.AttendanceModeId.HasValue)
                {
                    courses = courses.Where(x => x.CourseInstances.Any(i => i.AttendanceTypeId == model.AttendanceModeId));
                }
                if (model.AttendancePatternId.HasValue)
                {
                    courses = courses.Where(x => x.CourseInstances.Any(i => i.AttendancePatternId == model.AttendancePatternId));
                }
                if (model.VenueId.HasValue)
                {
                    courses = courses.Where(x => x.CourseInstances.Any(i => i.Venues.Any(v => v.VenueId == model.VenueId)));
                }
                if (model.StartDateId.HasValue && model.StartDate.HasValue)
                {
                    if (model.StartDateId == 1)
                    {
                        /* Before */
                        courses = courses.Where(x => x.CourseInstances.Any(i => i.CourseInstanceStartDates.Any(sd => sd.StartDate < model.StartDate)));
                    }
                    else
                    {
                        /* After */
                        courses = courses.Where(x => x.CourseInstances.Any(i => i.CourseInstanceStartDates.Any(sd => sd.StartDate >= model.StartDate)));
                    }
                }
                if (!String.IsNullOrEmpty(model.StartDateDescription))
                {
                    courses = courses.Where(x => x.CourseInstances.Any(i => i.StartDateDescription.Contains(model.StartDateDescription)));
                }
                if (model.OpportunityStatus.HasValue)
                {
                    courses = courses.Where(x => x.CourseInstances.Any(i => i.RecordStatusId == model.OpportunityStatus));
                }
            }


            if (model.QualitySearchMode.HasValue)
            {
                //New quality score filtering options
                if (model.QualitySearchMode == Constants.CourseSearchQAFilter.CourseShortSummary)
                {
                    courses = courses.Where(x => x.CourseSummary.Length <= 200);
                }
                else if (model.QualitySearchMode == Constants.CourseSearchQAFilter.CourseNonDistinctSummary)
                {
                    var nonDistinct = courses.GroupBy(c => c.CourseSummary).Where(grp => grp.Count() > 1).Select(grp => grp.Key);
                    courses = courses.Where(x => x.CourseSummary.Length > 200 && nonDistinct.Contains(x.CourseSummary));
                }
                else if (model.QualitySearchMode == Constants.CourseSearchQAFilter.CoursesPending)
                {
                    courses = courses.Where(x => x.RecordStatusId == (int)Constants.RecordStatus.Pending);
                }
            }

            CourseDateStatusModel courseDateStatuses = new CourseDateStatusModel
            {
                ProviderId = provider.ProviderId
            };
            courseDateStatuses.Populate(db);

            foreach (Course c in courses.OrderByDescending(x => x.ModifiedDateTimeUtc ?? x.CreatedDateTimeUtc))
            {
                CourseDateStatusModelItem item = courseDateStatuses.Items.Where(x => x.CourseId == c.CourseId).FirstOrDefault();
                if (item != null)
                {
                    model.Courses.Add(new CourseListModel(c, item.MaxStartDate, item.DateStatus));
                }
                else
                {
                    model.Courses.Add(new CourseListModel(c, (DateTime?)null, null));
                }
            }

            if (model.QualitySearchMode.HasValue)
            {
                //New quality score filtering options
                if (model.QualitySearchMode == Constants.CourseSearchQAFilter.CoursesOutOfDate)
                {
                    model.Courses = model.Courses.Where(x => x.DateStatus == Constants.CourseFilterDateStatus.OutOfDate).ToList();
                }
                if (model.QualitySearchMode == Constants.CourseSearchQAFilter.CoursesExpiring)
                {
                    model.Courses = model.Courses.Where(x => x.DateStatus == Constants.CourseFilterDateStatus.Expiring).ToList();
                }
                if (model.QualitySearchMode == Constants.CourseSearchQAFilter.CoursesUpToDate)
                {
                    model.Courses = model.Courses.Where(x => x.DateStatus == Constants.CourseFilterDateStatus.UpToDate).ToList();
                }
                else if (model.QualitySearchMode == Constants.CourseSearchQAFilter.LearningAimExpired)
                {
                    model.Courses = model.Courses.Where(x => x.IsLARSExpired.HasValue && x.IsLARSExpired.Value).ToList();
                }
                else if (model.QualitySearchMode == Constants.CourseSearchQAFilter.LearningAimNone)
                {
                    model.Courses = model.Courses.Where(x => !x.IsLARSExpired.HasValue).ToList();
                }
            }


            // Populate drop downs
            GetLookups(model, userContext.ItemId ?? -1);

            model.NumberOfPendingCourses = courses.Count(x => x.RecordStatusId == (Int32) Constants.RecordStatus.Pending);

            return View("List", model);
        }

        [PermissionAuthorize(Permission.PermissionName.CanViewProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult PendingCourseList(Int32? venueId)
        {
            CourseSearchModel model = new CourseSearchModel
            {
                CourseStatus = (Int32) Constants.RecordStatus.Pending
            };

            return List(venueId, null, model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderCourse)]
        public ActionResult AddCourseDialog(String learningAimRef)
        {
            UserContext.UserContextInfo uco = UserContext.GetUserContext();
            if (uco.ContextName != UserContext.UserContextName.Provider)
            {
                return HttpNotFound();
            }

            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            AddCourseModel model = new AddCourseModel();

            if (learningAimRef == null)
            {
                model.CourseHasLearningAimRef = 0; /* Used for Adding a new course - no default */
            }
            else if (learningAimRef == "")
            {
                model.CourseHasLearningAimRef = 1; /* No */
            }
            else
            {
                model.CourseHasLearningAimRef = 2; /* Yes */
                model.LearningAim = db.LearningAims.Find(learningAimRef);
            }

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public NewtonsoftJsonResult ArchiveSelected(String courseIds)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                throw new Exception("");
            }

            List<Int32> archivedIds = new List<Int32>();
            foreach (String cid in courseIds.Split(','))
            {
                Int32 courseId;
                if (Int32.TryParse(cid, out courseId))
                {
                    Course course = db.Courses.Find(courseId);
                    if (course.ProviderId == userContext.ItemId && course.RecordStatusId == (Int32)Constants.RecordStatus.Live)
                    {
                        course.Archive(db);
                        archivedIds.Add(courseId);
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
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public NewtonsoftJsonResult UnarchiveSelected(String courseIds)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                throw new Exception("");
            }

            List<Int32> unarchivedIds = new List<Int32>();
            foreach (String cid in courseIds.Split(','))
            {
                Int32 courseId;
                if (Int32.TryParse(cid, out courseId))
                {
                    Course course = db.Courses.Find(courseId);
                    if (course.ProviderId == userContext.ItemId && course.RecordStatusId == (Int32)Constants.RecordStatus.Archived)
                    {
                        course.Unarchive(db);
                        unarchivedIds.Add(courseId);
                    }
                }
            }
            if (unarchivedIds.Count > 0)
            {
                db.SaveChanges();
            }

            return Json(unarchivedIds.ToArray());
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderCourse, Permission.PermissionName.CanEditProviderCourse)]
        public ActionResult View(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Course course = db.Courses.FirstOrDefault(x => x.CourseId == id);
            if (course == null || course.ProviderId != userContext.ItemId)
            {
                return HttpNotFound();
            }

            ViewCourseModel model = new ViewCourseModel(course);

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderCourse, Permission.PermissionName.CanEditProviderCourse)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public NewtonsoftJsonResult GetLearningAimDetails(String learningAimRef)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                throw new Exception("");
            }

            LearningAim learningAim = db.LearningAims.Find(learningAimRef);

            return Json(new LearningAimRefModel(learningAim));
        }


        [NonAction]
        private void GetLookups(AddEditCourseModel model)
        {
            ViewBag.QualificationLevels = new SelectList(
                db.QualificationLevels.OrderBy(x => x.DisplayOrder),
                "QualificationLevelId",
                "QualificationLevelName",
                model.QualificationLevelId);
            ViewBag.QualificationTypes = new SelectList(
                db.QualificationTypes.Where(x => x.IsHidden == false || x.QualificationTypeId == model.QualificationTypeId).OrderBy(x => x.DisplayOrder),
                "QualificationTypeId",
                "QualificationTypeName",
                model.QualificationTypeId);
        }

        [NonAction]
        private void GetLookups(CourseSearchModel model, Int32 providerId)
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
        private void CheckModel(AddEditCourseModel model)
        {
            LearningAim la = null;
            if (!String.IsNullOrWhiteSpace(model.LearningAimId))
            {
                la = db.LearningAims.Find(model.LearningAimId);
            }
            if (la == null || la.QualificationTypeId == null)
            {
                if (!model.QualificationTypeId.HasValue)
                {
                    ModelState.AddModelError("QualificationTypeId", AppGlobal.Language.GetText(this, "QualificationTypeMandatory", "The Qualification Type field is required"));
                }
            }

            List<String> LearnDirectClassifications = new List<String>();
            if (!String.IsNullOrEmpty(model.LearnDirectClassificationId1))
            {
                LearnDirectClassifications.Add(model.LearnDirectClassificationId1);
            }
            if (!String.IsNullOrEmpty(model.LearnDirectClassificationId2))
            {
                if (LearnDirectClassifications.Contains(model.LearnDirectClassificationId2))
                {
                    ModelState.AddModelError("LearnDirectClassificationId2", AppGlobal.Language.GetText(this, "LearnDirectClassificationInUse", "This Learn Direct Classification has already been specified for this course"));
                }
                else
                {
                    LearnDirectClassifications.Add(model.LearnDirectClassificationId2);
                }
            }
            if (!String.IsNullOrEmpty(model.LearnDirectClassificationId3))
            {
                if (LearnDirectClassifications.Contains(model.LearnDirectClassificationId3))
                {
                    ModelState.AddModelError("LearnDirectClassificationId3", AppGlobal.Language.GetText(this, "LearnDirectClassificationInUse", "This Learn Direct Classification has already been specified for this course"));
                }
                else
                {
                    LearnDirectClassifications.Add(model.LearnDirectClassificationId3);
                }
            }
            if (!String.IsNullOrEmpty(model.LearnDirectClassificationId4))
            {
                if (LearnDirectClassifications.Contains(model.LearnDirectClassificationId4))
                {
                    ModelState.AddModelError("LearnDirectClassificationId4", AppGlobal.Language.GetText(this, "LearnDirectClassificationInUse", "This Learn Direct Classification has already been specified for this course"));
                }
                else
                {
                    LearnDirectClassifications.Add(model.LearnDirectClassificationId4);
                }
            }
            if (!String.IsNullOrEmpty(model.LearnDirectClassificationId5))
            {
                if (LearnDirectClassifications.Contains(model.LearnDirectClassificationId5))
                {
                    ModelState.AddModelError("LearnDirectClassificationId5", AppGlobal.Language.GetText(this, "LearnDirectClassificationInUse", "This Learn Direct Classification has already been specified for this course"));
                }
                else
                {
                    LearnDirectClassifications.Add(model.LearnDirectClassificationId5);
                }
            }
        }
	}
}