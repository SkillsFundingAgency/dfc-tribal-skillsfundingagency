using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.CourseSearchService.Entities
{
    /// <summary>
    /// CourseListResponse entity.
    /// </summary>
    public class CourseListResponse
    {
        #region Variables

        private List<Course> _courses;
        private List<LdcsCode> _ldcsCodes;
        private int _numberOfRecords;
        private string _searchHeaderId;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Courses field.
        /// </summary>
        public List<Course> Courses
        {
            get
            {
                if (_courses == null)
                {
                    _courses = new List<Course>();
                }
                return _courses;
            }
            set
            {
                _courses = value;
            }
        }

        /// <summary>
        /// LdcsCodes field.
        /// </summary>
        public List<LdcsCode> LdcsCodes
        {
            get
            {
                if (_ldcsCodes == null)
                {
                    _ldcsCodes = new List<LdcsCode>();
                }
                return _ldcsCodes;
            }
            set
            {
                _ldcsCodes = value;
            }
        }

        /// <summary>
        /// Number of Records field.
        /// </summary>
        public int NumberOfRecords
        {
            get
            {
                return _numberOfRecords;
            }
            set
            {
                _numberOfRecords = value;
            }
        }

        /// <summary>
        /// Search Header Id field.
        /// </summary>
        public string SearchHeaderId
        {
            get
            {
                return _searchHeaderId;
            }
            set
            {
                _searchHeaderId = value;
            }
        }

        #endregion Properties
    }
}
