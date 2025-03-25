using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace DistributorWebsite.MVC.WebUI.Helpers.Security
{
    /// <summary>
    /// Define the root information about the current user
    /// </summary>
    public class UserInfoClaim
    {
        /// <summary>
        /// Get/set the current user's user id
        /// </summary>
        [JsonProperty("id")]
        public Int32 UserID { get; set; }

        /// <summary>
        /// Get/set the current user's default brand
        /// </summary>
        [JsonProperty("brand")]
        public string Brand { get; set; }

        /// <summary>
        /// Get/set the current user's username
        /// </summary>
        [JsonProperty("un")]
        public string UserName { get; set; }

        /// <summary>
        /// Get/set the current user's first name
        /// </summary>
        [JsonProperty("fn")]
        public string FirstName { get; set; }

        /// <summary>
        /// Get/set the current user's middle name
        /// </summary>
        [JsonProperty("mn")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Get/set the current user's last name
        /// </summary>
        [JsonProperty("ln")]
        public string LastName { get; set; }

        /// <summary>
        /// Get/set the current user's full name
        /// </summary>
        [JsonProperty("nm")]
        public string FullName { get; set; }

        /// <summary>
        /// Get/set whether or not the current user is a master user
        /// </summary>
        [JsonProperty("mstr")]
        public bool IsMasterUser { get; set; }

        /// <summary>
        /// Get/set whether or not the current user is a hycite employee logging in internally
        /// </summary>
        [JsonProperty("hcemp")]
        public bool IsInternalUser { get; set; }

        /// <summary>
        /// Get/set the current user's distributor number
        /// </summary>
        [JsonProperty("dno")]
        public string DistributorNo { get; set; }

        /// <summary>
        /// Get/set the current user's salesperson code
        /// </summary>
        [JsonProperty("spcd")]
        public string SalespersonCode { get; set; }

        /// <summary>
        /// Get/set the user type
        /// </summary>
        [JsonProperty("type")]
        public eEntityType UserType { get; set; }

        /// <summary>
        /// Get/set the user's email address
        /// </summary>
        [JsonProperty("email")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Get/set the user's language preference
        /// </summary>
        [JsonProperty("lang")]
        public string LanguagePreference { get; set; }

        /// <summary>
        /// Get/set whether or not the user needs to re-login
        /// </summary>
        [JsonProperty("mrl")]
        public bool MustReLogin { get; set; }

        /// <summary>
        /// Get/set the list of linked distributors
        /// </summary>
        [JsonProperty("dlst", NullValueHandling = NullValueHandling.Ignore)]
        public UserInfoDistributor[] DistributorLinks { get; set; }

        /// <summary>
        /// Get/set the entity context the current user is trying to use
        /// </summary>
        [JsonProperty("entity", NullValueHandling = NullValueHandling.Ignore)]
        public UserInfoEntity Entity { get; set; }

        #region "STATIC HELPER FUNCTIONS FOR SECURITY AND PARSING"

        #region "FUNCTION: ToString"
        /// <summary>
        /// Return the JSON string representation of the current class
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.None);
        }
        #endregion

        #region "FUNCTION: ToStringIndented"
        /// <summary>
        /// Return the indented JSON string
        /// </summary>
        /// <returns></returns>
        public string ToStringIndented()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion

        #region "FUNCTION: ToBase64String"
        /// <summary>
        /// Convert to a base 64 string
        /// </summary>
        /// <returns></returns>
        public string ToBase64String()
        {
            var source = System.Text.Encoding.UTF8.GetBytes(this.ToString());
            return System.Convert.ToBase64String(source);
        }
        #endregion

        #region "FUNCTION: Parse"
        /// <summary>
        /// Parse the json string into an object
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static UserInfoClaim Parse(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoClaim>(json);
        }
        #endregion

        #region "FUNCTION: FromBase64String"
        /// <summary>
        /// Convert from a base 64 string
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static UserInfoClaim ParseBase64String(string base64String)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64String);
            string decodedString = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            return Parse(decodedString);
        }
        #endregion

        #endregion

        #region "STATIC HELPER FUNCTIONS FOR SECURITY AND PARSING"

        #region "FUNCTION: CompressUserSecurityString"
        /// <summary>
        /// Compress the user security string into a hex value
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static String CompressUserSecurityString(String flags)
        {
            String binaryString;
            Int32 rest;
            String output = "";

            try
            {
                binaryString = flags.Replace("Y", "1").Replace("N", "0");
                rest = binaryString.Length % 4;

                if (rest != 0)
                    binaryString = binaryString + new string('0', 4 - rest);    //-- PAD THE STRING LENGHT TO BE DIVISIBLE BY 4 --

                for (int i = 0; i <= binaryString.Length - 4; i += 4)
                {
                    output += string.Format("{0:X}", Convert.ToByte(binaryString.Substring(i, 4), 2));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return output;
        }
        #endregion

        #region "FUNCTION: DecompressUserSecurityString"
        /// <summary>
        /// Decompress the specified user security string
        /// </summary>
        /// <param name="compressedString"></param>
        /// <returns></returns>
        public static String DecompressUserSecurityString(String compressedString)
        {
            String retVal;

            try
            {
                retVal = String.Join(String.Empty, compressedString.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                retVal = new string('0', 256);
            }

            return (retVal);
        }
        #endregion

        #region "FUNCTION: CanAccess"
        /// <summary>
        /// Determine whether or not the user can access the specified security index
        /// </summary>
        /// <param name="securityString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Boolean CanAccess(String securityString, Int32 securityIndex)
        {
            String ss;

            try
            {
                //-- DECOMPRESS AND VALIDATE THE SECURITY STRING --
                ss = GetUncompressedSecurityString(securityString);
                if (ss == null)
                    return (false);

                //-- CHECK TO SEE IF THE USER HAS ACCESS --
                if (securityIndex > (ss.Length - 1))
                    return false;
                else if ((ss.Substring(securityIndex, 1) == "1") || (ss.Substring(securityIndex, 1) == "Y"))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }

            return false;
        }
        #endregion

        #region "FUNCTION: SetSecurityBit"
        /// <summary>
        /// Set the specified security bit
        /// </summary>
        /// <param name="securityString"></param>
        /// <param name="securityIndex"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String SetSecurityBit(String securityString, Int32 securityIndex)
        {
            return UpdateSecurityBit(securityString, securityIndex, '1');
        }
        #endregion

        #region "FUNCTION: UnsetSecurityBit"
        /// <summary>
        /// Unset the specified security bit
        /// </summary>
        /// <param name="securityString"></param>
        /// <param name="securityIndex"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String UnsetSecurityBit(String securityString, Int32 securityIndex)
        {
            return UpdateSecurityBit(securityString, securityIndex, '0');
        }
        #endregion

        #region "FUNCTION: UpdateSecurityBit"
        /// <summary>
        /// Set/unset the specified security bit
        /// </summary>
        /// <param name="securityString"></param>
        /// <param name="securityIndex"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private static String UpdateSecurityBit(String securityString, Int32 securityIndex, Char status)
        {
            String ss;
            System.Text.StringBuilder sb;

            try
            {
                //-- GET THE UNCOMPRESSED SECURITY STRING --
                ss = GetUncompressedSecurityString(securityString);
                if (ss == null)
                    return (securityString);

                //-- CHECK TO SEE IF THE USER HAS ACCESS --
                if (securityIndex > (ss.Length - 1))
                    return securityString;
                else if ((ss.Substring(securityIndex, 1) == status.ToString()))
                    return securityString;
                else
                {
                    sb = new StringBuilder(ss);
                    sb[securityIndex] = status;

                    switch (securityString.Length)
                    {
                        case 64:
                            return CompressUserSecurityString(sb.ToString());

                        default:
                            return sb.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.ToString());
            }

            return securityString;
        }
        #endregion

        #region "FUNCTION: GetUncompressedSecurityString"
        /// <summary>
        /// Get the uncompressed security string
        /// </summary>
        /// <param name="securityString"></param>
        /// <returns></returns>
        private static string GetUncompressedSecurityString(string securityString)
        {
            System.Text.RegularExpressions.Regex regex;

            //-- MAKE SURE WE HAVE A VALID COMPRESSED OR UNCOMPRESSED SECURITY STRING --
            if (String.IsNullOrWhiteSpace(securityString))
                return null;

            securityString = securityString.ToUpper();

            if (securityString.Length == 64)
            {
                //-- PARSE THE COMPRESSED SECURITY STRING --
                regex = new System.Text.RegularExpressions.Regex("^[0-9A-F]{64}$");
                if (!regex.IsMatch(securityString.ToUpper()))
                    return null;

                return (DecompressUserSecurityString(securityString));
            }
            else if (securityString.Length == 256)
            {
                //-- PARSE THE UNCOMPRESSED SECURITY STRING --
                securityString = securityString.ToUpper();
                regex = new System.Text.RegularExpressions.Regex("^[0-1]{256}$");
                if (!regex.IsMatch(securityString))
                {
                    regex = new System.Text.RegularExpressions.Regex("^[N,Y]{256}$");
                    if (!regex.IsMatch(securityString))
                        return null;
                }

                return (securityString);
            }
            else
                //-- INVALID SECURITY STRING LENGTH --
                return null;
        }
        #endregion

        #endregion
    }
}