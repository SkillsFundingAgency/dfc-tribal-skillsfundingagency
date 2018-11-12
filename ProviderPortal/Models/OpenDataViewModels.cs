using System;
using System.ComponentModel.DataAnnotations;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Models
{
    public class OpenDataListFilesModel : IComparable
    {
        [LanguageDisplay("File Name")]
        public String FileName { get; set; }

        [LanguageDisplay("Created Date")]
        [DateDisplayFormat(Format = DateFormat.LongDate)]
        public DateTime CreatedDateTime { get; set; }

        [LanguageDisplay("File Type")]
        public String FileExtension { get; set; }

        [LanguageDisplay("File Size")]
        public Int64 FileLength { get; set; }

        [LanguageDisplay("File Name")]
        public String FileNameWithoutFolder
        {
            get
            {
                if (!FileName.Contains(@"\"))
                {
                    return FileName;
                }
                return FileName.Substring(FileName.LastIndexOf(@"\", StringComparison.Ordinal) + 1);
            }
        }

        [LanguageDisplay("File Size")]
        public String FileLengthDescription
        {
            get
            {
                const Decimal OneKiloByte = 1024M;
                const Decimal OneMegaByte = OneKiloByte * 1024M;
                const Decimal OneGigaByte = OneMegaByte * 1024M;

                if (FileLength <= 0)
                {
                    return "0 Bytes";
                }

                String suffix;
                Decimal size = Convert.ToDecimal(FileLength);
                Int32 precision = 0;

                if (size > OneGigaByte)
                {
                    size /= OneGigaByte;
                    precision = 1;
                    suffix = "GB";
                }
                else if (size > OneMegaByte)
                {
                    size /= OneMegaByte;
                    suffix = "MB";
                }
                else if (size > OneKiloByte)
                {
                    size /= OneKiloByte;
                    suffix = "kB";
                }
                else
                {
                    suffix = " Byte" + (FileLength > 1 ? "s" : "");
                }

                return String.Format(String.Concat("{0:N", precision, "}{1}"), size, suffix);
            }
        }

        public Int32 NumberOfTimesDownloaded { get; set; }

        public int CompareTo(object obj)
        {
            OpenDataListFilesModel other = (OpenDataListFilesModel)obj;
            if (other.CreatedDateTime < this.CreatedDateTime)
            {
                return -1;
            }
            if (other.CreatedDateTime == this.CreatedDateTime)
            {
                return 0;
            }
            return 1;
        }
    }
}