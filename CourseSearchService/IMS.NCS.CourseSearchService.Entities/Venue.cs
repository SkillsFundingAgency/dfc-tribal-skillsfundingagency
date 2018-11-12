using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.CourseSearchService.Entities
{
    /// <summary>
    /// Venue Entity.
    /// </summary>
    public class Venue
    {
        #region Variables

        private string _addressLine1;
        private string _addressLine2;
        private string _county;
        private string _email;
        private string _facilities;
        private string _fax;
        private string _phone;
        private string _postcode;
        private string _town;
        private long _venueId;
        private string _venueName;
        private string _website;
        private string _latitude;
        private string _longitude;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Address Line 1 field.
        /// </summary>
        public string AddressLine1
        {
            get
            {
                return _addressLine1;
            }
            set
            {
                _addressLine1 = value;
            }
        }

        /// <summary>
        /// Address Line 2 field.
        /// </summary>
        public string AddressLine2
        {
            get
            {
                return _addressLine2;
            }
            set
            {
                _addressLine2 = value;
            }
        }

        /// <summary>
        /// County fields.
        /// </summary>
        public string County
        {
            get
            {
                return _county;
            }
            set
            {
                _county = value;
            }
        }

        /// <summary>
        /// Email field.
        /// </summary>
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        /// <summary>
        /// Facilities field.
        /// </summary>
        public string Facilities
        {
            get
            {
                return _facilities;
            }
            set
            {
                _facilities = value;
            }
        }

        /// <summary>
        /// Fax field.
        /// </summary>
        public string Fax
        {
            get
            {
                return _fax;
            }
            set
            {
                _fax = value;
            }
        }

        /// <summary>
        /// Phone field.
        /// </summary>
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
            }
        }

        /// <summary>
        /// Postcode field.
        /// </summary>
        public string Postcode
        {
            get
            {
                return _postcode;
            }
            set
            {
                _postcode = value;
            }
        }

        /// <summary>
        /// Town field.
        /// </summary>
        public string Town
        {
            get
            {
                return _town;
            }
            set
            {
                _town = value;
            }
        }

        /// <summary>
        /// Venue Id field.
        /// </summary>
        public long VenueId
        {
            get
            {
                return _venueId;
            }
            set
            {
                _venueId = value;
            }
        }

        /// <summary>
        /// Venue name field.
        /// </summary>
        public string VenueName
        {
            get
            {
                return _venueName;
            }
            set
            {
                _venueName = value;
            }
        }

        /// <summary>
        /// Website field.
        /// </summary>
        public string Website
        {
            get
            {
                return _website;
            }
            set
            {
                _website = value;
            }
        }


        /// <summary>
        /// Latitude field
        /// </summary>
        public string Latitude
        {
            get 
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
            }
        }

        /// <summary>
        /// Longitude field
        /// </summary>
        public string Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
            }
        }

        #endregion Properties
    }
}
