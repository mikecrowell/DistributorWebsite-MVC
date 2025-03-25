using DistributorWebsite.MVC.WebUI.Controllers;
using DistributorWebsite.MVC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI.Helpers
{
    public class APIMenuItem : IMenuItem
    {
        /// <summary>
        /// Get/set the CSS Icon to display for the current menu item
        /// </summary>
        public string CSSIcon { get; set; }

        /// <summary>
        /// Get/set the selected class to use when the current menu item is selected
        /// </summary>
        public string CSSSelectedClass { get; set; }

        /// <summary>
        /// Get/set whether or not the current menu item is selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Get/set the current menu item name
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Get/set the current menu item text
        /// </summary>
        public string ItemText { get; set; }

        /// <summary>
        /// Get/set the current MVC action
        /// </summary>
        public string MVCAction { get; set; }

        /// <summary>
        /// Get/set the current MVC Controller
        /// </summary>
        public string MVCController { get; set; }

        /// <summary>
        /// Get/set the current security index
        /// </summary>
        public int SecurityIndex { get; set; }

        /// <summary>
        /// Get/set the linked security item
        /// </summary>
        public eSecurityItem? SecurityItem { get; set; }

        /// <summary>
        /// Get/set the menu item sequence
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Get/set the static menu link
        /// </summary>
        public string StaticLink { get; set; }

        /// <summary>
        /// Get/set whether or not to display the link in a new window
        /// </summary>
        public Boolean ShowInNewWindow { get; set; }

        /// <summary>
        /// Get/set the list of child menu items
        /// </summary>
        public List<IMenuItem> Children { get; set; }

        /// <summary>
        /// Create a menu item from an existing security item
        /// </summary>
        /// <param name="securityItem"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static APIMenuItem Parse(SecurityItemDTO securityItem, BaseController controller)
        {
            APIMenuItem menuItem = new APIMenuItem();
            menuItem.IsSelected = false;
            menuItem.CSSIcon = "";
            menuItem.CSSSelectedClass = "";
            menuItem.ItemName = securityItem.SecurityItemName;
            
            switch (controller.CurrentLanguage.ToLower())
            {
                case "es":
                    menuItem.ItemText = securityItem.DescriptionES;
                    break;

                case "pt":
                    menuItem.ItemText = securityItem.DescriptionPT;
                    break;

                default:
                    menuItem.ItemText = securityItem.DescriptionEN;
                    break;
            }

            menuItem.MVCAction = securityItem.MVCAction;
            menuItem.MVCController = securityItem.MVCController;
            menuItem.SecurityIndex = securityItem.SecurityIndex;
            menuItem.SecurityItem = (eSecurityItem)securityItem.SecurityIndex;
            menuItem.Sequence = securityItem.Sequence.Value * 20;
            menuItem.StaticLink = securityItem.StaticURL;
            menuItem.ShowInNewWindow = securityItem.OpenInNewWindow.HasValue && securityItem.OpenInNewWindow.Value == true;

            if (!String.IsNullOrWhiteSpace(menuItem.MVCAction) && !String.IsNullOrWhiteSpace(menuItem.MVCController))
            {             
                if (String.IsNullOrWhiteSpace(menuItem.StaticLink))
                {
                    menuItem.StaticLink = controller.Url.Action(menuItem.MVCAction, menuItem.MVCController);
                }

                if ((menuItem.MVCAction == controller.CurrentMVCAction) && (menuItem.MVCController == controller.CurrentMVCController))
                    menuItem.IsSelected = true;
            }

            if (String.IsNullOrWhiteSpace(menuItem.StaticLink))
                menuItem.StaticLink = "#";

            menuItem.Children = new List<IMenuItem>();

            //-- CHECK TO SEE IF THE MENU ITEM SHOULD BE SELECTED --
            return (menuItem);
        }
    }
}