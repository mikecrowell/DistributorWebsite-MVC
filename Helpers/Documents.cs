using DistributorWebsite.MVC.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI.Helpers
{
    public static class Documents
    {
        #region "FUNCTION: GetFileTypeFromExtension"
        /// <summary>
        /// Get the file type from the specified extension
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static eFileType GetFileTypeFromExtension(String extension)
        {
            switch (extension.ToLower().Replace(".", ""))
            {
                case "pdf":
                    return eFileType.PDF;

                case "xls":
                case "xlsx":
                    return eFileType.Excel;

                case "jpg":
                case "jpeg":
                    return eFileType.ImageJpeg;

                case "tif":
                    return eFileType.ImageTIF;

                case "png":
                    return eFileType.ImagePNG;

                case "gif":
                    return eFileType.ImageGIF;

                case "eps":
                    return eFileType.ImageEPS;

                case "psd":
                    return eFileType.ImagePSD;

                case "bmp":
                    return eFileType.ImageBMP;

                case "ppt":
                    return eFileType.Powerpoint;

                case "mp3":
                    return eFileType.AudioMP3;

                case "wma":
                    return eFileType.AudioWMA;

                case "avi":
                    return eFileType.VideoAVI;

                case "mov":
                    return eFileType.VideoMOV;

                case "mpg":
                    return eFileType.VideoMPG;

                case "wmv":
                    return eFileType.VideoWMV;

                case "zip":
                    return eFileType.Zip;

                default:
                    return eFileType.Unknown;
            }
        }
        #endregion

        #region "FUNCTION: GetContentTypeFromExtension"
        /// <summary>
        /// Get the content type of the current file
        /// </summary>
        public static String GetContentTypeFromExtension(String extension)
        {
            switch (GetFileTypeFromExtension(extension))
            {
                case eFileType.AudioMP3:
                    return (@"audio/mpeg");

                case eFileType.AudioWMA:
                    return (@"audio/x-ms-wma");

                case eFileType.ImageBMP:
                    return (@"image/bmp");

                case eFileType.ImageEPS:
                    return (@"application/postscript");

                case eFileType.ImageGIF:
                    return (@"image/gif");

                case eFileType.ImageJpeg:
                    return (@"image/jpeg");

                case eFileType.ImagePNG:
                    return (@"image/png");

                case eFileType.ImagePSD:
                    return (@"application/octet-stream");

                case eFileType.ImageTIF:
                    return (@"image/tiff");

                case eFileType.PDF:
                    return (@"application/pdf");

                case eFileType.Powerpoint:
                    return (@"application/mspowerpoint");

                case eFileType.Excel:
                    return (@"application/ms-excel");

                case eFileType.Zip:
                    return (@"application/zip");

                case eFileType.VideoAVI:
                    return (@"video/avi");

                case eFileType.VideoMOV:
                    return (@"video/quicktime");

                case eFileType.VideoMPG:
                    return (@"video/mpeg");

                case eFileType.VideoWMV:
                    return (@"video/x-ms-wmv");

                default:
                    return (@"text/plain");
            }
        }    
        #endregion
    }
}