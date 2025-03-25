using DistributorWebsite.MVC.WebUI.Helpers;
using DistributorWebsite.MVC.WebUI.Helpers.Models;
using DistributorWebsite.MVC.WebUI.Helpers.Structures;
using DistributorWebsite.MVC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    public class BaseControllerNoEntityChecks : BaseController
    {
        public BaseControllerNoEntityChecks():base(false, true)
        {

        }
    }

    [NoCache]
    [HCAuthorize]
    public class BaseController : Controller
    {
        #region "PROPERTY: SkipExpiredAccessTokenCheck"
        /// <summary>
        /// Get/set whether or not to skip the access token check
        /// </summary>
        public Boolean SkipExpiredAccessTokenCheck { get; set; }
        #endregion

        #region "PROPERTY: SkipLinkedEntityCheck"
        /// <summary>
        /// Get/set whether or not to skip the linked entity checks
        /// </summary>
        public Boolean SkiplinkedEntityCheck { get; set; }
        #endregion

        #region "FUNCTION: IsAuthenticated"
        /// <summary>
        /// Get whether or not the current request is authenticated
        /// </summary>
        private bool IsAuthenticated
        {
            get
            {
                if (this.User == null || this.User.Identity == null || !this.User.Identity.IsAuthenticated)
                    return (false);
                else
                    return (true);
            }
        }
        #endregion

        #region "PROPERTY: CurrentBrand"
        /// <summary>
        /// Get/set the current brand
        /// </summary>
        public string CurrentBrand { get; set; }
        #endregion

        #region "PROPERTY: CurrentFullName"
        /// <summary>
        /// Get/set the current user's full name
        /// </summary>
        public string CurrentFullName { get; set; }
        #endregion

        #region "PROPERTY: CurrentLanguage"
        /// <summary>
        /// Get the user's current language
        /// </summary>
        public String CurrentLanguage { get; set; }
        #endregion

        #region "PROPERTY: CurrentCulture"
        /// <summary>
        /// Get the user's current culture
        /// </summary>
        public String CurrentCulture { get; set; }
        #endregion

        #region "PROPERTY: EntityAccessToken"
        private Helpers.Security.AccessToken m_EntityAccessToken = null;

        /// <summary>
        /// Parse the current entity access token
        /// </summary>
        public Helpers.Security.AccessToken EntityAccessToken
        {
            get
            {
                if (m_EntityAccessToken == null)
                {
                    try
                    {
                        //-- PARSE THE ACCESS TOKEN --
                        m_EntityAccessToken = Common.GetCurrentEntityToken();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex.Message, ex);
                        throw;
                    }
                }

                return (m_EntityAccessToken);
            }
        }
        #endregion

        #region "PROPERTY: CurrentUser"
        private Helpers.Security.AccessToken  m_CurrentUser = null;

        /// <summary>
        /// Get the current user info on demand - populate the user object the first time this is referenced in the current page
        /// </summary>
        public Helpers.Security.AccessToken CurrentUser
        {
            get
            {
                if (m_CurrentUser == null)
                    ParseCurrentUser();
                return (m_CurrentUser);
            }
        }
        #endregion

        #region "PROPERTY: CurrentLanguageText"
        /// <summary>
        /// Get the current language description
        /// </summary>
        public String CurrentLanguageText
        {
            get
            {
                switch (this.CurrentLanguage)
                {
                    case "es":
                        return "Español";

                    case "pt":
                        return "Português";

                    default:
                        return "English";
                }
            }
        }
        #endregion

        #region "PROPERTY: CurrentLanguageHTML"
        /// <summary>
        /// Get the current language HTML
        /// </summary>
        public String CurrentLanguageHTML
        {
            get
            {
                return String.Format("<span class=\"hidden-sm hidden-md hidden-lg\">{0}</span><span class=\"hidden-xs\">{1}</span>", this.CurrentLanguage.ToUpper(), this.CurrentLanguageText);                
            }
        }
        #endregion

        #region "PROPERTY: CurrentMVCAction"
        /// <summary>
        /// Retrieve the name of the current MVC action
        /// </summary>
        public String CurrentMVCAction
        {
            get
            {
                String action;

                try
                {
                    action = this.ControllerContext.RouteData.Values["action"].ToString();
                }
                catch (Exception)
                {
                    action = "";                 
                }

                return action;
            }
        }
        #endregion

        #region "PROPERTY: CurrentMVCController"
        /// <summary>
        /// Retrieve the name of the current MVC controller
        /// </summary>
        public String CurrentMVCController
        {
            get
            {
                String controller;

                try
                {
                    controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                }
                catch (Exception)
                {
                    controller = "";
                }

                return controller;
            }
        }
        #endregion

        #region "PROPERTY: CurrentAccessToken"
        /// <summary>
        /// Get/set the current access token
        /// </summary>
        public String CurrentAccessToken { get; set; }
        #endregion

        #region "PROPERTY: UseInlineDownload"
        /// <summary>
        /// Get/set whether or not to use inline file attachments
        /// </summary>
        public Boolean UseInlineDownload { get; set; }
        #endregion

        #region "PROPERTY: MenuItems"
        /// <summary>
        /// Get/set the list of customized menu items for the currently logged in user
        /// </summary>
        public List<IMenuItem> MenuItems { get; private set; }
        #endregion

        #region "PROPERTY: CurrentMenuText"
        /// <summary>
        /// Get/set the text of the currently selected menu item
        /// </summary>
        public String CurrentMenuText { get; private set; }
        #endregion

        #region "PROPERTY: RootURL"
        private string m_RootURL;

        /// <summary>
        /// Return the root URL of the current page
        /// </summary>
        public string RootURL
        {
            get
            {
                if (String.IsNullOrWhiteSpace(m_RootURL))
                {
                    //-- GET THE CURRENT ROOT URL --
                    m_RootURL = string.Format("{0}://{1}{2}",
                                                 System.Web.HttpContext.Current.Request.Url.Scheme,
                                                 System.Web.HttpContext.Current.Request.Url.Authority,
                                                 this.Url.Content("~"));
                    while (m_RootURL.EndsWith(@"/"))
                    {
                        m_RootURL = m_RootURL.Substring(0, m_RootURL.Length - 1);
                    }
                }

                return (m_RootURL);
            }
        }
        #endregion

        #region "CONSTRUCTOR"
        /// <summary>
        /// Initialize the base controller - don't skip the access token check by default
        /// </summary>
        public BaseController():this(false,false)
        {
        }

        /// <summary>
        /// Initialize the base controller
        /// </summary>
        /// <param name="skipAccessTokenCheck"></param>
        public BaseController(Boolean skipAccessTokenCheck, Boolean skipLinkedEntityCheck = false):base()
        {
            this.SkipExpiredAccessTokenCheck = skipAccessTokenCheck;
            this.SkiplinkedEntityCheck = false;
        }
        #endregion

        #region "PROCEDURE: BeginExecuteCore"
        /// <summary>
        /// Handle common processing needed by all pages in the distributor website
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            if (this.IsAuthenticated)
            {
                try
                {
                    Logger.LogControllerAndAction();

                    if (this.CurrentUser != null)
                        System.Diagnostics.Trace.TraceInformation(this.CurrentUser.ToStringIndented());

                    //-- INITIALIZE THE FULL NAME COOKIE --
                    this.CurrentFullName = this.InitializeAndGetFullName();

                    //-- UPDATE THE CULTURE --
                    this.CurrentCulture = Helpers.CultureHelper.InitializeCurrentCulture();

                    //-- UPDATE THE LANGUAGE --
                    this.CurrentLanguage = Helpers.CultureHelper.GetLanguageFromCulture(this.CurrentCulture);

                    //-- GET THE USER SECURITY SETTINGS --
                    this.CurrentAccessToken = Helpers.CookieHelper.AccessToken;

                    //-- GET THE CURRENT BRAND --
                    this.CurrentBrand = Helpers.CookieHelper.CurrentEntityBrand;
                    if (String.IsNullOrWhiteSpace(this.CurrentBrand))
                        this.CurrentBrand = "HC";

                    //-- GET THE MENU ITEMS --
                    if (this.EntityAccessToken != null)
                    {
                        var menuInfo = Helpers.Navigation.GetAccessibleMenuItems(this);
                        this.MenuItems = menuInfo.Item1;
                        this.CurrentMenuText = menuInfo.Item2;
                    }

                    //-- GET THE INLINE SETTINGS --
                    this.UseInlineDownload = Helpers.Common.UseInlineDownload;
                }
                catch (Exception)
                {
                    throw;
                }

                //-- SET THE VIEWBAG PROPERTIES THAT CAN BE USED IN THE VIEWS --
                SetCoreViewbagProperties();
            }

            return base.BeginExecuteCore(callback, state);
        }
        #endregion

        #region "PROCEDURE: SetCoreViewbagProperties"
        /// <summary>
        /// Initialize the core Viewbag properties used by all pages
        /// </summary>
        private void SetCoreViewbagProperties()
        {
            Dictionary<String, String> linkedEntities;

            //-- SET THE VIEWBAG PROPERTIES THAT CAN BE USED IN THE VIEWS --
            ViewBag.SideMenu = this.MenuItems;
            ViewBag.Title = this.CurrentMenuText;
            ViewBag.CurrentLanguage = this.CurrentLanguage;
            ViewBag.CurrentCulture = this.CurrentCulture;
            ViewBag.CurrentLanguageText = this.CurrentLanguageText;
            ViewBag.CurrentLanguageHTML = this.CurrentLanguageHTML;
            ViewBag.CurrentAccessToken = this.CurrentAccessToken;
            ViewBag.IDServer = Helpers.Application.Instance.IdentityServer.Authority;
            ViewBag.UseInlineFiles = this.UseInlineDownload;
            ViewBag.CurrentEntity = Helpers.CookieHelper.CurrentEntity;
            ViewBag.CurrentEntityName = Helpers.CookieHelper.CurrentEntityName;
            ViewBag.CurrentEntityNameHTML = GetDisplayNameHTML(Helpers.CookieHelper.CurrentEntityName, true, 20, 40, 40, 80);
            ViewBag.CurrentBrand = this.CurrentBrand;
            ViewBag.CurrentSalespersonCode = Helpers.CookieHelper.CurrentSalespersonCode;
            ViewBag.DefaultClient = Helpers.CookieHelper.DefaultClient;
            ViewBag.RootURL = this.RootURL;
            ViewBag.UserNameHTML = GetDisplayNameHTML(User.Identity.Name, false, 20, 30, 35, 35);
            ViewBag.HASLEADS = Helpers.CookieHelper.HasPossibleLeads;

            //-- LANGUAGE PAGE LINKS --
            ViewBag.LINKEN = GetLanguageLink("en");
            ViewBag.LINKES = GetLanguageLink("es");
            ViewBag.LINKPT = GetLanguageLink("pt");

            //-- FORMAT THE FULL NAME --
            if (Helpers.Application.Instance.AuthorizationType == eWebAuthType.WINDOWS)
            {
                ViewBag.LogoutURL = Url.Action("SelectEntity", "Internal");
                ViewBag.ShowSelectEntities = false;
                ViewBag.ShowSwitchEntity = true;
                ViewBag.UserEntities = null;
                ViewBag.IsInternalLogin = true;

                if (!String.IsNullOrWhiteSpace(Helpers.CookieHelper.CurrentUserBeingImpersonated))
                    ViewBag.DisplayName = "(" + Helpers.CookieHelper.CurrentUserBeingImpersonated + ")";
                else
                    ViewBag.DisplayName = this.CurrentFullName;
            }
            else
            {
                ViewBag.LogoutURL = Url.Action("Signout", "Home");
                ViewBag.ShowSwitchEntity = false;
                ViewBag.DisplayName = this.CurrentFullName;
                ViewBag.IsInternalLogin = false;

                if (!SkiplinkedEntityCheck)
                {
                    linkedEntities = GetUserEntities();
                    if (linkedEntities == null)
                    {
                        Response.RedirectToRoute(new { controller = "Error", action = "AccessProhibited" });
                    }
                    ViewBag.UserEntities = linkedEntities;
                    ViewBag.ShowSelectEntities = linkedEntities.Count > 1;
                }
                else
                {
                    ViewBag.UserEntities = null;
                    ViewBag.ShowSelectEntities = false;
                }
            }

            ViewBag.DisplayNameHTML = GetDisplayNameHTML(ViewBag.DisplayName, false, 20, 30, 35, 35);
            ViewBag.CurrentAccessTokenRef = Helpers.CookieHelper.ACCESSTOKENKEY;
            ViewBag.CurrentRefreshTokenRef = Helpers.CookieHelper.REFRESHTOKENKEY;
        }
        #endregion

        #region "HELPER FUNCTIONS"

        #region "FUNCTION: GetLanguageLink"
        /// <summary>
        /// Get the link to change the current page's language
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private string GetLanguageLink(string language)
        {
            String url = Request.Url.AbsoluteUri.ToString();

            if (url.Contains("changelang="))
            {
                //-- REPLACE ANY EXISTING LANGUAGE CHANGE PARAMETER --
                url = url.Replace("changelang=es", $"changelang={language.ToLower()}")
                         .Replace("changelang=pt", $"changelang={language.ToLower()}")
                         .Replace("changelang=en", $"changelang={language.ToLower()}");
            }
            else
            {
                //-- ADD THE NEW LANGUAGE CHANGE PARAMETER --
                if (url.Contains("?"))
                    url += $"&changelang={language.ToLower()}";
                else
                    url += $"?changelang={language.ToLower()}";
            }

            return (url);
        }
        #endregion

        #region "FUNCTION: GetUserEntities"
        /// <summary>
        /// Get the list of user entities
        /// </summary>
        /// <returns></returns>
        private Dictionary<String, String> GetUserEntities()
        {
            Dictionary<String, String> entities = new Dictionary<string, string>();
           Helpers.Security.UserInfoClaim userInfo;

            try
            {
                //-- GET THE USER INFO --
                userInfo = Helpers.CookieHelper.UserEntities;
                if (userInfo == null)
                    return (entities);

                //-- ADD ANY LINKED DISTRIBUTORS --
                if (userInfo.DistributorLinks != null && userInfo.DistributorLinks.Count() > 0)
                {
                    foreach (var dist in userInfo.DistributorLinks)
                    {
                        entities.Add(dist.DistributorNo, dist.CompanyName);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                return new Dictionary<string, string>();
            }

            return (entities);
        }
        #endregion
        
        #region "FUNCTION: GetStaticFileResponse"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="file"></param>
        /// <param name="fileExtension"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected FileContentResult GetStaticFileResponse(byte[] file, String fileExtension, String fileName, String mimeType = null, bool isDownloadLink = false)
        {
            String contentType;

            //-- MAKE SURE THE DOCUMENT EXISTS --
            if ((file == null) || (file.Length <= 0))
                throw new HttpException(404, "Not Found") { HelpLink = "NOHEADER" };

            //-- GET THE CONTENT TYPE TO BE GENERATED --
            if (String.IsNullOrWhiteSpace(mimeType))
                contentType = Helpers.Documents.GetContentTypeFromExtension(fileExtension);
            else
                contentType = mimeType;

            //-- CLEAR THE RESPONSE --            
            Response.Clear();

            if ((!Helpers.Common.UseInlineDownload) || (isDownloadLink))
            {
                //-- RETURN THE FILE AS AN ATTACHMENT --
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}\"", fileName));
                return File(file, contentType);
            }
            else
            {
                //-- RETURN THE FILE AS AN INLINE FILE --
                Response.AddHeader("Content-Disposition", String.Format("inline; filename=\"{0}\"", fileName));
                return File(file, contentType);
            }
        }
        #endregion

        #region "FUNCTION: ParseCurrentUser"
        /// <summary>
        /// Get the distributor token of the currently logged in user
        /// </summary>
        /// <returns></returns>
        private void ParseCurrentUser(HttpContextBase context = null)
        {
            ClaimsIdentity id;
            Helpers.Security.AccessToken user = null;

            try
            {
                //-- GET THE CONTEXT --
                if (context == null)
                    context = new HttpContextWrapper(System.Web.HttpContext.Current);

                //-- GET THE CURRENT USER --
                id = context.User.Identity as ClaimsIdentity;

                if ((id != null) && (!String.IsNullOrWhiteSpace(Helpers.CookieHelper.AccessToken)))
                {
                    var jwtToken = new JwtSecurityToken(Helpers.CookieHelper.AccessToken);
                    if (jwtToken != null)
                    {
                        if (jwtToken.Claims.Any(o => o.Type == "hcdistwebapi"))
                        {
                            user = Helpers.Security.AccessToken.Parse(jwtToken.Claims.First(o => o.Type == "hcdistwebapi").Value);
                        }
                    }

                    //if ((id.HasClaim(o => o.Type == "preferred_username")))
                    //    user.UserName = id.FindFirst("preferred_username").Value;
                    //else
                    //    user.UserName = "UNKNOWN USER";

                    //if ((user.Entity == null) || (String.IsNullOrWhiteSpace(user.ACL)) || (user.ACL.Length < 20))
                    //    user = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.ToString());

                user = null;
            }

            m_CurrentUser = user;
            //return user;
        }
        #endregion

        #region "FUNCTION: GetDocument"
        /// <summary>
        /// Get an individual document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        protected async Task<ActionResult> GetDocument(String url, bool isDownloadLink = false)
        {
            HCFileInfo fileInfo;

            //-- GET THE SPECIFIC ANNOUNCEMENT --
            try
            {
                using (Helpers.WebAPI oAPI = new Helpers.WebAPI(language: this.CurrentLanguage, existingToken: this.CurrentUser))
                {
                    try
                    {
                        fileInfo = await oAPI.GetFile(url);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex.ToString());
                        fileInfo = null;
                    }

                }

                if ((fileInfo == null) || (fileInfo.File == null) || (fileInfo.File.Length <= 0))
                {
                    //-- THE FILE COULD NOT BE FOUND OR WASN'T ACCESSIBLE --
                    return new RedirectResult(this.Url.Content("~/Error/DocumentNotFound"));
                }
                else
                {
                    //-- STREAM THE MEMO TO THE BROWSER --
                    return this.GetStaticFileResponse(fileInfo.File, fileInfo.FileExtension, fileInfo.FileName, fileInfo.ContentType, isDownloadLink);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #endregion

        #region "PROCEDURE: InitializeAndGetFullName"
        /// <summary>
        /// Initialize the user's full name
        /// </summary>
        /// <returns></returns>
        private string InitializeAndGetFullName()
        {
            String fullName = "Unknown";

            if (String.IsNullOrWhiteSpace(Helpers.CookieHelper.CurrentFullName))
            {
                if (Helpers.Application.Instance.AuthorizationType == eWebAuthType.WINDOWS)
                {
                    //----------------------------------------
                    //-- INITIALIZE WINDOWS AUTH PROPERTIES --
                    //----------------------------------------
                    if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
                    {
                        try
                        {
                            System.DirectoryServices.DirectoryEntry de = new System.DirectoryServices.DirectoryEntry("WinNT://" + Environment.UserDomainName + "/" + Environment.UserName);
                            fullName = de.Properties["fullName"].Value.ToString();
                            if (String.IsNullOrWhiteSpace(fullName))
                                fullName = String.Format("Hycite Employee ({0})", User.Identity.Name);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.TraceError(ex.ToString());
                            fullName = "Hycite Employee";
                        }
                    }
                    else
                        fullName = "Hycite Employee";
                }
                else if (this.CurrentUser != null && !String.IsNullOrWhiteSpace(this.CurrentUser.Name))
                {
                    //---------------------------------------
                    //-- GET FULL NAME FROM IDENTITY TOKEN --
                    //---------------------------------------
                    fullName = this.CurrentUser.Name;
                }

                //-- SET THE FULL NAME COOKIE --
                Helpers.CookieHelper.CurrentFullName = fullName;
            }
            else
                fullName = Helpers.CookieHelper.CurrentFullName;

            return (fullName);
        }
        #endregion

        #region "FUNCTION: GetDisplayNameHTML"
        /// <summary>
        /// Generate the html for the entity name
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        private string GetDisplayNameHTML(string entityName, bool capitalize = true, Int32 maxXSSize = 20, Int32 maxSMSize = 35, Int32 maxMDSize = 45, Int32 maxLGSize = 80)
        {
            String formattedName;

            if (String.IsNullOrWhiteSpace(entityName) || entityName.Length <= maxXSSize)
            {
                //-- RETURN THE ENTITY NAME - IT IS LESS THAN THE MAX SIZE FOR SMALL DISPLAY --
                if (capitalize)
                    return (entityName ?? "").ToUpper();
                else
                    return (entityName ?? "");
            }
            else
            {
                try
                {
                    string xsName = entityName.Substring(0, maxXSSize).Trim();
                    if (entityName.Length <= maxSMSize)
                    {
                        //-- SEPARATE VIEW FOR XS/SM and MD/LG --
                        formattedName = $"<span class=\"visible-xs\">{EllipseName((capitalize ? xsName.ToUpper() : xsName))}</span>" +
                                        $"<span class=\"hidden-xs\">{(capitalize ? entityName.ToUpper() : entityName)}</span>";
                    }
                    else
                    {
                        string smallName = entityName.Substring(0, maxSMSize).Trim();
                        if (entityName.Length <= maxMDSize)
                        {
                            //-- SEPARATE VIEW FOR XS/SM and MD/LG --
                            formattedName = $"<span class=\"visible-xs\">{EllipseName((capitalize ? xsName.ToUpper() : xsName))}</span>" +
                                            $"<span class=\"visible-sm\">{EllipseName((capitalize ? smallName.ToUpper() : smallName))}</span>" +
                                            $"<span class=\"visible-md visible-lg\">{(capitalize ? entityName.ToUpper() : entityName)}</span>";
                        }
                        else
                        {
                            string mediumName = entityName.Substring(0, maxMDSize).Trim();
                            if (entityName.Length <= maxLGSize)
                            {
                                //-- SEPARATE VIEW FOR XS/SM, MD AND LG --
                                formattedName = $"<span class=\"visible-xs\">{EllipseName((capitalize ? xsName.ToUpper() : xsName))}</span>" +
                                                $"<span class=\"visible-sm\">{EllipseName((capitalize ? smallName.ToUpper() : smallName))}</span>" +
                                                $"<span class=\"visible-md\">{EllipseName((capitalize ? mediumName.ToUpper() : mediumName))}</span>" +
                                                $"<span class=\"visible-lg\">{(capitalize ? entityName.ToUpper() : entityName)}</span>";
                            }
                            else
                            {
                                string largeName = entityName.Substring(0, maxLGSize).Trim();

                                //-- SEPARATE VIEW FOR XS/SM, MD AND LG --
                                formattedName = $"<span class=\"visible-xs\">{EllipseName((capitalize ? xsName.ToUpper() : xsName))}</span>" +
                                                $"<span class=\"visible-sm\">{EllipseName((capitalize ? smallName.ToUpper() : smallName))}</span>" +
                                                $"<span class=\"visible-md\">{EllipseName((capitalize ? mediumName.ToUpper() : mediumName))}</span>" +
                                                $"<span class=\"visible-lg\">{EllipseName((capitalize ? largeName.ToUpper() : largeName))}</span>";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.ToString());
                    formattedName = entityName.Substring(0, maxSMSize);
                }
            }

            return (formattedName);
        }
        #endregion

        #region "FUNCTION: EllipseName"
        /// <summary>
        /// Get a name up to the last space and add ellipse to the end of it
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string EllipseName(String name)
        {
            try
            {
                return name.Substring(0, name.LastIndexOf(' ')) + "...";
            }
            catch (Exception)
            {
                return (name ?? "") + "...";
            }
        }
        #endregion

        #region "PROCEDURE: Dispose"
        /// <summary>
        /// Clean up any managed resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (m_CurrentUser != null)
            {
                //-- CLEAN UP THE CURRENT USER OBJECT IF IT WAS CREATED --
                //m_CurrentUser.Dispose();
                m_CurrentUser = null;
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}