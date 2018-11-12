using System;
using System.Collections.Generic;
using System.Linq;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class SearchPhraseModelExtensions
    {
        public static SearchPhraseListModel Populate(this SearchPhraseListModel model, ProviderPortalEntities db)
        {
            foreach (SearchPhrase searchPhrase in db.SearchPhrases.OrderBy(x => x.Ordinal))
            {
                model.Items.Add(new SearchPhraseListItemModel {
                    SearchPhraseId = searchPhrase.SearchPhraseId,
                    SearchPhrase = searchPhrase.Phrase,
                    QualificationLevels = String.Join("<br />", searchPhrase.QualificationLevels.Select(x => x.QualificationLevelName).ToList()),
                    StudyModes = String.Join("<br />", searchPhrase.StudyModes.Select(x => x.StudyModeName).ToList()),
                    AttendanceTypes = String.Join("<br />", searchPhrase.AttendanceTypes.Select(x => x.AttendanceTypeName).ToList()),
                    AttendancePatterns = String.Join("<br />", searchPhrase.AttendancePatterns.Select(x => x.AttendancePatternName).ToList()),
                    UpdatedDateTimeUtc = searchPhrase.ModifiedDateTimeUtc ?? searchPhrase.CreatedDateTimeUtc,
                    UpdatedByUser = searchPhrase.ModifiedByUserId != null 
                            ? searchPhrase.ModifiedByUser.Name
                            : searchPhrase.CreatedByUser.Name,
                    Status = searchPhrase.RecordStatus.RecordStatusName,
                    RemovePhraseFromSearch = searchPhrase.RemovePhraseFromSearch
                });
            }

            return model;
        }

        public static SearchPhrase ToEntity(this AddEditSearchPhraseModel model, ProviderPortalEntities db)
        {
            SearchPhrase searchPhrase = new SearchPhrase();

            if (model.SearchPhraseId.HasValue)
            {
                searchPhrase = db.SearchPhrases.Find(model.SearchPhraseId);
                if (searchPhrase == null)
                {
                    return null;
                }
            }

            searchPhrase.Phrase = model.SearchPhrase;
            searchPhrase.RemovePhraseFromSearch = model.RemovePhraseFromSearch;

            if (model.SearchPhraseId.HasValue)
            {
                searchPhrase.ModifiedByUserId = Permission.GetCurrentUserId();
                searchPhrase.ModifiedDateTimeUtc = DateTime.UtcNow;
            }
            else
            {
                searchPhrase.CreatedByUserId = Permission.GetCurrentUserId();
                searchPhrase.CreatedDateTimeUtc = DateTime.UtcNow;
                Int32 nextOrdinal = 1;
                SearchPhrase maxOrdinal = db.SearchPhrases.OrderByDescending(x => x.Ordinal).FirstOrDefault();
                if (maxOrdinal != null)
                {
                    nextOrdinal = maxOrdinal.Ordinal + 1;
                }
                searchPhrase.Ordinal = nextOrdinal;
                searchPhrase.RecordStatusId = (Int32)Constants.RecordStatus.Live;
            }

            // Remove any existing and not selected Qualification Levels
            List<QualificationLevel> existingQualificationLevels = searchPhrase.QualificationLevels.ToList();
            foreach (QualificationLevel qualificationLevel in existingQualificationLevels.Where(x => !model.SelectedQualificationLevels.Contains(x.QualificationLevelId)))
            {
                searchPhrase.QualificationLevels.Remove(qualificationLevel);
            }

            // Add any new Qualification Levels
            foreach (Int32 qlId in model.SelectedQualificationLevels)
            {
                QualificationLevel ql = searchPhrase.QualificationLevels.FirstOrDefault(x => x.QualificationLevelId == qlId);
                if (ql == null)
                {
                    ql = db.QualificationLevels.Find(qlId);
                    if (ql != null)
                    {
                        searchPhrase.QualificationLevels.Add(ql);
                    }
                }
            }

            // Remove any existing and not selected Study Modes
            List<StudyMode> existingStudyModes = searchPhrase.StudyModes.ToList();
            foreach (StudyMode studyMode in existingStudyModes.Where(x => !model.SelectedStudyModes.Contains(x.StudyModeId)))
            {
                searchPhrase.StudyModes.Remove(studyMode);
            }

            // Add any new Study Modes
            foreach (Int32 smId in model.SelectedStudyModes)
            {
                StudyMode sm = searchPhrase.StudyModes.FirstOrDefault(x => x.StudyModeId == smId);
                if (sm == null)
                {
                    sm = db.StudyModes.Find(smId);
                    if (sm != null)
                    {
                        searchPhrase.StudyModes.Add(sm);
                    }
                }
            }

            // Remove any existing and not selected Attendance Types
            List<AttendanceType> existingAttendanceTypes = searchPhrase.AttendanceTypes.ToList();
            foreach (AttendanceType attendanceType in existingAttendanceTypes.Where(x => !model.SelectedAttendanceTypes.Contains(x.AttendanceTypeId)))
            {
                searchPhrase.AttendanceTypes.Remove(attendanceType);
            }

            // Add any new Attendance Types
            foreach (Int32 atId in model.SelectedAttendanceTypes)
            {
                AttendanceType at = searchPhrase.AttendanceTypes.FirstOrDefault(x => x.AttendanceTypeId == atId);
                if (at == null)
                {
                    at = db.AttendanceTypes.Find(atId);
                    if (at != null)
                    {
                        searchPhrase.AttendanceTypes.Add(at);
                    }
                }
            }

            // Remove any existing and not selected Attendance Patterns
            List<AttendancePattern> existingAttendancePatterns = searchPhrase.AttendancePatterns.ToList();
            foreach (AttendancePattern attendancePattern in existingAttendancePatterns.Where(x => !model.SelectedAttendancePatterns.Contains(x.AttendancePatternId)))
            {
                searchPhrase.AttendancePatterns.Remove(attendancePattern);
            }

            // Add any new Attendance Patterns
            foreach (Int32 apId in model.SelectedAttendancePatterns)
            {
                AttendancePattern ap = searchPhrase.AttendancePatterns.FirstOrDefault(x => x.AttendancePatternId == apId);
                if (ap == null)
                {
                    ap = db.AttendancePatterns.Find(apId);
                    if (ap != null)
                    {
                        searchPhrase.AttendancePatterns.Add(ap);
                    }
                }
            }

            return searchPhrase;
        }
    }
}