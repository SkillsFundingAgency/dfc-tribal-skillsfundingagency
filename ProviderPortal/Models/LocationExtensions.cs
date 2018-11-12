using System;
using System.Data.Entity;
using System.Linq;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class LocationExtensions
    {
        /// <summary>
        /// Archives the <see cref="Location"/> and also manages status of it's associated <see cref="CourseInstance"/>s
        /// </summary>
        /// <param name="location">The <see cref="Location"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        /// <param name="archiveApprenticeships">If true <see cref="Apprenticeship"/>s assigned to this <see cref="Location"/> will also be archived</param>
        public static void Archive(this Location location, ProviderPortalEntities db, Boolean archiveApprenticeships)
        {
            if (archiveApprenticeships)
            {
                foreach (ApprenticeshipLocation apprenticeshipLocation in location.ApprenticeshipLocations.ToList())
                {
                    apprenticeshipLocation.Archive(db);
                }
            }

            location.RecordStatusId = (Int32)Constants.RecordStatus.Archived;
            db.Entry(location).State = EntityState.Modified;
        }

        /// <summary>
        /// Unarchives the <see cref="Location"/>
        /// </summary>
        /// <param name="location">The <see cref="Location"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Unarchive(this Location location, ProviderPortalEntities db)
        {
            location.RecordStatusId = (Int32)Constants.RecordStatus.Live;
            db.Entry(location).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the <see cref="Location"/> and also archives it's associated <see cref="CourseInstance"/>s
        /// </summary>
        /// <param name="location">The <see cref="Location"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Delete(this Location location, ProviderPortalEntities db)
        {
            foreach (ApprenticeshipLocation apprenticeshipLocation in location.ApprenticeshipLocations.ToList())
            {
                apprenticeshipLocation.Delete(db);
            }

            db.Entry(location).State = EntityState.Deleted;
        }

        /// <summary>
        /// Deletes the <see cref="Location"/> and also delete it's associated <see cref="Address"/> and <see cref="CourseInstance"/>
        /// </summary>
        /// <param name="location"></param>
        /// <param name="db"></param>
        public static void DeleteCascade(this Location location, ProviderPortalEntities db)
        {
            location.Address.Delete(db);
            
            db.Entry(location).State = EntityState.Deleted;
        }
    }
}