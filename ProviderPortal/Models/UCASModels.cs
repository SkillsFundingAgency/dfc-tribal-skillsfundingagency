using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class UploadUCASDataModel
    {
        [LanguageDisplay("Last Uploaded By")]
        public String LastUploadedBy { get; set; }

        [LanguageDisplay("Last Updated Date/Time")]
        [DateDisplayFormat(Format = DateFormat.ShortDateTime)]
        public DateTime? LastUploadDateTimeUtc { get; set; }

        [LanguageDisplay("Last Updated Date/Time")]
        [DataType(DataType.DateTime)]
        [DateDisplayFormat(Format = DateFormat.ShortDateTime)]
        public DateTime? LastUploadDateTimeLocalTime
        {
            get
            {
                if (LastUploadDateTimeUtc.HasValue)
                {
                    return LastUploadDateTimeUtc.Value.ToLocalTime();
                }

                return null;
            }
        }
        [LanguageDisplay("Last Uploaded File Name")]
        public String LastUploadFileName { get; set; }

        [Required(ErrorMessage = "Please specify a ZIP file.")]
        public HttpPostedFileBase File { get; set; }
    }
}