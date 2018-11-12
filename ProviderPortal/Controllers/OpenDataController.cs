using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Controllers
{
    using Tribal.SkillsFundingAgency.ProviderPortal.Classes;

    public class OpenDataController : BaseController
    {
        //
        // GET: /OpenData/
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<OpenDataListFilesModel> model = new List<OpenDataListFilesModel>();
            try
            {
                foreach (String file in Directory.GetFiles(Constants.ConfigSettings.NightlyCsvFilesDirectoryLocation, "*.zip"))
                {
                    try
                    {
                        FileInfo fi = new FileInfo(file);
                        OpenDataListFilesModel m = new OpenDataListFilesModel
                        {
                            FileName = fi.FullName,
                            CreatedDateTime = fi.CreationTime,
                            FileLength = fi.Length,
                            FileExtension = fi.Extension
                        };
                        m.NumberOfTimesDownloaded = db.OpenDataDownloads.Count(x => x.Filename == m.FileNameWithoutFolder);
                        model.Add(m);
                    }
                    catch {}
                }

            }
            catch {}

            // Sort the files so that the most recent is at the top
            model.Sort();

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Download(String fileName)
        {
            // If the file passed in has a \ in it then return nothing.
            if (!fileName.IsPathSafe())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            // Get the full path
            String fullFileName = Constants.ConfigSettings.NightlyCsvFilesDirectoryLocation;
            if (!fullFileName.EndsWith(@"\"))
            {
                fullFileName += @"\";
            }
            fullFileName += fileName;

            if (!fullFileName.IsValidPath()) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            // If the file doesn't exist the return nothing
            if (!System.IO.File.Exists(fullFileName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            try
            {
                FileInfo fi = new FileInfo(Constants.ConfigSettings.NightlyCsvFilesDirectoryLocation + @"\" + fileName);
                if (fi.Extension.ToLower() != ".zip")
                {
                    // Requested file is not a zip file - return nothing
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
            }
            catch
            {
                // An error happened - return nothing
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            // Log the fact that someone downloaded the file
            OpenDataDownload odd = new OpenDataDownload
            {
                DateTimeUtc = DateTime.UtcNow,
                IPAddress = Request.UserHostAddress,
                Filename = fileName
            };
            db.OpenDataDownloads.Add(odd);
            db.SaveChanges();

            // Hopefully the file exists and is a file we are prepared to send
            var fileBytes = System.IO.File.ReadAllBytes(fullFileName);
            var response = new FileContentResult(fileBytes, "application/x-zip-compressed") { FileDownloadName = fileName };
            return response;
        }
	}
}