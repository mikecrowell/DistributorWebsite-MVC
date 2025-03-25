using DistributorWebsite.MVC.WebUI.Helpers.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Security
{
    /// <summary>
    /// Access token definition that will be returned from the identity server for the current user/distributor/salesperson
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// Get/set whether or not the current user must re login
        /// </summary>
        [JsonProperty("mrl")]
        [JsonConverter(typeof(BoolConverter))]
        public Boolean MustReLogin { get; set; }

        /// <summary>
        /// Get/set the user's current user id
        /// </summary>
        [JsonProperty("id")]
        public Int32 UserID { get; set; }

        /// <summary>
        /// Get/set the user's name
        /// </summary>
        [JsonProperty("nm")]
        public String Name { get; set; }

        /// <summary>
        /// Get/set the user's current username
        /// </summary>
        [JsonProperty("un")]
        public String UserName { get; set; }

        /// <summary>
        /// Get/set the active status
        /// </summary>
        [JsonProperty("act")]
        [JsonConverter(typeof(BoolConverter))]
        public Boolean IsActive { get; set; }

        /// <summary>
        /// Get/set whether or not the current user is an internal login
        /// </summary>
        [JsonProperty("hci")]
        [JsonConverter(typeof(BoolConverter))]
        public Boolean IsInternal { get; set; }

        /// <summary>
        /// Get/set whether or not the current user can access the InCite application
        /// </summary>
        [JsonProperty("sfa")]
        [JsonConverter(typeof(BoolConverter))]
        public Boolean CanAccessIncite { get; set; }

        /// <summary>
        /// Get/set the current entity's code
        /// </summary>
        [JsonProperty("ec")]
        public string EntityCode { get; set; }

        /// <summary>
        /// Get/set the current entity's name
        /// </summary>
        [JsonProperty("en")]
        public string EntityName { get; set; }

        /// <summary>
        /// Get/set the current distributor number
        /// </summary>
        [JsonProperty("dno", NullValueHandling = NullValueHandling.Ignore)]
        public String DistributorNo { get; set; }

        /// <summary>
        /// Get/set the salesperson code
        /// </summary>
        [JsonProperty("scd", NullValueHandling = NullValueHandling.Ignore)]
        public String SalespersonCode { get; set; }

        /// <summary>
        /// Get/set the user's current league
        /// </summary>
        [JsonProperty("lg")]
        public String League { get; set; }

        /// <summary>
        /// Get/set the user's default client
        /// </summary>
        [JsonProperty("dc")]
        public String DefaultClient { get; set; }

        /// <summary>
        /// Get/set the user's entity bit
        /// </summary>
        [JsonProperty("eb")]
        public Int32 EntityAttributeBit { get; set; }

        /// <summary>
        /// Get/set the user's brand bit
        /// </summary>
        [JsonProperty("bb")]
        public String Brand { get; set; }

        /// <summary>
        /// Get/set the user's state
        /// </summary>
        [JsonProperty("st")]
        public string State { get; set; }

        /// <summary>
        /// Get/set the user's country code
        /// </summary>
        [JsonProperty("cc")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Get/set the user's territory director
        /// </summary>
        [JsonProperty("td")]
        public string TerritoryDirector { get; set; }

        /// <summary>
        /// Get/set the home distributor number
        /// </summary>
        [JsonProperty("hd")]
        public string HomeDistributor { get; set; }

        /// <summary>
        /// Get/set the entity's supported client(s)
        /// </summary>
        [JsonProperty("cl")]
        public Int32 ClientBit { get; set; }

        /// <summary>
        /// Get/set the entity's security string
        /// </summary>
        [JsonProperty("ss")]
        public String SecurityString { get; set; }

        /// <summary>
        /// Get/set whether or not the current user can create Esignature order
        /// </summary>
        [JsonProperty("esd")]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsCreateEsignatureDisabled { get; set; }

        #region "CALCULATED PROPERTIES"

        #region "(READONLY) PROPERTY: EntityType"
        /// <summary>
        /// Get the entity type corresponding to the current entity code
        /// </summary>
        [JsonIgnore]
        public eEntityType EntityType
        {
            get
            {
                if (String.IsNullOrWhiteSpace(EntityCode))
                    return eEntityType.Other;
                else if (EntityCode.Length == 5)
                    return eEntityType.Distributor;
                else if (EntityCode.Length == 8)
                    return eEntityType.Salesperson;
                else
                    return eEntityType.Other;
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: LeagueBit"
        /// <summary>
        /// Get the league bit corresponding to the current user's league
        /// </summary>
        [JsonIgnore]
        public Int32 LeagueBit
        {
            get
            {
                if (String.IsNullOrWhiteSpace(League))
                    return (Int32)eLeague.Unknown;
                else if (League.Substring(0, 1) == "A")
                    return (Int32)eLeague.American;
                else if (League.Substring(0, 1) == "I")
                    return (Int32)eLeague.International;
                else if (League.Substring(0, 1) == "W")
                    return (Int32)eLeague.World;
                else
                    return (Int32)eLeague.Unknown;
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: DistributorLevel"
        /// <summary>
        /// Get the current distributor's level
        /// </summary>
        [JsonIgnore]
        public Int32? DistributorLevel
        {
            get
            {
                if ((EntityAttributeBit & (Int32)eType.Level1) == (Int32)eType.Level1)
                    return (1);
                else if ((EntityAttributeBit & (Int32)eType.Level2) == (Int32)eType.Level2)
                    return (2);
                else if ((EntityAttributeBit & (Int32)eType.Level3) == (Int32)eType.Level3)
                    return (3);
                else if ((EntityAttributeBit & (Int32)eType.Level4) == (Int32)eType.Level4)
                    return (4);
                else
                    return (null);
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: UserType"
        /// <summary>
        /// Get the current user type
        /// </summary>
        [JsonIgnore]
        public eUserType UserType
        {
            get
            {
                if ((EntityAttributeBit & (Int32)eType.Salesperson) == (Int32)eType.Salesperson)
                    return eUserType.Salesperson;
                else
                    return eUserType.Distributor;
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: IsTerritoryDirector"
        /// <summary>
        /// Get whether or not the current user is a territory director
        /// </summary>
        [JsonIgnore]
        public bool IsTerritoryDirector
        {
            get
            {
                return ((EntityAttributeBit & (Int32)eType.TD) == (Int32)eType.TD);
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: IsDistributor"
        /// <summary>
        /// Get whether or not the current user is a distributor
        /// </summary>
        [JsonIgnore]
        public bool IsDistributor
        {
            get
            {
                return ((EntityAttributeBit & (Int32)eType.Distributor) == (Int32)eType.Distributor);
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: IsSalesperson"
        /// <summary>
        /// Get whether or not the current user is a salesperson
        /// </summary>
        [JsonIgnore]
        public bool IsSalesperson
        {
            get
            {
                return ((EntityAttributeBit & (Int32)eType.Salesperson) == (Int32)eType.Salesperson);
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: IsTerritoryMaster"
        /// <summary>
        /// Get whether or not the current user is a territory master
        /// </summary>
        [JsonIgnore]
        public bool IsTerritoryMster
        {
            get
            {
                return ((EntityAttributeBit & (Int32)eType.Master) == (Int32)eType.Master);
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: IsNewcomer"
        /// <summary>
        /// Get whether or not the current user is a newcomer
        /// </summary>
        [JsonIgnore]
        public bool IsNewcomer
        {
            get
            {
                return ((EntityAttributeBit & (Int32)eType.Newcomer) == (Int32)eType.Newcomer);
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: IsInAngloMarketSegment"
        /// <summary>
        /// Get whether or not the current user is in the anglo market segment
        /// </summary>
        [JsonIgnore]
        public bool IsInAngloMarketSegment
        {
            get
            {
                return ((EntityAttributeBit & (Int32)eType.Anglo) == (Int32)eType.Anglo);
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: IsInHispanicMarketSegment"
        /// <summary>
        /// Get whether or not the current user is in the Hispanic market segment
        /// </summary>
        [JsonIgnore]
        public bool IsInHispanicMarketSegment
        {
            get
            {
                return ((EntityAttributeBit & (Int32)eType.Hispanic) == (Int32)eType.Hispanic);
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: IsJointVenture"
        /// <summary>
        /// Get whether or not the current user is a joint venture
        /// </summary>
        [JsonIgnore]
        public bool IsJointVenture
        {
            get
            {
                return ((EntityAttributeBit & (Int32)eType.JV) == (Int32)eType.JV);
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: IsFairShowDistributor"
        /// <summary>
        /// Get whether or not the current user is a fair/show distributor
        /// </summary>
        [JsonIgnore]
        public bool IsFairShowDistributor
        {
            get
            {
                return ((EntityAttributeBit & (Int32)eType.FairShowDistributor) == (Int32)eType.FairShowDistributor);
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: IsBridalDistributor"
        /// <summary>
        /// Get whether or not the current user is a bridal distributor
        /// </summary>
        [JsonIgnore]
        public bool IsBridalDistributor
        {
            get
            {
                return ((EntityAttributeBit & (Int32)eType.BridalDistributor) == (Int32)eType.BridalDistributor);
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: Brand"
        /// <summary>
        /// Get whether or not the current user is a territory director
        /// </summary>
        [JsonIgnore]
        public Int32 BrandBit
        {
            get
            {
                eBrand brand;

                try
                {
                    if (!Enum.TryParse<eBrand>(this.Brand, out brand))
                        brand = eBrand.Unknown;
                }
                catch (Exception)
                {
                    brand = eBrand.Unknown;
                }

                return (Int32)brand;
            }
        }
        #endregion

        #region "(READONLY) PROPERTY: Clients"
        /// <summary>
        /// Get/set the list of supported clients
        /// </summary>
        [JsonIgnore]
        public string[] Clients
        {
            get
            {
                List<String> clients = new List<string>();

                if ((this.ClientBit & (Int32)eClient.BA) == (Int32)eClient.BA)
                    clients.Add("BA");

                if ((this.ClientBit & (Int32)eClient.BR) == (Int32)eClient.BR)
                    clients.Add("BR");

                if ((this.ClientBit & (Int32)eClient.BZ) == (Int32)eClient.BZ)
                    clients.Add("BZ");

                if ((this.ClientBit & (Int32)eClient.CO) == (Int32)eClient.CO)
                    clients.Add("CO");

                if ((this.ClientBit & (Int32)eClient.DR) == (Int32)eClient.DR)
                    clients.Add("DR");

                if ((this.ClientBit & (Int32)eClient.HC) == (Int32)eClient.HC)
                    clients.Add("HC");

                if ((this.ClientBit & (Int32)eClient.MX) == (Int32)eClient.MX)
                    clients.Add("MX");

                if ((this.ClientBit & (Int32)eClient.PE) == (Int32)eClient.PE)
                    clients.Add("PE");

                if ((this.ClientBit & (Int32)eClient.PH) == (Int32)eClient.PH)
                    clients.Add("PH");

                if ((this.ClientBit & (Int32)eClient.RC) == (Int32)eClient.RC)
                    clients.Add("RC");

                if ((this.ClientBit & (Int32)eClient.EC) == (Int32)eClient.EC)
                    clients.Add("EC");

                return (clients.ToArray());
            }
        }
        #endregion

        #endregion

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
        public static AccessToken Parse(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AccessToken>(json);
        }
        #endregion

        #region "FUNCTION: FromBase64String"
        /// <summary>
        /// Convert from a base 64 string
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static AccessToken ParseBase64String(string base64String)
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