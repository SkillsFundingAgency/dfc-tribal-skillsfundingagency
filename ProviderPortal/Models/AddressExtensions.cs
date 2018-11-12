using System;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class AddressExtensions
    {
        public static String GetSingleLineHTMLAddress(this Address address)
        {
            return GetFullAddress(address, ", ");
        }

        public static String GetMultipleLineHTMLAddress(this Address address)
        {
            return GetFullAddress(address, "<br />");
        }

        private static String GetFullAddress(Address address, String delimiter)
        {
            String retValue = !String.IsNullOrEmpty(address.AddressLine1) ? address.AddressLine1 + delimiter : "";
            retValue += !String.IsNullOrEmpty(address.AddressLine2) ? address.AddressLine2 + delimiter : "";
            retValue += !String.IsNullOrEmpty(address.Town) ? address.Town + delimiter : "";
            retValue += !String.IsNullOrEmpty(address.County) ? address.County + delimiter : "";
            retValue += !String.IsNullOrEmpty(address.Postcode) ? address.Postcode /*+ delimiter*/ : "";

            return retValue;
        }

        public static void Delete(this Address address, ProviderPortalEntities db)
        {
            db.Entry(address).State = System.Data.Entity.EntityState.Deleted;
        }

        /// <summary>
        /// Clones the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static Address Clone(this Address address)
        {
            return new Address
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                Town = address.Town,
                County = address.County,
                Postcode = address.Postcode,
                ProviderRegionId = address.ProviderRegionId,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                Geography = address.Geography
            };
        }
    }
}