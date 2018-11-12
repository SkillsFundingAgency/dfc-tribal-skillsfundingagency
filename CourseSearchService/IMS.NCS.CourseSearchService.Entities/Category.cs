using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.CourseSearchService.Entities
{
    /// <summary>
    /// Category Entity.
    /// </summary>
    public class Category
    {
        #region Variables

        private string _categoryCode;
        private int _courseCount;
        private string _description;
        private int _level;
        private string _parentCategoryCode;
        private string _searchable = "N";

        #endregion Variables

        #region Properties

        /// <summary>
        /// Category code field.
        /// </summary>
        public string CategoryCode
        {
            get
            {
                return _categoryCode;
            }
            set
            {
                _categoryCode = value;
            }
        }

        /// <summary>
        /// Course count field.
        /// </summary>
        public int CourseCount
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
        /// Category description field.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Category level field.
        /// </summary>
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        /// <summary>
        /// Parent category code field.
        /// </summary>
        public string ParentCategoryCode
        {
            get
            {
                return _parentCategoryCode;
            }
            set
            {
                _parentCategoryCode = value;
            }
        }

        /// <summary>
        /// Category is searchable field (takes values Y or N).
        /// </summary>
        public string Searchable
        {
            get
            {
                return _searchable;
            }
            set
            {
                _searchable = value;
            }
        }

        #endregion Properties

    }
}
