// OWNER MK, 06-07-2009
using Cic.OpenOne.Common.Util;
using System;
namespace Cic.OpenLease.ServiceAccess
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class MessageHeader
    {
        #region Constructors
        public MessageHeader(string userName, string password, long sysBRAND, long sysPEROLE, string isoLanguageCode)
        {
            if (StringUtil.IsTrimedNullOrEmpty(userName))
            {
                throw new Exception("userName");
            }

            this.UserName = userName;
            this.Password = password;
            this.SysBRAND = sysBRAND;
            this.SysPEROLE = sysPEROLE;
            this.ISOLanguageCode = isoLanguageCode;
        }
        #endregion

        #region Properties
		[System.Runtime.Serialization.DataMember]
		public string UserName
		{
			get;
			set;
		}

		[System.Runtime.Serialization.DataMember]
        public string Password
        {
            get;
			set;
        }

        [System.Runtime.Serialization.DataMember]
        public long SysBRAND
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public long SysPEROLE
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string ISOLanguageCode
        {
            get;
            set;
        }
        #endregion
    }
    
}