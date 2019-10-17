using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class ItDto : EntityDto
    {

        override public long getEntityId()
        {
            return sysit;
        }

        override public String getEntityBezeichnung()
        {
            return name + " " + vorname;
        }

        public int aausweistyp { get; set; }


        public String abbewilldurch { get; set; }


        public DateTime? abbewilligung { get; set; }


        public DateTime? abbewilligungbis { get; set; }


        public int abgueltig { get; set; }


        public String ahbewilldurch { get; set; }


        public DateTime? ahbewilligung { get; set; }


        public DateTime? ahbewilligungbis { get; set; }


        public int ahgueltig { get; set; }


        public String anrede { get; set; }


        public String anredeCode { get; set; }


        public String anredeCodeKont { get; set; }


        public String anredeKont { get; set; }


        public int anzahlvollstr { get; set; }


        public int anzaufenthalt { get; set; }


        public int anzeink { get; set; }


        public int anzkind1 { get; set; }


        public int anzkind2 { get; set; }


        public int anzkind3 { get; set; }


        public int anzkind4 { get; set; }


        public int anzneink { get; set; }


        public int anzzeink { get; set; }


        public String artsonstverm { get; set; }


        public double auslagen { get; set; }


        public String auslausweis { get; set; }


        public String auslausweiscode { get; set; }


        public DateTime? auslausweisgueltig { get; set; }


        public DateTime? ausweisablauf { get; set; }


        public int ausweisart { get; set; }


        public String ausweisbehoerde { get; set; }


        public DateTime? ausweisdatum { get; set; }


        public int ausweisgueltig { get; set; }


        public String ausweisNr { get; set; }


        public String ausweisOrt { get; set; }


        public String bankName { get; set; }


        public String beruf { get; set; }


        public int beschartag { get; set; }


        public int beschartag1 { get; set; }


        public int beschartag2 { get; set; }


        public int beschartag3 { get; set; }


        public DateTime? beschbisag { get; set; }


        public DateTime? beschbisag1 { get; set; }


        public DateTime? beschbisag2 { get; set; }


        public DateTime? beschbisag3 { get; set; }


        public DateTime? beschseitag { get; set; }


        public DateTime? beschseitag1 { get; set; }


        public DateTime? beschseitag2 { get; set; }


        public DateTime? beschseitag3 { get; set; }


        public long betragvollstr { get; set; }


        public String bic { get; set; }


        public double bilanz { get; set; }


        public String blz { get; set; }


        public double cashflow { get; set; }


        public String code { get; set; }


        public double eigenkapital { get; set; }


        public double einkBrutto { get; set; }


        public double einkNetto { get; set; }


        public int einkNettoFlag { get; set; }


        public DateTime? einreisedatum { get; set; }


        public String email { get; set; }


        public String email2 { get; set; }


        public String emailkont { get; set; }


        public long erreichbbis { get; set; }


        public int erreichbtel { get; set; }


        public int erreichbtrel { get; set; }


        public long erreichbvon { get; set; }


        public String extreferenz { get; set; }


        public int fameinrFlag { get; set; }


        public int familienstand { get; set; }


        public String fax { get; set; }


        public String faxkont { get; set; }


        public long fparkgroesse { get; set; }


        public DateTime? gebdatum { get; set; }


        public int geschlecht { get; set; }


        public int geschlechtkont { get; set; }


        public DateTime? grenzgseit { get; set; }


        public DateTime? gruendung { get; set; }


        public String handy { get; set; }


        public String handykont { get; set; }


        public String hregister { get; set; }


        public int hregisterFlag { get; set; }


        public String hsnr { get; set; }


        public String hsnr2 { get; set; }


        public String hsnrag { get; set; }


        public String hsnrag1 { get; set; }


        public String hsnrag2 { get; set; }


        public String hsnrag3 { get; set; }


        public String iban { get; set; }


        public String identeg { get; set; }


        public String identeghist { get; set; }


        public String identUst { get; set; }


        public int infomailFlag { get; set; }

        public int infomail2Flag { get; set; }


        public int infosmsFlag { get; set; }


        public DateTime? inlandseit { get; set; }


        public double jahresumsatz { get; set; }


        public long jbonusBrutto { get; set; }


        public long jbonusNetto { get; set; }


        public int kinderimhaus { get; set; }


        public int konkursFlag { get; set; }


        public String kontoinhaber { get; set; }


        public String kontonr { get; set; }


        public long kredrate1 { get; set; }


        public long kredrate2 { get; set; }


        public long kredrate3 { get; set; }


        public long kredrate4 { get; set; }


        public String kundengruppe { get; set; }


        public long leasingrate1 { get; set; }


        public long leasingrate2 { get; set; }


        public long leasingrate3 { get; set; }


        public long leasingrate4 { get; set; }


        public String legitabnehmer { get; set; }


        public DateTime? legitdatum { get; set; }


        public DateTime? letzterjabschl { get; set; }


        public String matchcode { get; set; }


        public DateTime? meldedatum { get; set; }


        public long miete { get; set; }


        public long mietneben { get; set; }


        public int mitarbeiterFlag { get; set; }


        public String name { get; set; }


        public String nameag { get; set; }


        public String nameag1 { get; set; }


        public String nameag2 { get; set; }


        public String nameag3 { get; set; }


        public String namekont { get; set; }


        public double nebeneinkBrutto { get; set; }


        public double nebeneinkNetto { get; set; }


        public double nebeninmiete { get; set; }


        public String notiz { get; set; }


        public String ort { get; set; }


        public String ort2 { get; set; }


        public String ortag { get; set; }


        public String ortag1 { get; set; }


        public String ortag2 { get; set; }


        public String ortag3 { get; set; }


        public int pfaendungFlag { get; set; }


        public String plz { get; set; }


        public String plz2 { get; set; }


        public String plzag { get; set; }


        public String plzag1 { get; set; }


        public String plzag2 { get; set; }


        public String plzag3 { get; set; }


        public int privatFlag { get; set; }


        public String ptelefon { get; set; }


        public String ptelefonkont { get; set; }


        public int rechtsform { get; set; }


        public String rechtsformCode { get; set; }


        public String rechtsformlang { get; set; }


        public int revFlag { get; set; }


        public double sonstverm { get; set; }


        public String strasse { get; set; }


        public String strasse2 { get; set; }


        public String strasseag { get; set; }


        public String strasseag1 { get; set; }


        public String strasseag2 { get; set; }


        public String strasseag3 { get; set; }


        public String strassezusatz { get; set; }


        public String suffix { get; set; }


        public String suffixkont { get; set; }


        public String svnr { get; set; }


        public long sysbranche { get; set; }


        public long sysctlang { get; set; }


        public long sysctlangkont { get; set; }


        public long sysctlangkorr { get; set; }


        public long sysit { get; set; }


        public long syskdtyp { get; set; }


        public long sysland { get; set; }
        public long sysland2 { get; set; }


        public long syslandag { get; set; }


        public long syslandag1 { get; set; }


        public long syslandag2 { get; set; }


        public long syslandag3 { get; set; }


        public long syslandnat { get; set; }


        public long sysperson { get; set; }


        public long sysstaat { get; set; }


        public long sysstaat2 { get; set; }


        public long sysstaatag { get; set; }


        public long sysstaatag1 { get; set; }


        public long sysstaatag2 { get; set; }


        public long sysstaatag3 { get; set; }


        public String telefon { get; set; }


        public String telefon2 { get; set; }


        public String telefonkont { get; set; }


        public String titel { get; set; }


        public String titel2 { get; set; }


        public String titel2code { get; set; }


        public String titelCode { get; set; }


        public String titelkont { get; set; }


        public String uidnummer { get; set; }


        public int unbefrag { get; set; }


        public int unbefrag1 { get; set; }


        public int unbefrag2 { get; set; }


        public int unbefrag3 { get; set; }


        public double unterhalt { get; set; }


        public String url { get; set; }


        public int ustabzug { get; set; }


        public double versicherung { get; set; }


        public String vorname { get; set; }


        public String vornamekont { get; set; }


        public String wbeguenst { get; set; }


        public String wehrdienst { get; set; }


        public DateTime? wohnseit { get; set; }


        public String wohnungart { get; set; }


        public int wohnverh { get; set; }


        public int zeinkart { get; set; }


        public double zeinkBrutto { get; set; }


        public double zeinkNetto { get; set; }


        public String zusatz { get; set; }


        public String zusatzag { get; set; }


        public String zusatzag1 { get; set; }


        public String zusatzag2 { get; set; }


        public String zusatzag3 { get; set; }


        public String zusatzkont { get; set; }

        /// <summary>
        /// Keine Werbung
        /// </summary>
        public int nomailingflag { get; set; }

        /// <summary>
        /// Erreichbar Von
        /// </summary>
        public DateTime? erreichbVonGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)erreichbvon);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    erreichbvon = (long)val.Value;
                else
                    erreichbvon = 0;
            }
        }
        /// <summary>
        /// Erreichbar Bis
        /// </summary>
        public DateTime? erreichbBisGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)erreichbbis);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    erreichbbis = (long)val.Value;
                else
                    erreichbbis = 0;
            }
        }

        /// <summary>
        /// Gesellschaft
        /// </summary>
        public int gesFlag { get; set; }


        /// <summary>
        /// Kann den Account zu einer Prkgruppe hinzufügen.
        /// </summary>
        public long sysPrkgroup { get; set; }



        /// <summary>
        /// CAS filled fields
        /// </summary>
        public double obligo { get; set; }
        public double obligokwg { get; set; }
        public double op { get; set; }

		/// <summary>
		/// ANG-Counter
		/// </summary>
		public long angebotcount { get; set; }

		/// <summary>
		/// ANT-Counter
		/// </summary>
		public long antragcount { get; set; }
		/// <summary>
		/// VT-Counter
		/// </summary>
		public long vtcount { get; set; }




	}
}

