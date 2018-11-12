using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.CourseSearchService.Entities
{
    /// <summary>
    /// Course Entity.
    /// </summary>
    public class Course
    {
        #region Variables

        private string _accreditactionEndDate;
        private string _accreditactionStartDate;
        private string _adultLRStatus;
        private string _assessmentMethod;
        private string _awardingBody;
        private string _bookingUrl;
        private string _certificationEndDate;
        private long _courseId;
        private string _courseSummary;
        private string _courseTitle;
        private string _creditValue;
        private string _dataType;
        private string _entryRequirements;
        private string _equipmentRequired;
        private string _erAppStatus;
        private string _erTtgStatus;
        private string _independentLivingSkills;
        private string _ladId;
        private string _ldcsCode1;
        private string _ldcsCode2;
        private string _ldcsCode3;
        private string _ldcsCode4;
        private string _ldcsCode5;
        private List<LdcsCode> _ldcsCodes;
        private string _ldcsDesc1;
        private string _ldcsDesc2;
        private string _ldcsDesc3;
        private string _ldcsDesc4;
        private string _ldcsDesc5;
        private string _level2EntitlementDesc;
        private string _level3EntitlementDesc;
        private long _numberOfOpportunities;
        private List<Opportunity> _opportunities;
        private string _otherFundingNonFundedStatus;
        private Provider _provider;
        private string _providerName;
        private string _qcaGuidedLearningHours;
        private string _qualificationLevel;
        private string _qualificationReference;
        private string _qualificationTitle;
        private string _qualificationType;
        private string _qualificationReferenceAuthority;
        private string _sectorLeadBodyDesc;
        private string _skillsForLifeFlag;
        private string _skillsForLifeTypeDescription;
        private string _tariffRequired;
        private string _url;
        private List<Venue> _venues;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Accreditation end date field.
        /// </summary>
        public string AccreditationEndDate
        {
            get
            {
                return _accreditactionEndDate;
            }
            set
            {
                _accreditactionEndDate = value;
            }
        }

        /// <summary>
        /// Accreditation start date field.
        /// </summary>
        public string AccreditationStartDate
        {
            get
            {
                return _accreditactionStartDate;
            }
            set
            {
                _accreditactionStartDate = value;
            }
        }

        /// <summary>
        /// Adult LR status field.
        /// </summary>
        public string AdultLRStatus
        {
            get
            {
                return _adultLRStatus;
            }
            set
            {
                _adultLRStatus = value;
            }
        }

        /// <summary>
        /// Assessment method field.
        /// </summary>
        public string AssessmentMethod
        {
            get
            {
                return _assessmentMethod;
            }
            set
            {
                _assessmentMethod = value;
            }
        }

        /// <summary>
        /// Awarding body field.
        /// </summary>
        public string AwardingBody
        {
            get
            {
                return _awardingBody;
            }
            set
            {
                _awardingBody = value;
            }
        }

        /// <summary>
        /// Booking url field.
        /// </summary>
        public string BookingUrl
        {
            get
            {
                return _bookingUrl;
            }
            set
            {
                _bookingUrl = value;
            }
        }

        /// <summary>
        /// Certification end date field.
        /// </summary>
        public string CertificationEndDate
        {
            get
            {
                return _certificationEndDate;
            }
            set
            {
                _certificationEndDate = value;
            }
        }

        /// <summary>
        /// Course Id field.
        /// </summary>
        public long CourseId
        {
            get
            {
                return _courseId;
            }
            set
            {
                _courseId = value;
            }
        }

        /// <summary>
        /// Course summary field.
        /// </summary>
        public string CourseSummary
        {
            get
            {
                return _courseSummary;
            }
            set
            {
                _courseSummary = value;
            }
        }

        /// <summary>
        /// Course title field.
        /// </summary>
        public string CourseTitle
        {
            get
            {
                return _courseTitle;
            }
            set
            {
                _courseTitle = value;
            }
        }

        /// <summary>
        /// Credit value field.
        /// </summary>
        public string CreditValue
        {
            get
            {
                return _creditValue;
            }
            set
            {
                _creditValue = value;
            }
        }

        /// <summary>
        /// Data type field.
        /// </summary>
        public string DataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                _dataType = value;
            }
        }

        /// <summary>
        /// Entry requirements field.
        /// </summary>
        public string EntryRequirements
        {
            get
            {
                return _entryRequirements;
            }
            set
            {
                _entryRequirements = value;
            }
        }

        /// <summary>
        /// Equipment required field.
        /// </summary>
        public string EquipmentRequired
        {
            get
            {
                return _equipmentRequired;
            }
            set
            {
                _equipmentRequired = value;
            }
        }

        /// <summary>
        /// ER App status field.
        /// </summary>
        public string ERAppStatus
        {
            get
            {
                return _erAppStatus;
            }
            set
            {
                _erAppStatus = value;
            }
        }

        /// <summary>
        /// ER TTG status field.
        /// </summary>
        public string ERTtgStatus
        {
            get
            {
                return _erTtgStatus;
            }
            set
            {
                _erTtgStatus = value;
            }
        }

        /// <summary>
        /// Independent living skills field.
        /// </summary>
        public string IndependentLivingSkills
        {
            get
            {
                return _independentLivingSkills;
            }
            set
            {
                _independentLivingSkills = value;
            }
        }

        /// <summary>
        /// LAD Id field.
        /// </summary>
        public string LadId
        {
            get
            {
                return _ladId;
            }
            set
            {
                _ladId = value;
            }
        }

        /// <summary>
        /// LDCS code 1 field.
        /// </summary>
        public string LdcsCode1
        {
            get
            {
                return _ldcsCode1;
            }
            set
            {
                _ldcsCode1 = value;
            }
        }

        /// <summary>
        /// LDCS code 2 field.
        /// </summary>
        public string LdcsCode2
        {
            get
            {
                return _ldcsCode2;
            }
            set
            {
                _ldcsCode2 = value;
            }
        }

        /// <summary>
        /// LDCS code 3 field.
        /// </summary>
        public string LdcsCode3
        {
            get
            {
                return _ldcsCode3;
            }
            set
            {
                _ldcsCode3 = value;
            }
        }

        /// <summary>
        /// LDCS code 4 field.
        /// </summary>
        public string LdcsCode4
        {
            get
            {
                return _ldcsCode4;
            }
            set
            {
                _ldcsCode4 = value;
            }
        }

        /// <summary>
        /// LDCS code 5 field.
        /// </summary>
        public string LdcsCode5
        {
            get
            {
                return _ldcsCode5;
            }
            set
            {
                _ldcsCode5 = value;
            }
        }

        /// <summary>
        /// LDCS codes collection field.
        /// </summary>
        public List<LdcsCode> LdcsCodes
        {
            get
            {
                if (_ldcsCodes == null)
                {
                    _ldcsCodes = new List<LdcsCode>();
                }
                return _ldcsCodes;
            }
            set
            {
                _ldcsCodes = value;
            }
        }

        /// <summary>
        /// LDCS description 1 field.
        /// </summary>
        public string LdcsDesc1
        {
            get
            {
                return _ldcsDesc1;
            }
            set
            {
                _ldcsDesc1 = value;
            }
        }

        /// <summary>
        /// LDCS description 2 field.
        /// </summary>
        public string LdcsDesc2
        {
            get
            {
                return _ldcsDesc2;
            }
            set
            {
                _ldcsDesc2 = value;
            }
        }

        /// <summary>
        /// LDCS description 3 field.
        /// </summary>
        public string LdcsDesc3
        {
            get
            {
                return _ldcsDesc3;
            }
            set
            {
                _ldcsDesc3 = value;
            }
        }

        /// <summary>
        /// LDCS description 4 field.
        /// </summary>
        public string LdcsDesc4
        {
            get
            {
                return _ldcsDesc4;
            }
            set
            {
                _ldcsDesc4 = value;
            }
        }

        /// <summary>
        /// LDCS description 5 field.
        /// </summary>
        public string LdcsDesc5
        {
            get
            {
                return _ldcsDesc5;
            }
            set
            {
                _ldcsDesc5 = value;
            }
        }

        /// <summary>
        /// Level 2 entitlement description field.
        /// </summary>
        public string Level2EntitlementDescription
        {
            get
            {
                return _level2EntitlementDesc;
            }
            set
            {
                _level2EntitlementDesc = value;
            }
        }

        /// <summary>
        /// Level 3 entitlement description field.
        /// </summary>
        public string Level3EntitlementDescription
        {
            get
            {
                return _level3EntitlementDesc;
            }
            set
            {
                _level3EntitlementDesc = value;
            }
        }

        /// <summary>
        /// Number of opportunities field.
        /// </summary>
        public long NumberOfOpportunities
        {
            get
            {
                return _numberOfOpportunities;
            }
            set
            {
                _numberOfOpportunities = value;
            }
        }

        /// <summary>
        /// Opportunities collection field.
        /// </summary>
        public List<Opportunity> Opportunities
        {
            get
            {
                if (_opportunities == null)
                {
                    _opportunities = new List<Opportunity>();
                }
                return _opportunities;
            }
            set
            {
                _opportunities = value;
            }
        }

        /// <summary>
        /// Other funding non-funded status field.
        /// </summary>
        public string OtherFundingNonFundedStatus
        {
            get
            {
                return _otherFundingNonFundedStatus;
            }
            set
            {
                _otherFundingNonFundedStatus = value;
            }
        }

        /// <summary>
        /// Provider field.
        /// </summary>
        public Provider Provider
        {
            get
            {
                return _provider;
            }
            set
            {
                _provider = value;
            }
        }

        ///// <summary>
        ///// Provider name field.
        ///// </summary>
        //public string ProviderName
        //{
        //    get
        //    {
        //        return _providerName;
        //    }
        //    set
        //    {
        //        _providerName = value;
        //    }
        //}

        /// <summary>
        /// QCA guided learning hours field.
        /// </summary>
        public string QcaGuidedLearningHours
        {
            get
            {
                return _qcaGuidedLearningHours;
            }
            set
            {
                _qcaGuidedLearningHours = value;
            }
        }

        /// <summary>
        /// Qualification level field.
        /// </summary>
        public string QualificationLevel
        {
            get
            {
                return _qualificationLevel;
            }
            set
            {
                _qualificationLevel = value;
            }
        }

        /// <summary>
        /// Qualification reference field.
        /// </summary>
        public string QualificationReference
        {
            get
            {
                return _qualificationReference;
            }
            set
            {
                _qualificationReference = value;
            }
        }

        /// <summary>
        /// Qualification title field.
        /// </summary>
        public string QualificationTitle
        {
            get
            {
                return _qualificationTitle;
            }
            set
            {
                _qualificationTitle = value;
            }
        }

        /// <summary>
        /// Qualification type field.
        /// </summary>
        public string QualificationType
        {
            get
            {
                return _qualificationType;
            }
            set
            {
                _qualificationType = value;
            }
        }

        /// <summary>
        /// Qualification reference authority field.
        /// </summary>
        public string QualificationReferenceAuthority
        {
            get
            {
                return _qualificationReferenceAuthority;
            }
            set
            {
                _qualificationReferenceAuthority = value;
            }
        }

        /// <summary>
        /// Sector lead body description field.
        /// </summary>
        public string SectorLeadBodyDescription
        {
            get
            {
                return _sectorLeadBodyDesc;
            }
            set
            {
                _sectorLeadBodyDesc = value;
            }
        }

        /// <summary>
        /// Skills for life flag field.
        /// </summary>
        public string SkillsForLifeFlag
        {
            get
            {
                return _skillsForLifeFlag;
            }
            set
            {
                _skillsForLifeFlag = value;
            }
        }

        /// <summary>
        /// Skills for life type description field.
        /// </summary>
        public string SkillsForLifeTypeDescription
        {
            get
            {
                return _skillsForLifeTypeDescription;
            }
            set
            {
                _skillsForLifeTypeDescription = value;
            }
        }

        /// <summary>
        /// Tariff required field.
        /// </summary>
        public string TariffRequired
        {
            get
            {
                return _tariffRequired;
            }
            set
            {
                _tariffRequired = value;
            }
        }

        /// <summary>
        /// Url field.
        /// </summary>
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        /// <summary>
        /// Venues collection field.
        /// </summary>
        public List<Venue> Venues
        {
            get
            {
                if (_venues == null)
                {
                    _venues = new List<Venue>();
                }
                return _venues;
            }
            set
            {
                _venues = value;
            }
        }
        
        #endregion Properties
    }
}
