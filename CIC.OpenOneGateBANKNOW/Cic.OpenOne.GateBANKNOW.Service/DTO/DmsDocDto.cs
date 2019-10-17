using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    public class DmsDocDto
    {
        /// <summary>
        /// Primary tech. Key
        /// </summary>
        public long sysdmsdoc { get; set; }

        /// <summary>
        /// File binary content
        /// </summary>
        public byte[] inhalt { get; set; }
        
        /// <summary>
        /// File title
        /// </summary>
        public String name { get; set; }

        /// <summary>
        /// Filename
        /// </summary>
        public String dateiname { get; set; }

        /// <summary>
        /// File format (PDF)
        /// </summary>
        public String format { get; set; }

        /// <summary>
        /// File type
        /// </summary>
        public String typ { get; set; }

        /// <summary>
        /// File remark
        /// </summary>
        public String bemerkung { get; set; }

        /// <summary>
        /// File printdate
        /// </summary>
        public String gedrucktvon { get; set; }

        /// <summary>
        /// File printtime
        /// </summary>
        public DateTime? gedrucktam { get; set; }

        /// <summary>
        /// File invalid from date
        /// </summary>
        public String ungueltigvon { get; set; }

        /// <summary>
        /// File invalid time from
        /// </summary>
        public DateTime? ungueltigam { get; set; }

        /// <summary>
        /// File invalid flag
        /// </summary>
        public int ungueltigflag { get; set; }

        /// <summary>
        /// File invalidation comment
        /// </summary>
        public String ungueltigcomment { get; set; }

        /// <summary>
        /// File Search keywords
        /// </summary>
        public String searchterms { get; set; }

        /// <summary>
        /// File template id
        /// </summary>
        public long syswftx { get; set; }

        /// <summary>
        /// File template code
        /// </summary>
        public String wftxcode { get; set; }
    }
}
