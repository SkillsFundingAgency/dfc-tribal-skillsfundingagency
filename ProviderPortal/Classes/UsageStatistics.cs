// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Protocols.WSTrust;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.WebPages;

    using ProviderPortal;

    using Tribal.SkillsFundingAgency.ProviderPortal.Classes;

    public static class UsageStatistics
    {
        #region Static Properties

        #region Public

        /// <summary>
        /// The format string to use when formatting a date to 
        /// the foldername format used when storing files
        /// </summary>
        public static string FolderNameFormat = "yyyyMMdd";

        /// <summary>
        /// List of allowed file extensions for file uploads
        /// </summary>
        public static List<String> FileExtensionWhitelist = new List<string>
        {
            ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".csv"
        };

        #endregion

        #region Private

        /// <summary>
        /// Key for the file cache
        /// </summary>
        private static string CacheKey = "Report_UsageStatistics";

        /// <summary>
        /// Timespan for maintining the cache
        /// </summary>
        private static TimeSpan Ttl = new TimeSpan(0, 0, 5);

        /// <summary>
        /// Regex for interpreting a yyyyMMdd string as a date
        /// </summary>
        private static readonly Regex DecodeDate = new Regex("(?<year>[0-9]{4})(?<month>[0-9]{2})(?<day>[0-9]{2})", RegexOptions.Compiled);

        #endregion

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Get the Usage Statstics files and the folders they're stored in
        /// </summary>
        /// <param name="includeEmptyFolders">Return empty folders in the results - for admin users</param>
        /// <returns>Dictionary of folders to files</returns>
        public static Dictionary<DateTime, List<string>> GetAll(bool includeEmptyFolders = false)
        {
            var items = (Dictionary<DateTime, List<string>>)CacheManagement.CacheHandler.Get(CacheKey);
            if (items != null) return items;
            var path = GetFilePath();

            items = new Dictionary<DateTime, List<string>>();
            var dirs = Directory.GetDirectories(path).OrderByDescending(x => x);
            foreach (var dir in dirs)
            {
                var leaf = dir.Substring(dir.LastIndexOf('\\') + 1);
                var match = DecodeDate.Match(leaf);
                if (!match.Success) continue;
                try
                {
                    var files = Directory.GetFiles(dir).OrderBy(x => x);
                    var key = new DateTime(
                        int.Parse(match.Groups["year"].Value),
                        int.Parse(match.Groups["month"].Value),
                        int.Parse(match.Groups["day"].Value));
                    if (!files.Any())
                    {
                        if (includeEmptyFolders) items.Add(key, new List<string>());
                        continue;
                    }
                    items.Add(key, new List<string>());
                    foreach (var file in files)
                    {
                        leaf = file.Substring(file.LastIndexOf('\\') + 1);
                        items[key].Add(leaf);
                    }
                }
                catch
                {
                    // Skip
                }
            }
            CacheManagement.CacheHandler.Add(CacheKey, items, Ttl);
            return items;
        }

        /// <summary>
        /// Create a new folder in the file store, the folder name coming from the date
        /// </summary>
        /// <param name="newFolderName"></param>
        public static void CreateFolder(DateTime newFolderName)
        {
            var folder = String.Format("{0}\\{1}", GetFilePath(), newFolderName.ToString(FolderNameFormat));
            Directory.CreateDirectory(folder);
            CacheManagement.CacheHandler.Invalidate(CacheKey);
        }

        /// <summary>
        /// Delete a folder in the file store.  Valid for empty folders only.
        /// Deleting a non-empty folder will silently fail.
        /// </summary>
        /// <param name="folderName"></param>
        public static void DeleteFolder(string folderName)
        {
            if (!folderName.IsPathSafe()) throw new ArgumentException("Folder name is not valid");
            try
            {
                var folder = String.Format("{0}\\{1}", GetFilePath(), folderName);
                Directory.Delete(folder, false);
                CacheManagement.CacheHandler.Invalidate(CacheKey);
            }
            catch (IOException)
            {
                // silently surpress this exception - probably the directory isn't empty. No big deal.
            }
        }

        /// <summary>
        /// Delete a file in the file store
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        public static void DeleteFile(string folder, string filename)
        {
            if (!folder.IsPathSafe() || !filename.IsPathSafe()) throw new ArgumentException("Invalid folder or file name");

            try
            {
                var filepath = String.Format("{0}\\{1}\\{2}", GetFilePath(), folder, filename);
                File.Delete(filepath);
                CacheManagement.CacheHandler.Invalidate(CacheKey);
            }
            catch (IOException)
            {
                // silently surpress this exception - file in use
            }
            catch (UnauthorizedAccessException)
            {
                // silently surpress this exception - file is "readonly"?
            }
        }

        /// <summary>
        /// Save an uploaded usage statistics file to the file space
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="file"></param>
        public static void AddFile(string folder, HttpPostedFileBase file)
        {
            if (!folder.IsPathSafe()) throw new ArgumentException("InvalidFolderName");
            var filepath = String.Format("{0}\\{1}\\{2}", GetFilePath(), folder, Path.GetFileName(file.FileName));
            if (!FileExtensionWhitelist.Contains(Path.GetExtension(filepath))) throw new ArgumentException("FileExtensionNotAllowed");
            if (File.Exists(filepath)) throw new ArgumentException("DuplicateFileWarning");
            if (!FileIsVirusFree(file)) throw new InvalidDataException();
            file.SaveAs(filepath);
            CacheManagement.CacheHandler.Invalidate(CacheKey);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Virus scan an uploaded file
        /// </summary>
        /// <param name="file"></param>
        /// <returns>True is the file is OK</returns>
        public static bool FileIsVirusFree(HttpPostedFileBase file)
        {
            var sophosExePath = Constants.ConfigSettings.VirusScanPath;
            var virusScan = new VirusScan(sophosExePath, VirusScanEngineType.Sophos);
            var bytes = new byte[file.ContentLength];
            file.InputStream.Read(bytes, 0, Convert.ToInt32(file.ContentLength));

            try
            {
                return virusScan.Scan(bytes);
            }
            catch (FileNotFoundException)
            {
                // av not installed or misconfigured
                AppGlobal.Log.WriteWarning("Virus scanner not installed or misconfigured.");
                return true;
            }
        }

        /// <summary>
        /// Get a byte array representing a single file
        /// </summary>
        /// <param name="folder">The folder the file ius stored in</param>
        /// <param name="filename">The name of the file</param>
        /// <returns></returns>
        public static byte[] GetFile(string folder, string filename)
        {
            if (folder.IsPathSafe() && filename.IsPathSafe())
            {
                var filepath = String.Format("{0}\\{1}\\{2}", GetFilePath(), folder, filename);
                if (filepath.IsValidPath())
                {
                    return File.ReadAllBytes(filepath);
                }
                else
                {
                    throw new ArgumentException("File path is not valid");
                }
            }
            else
            {
                throw new ArgumentException("Request contains invalid characters");
            }
        }

        /// <summary>
        /// Get the path to the file location on disk
        /// </summary>
        /// <returns></returns>
        private static string GetFilePath()
        {
            var path = Constants.ConfigSettings.UsageStatisticsFilesLocation;
            path = path.Replace("%BASE%", AppDomain.CurrentDomain.BaseDirectory);
            return path;
        }

        #endregion
    }
}