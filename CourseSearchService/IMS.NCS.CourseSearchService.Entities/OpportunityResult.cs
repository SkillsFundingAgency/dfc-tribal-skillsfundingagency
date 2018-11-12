using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.CourseSearchService.Entities
{
    /// <summary>
    /// OpportunityResult entity.
    /// </summary>
    public class OpportunityResult : Opportunity
    {
        #region Variables

        private long _courseId;

        #endregion

        #region Properties

        /// <summary>
        /// Course Id field.
        /// </summary>
        public long CourseId
        {
            get
            {
                return _courseId;
            }
            set
            {
                _courseId = value;
            }
        }

        #endregion
    }
}
