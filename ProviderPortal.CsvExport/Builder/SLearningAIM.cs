using System;
using System.IO;
using System.Reflection;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;

// ReSharper disable once CheckNamespace

namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder
{
    public class SLearningAIM : BuilderBase
    {
        private readonly Action<string> _logger;

        public SLearningAIM(Action<string> logger)
        {
            _logger = logger;
        }

        public override void GenerateCsv()
        {
            var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var fileName = string.Format("{0}\\{1}", workingDirectory, "\\Data\\S_LEARNING_AIMS.csv");

            if (File.Exists(fileName))
            {
                File.Copy(fileName, Constants.S_Learning_AIM_CsvFilename, true);

                _logger("Creating CSV for Learning AIM.");
            }
            else
            {
                throw new Exception(string.Format("Unable to find source file {0}", fileName));
            }
        }
    }
}
