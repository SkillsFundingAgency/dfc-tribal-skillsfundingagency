using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using Tribal.SkillsFundingAgency.ProviderPortal;
using Tribal.SkillsFundingAgency.ProviderPortal.Classes;

namespace TinyFileManager.NET
{
    public partial class dialog : Page
    {
        private string[] arrFiles;
        private string[] arrFolders;
        public ArrayList arrLinks = new ArrayList();
        private bool boolOnlyImage;
        private bool boolOnlyVideo;
        private int intColNum;
        public clsConfig objConfig;
        private clsFileItem objFItem;
        public string strAllowedFileExt;
        public string strApply;
        public string strCmd;
        public string strCurrLink; // /WebForms/FileManager/dialog.aspx?editor=.... for simplicity
        public string strCurrPath;
        public string strEditor;
        public string strFile;
        public string strFolder;
        public string strLang;
        private string strProfile;
        public string strType;

        protected void Page_Load(object sender, EventArgs e)
        {
            strCmd = Request.QueryString["cmd"] + "";
            strType = Request.QueryString["type"] + "";
            strFolder = Request.QueryString["folder"] + "";
            strFile = Request.QueryString["file"] + "";
            strLang = Request.QueryString["lang"] + ""; //not used right now, but grab it
            strEditor = Request.QueryString["editor"] + "";
            strCurrPath = Request.QueryString["currpath"] + "";
            strProfile = Request.QueryString["profile"] + "";

            if (!Permission.HasPermission(false, true, Permission.PermissionName.CanManageContent))
            {
                Response.Redirect("/404-NotFound");
            }

            // load config
            objConfig = new clsConfig(strProfile);

            //check inputs
            if (strCurrPath.Length > 0)
            {
                strCurrPath = strCurrPath.TrimEnd('\\') + "\\";
            }

            //set the apply string, based on the passed type
            if (strType == "")
            {
                strType = "0";
            }
            switch (strType)
            {
                case "1":
                    strApply = "apply_img";
                    boolOnlyImage = true;
                    strAllowedFileExt = objConfig.strAllowedImageExtensions;
                    break;
                case "2":
                    strApply = "apply_link";
                    strAllowedFileExt = objConfig.strAllowedAllExtensions;
                    break;
                default:
                    if (Convert.ToInt32(strType) >= 3)
                    {
                        strApply = "apply_video";
                        boolOnlyVideo = true;
                        strAllowedFileExt = objConfig.strAllowedVideoExtensions;
                    }
                    else
                    {
                        strApply = "apply";
                        strAllowedFileExt = objConfig.strAllowedAllExtensions;
                    }
                    break;
            }

            //setup current link
            strCurrLink = "/WebForms/FileManager/dialog.aspx?type=" + strType + "&editor=" + strEditor + "&lang=" +
                          strLang + "&profile=" + strProfile;

            switch (strCmd)
            {
                case "debugsettings":
                    Response.Write("<style>");
                    Response.Write("body {font-family: Verdana; font-size: 10pt;}");
                    Response.Write(
                        ".table {display: table; border-collapse: collapse; margin: 20px; background-color: #e7e5e5;}");
                    Response.Write(
                        ".tcaption {display: table-caption; padding: 5px; font-size: 14pt; font-weight: bold; background-color: #9fcff7;}");
                    Response.Write(".tr {display: table-row;}");
                    Response.Write(".tr:hover {background-color: #f0f2f3;}");
                    Response.Write(".td {display: table-cell; padding: 5px; border: 1px solid #a19e9e;}");
                    Response.Write("</style>");

                    Response.Write("<div class=\"table\">"); //start table

                    Response.Write("<div class=\"tcaption\">Operating Settings</div>"); //caption

                    Response.Write("<div class=\"tbody\">"); //start body

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>AllowCreateFolder:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.boolAllowCreateFolder + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>AllowDeleteFile:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.boolAllowDeleteFile + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>AllowDeleteFolder:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.boolAllowDeleteFolder + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>AllowUploadFile:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.boolAllowUploadFile + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>MaxUploadSizeMb:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.intMaxUploadSizeMb + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>AllowedAllExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strAllowedAllExtensions + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>AllowedFileExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strAllowedFileExtensions + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>AllowedImageExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strAllowedImageExtensions + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>AllowedMiscExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strAllowedMiscExtensions + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>AllowedMusicExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strAllowedMusicExtensions + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>AllowedVideoExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strAllowedVideoExtensions + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>BaseURL:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strBaseURL + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>DocRoot:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strDocRoot + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>ThumbPath:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strThumbPath + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>ThumbURL:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strThumbURL + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>UploadPath:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strUploadPath + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>UploadURL:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strUploadURL + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>FillSelector:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strFillSelector + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("<div class=\"tr\">"); // start row
                    Response.Write("<div class=\"td\"><b>PopupCloseCode:</b></div>");
                    Response.Write("<div class=\"td\">" + objConfig.strPopupCloseCode + "</div>");
                    Response.Write("</div>"); //end row

                    Response.Write("</div>"); //end body
                    Response.Write("</div>"); //end table


                    Response.End();
                    break;
                case "createfolder":
                    try
                    {
                        strFolder = Request.Form["folder"] + "";
                        //forge ahead without checking for existence
                        //catch will save us
                        Directory.CreateDirectory(objConfig.strUploadPath + strFolder);
                        Directory.CreateDirectory(objConfig.strThumbPath + strFolder);

                        // end response, since it's an ajax call
                        Response.End();
                    }
                    catch
                    {
                        //TODO: write error
                    }
                    break;

                case "upload":
                    strFolder = Request.Form["folder"] + "";
                    var filUpload = Request.Files["file"];
                    string strTargetFile;
                    string strThumbFile;

                    //check file was submitted
                    if ((filUpload != null) && (filUpload.ContentLength > 0)
                        && FileIsVirusFree(filUpload))
                    {
                        //strTargetFile = this.objConfig.strUploadPath + this.strFolder + filUpload.FileName.ToLower();
                        //strThumbFile = this.objConfig.strThumbPath + this.strFolder + filUpload.FileName.ToLower();
                        var fileName = Path.GetFileName(filUpload.FileName);
                        strTargetFile = objConfig.strUploadPath + strFolder + fileName;
                        strThumbFile = objConfig.strThumbPath + strFolder + fileName;
                        filUpload.SaveAs(strTargetFile);

                        if (isImageFile(strTargetFile))
                        {
                            createThumbnail(strTargetFile, strThumbFile);
                        }
                    }

                    // end response
                    if (Request.Form["fback"] == "true")
                    {
                        Response.Redirect(strCurrLink);
                    }
                    else
                    {
                        Response.End();
                    }

                    break;

                case "download":
                {
                    var filePath = objConfig.strUploadPath + strFile;
                    if (!filePath.IsValidPath())
                    {
                        Response.Redirect("/404-NotFound");
                    }

                    var objFile = new FileInfo(filePath);
                    Response.ClearHeaders();
                    Response.AddHeader("Pragma", "private");
                    Response.AddHeader("Cache-control", "private, must-revalidate");
                    Response.AddHeader("Content-Type", "application/octet-stream");
                    Response.AddHeader("Content-Length", objFile.Length.ToString());
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(strFile));
                    Response.WriteFile(filePath);
                    break;
                }
                case "delfile":
                {
                    var filePath = objConfig.strUploadPath + strFile;
                    var thumbPath = objConfig.strThumbPath + strFile;
                    if (filePath.IsValidPath() && thumbPath.IsValidPath())
                    {
                        try
                        {
                            File.Delete(filePath);
                            if (File.Exists(thumbPath))
                            {
                                File.Delete(thumbPath);
                            }
                        }
                        catch
                        {
                            //TODO: set error
                        }
                    }
                    goto default;
                }
                case "delfolder":
                {
                    var folderPath = objConfig.strUploadPath + strFolder;
                    var thumbPath = objConfig.strThumbPath + strFolder;
                    if (folderPath.IsValidPath() && thumbPath.IsValidPath())
                    {
                        try
                        {
                            Directory.Delete(folderPath, true);
                            Directory.Delete(thumbPath, true);
                        }
                        catch
                        {
                            //TODO: set error
                        }
                    }
                    goto default;
                }
                default: //just a regular page load

                    // Disallow directory traversal outside of the root
                    if (!(objConfig.strUploadPath + strCurrPath).IsValidPath())
                    {
                        strCurrPath = "";
                    }

                    if (strCurrPath != "")
                    {
                        // add "up one" folder
                        objFItem = new clsFileItem();
                        objFItem.strName = "..";
                        objFItem.boolIsFolder = true;
                        objFItem.boolIsFolderUp = true;
                        objFItem.intColNum = getNextColNum();
                        objFItem.strPath = getUpOneDir(strCurrPath);
                        objFItem.strClassType = "dir";
                        objFItem.strDeleteLink =
                            "<a class=\"btn erase-button top-right disabled\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                        objFItem.strThumbImage = "/Scripts/TinyFileManager.Net/img/ico/folder_return.png";
                        objFItem.strLink = "<a title=\"Open\" href=\"" + strCurrLink + "&currpath=" + objFItem.strPath +
                                           "\"><img class=\"directory-img\" src=\"" + objFItem.strThumbImage +
                                           "\" alt=\"folder\" /><h3>..</h3></a>";
                        arrLinks.Add(objFItem);
                    }

                    //load folders
                    arrFolders = Directory.GetDirectories(objConfig.strUploadPath + strCurrPath);
                    foreach (var strF in arrFolders)
                    {
                        objFItem = new clsFileItem();
                        objFItem.strName = Path.GetFileName(strF);
                        objFItem.boolIsFolder = true;
                        objFItem.intColNum = getNextColNum();
                        objFItem.strPath = strCurrPath + Path.GetFileName(strF);
                        objFItem.strClassType = "dir";
                        if (objConfig.boolAllowDeleteFolder)
                        {
                            objFItem.strDeleteLink = "<a href=\"" + strCurrLink + "&cmd=delfolder&folder=" +
                                                     objFItem.strPath + "&currpath=" + strCurrPath +
                                                     "\" class=\"btn erase-button top-right\" onclick=\"return confirm('Are you sure to delete the folder and all the objects in it?');\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                        }
                        else
                        {
                            objFItem.strDeleteLink =
                                "<a class=\"btn erase-button top-right disabled\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                        }
                        objFItem.strThumbImage = "/Scripts/TinyFileManager.Net/img/ico/folder.png";
                        objFItem.strLink = "<a title=\"Open\" href=\"" + strCurrLink + "&currpath=" + objFItem.strPath +
                                           "\"><img class=\"directory-img\" src=\"" + objFItem.strThumbImage +
                                           "\" alt=\"folder\" /><h3>" + objFItem.strName + "</h3></a>";
                        arrLinks.Add(objFItem);
                    }

                    // load files
                    arrFiles = Directory.GetFiles(objConfig.strUploadPath + strCurrPath);
                    foreach (var strF in arrFiles)
                    {
                        objFItem = new clsFileItem();
                        objFItem.strName = Path.GetFileNameWithoutExtension(strF);
                        objFItem.boolIsFolder = false;
                        objFItem.strPath = strCurrPath + Path.GetFileName(strF);
                        objFItem.boolIsImage = isImageFile(Path.GetFileName(strF));
                        objFItem.boolIsVideo = isVideoFile(Path.GetFileName(strF));
                        objFItem.boolIsMusic = isMusicFile(Path.GetFileName(strF));
                        objFItem.boolIsMisc = isMiscFile(Path.GetFileName(strF));

                        // check to see if it's the type of file we are looking at
                        if ((boolOnlyImage && objFItem.boolIsImage) || (boolOnlyVideo && objFItem.boolIsVideo) ||
                            (!boolOnlyImage && !boolOnlyVideo))
                        {
                            objFItem.intColNum = getNextColNum();
                            // get display class type
                            if (objFItem.boolIsImage)
                            {
                                objFItem.strClassType = "2";
                            }
                            else
                            {
                                if (objFItem.boolIsMisc)
                                {
                                    objFItem.strClassType = "3";
                                }
                                else
                                {
                                    if (objFItem.boolIsMusic)
                                    {
                                        objFItem.strClassType = "5";
                                    }
                                    else
                                    {
                                        if (objFItem.boolIsVideo)
                                        {
                                            objFItem.strClassType = "4";
                                        }
                                        else
                                        {
                                            objFItem.strClassType = "1";
                                        }
                                    }
                                }
                            }
                            // get delete link
                            if (objConfig.boolAllowDeleteFile)
                            {
                                objFItem.strDeleteLink = "<a href=\"" + strCurrLink + "&cmd=delfile&file=" +
                                                         objFItem.strPath + "&currpath=" + strCurrPath +
                                                         "\" class=\"btn erase-button\" onclick=\"return confirm('Are you sure to delete this file?');\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                            }
                            else
                            {
                                objFItem.strDeleteLink =
                                    "<a class=\"btn erase-button disabled\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                            }
                            // get thumbnail image
                            if (objFItem.boolIsImage)
                            {
                                // first check to see if thumb exists
                                string overrideThumbUrl = null;
                                if (!File.Exists(objConfig.strThumbPath + objFItem.strPath))
                                {
                                    // thumb doesn't exist, create it
                                    strTargetFile = objConfig.strUploadPath + objFItem.strPath;
                                    strThumbFile = objConfig.strThumbPath + objFItem.strPath;
                                    overrideThumbUrl = createThumbnail(strTargetFile, strThumbFile);
                                }
                                objFItem.strThumbImage = overrideThumbUrl ??
                                                         objConfig.strThumbURL + "/" +
                                                         objFItem.strPath.Replace('\\', '/');
                            }
                            else
                            {
                                //if (File.Exists(Directory.GetParent(Request.PhysicalPath).FullName + "\\img\\ico\\" + Path.GetExtension(strF).TrimStart('.').ToUpper() + ".png"))
                                var appPath = HttpContext.Current.Request.PhysicalApplicationPath;
                                if (
                                    File.Exists(appPath + "\\Scripts\\TinyFileManager.Net\\img\\ico\\" +
                                                Path.GetExtension(strF).TrimStart('.').ToUpper() + ".png"))
                                {
                                    objFItem.strThumbImage = "/Scripts/TinyFileManager.Net/img/ico/" +
                                                             Path.GetExtension(strF).TrimStart('.').ToUpper() + ".png";
                                }
                                else
                                {
                                    objFItem.strThumbImage = "/Scripts/TinyFileManager.Net/img/ico/Default.png";
                                }
                            }
                            objFItem.strDownFormOpen =
                                "<form action=\"/WebForms/FileManager/dialog.aspx?cmd=download&file=" + objFItem.strPath +
                                "\" method=\"post\" class=\"download-form\">";
                            if (objFItem.boolIsImage)
                            {
                                objFItem.strPreviewLink = "<a class=\"btn preview\" title=\"Preview\" data-url=\"" +
                                                          objConfig.strUploadURL + "/" +
                                                          objFItem.strPath.Replace('\\', '/') +
                                                          "\" data-toggle=\"lightbox\" href=\"#previewLightbox\"><i class=\"icon-eye-open\"></i></a>";
                            }
                            else
                            {
                                objFItem.strPreviewLink =
                                    "<a class=\"btn preview disabled\" title=\"Preview unavailable\"><i class=\"icon-eye-open\"></i></a>";
                            }
                            objFItem.strLink = "<a href=\"#\" title=\"Select\" onclick=\"" + strApply + "('" +
                                               HttpUtility.JavaScriptStringEncode(objConfig.strUploadURL + "/" +
                                                                                  objFItem.strPath.Replace('\\', '/')) +
                                               "'," +
                                               strType +
                                               ")\";\"><img data-src=\"holder.js/140x100\" alt=\"140x100\" src=\"" +
                                               objFItem.strThumbImage + "\" height=\"100\"><h4>" + objFItem.strName +
                                               "</h4></a>";

                            arrLinks.Add(objFItem);
                        }
                    } // foreach

                    break;
            } // switch
        } // page load

        public string getBreadCrumb()
        {
            string strRet;
            string[] arrFolders;
            var strTempPath = "";
            var intCount = 0;

            strRet = "<li><a href=\"" + strCurrLink + "&currpath=\"><i class=\"icon-home\"></i></a>";
            arrFolders = strCurrPath.Split('\\');

            foreach (var strFolder in arrFolders)
            {
                if (strFolder != "")
                {
                    strTempPath += strFolder + "\\";
                    intCount++;

                    if (intCount == arrFolders.Length - 1)
                    {
                        strRet += " <span class=\"divider\">/</span></li> <li class=\"active\">" + strFolder + "</li>";
                    }
                    else
                    {
                        strRet += " <span class=\"divider\">/</span></li> <li><a href=\"" + strCurrLink + "&currpath=" +
                                  strTempPath + "\">" + strFolder + "</a>";
                    }
                }
            } // foreach

            return strRet;
        } // getBreadCrumb 

        private bool isImageFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(objConfig.arrAllowedImageExtensions,
                Path.GetExtension(strFilename).ToLower().TrimStart('.'));
            return intPosition > -1; // if > -1, then it was found in the list of image file extensions
        } // isImageFile

        private bool isVideoFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(objConfig.arrAllowedVideoExtensions,
                Path.GetExtension(strFilename).ToLower().TrimStart('.'));
            return intPosition > -1; // if > -1, then it was found in the list of video file extensions
        } // isVideoFile

        private bool isMusicFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(objConfig.arrAllowedMusicExtensions,
                Path.GetExtension(strFilename).ToLower().TrimStart('.'));
            return intPosition > -1; // if > -1, then it was found in the list of music file extensions
        } // isMusicFile

