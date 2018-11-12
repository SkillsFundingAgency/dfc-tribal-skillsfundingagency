using System;

namespace IMS.NCS.CourseSearchService.Entities
{
    /// <summary>
    /// CourseListRequest entity.
    /// </summary>
    public class CourseListRequest
    {
        public String APIKey { get; set; }
        public string A10Codes { get; set; }
        public string AdultLRStatus { get; set; }
        public string AppClosedFlag { get; set; }
        public string AttendanceModes { get; set; }
        public string AttendancePatterns { get; set; }
        public float Distance { get; set; }
        public bool DistanceSpecified { get; set; }
        public string DFE1619Funded { get; set; }
        public string EarliestStartDate { get; set; }
        public string ERAppStatus { get; set; }
        public string ERTtgStatus { get; set; }
        public string FlexStartFlag { get; set; }
        public string IesFlag { get; set; }
        public string IlsFlag { get; set; }
        public string LdcsCategoryCode { get; set; }
        public string Location { get; set; }
        public string OtherFundingStatus { get; set; }
        public int ProviderId { get; set; }
        public string ProviderKeyword { get; set; }
        public string QualificationLevels { get; set; }
        public string QualificationTypes { get; set; }
        public string SflFlag { get; set; }
        public string StudyModes { get; set; }
        public string SubjectKeyword { get; set; }
        public string TqsFlag { get; set; }
        public string TtgFlag { get; set; }
        public long PageNumber { get; set; }
        public int RecordsPerPage { get; set; }
        public string SortBy { get; set; }
    }
}
