using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    public class DeliveryLocationController : BaseController
    {
        //
        // GET: /Apprenticeship
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Index()
        {
            return RedirectToAction("List", "Apprenticeship");
        }

        [PermissionAuthorize(Permission.PermissionName.CanViewProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult List()
        {
            return RedirectToAction("List", "Apprenticeship");
        }

        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Create(int id)
        {
            var model = new AddEditDeliveryLocationViewModel();
            model = model.PopulateNew(id, db);
            return View(model);
        }

        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        [HttpPost]
        public ActionResult Create(int id, AddEditDeliveryLocationViewModel model)
        {
            Apprenticeship app = db.Apprenticeships.Find(id);
            if (app == null)
            {
                return HttpNotFound();
            }

            if (app.ProviderId != userContext.ItemId)
            {
                return HttpNotFound();
            }

            Location location = db.Locations.Find(model.LocationId);
            if (location == null || location.ProviderId != app.ProviderId)
            {
                ModelState.AddModelError("Location", AppGlobal.Language.GetText("DeliveryLocation_Create_InvalidLocation", "The Location is not valid."));
            }

            // Ensure that the selected location is valid for the delivery modes selected
            CheckLocationIsValidForDeliveryModes(location, model.SelectedDeliveryModes);

            model.ApprenticeshipLocationId = 0;
            model.Validate(db, ModelState);
            if (ModelState.IsValid)
            {
                model.ApprenticeshipId = id;
                var deliveryLocation = model.ToEntity(id, 0, db);
                db.ApprenticeshipLocations.Add(deliveryLocation);
                UpdateDeliveryModes(model, deliveryLocation);
                db.SaveChanges();
                ShowGenericSavedMessage();
                return Request.Form["Create"] != null
                    ? RedirectToAction("Edit", "Apprenticeship", new { id = deliveryLocation.ApprenticeshipId })
                    : RedirectToAction("Create", "DeliveryLocation", new { id = deliveryLocation.ApprenticeshipId });
            }

            model.AddMetaData(db, userContext);
            return View(model);
        }

        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Edit(Int32? id)
        {
            if (id == null || id == 0)
            {
                RedirectToAction("Create");
            }

            if (!db.Providers.Any(x => x.ProviderId == userContext.ItemId.Value)
                || !db.ApprenticeshipLocations
                    .Any(x => x.ApprenticeshipLocationId == id
                              && x.Apprenticeship.ProviderId == userContext.ItemId.Value))
            {
                return HttpNotFound();
            }

            var model = new AddEditDeliveryLocationViewModel();
            model = model.Populate(id.Value, db);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        [HttpPost]
        public ActionResult Edit(AddEditDeliveryLocationViewModel model)
        {
            if (!db.Providers.Any(x => x.ProviderId == userContext.ItemId.Value)
                || !db.ApprenticeshipLocations
                    .Any(x => x.ApprenticeshipLocationId == model.ApprenticeshipLocationId
                              && x.Apprenticeship.ProviderId == userContext.ItemId.Value))
            {
                return HttpNotFound();
            }

            Location location = db.Locations.Find(model.LocationId);
            if (location == null || location.ProviderId != userContext.ItemId)
            {
                ModelState.AddModelError("Location", AppGlobal.Language.GetText("DeliveryLocation_Edit_InvalidLocation", "The Location is not valid."));
            }

            // Ensure that the selected location is valid for the delivery modes selected
            CheckLocationIsValidForDeliveryModes(location, model.SelectedDeliveryModes);

            model.Validate(db, ModelState);
            if (ModelState.IsValid)
            {
                var deliveryLocation = model.ToEntity(model.ApprenticeshipId, model.ApprenticeshipLocationId, db);
                db.Entry(deliveryLocation).State = EntityState.Modified;
                db.SaveChanges();
                UpdateDeliveryModes(model, deliveryLocation);
                ShowGenericSavedMessage();
                return RedirectToAction("Edit", "Apprenticeship", new { id = model.ApprenticeshipId });
            }

            model.AddMetaData(db, userContext);
            return View(model);
        }

        [HttpGet]
        [PermissionAuthorize(Permission.PermissionName.CanViewProviderApprenticeship, Permission.PermissionName.CanEditProviderApprenticeship)]
        public ActionResult View(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            ApprenticeshipLocation apprenticeshipLocation = db.ApprenticeshipLocations.FirstOrDefault(x => x.ApprenticeshipLocationId == id);
            if (apprenticeshipLocation == null || apprenticeshipLocation.Apprenticeship.ProviderId != userContext.ItemId)
            {
                return HttpNotFound();
            }

            ViewDeliveryLocationModel model = new ViewDeliveryLocationModel(apprenticeshipLocation);

            return View(model);
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Archive(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            ApprenticeshipLocation deliveryLocation = db.ApprenticeshipLocations.Find(id);
            if (deliveryLocation == null || deliveryLocation.Apprenticeship.ProviderId != userContext.ItemId ||
                deliveryLocation.Apprenticeship.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            deliveryLocation.Archive(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Edit", "Apprenticeship", new { Id = deliveryLocation.ApprenticeshipId });
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Unarchive(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            var apprenticeshipLocations = db.ApprenticeshipLocations.Find(id);
            if (apprenticeshipLocations == null ||
                apprenticeshipLocations.Apprenticeship.ProviderId != userContext.ItemId ||
                apprenticeshipLocations.Apprenticeship.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            apprenticeshipLocations.Unarchive(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Edit", "Apprenticeship", new { id = apprenticeshipLocations.ApprenticeshipId });
        }

        [HttpPost]
        [PermissionAuthorize(Permission.PermissionName.CanEditProviderApprenticeship)]
        [ContextAuthorize(UserContext.UserContextName.Provider)]
        public ActionResult Delete(Int32 id)
        {
            Provider provider = db.Providers.Find(userContext.ItemId);
            if (provider == null)
            {
                return HttpNotFound();
            }

            var apprenticeshipLocation = db.ApprenticeshipLocations.Find(id);
            if (apprenticeshipLocation == null || apprenticeshipLocation.Apprenticeship.ProviderId != userContext.ItemId ||
                apprenticeshipLocation.Apprenticeship.RecordStatusId == (Int32)Constants.RecordStatus.Deleted)
            {
                return HttpNotFound();
            }

            apprenticeshipLocation.Apprenticeship.ModifiedByUserId = Permission.GetCurrentUserId();
            apprenticeshipLocation.Apprenticeship.ModifiedDateTimeUtc = DateTime.UtcNow;
            apprenticeshipLocation.Delete(db);
            db.SaveChanges();
            ShowGenericSavedMessage(true);

            return RedirectToAction("Edit", "Apprenticeship", new { id = apprenticeshipLocation.ApprenticeshipId });
        }

        [NonAction]
        private void UpdateDeliveryModes(AddEditDeliveryLocationViewModel model, ApprenticeshipLocation deliveryLocation)
        {

        }

        [NonAction]
        private void CheckLocationIsValidForDeliveryModes(Location location, IEnumerable<Int32> deliveryModes)
        {
            if (location != null && String.IsNullOrEmpty(location.Address.AddressLine1) && deliveryModes != null)
            {
                String deliveryModeNames = String.Empty;
                foreach (Int32 id in deliveryModes)
                {
                    DeliveryMode dm = db.DeliveryModes.Find(id);
                    if (dm != null && dm.MustHaveFullLocation)
                    {
                        if (deliveryModeNames.Length > 0)
                        {
                            deliveryModeNames += ", ";
                        }
                        deliveryModeNames += dm.DeliveryModeName;
                    }
                }
                if (!String.IsNullOrWhiteSpace(deliveryModeNames))
                {
                    // Add the model error
                    ModelState.AddModelError("Location", String.Format(AppGlobal.Language.GetText("DeliveryLocation_Create_DeliveryModeMustHaveFullLocation", "The Location must have a full address (including Address Line 1) when the following delivery mode(s) are used: {0}"), deliveryModeNames));
                }
            }
        }
    }
}