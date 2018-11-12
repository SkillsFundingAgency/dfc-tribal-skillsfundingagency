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
    public class VenueController : BaseController
    {
        //
        // GET: /Venue
        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // GET: /Venue/Create
        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Create()
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            AddEditVenueModel model = new AddEditVenueModel();

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Create(AddEditVenueModel model)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                CreateFromModel(model, provider.ProviderId);

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
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult CreateFromDialog()
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            AddEditVenueModel model = new AddEditVenueModel
            {
                IsInDialog = true
            };

            // Populate drop downs
            GetLookups(model);

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult CreateFromDialog(AddEditVenueModel model)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            model.IsInDialog = true;

            if (ModelState.IsValid)
            {
                Venue venue = CreateFromModel(model, provider.ProviderId);
                return Json(new VenueJsonModel(venue));
            }

            // Populate drop downs
            GetLookups(model);

            return View("Create", model);
        }

        [NonAction]
        private Venue CreateFromModel(AddEditVenueModel model, Int32 providerId)
        {
            Venue venue = model.ToEntity(db);
            venue.ProviderId = providerId;
            venue.RecordStatusId = (Int32)Constants.RecordStatus.Live;
            Address venueAddress = model.Address.ToEntity(db);
            venue.Address = venueAddress;
            db.Addresses.Add(venueAddress);
            db.Venues.Add(venue);
            db.SaveChanges();

            return venue;
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Edit(Int32? id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Venue venue = db.Venues.Include("Address").FirstOrDefault(x => x.VenueId == id);
            if (venue == null || venue.ProviderId != userContext.ItemId)
            {
                return HttpNotFound();
            }

            AddEditVenueModel model = new AddEditVenueModel(venue);

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public async Task<ActionResult> Edit(Int32 id, AddEditVenueModel model)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (model.VenueId != id)
            {
                return HttpNotFound();
            }

            Venue venue = db.Venues.Find(id);
            if (venue == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                venue = model.ToEntity(db);
                if (venue.ProviderId != userContext.ItemId)
                {
                    // User is trying to change the VenueId (or the context has changed)
                    return HttpNotFound();
                }

                venue.ModifiedByUserId = Permission.GetCurrentUserId();
                venue.ModifiedDateTimeUtc = DateTime.UtcNow;

                Address venueAddress = model.Address.ToEntity(db);

                db.Entry(venueAddress).State = EntityState.Modified;
                db.Entry(venue).State = EntityState.Modified;
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

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Archive(Int32 id, Boolean archiveOpportunities)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Venue venue = db.Venues.Find(id);
            if (venue == null || venue.ProviderId != userContext.ItemId || venue.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            venue.Archive(db, archiveOpportunities);
            db.SaveChanges();
            ShowGenericSavedMessage(true);
            return RedirectToAction("Edit", "Venue", new { Id = venue.VenueId });
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Unarchive(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Venue venue = db.Venues.Find(id);
            if (venue == null || venue.ProviderId != userContext.ItemId || venue.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            venue.Unarchive(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);
            return RedirectToAction("Edit", "Venue", new { Id = venue.VenueId });
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanDeleteProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Delete(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Venue venue = db.Venues.Find(id);
            if (venue == null || venue.ProviderId != userContext.ItemId || venue.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            venue.Delete(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);
            return RedirectToAction("List");
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public NewtonsoftJsonResult GetOpportunities(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                throw new Exception("Invalid User Context");
            }

            Venue venue = db.Venues.Find(id);
            if (venue == null || venue.ProviderId != userContext.ItemId || venue.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                throw new Exception("Invalid User Context");
            }

            return Json(venue.CourseInstances.Where(x => x.RecordStatusId == (Int32)Constants.RecordStatus.Live).Select(x => x.CourseInstanceId));
        }
        
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult List()
        {
            if (userContext.ContextName != UserContext.UserContextName.Provider)
            {
                return RedirectToAction("Index", "Home");
            }

            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            VenueSearchModel model = new VenueSearchModel();
            foreach (Venue v in db.Venues.Include("Address").Include("RecordStatu").Where(x => x.ProviderId == userContext.ItemId).OrderByDescending(x => x.ModifiedDateTimeUtc ?? x.CreatedDateTimeUtc))
            {
                model.Venues.Add(new ListVenueModel(v));
            }

            return View(model);
        }


        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderVenue)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult View(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Venue venue = db.Venues.Include("Address").FirstOrDefault(x => x.VenueId == id);
            if (venue == null || venue.ProviderId != userContext.ItemId)
            {
                return HttpNotFound();
            }

            ViewVenueModel model = new ViewVenueModel(venue);

            return View(model);
        }

        [NonAction]
        private void GetLookups(AddEditVenueModel model)
        {
            model.Address.Populate(db);
            ViewBag.RecordStatuses = new SelectList(
                db.RecordStatus,
                "RecordStatusId",
                "RecordStatusName",
                model.RecordStatusId);
        }
    }
}