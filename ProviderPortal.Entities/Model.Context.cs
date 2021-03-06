﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tribal.SkillsFundingAgency.ProviderPortal.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ProviderPortalEntities : DbContext
    {
        public ProviderPortalEntities()
            : base("name=ProviderPortalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<A10FundingCode> A10FundingCode { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AttendancePattern> AttendancePatterns { get; set; }
        public virtual DbSet<AttendanceType> AttendanceTypes { get; set; }
        public virtual DbSet<ConfigurationSetting> ConfigurationSettings { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseInstanceStartDate> CourseInstanceStartDates { get; set; }
        public virtual DbSet<CourseLearnDirectClassification> CourseLearnDirectClassifications { get; set; }
        public virtual DbSet<DurationUnit> DurationUnits { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<EmailTemplateGroup> EmailTemplateGroups { get; set; }
        public virtual DbSet<GlobalEventComputer> GlobalEventComputers { get; set; }
        public virtual DbSet<GlobalEventLog> GlobalEventLogs { get; set; }
        public virtual DbSet<GlobalEventSource> GlobalEventSources { get; set; }
        public virtual DbSet<GlobalEventType> GlobalEventTypes { get; set; }
        public virtual DbSet<GlobalEventUser> GlobalEventUsers { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LanguageField> LanguageFields { get; set; }
        public virtual DbSet<LanguageKeyChild> LanguageKeyChilds { get; set; }
        public virtual DbSet<LanguageKeyGroup> LanguageKeyGroups { get; set; }
        public virtual DbSet<LanguageText> LanguageTexts { get; set; }
        public virtual DbSet<LearnDirectClassification> LearnDirectClassifications { get; set; }
        public virtual DbSet<LearningAim> LearningAims { get; set; }
        public virtual DbSet<LearningAimAwardOrg> LearningAimAwardOrgs { get; set; }
        public virtual DbSet<OrganisationType> OrganisationTypes { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Prison> Prisons { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<ProviderPortalUsageStatistic> ProviderPortalUsageStatistics { get; set; }
        public virtual DbSet<ProviderRegion> ProviderRegions { get; set; }
        public virtual DbSet<ProviderType> ProviderTypes { get; set; }
        public virtual DbSet<ProviderUserType> ProviderUserTypes { get; set; }
        public virtual DbSet<ProviderUserTypeInRole> ProviderUserTypeInRoles { get; set; }
        public virtual DbSet<QualificationLevel> QualificationLevels { get; set; }
        public virtual DbSet<QualificationType> QualificationTypes { get; set; }
        public virtual DbSet<RecordStatu> RecordStatus { get; set; }
        public virtual DbSet<StatisticAction> StatisticActions { get; set; }
        public virtual DbSet<StatisticType> StatisticTypes { get; set; }
        public virtual DbSet<StudyMode> StudyModes { get; set; }
        public virtual DbSet<UcasStudyModeMapping> UcasStudyModeMappings { get; set; }
        public virtual DbSet<Ukrlp> Ukrlps { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<VenueLocation> VenueLocations { get; set; }
        public virtual DbSet<AddressBase> AddressBases { get; set; }
        public virtual DbSet<GeoLocation> GeoLocations { get; set; }
        public virtual DbSet<OrganisationProvider> OrganisationProviders { get; set; }
        public virtual DbSet<Organisation> Organisations { get; set; }
        public virtual DbSet<CourseLanguage> CourseLanguages { get; set; }
        public virtual DbSet<QualificationTitle> QualificationTitles { get; set; }
        public virtual DbSet<CourseInstance> CourseInstances { get; set; }
        public virtual DbSet<QualityEmailStatuses> QualityEmailStatuses { get; set; }
        public virtual DbSet<BulkUploadErrorType> BulkUploadErrorTypes { get; set; }
        public virtual DbSet<BulkUploadStatu> BulkUploadStatus { get; set; }
        public virtual DbSet<QualificationTypeMap> QualificationTypeMaps { get; set; }
        public virtual DbSet<LearningAimValidity> LearningAimValidities { get; set; }
        public virtual DbSet<FEChoice> FEChoices { get; set; }
        public virtual DbSet<OrganisationQualityScore> OrganisationQualityScores { get; set; }
        public virtual DbSet<DfEEstablishmentPhase> DfEEstablishmentPhases { get; set; }
        public virtual DbSet<DfEEstablishmentStatu> DfEEstablishmentStatus { get; set; }
        public virtual DbSet<DfEEstablishmentType> DfEEstablishmentTypes { get; set; }
        public virtual DbSet<DfELocalAuthority> DfELocalAuthorities { get; set; }
        public virtual DbSet<DfEProviderStatu> DfEProviderStatus { get; set; }
        public virtual DbSet<DfEProviderType> DfEProviderTypes { get; set; }
        public virtual DbSet<DfERegion> DfERegions { get; set; }
        public virtual DbSet<DfEWsProviderStatu> DfEWsProviderStatus { get; set; }
        public virtual DbSet<OpenDataDownload> OpenDataDownloads { get; set; }
        public virtual DbSet<PublicAPIUser> PublicAPIUsers { get; set; }
        public virtual DbSet<BulkUpload> BulkUploads { get; set; }
        public virtual DbSet<BulkUploadStatusHistory> BulkUploadStatusHistories { get; set; }
        public virtual DbSet<BulkUploadProvider> BulkUploadProviders { get; set; }
        public virtual DbSet<BulkUploadExceptionItem> BulkUploadExceptionItems { get; set; }
        public virtual DbSet<ProgressMessage> ProgressMessages { get; set; }
        public virtual DbSet<v_BulkUploadCurrentStatus> v_BulkUploadCurrentStatus { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<QualityScore> QualityScores { get; set; }
        public virtual DbSet<QualityEmailLog> QualityEmailLogs { get; set; }
        public virtual DbSet<MetadataUpload> MetadataUploads { get; set; }
        public virtual DbSet<MetadataUploadType> MetadataUploadTypes { get; set; }
        public virtual DbSet<UCAS_CourseEntry> UCAS_CourseEntries { get; set; }
        public virtual DbSet<UCAS_Course> UCAS_Courses { get; set; }
        public virtual DbSet<UCAS_CoursesIndex> UCAS_CoursesIndexes { get; set; }
        public virtual DbSet<UCAS_Currency> UCAS_Currencies { get; set; }
        public virtual DbSet<UCAS_Deletion> UCAS_Deletions { get; set; }
        public virtual DbSet<UCAS_Fee> UCAS_Fees { get; set; }
        public virtual DbSet<UCAS_Org> UCAS_Orgs { get; set; }
        public virtual DbSet<UCAS_PlaceOfStudy> UCAS_PlacesOfStudy { get; set; }
        public virtual DbSet<UCAS_Start> UCAS_Starts { get; set; }
        public virtual DbSet<UCAS_StartsIndex> UCAS_StartsIndexes { get; set; }
        public virtual DbSet<UCAS_Town> UCAS_Towns { get; set; }
        public virtual DbSet<UCAS_FeeYear> UCAS_FeeYears { get; set; }
        public virtual DbSet<UserProvisionHistory> UserProvisionHistories { get; set; }
        public virtual DbSet<SectorSubjectAreaTier1> SectorSubjectAreaTier1 { get; set; }
        public virtual DbSet<SectorSubjectAreaTier2> SectorSubjectAreaTier2 { get; set; }
        public virtual DbSet<Standard> Standards { get; set; }
        public virtual DbSet<StandardSectorCode> StandardSectorCodes { get; set; }
        public virtual DbSet<ProgType> ProgTypes { get; set; }
        public virtual DbSet<Apprenticeship> Apprenticeships { get; set; }
        public virtual DbSet<DeliveryMode> DeliveryModes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<ApprenticeshipLocation> ApprenticeshipLocations { get; set; }
        public virtual DbSet<Framework> Frameworks { get; set; }
        public virtual DbSet<UCAS_Qualification> UCAS_Qualifications { get; set; }
        public virtual DbSet<UCAS_PG_Course> UCAS_PG_Courses { get; set; }
        public virtual DbSet<UCAS_PG_CourseOption> UCAS_PG_CourseOptions { get; set; }
        public virtual DbSet<UCAS_PG_CourseOptionFee> UCAS_PG_CourseOptionFees { get; set; }
        public virtual DbSet<UCAS_PG_Location> UCAS_PG_Locations { get; set; }
        public virtual DbSet<UCAS_PG_Provider> UCAS_PG_Providers { get; set; }
        public virtual DbSet<QAComplianceFailureReason> QAComplianceFailureReasons { get; set; }
        public virtual DbSet<QAStyleFailureReason> QAStyleFailureReasons { get; set; }
        public virtual DbSet<ApprenticeshipQACompliance> ApprenticeshipQACompliances { get; set; }
        public virtual DbSet<ApprenticeshipQAStyle> ApprenticeshipQAStyles { get; set; }
        public virtual DbSet<ProviderQACompliance> ProviderQACompliances { get; set; }
        public virtual DbSet<ProviderQAStyle> ProviderQAStyles { get; set; }
        public virtual DbSet<AutomatedTask> AutomatedTasks { get; set; }
        public virtual DbSet<Provider_AllCoursesOKConfirmations> Provider_AllCoursesOKConfirmations { get; set; }
        public virtual DbSet<RoATPProviderType> RoATPProviderTypes { get; set; }
        public virtual DbSet<ProviderUnableToComplete> ProviderUnableToCompletes { get; set; }
        public virtual DbSet<UnableToCompleteFailureReason> UnableToCompleteFailureReasons { get; set; }
        public virtual DbSet<ImportBatch> ImportBatches { get; set; }
        public virtual DbSet<ImportBatchProvider> ImportBatchProviders { get; set; }
        public virtual DbSet<SearchPhrase> SearchPhrases { get; set; }
        public virtual DbSet<ProviderTASRefresh> ProviderTASRefreshes { get; set; }
    
        public virtual ObjectResult<up_ReportProviderCourses_Result> up_ReportProviderCourses(Nullable<int> providerId)
        {
            var providerIdParameter = providerId.HasValue ?
                new ObjectParameter("ProviderId", providerId) :
                new ObjectParameter("ProviderId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_ReportProviderCourses_Result>("up_ReportProviderCourses", providerIdParameter);
        }
    
        public virtual ObjectResult<up_ReportProviderOpportunities_Result> up_ReportProviderOpportunities(Nullable<int> providerId)
        {
            var providerIdParameter = providerId.HasValue ?
                new ObjectParameter("ProviderId", providerId) :
                new ObjectParameter("ProviderId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_ReportProviderOpportunities_Result>("up_ReportProviderOpportunities", providerIdParameter);
        }
    
        public virtual ObjectResult<up_ReportProviderVenues_Result> up_ReportProviderVenues(Nullable<int> providerId)
        {
            var providerIdParameter = providerId.HasValue ?
                new ObjectParameter("ProviderId", providerId) :
                new ObjectParameter("ProviderId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_ReportProviderVenues_Result>("up_ReportProviderVenues", providerIdParameter);
        }
    
        public virtual ObjectResult<up_ReportProviderDashboard_Result> up_ReportProviderDashboard(Nullable<int> providerId)
        {
            var providerIdParameter = providerId.HasValue ?
                new ObjectParameter("ProviderId", providerId) :
                new ObjectParameter("ProviderId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_ReportProviderDashboard_Result>("up_ReportProviderDashboard", providerIdParameter);
        }
    
        public virtual int up_ProviderUpdateQualityScore(Nullable<int> providerId, Nullable<bool> force)
        {
            var providerIdParameter = providerId.HasValue ?
                new ObjectParameter("ProviderId", providerId) :
                new ObjectParameter("ProviderId", typeof(int));
    
            var forceParameter = force.HasValue ?
                new ObjectParameter("Force", force) :
                new ObjectParameter("Force", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_ProviderUpdateQualityScore", providerIdParameter, forceParameter);
        }
    
        public virtual ObjectResult<up_ReportOrganisationTrafficLight_Result> up_ReportOrganisationTrafficLight(Nullable<int> organisationId)
        {
            var organisationIdParameter = organisationId.HasValue ?
                new ObjectParameter("OrganisationId", organisationId) :
                new ObjectParameter("OrganisationId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_ReportOrganisationTrafficLight_Result>("up_ReportOrganisationTrafficLight", organisationIdParameter);
        }
    
        public virtual int up_ProviderUpdateAllQualityScores(Nullable<bool> force)
        {
            var forceParameter = force.HasValue ?
                new ObjectParameter("Force", force) :
                new ObjectParameter("Force", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_ProviderUpdateAllQualityScores", forceParameter);
        }
    
        public virtual ObjectResult<up_ReportAdminReport_Result> up_ReportAdminReport(Nullable<bool> includeProviders, Nullable<bool> includeOrganisations, Nullable<bool> contractingBodiesOnly, Nullable<bool> sFAFunded, Nullable<bool> dFEFunded)
        {
            var includeProvidersParameter = includeProviders.HasValue ?
                new ObjectParameter("IncludeProviders", includeProviders) :
                new ObjectParameter("IncludeProviders", typeof(bool));
    
            var includeOrganisationsParameter = includeOrganisations.HasValue ?
                new ObjectParameter("IncludeOrganisations", includeOrganisations) :
                new ObjectParameter("IncludeOrganisations", typeof(bool));
    
            var contractingBodiesOnlyParameter = contractingBodiesOnly.HasValue ?
                new ObjectParameter("ContractingBodiesOnly", contractingBodiesOnly) :
                new ObjectParameter("ContractingBodiesOnly", typeof(bool));
    
            var sFAFundedParameter = sFAFunded.HasValue ?
                new ObjectParameter("SFAFunded", sFAFunded) :
                new ObjectParameter("SFAFunded", typeof(bool));
    
            var dFEFundedParameter = dFEFunded.HasValue ?
                new ObjectParameter("DFEFunded", dFEFunded) :
                new ObjectParameter("DFEFunded", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_ReportAdminReport_Result>("up_ReportAdminReport", includeProvidersParameter, includeOrganisationsParameter, contractingBodiesOnlyParameter, sFAFundedParameter, dFEFundedParameter);
        }
    
        public virtual ObjectResult<up_CourseBrowseListForCsvExport_Result> up_CourseBrowseListForCsvExport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_CourseBrowseListForCsvExport_Result>("up_CourseBrowseListForCsvExport");
        }
    
        public virtual ObjectResult<up_CourseInstanceA10CodesForCsvExport_Result> up_CourseInstanceA10CodesForCsvExport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_CourseInstanceA10CodesForCsvExport_Result>("up_CourseInstanceA10CodesForCsvExport");
        }
    
        public virtual ObjectResult<up_CourseInstanceStartDatesListForCsvExport_Result> up_CourseInstanceStartDatesListForCsvExport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_CourseInstanceStartDatesListForCsvExport_Result>("up_CourseInstanceStartDatesListForCsvExport");
        }
    
        public virtual ObjectResult<up_CourseSearchTextForCsvExport_Result> up_CourseSearchTextForCsvExport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_CourseSearchTextForCsvExport_Result>("up_CourseSearchTextForCsvExport");
        }
    
        public virtual ObjectResult<up_VenueListForCsvExport_Result> up_VenueListForCsvExport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_VenueListForCsvExport_Result>("up_VenueListForCsvExport");
        }
    
        public virtual int up_OrganisationUpdateQualityScore(Nullable<int> organisationId)
        {
            var organisationIdParameter = organisationId.HasValue ?
                new ObjectParameter("OrganisationId", organisationId) :
                new ObjectParameter("OrganisationId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_OrganisationUpdateQualityScore", organisationIdParameter);
        }
    
        public virtual ObjectResult<up_ProviderListForCsvExport_Result> up_ProviderListForCsvExport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_ProviderListForCsvExport_Result>("up_ProviderListForCsvExport");
        }
    
        public virtual ObjectResult<up_ProviderSearchListForCsvExport_Result> up_ProviderSearchListForCsvExport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_ProviderSearchListForCsvExport_Result>("up_ProviderSearchListForCsvExport");
        }
    
        public virtual ObjectResult<up_CourseInstanceListForCsvExport_Result> up_CourseInstanceListForCsvExport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_CourseInstanceListForCsvExport_Result>("up_CourseInstanceListForCsvExport");
        }
    
        public virtual int up_LanguageDeleteAllFields(Nullable<bool> deleteTableLanguageFieldNames, Nullable<bool> deleteNormalLanguageFieldNames)
        {
            var deleteTableLanguageFieldNamesParameter = deleteTableLanguageFieldNames.HasValue ?
                new ObjectParameter("DeleteTableLanguageFieldNames", deleteTableLanguageFieldNames) :
                new ObjectParameter("DeleteTableLanguageFieldNames", typeof(bool));
    
            var deleteNormalLanguageFieldNamesParameter = deleteNormalLanguageFieldNames.HasValue ?
                new ObjectParameter("DeleteNormalLanguageFieldNames", deleteNormalLanguageFieldNames) :
                new ObjectParameter("DeleteNormalLanguageFieldNames", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_LanguageDeleteAllFields", deleteTableLanguageFieldNamesParameter, deleteNormalLanguageFieldNamesParameter);
        }
    
        public virtual int up_LanguageFieldAddSetDefaultByFieldName(string fieldName, string defaultText, Nullable<bool> alwaysUpdateDefaultText)
        {
            var fieldNameParameter = fieldName != null ?
                new ObjectParameter("FieldName", fieldName) :
                new ObjectParameter("FieldName", typeof(string));
    
            var defaultTextParameter = defaultText != null ?
                new ObjectParameter("DefaultText", defaultText) :
                new ObjectParameter("DefaultText", typeof(string));
    
            var alwaysUpdateDefaultTextParameter = alwaysUpdateDefaultText.HasValue ?
                new ObjectParameter("AlwaysUpdateDefaultText", alwaysUpdateDefaultText) :
                new ObjectParameter("AlwaysUpdateDefaultText", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_LanguageFieldAddSetDefaultByFieldName", fieldNameParameter, defaultTextParameter, alwaysUpdateDefaultTextParameter);
        }
    
        public virtual int up_LanguageFieldDelete(Nullable<int> languageFieldId)
        {
            var languageFieldIdParameter = languageFieldId.HasValue ?
                new ObjectParameter("LanguageFieldId", languageFieldId) :
                new ObjectParameter("LanguageFieldId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_LanguageFieldDelete", languageFieldIdParameter);
        }
    
        public virtual ObjectResult<up_LanguageFieldListTablesWithLanguageFields_Result> up_LanguageFieldListTablesWithLanguageFields()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_LanguageFieldListTablesWithLanguageFields_Result>("up_LanguageFieldListTablesWithLanguageFields");
        }
    
        public virtual int up_LanguageFieldSetupTables(Nullable<bool> recreateAllTableKeys)
        {
            var recreateAllTableKeysParameter = recreateAllTableKeys.HasValue ?
                new ObjectParameter("RecreateAllTableKeys", recreateAllTableKeys) :
                new ObjectParameter("RecreateAllTableKeys", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_LanguageFieldSetupTables", recreateAllTableKeysParameter);
        }
    
        public virtual ObjectResult<string> up_LanguageFullTextListStopWords(Nullable<int> languageId)
        {
            var languageIdParameter = languageId.HasValue ?
                new ObjectParameter("LanguageId", languageId) :
                new ObjectParameter("LanguageId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("up_LanguageFullTextListStopWords", languageIdParameter);
        }
    
        public virtual ObjectResult<up_LanguageKeyChildList_Result> up_LanguageKeyChildList(Nullable<int> languageKeyGroupId)
        {
            var languageKeyGroupIdParameter = languageKeyGroupId.HasValue ?
                new ObjectParameter("LanguageKeyGroupId", languageKeyGroupId) :
                new ObjectParameter("LanguageKeyGroupId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_LanguageKeyChildList_Result>("up_LanguageKeyChildList", languageKeyGroupIdParameter);
        }
    
        public virtual ObjectResult<up_LanguageKeyGroupList_Result> up_LanguageKeyGroupList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_LanguageKeyGroupList_Result>("up_LanguageKeyGroupList");
        }
    
        public virtual ObjectResult<up_LanguageList_Result> up_LanguageList(Nullable<int> languageId, Nullable<int> lookupLanguageId)
        {
            var languageIdParameter = languageId.HasValue ?
                new ObjectParameter("LanguageId", languageId) :
                new ObjectParameter("LanguageId", typeof(int));
    
            var lookupLanguageIdParameter = lookupLanguageId.HasValue ?
                new ObjectParameter("LookupLanguageId", lookupLanguageId) :
                new ObjectParameter("LookupLanguageId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_LanguageList_Result>("up_LanguageList", languageIdParameter, lookupLanguageIdParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> up_LanguageListDefaultLanguageId()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("up_LanguageListDefaultLanguageId");
        }
    
        public virtual ObjectResult<up_LanguageTextListByKeyGroupId_Result> up_LanguageTextListByKeyGroupId(Nullable<int> languageId, Nullable<int> languageKeyGroupId, Nullable<int> languageKeyChildId, string languageText, Nullable<int> sortColumn)
        {
            var languageIdParameter = languageId.HasValue ?
                new ObjectParameter("LanguageId", languageId) :
                new ObjectParameter("LanguageId", typeof(int));
    
            var languageKeyGroupIdParameter = languageKeyGroupId.HasValue ?
                new ObjectParameter("LanguageKeyGroupId", languageKeyGroupId) :
                new ObjectParameter("LanguageKeyGroupId", typeof(int));
    
            var languageKeyChildIdParameter = languageKeyChildId.HasValue ?
                new ObjectParameter("LanguageKeyChildId", languageKeyChildId) :
                new ObjectParameter("LanguageKeyChildId", typeof(int));
    
            var languageTextParameter = languageText != null ?
                new ObjectParameter("LanguageText", languageText) :
                new ObjectParameter("LanguageText", typeof(string));
    
            var sortColumnParameter = sortColumn.HasValue ?
                new ObjectParameter("SortColumn", sortColumn) :
                new ObjectParameter("SortColumn", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_LanguageTextListByKeyGroupId_Result>("up_LanguageTextListByKeyGroupId", languageIdParameter, languageKeyGroupIdParameter, languageKeyChildIdParameter, languageTextParameter, sortColumnParameter);
        }
    
        public virtual ObjectResult<up_LanguageTextListByLanguageId_Result> up_LanguageTextListByLanguageId(Nullable<int> languageId, Nullable<int> languageFieldId)
        {
            var languageIdParameter = languageId.HasValue ?
                new ObjectParameter("LanguageId", languageId) :
                new ObjectParameter("LanguageId", typeof(int));
    
            var languageFieldIdParameter = languageFieldId.HasValue ?
                new ObjectParameter("LanguageFieldId", languageFieldId) :
                new ObjectParameter("LanguageFieldId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_LanguageTextListByLanguageId_Result>("up_LanguageTextListByLanguageId", languageIdParameter, languageFieldIdParameter);
        }
    
        public virtual int up_LanguageTextSetByQualifiedFieldName(Nullable<int> languageId, string fieldName, string defaultLanguageText, string languageText)
        {
            var languageIdParameter = languageId.HasValue ?
                new ObjectParameter("LanguageId", languageId) :
                new ObjectParameter("LanguageId", typeof(int));
    
            var fieldNameParameter = fieldName != null ?
                new ObjectParameter("FieldName", fieldName) :
                new ObjectParameter("FieldName", typeof(string));
    
            var defaultLanguageTextParameter = defaultLanguageText != null ?
                new ObjectParameter("DefaultLanguageText", defaultLanguageText) :
                new ObjectParameter("DefaultLanguageText", typeof(string));
    
            var languageTextParameter = languageText != null ?
                new ObjectParameter("LanguageText", languageText) :
                new ObjectParameter("LanguageText", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_LanguageTextSetByQualifiedFieldName", languageIdParameter, fieldNameParameter, defaultLanguageTextParameter, languageTextParameter);
        }
    
        public virtual ObjectResult<up_ReportSalesForceAccounts_Result> up_ReportSalesForceAccounts()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_ReportSalesForceAccounts_Result>("up_ReportSalesForceAccounts");
        }
    
        public virtual int up_QualityEmailLogDeleteDuplicateRows()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_QualityEmailLogDeleteDuplicateRows");
        }
    
        public virtual ObjectResult<up_BulkUploadValidationQueueNext_Result> up_BulkUploadValidationQueueNext(Nullable<int> largeFileSize, Nullable<bool> largeFilesAllowed)
        {
            var largeFileSizeParameter = largeFileSize.HasValue ?
                new ObjectParameter("LargeFileSize", largeFileSize) :
                new ObjectParameter("LargeFileSize", typeof(int));
    
            var largeFilesAllowedParameter = largeFilesAllowed.HasValue ?
                new ObjectParameter("LargeFilesAllowed", largeFilesAllowed) :
                new ObjectParameter("LargeFilesAllowed", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_BulkUploadValidationQueueNext_Result>("up_BulkUploadValidationQueueNext", largeFileSizeParameter, largeFilesAllowedParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> up_BulkUploadPendingOrganisationUploadForProvider(Nullable<int> providerId)
        {
            var providerIdParameter = providerId.HasValue ?
                new ObjectParameter("ProviderId", providerId) :
                new ObjectParameter("ProviderId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("up_BulkUploadPendingOrganisationUploadForProvider", providerIdParameter);
        }
    
        public virtual ObjectResult<up_BulkUploadConfirmedQueueNext_Result> up_BulkUploadConfirmedQueueNext()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_BulkUploadConfirmedQueueNext_Result>("up_BulkUploadConfirmedQueueNext");
        }
    
        public virtual ObjectResult<up_CourseListForCsvExport_Result> up_CourseListForCsvExport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<up_CourseListForCsvExport_Result>("up_CourseListForCsvExport");
        }
    
        public virtual int up_BulkUploadProviderClearData(Nullable<int> providerId, Nullable<bool> clearCourseData, Nullable<bool> clearApprenticeshipData)
        {
            var providerIdParameter = providerId.HasValue ?
                new ObjectParameter("ProviderId", providerId) :
                new ObjectParameter("ProviderId", typeof(int));
    
            var clearCourseDataParameter = clearCourseData.HasValue ?
                new ObjectParameter("ClearCourseData", clearCourseData) :
                new ObjectParameter("ClearCourseData", typeof(bool));
    
            var clearApprenticeshipDataParameter = clearApprenticeshipData.HasValue ?
                new ObjectParameter("ClearApprenticeshipData", clearApprenticeshipData) :
                new ObjectParameter("ClearApprenticeshipData", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_BulkUploadProviderClearData", providerIdParameter, clearCourseDataParameter, clearApprenticeshipDataParameter);
        }
    
        public virtual int up_UCAS_PrepareForImport(Nullable<bool> incremental)
        {
            var incrementalParameter = incremental.HasValue ?
                new ObjectParameter("Incremental", incremental) :
                new ObjectParameter("Incremental", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_UCAS_PrepareForImport", incrementalParameter);
        }
    
        public virtual int up_UCAS_HandleDeletions()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_UCAS_HandleDeletions");
        }
    
        public virtual int up_UCAS_PrepareForImport1()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_UCAS_PrepareForImport1");
        }
    
        public virtual int up_CanRunAutomatedTask(string taskName)
        {
            var taskNameParameter = taskName != null ?
                new ObjectParameter("TaskName", taskName) :
                new ObjectParameter("TaskName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_CanRunAutomatedTask", taskNameParameter);
        }
    }
}
