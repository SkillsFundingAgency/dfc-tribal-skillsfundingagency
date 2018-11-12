using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.CourseSearchService.Entities
{
    /// <summary>
    /// LDCS Code Entity.
    /// </summary>
    public class LdcsCode
    {
        #region Variables

        private long _courseCount;
        private string _ldcsCodeDescription;
        private string _ldcsCodeValue;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Course count field.
        /// </summary>
        public long CourseCount
        {
            get
            {
                return _courseCount;
            }
            set
            {
                _courseCount = value;
            }
        }

        /// <summary>
        /// LDCS code description field.
        /// </summary>
        public string LdcsCodeDescription
        {
            get
            {
                return _ldcsCodeDescription;
            }
            set
            {
                _ldcsCodeDescription = value;
            }
        }

        /// <summary>
        /// LDCS code value field.
        /// </summary>
        public string LdcsCodeValue
        {
            get
            {
                return _ldcsCodeValue;
            }
            set
            {
                _ldcsCodeValue = value;
            }
        }

        #endregion Properties
    }
}
