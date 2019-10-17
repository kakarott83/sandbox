using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    /// <summary>
	/// Dto for GELESEN-Flag (rh 20170515)
    /// 
    /// </summary>
	/// 
	//// SYSPREAD  NOT NULL NUMBER(12)   
	//// FLAGREAD           NUMBER(3)    
	//// AREA               VARCHAR2(128)
	//// SYSID              NUMBER(12)   
	//// SYSWFUSER          NUMBER(12)   
	//// DATUM              DATE         
	//// UHRZEIT            NUMBER(10)   
    public class PreadDto : EntityDto
    {
		/// <summary>
        /// Primärschlüssel
        /// </summary>
		public long sysPread { get; set; }

		/// <summary>
		/// ID of read/unread row 
		/// </summary>
		public short flagRead { get; set; }

		/// <summary>
        /// OL Entity Area
        /// </summary>
        public String area { get; set; }

		/// <summary>
		/// ID of read/unread row 
		/// </summary>
		public long sysID { get; set; }

		/// <summary>
		/// sysWfUser-ID
		/// </summary>
		public String sysWfUser { get; set; }

		public DateTime? datum { get; set; }
		public long uhrzeit { get; set; }
		
		/// <summary>
		/// StatusTimeGUI-Helper
		/// </summary>
		public DateTime? uhrzeitGUI
		{
			get
			{
				return DateTimeHelper.ClarionTimeToDateTimeNoException ((int) uhrzeit);
			}
			set
			{
				int? val = DateTimeHelper.DateTimeToClarionTime (value);
				if (val.HasValue)
					uhrzeit = (long) val.Value;
				else
					uhrzeit = 0;
			}
		}
		
        /// <summary>
        /// Maybe needed?: Contains all primary keys of this generics data base tables
        /// </summary>
        public List<Pkey> pkeys { get; set; }
        
        override public long getEntityId()
        {
			return sysPread;
        }
        public override string getEntityBezeichnung()
        {
			return "Pread" + sysPread;
        }
    }
}