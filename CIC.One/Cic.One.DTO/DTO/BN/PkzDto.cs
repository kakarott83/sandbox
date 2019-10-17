using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// zusatzdatenPrivat
    /// </summary>
    public class PkzDto
    {
        /// <summary>
        /// PKEY 
        /// </summary>
        public long syspkz { get; set; }
        /// <summary>
        /// Zivilstand (ledig, verheiratet, geschieden …) 
        /// </summary>
        public short familienstand { get; set; }
        /// <summary>
        /// Ehepartner (nur relevant wenn zweiter Antragsteller) 
        /// </summary>
        public int ehepartnerFlag { get; set; }
        /// <summary>
        /// Verweis zur Wohnsituation (Lookup und Übersetzung) 
        /// </summary>
        public String wohnverhCode { get; set; }
        /// <summary>
        /// Anzahl Kinder bis 6 Jahre 
        /// </summary>
        public short anzkinder { get; set; }
        /// <summary>
        /// Anzahl Kinder 6-10 Jahre 
        /// </summary>
        public short anzkinder1 { get; set; }
        /// <summary>
        /// Anzahl Kinder 10-12 Jahre 
        /// </summary>
        public short anzkinder2 { get; set; }
        /// <summary>
        /// Anzahl Kinder (unterstützungspflichtig) über 12 
        /// </summary>
        public short anzkinder3 { get; set; }
        /// <summary>
        /// Verweis zur beruflichen Situation (Lookup und Übersetzung) 
        /// </summary>
        public String beruflichCode { get; set; }
        /// <summary>
        /// Verweis zur beruflichen Situation Nebenerwerb.(Lookup und Übersetzung) 
        /// </summary>
        public String beruflichCode2 { get; set; }
        /// <summary>
        /// Arbeitgeber Name 
        /// </summary>
        public String nameAg1 { get; set; }
        /// <summary>
        /// Arbeitgeber Strasse 
        /// </summary>
        public String strasseAg1 { get; set; }
        /// <summary>
        /// Arbeitgeber Hausnummer 
        /// </summary>
        public String hsnrAg1 { get; set; }
        /// <summary>
        /// Arbeitgeber Postleitzahl 
        /// </summary>
        public String plzAg1 { get; set; }
        /// <summary>
        /// Arbeitgeber Ort 
        /// </summary>
        public String ortAg1 { get; set; }
        /// <summary>
        /// Beginn der Beschäftigung bei Arbeitgeber 
        /// </summary>
        public DateTime? beschseitAg1 { get; set; }
        /// <summary>
        /// Ende der Beschäftigung bei Arbeitgeber 
        /// </summary>
        public DateTime? beschbisAg1 { get; set; }
        /// <summary>
        /// Arbeitgeber Nebenerwerb Name 
        /// </summary>
        public String nameAg2 { get; set; }
        /// <summary>
        /// Arbeitgeber Nebenerwerb Strasse 
        /// </summary>
        public String strasseAg2 { get; set; }
        /// <summary>
        /// Arbeitgeber Nebenerwerb Hausnummer 
        /// </summary>
        public String hsnrAg2 { get; set; }
        /// <summary>
        /// Arbeitgeber Nebenerwerb Postleitzahl 
        /// </summary>
        public String plzAg2 { get; set; }
        /// <summary>
        /// Arbeitgeber Nebenerwerb Ort 
        /// </summary>
        public String ortAg2 { get; set; }
        /// <summary>
        /// Beginn der Beschäftigung bei Arbeitgeber Nebenerwerb 
        /// </summary>
        public DateTime? beschseitAg2 { get; set; }
        /// <summary>
        /// Ende der Beschäftigung bei Arbeitgeber Nebenerwerb 
        /// </summary>
        public DateTime? beschbisAg2 { get; set; }
        /// <summary>
        /// Haupteinkommen netto 
        /// </summary>
        public double? einknetto { get; set; }
        /// <summary>
        /// Haupteinkommen brutto 
        /// </summary>
        public double? einkbrutto { get; set; }
        /// <summary>
        /// Nebeneinkommen netto 
        /// </summary>
        public double? nebeinknetto { get; set; }
        /// <summary>
        /// Nebeneinkommen brutto 
        /// </summary>
        public double? nebeinkbrutto { get; set; }
        /// <summary>
        /// Jahresbonus netto 
        /// </summary>
        public double? jbonusnetto { get; set; }
        /// <summary>
        /// Jahresbonus brutto 
        /// </summary>
        public double? jbonusbrutto { get; set; }
        /// <summary>
        /// Zusatzeinkommen netto 
        /// </summary>
        public double? zeinknetto { get; set; }
        /// <summary>
        /// Zusatzeinkommen brutto 
        /// </summary>
        public double? zeinkbrutto { get; set; }
        /// <summary>
        /// Verweis zum Zusatzeinkommen Art (Lookup und Übersetzung) 
        /// </summary>
        public string zeinkCode { get; set; }
        /// <summary>
        /// Angaben Einkommen netto/brutto (TRUE = netto) 
        /// </summary>
        public int einknettoFlag { get; set; }
        /// <summary>
        /// Bekommt 13ten Monatslohn 
        /// </summary>
        public int monatslohnXtdFlag { get; set; }
        /// <summary>
        /// Quellensteuer abgezogen 
        /// </summary>
        public int quellensteuerFlag { get; set; }
        /// <summary>
        /// Anzahl Betreibungen 
        /// </summary>
        public short anzvollstr { get; set; }
        /// <summary>
        /// Höhe Betreibungen 
        /// </summary>
        public double? betragvollstr { get; set; }
        /// <summary>
        /// Konkurs/Pfändung/Verlustschein 
        /// </summary>
        public int konkursFlag { get; set; }
        /// <summary>
        /// Wohnkosten/Miete 
        /// </summary>
        public double? miete { get; set; }
        /// <summary>
        /// Bestehende Kreditrate 
        /// </summary>
        public double? kredtrate { get; set; }
        /// <summary>
        /// Bestehende Leasingrate 
        /// </summary>
        public double? leasingrate { get; set; }
        /// <summary>
        /// Bestehende Auslagen (regelmäßig) 
        /// </summary>
        public double? auslagen { get; set; }
        /// <summary>
        /// Verweis zur Auslagenart (Lookup und Übersetzung) 
        /// </summary>
        public string auslagenCode { get; set; }
        /// <summary>
        /// Bestehende Unterstützungsverpflichtungen (regelmäßig) 
        /// </summary>
        public double? unterhalt { get; set; }
        /// <summary>
        /// Verweis zur Unterstüzungsart (Lookup und Übersetzung) 
        /// </summary>
        public string unterhaltCode { get; set; }
        /// <summary>
        /// Bestehende Auslagen (regelmäßig beruflich) 
        /// </summary>
        public double? berufsauslagen { get; set; }
        /// <summary>
        /// Verweis zur Auslagenart beruflich (Lookup und Übersetzung) 
        /// </summary>
        public string berufsauslagenCode { get; set; }
        /// <summary>
        /// Weitere Auslagen
        /// </summary>
        public double? weitereauslagen { get; set; }
        /// <summary>
        /// Weitere Auslagen Code
        /// </summary>
        public string weitereauslagenCode { get; set; }

        /// <summary>
        /// Kinderzulage
        /// </summary>
        public double? zulagekind { get; set; }
        /// <summary>
        /// Ausbildungszulage
        /// </summary>
        public double? zulageausbildung { get; set; }
        /// <summary>
        /// Sonstige Zulagen
        /// </summary>
        public double? zulagesonst { get; set; }

        /// <summary>
        /// Land Arbeitgeber 1
        /// </summary>
        public long syslandAg1 { get; set; }

        /// <summary>
        /// Wohnungart
        /// </summary>
        public String wohnungart { get; set; }

        /// <summary>
        /// Identifikationsdatum
        /// </summary>
        public DateTime? legitDatum { get; set; }
        /// <summary>
        /// Identifikationsmethode
        /// </summary>
        public string legitMethodCode { get; set; }
        /// <summary>
        /// Derjenige, der den Kunden identifiziert hat
        /// </summary>
        public string legitAbnehmer { get; set; }
        /// <summary>
        /// Kunde Identifiziert
        /// </summary>
        public int kdidentflag { get; set; }

        /// <summary>
        /// infomailflag
        /// </summary>
        public int infomailflag { get; set; }
        
        /// <summary>
        /// infosmsflag
        /// </summary>
        public int infosmsflag { get; set; }
        
        /// <summary>
        /// infotelflag
        /// </summary>
        public int infotelflag { get; set; }

       

    }
}