// OWNER MK, 06-07-2009
using System.Web.Services.Protocols;
using System.Xml.Serialization;
namespace Cic.OpenOne.CarConfigurator.BO
{
    /// <summary>
    /// Message Header
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
    public class MessageHeader 
    {
        #region Constructors
        /// <summary>
        /// Standard Constructor is needed for serialization
        /// </summary>
        public MessageHeader() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="sysBRAND"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="isoLanguageCode"></param>
        public MessageHeader(string userName, string password, long sysBRAND, long sysPEROLE, string isoLanguageCode)
        {
            if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(userName))
            {
                throw new System.ArgumentNullException("userName");
            }

            this.UserName = userName;
            this.Password = password;
            this.SysBRAND = sysBRAND;
            this.SysPEROLE = sysPEROLE;
            this.ISOLanguageCode = isoLanguageCode;
        }
        #endregion

        #region Properties

        /// <summary>
        /// User Name
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Password
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Sys Brand
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long SysBRAND
        {
            get;
            set;
        }

        /// <summary>
        /// Sys Perole
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long SysPEROLE
        {
            get;
            set;
        }

        /// <summary>
        /// ISO Language Code
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string ISOLanguageCode
        {
            get;
            set;
        }
        #endregion
    }
}