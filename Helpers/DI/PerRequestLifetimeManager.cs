using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity.Lifetime;

namespace DistributorWebsite.MVC.WebUI.Helpers.DI
{
    /// <summary>
    /// Unity lifetime manager to keep an object alive for the duration of an individual request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PerRequestLifetimeManager<T> : LifetimeManager, IDisposable
    {
        #region "PROPERTY: DIKey"
        /// <summary>
        /// Get the fully qualified name of the type T to use as the key in the current HttpContext
        /// </summary>
        private string DIKey
        {
            get
            {
                return typeof(T).AssemblyQualifiedName;
            }
        }
        #endregion

        #region "PROCEDURE: SetValue"
        /// <summary>
        /// Create an instance of the object in the current http context
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="container"></param>
        public override void SetValue(object newValue, ILifetimeContainer container = null)
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items[DIKey] = newValue;
        }
        #endregion

        #region "PROCEDURE: GetValue"
        /// <summary>
        /// Get a value from the current HttpContext
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public override object GetValue(ILifetimeContainer container = null)
        {
            if (HttpContext.Current != null && HttpContext.Current.Items.Contains(DIKey))
                return HttpContext.Current.Items[DIKey];
            else
                return null;
        }
        #endregion

        #region "PROCEDURE: RemoveValue"
        /// <summary>
        /// Remove a cached value from the current HttpContext
        /// </summary>
        /// <param name="container"></param>
        public override void RemoveValue(ILifetimeContainer container = null)
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items.Remove(DIKey);
        }
        #endregion

        #region "PROCEDURE: Dispose"
        /// <summary>
        /// Remove the item if it hasn't been removed yet
        /// </summary>
        public void Dispose()
        {
            RemoveValue();
        }
        #endregion

        #region "FUNCTION: OnCreateLifetimeManager"
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override LifetimeManager OnCreateLifetimeManager()
        {
            System.Diagnostics.Trace.TraceInformation("OnCreateLifetimeManager");
            return (null);
        }
        #endregion
    }
}