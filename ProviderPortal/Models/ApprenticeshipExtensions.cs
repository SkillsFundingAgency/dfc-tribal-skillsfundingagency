using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public static class ApprenticeshipExtensions
    {
        /// <summary>
        /// Archives the <see cref="Apprenticeship"/> and also manages status of it's associated <see cref="ApprenticeshipLocation"/>s
        /// </summary>
        /// <param name="apprenticeship">The <see cref="Apprenticeship"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Archive(this Apprenticeship apprenticeship, ProviderPortalEntities db)
        {
            foreach (ApprenticeshipLocation apprenticeshipLocation in apprenticeship.ApprenticeshipLocations.ToList())
            {
                apprenticeshipLocation.Archive(db);
            }

            apprenticeship.RecordStatusId = (Int32) Constants.RecordStatus.Archived;
            apprenticeship.AddedByApplicationId = (Int32) Constants.Application.Portal;
            apprenticeship.ModifiedDateTimeUtc = DateTime.UtcNow;
            apprenticeship.ModifiedByUserId = Permission.GetCurrentUserId();
            db.Entry(apprenticeship).State = EntityState.Modified;
        }

        /// <summary>
        /// Unarchives the <see cref="Apprenticeship"/>
        /// </summary>
        /// <param name="apprenticeship">The <see cref="Apprenticeship"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Unarchive(this Apprenticeship apprenticeship, ProviderPortalEntities db)
        {
            apprenticeship.RecordStatusId = (Int32) Constants.RecordStatus.Pending;
            apprenticeship.AddedByApplicationId = (Int32) Constants.Application.Portal;
            apprenticeship.ModifiedDateTimeUtc = DateTime.UtcNow;
            apprenticeship.ModifiedByUserId = Permission.GetCurrentUserId();
            db.Entry(apprenticeship).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the <see cref="Apprenticeship"/> and also deletes it's associated <see cref="CourseInstance"/>s-->
        /// </summary>
        /// <param name="apprenticeship">The <see cref="Course"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Delete(this Apprenticeship apprenticeship, ProviderPortalEntities db)
        {
            foreach (ApprenticeshipLocation apprenticeshipLocation in apprenticeship.ApprenticeshipLocations.ToList())
            {
                foreach (DeliveryMode deliveryMode in apprenticeshipLocation.DeliveryModes.ToList())
                {
                    apprenticeshipLocation.DeliveryModes.Remove(deliveryMode);
                }
                db.Entry(apprenticeshipLocation).State = EntityState.Deleted;
            }

            foreach (ApprenticeshipQACompliance qaCompliance in apprenticeship.ApprenticeshipQACompliances.ToList())
            {
                foreach (QAComplianceFailureReason complianceFailureReason in qaCompliance.QAComplianceFailureReasons.ToList())
                {
                    qaCompliance.QAComplianceFailureReasons.Remove(complianceFailureReason);
                }
                db.Entry(qaCompliance).State = EntityState.Deleted;
            }

            foreach (ApprenticeshipQAStyle qaStyle in apprenticeship.ApprenticeshipQAStyles.ToList())
            {
                foreach (QAStyleFailureReason styleFailureReason in qaStyle.QAStyleFailureReasons.ToList())
                {
                    qaStyle.QAStyleFailureReasons.Remove(styleFailureReason);
                }
                db.Entry(qaStyle).State = EntityState.Deleted;
            }

            db.Entry(apprenticeship).State = EntityState.Deleted;
        }

        public static Apprenticeship DecodeSearchFrameworkOrStandard(string frameworkOrStandardId)
        {
            if (frameworkOrStandardId == null) return null;
            var rx =
                new Regex(
                    "(F(?<FrameworkCode>[0-9]+)-(?<ProgType>[0-9]+)-(?<PathwayCode>[0-9]+))|(S(?<StandardCode>[0-9]+)-(?<Version>[0-9]+))",
                    RegexOptions.Compiled);
            var match = rx.Match(frameworkOrStandardId);
            if (match.Groups["FrameworkCode"].Success)
            {
                return new Apprenticeship
                {
                    FrameworkCode = Int32.Parse(match.Groups["FrameworkCode"].Value),
                    ProgType = Int32.Parse(match.Groups["ProgType"].Value),
                    PathwayCode = Int32.Parse(match.Groups["PathwayCode"].Value),
                };
            }
            if (match.Groups["StandardCode"].Success)
            {
                return new Apprenticeship
                {
                    StandardCode = Int32.Parse(match.Groups["StandardCode"].Value),
                    Version = Int32.Parse(match.Groups["Version"].Value),
                };
            }
            return new Apprenticeship();
        }

        public static Apprenticeship DecodeSearchFrameworkOrStandardByName(String frameworkOrStandardName)
        {
            if (frameworkOrStandardName == null)
            {
                return null;
            }

            ProviderPortalEntities _db = new ProviderPortalEntities();

            String[] nameParts = frameworkOrStandardName.Split('-');
            switch (nameParts.GetLength(0))
            {
                case 2: // Must be a standard

                    String standardSectorCode = nameParts[0].Trim();
                    String standardName = nameParts[1].Trim();

                    StandardSectorCode ssc = _db.StandardSectorCodes.FirstOrDefault(x => x.StandardSectorCodeDesc == standardSectorCode);
                    if (ssc != null)
                    {
                        Standard s = _db.Standards.FirstOrDefault(x => x.StandardName == standardName && x.StandardSectorCode == ssc.StandardSectorCodeId && x.RecordStatusId == (Int32) Constants.RecordStatus.Live);
                        if (s != null)
                        {
                            return new Apprenticeship
                            {
                                StandardCode = s.StandardCode,
                                Version = s.Version
                            };
                        }
                    }

                    break;

                case 3: // Must be a framework

                    String nasTitle = nameParts[0].Trim();
                    String progTypeDesc = nameParts[1].Trim();
                    String pathwayName = nameParts[2].Trim();

                    ProgType pt = _db.ProgTypes.FirstOrDefault(x => x.ProgTypeDesc == progTypeDesc);
                    if (pt != null)
                    {
                        Framework fw = _db.Frameworks.FirstOrDefault(x => x.NasTitle == nasTitle && x.ProgType == pt.ProgTypeId && x.PathwayName == pathwayName && x.RecordStatusId == (Int32)Constants.RecordStatus.Live);
                        if (fw != null)
                        {
                            return new Apprenticeship
                            {
                                FrameworkCode = fw.FrameworkCode,
                                ProgType = fw.ProgType,
                                PathwayCode = fw.PathwayCode
                            };
                        }
                    }

                    break;
            }

            return null;
        }

        public static string GetFrameworkOrStandardId(Apprenticeship apprenticeship)
        {
            if (apprenticeship == null) return null;
            if (apprenticeship.FrameworkCode != null)
            {
                return String.Format("F{0}-{1}-{2}", apprenticeship.FrameworkCode, apprenticeship.ProgType,
                    apprenticeship.PathwayCode);
            }
            if (apprenticeship.StandardCode != null)
            {
                return String.Format("S{0}-{1}", apprenticeship.StandardCode, apprenticeship.Version);
            }
            return null;
        }

        public static String FullTextString(this Apprenticeship apprenticeship)
        {
            if (apprenticeship.FrameworkCode.HasValue)
            {
                Framework framework = apprenticeship.Framework;
                return (String.IsNullOrEmpty(framework.PathwayName)
                    ? framework.NasTitle + " - " + framework.ProgType1.ProgTypeDesc
                    : framework.NasTitle + " - " + framework.PathwayName + " - " + framework.ProgType1.ProgTypeDesc);
            }

            Standard standard = apprenticeship.Standard;
            return standard.StandardSectorCode1.StandardSectorCodeDesc + " - "
                   + standard.StandardName + " - "
                   + AppGlobal.Language.GetText("Apprenticeship_Standard_Level", "Level") + " " + standard.NotionalEndLevel;
        }
    }

    public static class ApprenticeshipLocationExtensions
    {
        /// <summary>
        /// Checks whether the <see cref="Apprenticeship"/>'s status need to be set to Pending and sets it if required
        /// </summary>
        /// <param name="apprenticeshipLocation">The <see cref="ApprenticeshipLocation"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        private static void ChangeApprenticeshipStatusToPending(ApprenticeshipLocation apprenticeshipLocation,
            ProviderPortalEntities db)
        {
            // If there are no other LIVE delivery locations for this apprenticeship and the apprenticeship is currently LIVE then set the apprenticeship status to Pending
            Apprenticeship apprenticeship = apprenticeshipLocation.Apprenticeship;
            if (apprenticeship.RecordStatusId == (Int32) Constants.RecordStatus.Live)
            {
                if (
                    apprenticeship.ApprenticeshipLocations.Count(
                        x =>
                            x.RecordStatusId == (Int32) Constants.RecordStatus.Live &&
                            x.ApprenticeshipLocationId != apprenticeshipLocation.ApprenticeshipLocationId) == 0)
                {
                    apprenticeship.RecordStatusId = (Int32) Constants.RecordStatus.Pending;
                    apprenticeship.AddedByApplicationId = (Int32) Constants.Application.Portal;
                    apprenticeship.ModifiedByUserId = Permission.GetCurrentUserId();
                    apprenticeship.ModifiedDateTimeUtc = DateTime.UtcNow;
                    db.Entry(apprenticeship).State = EntityState.Modified;
                }
            }
        }

        /// <summary>
        /// Archives the <see cref="ApprenticeshipLocation"/> and also manages status of it's associated <see cref="Apprenticeship"/>
        /// </summary>
        /// <param name="apprenticeshipLocation">The <see cref="ApprenticeshipLocation"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Archive(this ApprenticeshipLocation apprenticeshipLocation, ProviderPortalEntities db)
        {
            // Check whether apprenticeship status should be changed to pending
            ChangeApprenticeshipStatusToPending(apprenticeshipLocation, db);

            apprenticeshipLocation.RecordStatusId = (Int32) Constants.RecordStatus.Archived;
            apprenticeshipLocation.AddedByApplicationId = (Int32) Constants.Application.Portal;
            apprenticeshipLocation.ModifiedByUserId = Permission.GetCurrentUserId();
            apprenticeshipLocation.ModifiedDateTimeUtc = DateTime.UtcNow;
            db.Entry(apprenticeshipLocation).State = EntityState.Modified;
        }

        /// <summary>
        /// Unarchives the <see cref="ApprenticeshipLocation"/> and also manages status of it's associated <see cref="Apprenticeship"/>
        /// </summary>
        /// <param name="apprenticeshipLocation">The <see cref="ApprenticeshipLocation"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Unarchive(this ApprenticeshipLocation apprenticeshipLocation, ProviderPortalEntities db)
        {
            // Set the apprenticeship to LIVE if not currently LIVE
            Apprenticeship apprenticeship = apprenticeshipLocation.Apprenticeship;
            if (apprenticeship.RecordStatusId != (Int32) Constants.RecordStatus.Live)
            {
                apprenticeship.RecordStatusId = (Int32) Constants.RecordStatus.Live;
                apprenticeship.AddedByApplicationId = (Int32) Constants.Application.Portal;
                apprenticeship.ModifiedByUserId = Permission.GetCurrentUserId();
                apprenticeship.ModifiedDateTimeUtc = DateTime.UtcNow;
                db.Entry(apprenticeship).State = EntityState.Modified;
            }

            apprenticeshipLocation.RecordStatusId = (Int32) Constants.RecordStatus.Live;
            apprenticeshipLocation.AddedByApplicationId = (Int32) Constants.Application.Portal;
            apprenticeshipLocation.ModifiedByUserId = Permission.GetCurrentUserId();
            apprenticeshipLocation.ModifiedDateTimeUtc = DateTime.UtcNow;
            db.Entry(apprenticeshipLocation).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the <see cref="ApprenticeshipLocation"/> and also manages status of it's associated <see cref="Apprenticeship"/>
        /// </summary>
        /// <param name="apprenticeshipLocation">The <see cref="ApprenticeshipLocation"/> object</param>
        /// <param name="db">The <see cref="ProviderPortalEntities"/> object</param>
        public static void Delete(this ApprenticeshipLocation apprenticeshipLocation, ProviderPortalEntities db)
        {
            // Check whether apprenticeship status should be changed to pending
            ChangeApprenticeshipStatusToPending(apprenticeshipLocation, db);

            foreach (DeliveryMode item in apprenticeshipLocation.DeliveryModes.ToList())
            {
                apprenticeshipLocation.DeliveryModes.Remove(item);
            }

            db.Entry(apprenticeshipLocation).State = EntityState.Deleted;
        }
    }
}