using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributorWebsite.MVC.WebUI.Helpers
{
    public interface IMenuItem
    {
        /// <summary>
        /// Get/set the item's security index
        /// </summary>
        Int32 SecurityIndex { get; set; }

        /// <summary>
        /// Get/set the font awesome icon to display next to the current menu item
        /// </summary>
        String CSSIcon { get; set; }

        /// <summary>
        /// Get/set the css class to use when the current item is selected
        /// </summary>
        String CSSSelectedClass { get; set; }

        /// <summary>
        /// Get/set whether or not the current menu item should be selected
        /// </summary>
        Boolean IsSelected { get; set; }

        /// <summary>
        /// Get/set the security item name
        /// </summary>
        String ItemName { get; set; }

        /// <summary>
        /// Get/set the security item text
        /// </summary>
        String ItemText { get; set; }

        /// <summary>
        /// Get/set the MVC Action
        /// </summary>
        String MVCAction { get; set; }

        /// <summary>
        /// Get/set the MVC Controller
        /// </summary>
        String MVCController { get; set; }

        /// <summary>
        /// Get/set the sequence
        /// </summary>
        Int32 Sequence { get; set; }

        /// <summary>
        /// Get/set the static link
        /// </summary>
        String StaticLink { get; set; }

        /// <summary>
        /// Get/set whether or not to display the link in a new window
        /// </summary>
        Boolean ShowInNewWindow { get; set; }

        /// <summary>
        /// Get/set the security item link
        /// </summary>
        eSecurityItem? SecurityItem { get; set; }

        /// <summary>
        /// Get/set the list of child menu items
        /// </summary>
        List<IMenuItem> Children { get; set; }
    }
}
