using System;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using Entities;

    /// <summary>
    /// The address view model extensions.
    /// </summary>
    public static class AddressViewModelExtensions
    {
        /// <summary>
        /// The cache key.
        /// </summary>
        private const string CacheKey = "AddressViewModel_RegionSelectList";

        /// <summary>
        /// Populate an <see cref="AddressViewModel"/>.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <returns>
        /// The <see cref="AddressViewModel"/>.
        /// </returns>
        public static AddressViewModel Populate(this AddressViewModel model, ProviderPortalEntities db)
        {
            model.Regions = GetRegions(db, model);
            return model;
        }

        /// <summary>
        /// Convert an <see cref="AddressViewModel"/> to an <see cref="Address"/>.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <returns>
        /// The <see cref="Address"/>.
        /// </returns>
        public static Address ToEntity(this AddressViewModel model, ProviderPortalEntities db)
        {
            // Fix the postcode format
            model.Postcode = model.Postcode.ToUpper();
            if (model.Postcode.IndexOf(" ") == -1 && model.Postcode.Length > 3)
            {
                model.Postcode = model.Postcode.Substring(0, model.Postcode.Length - 3) + " " + model.Postcode.Substring(model.Postcode.Length - 3, 3);
            }

            Address address = model.AddressId == 0 ? new Address() : db.Addresses.Find(model.AddressId);
            address.AddressLine1 = model.AddressLine1;
            address.AddressLine2 = model.AddressLine2;
            address.County = model.County;
            address.Town = model.Town;
            address.Postcode = model.Postcode.ToUpper();
            if (!model.HideRegion)
            {
                address.ProviderRegionId = model.RegionId;
            }

            if (model.AddressBaseId != null)
            {
                AddressBase addressBase = db.AddressBases.Find(model.AddressBaseId);
                if (addressBase != null && String.Equals(addressBase.Postcode, model.Postcode, StringComparison.CurrentCultureIgnoreCase))
                {
                    address.Latitude = addressBase.Latitude == null ? (Double?)null : Convert.ToDouble(addressBase.Latitude.Value);
                    address.Longitude = addressBase.Longitude == null ? (Double?)null : Convert.ToDouble(addressBase.Longitude.Value);
                }
            }

            if (address.Latitude == null)
            {
                GeoLocation geo = db.GeoLocations.Find(address.Postcode);
                if (geo != null)
                {
                    address.Latitude = geo.Lat;
                    address.Longitude = geo.Lng;
                }
            }

            return address;
        }

        /// <summary>
        /// Validate an <see cref="AddressViewModel"/>. Base validation is done via model annotations this does additional sanity checks.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public static void Validate(this AddressViewModel model,ProviderPortalEntities db, ModelStateDictionary state)
        {
            if (model.RegionId != null && GetRegions(db, model).All(x => x.Value != model.RegionId.Value.ToString(CultureInfo.InvariantCulture)))
            {
                state.AddModelError("Address.RegionId",
                    AppGlobal.Language.GetText("Address_Region_InvalidRegion", "Invalid region"));
            }
        }

        /// <summary>
        /// Get the regions select list.
        /// </summary>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <param name="model"></param>
        /// <returns>
        /// The <see />.
        /// </returns>
        private static IEnumerable<SelectListItem> GetRegions(ProviderPortalEntities db, AddressViewModel model)
        {
            var items = (List<SelectListItem>)CacheManagement.CacheHandler.Get(CacheKey);
            if (items == null)
            {
                items = new SelectList(db.ProviderRegions.OrderBy(x => x.RegionName), "ProviderRegionId", "RegionName", model.RegionId).ToList();
                items.Insert(
                    0,
                    new SelectListItem
                    {
                        Value = string.Empty,
                        Text =
                            AppGlobal.Language.GetText("Address_Region_SelectRegion", "Select a region")
                    });
                CacheManagement.CacheHandler.Add(CacheKey, items);
            }
            return items;
        }

        public static String GetSingleLineHTMLAddress(this AddressViewModel address)
        {
            return GetFullAddress(address, ", ");
        }

        public static String GetMultipleLineHTMLAddress(this AddressViewModel address)
        {
            return GetFullAddress(address, "<br />");
        }

        private static String GetFullAddress(AddressViewModel address, String delimiter)
        {
            String retValue = !String.IsNullOrEmpty(address.AddressLine1) ? address.AddressLine1 + delimiter : "";
            retValue += !String.IsNullOrEmpty(address.AddressLine2) ? address.AddressLine2 + delimiter : "";
            retValue += !String.IsNullOrEmpty(address.Town) ? address.Town + delimiter : "";
            retValue += !String.IsNullOrEmpty(address.County) ? address.County + delimiter : "";
            retValue += !String.IsNullOrEmpty(address.Postcode) ? address.Postcode + delimiter : "";

            return retValue;
        }

    }
}