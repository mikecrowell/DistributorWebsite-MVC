using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributorWebsite.MVC.WebUI.Helpers.Security
{
    /// <summary>
    /// Define a generic user access token interface
    /// </summary>
    public interface IUserAccessToken
    {
        /// <summary>
        /// Get/set the user ID
        /// </summary>
        Int32 UserID { get; set; }

        /// <summary>
        /// Get/set the user name
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Get/set the entity code
        /// </summary>
        string EntityCode { get; set; }

        /// <summary>
        /// Get/set the client bits for the current user/entity
        /// </summary>
        Int32 ClientBits { get; set; }

        /// <summary>
        /// Get/set the brand bits for the current user/entity
        /// </summary>
        Int32 BrandBits { get; set; }

        /// <summary>
        /// Get/set the league bits for the current user/entity
        /// </summary>
        Int32 LeagueBits { get; set; }

        /// <summary>
        /// Get/set the type bits for the current user/entity
        /// </summary>
        Int32 TypeBits { get; set; }

        /// <summary>
        /// Get/set the current entity's state of residence
        /// </summary>
        String State { get; set; }

        /// <summary>
        /// Get/set the current entity's country of residence
        /// </summary>
        String Country { get; set; }

        /// <summary>
        /// Get/set the security string for the current user/entity
        /// </summary>
        String SecurityString { get; set; }

        /// <summary>
        /// Get/set whether or not this is an internal login
        /// </summary>
        bool IsInternalLogin { get; set; }

        /// <summary>
        /// Get/set whether or not to perform the identity server check(s)
        /// </summary>
        bool ValidateIdentity { get; set; }

        /// <summary>
        /// Determine whether or not the current user has access to the specified security item
        /// </summary>
        /// <param name="securityBit"></param>
        /// <returns></returns>
        bool CanAccess(eSecurityItem securityBit);

        /// <summary>
        /// Determine whether or not the current user can access any of the specified security items
        /// </summary>
        /// <param name="securityBits"></param>
        /// <returns></returns>
        bool CanAccessAny(eSecurityItem[] securityBits);

        /// <summary>
        /// Determine whether or not the current user can access all of the specified security items
        /// </summary>
        /// <param name="securityBits"></param>
        /// <returns></returns>
        bool CanAccessAll(eSecurityItem[] securityBits);
    }
}
