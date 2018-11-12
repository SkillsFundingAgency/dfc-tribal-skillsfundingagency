namespace Tribal.SkillsFundingAgency.ProviderPortal.BulkUpload.Entities
{
    public class UploadSummaryExceptionItem
    {
        public UploadSummaryExceptionItem(Constants.BulkUpload_Validation_ErrorType validationErrorType, int? providerId, string lineNumber, string columnName, string columnValue, string details, Constants.BulkUpload_SectionName sectionName)
        {
            ValidationErrorType = validationErrorType;
            ColumnName = columnName;
            ColumnValue = columnValue;
            Details = details;
            LineNumber = lineNumber;
            ProviderId = providerId.ToString();
            SectionName = sectionName;
        }

        public Constants.BulkUpload_Validation_ErrorType ValidationErrorType { get; set; }

        public Constants.BulkUpload_SectionName SectionName { get; set; }

        public string LineNumber { get; private set; }

        public string ColumnName { get; private set; }

        public string ColumnValue { get; private set; }

        public string Details { get; private set; }

        public string ProviderId { get; private set; }
    }
}