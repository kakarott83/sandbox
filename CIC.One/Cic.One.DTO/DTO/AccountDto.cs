using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    /// <summary>
    /// Base Class Mapping all PERSON DB Table fields
    /// </summary>
    public class AccountDto : EntityDto
    {
     /// <summary>
        /// Sys ID
        /// </summary>
        public long sysperson { get; set; }
        public String matchcode {get;set;}
        public String code { get; set; }

        public double waehrung { get; set; }
        public double zins { get; set; }
        public double zahl { get; set; }

        private String iarea;
        public String area { get { if (iarea != null) return iarea; return getArea(); } set { iarea = value; } }

        public long sysid { get; set; }

        override public long getEntityId()
        {
            return sysid>0?sysid:sysperson;
        }

        override public String getEntityBezeichnung()
        {
            return name+" "+vorname;
        }
        //Aussendienstmitarbeiter
        public long sysadmadd { get; set; }
        /// <summary>
        /// Backofficemitarbeiter
        /// </summary>
        public String bomitarbeiter { get; set; }

        /// <summary>
        /// Kundentyp
        /// </summary>
        public long syskdtyp { get; set; }
        /// <summary>
        /// Kundentyp-Bezeichnung
        /// </summary>
        public String kdtypBezeichnung { get; set; }

        /// <summary>
        /// Anrede
        /// </summary>
        public String anrede { get; set; }
        /// <summary>
        /// Anredecode
        /// </summary>
        public String anredeCode { get; set; }
        /// <summary>
        /// Titel
        /// </summary>
        public String titel { get; set; }
        /// <summary>
        /// Titelcode
        /// </summary>
        public String titelCode { get; set; }
        /// <summary>
        /// Titel2
        /// </summary>
        public String titel2 { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// Vorname
        /// </summary>
        public String vorname { get; set; }
        /// <summary>
        /// Geburtsdatum
        /// </summary>
        public DateTime? gebdatum { get; set; }



        /// <summary>
        /// CT? Sprache
        /// </summary>
        public long sysctlang { get; set; }
        /// <summary>
        /// Sprache-Bezeichnung
        /// </summary>
        public String langBezeichnung { get; set; }
        /// <summary>
        /// CT? Sprache Korrespondenz
        /// </summary>
        public long sysctlangkorr { get; set; }
        /// <summary>
        /// Korrespondenz-Sprache-Bezeichnung
        /// </summary>
        public String langKorrBezeichnung { get; set; }
        
        
        /// <summary>
        /// Geburtsland 
        /// </summary>
        public long syslandnat { get; set; }
        /// <summary>
        /// Land (Nationalität) Bezeichnung
        /// </summary>
        public String landNatBezeichnung { get; set; }


        /// <summary>
        /// Einreisedatum
        /// </summary>
        public DateTime? einreisedatum { get; set; }
        /// <summary>
        /// Ausländischer Ausweis
        /// </summary>
        public String auslausweis { get; set; }
        /// <summary>
        /// Ausländischer Ausweiscode
        /// </summary>
        public String auslausweisCode { get; set; }
        /// <summary>
        /// Ausländischer Ausweis Gültigkeit
        /// </summary>
        public DateTime? auslausweisGueltig { get; set; }
        /// <summary>
        /// Strasse
        /// </summary>
        public String strasse { get; set; }
        /// <summary>
        /// Hausnummer
        /// </summary>
        public String hsnr { get; set; }
        /// <summary>
        /// Postleitzahl
        /// </summary>
        public String plz { get; set; }
        /// <summary>
        /// Ort
        /// </summary>
        public String ort { get; set; }

        /// <summary>
        /// Land
        /// </summary>
        public long sysland { get; set; }
        /// <summary>
        /// Land (sysland) Bezeichnung
        /// </summary>
        public String landBezeichnung { get; set; }
        
        /// <summary>
        /// Staat
        /// </summary>
        public long sysstaat { get; set; }
        /// <summary>
        /// Staat-Bezeichnung
        /// </summary>
        public String staatBezeichnung { get; set; }


        /// <summary>
        /// Wohnhaft seit
        /// </summary>
        public DateTime? wohnseit { get; set; }
        /// <summary>
        /// Strasse
        /// </summary>
        public String strasse2 { get; set; }
        /// <summary>
        /// Hasusnummer
        /// </summary>
        public String hsnr2 { get; set; }
        /// <summary>
        /// Postleitzahl 2
        /// </summary>
        public String plz2 { get; set; }
        /// <summary>
        /// Ort 2
        /// </summary>
        public String ort2 { get; set; }


        /// <summary>
        /// Land 2
        /// </summary>
        public long sysland2 { get; set; }
        /// <summary>
        /// Land2-Bezeichnung
        /// </summary>
        public String land2Bezeichnung { get; set; }

        /// <summary>
        /// Staat 2
        /// </summary>
        public long sysstaat2 { get; set; }
        /// <summary>
        /// Staat2-Bezeichnung
        /// </summary>
        public String staat2Bezeichnung { get; set; }

        /// <summary>
        /// Telefon
        /// </summary>
        public String telefon { get; set; }
        /// <summary>
        /// Telefon 2
        /// </summary>
        public String telefon2 { get; set; }
        public String ptelefon { get; set; }
        /// <summary>
        /// Mobiltelefon
        /// </summary>
        public String handy { get; set; }
        /// <summary>
        /// Erreichbar Telefonisch
        /// </summary>
        public int erreichbtel { get; set; }
        /// <summary>
        /// Erreichbar Von
        /// </summary>
        public long erreichbVon { get; set; }
        /// <summary>
        /// Erreichbar Bis
        /// </summary>
        public long erreichbBis { get; set; }


        /// <summary>
        /// Erreichbar Von
        /// </summary>
        public DateTime? erreichbVonGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)erreichbVon);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    erreichbVon = (long)val.Value;
                else
                    erreichbVon = 0;
            }
        }
        /// <summary>
        /// Erreichbar Bis
        /// </summary>
        public DateTime? erreichbBisGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)erreichbBis);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    erreichbBis = (long)val.Value;
                else
                    erreichbBis = 0;
            }
        }

        /// <summary>
        /// E-Mail
        /// </summary>
        public String email { get; set; }
        /// <summary>
        /// Webseite
        /// </summary>
        public String url { get; set; }

        /// <summary>
        /// Branche
        /// </summary>
        public long sysbranche { get; set; }
        /// <summary>
        /// Branche-Bezeichnung
        /// </summary>
        public String brancheBezeichnung { get; set; }

        /// <summary>
        /// Rechtsform
        /// </summary>
        public String rechtsform { get; set; }
        /// <summary>
        /// Rechtsformcode
        /// </summary>
        public String rechtsformCode { get; set; }
        /// <summary>
        /// Gründung
        /// </summary>
        public DateTime? gruendung { get; set; }
        /// <summary>
        /// Handelsregistereintrag
        /// </summary>
        public String hregister { get; set; }
        /// <summary>
        /// Handelsregister Flag
        /// </summary>
        public int hregisterFlag { get; set; }
        /// <summary>
        /// Referenzflag
        /// </summary>
        public int revFlag { get; set; }
        /// <summary>
        /// Kontos Array
        /// </summary>
        //public KontoDto[] kontos { get; set; }
        /// <summary>
        /// Adressen Array
        /// </summary>
       // public AdresseDto[] adressen { get; set; }

        /// <summary>
        /// Zusatzdaten Array
        /// </summary>
        //public ZusatzdatenDto[] zusatzdaten { get; set; }

        /// <summary>
        /// Zusatz
        /// </summary>
        public String zusatz { get; set; }

        /// <summary>
        /// Name Kontaktperson
        /// </summary>
        public String nameKont { get; set; }

        /// <summary>
        /// Vorname Kontaktperson
        /// </summary>
        public String vornameKont { get; set; }


        /// <summary>
        /// Anrede Kontaktperson
        /// </summary>
        public String anredeKont { get; set; }
        /// <summary>
        /// Anredecode Kontaktperson
        /// </summary>
        public String anredeCodeKont { get; set; }
        /// <summary>
        /// Titelcode Kontaktperson
        /// </summary>
        public String titelKont { get; set; }
        /// <summary>
        /// Fax
        /// </summary>
        public String fax { get; set; }
        /// <summary>
        /// Geburtsort
        /// </summary>
        public String gebOrt { get; set; }
        /// <summary>
        /// Amtsgericht
        /// </summary>
        public String hregisterOrt { get; set; }
         /// <summary>
         /// SteuerID
         /// </summary>
        public String  identEu { get; set; }
        /// <summary>
        /// Steuernummer
        /// </summary>
        public String steuerNr { get; set; }
        /// <summary>
        /// UstID
        /// </summary>
        public String identUst { get; set; }
        /// <summary>
        /// Keine Werbung
        /// </summary>
        public int nomailingflag { get; set; }
        /// <summary>
        /// Privatperson
        /// </summary>
        public int privatFlag { get; set; }
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

        public String kundengruppe {get;set;}
        public int flagkd {get;set;}
        public int aktivkz { get; set; }

        //BNOW-FIELDS:

        /// <summary>
        /// Kundenreferenz
        /// </summary>
        public long sysreferenz { get; set; }
        /// <summary>
        /// Risikoklasse
        /// </summary>
        public long sysrisikokl { get; set; }
        /// <summary>
        /// Flag E-Mail
        /// </summary>
        public int infomailflag { get; set; }
        /// <summary>
        /// Flag Vertrag per Email
        /// </summary>
        public int infomail2flag { get; set; }
        /// <summary>
        /// Flag SMS
        /// </summary>
        public int infosmsflag { get; set; }

        /// <summary>
        /// Person ist Mitarbeiter
        /// </summary>
        public int mitarbeiterflag { get; set; }

        /// <summary>
        /// Ausschluss-Flag
        /// </summary>
        public int ausschluss { get; set; }

        /// <summary>
        /// ID-Berechtigung-Flag
        /// </summary>
        public int idberechtigung { get; set; }

        /// <summary>
        /// Zusätzlich ermittelte Daten aus anderen Bereichen
        /// </summary>
        public AccountExtDataDto zusatzDaten { get;set; }

        /// <summary>
        /// Werbestopp
        /// </summary>
        public String werbecode { get; set; }
        public String werbecodegrund { get; set; }

        //Gespraechspartner/Korrespondenzadressdaten
        public String plastname { get; set; }
        public String pfirstname { get; set; }
        public String panrede { get; set; }
        public String pstrasse { get; set; }
        public String phsnr { get; set; }
        public String port { get; set; }
        public String pplz { get; set; }
        public long psysland { get; set; }
        public long psysstaat { get; set; }
        
        public String ptitel { get; set; }
        public String pmobil { get; set; }
        public String pemail { get; set; }
        public String pfax { get; set; }

        
        /// <summary>
        /// PEOPTION.OPTION16
        /// </summary>
        public String fax2 { get; set; }
        /// <summary>
        /// PEOPTION.OPTION7
        /// </summary>
        public String handy2 { get; set; }
        /// <summary>
        /// PEOPTION.OPTION3
        /// </summary>
        public String email2 { get; set; }

        /// <summary>
        /// Gesperrt Flag
        /// </summary>
        public int locked { get; set; }

       
        public int inactiveflag { get; set; }
        //END BNOW-FIELDS


        public int flagbn { get; set; }
        public int flaghd { get; set; }
        public String crefoid { get; set; }
    }
}
