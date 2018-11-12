using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class AddEditSearchPhraseModel
    {
        public Int32? SearchPhraseId { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Search Phrase")]
        [LanguageStringLength(50, ErrorMessage = "The maximum length of {0} is {1} characters.")]
        [Display(Description = "Please enter the phrase that you would like to replace in the course search API.")]
        public String SearchPhrase { get; set; }

        public Int32[] SelectedQualificationLevels { get; set; }

        public Int32[] SelectedStudyModes { get; set; }

        public Int32[] SelectedAttendanceTypes { get; set; }

        public Int32[] SelectedAttendancePatterns { get; set; }

        [LanguageDisplay("Qualification Levels")]
        public IEnumerable<QualificationLevel> QualificationLevels { get; set; }

        [LanguageDisplay("Study Modes")]
        public IEnumerable<StudyMode> StudyModes { get; set; }

        [LanguageDisplay("Attendance Modes")]
        public IEnumerable<AttendanceType> AttendanceTypes { get; set; }

        [LanguageDisplay("Attendance Patterns")]
        public IEnumerable<AttendancePattern> AttendancePatterns { get; set; }

        [LanguageRequired]
        [LanguageDisplay("Status")]
        public Int32 RecordStatusId { get; set; }

        [LanguageDisplay("Remove Phrase From Search Terms")]
        public Boolean RemovePhraseFromSearch { get; set; }

        public AddEditSearchPhraseModel()
        {
            RecordStatusId = (Int32)Constants.RecordStatus.Live;
            SelectedQualificationLevels = new Int32[0];
            SelectedStudyModes = new Int32[0];
            SelectedAttendanceTypes = new Int32[0];
            SelectedAttendancePatterns = new Int32[0];
            RemovePhraseFromSearch = true;
        }

        public AddEditSearchPhraseModel(SearchPhrase searchPhrase) : this()
        {
            SearchPhraseId = searchPhrase.SearchPhraseId;
            SearchPhrase = searchPhrase.Phrase;
            SelectedQualificationLevels = searchPhrase.QualificationLevels.Select(x => x.QualificationLevelId).ToArray();
            SelectedStudyModes = searchPhrase.StudyModes.Select(x => x.StudyModeId).ToArray();
            SelectedAttendanceTypes = searchPhrase.AttendanceTypes.Select(x => x.AttendanceTypeId).ToArray();
            SelectedAttendancePatterns = searchPhrase.AttendancePatterns.Select(x => x.AttendancePatternId).ToArray();
            RecordStatusId = searchPhrase.RecordStatusId;
            RemovePhraseFromSearch = searchPhrase.RemovePhraseFromSearch;
        }
    }

    public class SearchPhraseListModel
    {
        public List<SearchPhraseListItemModel> Items { get; set; }

        public SearchPhraseListModel()
        {
            Items = new List<SearchPhraseListItemModel>();
        }
    }

    public class SearchPhraseListItemModel
    {
        public Int32 SearchPhraseId { get; set; }

        [LanguageDisplay("Search Phrase")]
        public String SearchPhrase { get; set; }

        [LanguageDisplay("Qualification Levels")]
        public String QualificationLevels { get; set; }

        [LanguageDisplay("Attendance Modes")]
        public String AttendanceTypes { get; set; }

        [LanguageDisplay("Attendance Patterns")]
        public String AttendancePatterns { get; set; }

        [LanguageDisplay("Study Modes")]
        public String StudyModes { get; set; }

        [LanguageDisplay("Status")]
        public String Status { get; set; }

        [LanguageDisplay("Ordinal")]
        public Int32 Ordinal { get; set; }

        [LanguageDisplay("Updated By")]
        public String UpdatedByUser { get; set; }

        [LanguageDisplay("Last Updated")]
        [DataType(DataType.Date)]
        [DateDisplayFormat(Format = DateFormat.ShortDate)]
        public DateTime UpdatedDateTimeUtc { get; set; }

        [LanguageDisplay("Remove Phrase From Search Terms")]
        public Boolean RemovePhraseFromSearch { get; set; }
    }
}