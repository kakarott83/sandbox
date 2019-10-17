using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Describes Data sent to Aconso DMS
    /// </summary>
    public class DMSExportDataDto
    {
        public long KNR { get; set; }
        public String AKTEID { get; set; }
        public String VORGNR { get; set; }
        public String AS2 { get; set; }
        public String CHANNELID { get; set; }
        public long ID { get; set; }
        public long ITID { get; set; }
        public String STATUS { get; set; }
        public String NAME { get; set; }
        public String VORNA { get; set; }
        public String ZUSATZ { get; set; }
        public String PLZ { get; set; }
        public String ORT { get; set; }
        public String STRNR { get; set; }
        public DateTime? GEB { get; set; }
        public String KONTY { get; set; }
        public int KUNTY { get; set; }
        public int CSMA { get; set; }
        public long sysdmsupl { get; set; }
        public long SYSKD { get; set; }

        //File Attributes
        public String ATT_FILE_TYPE { get; set; }
        public String ATT_SCAN_DATE { get; set; }
        public String ATT_DOC_DATE { get; set; }
        public String ATT_PAGE_COUNT { get; set; }
        public String ATT_CHANNEL_TYPE { get; set; }
        public String ATT_CHANNEL_INFO { get; set; }
        public String ATT_PRODUCT { get; set; }
        public String ATT_LANGUAGE { get; set; }
        public String ATT_DOC_TYPE { get; set; }
        

        //filedata
        public byte[] FILEDATA { get; set; }
        public String FILENAME { get; set; }

        public String AREA { get; set; }
        public String ZUSTAND { get; set; }
        public String ATTRIBUT { get; set; }

        public String REFERENZ { get; set; }
		public String ARCHIVE { get; set; }

        public String PARTNERNR { get; set; }
        public String HAENDLNR { get; set; }
        public String TYPE { get; set; }
        public String AKTIV { get; set; }

        public DateTime? RETENTIONDATE { get; set; }
    }
}
