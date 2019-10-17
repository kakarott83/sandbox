using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class igetValidateUserDto
    {
        /// <summary>
        /// Benutzername
        /// </summary>
        public string username
        {
            get;
            set;
        }

        /// <summary>
        /// Password
        /// </summary>
        public string password
        {
            get;
            set;
        }

        /// <summary>
        /// Technisches Passwort
        /// </summary>
        public string presharedKey
        {
            get;
            set;
        }

        /// <summary>
        /// GUID
        /// </summary>
        public string guid
        {
            get;
            set;
        }

        /// <summary>
        /// vlmCode
        /// </summary>
        public string vlmCode
        {
            get;
            set;
        }

        /// <summary>
        /// 0 = WFUSER
        /// 1 = PUSER
        /// </summary>
        public int userType { get; set; }
    }
}