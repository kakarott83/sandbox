using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// VT Objekt OB
    /// </summary>
    public class ObDto : ObjektDto
    {
        /*	Verweis zur Person (Händler)	*/
        public long sysHd { get; set; }
        /*	Verweis zur Finanzierung (NKK)	*/
        public long sysNkk { get; set; }
        /*	Zustand gesetzt am	*/
        public DateTime? zustandam { get; set; }
        /*	Standort (via Lookup ddlkppos)	*/
        public String standortbrief { get; set; }
        /*	Sonderausstattung in Rechnungswährung	*/
        public double sonzub { get; set; }
        /*	Sperre Freigabe Kfz-Brief	*/
        public int sperrefreigabe { get; set; }
        /*	Sperre Zahlung (Objektpreis)	*/
        public int sperrezahlung { get; set; }
        /*	Ordertype	*/
        public String ordertype { get; set; }
        /*	Pakete in Rechnungswährung	*/
        public double pakete { get; set; }
        /*	Herstellerzubehör in Rechnungswährung	*/
        public double herzub { get; set; }

        public long kmbmw { get; set; }
        public long kmstand { get; set; }


        public DateTime? datum { get; set; }
        public String source { get; set; }
        public String source2  { get; set; }

        public ObbriefDto obbrief { get; set; }
        public ObIniDto zusatzdaten {get;set;}

        /// <summary>
        /// Briefnummer
        /// </summary>
        public String brief { get; set; }
    }
}