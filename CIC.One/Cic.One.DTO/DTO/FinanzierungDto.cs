using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class FinanzierungDto : EntityDto
    {
        public long sysnkk { get; set; }
        public long sysskonto { get; set; }
        public long sysnkonto { get; set; }
        public long sysvt { get; set; }
        public DateTime? beginn { get; set; }
        public double zinss { get; set; }
        public double zinsh { get; set; }
        public long sysszinstab { get; set; }
        public long syshzinstab { get; set; }
        public DateTime? stand { get; set; }
        public DateTime? lstand { get; set; }
        public int sollhaben { get; set; }
        public double kontostand { get; set; }
        public double saldo { get; set; }
        public long ppy { get; set; }
        public long ppyauszug { get; set; }
        public DateTime? standauszug { get; set; }
        public long sysnkktyp { get; set; }
        public long sysintks { get; set; }
        public long sysintkh { get; set; }
        public long sysintstrcts { get; set; }
        public long sysintstrcth { get; set; }
        public long sysnkkparent { get; set; }
        public long sysrun { get; set; }
        public long sysrvt { get; set; }
        public long sysprproduct { get; set; }
        public long sysprintsetvf { get; set; }
        public long sysprintsetaf { get; set; }
        public long sysprtlgset { get; set; }
        public String konto { get; set; }
        public String bezeichnung { get; set; }
        public String zustand { get; set; }
        public DateTime? zustandam { get; set; }
        public DateTime? ende { get; set; }
        public double nominal { get; set; }
        public double auszahlung { get; set; }
        public double auszahlungp { get; set; }
        public DateTime? auszahlungam { get; set; }
        public double restschuld { get; set; }
        public double restschuldp { get; set; }
        public DateTime? restschuldam { get; set; }
        public DateTime? endevf { get; set; }
        public DateTime? msperrevon { get; set; }
        public DateTime? msperrebis { get; set; }

        public String nkonto { get; set; }
        public String rahmen { get; set; }
        public String kontotyp { get; set; }

        public String ernummer { get; set; }
        /// <summary>
        /// OBJEKT
        /// </summary>
        public String OBJEKT { get; set; }

        /// <summary>
        /// Tilgungsplan
        /// </summary>
        public long SYSPRTLGSE { get; set; }

        /// <summary>
        /// Zinstermite
        /// </summary>
        public int zinstermineInt { get; set; }

        /// <summary>
        /// Tilgungsplan
        /// </summary>
        public int zinsusanceInt { get; set; }

        /// <summary>
        /// Tilgungsplan
        /// </summary>
        public int feirtagkorrekturInt { get; set; }

        /// <summary>
        /// Tilgungsplan
        /// </summary>
        public int SUPPRESSCAP { get; set; }

        /// <summary>
        /// Tilgungsplan
        /// </summary>
        public int INTFIRSTDAY { get; set; }

        /// <summary>
        /// Tilgungsplan
        /// </summary>
        public int INTLASTDAY { get; set; }


        public long SYSOB { get; set; }
        public String HSN { get; set; }
        public String TSN { get; set; }
        public String FIDENT { get; set; }
        public String FGNR { get; set; }
        public String MODELL { get; set; }
        public String FABRIKAT { get; set; }
        public String HERSTELLER { get; set; }
        public String SCHILD { get; set; }

        public String LAUFNUMMER { get; set; }
        public String RECHNUNG { get; set; }
        public DateTime? VALUTADATUM { get; set; }
        public String WAEHRUNGCODE { get; set; }
        public String PRODUCTNAME { get; set; }
        public long SYSPERSON { get; set; }
        public String PERSONCODE { get; set; }
        public String PERSONNAME { get; set; }

      
        override public long getEntityId()
        {
            return sysnkk;
        }

        /// <summary>
        /// sysWaehrung/ANTRAG/WAEHRUNG
        /// </summary>
        public WaehrungDto waehrung { get; set; }

        public ObDto ob { get; set; }

        /// <summary>
        /// zukünftiger Saldo
        /// </summary>
        public double zksaldo { get { return saldo - nextsl; } set { ; } }
        /// <summary>
        /// nächste Abschlagszahlung
        /// </summary>
        public double nextsl { get; set; }
        /// <summary>
        /// nächste Abschlagszahlung Datum
        /// </summary>
        public DateTime? nextsldate { get; set; }
        /// <summary>
        /// nächste Abschlagszahlung Tage von heute
        /// </summary>
        public double nextsldays { get { if (!nextsldate.HasValue) return 0; return (nextsldate.Value - DateTime.Today).TotalDays; } set { ;} }

        /// <summary>
        /// akt. Zinssatz
        /// </summary>
        public double aktZins { get; set; }

        public double belZins { get; set; }
        public double belTilg { get; set; }

        public long sysversandperson { get; set; }
        public String versandart { get; set; }
        public String versandgrund { get; set; }

        public long finanzierbar { get; set; }

        public long sysklinie { get; set; }

        public String versandstatus { get; set; }
        public String kreditlinie { get; set; }
        public String lagerortbrief { get; set; }
        public DateTime? datumversand { get; set; }

        override public String getEntityBezeichnung()
        {
            return konto;
        }
    }
}