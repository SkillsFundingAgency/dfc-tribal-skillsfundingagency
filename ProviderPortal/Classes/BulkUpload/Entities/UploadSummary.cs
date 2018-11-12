using System.Collections.Generic;

namespace Tribal.SkillsFundingAgency.ProviderPortal.BulkUpload.Entities
{
    public class UploadSummary
    {
        public UploadSummary()
        {
            SummaryItems = new List<UploadSummaryExceptionItem>();
            ValidProviders = new Dictionary<int, bool>();
        }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public int ContentLength { get; set; }

        public string TargetFileUrl { get; set; }

        public string UserName { get; set; }

        public int ExistingVenueCount { get; set; }

        public int NewVenueCount { get; set; }

        public int InvalidVenueCount { get; set; }

        public int ExistingCourseCount { get; set; }

        public int NewCourseCount { get; set; }

        public int InvalidCourseCount { get; set; }

        public int ExistingOpportunityCount { get; set; }

        public int NewOpportunityCount { get; set; }

        public int InvalidOpportunityCount { get; set; }

        public Constants.BulkUploadStatus Status { get; set; }

        public bool PendingOrganisationUploadExists { get; set; }

        public Dictionary<int, bool> ValidProviders { get; set; }

        public List<UploadSummaryExceptionItem> SummaryItems { get; set; }
    }
}