using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class RechnungDto : EntityDto
    {
        /// <summary>
        /// Sys ID
        /// </summary>
        public long sysid { get; set; }

        override public long getEntityId()
        {
            return sysid;
        }

        override public String getEntityBezeichnung()
        {
            return rechnung;
        }

        public long sysrun { get; set; }
        public long sysrntyp { get; set; }
        public long syswfuser { get; set; }
        public long sysls { get; set; }
        public long sysrwaehrung { get; set; }
        public long sysmwst { get; set; }
        public long sysfi { get; set; }
        public long sysbn { get; set; }
        public long sysperson { get; set; }
        public long sysob { get; set; }
        public long sysgebiet { get; set; }
        public long gebiet { get; set; }
        public DateTime? syscreate { get; set; }
        public DateTime? syschange { get; set; }
        public int kreis { get; set; }
        public bool art { get; set; }
        public int typ { get; set; }
        public DateTime? eindatum { get; set; }
        public String rechnung { get; set; }
        public String erechnung { get; set; }
        public String beleg { get; set; }
        public String beleg2 { get; set; }
        public String text { get; set; }
        public String konto { get; set; }
        public bool druck { get; set; }
        public DateTime? druckdatum { get; set; }
        public bool stornokz { get; set; }
        public double storno { get; set; }
        public DateTime? stornodatum { get; set; }
        public DateTime? belegdatum { get; set; }
        public DateTime? valutadatum { get; set; }
        public DateTime? faelligdatum { get; set; }
        public double gbetrag { get; set; }
        public double gsteuer { get; set; }
        public double fbetrag { get; set; }
        public double fsteuer { get; set; }
        public double kurs { get; set; }
        public bool einzug { get; set; }
        public bool nettingflag { get; set; }
        public bool bezahlt { get; set; }
        public DateTime? bezahltdatum { get; set; }
        public double letztezahlung { get; set; }
        public double teilzahlung { get; set; }
        public double verrechnung { get; set; }
        public double gebuehr { get; set; }
        public double fremdgebuehr { get; set; }
        public DateTime? zinsdatum { get; set; }
        public double zinsen { get; set; }
        public bool zahlsperre { get; set; }
        public DateTime? zahlsperrevon { get; set; }
        public DateTime? zahlsperrebis { get; set; }
        public double mahnbetrag { get; set; }
        public DateTime? mahndatum { get; set; }
        public bool mahnsperre { get; set; }
        public DateTime? msperrevon { get; set; }
        public DateTime? msperrebis { get; set; }
        public int mahnstufe { get; set; }
        public String zgiban { get; set; }
        public String zgbic { get; set; }
        public String zgfeld1 { get; set; }
        public String zgfeld2 { get; set; }
        public String zgfeld3 { get; set; }
        //public RNPOS rnpos { get; set; }
        //public RNZHAL rnzahl { get; set; }

    }
}

