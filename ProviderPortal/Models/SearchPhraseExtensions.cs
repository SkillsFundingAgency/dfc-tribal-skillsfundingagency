using System;
using System.Data.Entity;
using System.Linq;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class SearchPhraseExtensions
    {
        /// <summary>
        /// Archives the <see cref="SearchPhrase"/> and also manages status of it's associated <see cref="Course"/>
        /// </summary>
        /// <param name="SearchPhrase">The <see cref="SearchPhrase"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Archive(this SearchPhrase SearchPhrase, ProviderPortalEntities db)
        {
            SearchPhrase.RecordStatusId = (Int32)Constants.RecordStatus.Archived;
            SearchPhrase.ModifiedByUserId = Permission.GetCurrentUserId();
            SearchPhrase.ModifiedDateTimeUtc = DateTime.UtcNow;
            db.Entry(SearchPhrase).State = EntityState.Modified;
        }

        /// <summary>
        /// Unarchives the <see cref="SearchPhrase"/> and also manages status of it's associated <see cref="Course"/>
        /// </summary>
        /// <param name="SearchPhrase">The <see cref="SearchPhrase"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Unarchive(this SearchPhrase SearchPhrase, ProviderPortalEntities db)
        {
            SearchPhrase.RecordStatusId = (Int32)Constants.RecordStatus.Live;
            SearchPhrase.ModifiedByUserId = Permission.GetCurrentUserId();
            SearchPhrase.ModifiedDateTimeUtc = DateTime.UtcNow;
            db.Entry(SearchPhrase).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the <see cref="SearchPhrase"/> and also manages status of it's associated <see cref="Course"/>
        /// </summary>
        /// <param name="SearchPhrase">The <see cref="SearchPhrase"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Delete(this SearchPhrase SearchPhrase, ProviderPortalEntities db)
        {
            foreach (QualificationLevel qualLevel in SearchPhrase.QualificationLevels.ToList())
            {
                SearchPhrase.QualificationLevels.Remove(qualLevel);
            }

            foreach (StudyMode studyMode in SearchPhrase.StudyModes.ToList())
            {
                SearchPhrase.StudyModes.Remove(studyMode);
            }

            foreach (AttendanceType attendanceType in SearchPhrase.AttendanceTypes.ToList())
            {
                SearchPhrase.AttendanceTypes.Remove(attendanceType);
            }

            foreach (AttendancePattern attendancePattern in SearchPhrase.AttendancePatterns.ToList())
            {
                SearchPhrase.AttendancePatterns.Remove(attendancePattern);
            }

            db.Entry(SearchPhrase).State = EntityState.Deleted;
        }
    }
}