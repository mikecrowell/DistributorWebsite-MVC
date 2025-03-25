using DistributorWebsite.MVC.WebUI.Controllers;
using DistributorWebsite.MVC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributorWebsite.MVC.WebUI.Helpers
{
    public static partial class Navigation
    {
        #region "STRUCTURE: MenuCSS"
        /// <summary>
        /// Define css characteristics for a menu item
        /// </summary>
        private struct MenuCSS
        {
            public String Icon { get; set; }
            public String SelectedClass { get; set; }
        }
        #endregion

        #region "FUNCTION: RefreshSecurityItemsFromAPI"
        /// <summary>
        /// Get the list of security items from the identity server database
        /// </summary>
        /// <returns></returns>
        public static async Task<Boolean> RefreshSecurityItemsFromAPI()
        {
            Boolean retVal = false;
            Dictionary<Int32, SecurityItemDTO> securityItemList = new Dictionary<int, SecurityItemDTO>();
            IEnumerable<SecurityItemDTO> securityItems = null;

            try
            {
                //-- MAKE SURE THE CORE SERVER TOKEN IS CURRENT --
                await Helpers.EndpointAndTokenHelper.CheckAndPossiblyRefreshToken(eAccessTokenType.InternalCore);
               
                //-- GET THE SECURITY ITEMS --
                using (var oAPI = new Helpers.WebAPI(type: eAccessTokenType.InternalCore))
                {
                    securityItems = await oAPI.GetSecurityItems();
                }

                var itm = securityItems.Where(o => o.SecurityItemName == "tabLayaway").FirstOrDefault();

                //-- BUILD THE NESTED LIST --
                foreach (var item in securityItems)
                {
                    securityItemList.Add(item.SecurityIndex, item);
                }

                //-- UPDATE THE APPLICATION OBJECTS --
                if (securityItemList != null)
                {
                    Helpers.Application.Instance.RefreshSecurityItems(securityItemList); //, securityItemLinks);
                    retVal = true;
                }
                else
                    retVal = false;
            }
            catch (Exception ex)
            {
                retVal = false;
                Logger.LogError(ex.ToString());
                throw;
            }

            return (retVal);
        }
        #endregion
       
        #region "FUNCTION: GetAccessibleMenuItems"
        /// <summary>
        /// Get the list of accessible menu items
        /// </summary>
        /// <returns></returns>
        public static Tuple<System.Collections.Generic.List<IMenuItem>,String> GetAccessibleMenuItems(BaseController controller)
        {            
            System.Collections.Generic.List<IMenuItem> menuItems = new List<IMenuItem>();
            IMenuItem currentMenu;
            Boolean isInternalLogin;
            String ss;
            String selectedMenuText = "";
            MenuCSS css;
            eSecurityItem menuType;
            bool addMenu;

            try
            {
                //-- GET THE CURRENT USER INFORMATION --
                isInternalLogin = controller.EntityAccessToken.IsInternal;
                ss = Helpers.Security.AccessToken.DecompressUserSecurityString(controller.EntityAccessToken.SecurityString);
                if (ss.Length != 256)
                    ss = new string('0', 256);

                //-- GET THE SORTED LIST OF MENUS TO BE GENERATED --
                var topLevelMenus = Helpers.Application.Instance.SecurityItems.Values.Where(o => o.ParentSecurityIndex == 0 && o.SecurityItemType == "Menu" && o.Enabled == true && o.VisibleOnWebsite == true).OrderBy(o => o.Sequence);

                //-- ADD EACH TOP LEVEL MENU AND ANY CHILD MENU ITEMS --
                foreach (var topLevelMenu in topLevelMenus)
                {
                    //-- ADD THE TOP LEVEL MENU ITEM --
                    menuType = (eSecurityItem)topLevelMenu.SecurityIndex;
                    css = GetMenuCSSProperties(menuType);

                    //-- CHECK SPECIAL ACCESS RESTRICTIONS --
                    switch (topLevelMenu.SecurityItemName.ToUpper())
                    {
                        case "TABINCITE":
                            addMenu = (CanAccessInciteMenu(controller, eSecurityItem.tabInCite));
                            css.Icon = "fa fa-cloud";                            
                            break;

                        default:
                            addMenu = true;
                            break;
                    }

                    //-- GET THE MAIN MENU --
                    if (addMenu)
                    {
                        currentMenu = GetMainMenu(controller, topLevelMenu, css.Icon, css.SelectedClass, isInternalLogin, ss, controller.EntityAccessToken.UserID);
                        if (currentMenu != null)
                        {
                            menuItems.Add(currentMenu);

                            if (currentMenu.IsSelected)
                            {
                                if (currentMenu.Children.Any(o => o.IsSelected))
                                    selectedMenuText = currentMenu.Children.First(o => o.IsSelected).ItemText;
                                else
                                    selectedMenuText = currentMenu.ItemText;
                            }
                        }
                    }

                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                currentMenu = null;
            }

            return new Tuple<List<IMenuItem>, string>(menuItems, selectedMenuText);
        }
        #endregion

        #region "FUNCTION: GetMenuCSSProperties"
        /// <summary>
        /// Get the CSS menu properties for a menu
        /// </summary>
        /// <param name="menuType"></param>
        /// <returns></returns>
        private static MenuCSS GetMenuCSSProperties(eSecurityItem menuType)
        {
            MenuCSS css = new MenuCSS();

            switch (menuType)
            {
                case eSecurityItem.tabCompanyInformation:
                    css.Icon = "fa fa-th-large";
                    css.SelectedClass = "red";
                    break;

                case eSecurityItem.tabWorkflow:
                    css.Icon = "fa fa-exchange";
                    css.SelectedClass = "orange";
                    break;

                case eSecurityItem.tabPurchases:
                    css.Icon = "fa fa-dollar";
                    css.SelectedClass = "darkorange";
                    break;

                case eSecurityItem.tabJuniorDistributor:
                    css.Icon = "fa fa-user";
                    css.SelectedClass = "darkorange";
                    break;

                case eSecurityItem.tabCompetetiveArena:
                    css.Icon = "fa fa-trophy";
                    css.SelectedClass = "yellow";
                    break;

                case eSecurityItem.tabAccountsReceivable:
                    css.Icon = "fa fa-money";
                    css.SelectedClass = "yellowgreen";
                    break;

                case eSecurityItem.tabSalesAndMarketing:
                    css.Icon = "fa fa-globe";
                    css.SelectedClass = "green";
                    break;

                case eSecurityItem.tabOverrides:
                    css.Icon = "fa fa-sitemap";
                    css.SelectedClass = "blue";
                    break;

                case eSecurityItem.tabDistributorService:
                    css.Icon = "fa fa-user";
                    css.SelectedClass = "lightblue";
                    break;

                case eSecurityItem.tabDistributorFinancing:
                    css.Icon = "fa fa-calculator";
                    css.SelectedClass = "purple";
                    break;

                case eSecurityItem.tabSalesLeadAdministration:
                    css.Icon = "fa fa-users";
                    css.SelectedClass = "orange";
                    break;

                case eSecurityItem.tabLayaway:
                    css.Icon = "fa fa-address-book";
                    css.SelectedClass = "darkorange";
                    break;

                case eSecurityItem.tabInCite:
                    css.Icon = "fa fa-cloud";
                    css.SelectedClass = "lightblue";
                    break;

                case eSecurityItem.tabProfile:
                    css.Icon = "fa fa-user-plus";
                    css.SelectedClass = "red";
                    break;

                default:
                    css.Icon = "";
                    css.SelectedClass = "";
                    break;
            }

            return (css);
        }
        #endregion

        #region "FUNCTION: GetMainMenu"
        /// <summary>
        /// Build a menu from the SecurityItem definitions in the Identity database
        /// </summary>
        /// <returns></returns>
        private static IMenuItem GetMainMenu(BaseController controller, SecurityItemDTO menu, String faicon, string selectedClass, Boolean isInternalLogin, String securityString, Int32 userID)
        {
            eSecurityItem menuItem;
            APIMenuItem mainMenu = null;
            Boolean hasInaccessibleChildren = false;
            Boolean canAccessMenu;

            //-- CREATE THE TOP LEVEL MENU ITEM --
            if (CanAccess(menu, isInternalLogin, securityString, userID))
            {
                //-- INITIALIZE THE MAIN MENU --
                mainMenu = APIMenuItem.Parse(menu, controller);
                mainMenu.CSSSelectedClass = selectedClass;
                mainMenu.CSSIcon = faicon;

                //-- ADD THE CHILD ITEMS --
                foreach (var childItem in Helpers.Application.Instance.SecurityItems.Values.Where(o => o.ParentSecurityIndex == menu.SecurityIndex).OrderBy(o => o.Sequence))
                {
                    if (!Enum.TryParse<eSecurityItem>(childItem.SecurityItemName, out menuItem))
                    {
                        canAccessMenu = false;
                    }
                    else
                    {
                        switch ((eSecurityItem)Enum.Parse(typeof(eSecurityItem), childItem.SecurityItemName))
                        {
                            case eSecurityItem.mnuInciteLogin:
                                canAccessMenu = CanAccessInciteMenu(controller, eSecurityItem.mnuInciteLogin);
                                break;

                            case eSecurityItem.mnuTelemarketers:
                                canAccessMenu = CanAccessInciteMenu(controller, eSecurityItem.mnuTelemarketers);
                                break;

                            default:
                                canAccessMenu = CanAccess(childItem, isInternalLogin, securityString, userID);
                                break;
                        }
                    }

                    if (canAccessMenu)
                    {
                        switch (childItem.SecurityItemName)
                        {
                            case "mnuUniversidad":
                                childItem.OpenInNewWindow = true;
                                if (controller.EntityAccessToken.IsDistributor)
                                    childItem.StaticURL = $"{Helpers.Application.Instance.RPUniversityURL}?USER={controller.EntityAccessToken.UserName}&DISTR={controller.EntityAccessToken.DistributorNo}&VIEWINACTIVE={controller.EntityAccessToken.IsInternal.ToString().ToLower()}";
                                else
                                    childItem.StaticURL = $"{Helpers.Application.Instance.RPUniversityURL}?USER={controller.EntityAccessToken.UserName}&DISTR={controller.EntityAccessToken.HomeDistributor}&VIEWINACTIVE={controller.EntityAccessToken.IsInternal.ToString().ToLower()}";
                                break;

                            case "mnuInciteLogin":
                                childItem.OpenInNewWindow = true;
                                break;
                        }

                        mainMenu.Children.Add(APIMenuItem.Parse(childItem, controller));

                        //-- ADD FAKE MENU ITEMS FOR 0 TO 30 AND REPURCHASE LETTER ACCOUNTS --
                        switch (childItem.SecurityItemName)
                        {
                            case "mnuAccountSearch":
                                if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewDelinquent0to30, controller.EntityAccessToken))
                                {
                                    mainMenu.Children.Add(new APIMenuItem()
                                    {
                                        Children = null,
                                        ItemName = "mnu0to30",
                                        ItemText = LocalResources.HCResources.AccountSearchDelinquent0to30 + $" <span id='mnu0to30Count' class='badge badge-danger'>{Helpers.CookieHelper.Total0to30Accounts}</span>",
                                        MVCAction = "Delinquent0to30",
                                        MVCController = "AccountsReceivable",
                                        StaticLink = controller.Url.Action("Delinquent0to30", "AccountsReceivable"),
                                        ShowInNewWindow = false
                                    });
                                }

                                if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewRepurchasedAccounts, controller.EntityAccessToken))
                                {
                                    mainMenu.Children.Add(new APIMenuItem()
                                    {
                                        Children = null,
                                        ItemName = "mnuRepoLetters",
                                        ItemText = LocalResources.HCResources.AccountSearchRepurchaseLetter + $" <span id='mnuRepoLettersCount' class='badge badge-danger'>{Helpers.CookieHelper.TotalRepurchaseLetterAccounts}</span>",
                                        MVCAction = "RepurchaseLetters",
                                        MVCController = "AccountsReceivable",
                                        StaticLink = controller.Url.Action("RepurchaseLetters", "AccountsReceivable"),
                                        ShowInNewWindow = false
                                    });
                                }
                                break;
                        }
                    }
                    else
                    {
                        hasInaccessibleChildren = true;
                    }
                }

                //-- UPDATE THE SELECTED ON THE MAIN MENU IF ANY CHILD ITEMS ARE SELECTED --
                mainMenu.IsSelected = mainMenu.Children.Any(o => o.IsSelected);

                //-- DON'T CREATE THE MENU IF THERE ARE NO ACCESSIBLE CHILDREN --
                if (hasInaccessibleChildren && mainMenu.Children.Count <= 0)
                    mainMenu = null;
            }
            else
                System.Diagnostics.Trace.TraceInformation("Can not access menu");

            return mainMenu;
        }
        #endregion

        #region "FUNCTION: CanAccess"
        /// <summary>
        /// Determine whether or not the current user can access the specified security item
        /// </summary>
        /// <param name="securityItem"></param>
        /// <param name="isInternalLogin"></param>
        /// <param name="ss"></param>
        /// <returns></returns>
        private static Boolean CanAccess(SecurityItemDTO securityItem, Boolean isInternalLogin, String ss, Int32 userID)
        {
            eSecurityItem securityItemType;

            if (securityItem == null)
                return (false);
            else if (!securityItem.Enabled.HasValue || securityItem.Enabled.Value == false)
                return (false);
            else if (!securityItem.VisibleOnWebsite.HasValue || securityItem.VisibleOnWebsite.Value == false)
                return (false);
            else if (securityItem.Visibility == "I" && !isInternalLogin)
                return (false);
            else if (securityItem.Visibility == "E" && isInternalLogin && userID == Helpers.Constants.INTERNALUSERID)
                return (false);
            else if (ss.Substring(securityItem.SecurityIndex, 1) != "1")
                return (false);
            else if (!Enum.TryParse<eSecurityItem>(securityItem.SecurityItemName, true, out securityItemType))
                return (false);
            else
            {
                switch (securityItemType)
                {
                    //-- ONLY SHOW ADMIN USER MENU FOR EXTERNAL USERS OR INTERNAL USERS IMPERSONATING OTHER USERS --
                    case eSecurityItem.mnuChildUsers:
                        return (!isInternalLogin || !String.IsNullOrEmpty(Helpers.CookieHelper.CurrentUserBeingImpersonated));
                }

                return (true);
            }
        }
        #endregion

        #region "FUNCTION: CanAccessIncite"
        /// <summary>
        /// Determine whether or not to display the InCite tab
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        private static bool CanAccessInciteMenu(BaseController controller, eSecurityItem securityItem)
        {
            //-- MAKE SURE THE USER HAS INCITE APPLICATION ACCESS --
            bool canAccessTab = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.tabInCite, controller.EntityAccessToken);
            bool canAccessInciteTelemarketers = canAccessTab && Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.mnuTelemarketers, controller.EntityAccessToken);
            bool canAccessInciteLogin = canAccessTab
                                        && Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.mnuInciteLogin, controller.EntityAccessToken)
                                        && controller.EntityAccessToken.CanAccessIncite
                                        && (!controller.EntityAccessToken.IsInternal); // || !String.IsNullOrEmpty(Helpers.CookieHelper.CurrentUserBeingImpersonated));

            if (!canAccessInciteTelemarketers && !canAccessInciteLogin)
                canAccessTab = false;

            switch (securityItem)
            {
                case eSecurityItem.tabInCite:
                    return canAccessTab;

                case eSecurityItem.mnuInciteLogin:
                    return canAccessInciteLogin;

                case eSecurityItem.mnuTelemarketers:
                    return canAccessInciteTelemarketers;

                default:
                    return false;
            }
        }
        #endregion

    }
}