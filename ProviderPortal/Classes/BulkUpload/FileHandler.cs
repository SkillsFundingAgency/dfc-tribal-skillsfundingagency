using System;
using System.IO;
using System.Web;
//using Tribal.SkillsFundingAgency.ProviderPortal.BulkUpload.Validators;
using Tribal.SkillsFundingAgency.ProviderPortal.Models;

namespace Tribal.SkillsFundingAgency.ProviderPortal.BulkUpload
{
    public class FileHandler //: BaseBulkUploadValidator
    {
        private readonly HttpPostedFileBase _file;
        private readonly BulkUploadViewModel _model;

        public FileHandler(BulkUploadViewModel model)
        {
            _model = model;
            _file = model.File;
        }

        public void CopyFileToDataStore()
        {
            //if it is override exception process, not required to copy file
            if (!_model.OverrideException)
            {
                var fullFilePath = String.Format(@"{0}{1}{2}_{3}{4}",
                                                 Constants.ConfigSettings.BulkUploadVirtualDirectoryName,
                                                 !Constants.ConfigSettings.BulkUploadVirtualDirectoryName.EndsWith(@"\") ? @"\" : @"",
                                                 Path.GetFileName(_file.FileName),
                                                 DateTime.Now.ToString("ddMMMyyyyhhmmss"),
                                                 Path.GetExtension(_file.FileName));

                _file.SaveAs(fullFilePath);

                _model.Summary.FilePath = fullFilePath;
                _model.Summary.FileName = Path.GetFileName(_file.FileName);
                _model.Summary.ContentLength = _file.ContentLength;

                _model.Summary.TargetFileUrl = fullFilePath;
            }
        }

        //public override bool Validate()
        //{
        //    return true;
        //}
    }
}