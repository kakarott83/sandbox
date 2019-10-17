using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Dto für aggregierte Daten aus OpenLease
    /// </summary>
    public class AggregationOLOutDto 
    {
        /// <summary>
        /// Anzahl Annulierungen in den letzten 12 Monaten
        /// </summary>
        public int? ANZANL12 { get; set; }

        /// <summary>
        /// Anzahl Antraege
        /// </summary>
        public int? ANZAT { get; set; }

        /// <summary>
        /// AnzahlKundenIDs
        /// </summary>
        public int? ANZKD { get; set; }

        /// <summary>
        /// Anzahl Mahnungen1
        /// </summary>
        public int? ANZMAHN1 { get; set; }

        /// <summary>
        /// Anzahl Mahnungen2
        /// </summary>
        public int? ANZMAHN2 { get; set; }

        /// <summary>
        /// Anzahl Mahnungen3
        /// </summary>
        public int? ANZMAHN3 { get; set; }

        /// <summary>
        /// Anzahl Mehrfachantraege
        /// </summary>
        public int? ANZMAT { get; set; }

        /// <summary>
        /// Anzahl OP
        /// </summary>
        public int? ANZOP { get; set; }

        /// <summary>
        /// Anzahl Stundungen
        /// </summary>
        public int? ANZSTUNDUNGEN { get; set; }

        /// <summary>
        /// Anzahl Vertraege
        /// </summary>
        public int? ANZVT { get; set; }

        /// <summary>
        /// Anzahl laufende Vertraege
        /// </summary>
        public int? ANZVTL { get; set; }

        /// <summary>
        /// Anzahl Vertraege im Recovery
        /// </summary>
        public int? ANZVTR { get; set; }

        /// <summary>
        /// Anzahl Verzichte in den letzten 12 Monaten
        /// </summary>
        public int? ANZVZL12 { get; set; }

        /// <summary>
        /// Anzahl Zahlungsvereinbarungen
        /// </summary>
        public int? ANZZVB { get; set; }

        /// <summary>
        /// Dauer Kundenbeziehun (sic!)
        /// In der AGGOUTOL heisst das Feld DAUERKUNDENBEZIEHUN.
        /// </summary>
        public int? DAUERKUNDENBEZIEHUN { get; set; }

        /// <summary>
        /// Effektive Kundenbeziehung
        /// </summary>
        public int? EFFKUNDENBEZIEHUNG { get; set; }

        /// <summary>
        /// Engagement
        /// </summary>
        public decimal? ENGAGEMENT { get; set; }

        /// <summary>
        /// Eventualengagement
        /// </summary>
        public decimal? EVTLENGAGEMENT { get; set; }

        /// <summary>
        /// Gesamtengagement
        /// </summary>
        public decimal? GESAMTENGAGEMENT { get; set; }

        /// <summary>
        /// Haushaltsengagement
        /// </summary>
        public decimal? HAUSHALTENGAGEMENT { get; set; }

        /// <summary>
        /// Miete
        /// </summary>
        public decimal? LETZTEMIETE { get; set; }

        /// <summary>
        /// Nationalitaet
        /// </summary>
        public String LETZTENAT { get; set; }

        /// <summary>
        /// Letzter Bonus
        /// </summary>
        public decimal? LETZTERBONUS { get; set; }

        /// <summary>
        /// Letzte Risikoklasse
        /// </summary>
        public String LETZTERISIKOKL { get; set; }

        /// <summary>
        /// Zivilstand
        /// </summary>
        public String LETZTERZIVILSTAND { get; set; }

        /// <summary>
        /// Letztes Arbeitsverhaeltnis
        /// </summary>
        public String LETZTESAV { get; set; }

        /// <summary>
        /// Letztes Haupteinkommen
        /// </summary>
        public decimal? LETZTESHE { get; set; }

        /// <summary>
        /// Letztes Nebeneinkommen
        /// </summary>
        public decimal? LETZTESNE { get; set; }

        /// <summary>
        /// Letztes Wohnverhaeltins
        /// </summary>
        public String LETZTESWV { get; set; }

        /// <summary>
        /// Letztes Zusatzeinkommen
        /// </summary>
        public decimal? LETZTESZE { get; set; }

        /// <summary>
        /// Maximaler Badlisteintrag
        /// </summary>
        public int? MAXBADLIST { get; set; }

        /// <summary>
        /// Maximale aktuelle Risikoklasse
        /// </summary>
        public String MAXCURRISIKOKL { get; set; }

        /// <summary>
        /// Maximale Mahnstufe
        /// </summary>
        public int? MAXMAHNSTUFE { get; set; }

        /// <summary>
        /// Maximale Risikoklasse
        /// </summary>
        public String MAXRISIKOKL { get; set; }

        /// <summary>
        /// Minimales Datum Kunde Seit
        /// </summary>
        public DateTime? MINKUNDESEIT { get; set; }

        /// <summary>
        /// Summe OP
        /// </summary>
        public decimal? SUMOP { get; set; }

        /// <summary>
        /// Jüngste Stundung
        /// </summary>
        public DateTime? DATESTUNDUNGEN { get; set; }

        /// <summary>
        /// Jüngste ZVB
        /// </summary>
        public int? DATEZVB { get; set; }

        /// <summary>
        /// Jüngste 1. Mahnung
        /// </summary>
        public DateTime? DATEMAHN1 { get; set; }

        /// <summary>
        /// Jüngste 2. Mahnung
        /// </summary>
        public DateTime? DATEMAHN2 { get; set; }

        /// <summary>
        /// Jüngste 3. Mahnung
        /// </summary>
        public DateTime? DATEMAHN3 { get; set; }

        /// <summary>
        /// die Anzahl der Stops aus allen laufenden Verträgen
        /// </summary>
        public int? ANZAUFSTOCKSTOP { get; set; }

        /// <summary>
        /// das maximale Datum der Stops 
        /// </summary>
        public DateTime? DATEAUFSTOCKSTOP { get; set; }

        /// <summary>
        /// Zähle die manuellen Ablehnungen in OL der letzten 183 Tage
        /// </summary>
        public int? ANZMANABL6M  { get; set; } // NUMBER(5)   
        
        /// <summary>
        /// Zähle die manuellen Ablehnungen in OL der letzten 366 Tage
        /// </summary>                                                                                                                                                      
        public int? ANZMANABL12M { get; set; } // NUMBER(5)   

        /// <summary>
        /// Zähle die Verträge, bei denen vtoption.option06 nicht leer (oder 0?) ist
        /// </summary>
        public int? ANZVTSPEZ { get; set; } // NUMBER(5)  

        /// <summary>
        /// Zähle die Verträge, bei denen vtoption.option06 nicht leer (oder 0?) ist UND die noch nicht saldiert sind (vt.endeam = NULL
        /// </summary>
        public int? ANZVTSPEZLFD { get; set; } // NUMBER(5)  

        /// <summary>
        /// Datum erster Antrag des Kunden
        /// </summary>
        public DateTime? DATUMERSTERANTRAG { get; set; }

        /// <summary>
        /// Datum letzter Antrag des Kunden
        /// </summary>
        public DateTime? DATUMLETZTERANTRAG { get; set; }

        /// <summary>
        /// Durchschnittliche Anzahl erster Mahnungen pro Vertrag (auf Person/Dublette aggregiert) in den letzten 6 Monaten à (rnmahn.mahnstufe = 1)
        /// </summary>
        public decimal? ANZMAHN1AVG6M { get; set; }

        /// <summary>
        /// Durchschnittliche Anzahl zweiter Mahnungen pro Vertrag (auf Person/Dublette aggregiert) in den letzten 6 Monaten à (rnmahn.mahnstufe = 2)
        /// </summary>
        public decimal? ANZMAHN2AVG6M { get; set; }

        /// <summary>
        /// Durchschnittliche Anzahl zweiter Mahnungen pro Vertrag (auf Person/Dublette aggregiert) in den letzten 6 Monaten à (rnmahn.mahnstufe = 3)
        /// </summary>
        public decimal? ANZMAHN3AVG6M { get; set; }

        /// <summary>
        /// Durchschnittliche Anzahl Einzahlung pro Vertrag (auf Person/Dublette aggregiert) in den letzten 12 Monaten à count(fi.gebuchtdatum)
        /// </summary>
        public decimal? ANZZAHLAVG12M { get; set; }

        /// <summary>
        /// Durchschnittliche Reduktion Buchsaldo pro Vertrag (auf Person/Dublette aggregiert) in den letzten 12 Monaten
        /// </summary>
        public decimal? RUECKSTANDAVG { get; set; }

        /// <summary>
        /// Durchschnittlicher offene Posten (Rückstand) über alle Verträge, zum aktuellen Zeitpunkt
        /// </summary>
        public decimal? BUCHSALDOAVG { get; set; }

      
    }
}
