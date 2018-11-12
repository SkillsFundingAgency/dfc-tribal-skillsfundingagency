using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.NCS.CourseSearchService.TestHarness.Models
{
    [Serializable]
    public class ProviderSearchResults : List<ProviderSearchResult>
    {
        public string Message { get; set; }
        public string RowsPerPage { get; set; }
    }
}