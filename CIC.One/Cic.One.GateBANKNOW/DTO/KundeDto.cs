using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.GateBANKNOW.DTO
{
    /// <summary>
    /// BN Partner Service Kunde (PORSCHE)
    /// </summary>
    public class KundeDto
    {
        /*	Anredecode Mapping Excel ANREDE	*/
        public String anredeCode { get; set; }
        /*	E-Mail 	*/
        public String email { get; set; }
        /*	Kunden ID Partnersystem (C@P)	*/
        public String extreferenz { get; set; }
        /*	Geburtsdatum 	*/
        public DateTime? gebdatum { get; set; }
        /*	Mobiltelefon 	*/
        public String handy { get; set; }
        /*	Hausnummer 	*/
        public String hsnr { get; set; }
        /*	Name 	*/
        public String name { get; set; }
        /*	Ort 	*/
        public String ort { get; set; }
        /*	Postleitzahl 	*/
        public String plz { get; set; }
        /*	Strasse 	*/
        public String strasse { get; set; }
        /*	Kundentyp. Ermittlung über Tabelle Kundentyp  Siehe Excel KDTYP	*/
        public long syskdtyp { get; set; }
        /*	Land. Ermittlung über Tabelle Land Mapping Excel LAND 0=KeinWert	*/
        public long sysland { get; set; }
        /*	Sprache Mapping Excel SPRACHEN 0=Kein Wert	*/
        public long sysctlang { get; set; }
        /*	Geburtsland. Ermittlung über Tabelle Nationalität Mapping Excel NATIONALITAET 0=Kein Wert	*/
        public long syslandnat { get; set; }
        /*	Staat. Ermittlung über Tabelle Kantone. Mapping Excel STAAT 0=Kein Wert	*/
        public long sysstaat { get; set; }
        /*	Telefon 	*/
        public String telefon { get; set; }
        /*	Telefon privat	*/
        public String ptelefon { get; set; }
        /*	Titelcode Freitext trotz Codefeld 	*/
        public String titelCode { get; set; }
        /*	Vorname 	*/
        public String vorname { get; set; }
        /*	Firmenzusatz 	*/
        public String zusatz { get; set; }
        /*	Anredecode Kontaktperson Mapping Excel ANREDE	*/
        public String anredeCodeKont { get; set; }
        /*	Name Kontaktperson 	*/
        public String nameKont { get; set; }
        /*	Vorname Kontaktperson 	*/
        public String vornameKont { get; set; }
        /*	Ausländischer Ausweiscode 	*/
        public String auslausweisCode { get; set; }
        /*	Ausländischer Ausweis Gültigkeit 	*/
        public DateTime? auslausweisGueltig { get; set; }
        /*	Einreisedatum 	*/
        public DateTime? einreisedatum { get; set; }
        /*	Handelsregister Flag 	*/
        public bool hregisterFlag { get; set; }
        /*	Ort 	*/
        public String ort2 { get; set; }
        /*	Postleitzahl 	*/
        public String plz2 { get; set; }
        /*	Revisionsstelle vorhanden	*/
        public bool revFlag { get; set; }
        /*	Strasse 	*/
        public String strasse2 { get; set; }
        /*	Land. Ermittlung über Tabelle Land 0=Kein Wert	*/
        public long sysland2 { get; set; }
        /*	Staat. Ermittlung über Tabelle Kantone.  0=Kein Wert	*/
        public long sysstaat2 { get; set; }
        /*	Wohnhaft seit 	*/
        public DateTime? wohnseit { get; set; }
        /*	Anzahl Kinder bis 6 Jahre 	*/
        public short anzkinder { get; set; }
        /*	Anzahl Kinder 6-10 Jahre 	*/
        public short anzkinder1 { get; set; }
        /*	Anzahl Kinder 10-12 Jahre 	*/
        public short anzkinder2 { get; set; }
        /*	Anzahl Kinder (unterstützungspflichtig) über 12 	*/
        public short anzkinder3 { get; set; }
        /*	Anzahl Betreibungen 	*/
        public short anzvollstr { get; set; }
        /*	Bestehende Auslagen (regelmäßig) 	*/
        public double? auslagen { get; set; }
        /*	Verweis zur Auslagenart (Lookup und Übersetzung) 	*/
        public String auslagenCode { get; set; }
        /*	Verweis zur beruflichen Situation (Lookup und Übersetzung) 	*/
        public String beruflichCode { get; set; }
        /*	Verweis zur beruflichen Situation (Lookup und Übersetzung) 	*/
        public String beruflichCode2 { get; set; }
        /*	Bestehende Auslagen (regelmäßig beruflich) 	*/
        public double? berufsauslagen { get; set; }
        /*	Verweis zur Auslagenart beruflich (Lookup und Übersetzung) 	*/
        public String berufsauslagenCode { get; set; }
        /*	Ende der Beschäftigung bei Arbeitgeber 	*/
        public DateTime? beschbisAg1 { get; set; }
        /*	Beginn der Beschäftigung bei Arbeitgeber 	*/
        public DateTime? beschseitAg1 { get; set; }
        /*	Ende der Beschäftigung bei Arbeitgeber 	*/
        public DateTime? beschbisAg2 { get; set; }
        /*	Beginn der Beschäftigung bei Arbeitgeber 	*/
        public DateTime? beschseitAg2 { get; set; }
        /*	Höhe Betreibungen 	*/
        public double? betragvollstr { get; set; }
        /*	Haupteinkommen brutto 	*/
        public double? einkbrutto { get; set; }
        /*	Haupteinkommen netto 	*/
        public double? einknetto { get; set; }
        /*	Angaben Einkommen netto/brutto (TRUE = netto) 	*/
        public bool einknettoFlag { get; set; }
        /*	Zivilstand (ledig, verheiratet, geschieden …) 	*/
        public short familienstand { get; set; }
        /*	Arbeitgeber Hausnummer 	*/
        public String hsnrAg1 { get; set; }
        /*	Arbeitgeber Hausnummer 	*/
        public String hsnrAg2 { get; set; }
        /*	Jahresbonus brutto 	*/
        public double? jbonusbrutto { get; set; }
        /*	Jahresbonus netto 	*/
        public double? jbonusnetto { get; set; }
        /*	Konkurs/Pfändung/Verlustschein 	*/
        public bool konkursFlag { get; set; }
        /*	Wohnkosten/Miete 	*/
        public double? miete { get; set; }
        /*	Bekommt 13ten Monatslohn 	*/
        public bool monatslohnXtdFlag { get; set; }
        /*	Arbeitgeber Name 	*/
        public String nameAg1 { get; set; }
        /*	Arbeitgeber Name 	*/
        public String nameAg2 { get; set; }
        /*	Nebeneinkommen brutto 	*/
        public double? nebeinkbrutto { get; set; }
        /*	Nebeneinkommen netto 	*/
        public double? nebeinknetto { get; set; }
        /*	Arbeitgeber Ort 	*/
        public String ortAg1 { get; set; }
        /*	Arbeitgeber Postleitzahl 	*/
        public String plzAg1 { get; set; }
        /*	Arbeitgeber Strasse 	*/
        public String strasseAg1 { get; set; }
        /*	Arbeitgeber Ort 	*/
        public String ortAg2 { get; set; }
        /*	Arbeitgeber Postleitzahl 	*/
        public String plzAg2 { get; set; }
        /*	Arbeitgeber Strasse 	*/
        public String strasseAg2 { get; set; }
        /*	Bestehende Unterstützungsverpflichtungen (regelmäßig) 	*/
        public double? unterhalt { get; set; }
        /*	Verweis zur Unterstüzungsart (Lookup und Übersetzung) 	*/
        public String unterhaltCode { get; set; }
        /*	Weitere Auslagen 	*/
        public double? weitereauslagen { get; set; }
        /*	Weitere Auslagen Code 	*/
        public String weitereauslagenCode { get; set; }
        /*	Verweis zur Wohnsituation (Lookup und Übersetzung) 	*/
        public String wohnverhCode { get; set; }
        /*	Zusatzeinkommen brutto 	*/
        public double? zeinkbrutto { get; set; }
        /*	Verweis zum Zusatzeinkommen Art (Lookup und Übersetzung) 	*/
        public String zeinkCode { get; set; }
        /*	Zusatzeinkommen netto 	*/
        public double? zeinknetto { get; set; }
        /*	Jahresumsatz	*/
        public double? jumsatz { get; set; }
        /*	Eigenkapital	*/
        public double? ekapital { get; set; }
        /*	Bilanzsumme	*/
        public double? bilanzwert { get; set; }
        /*	Jahresgewinn	*/
        public double? ergebnis1 { get; set; }
        /*	Flüssige Mittel	*/
        public double? liquiditaet { get; set; }
        /*	Kurzfristige Verbindlichkeiten	*/
        public double? obligoeigen { get; set; }
        /*	Datum letzter Jahresabschluss	*/
        public DateTime? jabschl { get; set; }
        /*	Anzahl Mitarbieter	*/
        public short anzma { get; set; }

    }
}