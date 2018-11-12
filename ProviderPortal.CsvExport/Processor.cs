using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using CsvHelper;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Builder;
using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport.Classes;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal.CsvExport
{
    public class Processor
    {
        private readonly ProviderPortalEntities _db;
        readonly Action<string> _logger;

        public Processor()
        {
            Constants.ConfigSettings = new Classes.ConfigurationSettings();

            _logger = new Action<string>(Log);

            _logger("**************Starting CSV Export***************");

            _logger("Loading Entities");

            _db = new ProviderPortalEntities();
            _db.Database.CommandTimeout = 300; /* 5 minutes */

            _logger("Entities Loaded");

            ManageLogFile();
        }

        public void Process()
        {
            try
            {
                foreach (var builder in LoadCsvBuilders(_db))
                {
                    builder.GenerateCsv();
                }

                BuilderBase.Compress(_logger);

                // Retain only the last x files (based on config setting)
                foreach (FileInfo fi in new DirectoryInfo(Constants.ConfigSettings.NightlyCsvFilesDirectoryLocation).GetFiles("*.zip").OrderByDescending(x => x.CreationTimeUtc).Skip(Constants.ConfigSettings.NumberOfZipFilesToRetain))
                {
                    fi.Delete();
                }
            }
            catch (Exception ex)
            {
                NotifyException(ex);
                _logger(string.Concat(ex.Message, ex.StackTrace));
            }
        }

        public List<BuilderBase> LoadCsvBuilders(ProviderPortalEntities db)
        {
            var builders = new List<BuilderBase>
            {
                new CProviders(_db, _logger),
                new CVenues(_db, _logger),
                new OCourses(db, _logger),
                new OOpportunityA10(db, _logger),
                new OOpportunityStartDates(db, _logger),
                new OOpportunities(db, _logger),
                //new WCourseBrowse(db, _logger),
                //new WProviderSearch(db, _logger),
                //new WSearchText(db, _logger),
                //Removed as part of TFS 136319
                //new SLearningAIM(_logger)
            };

            return builders;
        }

        private static void NotifyException(Exception ex)
        {
            //AppGlobal.EmailQueue.AddToSendQueue(TemplatedEmail.EmailMessage("5707fe90-10b8-4761-829d-3c6822997028",
            //                                                                Constants.EmailTemplates.BukUploadSummary,
            //                                                                new List<EmailParameter>
            //                                                                {
            //                                                                    new EmailParameter("%TABLEDATA%", ex.InnerException.ToString())
            //                                                                }));

        }

        private static void Log(string message)
        {
            using (Stream stream = File.Open(Constants.LogFileName, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    var csv = new CsvWriter(writer);
                    csv.WriteField<string>(string.Format("{0} - {1}", DateTime.Now, message));
                    csv.NextRecord();
                }
            }
        }

        private static void ManageLogFile()
        {
            if (File.Exists(Constants.LogFileName))
            {
                FileInfo f = new FileInfo(Constants.LogFileName);
                var filesize = f.Length / 1024000;

                int configMaxSize;
                int.TryParse(ConfigurationManager.AppSettings["MaxLogFileSizeInMB"], out configMaxSize);

                if (filesize >= configMaxSize)
                    File.Delete(Constants.LogFileName);
            }
        }
    }
}