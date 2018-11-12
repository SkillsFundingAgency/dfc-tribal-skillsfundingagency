using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.NCS.CourseSearchService.TestHarness.Models
{
    [Serializable]
    public class LDCSCode
    {
        // LDCS properties
        public string Code { get; set; }
        public string Description { get; set; }
        public int CourseCount { get; set; }
    }
}