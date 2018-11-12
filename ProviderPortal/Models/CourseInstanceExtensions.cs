using System;
using System.Data.Entity;
using System.Linq;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class CourseInstanceExtensions
    {
        /// <summary>
        /// Checks whether the <see cref="Course"/>'s status need to be set to Pending and sets it if required
        /// </summary>
        /// <param name="courseInstance">The <see cref="CourseInstance"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        private static void ChangeCourseStatusToPending(CourseInstance courseInstance, ProviderPortalEntities db)
        {
            // If there are no other LIVE opportunities for this course and the course is currently LIVE then set the course status to Pending
            Course course = courseInstance.Course;
            if (course.RecordStatusId == (Int32) Constants.RecordStatus.Live)
            {
                if (course.CourseInstances.Count(x => x.RecordStatusId == (Int32) Constants.RecordStatus.Live && x.CourseInstanceId != courseInstance.CourseInstanceId) == 0)
                {
                    course.RecordStatusId = (Int32) Constants.RecordStatus.Pending;
                    course.AddedByApplicationId = (Int32)Constants.Application.Portal;
                    course.ModifiedByUserId = Permission.GetCurrentUserId();
                    course.ModifiedDateTimeUtc = DateTime.UtcNow;
                    db.Entry(course).State = EntityState.Modified;
                }
            }
        }

        /// <summary>
        /// Archives the <see cref="CourseInstance"/> and also manages status of it's associated <see cref="Course"/>
        /// </summary>
        /// <param name="courseInstance">The <see cref="CourseInstance"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Archive(this CourseInstance courseInstance, ProviderPortalEntities db)
        {
            // Check whether course status should be changed to pending
            ChangeCourseStatusToPending(courseInstance, db);

            courseInstance.RecordStatusId = (Int32)Constants.RecordStatus.Archived;
            courseInstance.AddedByApplicationId = (Int32)Constants.Application.Portal;
            courseInstance.ModifiedByUserId = Permission.GetCurrentUserId();
            courseInstance.ModifiedDateTimeUtc = DateTime.UtcNow;
            db.Entry(courseInstance).State = EntityState.Modified;
        }

        /// <summary>
        /// Unarchives the <see cref="CourseInstance"/> and also manages status of it's associated <see cref="Course"/>
        /// </summary>
        /// <param name="courseInstance">The <see cref="CourseInstance"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Unarchive(this CourseInstance courseInstance, ProviderPortalEntities db)
        {
            // Set the course to LIVE if not currently LIVE
            Course course = courseInstance.Course;
            if (course.RecordStatusId != (Int32)Constants.RecordStatus.Live)
            {
                course.RecordStatusId = (Int32) Constants.RecordStatus.Live;
                course.AddedByApplicationId = (Int32)Constants.Application.Portal;
                course.ModifiedByUserId = Permission.GetCurrentUserId();
                course.ModifiedDateTimeUtc = DateTime.UtcNow;
                db.Entry(course).State = EntityState.Modified;
            }

            courseInstance.RecordStatusId = (Int32)Constants.RecordStatus.Live;
            courseInstance.AddedByApplicationId = (Int32)Constants.Application.Portal;
            courseInstance.ModifiedByUserId = Permission.GetCurrentUserId();
            courseInstance.ModifiedDateTimeUtc = DateTime.UtcNow;
            db.Entry(courseInstance).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the <see cref="CourseInstance"/> and also manages status of it's associated <see cref="Course"/>
        /// </summary>
        /// <param name="courseInstance">The <see cref="CourseInstance"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Delete(this CourseInstance courseInstance, ProviderPortalEntities db)
        {
            // Check whether course status should be changed to pending
            ChangeCourseStatusToPending(courseInstance, db);

            foreach (A10FundingCode a10 in courseInstance.A10FundingCode.ToList())
            {
                courseInstance.A10FundingCode.Remove(a10);
            }

            foreach (CourseInstanceStartDate sd in courseInstance.CourseInstanceStartDates.ToList())
            {
                courseInstance.CourseInstanceStartDates.Remove(sd);
                db.Entry(sd).State = EntityState.Deleted;
            }

            foreach (Venue venue in courseInstance.Venues.ToList())
            {
                courseInstance.Venues.Remove(venue);
            }

            db.Entry(courseInstance).State = EntityState.Deleted;
        }
    }
}