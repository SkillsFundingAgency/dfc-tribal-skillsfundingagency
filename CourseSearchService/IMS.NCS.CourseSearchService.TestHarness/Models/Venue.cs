using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.NCS.CourseSearchService.TestHarness.Models
{
    /// <summary>
    /// Represents an venue as part of the course details
    /// </summary>
    public class Venue
    {
        public string VenueName { get; set; }
        public string VenueId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Facilities { get; set; }  
    }
}