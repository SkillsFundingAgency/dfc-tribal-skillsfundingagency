using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

//using PagedList.Mvc;
//using PagedList;

namespace IMS.NCS.CourseSearchService.TestHarness.Models
{
    [Serializable]
    public class CourseSearchResults : List<CourseSearchResult>
    {
        public string Message { get; set; }
        public string RowsPerPage { get; set; }

        public List<LDCSCode> MatchingLDCSCodes { get; set; }
    }
}