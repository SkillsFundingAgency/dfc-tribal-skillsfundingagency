using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class AddEditCourseModel
    {
        public Int32? CourseId { get; set; }

        [LanguageDisplay("Status")]
        public Int32? RecordStatusId { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Provider Course Title")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter your course title.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Course Title")]
        public String CourseTitle { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Summary")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(2000, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please give a brief description of the course and any additional information you may wish to add. Summaries are very important for prospective students to make an informative decision when selecting a course.")]
        //[ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Summary")]
        [ProviderPortalEditCourseOpportunityTextField(ErrorMessage = "Please enter a valid Summary")]
        public String CourseSummary { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Entry Requirements")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(4000, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter any requirements or skills the student will need to undertake this course. Examples include: Experience ; Academic level; Specific qualifications ; Any other requirements such as a driving licence.")]
        //[ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Entry Requirement")]
        [ProviderPortalEditCourseOpportunityTextField(ErrorMessage = "Please enter a valid Entry Requirement")]
        public String EntryRequirements { get; set; }

        [LanguageDisplay("Provider Course Id")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter any unique identifier that your provider has for this course. This may be a code/ID or even a URL unique to this course.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Provider Course Id")]
        public String ProviderOwnCourseRef { get; set; }

        [LanguageRequired]
        [LanguageDisplay("URL")]
        [ProviderPortalUrl(ErrorMessage = "Please enter a valid URL")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter a URL that may contain more information about the course.")]
        public String Url { get; set; }

        [LanguageDisplay("Subject Classification 1")]
        [Display(Description = "Please enter a subject keyword for your course. This will bring up a list of potential subject classifications for you to choose from, please select the most appropriate one to categorise your course. Subject classification is important as it helps determine where this course appears in the user facing websites.")]
        public String LearnDirectClassificationId1 { get; set; }

        public String LearnDirectClassification_1 { get; set; }

        [LanguageDisplay("Subject Classification 2")]
        [Display(Description = "If your course needs more then one subject classification to describe all aspects, please enter another subject keyword. This will bring up a list of potential subject classifications for you to choose from, please select another appropriate one to categorise your course. Subject classification is important as it helps determine where this course shows up in the user facing websites.")]
        public String LearnDirectClassificationId2 { get; set; }

        public String LearnDirectClassification_2 { get; set; }

        [LanguageDisplay("Subject Classification 3")]
        [Display(Description = "If your course needs more then two subject classification to describe all aspects, please enter another subject keyword. This will bring up a list of potential subject classifications for you to choose from, please select another appropriate one to categorise your course. Subject classification is important as it helps determine where this course shows up in the user facing websites.")]
        public String LearnDirectClassificationId3 { get; set; }

        public String LearnDirectClassification_3 { get; set; }

        [LanguageDisplay("Subject Classification 4")]
        [Display(Description = "If your course needs more then three subject classification to describe all aspects, please enter another subject keyword. This will bring up a list of potential subject classifications for you to choose from, please select another appropriate one to categorise your course. Subject classification is important as it helps determine where this course shows up in the user facing websites.")]
        public String LearnDirectClassificationId4 { get; set; }

        public String LearnDirectClassification_4 { get; set; }

        [LanguageDisplay("Subject Classification 5")]
        [Display(Description = "If your course needs more then four subject classification to describe all aspects, please enter another subject keyword. This will bring up a list of potential subject classifications for you to choose from, please select another appropriate one to categorise your course. Subject classification is important as it helps determine where this course shows up in the user facing websites.")]
        public String LearnDirectClassificationId5 { get; set; }

        public String LearnDirectClassification_5 { get; set; }

        [LanguageDisplay("Booking URL")]
        [ProviderPortalUrl(ErrorMessage = "Please enter a valid URL")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter the URL which directs the student to a page where they can book this course.")]
        public String BookingUrl { get; set; }

        [LanguageDisplay("Assessment Method")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(4000, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter the method of assessment used for the qualification awarded. Examples include: Exam; Assignment; and Continuous assessment.")]
        //[ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Assessment Method")]
        [ProviderPortalEditCourseOpportunityTextField(ErrorMessage = "Please enter a valid Assessment Method")]
        public String AssessmentMethod { get; set; }

        [LanguageDisplay("Equipment Required")]
        [DataType(DataType.MultilineText)]
        [LanguageStringLength(4000, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter any equipment needed to complete the course. For example: Computer, Musical Instrument, Calculator, yoga mat, footwear etc.")]
        //[ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter valid Required Equipment")]
        [ProviderPortalEditCourseOpportunityTextField(ErrorMessage = "Please enter a valid Required Equipment")]
        public String EquipmentRequired { get; set; }

        [LanguageDisplay("Awarding Organisation")]
        [LanguageStringLength(255, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "If the Qualification given at the end of the course is awarded or accredited by a particular body, please enter the name of the organisation.")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Awarding Organisation")]
        public String AwardingOrganisation { get; set; }

        [LanguageDisplay("Qualification Level")]
        [Display(Description = "Please select the Qualification Level for this course. This should be a National Qualifications Framework (NQF) Level. If a Level is already selected, you may change it if you believe it to be incorrect.")]
        public Int32? QualificationLevelId { get; set; }

        [LanguageDisplay("Qualification Type")]
        [Display(Description = "Please select the type of Qualification awarded at the end of the course from the drop down menu.")]
        public Int32? QualificationTypeId { get; set; }

        [LanguageDisplay("Qualification Title")]
        [Display(Description = "Please enter a detailed description of the type of Qualification awarded at the end of the course e.g. Advanced Apprenticeship at Level 3 in Plumbing; GCE A Level in Biology; BSc(Psych) - Bachelor of Science in Psychology; MA - Master of Arts")]
        [ProviderPortalTextFieldAttribute(ErrorMessage = "Please enter a valid Qualification Title")]
        public String WhenNoLarQualificationTitle { get; set; }

        [LanguageDisplay("UCAS Tariff Points")]
        [Display(Description = "The UCAS Tariff is the system for allocating points to qualifications used for entry to higher education. Please enter the tariff for this course")]
        public Int32? UcasTariffPoints { get; set; }

        [LanguageDisplay("Learning Aim")]
        [Display(Description = "The Learning Aim Reference is an identifier for a Learning Aim in the Learning Aims Database (LARS) administered by The Data Service (http://www.thedataservice.org.uk/). All courses funded by the Skills Funding Agency should have a Learning Aim Reference.")]
        public String LearningAimId { get; set; }

        public Int32? LearningAimQualificationTypeId { get; set; }

        [LanguageDisplay("Qualification")]
        public String Qualification { get; set; }

        public List<OpportunityListModel> Opportunities { get; set; }

        public AddEditCourseModel()
        {
            this.Opportunities = new List<OpportunityListModel>();
        }

        public AddEditCourseModel(Course course) : this()
        {
            this.CourseId = course.CourseId;
            this.CourseTitle = course.CourseTitle;
            this.CourseSummary = course.CourseSummary;
            this.EntryRequirements = course.EntryRequirements;
            this.ProviderOwnCourseRef = course.ProviderOwnCourseRef;
            this.Url = course.Url;
            this.BookingUrl = course.BookingUrl;
            this.AssessmentMethod = course.AssessmentMethod;
            this.EquipmentRequired = course.EquipmentRequired;
            this.AwardingOrganisation = course.AwardingOrganisationName;
            this.UcasTariffPoints = course.UcasTariffPoints;

            this.QualificationLevelId = course.QualificationLevelId;

            this.RecordStatusId = course.RecordStatusId;

            this.LearningAimId = course.LearningAimRefId;
            this.Qualification = course.LearningAim == null ? "" : !String.IsNullOrWhiteSpace(course.LearningAim.Qualification) ? course.LearningAim.Qualification : course.LearningAim.LearningAimTitle;
            this.LearningAimQualificationTypeId = course.LearningAim == null ? null : course.LearningAim.QualificationTypeId;

            this.QualificationTypeId = course.WhenNoLarQualificationTypeId;
            this.WhenNoLarQualificationTitle = course.WhenNoLarQualificationTitle;

            Int32 i = 0;
            foreach (CourseLearnDirectClassification ld in course.CourseLearnDirectClassifications.OrderBy(x => x.ClassificationOrder))
            {
                i++;

                switch (i)
                {
                    case 1:
                        this.LearnDirectClassificationId1 = ld.LearnDirectClassificationRef;
                        this.LearnDirectClassification_1 = ld.LearnDirectClassification.GetDescription();
                        break;
                    case 2:
                        this.LearnDirectClassificationId2 = ld.LearnDirectClassificationRef;
                        this.LearnDirectClassification_2 = ld.LearnDirectClassification.GetDescription();
                        break;
                    case 3:
                        this.LearnDirectClassificationId3 = ld.LearnDirectClassificationRef;
                        this.LearnDirectClassification_3 = ld.LearnDirectClassification.GetDescription();
                        break;
                    case 4:
                        this.LearnDirectClassificationId4 = ld.LearnDirectClassificationRef;
                        this.LearnDirectClassification_4 = ld.LearnDirectClassification.GetDescription();
                        break;
                    case 5:
                        this.LearnDirectClassificationId5 = ld.LearnDirectClassificationRef;
                        this.LearnDirectClassification_5 = ld.LearnDirectClassification.GetDescription();
                        break;
                }

                if (i == 5)
                {
                    break;
                }
            }

            foreach (CourseInstance courseInstance in course.CourseInstances.OrderByDescending(x=> x.ModifiedDateTimeUtc ?? x.CreatedDateTimeUtc))
            {
                Opportunities.Add(new OpportunityListModel(courseInstance));
            }
        }
    }

    public class ViewCourseModel : AddEditCourseModel
    {
        public String RecordStatusName { get; set; }
        public String QualificationType { get; set; }
        public String QualificationLevel { get; set; }
        public String LearningAim { get; set; }

        public ViewCourseModel()
        {}

        public ViewCourseModel(Course course) : base(course)
        {
            this.RecordStatusName = course.RecordStatu.RecordStatusName;
            this.QualificationLevel = course.QualificationLevel == null ? "" : course.QualificationLevel.QualificationLevelName;
            this.QualificationType = course.QualificationType == null ? "" : course.QualificationType.QualificationTypeName;
            this.LearningAim = course.LearningAim == null ? "" : course.LearningAim.LearningAimTitle;            
        }
    }

    public class CourseListModel
    {
        [LanguageDisplay("Course Id")]
        public Int32 CourseId { get; set; }

        [LanguageDisplay("Status")]
        public String Status { get; set; }

        [LanguageDisplay("Course Details")]
        public String CourseDetails { get; set; }

        [LanguageDisplay("Course Title")]
        public String CourseTitle { get; set; }

        [LanguageDisplay("Provider Course Id")]
        public String ProviderOwnCourseRef { get; set; }

        [LanguageDisplay("Last Update")]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime LastUpdate { get; set; }

        [LanguageDisplay("Date Status")]
        public String LastStartDate { get; set; }

        [LanguageDisplay("LAR Status")]
        public Boolean? IsLARSExpired { get; set; }

        public Constants.CourseFilterDateStatus? DateStatus { get; set; }

        public Int32? RecordStatusId { get; set; }

        public CourseListModel()
        {
            DateTime.SpecifyKind(this.LastUpdate, DateTimeKind.Utc);
        }

        public CourseListModel(Course course, DateTime? lastStartDate, Constants.CourseFilterDateStatus? courseDateStatus) : this()
        {
            this.CourseId = course.CourseId;
            this.Status = course.RecordStatu.RecordStatusName;
            this.RecordStatusId = course.RecordStatusId;

            this.CourseTitle = course.CourseTitle;
            this.ProviderOwnCourseRef = course.ProviderOwnCourseRef;

            this.CourseDetails = this.CourseTitle;
            if (course.LearningAim != null)
            {
                this.CourseDetails = String.IsNullOrWhiteSpace(course.LearningAim.Qualification) ? String.Format("{0} | {1}", course.CourseTitle, course.LearningAim.LearningAimTitle) : String.Format("{0} | {1} | {2}", course.CourseTitle, course.LearningAim.LearningAimTitle, course.LearningAim.Qualification);
            }

            this.DateStatus = courseDateStatus;

            if (courseDateStatus == Constants.CourseFilterDateStatus.UpToDate)
            {
                this.LastStartDate = AppGlobal.Language.GetText("CourseListModel_LastStartDate_CourseUpToDate", "OK");
            }
            else if (courseDateStatus == Constants.CourseFilterDateStatus.Expiring)
            {
                this.LastStartDate = string.Format(AppGlobal.Language.GetText("CourseListModel_LastStartDate_DueUpdate", "Due update. Last start {0}"), lastStartDate.HasValue ? lastStartDate.Value.ToString(Constants.ConfigSettings.ShortDateFormat) : "");
            }
            else if (courseDateStatus == Constants.CourseFilterDateStatus.OutOfDate)
            {
                this.LastStartDate = string.Format(AppGlobal.Language.GetText("CourseListModel_LastStartDate_IsOutOfDateYes", "Out of date. Last start {0}"), lastStartDate.HasValue ? lastStartDate.Value.ToString(Constants.ConfigSettings.ShortDateFormat) : "");
            }
            else
            {
                this.LastStartDate = string.Empty;
            }

            if (course.LearningAim != null)
            {
                this.IsLARSExpired = course.LearningAim.RecordStatusId != (Int32) Constants.RecordStatus.Live;
            }

            this.LastUpdate = course.ModifiedDateTimeUtc ?? course.CreatedDateTimeUtc;
        }
    }

    public class CourseSearchModel
    {
        [LanguageDisplay("Number of Pending Courses")]
        public Int32 NumberOfPendingCourses { get; set; }

        [LanguageDisplay("Provider Course Id")]
        public String ProviderCourseId { get; set; }

        [LanguageDisplay("Provider Course Title")]
        public String ProviderCourseTitle { get; set; }

        [LanguageDisplay("Learning Aim Reference")]
        public String LearningAimReference { get; set; }

        [LanguageDisplay("Last Course Updated Date")]
        [DateDisplayFormat(Format = DateFormat.ShortDate, ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? LastUpdated { get; set; }

        [LanguageDisplay("Course Status")]
        public Int32? CourseStatus { get; set; }

        [LanguageDisplay("Provider Opportunity Id")]
        public String ProviderOpportunityId { get; set; }

        [LanguageDisplay("Study Mode")]
        public Int32? StudyModeId { get; set; }

        [LanguageDisplay("Attendance Mode")]
        public Int32? AttendanceModeId { get; set; }

        [LanguageDisplay("Attendance Pattern")]
        public Int32? AttendancePatternId { get; set; }

        [LanguageDisplay("Venue")]
        public Int32? VenueId { get; set; }

        [LanguageDisplay("Start Date")]
        public Int32? StartDateId { get; set; }

        [LanguageDisplay("Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [LanguageDisplay("Start Date Description")]
        public String StartDateDescription { get; set; }

        [LanguageDisplay("Opportunity Status")]
        public Int32? OpportunityStatus { get; set; }

        public Constants.CourseSearchQAFilter? QualitySearchMode { get; set; }

        public List<CourseListModel> Courses { get; set; }

        public CourseSearchModel()
        {
            this.Courses = new List<CourseListModel>();
        }
    }

    public class OutOfDateCourseModel
    {
        [LanguageDisplay("Provider Id")]
        public Int32 ProviderId { get; set; }

        public List<OutOfDateCourseModelItem> Items { get; set; }
    }

    public class OutOfDateCourseModelItem
    {
        [LanguageDisplay("Course Id")]
        public Int32 CourseId { get; set; }

        [LanguageDisplay("Max Start Date")]
        [DateDisplayFormat(Format = DateFormat.ShortDate, ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime MaxStartDate { get; set; }
    }


    public class CourseDateStatusModelItem
    {
        [LanguageDisplay("Course Id")]
        public Int32 CourseId { get; set; }

        [LanguageDisplay("Max Start Date")]
        [DateDisplayFormat(Format = DateFormat.ShortDate, ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? MaxStartDate { get; set; }

        public Constants.CourseFilterDateStatus DateStatus {get; set;}
    }

    public class CourseDateStatusModel
    {
        public Int32 ProviderId { get; set; }
        public List<CourseDateStatusModelItem> Items { get; set; }
    }

    public class AddCourseModel
    {
        public Int32 CourseHasLearningAimRef { get; set; }
        public LearningAim LearningAim { get; set; }    
    }

    public class LearningAimRefModel
    {
        public String LearningAimRefId { get; set; }
        public String LearningAimTitle { get; set; }
        public String Qualification { get; set; }
        public Int32? QualificationTypeId { get; set; }
        public Int32? QualificationLevelId { get; set; }
        public String AwardingBody { get; set; }

        public LearningAimRefModel(LearningAim learningAim)
        {
            this.LearningAimRefId = learningAim.LearningAimRefId;
            this.LearningAimTitle = learningAim.LearningAimTitle;
            this.Qualification = learningAim.Qualification;
            this.QualificationTypeId = learningAim.QualificationTypeId;
            this.QualificationLevelId = learningAim.QualificationLevelId;
            this.AwardingBody = learningAim.LearningAimAwardOrg.AwardOrgName;
        }
    }
}