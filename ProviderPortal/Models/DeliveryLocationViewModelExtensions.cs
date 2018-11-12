using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Provider;
using Microsoft.SqlServer.Server;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class DeliveryLocationListViewModelExtensions
    {
        public static DeliveryLocationListViewModel Populate(this DeliveryLocationListViewModel model,
            int apprenticeshipId, ProviderPortalEntities db)
        {
            if (model == null)
            {
                model = new DeliveryLocationListViewModel();
            }

            model.Items = db.ApprenticeshipLocations
                .Where(x => x.ApprenticeshipId == apprenticeshipId)
                .Select(x => new DeliveryLocationListViewModelItem()
                {
                    ApprenticeshipLocationId = x.ApprenticeshipLocationId,
                    ProviderOwnLocationRef = x.Location.ProviderOwnLocationRef,
                    LocationName = x.Location.LocationName,
                    DeliveryModes = x.DeliveryModes.Select(y => y.DeliveryModeName).OrderBy(y => y),
                    Radius = x.Radius,
                    Status = x.RecordStatu.RecordStatusName,
                    LastUpdate = x.ModifiedDateTimeUtc ?? x.CreatedDateTimeUtc
                })
                .OrderByDescending(x => x.LastUpdate)
                .ToList();
            return model;
        }
    }

    public static class AddEditDeliveryLocationViewModelExtensions
    {
        public static AddEditDeliveryLocationViewModel PopulateNew(this AddEditDeliveryLocationViewModel model,
            int apprenticeshipId, ProviderPortalEntities db)
        {
            var userContext = UserContext.GetUserContext();
            model = new AddEditDeliveryLocationViewModel
            {
                RecordStatusId = (Int32) Constants.RecordStatus.Live,
                ApprenticeshipId = apprenticeshipId,
                SelectedDeliveryModes = new List<Int32>()
            };
            var apprenticeship = db.Apprenticeships
                .FirstOrDefault(x => x.ApprenticeshipId == apprenticeshipId);
            model.ApprenticeshipName = apprenticeship != null
                ? apprenticeship.ApprenticeshipDetails()
                : String.Empty;
            model.ProviderId = apprenticeship != null ? apprenticeship.ProviderId : 0;
            model.AddMetaData(db, userContext);
            return model;
        }

        public static void AddMetaData(this AddEditDeliveryLocationViewModel model,
            ProviderPortalEntities db, UserContext.UserContextInfo userContext)
        {
            model.DeliveryModes = db.DeliveryModes.ToList();
            model.Locations = db.Locations
                .Where(x => x.ProviderId == userContext.ItemId.Value)
                .Select(x => new SelectListItem
                {
                    Text = x.LocationName,
                    // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                    Value = x.LocationId.ToString()
                })
                .ToList();
            model.SelectedDeliveryModes = model.SelectedDeliveryModes ?? new List<Int32>();
            if (String.IsNullOrEmpty(model.ApprenticeshipName))
            {
                var apprenticeship = db.Apprenticeships
                    .FirstOrDefault(x => x.ApprenticeshipId == model.ApprenticeshipId);
                model.ApprenticeshipName = apprenticeship != null
                    ? apprenticeship.ApprenticeshipDetails()
                    : String.Empty;
            }
        }

        public static AddEditDeliveryLocationViewModel Populate(this AddEditDeliveryLocationViewModel model,
            int apprenticeshipLocationId, ProviderPortalEntities db)
        {
            var userContext = UserContext.GetUserContext();
            if (!userContext.IsProvider()) return null;
            if (apprenticeshipLocationId == 0) return null;

            var deliveryLocation = db.ApprenticeshipLocations
                .FirstOrDefault(x => x.Apprenticeship.ProviderId == userContext.ItemId.Value
                                     && x.ApprenticeshipLocationId == apprenticeshipLocationId);
            if (deliveryLocation == null) return null;

            model = new AddEditDeliveryLocationViewModel
            {
                ProviderId = deliveryLocation.Apprenticeship.ProviderId,
                ApprenticeshipLocationId = deliveryLocation.ApprenticeshipLocationId,
                ApprenticeshipId = deliveryLocation.ApprenticeshipId,
                RecordStatusId = deliveryLocation.RecordStatusId,
                DeliveryModes = db.DeliveryModes.ToList(),
                SelectedDeliveryModes = deliveryLocation.DeliveryModes.Select(x => x.DeliveryModeId).ToList(),
                LocationId = deliveryLocation.LocationId,
                Radius = deliveryLocation.Radius,
                ApprenticeshipName = deliveryLocation.Apprenticeship.ApprenticeshipDetails()
            };
            model.AddMetaData(db, userContext);

            return model;
        }

        public static bool Validate(this AddEditDeliveryLocationViewModel model, ProviderPortalEntities db, ModelStateDictionary modelState)
        {
            if (model.SelectedDeliveryModes == null || !model.SelectedDeliveryModes.Any())
            {
                modelState.AddModelError("DeliveryModes", AppGlobal.Language.GetText("DeliveryMode_Edit_DeliveryModesMandatory", "The Delivery Mode field is required"));
            }
            // Check Whether Location Has Already Been Used For This Apprenticeship
            ApprenticeshipLocation al = db.ApprenticeshipLocations.FirstOrDefault(x => x.ApprenticeshipId == model.ApprenticeshipId && x.LocationId == model.LocationId && (model.ApprenticeshipLocationId == 0 || x.ApprenticeshipLocationId != model.ApprenticeshipLocationId));
            if (al != null)
            {
                modelState.AddModelError("LocationId", AppGlobal.Language.GetText("DeliveryLocation_Edit_LocationAlreadyInUse", "The Location supplied is already in use for this apprenticeship"));
            }
            return modelState.IsValid;
        }

        public static ApprenticeshipLocation ToEntity(this AddEditDeliveryLocationViewModel model,
            int apprenticeshipId, int apprenticeshipLocationId, ProviderPortalEntities db)
        {
            var userContext = UserContext.GetUserContext();
            var userId = Permission.GetCurrentUserId();
            if (!userContext.IsProvider()) return null;
            var apprenticeshipLocation = model.ApprenticeshipLocationId != 0
                ? db.ApprenticeshipLocations.FirstOrDefault(
                    x =>
                        x.ApprenticeshipLocationId == model.ApprenticeshipLocationId &&
                        x.Apprenticeship.ProviderId == userContext.ItemId.Value)
                : null;
            apprenticeshipLocation = apprenticeshipLocation ?? new ApprenticeshipLocation()
            {
                CreatedByUserId = userId,
                CreatedDateTimeUtc = DateTime.UtcNow,
                ApprenticeshipId = apprenticeshipId,
                RecordStatusId = (int)Constants.RecordStatus.Live
            };
            apprenticeshipLocation.ModifiedByUserId = userId;
            apprenticeshipLocation.ModifiedDateTimeUtc = DateTime.UtcNow;
            apprenticeshipLocation.AddedByApplicationId = (int)Constants.Application.Portal;
            apprenticeshipLocation.Radius = model.Radius;
            apprenticeshipLocation.LocationId = model.LocationId;
            var apprenticeship = apprenticeshipLocation.Apprenticeship
                     ?? db.Apprenticeships.First(x => x.ApprenticeshipId == apprenticeshipId);
            apprenticeship.ModifiedByUserId = userId;
            apprenticeship.ModifiedDateTimeUtc = DateTime.UtcNow;
            if (apprenticeshipLocation.RecordStatusId != (int) Constants.RecordStatus.Archived)
            {
                apprenticeshipLocation.RecordStatusId = (int)Constants.RecordStatus.Live;
                apprenticeship.RecordStatusId = (int) Constants.RecordStatus.Live;
            }

            // Remove Existing Delivery Modes
            List<DeliveryMode> existingDeliveryModes = apprenticeshipLocation.DeliveryModes.ToList();
            foreach (var deliveryMode in existingDeliveryModes.Where(x => !model.SelectedDeliveryModes.Contains(x.DeliveryModeId)))
            {
                apprenticeshipLocation.DeliveryModes.Remove(deliveryMode);
            }

            // Add the Delivery Modes
            foreach (Int32 fcId in model.SelectedDeliveryModes)
            {
                DeliveryMode deliveryMode = apprenticeshipLocation.DeliveryModes.FirstOrDefault(x => x.DeliveryModeId == fcId);
                if (deliveryMode == null)
                {
                    deliveryMode = db.DeliveryModes.Find(fcId);
                    if (deliveryMode != null)
                    {
                        apprenticeshipLocation.DeliveryModes.Add(deliveryMode);
                    }
                }
            }

            return apprenticeshipLocation;
        }
    }
}