using System;
using System.Data.Entity;
using System.Linq;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class VenueExtensions
    {
        /// <summary>
        /// Archives the <see cref="Venue"/> and also manages status of it's associated <see cref="CourseInstance"/>s
        /// </summary>
        /// <param name="venue">The <see cref="Venue"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        /// <param name="archiveCourseInstances">If true <see cref="CourseInstance"/>s assigned to this <see cref="Venue"/> will also be archived</param>
        public static void Archive(this Venue venue, ProviderPortalEntities db, Boolean archiveCourseInstances)
        {
            if (archiveCourseInstances)
            {
                foreach (CourseInstance courseInstance in venue.CourseInstances.ToList())
                {
                    courseInstance.Archive(db);
                }
            }

            venue.RecordStatusId = (Int32)Constants.RecordStatus.Archived;
            db.Entry(venue).State = EntityState.Modified;
        }

        /// <summary>
        /// Unarchives the <see cref="Venue"/>
        /// </summary>
        /// <param name="venue">The <see cref="Venue"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Unarchive(this Venue venue, ProviderPortalEntities db)
        {
            venue.RecordStatusId = (Int32)Constants.RecordStatus.Live;
            db.Entry(venue).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the <see cref="Venue"/> and also archives it's associated <see cref="CourseInstance"/>s
        /// </summary>
        /// <param name="venue">The <see cref="Venue"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Delete(this Venue venue, ProviderPortalEntities db)
        {
            foreach (CourseInstance courseInstance in venue.CourseInstances.ToList())
            {
                courseInstance.Archive(db);
            }

            db.Entry(venue).State = EntityState.Deleted;
        }

        /// <summary>
        /// Deletes the <see cref="Venue"/> and also delete it's associated <see cref="Address"/> and <see cref="CourseInstance"/>
        /// </summary>
        /// <param name="venue"></param>
        /// <param name="db"></param>
        public static void DeleteCascade(this Venue venue, ProviderPortalEntities db)
        {
            venue.Address.Delete(db);
            
            db.Entry(venue).State = EntityState.Deleted;
        }
    }
}