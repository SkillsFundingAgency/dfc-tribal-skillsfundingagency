using System;
using System.Data.Entity;
using System.Linq;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class CourseExtensions
    {
        /// <summary>
        /// Archives the <see cref="Course"/> and also manages status of it's associated <see cref="CourseInstance"/>s
        /// </summary>
        /// <param name="course">The <see cref="Course"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Archive(this Course course, ProviderPortalEntities db)
        {
            foreach (CourseInstance courseInstance in course.CourseInstances.ToList())
            {
                courseInstance.Archive(db);
            }

            course.RecordStatusId = (Int32)Constants.RecordStatus.Archived;
            course.AddedByApplicationId = (Int32)Constants.Application.Portal;
            course.ModifiedDateTimeUtc = DateTime.UtcNow;
            course.ModifiedByUserId = Permission.GetCurrentUserId();
            db.Entry(course).State = EntityState.Modified;
        }

        /// <summary>
        /// Unarchives the <see cref="Course"/>
        /// </summary>
        /// <param name="course">The <see cref="Course"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Unarchive(this Course course, ProviderPortalEntities db)
        {
            course.RecordStatusId = (Int32)Constants.RecordStatus.Pending;
            course.AddedByApplicationId = (Int32)Constants.Application.Portal;
            course.ModifiedDateTimeUtc = DateTime.UtcNow;
            course.ModifiedByUserId = Permission.GetCurrentUserId();
            db.Entry(course).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the <see cref="Course"/> and also deletes it's associated <see cref="CourseInstance"/>s
        /// </summary>
        /// <param name="course">The <see cref="Course"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Delete(this Course course, ProviderPortalEntities db)
        {
            foreach (CourseInstance courseInstance in course.CourseInstances.ToList())
            {
                course.CourseInstances.Remove(courseInstance);
                courseInstance.Delete(db);
            }

            foreach (CourseLearnDirectClassification courseDirectClassification in course.CourseLearnDirectClassifications.ToList())
            {
                db.Entry(courseDirectClassification).State = EntityState.Deleted;
            }

            db.Entry(course).State = EntityState.Deleted;
        }
    }
}