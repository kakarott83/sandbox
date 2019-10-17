using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Entity for DMSDOC Table
    /// </summary>
    public class DmsdocDto:EntityDto
    {
        /// <summary>
        /// DMSDOCAREA Settings
        /// </summary>
        public String area { get; set; }
        public long sysid { get; set; }
        public long sysdmsdocarea { get; set; }

        public long sysdmsdoc { get; set; }
        public byte[] inhalt { get;set;}
        public String name { get; set; }
        public String dateiname { get; set; }
        public String bemerkung { get; set; }
        public String searchterms { get; set; }
        public DateTime? gedrucktam { get; set; }
        public int ungueltigflag { get; set; }
        public long syswftx { get; set; }
        
       

        override public long getEntityId()
        {
            return sysdmsdoc;
        }
    }
}
