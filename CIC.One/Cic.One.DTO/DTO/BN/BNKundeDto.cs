using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Parameterklasse für KundeDto
    /// </summary>
    public class BNKundeDto : EntityDto
    {
        /// <summary>
        /// Sys ID?
        /// </summary>
        public long sysit { get; set; }
        /// <summary>
        /// Sys ID Kunde?
        /// </summary>
        public long syskd { get; set; }

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
        public KontoDto[] kontos { get; set; }
        /// <summary>
        /// Adressen Array
        /// </summary>
        public AdresseDto[] adressen { get; set; }

        /// <summary>
        /// Zusatzdaten Array
        /// </summary>
        public ZusatzdatenDto[] zusatzdaten { get; set; }

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
        /// Flag für Infomail erlaubt
        /// </summary>
        public int infomailflag { get; set; }
        public int infomail2flag { get; set; }
        public int infosmsflag { get; set; }


        public override long getEntityId()
        {
            return sysit > 0 ? sysit : syskd;
        }

        //Additions to B2B NFE
        public String beruf { get; set; }
        public int beschartag { get; set; }


        /// <summary>
        /// CS-Flag
        /// </summary>
        public int mitarbeiterflag { get; set; }

        /// <summary>
        /// Korrespondenzadresse IT
        /// </summary>
        public long? sysitkorradresse { get; set; }
        /// <summary>
        /// Korrespondenzadresse PERSON
        /// </summary>
        public long? syskorradresse { get; set; }
    }
}
