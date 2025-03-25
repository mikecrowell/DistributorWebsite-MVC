using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Security.Tokens
{
    public abstract class UserAccessTokenBase : IUserAccessToken
    {
        /// <summary>
        /// Get/set the User ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Get/set the username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Get/set the entity code
        /// </summary>
        public string EntityCode { get; set; }

        /// <summary>
        /// Get/set the client security bit(s)
        /// </summary>
        public int ClientBits { get; set; }

        /// <summary>
        /// Get/set the brand security bit(s)
        /// </summary>
        public int BrandBits { get; set; }

        /// <summary>
        /// Get/set the league security bit(s)
        /// </summary>
        public int LeagueBits { get; set; }

        /// <summary>
        /// Get/set the type security bit(s)
        /// </summary>
        public int TypeBits { get; set; }

        /// <summary>
        /// Get/set the current state restriction
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Get/set the current country restriction
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Get/set the current security string
        /// </summary>
        public string SecurityString { get; set; }

        /// <summary>
        /// Get/set whether or not this is an internal login
        /// </summary>
        public bool IsInternalLogin { get; set; }

        /// <summary>
        /// Get/set whether or not to validate identity
        /// </summary>
        public bool ValidateIdentity { get; set; }

        /// <summary>
        /// Check to see if the user can access the specified security item
        /// </summary>
        /// <param name="securityBit"></param>
        /// <returns></returns>
        public bool CanAccess(eSecurityItem securityBit)
        {
            return Security.AccessToken.CanAccess(this.SecurityString, (Int32)securityBit);
        }

        /// <summary>
        /// Check to see if the current user can access all the specified security bits
        /// </summary>
        /// <param name="securityBits"></param>
        /// <returns></returns>
        public bool CanAccessAll(eSecurityItem[] securityBits)
        {
            foreach (eSecurityItem sec in securityBits)
            {
                if (!CanAccess(sec))
                    return (false);
            }

            return (true);
        }

        /// <summary>
        /// Check to see if the current user can access any of the specified security bits
        /// </summary>
        /// <param name="securityBits"></param>
        /// <returns></returns>
        public bool CanAccessAny(eSecurityItem[] securityBits)
        {
            foreach (eSecurityItem sec in securityBits)
            {
                if (CanAccess(sec))
                    return (true);
            }

            return (false);
        }

        #region "FUNCTION: ToString"
        /// <summary>
        /// Generate a formatted string representation of the current object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"================================================================" + Environment.NewLine +
                   $"USER ACCESS TOKEN" + Environment.NewLine +
                   $"User ID: {this.UserID}" + Environment.NewLine +
                   $"User Name: {this.UserName}" + Environment.NewLine +
                   $"Entity Code: {this.EntityCode}" + Environment.NewLine +
                   $"Client Bits: {this.ClientBits}" + Environment.NewLine +
                   $"Brand Bits: {this.BrandBits}" + Environment.NewLine +
                   $"League Bits: {this.LeagueBits}" + Environment.NewLine +
                   $"Type Bits: {this.TypeBits}" + Environment.NewLine +
                   $"State: {this.State}" + Environment.NewLine +
                   $"Country: {this.Country}" + Environment.NewLine +
                   $"SS: {this.SecurityString}" + Environment.NewLine +
                   $"Internal: {this.IsInternalLogin}" + Environment.NewLine +
                   $"Validate Identity: {this.ValidateIdentity}" + Environment.NewLine +
                   $"=================================================================" + Environment.NewLine;
        }
        #endregion
    }
}