
namespace Cic.OpenOne.Common.Util.SOAP
{
    /// <summary>
    /// DefaultMessageHeader-Klasse
    /// </summary>
    [System.CLSCompliant(true)]
    public class DefaultMessageHeader : IMessageHeader
    {
        private const string id = "DefaultMessageHeader";
        private const string ns = "http://cic-software.de/MessageHeader";

        /// <summary>
        /// DefaultMessageHeader-Konstruktor
        /// </summary>
        public DefaultMessageHeader()
        {
        }

        /// <summary>
        /// DefaultMessageHeader-Konstruktor
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="isoLanguageCode"></param>
        /// <param name="userType"></param>
        public DefaultMessageHeader(string userName, string password, long sysPEROLE, string isoLanguageCode, int userType)
        {
            this.UserName = userName;
            this.Password = password;
            this.SysPEROLE = sysPEROLE;
            this.ISOLanguageCode = isoLanguageCode;
            this.UserType = userType;
        }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// SysPEROLE
        /// </summary>
        public long SysPEROLE { get; set; }

        /// <summary>
        /// ISOLanguageCode
        /// </summary>
        public string ISOLanguageCode { get; set; }

        /// <summary>
        /// User-Type: PUSER = 0, WFUSER = 1
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// getNamespace
        /// </summary>
        /// <returns></returns>
        public string getNamespace()
        {
            return ns;
        }

        /// <summary>
        /// getID
        /// </summary>
        /// <returns></returns>
        public string getID()
        {
            return id;
        }
    }
}