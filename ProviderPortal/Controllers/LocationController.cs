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
    public class LocationController : BaseController
    {
        readonly String unableToFindLatLngMessage = AppGlobal.Language.GetText("Location_Edit_UnableToFindLatLong", "Postcode cannot be used because we are unable to find latitude and longitude information for it.");

        //
        // GET: /Location
        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderLocation)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // GET: /Location/Create
        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderLocation)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Create()
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            var model = new AddEditLocationModel
            {
                ProviderId = provider.ProviderId
            };

            // Populate drop downs
            GetLookups(model);

            ViewBag.CanProviderCreateNewLocation = CanProviderCreateNewLocation(provider, out Int32 maxNumberOfLocations);
            ViewBag.MaxNumberOfLocations = maxNumberOfLocations.ToString("N0");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderLocation)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Create(AddEditLocationModel model)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (model.ProviderId != provider.ProviderId)
            {
                return HttpNotFound();
            }

            Location loc = db.Locations.FirstOrDefault(x => x.LocationName == model.LocationName && x.ProviderId == provider.ProviderId);
            if (loc != null)
            {
                ModelState.AddModelError("LocationName", AppGlobal.Language.GetText("Location_Create_DuplicateLocatioName", "The Location Name supplied is already in use."));
            }

            Address address = model.Address.ToEntity(db);
            Location locDupPostcode = db.Locations.FirstOrDefault(x => x.RecordStatusId == (int)Constants.RecordStatus.Live && x.Address.Postcode == address.Postcode && x.ProviderId == provider.ProviderId);
            if (locDupPostcode != null)
            {
                ModelState.AddModelError("Address.Postcode", AppGlobal.Language.GetText("Location_Create_DuplicatePostcode", "The postcode supplied is already in use."));
            }

            CheckMaxNumberOfLocations(provider);

            if (ModelState.IsValid)
            {
                Location location = CreateFromModel(model, provider.ProviderId);
                if (!location.Address.Latitude.HasValue)
                {
                    ModelState.AddModelError("Address.Postcode", unableToFindLatLngMessage);
                }
                else
                {
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
            }

            // Populate drop downs
            GetLookups(model);

            // Set this to true here so that the message doesn't appear twice
            ViewBag.CanProviderCreateNewLocation = true;

            return View(model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderLocation)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult CreateFromDialog(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (id != userContext.ItemId)
            {
                return View("UnableToCreate");
            }

            var model = new AddEditLocationModel
            {
                ProviderId = provider.ProviderId,
                IsInDialog = true
            };

            // Populate drop downs
            GetLookups(model);

            ViewBag.CanProviderCreateNewLocation = CanProviderCreateNewLocation(provider, out Int32 maxNumberOfLocations);
            ViewBag.MaxNumberOfLocations = maxNumberOfLocations.ToString("N0");

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanAddProviderLocation)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult CreateFromDialog(AddEditLocationModel model)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (model.ProviderId != provider.ProviderId)
            {
                return View("UnableToCreate");
            }

            model.IsInDialog = true;

            Location loc = db.Locations.FirstOrDefault(x => x.LocationName == model.LocationName && x.ProviderId == provider.ProviderId);
            if (loc != null)
            {
                ModelState.AddModelError("LocationName", AppGlobal.Language.GetText("Location_Create_DuplicateLocatioName", "The Location Name supplied is already in use."));
            }

            Address address = model.Address.ToEntity(db);
            Location locDupPostcode = db.Locations.FirstOrDefault(x => x.RecordStatusId == (int)Constants.RecordStatus.Live && x.Address.Postcode == address.Postcode && x.ProviderId == provider.ProviderId);
            if (locDupPostcode != null)
            {
                ModelState.AddModelError("Address.Postcode", AppGlobal.Language.GetText("Location_Create_DuplicatePostcode", "The postcode supplied is already in use."));
            }

            if (!String.IsNullOrWhiteSpace(model.Website))
            {
                model.Website = UrlHelper.GetFullUrl(model.Website);
                Boolean websiteValid = false;
                Uri uriResult;
                if (Uri.TryCreate(model.Website, UriKind.Absolute, out uriResult))
                {
                    if (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                    {
                        websiteValid = true;
                    }
                }
                if (!websiteValid)
                {
                    ModelState.AddModelError("Website", AppGlobal.Language.GetText(this, "WebsiteNotValid", "Please enter a valid Url"));
                }
            }

            CheckMaxNumberOfLocations(provider);

            if (ModelState.IsValid)
            {
                Location location = CreateFromModel(model, provider.ProviderId);
                if (!location.Address.Latitude.HasValue)
                {
                    ModelState.AddModelError("Address.Postcode", unableToFindLatLngMessage);
                }
                else
                {
                    return Json(new LocationJsonModel(location));
                }
            }

            // Populate drop downs
            GetLookups(model);

            // Set this to true here so that the message doesn't appear twice
            ViewBag.CanProviderCreateNewLocation = true;

            return View("Create", model);
        }

        [NonAction]
        private Location CreateFromModel(AddEditLocationModel model, Int32 providerId)
        {
            Location location = model.ToEntity(db);
            location.ProviderId = providerId;
            location.RecordStatusId = (Int32) Constants.RecordStatus.Live;
            Address locationAddress = model.Address.ToEntity(db);
            location.Address = locationAddress;
            if (locationAddress.Latitude.HasValue)
            {
                db.Addresses.Add(locationAddress);
                db.Locations.Add(location);
                db.SaveChanges();
            }

            return location;
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderLocation)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Edit(Int32? id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Location location = db.Locations.Include("Address").FirstOrDefault(x => x.LocationId == id);
            if (location == null || location.ProviderId != userContext.ItemId)
            {
                return HttpNotFound();
            }

            var model = new AddEditLocationModel(location);

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderLocation)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public async Task<ActionResult> Edit(Int32 id, AddEditLocationModel model)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (model.LocationId != id)
            {
                return HttpNotFound();
            }

            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            Location loc = db.Locations.FirstOrDefault(x => x.LocationId != location.LocationId && x.LocationName == model.LocationName && x.ProviderId == provider.ProviderId);
            if (loc != null)
            {
                ModelState.AddModelError("LocationName", AppGlobal.Language.GetText("Location_Edit_DuplicateLocatioName", "The Location Name supplied is already in use."));
            }

            Address address = model.Address.ToEntity(db);
            Location locDupPostcode = db.Locations.FirstOrDefault(x => x.RecordStatusId == (int)Constants.RecordStatus.Live && x.Address.Postcode == address.Postcode && x.ProviderId == provider.ProviderId && x.LocationId != model.LocationId);
            if (locDupPostcode != null)
            {
                ModelState.AddModelError("Address.Postcode", AppGlobal.Language.GetText("Location_Create_DuplicatePostcode", "The postcode supplied is already in use."));
            }

            if (ModelState.IsValid)
            {
                location = model.ToEntity(db);
                if (location.ProviderId != userContext.ItemId)
                {
                    // User is trying to change the LocationId (or the context has changed)
                    return HttpNotFound();
                }

                location.ModifiedByUserId = Permission.GetCurrentUserId();
                location.ModifiedDateTimeUtc = DateTime.UtcNow;

                Address locationAddress = model.Address.ToEntity(db);

                if (!locationAddress.Latitude.HasValue)
                {
                    ModelState.AddModelError("Address.Postcode", unableToFindLatLngMessage);
                }
                else
                {
                    db.Entry(locationAddress).State = EntityState.Modified;
                    db.Entry(location).State = EntityState.Modified;
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
            }

            // Populate drop downs
            GetLookups(model);

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderLocation)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Archive(Int32 id, Boolean archiveApprenticeships)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Location location = db.Locations.Find(id);
            if (location == null || location.ProviderId != userContext.ItemId ||
                location.RecordStatusId == (Int32) Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            location.Archive(db, archiveApprenticeships);
            db.SaveChanges();
            ShowGenericSavedMessage(true);
            return RedirectToAction("Edit", "Location", new { Id = location.LocationId });
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderLocation)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Unarchive(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Location location = db.Locations.Find(id);
            if (location == null || location.ProviderId != userContext.ItemId ||
                location.RecordStatusId == (Int32) Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            Location locDupPostcode = db.Locations.FirstOrDefault(x => x.RecordStatusId == (int)Constants.RecordStatus.Live && x.LocationId != location.LocationId && x.Address.Postcode == location.Address.Postcode && x.ProviderId == provider.ProviderId);
            if (locDupPostcode != null)
            {
                var message = AppGlobal.Language.GetText(this, "UnArchiveFailed", "Unable to unarchive location, there is already a location using this postcode.");
                SessionMessage.SetMessage(message, SessionMessageType.Danger, 2);
                return RedirectToAction("Edit", "Location", new { Id = location.LocationId });
            }

            location.Unarchive(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);
            return RedirectToAction("Edit", "Location", new { Id = location.LocationId });
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanDeleteProviderLocation)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Delete(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Location location = db.Locations.Find(id);
            if (location == null || location.ProviderId != userContext.ItemId ||
                location.RecordStatusId == (Int32) Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            location.Delete(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);
            return RedirectToAction("List");
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderLocation)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public NewtonsoftJsonResult GetApprenticeships(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                throw new Exception("Invalid User Context");
            }

            Location location = db.Locations.Find(id);
            if (location == null || location.ProviderId != userContext.ItemId ||
                location.RecordStatusId == (Int32) Constants.RecordStatus.Deleted)
            {
                throw new Exception("Invalid User Context");
            }

            return
                Json(
                    location.ApprenticeshipLocations.Where(x => x.RecordStatusId == (Int32) Constants.RecordStatus.Live)
                        .Select(x => x.ApprenticeshipId));
        }

        [PermissionAuthorize(Permission.PermissionName.CanViewProviderLocation)]
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

            var model = new LocationSearchModel();
            foreach (
                Location v in
                    db.Locations.Include("Address")
                        .Include("RecordStatu")
                        .Where(x => x.ProviderId == userContext.ItemId)
                        .OrderByDescending(x => x.ModifiedDateTimeUtc ?? x.CreatedDateTimeUtc))
            {
                model.Locations.Add(new ListLocationModel(v));
            }

            ViewBag.CanProviderCreateNewLocation = CanProviderCreateNewLocation(provider, out Int32 maxNumberOfLocations);
            ViewBag.MaxNumberOfLocations = maxNumberOfLocations.ToString("N0");

            return View(model);
        }


        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderLocation)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult View(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            Location location = db.Locations.Include("Address").FirstOrDefault(x => x.LocationId == id);
            if (location == null || location.ProviderId != userContext.ItemId)
            {
                return HttpNotFound();
            }

            var model = new ViewLocationModel(location);

            return View(model);
        }

        [NonAction]
        private void GetLookups(AddEditLocationModel model)
        {
          //  model.Address.Populate(db);
            ViewBag.RecordStatuses = new SelectList(
                db.RecordStatus,
                "RecordStatusId",
                "RecordStatusName",
                model.RecordStatusId);
        }

        [NonAction]
        private void CheckMaxNumberOfLocations(Provider provider)
        {
            if (!CanProviderCreateNewLocation(provider, out Int32 maxNumberOfLocations))
            {
                ModelState.AddModelError("", String.Format(AppGlobal.Language.GetText(this, "MaxNumberOfLocationsReached", "You have reached the maximum number of locations allowed {0}.  If you require more locations please contact our helpdesk."), maxNumberOfLocations.ToString("N0")));
            }
        }

        [NonAction]
        private Boolean CanProviderCreateNewLocation(Provider provider, out Int32 maxNumberOfLocations)
        {
            maxNumberOfLocations = Constants.ConfigSettings.MaxLocations;
            if (provider.MaxLocations.HasValue)
            {
                maxNumberOfLocations = provider.MaxLocations.Value;
            }
            return provider.Locations.Count() < maxNumberOfLocations;
        }
    }
}