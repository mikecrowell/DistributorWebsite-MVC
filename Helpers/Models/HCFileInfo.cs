using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Models
{
    public class HCFileInfo
    {
        /// <summary>
        /// Get/set the name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Get/set the file type (extension)
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Get the file type from the current file extension
        /// </summary>
        public eFileType FileType { get; set; }

        /// <summary>
        /// Get/set the file size string
        /// </summary>
        public string FileSize { get; set; }

        /// <summary>
        /// Get the content type string based on the current file extension
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Get/set the file language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Get/set the actual file
        /// </summary>
        public byte[] File { get; set; }
    }
}