        private bool isMiscFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(objConfig.arrAllowedMiscExtensions,
                Path.GetExtension(strFilename).ToLower().TrimStart('.'));
            return intPosition > -1; // if > -1, then it was found in the list of misc file extensions
        } // isMiscFile

        private string createThumbnail(string strFilename, string strThumbFilename)
        {
            Image.GetThumbnailImageAbort objCallback;
            Image objFSImage;
            Image objTNImage;
            RectangleF objRect;
            var objUnits = GraphicsUnit.Pixel;
            var intHeight = 0;
            var intWidth = 0;
            string overrideThumbUrl = null;

            // open image and get dimensions in pixels
            try
            {
                objFSImage = Image.FromFile(strFilename);
                objRect = objFSImage.GetBounds(ref objUnits);

                // what are we going to resize to, to fit inside 156x78
                getProportionalResize(Convert.ToInt32(objRect.Width), Convert.ToInt32(objRect.Height), ref intWidth,
                    ref intHeight);

                // create thumbnail
                objCallback = ThumbnailCallback;
                objTNImage = objFSImage.GetThumbnailImage(intWidth, intHeight, objCallback, IntPtr.Zero);

                // finish up
                objFSImage.Dispose();
                objTNImage.Save(strThumbFilename);
                objTNImage.Dispose();
            }
            catch (Exception ex)
            {
                // was OutOfMemoryException but glomming onto all
                var appPath = HttpContext.Current.Request.PhysicalApplicationPath;
                var ext = Path.GetExtension(strFilename).TrimStart('.').ToUpper();
                if (
                    File.Exists(appPath + "\\Scripts\\TinyFileManager.Net\\img\\ico\\" +
                                ext + ".png"))
                {
                    overrideThumbUrl = "/Scripts/TinyFileManager.Net/img/ico/" +
                                       ext + ".png";
                }
                else
                {
                    overrideThumbUrl = "/Scripts/TinyFileManager.Net/img/ico/Default.png";
                }
            }
            return overrideThumbUrl;
        } // createThumbnail

