using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class AddEditDeliveryLocationViewModel
    {
        public Int32 ProviderId { get; set; }
        public Int32 ApprenticeshipLocationId { get; set; }
        public Int32 ApprenticeshipId { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Location")]
        [Display(Description = "Please select a delivery location.")]
        public Int32 LocationId { get; set; }

        public List<SelectListItem> Locations { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Catchment Radius (miles)")]
        [Display(Description = "Please enter a catchment radius in whole miles.")]
        // e.g. 874 miles, i.e. a provider in Lands End who accepts applicants from John o' Groats
        [ProviderPortalRange(1, 874, ErrorMessage = "Please enter a number of miles between {1} and {2}")]
        public Int32? Radius { get; set; }

        [LanguageDisplay("Delivery Modes <span class='required' aria-required='true'>*</span>")]
        [Display(Description = "Please select one or more delivery modes.")]
        public List<DeliveryMode> DeliveryModes { get; set; }
        public List<Int32> SelectedDeliveryModes { get; set; }

        public Int32 RecordStatusId { get; set; }

        [LanguageDisplay("Apprenticeship")]
        public String ApprenticeshipName { get; set; }
    }

    public class DeliveryLocationListViewModelItem
    {
        public int ApprenticeshipLocationId { get; set; }

        [LanguageDisplay("Location Ref.")]
        public string ProviderOwnLocationRef { get; set; }

        [LanguageDisplay("Location Name")]
        public String LocationName { get; set; }

        [LanguageDisplay("Delivery Modes")]
        public IEnumerable<String> DeliveryModes { get; set; }

        [LanguageDisplay("Catchment Radius (miles)")]
        public Int32? Radius { get; set; }

        [LanguageDisplay("Status")]
        public String Status { get; set; }

        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        [LanguageDisplay("Last Update")]
        public DateTime LastUpdate { get; set; }
    }

    public class DeliveryLocationListViewModel
    {
        public IEnumerable<DeliveryLocationListViewModelItem> Items { get; set; }

        public DeliveryLocationListViewModel()
        {
            Items = new List<DeliveryLocationListViewModelItem>();
        }
    }

    public class ViewDeliveryLocationModel : AddEditDeliveryLocationViewModel
    {
        [LanguageDisplay("Record Status")]
        public String RecordStatusName { get; set; }

        [LanguageDisplay("Location Name")]
        public String LocationName { get; set; }

        [LanguageDisplay("Delivery Modes")]
        public List<DeliveryMode> DeliveryModesChosen { get; set; }

        public ViewDeliveryLocationModel()
        { }

        public ViewDeliveryLocationModel(ApprenticeshipLocation apprenticeshipLocation)
        {
            RecordStatusName = apprenticeshipLocation.RecordStatu.RecordStatusName;
            this.ApprenticeshipName = apprenticeshipLocation.Apprenticeship.ApprenticeshipDetails();
            this.DeliveryModesChosen = apprenticeshipLocation.DeliveryModes.ToList();
            this.LocationName = apprenticeshipLocation.Location.LocationName;
            this.Radius = apprenticeshipLocation.Radius;
        }

    }
}