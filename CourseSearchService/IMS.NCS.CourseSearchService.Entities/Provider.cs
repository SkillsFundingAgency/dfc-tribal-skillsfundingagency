using System;

namespace IMS.NCS.CourseSearchService.Entities
{
    /// <summary>
    /// Provider Entity.
    /// </summary>
    public class Provider
    {
        public String AddressLine1 { get; set; }
        public String AddressLine2 { get; set; }
        public String County { get; set; }
        public String Email { get; set; }
        public String Fax { get; set; }
        public String Phone { get; set; }
        public String Postcode { get; set; }
        public String ProviderId { get; set; }
        public String ProviderName { get; set; }
        public String Town { get; set; }
        public String Ukprn { get; set; }
        public String Website { get; set; }
        public String Upin { get; set; }
        public Boolean TFPlusLoans { get; set; }
        public Boolean DFE1619Funded { get; set; }
        public Double? FEChoices_LearnerDestination { get; set; }
        public Double? FEChoices_LearnerSatisfaction { get; set; }
        public Double? FEChoices_EmployerSatisfaction { get; set; }
    }
}
