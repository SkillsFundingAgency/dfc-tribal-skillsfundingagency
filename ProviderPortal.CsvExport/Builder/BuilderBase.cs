using System;
using System.IO;
using System.IO.Compression;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder
{
    public abstract class BuilderBase
    {
        public abstract void GenerateCsv();

        public static void Compress(Action<string> _logger)
        {
            _logger("Starting Compression CSV Files.");

            //Create target directory if its doesnot exists
            if (File.Exists(Constants.NightlyCsvZipFileNamewithAbsolutePath))
            {
                _logger("Deleting existing Zip Files.");

                File.Delete(Constants.NightlyCsvZipFileNamewithAbsolutePath);
            }

            //Creates zip from source directory
            _logger("Creating Compressed ZIP.");

            ZipFile.CreateFromDirectory(Constants.NightlyCsvZipFolderPath, Constants.NightlyCsvZipFileNamewithAbsolutePath);
            
            _logger("ZIP Completed.");

            //Remove csv directory
            _logger("Deleting existing unzipped Files.");
            Directory.Delete(Constants.NightlyCsvZipFolderPath,true);
        }
        
    }
}
