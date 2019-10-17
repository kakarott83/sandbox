using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// KREMO Input Data Transfer Object
    /// </summary>
    public class KREMOInDto
    {
        /// <summary>
        /// Format: YYYYMMDD
        /// </summary>
        public double GebDatum { get;  set; }

        /// <summary>
        ///  0 = Herr / 1 = Frau
        /// </summary>
        public double Anredecode { get;  set; }

        /// <summary>
        ///  0=Kredit, 1=Leasing, 2=Teilzahler
        /// </summary>
        public double Kalkcode { get;  set; }

        /// <summary>
        /// Gesamtlaufzeit neuer Kredit/Leasing
        /// </summary>
        public double Glz { get;  set; }

        /// <summary>
        /// Betrag neuer Kredit/Leasing
        /// </summary>
        public double Kreditsumme { get;  set; }

        /// <summary>
        /// Restwert neues Leasing
        /// </summary>
        public double Rw { get;  set; }

        /// <summary>
        /// Zinssatz neuer Kredit/Leasing in Prozent
        /// </summary>
        public double Zins { get;  set; }

        /// <summary>
        /// 0=nominal, 1=effektiv
        /// </summary>
        public double Zinsnomflag { get;  set; }

        /// <summary>
        /// Bruttoeinkommen
        /// </summary>
        public double Einkbrutto { get;  set; }

        /// <summary>
        /// mtl. Nettoeinkommen
        /// </summary>
        public double Einknetto { get; set; }

        /// <summary>
        /// Nebeneinkommen brutto
        /// </summary>
        public double Nebeneinkbrutto { get; set; }

        /// <summary>
        /// Nebeneinkommen netto
        /// </summary>
        public double Nebeneinknetto { get; set; }

        /// <summary>
        /// 0=ledig, 1=verheiratet, 2=geschieden, 3=gerichtlich getrennt, 4=verwitwet, 5=eingetragene Partnerschaft, 
        /// 6=gerichtlich aufgelöste Partnerschaft (wird wie geschieden behandelt)
        /// </summary>
        public double Famstandcode { get; set; }

        /// <summary>
        /// 0=alleinstehende, 1=alleinstehende in Haushaltgemeinschaft mit Erwachsenem, 
        /// 2=Ehepaar in dauernder Haushaltgemeinschaft, 3=alleinerziehende mit Unterstützungspflichten ohne Haushaltgemeinschaft,
        /// 4=alleinerziehend mit Haushaltgemeinschaft
        /// </summary>
        public double Grundcode { get; set; }

        /// <summary>
        /// 0=AG, 1=AR, 2=AI, 3=BS, 4=BL, 5=BE, 6=FR, 7=GE, 8=GL, 9=GR, 10=JU, 11=LU, 12=NE, 13=NW, 14=OW, 15=SH, 16=SZ, 17=SO, 18=SG,
        /// 19=TG, 20=TI, 21=UR, 22=VS, 23=VD, 24=ZG, 25=ZH, 26=FL
        /// </summary>
        public double Kantoncode { get; set; }

        /// <summary>
        /// Anzahl Kinder bis 6 Jahre
        /// </summary>
        public double Anzkind1 { get; set; }

        /// <summary>
        /// Anzahl Kinder 11 bis 12 Jahre
        /// </summary>
        public double Anzkind2 { get;  set; }

        /// <summary>
        /// Anzahl Kinder über 12 Jahre
        /// </summary>
        public double Anzkind3 { get;  set; }

        /// <summary>
        /// Miete
        /// </summary>
        public double Miete { get;  set; }

        /// <summary>
        /// PLZ
        /// </summary>
 
        public double Plz { get;  set; }

        /// <summary>
        /// Unterstützungs-/Unterhaltsbeiträge
        /// </summary>
        public double Unterhalt { get;  set; }

        /// <summary>
        ///  Quellensteuer-ID: 0=nein, 1=ja
        /// </summary>
        public double Qstflag { get;  set; }

        /// <summary>
        /// Anzahl Kinder 7 bis 10 Jahre Der Betrag der gemäss Anzahl Kinder in die Haushaltsrechnung einfliesst, wird in die
        /// bestehenden KLKKDN.KIND1 (Kanton umgestellt) bzw. KLKKDN.KIND2 (Kanton noch nicht umgestellt) Felder zurückgegeben.
        /// </summary>
        public double Anzkind4 { get; set; }

        /// <summary>
        ///  Format: YYYYMMDD
        /// </summary>
        public double GebDatum2 { get;  set; }

        /// <summary>
        /// 0 = Herr / 1 = Frau
        /// </summary>
        public double Anredecode2 { get;  set; }

        /// <summary>
        /// Einkommen Brutto 2
        /// </summary>
        public double Einkbrutto2 { get;  set; }

        /// <summary>
        /// Einkommen Netto 2
        /// </summary>
        public double Einknetto2 { get; set; }

        /// <summary>
        /// Nebeneinkommen Brutto 2
        /// </summary>
        public double Nebeneinkbrutto2 { get; set; }

        /// <summary>
        /// Nebeneinkommen Netto 2
        /// </summary>
        public double Nebeneinknetto2 { get; set; }

        /// <summary>
        /// 0=ledig, 1=verheiratet, 2=geschieden, 3=gerichtlich getrennt, 4=verwitwet, 5=eingetragene Partnerschaft, 
        /// 6=gerichtlich aufgelöste Partnerschaft (wird wie geschieden behandelt)
        /// </summary>
        public double Famstandcode2 { get; set; }

        /// <summary>
        /// Kanton Code
        /// </summary>
        public double Kantoncode2 { get;  set; }

        /// <summary>
        /// PLZ 2
        /// </summary>
        public double Plz2 { get;  set; }

        /// <summary>
        /// Unterhalt 2
        /// </summary>
        public double Unterhalt2 { get;  set; }

        /// <summary>
        /// Ost Flag 2
        /// </summary>
        public double Qstflag2 { get; set; }

        /// <summary>
        /// Sys KREMO
        /// </summary>
        public long? SysKremo { get; set; }
        
        /// <summary>
        /// SYS Auskunft
        /// </summary>
        public long? SysAuskunft { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Fininstcode { get; set; }

        /// <summary>
        /// SysAngebot
        /// </summary>
        public long? SysAngebot { get; set; }

        /// <summary>
        /// SysAntrag
        /// </summary>
        public long? SysAntrag { get; set; }

        /// <summary>
        /// Sysit
        /// </summary>
        public long? SysIt { get; set; }
        /// <summary>
        /// betreuungskosten
        /// </summary>
        public double betreuungskosten { get; set; }

        /// <summary>
        /// arbeitswegpauschale1
        /// </summary>
        public double arbeitswegpauschale1 { get; set; }
        /// <summary>
        /// arbeitswegpauschale2
        /// </summary>
        public double arbeitswegpauschale2 { get; set; }

        /// <summary>
        /// Krankenkassenbetrag
        /// </summary>
        public double krankenkasse { get; set; }
    }
}
