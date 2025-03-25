using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI
{
    #region "ENUM: ePaymentPageHost"
    public enum ePaymentPageHost
    {
        CustomerWorkflow,
        CreditCardProcessing,
        ListPayments,
        CustomerPayments
    }
    #endregion

    #region "ENUM: eAccessTokenType"
    /// <summary>
    /// Define the various types of access tokens that can be used
    /// </summary>
    public enum eAccessTokenType
    {
        /// <summary>
        /// No value
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Internal core access token used for distributor/sp selection page
        /// </summary>
        InternalCore = 1,

        /// <summary>
        /// Internal user access token
        /// </summary>
        InternalUser = 2,

        /// <summary>
        /// External user access token from login to identity server
        /// </summary>
        ExternalUser = 3
    }
    #endregion

    #region "ENUM: eUserType"
    /// <summary>
    /// Get/set the user type
    /// </summary>
    public enum eUserType
    {
        /// <summary>
        /// User is a salesperson
        /// </summary>
        Salesperson,

        /// <summary>
        /// User is a distributor
        /// </summary>
        Distributor
    }
    #endregion

    #region "ENUM: eEntityType"
    /// <summary>
    /// Define the various types of users that can log into the distributor website
    /// </summary>
    public enum eEntityType
    {
        /// <summary>
        /// The user is a distributor
        /// </summary>
        Distributor,

        /// <summary>
        /// The user is a salesperson
        /// </summary>
        Salesperson,

        /// <summary>
        /// The user is not a distributor or a salesperson
        /// </summary>
        Other
    }
    #endregion

    #region "ENUM: eWebAuthType"
    /// <summary>
    /// List of valid authorization types
    /// </summary>
    public enum eWebAuthType
    {
        WINDOWS,
        IDENTITYSERVER
    }
    #endregion

    #region "ENUM: eDirectory"
    /// <summary>
    /// Define the various types of directories files can be located in
    /// </summary>
    public enum eDirectory
    {
        Reports
    }
    #endregion

    #region "ENUM: eFileType"
    /// <summary>
    /// Define the various types of files
    /// </summary>
    public enum eFileType
    {
        Unknown,
        PDF,
        Excel,
        Powerpoint,
        Zip,
        URL,
        ImageJpeg,
        ImageGIF,
        ImageTIF,
        ImagePNG,
        ImageBMP,
        ImagePSD,
        ImageEPS,
        VideoMPG,
        VideoAVI,
        VideoMOV,
        VideoWMV,
        AudioMP3,
        AudioWMA,
        Text,
        XML
    }
    #endregion

    #region "ENUM: eDeviceType"
    /// <summary>
    /// Define the various device types that are tracked
    /// </summary>
    public enum eDeviceType
    {
        Android,
        WindowsPhone,
        IPhone,
        IPAD,
        OtherMobileDevice,
        Unknown
    }
    #endregion
}