using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.BulkUpload.Helpers;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.BulkUpload.Convertors
{
    public class EntityToCsvConvertor
    {
        private readonly List<Provider> _providers;
        private readonly UserContext.UserContextInfo _userContext;
        private readonly Constants.BulkUpload_DataType _dataType;

        public EntityToCsvConvertor(List<Provider> providers, UserContext.UserContextInfo userContext, Constants.BulkUpload_DataType dataType)
        {
            _providers = providers;
            _userContext = userContext;
            _dataType = dataType;
        }

        public byte[] ToCsv()
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (TextWriter responseString = new StreamWriter(stream, new UTF8Encoding(true)))
                    {
                        using (var csvWriter = new CsvWriter(responseString))
                        {
                            foreach (var provider in _providers)
                            {
                                TransformProviderSection(csvWriter, provider);

                                if (_dataType == Constants.BulkUpload_DataType.CourseData)
                                {
                                    TransformVenueSection(csvWriter, provider);
                                    TransformCourseSection(csvWriter, provider);
                                    TransformOpportunitySection(csvWriter, provider);
                                }
                                else //Apprenticeship data
                                {
                                    TransformLocationSection(csvWriter, provider);
                                    TransformApprenticeshipSection(csvWriter, provider);
                                    TransformDeliveryLocationSection(csvWriter, provider);
                                }
                            }
                        }
                    }
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                AppGlobal.Log.WriteLog(string.Concat(ex.Message, "-", ex.StackTrace));
                throw;
            }
        }

        private void TransformProviderSection(CsvWriter csvWriter, Provider provider)
        {
            if (_userContext.IsOrganisation())
            {
                csvWriter.WriteField<string>("ProviderId");
                csvWriter.WriteField<string>(provider.ProviderId.ToString());
                csvWriter.NextRecord();
            }

            csvWriter.WriteField<string>("Providers");
            csvWriter.NextRecord();

            csvWriter.WriteField<string>("PROVIDER_NAME");
            csvWriter.WriteField<string>("PROVIDER_ALIAS");
            csvWriter.WriteField<string>("ADMIN_ADDRESS_1");
            csvWriter.WriteField<string>("ADMIN_ADDRESS_2");
            csvWriter.WriteField<string>("ADMIN_TOWN");
            csvWriter.WriteField<string>("ADMIN_COUNTY");
            csvWriter.WriteField<string>("ADMIN_POSTCODE");
            csvWriter.WriteField<string>("ADMIN_EMAIL");
            csvWriter.WriteField<string>("ADMIN_WEBSITE");
            csvWriter.WriteField<string>("ADMIN_PHONE");
            csvWriter.WriteField<string>("ADMIN_FAX");

            //The Marketing Information field appears in the Apprentceships file but not the Courses file
            if (_dataType == Constants.BulkUpload_DataType.ApprenticeshipData)
            {
                csvWriter.WriteField<string>("GENERIC_APPRENTICESHIP_INFO*");
            }

            csvWriter.NextRecord();

            csvWriter.WriteField<string>(provider.ProviderName);
            csvWriter.WriteField<string>(provider.ProviderNameAlias);
            csvWriter.WriteField<string>(provider.Address == null ? string.Empty : provider.Address.AddressLine1);
            csvWriter.WriteField<string>(provider.Address == null ? string.Empty : provider.Address.AddressLine2);
            csvWriter.WriteField<string>(provider.Address == null ? string.Empty : provider.Address.Town);
            csvWriter.WriteField<string>(provider.Address == null ? string.Empty : provider.Address.County);
            csvWriter.WriteField<string>(provider.Address == null ? string.Empty : provider.Address.Postcode);
            csvWriter.WriteField<string>(provider.Email);
            csvWriter.WriteField<string>(provider.Website);
            csvWriter.WriteField<string>(provider.Telephone);
            csvWriter.WriteField<string>(provider.Fax);

            //The Marketing Information field appears in the Apprentceships file but not the Courses file
            if (_dataType == Constants.BulkUpload_DataType.ApprenticeshipData)
            {
                csvWriter.WriteField<string>(provider.MarketingInformation);
            }

            csvWriter.NextRecord();
        }

        private static void TransformVenueSection(CsvWriter csvWriter, Provider provider)
        {
            //int venueRowId = 1;
            csvWriter.WriteField<string>("Venues");
            csvWriter.NextRecord();

            csvWriter.WriteField<string>("VENUE_ID*");
            csvWriter.WriteField<string>("PROVIDER_VENUE_ID");
            csvWriter.WriteField<string>("VENUE_NAME*");
            csvWriter.WriteField<string>("ADDRESS_1*");
            csvWriter.WriteField<string>("ADDRESS_2");
            csvWriter.WriteField<string>("TOWN*");
            csvWriter.WriteField<string>("COUNTY");
            csvWriter.WriteField<string>("POSTCODE*");
            csvWriter.WriteField<string>("EMAIL");
            csvWriter.WriteField<string>("WEBSITE");
            csvWriter.WriteField<string>("PHONE");
            csvWriter.WriteField<string>("FAX");
            csvWriter.WriteField<string>("FACILITIES");
            csvWriter.NextRecord();

            foreach (var venue in provider.Venues.Where(v => v.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))
            {
                csvWriter.WriteField<string>(CommonHelper.GetSafeString(venue.VenueId));
                //csvWriter.WriteField<string>(GetSafeString(venueRowId++));
                csvWriter.WriteField<string>(venue.ProviderOwnVenueRef);
                csvWriter.WriteField<string>(venue.VenueName);
                csvWriter.WriteField<string>(venue.Address == null ? string.Empty : venue.Address.AddressLine1);
                csvWriter.WriteField<string>(venue.Address == null ? string.Empty : venue.Address.AddressLine2);
                csvWriter.WriteField<string>(venue.Address == null ? string.Empty : venue.Address.Town);
                csvWriter.WriteField<string>(venue.Address == null ? string.Empty : venue.Address.County);
                csvWriter.WriteField<string>(venue.Address == null ? string.Empty : venue.Address.Postcode);
                csvWriter.WriteField<string>(venue.Email);
                csvWriter.WriteField<string>(venue.Website);
                csvWriter.WriteField<string>(venue.Telephone);
                csvWriter.WriteField<string>(venue.Fax);
                csvWriter.WriteField<string>(venue.Facilities);

                csvWriter.NextRecord();
            }
        }

        private static void TransformCourseSection(CsvWriter csvWriter, Provider provider)
        {
            csvWriter.WriteField<string>("Courses");
            csvWriter.NextRecord();

            csvWriter.WriteField<string>("COURSE_ID*");
            csvWriter.WriteField<string>("LAD_ID");
            csvWriter.WriteField<string>("PROVIDER_COURSE_TITLE*");
            csvWriter.WriteField<string>("SUMMARY*");
            csvWriter.WriteField<string>("PROVIDER_COURSE_ID");
            csvWriter.WriteField<string>("URL*");
            csvWriter.WriteField<string>("BOOKING_URL");
            csvWriter.WriteField<string>("ENTRY_REQUIREMENTS*");
            csvWriter.WriteField<string>("ASSESSMENT_METHOD");
            csvWriter.WriteField<string>("EQUIPMENT_REQUIRED");
            csvWriter.WriteField<string>("QUALIFICATION_TYPE*");
            csvWriter.WriteField<string>("QUALIFICATION_TITLE");
            csvWriter.WriteField<string>("AWARDING_ORG_NAME");
            csvWriter.WriteField<string>("QUALIFICATION_LEVEL");
            csvWriter.WriteField<string>("LDCS1");
            csvWriter.WriteField<string>("LDCS2");
            csvWriter.WriteField<string>("LDCS3");
            csvWriter.WriteField<string>("LDCS4");
            csvWriter.WriteField<string>("LDCS5");
            csvWriter.WriteField<string>("UCAS_TARIFF");
            csvWriter.NextRecord();

            //int courseRowId = 1;
            foreach (var course in provider.Courses.Where(c => c.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))
            {
                csvWriter.WriteField<string>(CommonHelper.GetSafeString(course.CourseId));
                //csvWriter.WriteField<string>(courseRowId++);
                csvWriter.WriteField<string>(CommonHelper.GetSafeString(course.LearningAimRefId));
                csvWriter.WriteField<string>(course.CourseTitle);
                csvWriter.WriteField<string>(course.CourseSummary);
                csvWriter.WriteField<string>(course.ProviderOwnCourseRef);
                csvWriter.WriteField<string>(course.Url);
                csvWriter.WriteField<string>(course.BookingUrl);
                csvWriter.WriteField<string>(course.EntryRequirements);
                csvWriter.WriteField<string>(course.AssessmentMethod);
                csvWriter.WriteField<string>(course.EquipmentRequired);
                csvWriter.WriteField<string>(course.QualificationType == null ? string.Empty : course.QualificationType.BulkUploadRef);
                //csvWriter.WriteField<string>(CommonHelper.GetSafeString(course.QualificationType.BulkUploadRef));
                csvWriter.WriteField<string>(CommonHelper.GetSafeString(course.WhenNoLarQualificationTitle));
                csvWriter.WriteField<string>(course.AwardingOrganisationName);
                csvWriter.WriteField<string>(course.QualificationLevel == null ? string.Empty : course.QualificationLevel.BulkUploadRef);
                csvWriter.WriteField<string>(GetSubjectClassification(course.CourseLearnDirectClassifications, 1));
                csvWriter.WriteField<string>(GetSubjectClassification(course.CourseLearnDirectClassifications, 2));
                csvWriter.WriteField<string>(GetSubjectClassification(course.CourseLearnDirectClassifications, 3));
                csvWriter.WriteField<string>(GetSubjectClassification(course.CourseLearnDirectClassifications, 4));
                csvWriter.WriteField<string>(GetSubjectClassification(course.CourseLearnDirectClassifications, 5));
                csvWriter.WriteField<string>(CommonHelper.GetSafeString(course.UcasTariffPoints));

                csvWriter.NextRecord();
            }
        }

        private void TransformOpportunitySection(CsvWriter csvWriter, Provider provider)
        {
            csvWriter.WriteField<string>("Opportunities");
            csvWriter.NextRecord();

            csvWriter.WriteField<string>("COURSE_ID*");
            csvWriter.WriteField<string>("VENUE_ID/REGION_NAME");
            csvWriter.WriteField<string>("PROVIDER_OPPORTUNITY_ID*");
            csvWriter.WriteField<string>("STUDY_MODE*");
            csvWriter.WriteField<string>("ATTENDANCE_MODE*");
            csvWriter.WriteField<string>("ATTENDANCE_PATTERN*");
            csvWriter.WriteField<string>("DURATION*");
            csvWriter.WriteField<string>("DURATION_UNITS*");
            csvWriter.WriteField<string>("DURATION_DESCRIPTION*");
            csvWriter.WriteField<string>("START_DATE*");
            csvWriter.WriteField<string>("END_DATE*");
            csvWriter.WriteField<string>("START_DATE_DESCRIPTION*");
            csvWriter.WriteField<string>("TIMETABLE");
            csvWriter.WriteField<string>("PRICE*");
            csvWriter.WriteField<string>("PRICE_DESCRIPTION*");
            csvWriter.WriteField<string>("LANGUAGE_OF_INSTRUCTION");
            csvWriter.WriteField<string>("LANGUAGE_OF_ASSESSMENT");
            csvWriter.WriteField<string>("PLACES_AVAILABLE");
            csvWriter.WriteField<string>("APPLY_FROM");
            csvWriter.WriteField<string>("APPLY_UNTIL");
            csvWriter.WriteField<string>("APPLY_UNTIL_DESC");
            csvWriter.WriteField<string>("APPLY_THROUGHOUT_YEAR");
            csvWriter.WriteField<string>("ENQUIRE_TO");
            csvWriter.WriteField<string>("APPLY_TO");
            csvWriter.WriteField<string>("URL");
            csvWriter.WriteField<string>("A10*");
            csvWriter.WriteField<string>("OFFERED_BY");
            csvWriter.WriteField<string>("DISPLAY_NAME");
            csvWriter.WriteField<string>("BOTH_SEARCHABLE");
            csvWriter.NextRecord();

            //int courseRowId = 1;
            //int venueRowId = 1;
            foreach (var course in provider.Courses.Where(c => c.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))
            {
                foreach (var instance in course.CourseInstances.Where(c => c.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))
                {
                    if (instance.CourseInstanceStartDates.Count().Equals(0))
                    {
                        this.WriteOpportunityRow(csvWriter, provider, instance, String.Empty);
                    }
                    else
                    {
                        foreach (var startDate in instance.CourseInstanceStartDates)
                        {
                            this.WriteOpportunityRow(csvWriter, provider, instance, CommonHelper.GetSafeDateString(startDate.StartDate));
                        }
                    }

                    csvWriter.NextRecord();
                }
            }
        }

        private static void TransformLocationSection(CsvWriter csvWriter, Provider provider)
        {
            //int venueRowId = 1;
            csvWriter.WriteField<string>("Locations");
            csvWriter.NextRecord();

            csvWriter.WriteField<string>("LOCATION_ID*");
            csvWriter.WriteField<string>("PROVIDER_LOCATION_ID");
            csvWriter.WriteField<string>("LOCATION_NAME*");
            csvWriter.WriteField<string>("ADDRESS_1");
            csvWriter.WriteField<string>("ADDRESS_2");
            csvWriter.WriteField<string>("TOWN");
            csvWriter.WriteField<string>("COUNTY");
            csvWriter.WriteField<string>("POSTCODE*");
            csvWriter.WriteField<string>("EMAIL");
            csvWriter.WriteField<string>("WEBSITE");
            csvWriter.WriteField<string>("PHONE");
            csvWriter.NextRecord();

            foreach (var location in provider.Locations.Where(l => l.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))
            {
                csvWriter.WriteField<string>(CommonHelper.GetSafeString(location.LocationId));
                csvWriter.WriteField<string>(location.ProviderOwnLocationRef);
                csvWriter.WriteField<string>(location.LocationName);
                csvWriter.WriteField<string>(location.Address == null ? string.Empty : location.Address.AddressLine1);
                csvWriter.WriteField<string>(location.Address == null ? string.Empty : location.Address.AddressLine2);
                csvWriter.WriteField<string>(location.Address == null ? string.Empty : location.Address.Town);
                csvWriter.WriteField<string>(location.Address == null ? string.Empty : location.Address.County);
                csvWriter.WriteField<string>(location.Address == null ? string.Empty : location.Address.Postcode);
                csvWriter.WriteField<string>(location.Email);
                csvWriter.WriteField<string>(location.Website);
                csvWriter.WriteField<string>(location.Telephone);

                csvWriter.NextRecord();
            }
        }

        private static void TransformApprenticeshipSection(CsvWriter csvWriter, Provider provider)
        {
            csvWriter.WriteField<string>("Apprenticeships");
            csvWriter.NextRecord();

            csvWriter.WriteField<string>("APPRENTICESHIP_ID*");
            csvWriter.WriteField<string>("STANDARD_CODE");
            csvWriter.WriteField<string>("STANDARD_VERSION");
            csvWriter.WriteField<string>("FRAMEWORK_CODE");
            csvWriter.WriteField<string>("FRAMEWORK_PROG_TYPE");
            csvWriter.WriteField<string>("FRAMEWORK_PATHWAY_CODE");
            csvWriter.WriteField<string>("APPRENTICESHIP_DELIVERY_INFO*");
            csvWriter.WriteField<string>("APPRENTICESHIP_INFO_URL");
            csvWriter.WriteField<string>("CONTACT_EMAIL*");
            csvWriter.WriteField<string>("CONTACT_US_URL");
            csvWriter.WriteField<string>("CONTACT_PHONE*");
            csvWriter.WriteField<string>("APPRENTICESHIP_DESCRIPTION (FOR YOUR INFO ONLY NOT REQUIRED FOR UPLOAD)");
            csvWriter.NextRecord();

            foreach (var apprenticeship in provider.Apprenticeships.Where(a => a.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))
            {
                csvWriter.WriteField<string>(CommonHelper.GetSafeString(apprenticeship.ApprenticeshipId));
                csvWriter.WriteField<string>(CommonHelper.GetSafeString(apprenticeship.StandardCode));
                csvWriter.WriteField<string>(CommonHelper.GetSafeString(apprenticeship.Version));
                csvWriter.WriteField<string>(CommonHelper.GetSafeString(apprenticeship.FrameworkCode));
                csvWriter.WriteField<string>(CommonHelper.GetSafeString(apprenticeship.ProgType));
                csvWriter.WriteField<string>(CommonHelper.GetSafeString(apprenticeship.PathwayCode));
                csvWriter.WriteField<string>(apprenticeship.MarketingInformation);
                csvWriter.WriteField<string>(apprenticeship.Url);
                csvWriter.WriteField<string>(apprenticeship.ContactEmail);
                csvWriter.WriteField<string>(apprenticeship.ContactWebsite);
                csvWriter.WriteField<string>(apprenticeship.ContactTelephone);
                csvWriter.WriteField<string>(apprenticeship.FullTextString());

                csvWriter.NextRecord();
            }
        }

        private static void TransformDeliveryLocationSection(CsvWriter csvWriter, Provider provider)
        {
            csvWriter.WriteField<string>("Delivery Locations");
            csvWriter.NextRecord();

            csvWriter.WriteField<string>("APPRENTICESHIP_ID*");
            csvWriter.WriteField<string>("LOCATION_ID*");
            csvWriter.WriteField<string>("RADIUS_(MILES)*");
            csvWriter.WriteField<string>("DELIVERY_MODES*");
            csvWriter.NextRecord();

            foreach (var apprenticeship in provider.Apprenticeships.Where(a => a.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))
            {
                foreach (var apprenticeshiplocation in apprenticeship.ApprenticeshipLocations.Where(al => al.Location.RecordStatusId.Equals((int)Constants.RecordStatus.Live)))
                {
                    csvWriter.WriteField<string>(CommonHelper.GetSafeString(apprenticeshiplocation.ApprenticeshipId));
                    csvWriter.WriteField<string>(CommonHelper.GetSafeString(apprenticeshiplocation.LocationId));
                    csvWriter.WriteField<string>(CommonHelper.GetSafeString(apprenticeshiplocation.Radius));
                    csvWriter.WriteField(apprenticeshiplocation.DeliveryModes == null ? string.Empty : GetDeliveryModeString(apprenticeshiplocation.DeliveryModes));

                    csvWriter.NextRecord();
                }
            }
        }

        private void WriteOpportunityRow(CsvWriter csvWriter, Provider provider, CourseInstance instance, string startDate)
        {
            csvWriter.WriteField<string>(CommonHelper.GetSafeString(instance.CourseId));
            LoadVenueName(csvWriter, instance);
            csvWriter.WriteField<string>(CommonHelper.GetSafeString(instance.ProviderOwnCourseInstanceRef));
            csvWriter.WriteField<string>(instance.StudyMode == null ? string.Empty : instance.StudyMode.BulkUploadRef);
            csvWriter.WriteField<string>(instance.AttendanceType == null ? string.Empty : instance.AttendanceType.BulkUploadRef);
            csvWriter.WriteField<string>(instance.AttendancePattern == null ? string.Empty : instance.AttendancePattern.BulkUploadRef);
            csvWriter.WriteField<string>(CommonHelper.GetSafeString(instance.DurationUnit));
            csvWriter.WriteField<string>(instance.DurationUnit1 == null ? string.Empty : instance.DurationUnit1.BulkUploadRef);
            csvWriter.WriteField<string>(CommonHelper.GetSafeString(instance.DurationAsText));
            csvWriter.WriteField<string>(startDate);
            csvWriter.WriteField<string>(CommonHelper.GetSafeDateString(instance.EndDate));
            csvWriter.WriteField<string>(instance.StartDateDescription);
            csvWriter.WriteField<string>(instance.TimeTable);
            csvWriter.WriteField<string>(CommonHelper.GetSafeString(instance.Price));
            csvWriter.WriteField<string>(instance.PriceAsText);
            csvWriter.WriteField<string>(instance.LanguageOfInstruction);
            csvWriter.WriteField<string>(instance.LanguageOfAssessment);
            csvWriter.WriteField<string>(CommonHelper.GetSafeString(instance.PlacesAvailable));
            csvWriter.WriteField<string>(CommonHelper.GetSafeDateString(instance.ApplyFromDate));
            csvWriter.WriteField<string>(CommonHelper.GetSafeDateString(instance.ApplyUntilDate));
            csvWriter.WriteField<string>(instance.ApplyUntilText);
            csvWriter.WriteField(instance.CanApplyAllYear ? "Y" : string.Empty);
            csvWriter.WriteField<string>(instance.EnquiryTo);
            csvWriter.WriteField<string>(instance.ApplyTo);
            csvWriter.WriteField<string>(instance.Url);
            csvWriter.WriteField<string>(instance.A10FundingCode == null ? string.Empty : GetA10String(instance.A10FundingCode));

            var offeredBy = CommonHelper.GetSafeString(instance.OfferedByOrganisationId);
            offeredBy = offeredBy == "0" ? "" : offeredBy;
            
            var displayedBy = CommonHelper.GetSafeString(instance.DisplayedByOrganisationId);
            displayedBy = displayedBy == "0" ? "" : displayedBy;

            if (offeredBy != displayedBy)
            {
                if (_userContext.IsOrganisation())
                {
                    if (offeredBy == String.Empty && displayedBy == _userContext.ItemId.ToString()) offeredBy = provider.ProviderId.ToString();
                    if (displayedBy == String.Empty && offeredBy == _userContext.ItemId.ToString()) displayedBy = provider.ProviderId.ToString();
                }
                if (_userContext.IsProvider())
                {
                    if (offeredBy != String.Empty && offeredBy != _userContext.ItemId.ToString() && displayedBy == String.Empty) displayedBy = provider.ProviderId.ToString();
                    if (displayedBy != String.Empty && displayedBy != _userContext.ItemId.ToString() && offeredBy == String.Empty) offeredBy = provider.ProviderId.ToString();
                }
            }

            csvWriter.WriteField<string>(offeredBy);
            csvWriter.WriteField<string>(displayedBy);
            csvWriter.WriteField<string>(instance.BothOfferedByDisplayBySearched ? "Y" : string.Empty);
        }

        private static void LoadVenueName(CsvWriter csvWriter, CourseInstance instance)
        {
            string venueName; 
            if (instance.Venues.Any())
            {
                venueName = instance.Venues.First().VenueId.ToString(CultureInfo.InvariantCulture);
            }
            else if (instance.VenueLocationId != null)
            {
                venueName = instance.VenueLocation.LocationName;

                if (instance.VenueLocation.ParentVenueLocation != null)
                    venueName += string.Concat(Constants.PipeSeperator, instance.VenueLocation.ParentVenueLocation.LocationName);
            }
            else
            {
                venueName = "UNITED KINGDOM|WORLD";
            }

            csvWriter.WriteField<string>(venueName);
        }

        private static string GetA10String(IEnumerable<A10FundingCode> collection)
        {
            var str = collection.Select(item => item.A10FundingCodeId.ToString()).ToList();

            return str.Count.Equals(0) ? Constants.BulkUpload_DataStandardMessage_Opportunity.Not_Applicable : string.Join(Constants.BulkUploadA10SplitCharacter.ToString(), str);
        }

        private static string GetDeliveryModeString(IEnumerable<DeliveryMode> collection)
        {
            var str = collection.Select(item => item.BulkUploadRef.ToString()).ToList();

            return string.Join(Constants.PipeSeperator.ToString(), str);
        }

        public List<Provider> LoadProviders(UserContext.UserContextInfo userContext, ProviderPortalEntities db, bool downloadableProvidersOnly = false)
        {
            var providers = new List<Provider>();

            switch (userContext.ContextName)
            {
                case UserContext.UserContextName.Provider:
                    {
                        var provider = db.Providers.Find(userContext.ItemId);

                        if (provider != null)
                        {
                            providers.Add(provider);
                        }
                        break;
                    }
                case UserContext.UserContextName.Organisation:
                    {
                        var organisation = db.Organisations.FirstOrDefault(x => x.OrganisationId == userContext.ItemId);

                        if (organisation != null)
                            foreach (var organisationProvider in organisation.OrganisationProviders)
                            {
                                if (downloadableProvidersOnly)
                                {
                                    if (organisationProvider.CanOrganisationEditProvider)
                                    {
                                        providers.Add(organisationProvider.Provider);
                                    }
                                }
                                else
                                {
                                    providers.Add(organisationProvider.Provider);
                                }
                            }
                        break;
                    }
            }
            return providers;
        }

        private static string GetSubjectClassification(ICollection<CourseLearnDirectClassification> courseLearnDirectClassifications, int id)
        {
            if (courseLearnDirectClassifications == null)
            {
                return string.Empty;
            }

            if (courseLearnDirectClassifications.Count < id)
            {
                return string.Empty;
            }

            return courseLearnDirectClassifications.ToList()[id - 1].LearnDirectClassificationRef;
        }
    }
}