        private void getProportionalResize(int intOldWidth, int intOldHeight, ref int intNewWidth, ref int intNewHeight)
        {
            var intHDiff = 0;
            var intWDiff = 0;
            decimal decProp = 0;
            var intTargH = 78;
            var intTargW = 156;

            if ((intOldHeight <= intTargH) && (intOldWidth <= intTargW))
            {
                // no resize needed
                intNewHeight = intOldHeight;
                intNewWidth = intOldWidth;
                return;
            }

            //get the differences between desired and current height and width
            intHDiff = intOldHeight - intTargH;
            intWDiff = intOldWidth - intTargW;

            //whichever is the bigger difference is the chosen proportion
            if (intHDiff > intWDiff)
            {
                decProp = intTargH/(decimal) intOldHeight;
                intNewHeight = intTargH;
                intNewWidth = Convert.ToInt32(Math.Round(intOldWidth*decProp, 0));
            }
            else
            {
                decProp = intTargW/(decimal) intOldWidth;
                intNewWidth = intTargW;
                intNewHeight = Convert.ToInt32(Math.Round(intOldHeight*decProp, 0));
            }
        } // getProportionalResize

        private bool ThumbnailCallback()
        {
            return false;
        } // ThumbnailCallback

        public string getEndOfLine(int intColNum)
        {
            if (intColNum == 6)
            {
                return "</div><div class=\"space10\"></div>";
            }
            return "";
        } // getEndOfLine

