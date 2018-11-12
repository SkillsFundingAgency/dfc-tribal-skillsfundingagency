using System.Linq;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Classes
{
    using System.IO;

    public static class FilePathHelper
    {
        public static bool IsPathSafe(this string pathComponent)
        {
            var invalidInPath = Path.GetInvalidPathChars();
            var invalidInFilename = Path.GetInvalidFileNameChars();
            var invalid = invalidInPath.Concat(invalidInFilename);
            return invalid.All(ch => !pathComponent.Contains(ch));
        }

        public static bool IsValidPath(this string filePath)
        {
            return filePath == Path.GetFullPath(filePath);
        }
    }
}