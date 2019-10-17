using Cic.OpenOne.Common.Util;
using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// General Baseclass for Angebot/Antrag Dto
    /// </summary>
    public class AngAntDto
    {
        /// <summary>
        /// Testfall
        /// </summary>
        public bool? testFlag { get; set; }
        /// <summary>
        /// Notstop
        /// </summary>
        public bool? notstopFlag { get; set; }
        /// <summary>
        /// Verweis zum Interessenten
        /// </summary>
        public long? sysit { get; set; }
        
        /// <summary>
        /// Verweis zum Bankkunden
        /// </summary>
        public long? syskd { get; set; }
        /// <summary>
        /// Bankkunde
        /// </summary>
        public String kdBezeichnung { get; set; }
        
        /// <summary>
        /// Verweis zum Kanal (FF, KF)
        /// </summary>
        public long? sysprchannel { get; set; }
        /// <summary>
        /// Kanal-Bezeichnung
        /// </summary>
        public String prChannelBezeichnung { get; set; }

        /// <summary>
        /// Verweis zur Handelsgruppe
        /// </summary>
        public long? sysprhgroup { get; set; }
        /// <summary>
        /// Handelsgruppe-Bezeichnung
        /// </summary>
        public String prHgroupBezeichnung { get; set; }

        /// <summary>
        /// Verweis zur Brand
        /// </summary>
        public long sysbrand { get; set; }
        /// <summary>
        /// Brand-Bezeichnung
        /// </summary>
        public String brandBezeichnung { get; set; }

        /// <summary>
        /// Verweis zur Marketingaktion (Kampagnencode)
        /// </summary>
        public long? sysmarktab { get; set; }
        /// <summary>
        /// Marketingaktion-Bezeichnung
        /// </summary>
        public String marktabBezeichnung { get; set; }

        /// <summary>
        /// Erfasser
        /// </summary>
        public long? syswfuser { get; set; }
        /// <summary>
        /// Erfasser-Bezeichnung
        /// </summary>
        public String wfuserBezeichnung { get; set; }

        /// <summary>
        /// Erfasst am
        /// </summary>
        public DateTime? erfassung { get; set; }

        /// <summary>
        /// Änderer
        /// </summary>
        public long? syswfuserchange { get; set; }
        /// <summary>
        /// Änderer-Bezeichnung
        /// </summary>
        public String wfUserChangeBezeichnung { get; set; }

        /// <summary>
        /// Geändert am
        /// </summary>
        public DateTime? aenderung { get; set; }

        /// <summary>
        /// Betreuer (Antragsowner)
        /// </summary>
        public long? sysberater { get; set; }
        
		/// <summary>
        /// Betreuer-Bezeichnung
        /// </summary>
        public String beraterBezeichnung { get; set; }

        /// <summary>
        /// Gültigkeit Angebot
        /// </summary>
        public DateTime? gueltigBis { get; set; }
        /// <summary>
        /// Zustand (Status)
        /// </summary>
        public String zustand { get; set; }
        /// <summary>
        /// Zustandsübergang am
        /// </summary>
        public DateTime? zustandAm { get; set; }
        /// <summary>
        /// Attribut
        /// </summary>
        public String attribut { get; set; }

        /// <summary>
        /// Vertriebsweg (berechnet, keine Eingabe, zb FF direkt etc)
        /// </summary>
        public String vertriebsweg { get; set; }
        /// <summary>
        /// KKG Pflicht
        /// </summary>
        public bool? kkgpflicht { get; set; }
        /// <summary>
        /// Kartennummer bei Auszahlung auf Kreditkarte
        /// </summary>
        public String kartennummer { get; set; }
        /// <summary>
        /// Verweis zum Verwendungszweck(Lookup und Übersetzung)
        /// </summary>
        public String verwendungszweckCode { get; set; }

        /// <summary>
        /// Attributsübergang am
        /// </summary>
        public String attributAm { get; set; }

        /// <summary>
        /// Antrag Object Data Transfer Object
        /// </summary>
        public AngAntObDto angAntObDto { get; set; }

        /// <summary>
        /// Korrespondenzadresse
        /// </summary>
        public long sysKorrAdresse { get; set; }
        /// <summary>
        /// Korrespondenzadresse-Bezeichnung
        /// </summary>
        public String korrAdresseBezeichnung { get; set; }

        /// <summary>
        /// Verknüpfung zu ITKONTO
        /// </summary>
        public long sysItKonto { get; set; }
        /// <summary>
        /// ItKonto-Bezeichnung
        /// </summary>
        public String itKontoBezeichnung { get; set; }

        /// <summary>
        /// ErfassungsClient
        /// (Ticket#2012080910000035 processAngebotToAntrag)
        /// Clienttyp (10=B2B, 20=MA, 30=B2C, 50=ONE)
        /// </summary>
        public long erfassungsclient { get; set; }

        /// <summary>
        /// Vermittler = Händler
        /// (Ticket#2012090510000531  processAngebotToAntrag)
        /// </summary>
        public long sysVM { get; set; }

        /// <summary>
        /// Verkäufer = Mitarbeiter
        /// (Ticket#2012090510000531  processAngebotToAntrag)
        /// </summary>
        public long sysVK { get; set; }

        /// <summary>
        /// SYSABWICKLUNG
        /// (Ticket#2012090510000531  processAngebotToAntrag)
        /// </summary>
        public long sysAbwicklung { get; set; }

        /// <summary>
        /// Abwicklungsort
        /// (BNRELF-1389)
        /// </summary>
        public string abwicklungsort { get; set; }

        /// <summary>
        /// sysMwst
        /// (Ticket#2012090510000531  CopyAngebotByID)
        /// </summary>
        public long sysMwst { get; set; }

        /// <summary>
        /// SysWaehrung 
        /// (Ticket#2012090510000531  CopyAngebotByID)
        /// </summary>
        public long sysWaehrung { get; set; }

        /// <summary>
        /// Kampagne
        /// </summary>
        public long syscamp { get; set; }

        /// <summary>
        /// Bemerkung - SYSWFMMKAT=4
        /// </summary>
        public String bemerkung { get; set; }

        public DateTime? aufstockstopBis { get; set; }
        public string gutschriftreferenz { get; set; }
        public DateTime? datEinreichung { get; set; }

        //CASA Spezialfelder DDLKPPOS
        public string eigenheim_strasse { get; set; }
        public string eigenheim_str_nr { get; set; }
        public string eigenheim_plz { get; set; }
        public string eigenheim_ort { get; set; }
        public string eigentuemer_seit_monat { get; set; }
        public string eigentuemer_seit_jahr { get; set; }
        public double? hypothekenhoehe { get; set; }

        //DIPLOMA Spezialfelder DDLKPPOS
        public string schule_name { get; set; }
        public string schule_strasse { get; set; }
        public string schule_str_nr { get; set; }
        public string schule_plz { get; set; }
        public string schule_ort { get; set; }

        public string emboss { get; set; }

        public short einzug { get; set; }
        /// <summary>
        /// Verweis auf die VertragsArt
        /// </summary>
        public long sysvart { get; set; }
        public long? sysLS { get; set; }
        public int ppy { get; set; }
        public String mandatreferenz { get; set; }
        public long? sysKI { get; set; }

        /// <summary>
        /// additional flags, dates and comments
        /// </summary>
        public AngAntOptionDto options { get; set; }


        /// <summary>
        /// Kampagnentyp
        /// </summary>
        public long syscamptp { get; set; }


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
        public long? dateinreichungzeit { get; set; }
        //Auto-combinded of above two fields
        public DateTime? einreichungsDatum
        {
            get
            {
                return DateTimeHelper.CreateDate(datEinreichung, dateinreichungzeit);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    dateinreichungzeit = (long)val.Value;
                else
                    dateinreichungzeit = 0;
                datEinreichung = value;
            }
        }

        /// <summary>
        /// Externe ID (in Verbindung mit erfassungsclient gespeichert)
        /// </summary>
        public String extident { get; set; }
    }
}