        public string getStartOfLine(int intColNum)
        {
            if (intColNum == 1)
            {
                return "<div class=\"row-fluid\">";
            }
            return "";
        } // getStartOfLine

        private int getNextColNum()
        {
            intColNum++;
            if (intColNum > 6)
            {
                intColNum = 1;
            }
            return intColNum;
        } // getNextColNum

        private string getUpOneDir(string strInput)
        {
            string[] arrTemp;

            arrTemp = strInput.TrimEnd('\\').Split('\\');
            arrTemp[arrTemp.Length - 1] = "";
            return string.Join("\\", arrTemp);
        }

        /// <summary>
        ///     Virus scan an uploaded file
        /// </summary>
        /// <param name="file"></param>
        /// <returns>True is the file is OK</returns>
        public static bool FileIsVirusFree(HttpPostedFile file)
        {
            var virusScanPath = Constants.ConfigSettings.VirusScanPath;
            var virusScanTypeString = Constants.ConfigSettings.VirusScanType;
            VirusScanEngineType virusScanType;
            if (!Enum.TryParse(virusScanTypeString, true, out virusScanType))
            {
                virusScanType = VirusScanEngineType.InstalledOnAccessVirusScanner;
            }

            var virusScan = new VirusScan(virusScanPath, virusScanType);
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
    } // class
} // namespace