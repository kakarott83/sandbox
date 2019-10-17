using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;

namespace Cic.One.DTO
{
    /// <summary>
    /// Wird aus der View VC_VORGANG_RED oder VC_ANGANT geladen und ist eine union View
    /// </summary>
    public class VorgangDto : EntityDto
    {
        /// <summary>
        /// Bezeichnung for searchresults/fav/recent list
        /// </summary>
        /// <returns></returns>
        override public String getEntityBezeichnung()
        {
            return Area + " " + Code + " " + Objekt + " " + Name;
        }


        public long SysId { get; set; }
        
        public string Area { get; set; }

        public string Code { get; set; }

        public long SysPerson { get; set; }

        public string Name { get; set; }

        public string Zusatz { get; set; }

        public string Vorname { get; set; }

        public string Objekt { get; set; }

        public long SysVart { get; set; }

        public string Vertragsart { get; set; }

        public string Vertriebsweg { get; set; }

        public double Anschaffungswert { get; set; }

        public DateTime? Erstelltam { get; set; }

        public long SysWfuser { get; set; }

        public string Username { get; set; }

        public string Uservorname { get; set; }

        public string Status { get; set; }

        public double obligo { get { return risikogr6; } set {} }
        public double obligokwg { get { return risikogr7; } set{} }

        public double risikogr6 { get; set; }
        public double risikogr7 { get; set; }
        
        public override long getEntityId()
        {
            return SysId;
        }

        public string angebot { get; set; }
        public string antrag { get; set; }
        public string nummer { get; set; }
        
        public string zustand { get; set; }
        public string attribut { get; set; }
        //DB Date only
        public DateTime? erfassung { get; set; }
        public DateTime? aenderung { get; set; }
        public DateTime? gueltigbis { get; set; }
        public string objektbez { get; set; }
        public string objektart { get; set; }
        public DateTime? beginn { get; set; }
        public DateTime? endeam { get; set; }
        public string fabrikat { get; set; }
        public string hersteller { get; set; }
        public string kundename { get; set; }
        public string kundeort { get; set; }
        public double betrag { get; set; }
        public string berater { get; set; }
        public string haendlername { get; set; }
        public string haendlerort { get; set; }
        public String mithafter { get; set; }
        public string kontfirmaname { get; set; }
        public string kontfirmavorname { get; set; }
        public string abwicklungsort { get; set; }
        public string eingangskanal { get; set; }

        public long syskd { get; set; }
        public long sysit { get; set; }

        public string itname { get; set; }
        public string itort { get; set; }
        public long syscamp { get; set; }

        public string kdname { get; set; }
        public string kdvorname { get; set; }
        public string kdplz { get; set; }
        public string kdort { get; set; }
        public string mhname { get; set; }
        public string mhvorname { get; set; }
        public long sysmh { get; set; }
        public int mitarbeiterflag { get; set; }

		public string slaindicator { get; set; }


        //DB CLA TIme only
        public long? erfassungzeit { get; set; }
        //Auto-combinded of above two fields
        public DateTime? erfassungsDatum
        {
            get
            {
                return DateTimeHelper.CreateDate(erfassung, erfassungzeit);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    erfassungzeit = (long)val.Value;
                else
                    erfassungzeit = 0;
                erfassung = value;
            }
        }

        /// <summary>
        /// Einreichungszeiten
        /// </summary>
        public DateTime? dateinreichung { get; set; }
        public long? dateinreichungzeit { get; set; }
        //Auto-combinded of above two fields
        public DateTime? einreichungsDatum
        {
            get
            {
                return DateTimeHelper.CreateDate(dateinreichung, dateinreichungzeit);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    dateinreichungzeit = (long)val.Value;
                else
                    dateinreichungzeit = 0;
                dateinreichung = value;
            }
        }

        
    }
}