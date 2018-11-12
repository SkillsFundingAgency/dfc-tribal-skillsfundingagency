using System;
using System.Web;
using System.Xml.Linq;

namespace TinyFileManager.NET
{
    public class clsConfig
    {
        #region Private Variables

        private clsProfile pobjProfile = new clsProfile();

        #endregion

        #region Settings Properties

        /// <summary>
        ///     Max upload filesize in Mb
        /// </summary>
        public int intMaxUploadSizeMb
        {
            get
            {
                if (HttpContext.Current.Session["TFM_MaxUploadSizeMb"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["TFM_MaxUploadSizeMb"]);
                }
                return Convert.ToInt32(pobjProfile.MaxUploadSizeMb);
            }
        }

        /// <summary>
        ///     Allowed image file extensions
        /// </summary>
        public string strAllowedImageExtensions
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowedImageExtensions"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_AllowedImageExtensions"]);
                }
                return pobjProfile.AllowedImageExtensions;
            }
        }

        /// <summary>
        ///     Allowed image file extensions as an array
        /// </summary>
        public string[] arrAllowedImageExtensions
        {
            get { return getArrayFromString(strAllowedImageExtensions); }
        }

        /// <summary>
        ///     Allowed document file extensions
        /// </summary>
        public string strAllowedFileExtensions
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowedFileExtensions"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_AllowedFileExtensions"]);
                }
                return pobjProfile.AllowedFileExtensions;
            }
        }

        /// <summary>
        ///     Allowed document file extensions as an array
        /// </summary>
        public string[] arrAllowedFileExtensions
        {
            get { return getArrayFromString(strAllowedFileExtensions); }
        }

        /// <summary>
        ///     Allowed video file extensions
        /// </summary>
        public string strAllowedVideoExtensions
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowedVideoExtensions"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_AllowedVideoExtensions"]);
                }
                return pobjProfile.AllowedVideoExtensions;
            }
        }

        /// <summary>
        ///     Allowed video file extensions as an array
        /// </summary>
        public string[] arrAllowedVideoExtensions
        {
            get { return getArrayFromString(strAllowedVideoExtensions); }
        }

        /// <summary>
        ///     Allowed music file extensions
        /// </summary>
        public string strAllowedMusicExtensions
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowedMusicExtensions"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_AllowedMusicExtensions"]);
                }
                return pobjProfile.AllowedMusicExtensions;
            }
        }

        /// <summary>
        ///     Allowed music file extensions as an array
        /// </summary>
        public string[] arrAllowedMusicExtensions
        {
            get { return getArrayFromString(strAllowedMusicExtensions); }
        }

        /// <summary>
        ///     Allowed misc file extensions
        /// </summary>
        public string strAllowedMiscExtensions
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowedMiscExtensions"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_AllowedMiscExtensions"]);
                }
                return pobjProfile.AllowedMiscExtensions;
            }
        }

        /// <summary>
        ///     Allowed misc file extensions as an array
        /// </summary>
        public string[] arrAllowedMiscExtensions
        {
            get { return getArrayFromString(strAllowedMiscExtensions); }
        }

        /// <summary>
        ///     All allowed file extensions
        /// </summary>
        public string strAllowedAllExtensions
        {
            get
            {
                var strRet = "";

                if (strAllowedImageExtensions.Length > 0)
                {
                    strRet = strAllowedImageExtensions;
                }
                if (strAllowedFileExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + strAllowedFileExtensions;
                    }
                    else
                    {
                        strRet = strAllowedFileExtensions;
                    }
                }
                if (strAllowedVideoExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + strAllowedVideoExtensions;
                    }
                    else
                    {
                        strRet = strAllowedVideoExtensions;
                    }
                }
                if (strAllowedMusicExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + strAllowedMusicExtensions;
                    }
                    else
                    {
                        strRet = strAllowedMusicExtensions;
                    }
                }
                if (strAllowedMiscExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + strAllowedMiscExtensions;
                    }
                    else
                    {
                        strRet = strAllowedMiscExtensions;
                    }
                }

                return strRet;
            }
        }

        /// <summary>
        ///     Returns document root
        /// </summary>
        public string strDocRoot
        {
            get
            {
                if (HttpContext.Current.Session["TFM_RootPath"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_RootPath"]).TrimEnd('\\');
                }
                if (pobjProfile.RootPath != "")
                {
                    return pobjProfile.RootPath.TrimEnd('\\');
                }
                return HttpContext.Current.Server.MapPath("/").TrimEnd('\\');
            }
        }

        /// <summary>
        ///     Returns the base url of the site
        /// </summary>
        public string strBaseURL
        {
            get
            {
                if (HttpContext.Current.Session["TFM_RootURL"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_RootURL"]).TrimEnd('/');
                }
                if (pobjProfile.RootURL != "")
                {
                    return pobjProfile.RootURL.TrimEnd('/');
                }
                return HttpContext.Current.Request.Url.Scheme + "://" +
                       HttpContext.Current.Request.Url.Authority.TrimEnd('/');
            }
        }

        /// <summary>
        ///     Returns the full upload drive path
        /// </summary>
        public string strUploadPath
        {
            get
            {
                if (HttpContext.Current.Session["TFM_UploadPath"] != null)
                {
                    return strDocRoot + "\\" +
                           Convert.ToString(HttpContext.Current.Session["TFM_UploadPath"]).TrimEnd('\\') + "\\";
                }
                return strDocRoot + "\\" + pobjProfile.UploadPath.TrimEnd('\\') + "\\";
            }
        }

        /// <summary>
        ///     Returns the full thumb drive path
        /// </summary>
        public string strThumbPath
        {
            get
            {
                if (HttpContext.Current.Session["TFM_ThumbPath"] != null)
                {
                    return strDocRoot + "\\" +
                           Convert.ToString(HttpContext.Current.Session["TFM_ThumbPath"]).TrimEnd('\\') + "\\";
                }
                return strDocRoot + "\\" + pobjProfile.ThumbPath.TrimEnd('\\') + "\\";
            }
        }

        /// <summary>
        ///     Returns the full upload url
        /// </summary>
        public string strUploadURL
        {
            get
            {
                if (HttpContext.Current.Session["TFM_UploadPath"] != null)
                {
                    return strBaseURL + "/" +
                           Convert.ToString(HttpContext.Current.Session["TFM_UploadPath"]).Replace('\\', '/');
                }
                return strBaseURL + "/" + pobjProfile.UploadPath.Replace('\\', '/');
            }
        }

        /// <summary>
        ///     Returns the full thumb url
        /// </summary>
        public string strThumbURL
        {
            get
            {
                if (HttpContext.Current.Session["TFM_ThumbPath"] != null)
                {
                    return strBaseURL + "/" +
                           Convert.ToString(HttpContext.Current.Session["TFM_ThumbPath"]).Replace('\\', '/');
                }
                return strBaseURL + "/" + pobjProfile.ThumbPath.Replace('\\', '/');
            }
        }

        /// <summary>
        ///     Returns the setting for a custom element to fill the selected item url
        /// </summary>
        public string strFillSelector
        {
            get
            {
                if (HttpContext.Current.Session["TFM_FillSelector"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_FillSelector"]);
                }
                return pobjProfile.FillSelector;
            }
        }

        /// <summary>
        ///     Returns the setting for custom code to close the popup
        /// </summary>
        public string strPopupCloseCode
        {
            get
            {
                if (HttpContext.Current.Session["TFM_PopupCloseCode"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["TFM_PopupCloseCode"]);
                }
                return pobjProfile.PopupCloseCode;
            }
        }

        /// <summary>
        ///     Returns the setting for allowing upload of file
        /// </summary>
        public bool boolAllowUploadFile
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowUploadFile"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["TFM_AllowUploadFile"]);
                    ;
                }
                return Convert.ToBoolean(pobjProfile.AllowUploadFile);
            }
        }

        /// <summary>
        ///     Returns the setting for allowing delete of file
        /// </summary>
        public bool boolAllowDeleteFile
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowDeleteFile"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["TFM_AllowDeleteFile"]);
                    ;
                }
                return Convert.ToBoolean(pobjProfile.AllowDeleteFile);
            }
        }

        /// <summary>
        ///     Returns the setting for allowing creation of folder
        /// </summary>
        public bool boolAllowCreateFolder
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowCreateFolder"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["TFM_AllowCreateFolder"]);
                    ;
                }
                return Convert.ToBoolean(pobjProfile.AllowCreateFolder);
            }
        }

        /// <summary>
        ///     Returns the setting for allowing delete of folder
        /// </summary>
        public bool boolAllowDeleteFolder
        {
            get
            {
                if (HttpContext.Current.Session["TFM_AllowDeleteFolder"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["TFM_AllowDeleteFolder"]);
                    ;
                }
                return Convert.ToBoolean(pobjProfile.AllowDeleteFolder);
            }
        }

        #endregion

        #region Constructors

        public clsConfig()
        {
            LoadConfig("Default");
        }

        public clsConfig(string strProfile)
        {
            if (strProfile == "")
            {
                LoadConfig("Default");
            }
            else
            {
                LoadConfig(strProfile);
            }
        }

        #endregion

        #region Private Routines

        private string[] getArrayFromString(string strInput)
        {
            string[] arrExt;
            string strTemp;

            //remove lead and trail single quotes so we can SPLIT the hell out of it
            strTemp = strInput.Trim('\'');
            arrExt = strTemp.Split(new[] {"'", ",", "'"}, StringSplitOptions.RemoveEmptyEntries);

            return arrExt;
        } // getArrayFromString

        private void LoadConfig(string strProfile)
        {
            string strConfig;
            XDocument objDoc;
            XElement objProfiles;

            strConfig = HttpContext.Current.Server.MapPath("~/web.config");
            objDoc = XDocument.Load(strConfig);
            objProfiles = objDoc.Element("configuration").Element("TFMProfiles");
            foreach (var objProfile in objProfiles.Descendants("profile"))
            {
                if (Convert.ToString(objProfile.Attribute("name").Value).ToLower() == strProfile.ToLower())
                {
                    pobjProfile = new clsProfile();
                    pobjProfile.AllowCreateFolder = Convert.ToBoolean(objProfile.Element("AllowCreateFolder").Value);
                    pobjProfile.AllowDeleteFile = Convert.ToBoolean(objProfile.Element("AllowDeleteFile").Value);
                    pobjProfile.AllowDeleteFolder = Convert.ToBoolean(objProfile.Element("AllowDeleteFolder").Value);
                    pobjProfile.AllowUploadFile = Convert.ToBoolean(objProfile.Element("AllowUploadFile").Value);
                    pobjProfile.AllowedFileExtensions = objProfile.Element("AllowedFileExtensions").Value;
                    pobjProfile.AllowedImageExtensions = objProfile.Element("AllowedImageExtensions").Value;
                    pobjProfile.AllowedMiscExtensions = objProfile.Element("AllowedMiscExtensions").Value;
                    pobjProfile.AllowedMusicExtensions = objProfile.Element("AllowedMusicExtensions").Value;
                    pobjProfile.AllowedVideoExtensions = objProfile.Element("AllowedVideoExtensions").Value;
                    pobjProfile.FillSelector = objProfile.Element("FillSelector").Value;
                    pobjProfile.MaxUploadSizeMb = Convert.ToInt16(objProfile.Element("MaxUploadSizeMb").Value);
                    pobjProfile.PopupCloseCode = objProfile.Element("PopupCloseCode").Value;
                    pobjProfile.RootPath = objProfile.Element("RootPath").Value;
                    pobjProfile.RootURL = objProfile.Element("RootURL").Value;
                    pobjProfile.ThumbPath = objProfile.Element("ThumbPath").Value;
                    pobjProfile.UploadPath = objProfile.Element("UploadPath").Value;
                    break;
                }
            } // foreach
        } // LoadConfig

        #endregion
    } // class
} // namespace