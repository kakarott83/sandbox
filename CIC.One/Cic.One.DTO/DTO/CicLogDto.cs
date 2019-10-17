using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// EaihotDto
    /// </summary>
	public class CicLogDto : EntityDto
	{

		// Name        Null     Typ           
		// ----------- -------- ------------- 
		// SYSCICLOG   NOT NULL NUMBER(12)    
		// LOGINDATE            DATE          
		// LOGOUTDATE           DATE          
		// ORABENUTZER          VARCHAR2(20)  
		// CICBENUTZER          VARCHAR2(20)  
		// MASCHINE             VARCHAR2(20)  
		// ID                   NUMBER(12)    
		// SOURCE               VARCHAR2(128) 


        /// <summary>
        /// primary key
        /// </summary>
		public long sysciclog { get; set; }

        /// <summary>
        /// code
        /// </summary>
		public DateTime? logindate { get; set; }

        /// <summary>
        /// oltable
        /// </summary>
		public DateTime? logoutdate { get; set; }

        /// <summary>
        /// sysoltable
        /// </summary>
		public string orabenutzer { get; set; }

        /// <summary>
        /// prozessstatus
        /// </summary>
		public string cicbenutzer { get; set; }

        /// <summary>
        /// sysportal
        /// </summary>
		public string maschine { get; set; }

        /// <summary>
        /// syseaiart
        /// </summary>
		public long? id { get; set; }

        /// <summary>
        /// fileflagin
        /// </summary>
		public string source { get; set; }


        public override long getEntityId()
        {
			return sysciclog;
        }
    }
}