// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageManager.cs" company="Tribal">
//   Copyright Tribal. All rights reserved.
// </copyright>
// <summary>
//   Language manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Mvc;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Ajax.Utilities;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

// ReSharper disable once CheckNamespace
namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    /// <summary>
    ///  Utility methods to interact with the language system.
    /// </summary>
    public class LanguageManager
    {
        /// <summary>
        /// The database
        /// </summary>
        private readonly ProviderPortalEntities _db;

        private const string CachedPrefix = "Cached_Language_|{0}|";

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageManager"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public LanguageManager(ProviderPortalEntities db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets a select list of available languages.
        /// </summary>
        /// <param name="languageId">The selected language identifier.</param>
        /// <returns></returns>
        public List<SelectListItem> GetLanguageSelectList(int? languageId)
        {
            return
                _db.up_LanguageList(null, null)
                    .Select(
                        x =>
                            new SelectListItem
                            {
                                Value = x.LanguageID.ToString(CultureInfo.InvariantCulture),
                                Text = x.DisplayName,
                                Selected = languageId != null && x.LanguageID == languageId
                            })
                    .ToList();
        }

        /// <summary>
        /// Get the list of available languages.
        /// </summary>
        /// <returns>A list of languages.</returns>
        public List<up_LanguageList_Result> GetLanguages()
        {
            return _db.up_LanguageList(null, null).ToList();
        }

        /// <summary>
        /// Prepare language for export.
        /// </summary>
        /// <param name="languageId">The language to export.</param>
        /// <returns>A language export model.</returns>
        public LanguageExportModel GenerateCsvLanguageFileBytes(int languageId)
        {
            using (var stream = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(stream, new UTF8Encoding(true)))
                {
                    var language = GetLanguageForExport(languageId);
                    var languageName = _db.up_LanguageList(languageId, null).First().DisplayName;
                    var csv = new CsvWriter(writer);
                    csv.WriteHeader(typeof (LanguageEntry));
                    csv.WriteRecords(language);
                    writer.Flush();
                    return new LanguageExportModel
                    {
                        FileName = String.Format("Language_{0}_{1}.csv",
                            languageName,
                            DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd")),
                        DisplayName = languageName,
                        Bytes = stream.ToArray()
                    };
                }
            }
        }

        /// <summary>
        /// Create a new language.
        /// </summary>
        /// <param name="newLanguageName">The language name (English, French)</param>
        /// <param name="newLanguageIetf">The two-letter IETF code for the language (en, fr)</param>
        /// <returns>True when language creation succeeds.</returns>
        public bool CreateNewLanguage(string newLanguageName, string newLanguageIetf)
        {
            var languages = _db.up_LanguageList(null, null);
            if (
                languages.Any(
                    x =>
                        x.IETF.Equals(newLanguageIetf, StringComparison.CurrentCultureIgnoreCase) ||
                        x.DefaultText.Equals(newLanguageName, StringComparison.CurrentCultureIgnoreCase)))
            {
                return false;
            }

            CultureInfo ci = null;
            try
            {
                ci = CultureInfo.GetCultureInfo(newLanguageIetf);
            }
            catch (CultureNotFoundException) { }

            var language = new Language
            {
                DefaultText = newLanguageName,
                IETF = newLanguageIetf,
                SqlLanguageId = ci == null ? (int?)null : ci.TextInfo.LCID,
                IsDefaultLanguage = false,
            };
            _db.Languages.Add(language);
            _db.SaveChanges();
            language.LanguageFieldName = String.Format("Table_Language_{0}", language.LanguageID);
            _db.SaveChanges();

            return true;
        }

        /// <summary>
        /// Get the full field name and text got all language entries for the specified language.
        /// </summary>
        /// <param name="languageId">The language ID</param>
        /// <returns>A list of language entries</returns>
        private IEnumerable<LanguageEntry> GetLanguageForExport(int languageId)
        {
            return
                _db.up_LanguageTextListByLanguageId(languageId, null)
                    .Select(x => new LanguageEntry {FieldName = x.FieldName, LanguageText = x.LanguageText})
                    .OrderBy(x => x.FieldName)
                    .ToList();
        }

        /// <summary>
        /// Processes an uploaded csv file of language entries into the database
        /// </summary>
        /// <param name="languageId">The language to update</param>
        /// <param name="fileUploadStream">The uploaded file containing the language entries</param>
        /// <returns>True if successful</returns>
        public bool ProcessUploadedCsvLanguageFile(int languageId, Stream fileUploadStream)
        {
            bool bReturn = false;

            using (var sr = new StreamReader(fileUploadStream))
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        using (var csv = new CsvReader(sr, new CsvConfiguration { HasHeaderRecord = true }))
                        {
                            var items = csv.GetRecords<LanguageEntry>();

                            foreach (var item in items)
                            {
                                _db.up_LanguageTextSetByQualifiedFieldName(languageId, item.FieldName, null,
                                    item.LanguageText);
                            }
                        }
                        _db.SaveChanges();
                        transaction.Complete();
                        bReturn = true;

                        new LanguageManager(_db).ReloadLanguage(languageId);
                    }
                    catch (CsvMissingFieldException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        //TODO Log exception
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }

            return bReturn;
        }

        /// <summary>
        /// Flushes the cached dictionary for the language Id forcing new language data to be loaded from the database. Also notifies other servers in the farm to reload language data.
        /// </summary>
        /// <param name="languageId">The language Id to reload</param>
        public void ReloadLanguage(int languageId)
        {
            string cacheKey = languageId > 0
                ? string.Format(CachedPrefix, languageId)
                : CachedPrefix.Replace("|{0}|", string.Empty);
            CacheManagement.CacheHandler.Invalidate(cacheKey);
        }
    }
}