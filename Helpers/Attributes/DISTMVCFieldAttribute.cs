using HyCite.MVC.DynamicDisplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI
{
    public class DISTMVCField : HCDynamicMVCFieldAttribute
    {
        public const eHCDynamicFieldOptions SORTSEARCHFILTER = eHCDynamicFieldOptions.Sortable | eHCDynamicFieldOptions.Searchable | eHCDynamicFieldOptions.Filterable;
        public const eHCDynamicFieldOptions SORTSEARCHFILTERGRID = SORTSEARCHFILTER | eHCDynamicFieldOptions.DisplayInGrid;
        public const eHCDynamicFieldOptions SORTSEARCHFILTERGRIDNOXS = SORTSEARCHFILTERGRID | eHCDynamicFieldOptions.HiddenExtraSmall;
        public const eHCDynamicFieldOptions SORTSEARCHFILTERGRIDNOXSNOSM = SORTSEARCHFILTERGRIDNOXS | eHCDynamicFieldOptions.HiddenSmall;
        public const eHCDynamicFieldOptions SORTSEARCHFILTERGRIDNOXSNOSMNOMD = SORTSEARCHFILTERGRIDNOXSNOSM | eHCDynamicFieldOptions.HiddenMedium;

        #region "CONSTRUCTORS"

        #region "CONSTRUCTOR"
        public DISTMVCField(DistributorWebsite.MVC.LocalResources.eResourceKey resourceKey, eHCDynamicFieldOptions options = SORTSEARCHFILTERGRID, Int16 gridColumnOrder = 1, Int16 searchRowOrder = 1, eHCDynamicFieldOptions extraOptions = eHCDynamicFieldOptions.None, String angularFormatString = "", eHCAngularFormats angularFormat = eHCAngularFormats.Default, String[] selectItemList = null) : base(LocalResources.HCResources.ResourceManager, resourceKey.ToString(), options, gridColumnOrder, searchRowOrder, extraOptions, angularFormatString, angularFormat, selectItemList)
        {
            this.Label = LocalResources.HCResources.ResourceManager.GetString(resourceKey.ToString());
        }
        #endregion

        #region "CONSTRUCTOR"
        /// <summary>
        /// Must have a constructor that matches the default constructor on the HCDynamicMVCFieldAttribute class
        /// </summary>
        /// <param name="label"></param>
        /// <param name="options"></param>
        /// <param name="gridColumnOrder"></param>
        /// <param name="searchRowOrder"></param>
        /// <param name="extraOptions"></param>
        /// <param name="angularFormatString"></param>
        /// <param name="angularFormat"></param>
        public DISTMVCField(String label, eHCDynamicFieldOptions options = eHCDynamicFieldOptions.Sortable | eHCDynamicFieldOptions.DisplayInGrid | eHCDynamicFieldOptions.Searchable | eHCDynamicFieldOptions.Filterable, Int16 gridColumnOrder = 1, Int16 searchRowOrder = 1, eHCDynamicFieldOptions extraOptions = eHCDynamicFieldOptions.None, String angularFormatString = "", eHCAngularFormats angularFormat = eHCAngularFormats.Default, String[] selectItemList = null):base(label, options, gridColumnOrder, searchRowOrder, extraOptions, angularFormatString, angularFormat, selectItemList)
        {

        }
        #endregion

        #endregion
    }

}