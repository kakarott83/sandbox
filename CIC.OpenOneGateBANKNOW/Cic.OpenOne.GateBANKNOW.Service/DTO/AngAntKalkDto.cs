﻿using System;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// General Angebot Antrag Kalkulation Dto
    /// </summary>
    public class AngAntKalkDto
    {
        /// <summary>
        /// Verweis zur Variante
        /// </summary>
        public long sysangvar { get; set; }
        /// <summary>
        /// Rap Produkte Effektivzins minimal
        /// </summary>
        public double rapzinseffMin { get; set; }
        /// <summary>
        /// Rap Produkte Effektivzins maximal
        /// </summary>
        public double rapzinseffMax { get; set; }
        /// <summary>
        /// Rap Produkte Bruttorate minimal
        /// </summary>
        public double rapratebruttoMin { get; set; }
        /// <summary>
        /// Rap Produkte Bruttorate maximal
        /// </summary>
        public double rapratebruttoMax { get; set; }
        /// <summary>
        /// Verweis zum Antrag
        /// </summary>
        public long sysantrag { get; set; }

        /// <summary>
        /// Verweis zum Produkt
        /// </summary>
        public long sysprproduct { get; set; }
        /// <summary>
        /// Produkt-Bezeichnung
        /// </summary>
        public String prProductBezeichnung { get; set; }

        /// <summary>
        /// Verweis zur Nutzungsart (privat, geschäftlich, demo)
        /// </summary>
        public long sysobusetype { get; set; }
        /// <summary>
        /// Nutzungsart-Bezeichnung
        /// </summary>
        public String obUseTypeBezeichnung { get; set; }

        /// <summary>
        /// Verweis zur Währung bei Express Auszhlung auf Karte (kann in Euro ausbezahlt werden)
        /// </summary>
        public long syswaehrung { get; set; }
        /// <summary>
        /// Währung-Bezeichnung
        /// </summary>
        public String waehrungBezeichnung { get; set; }

        /// <summary>
        /// Finanzierungsbarwert/Finanzierungssumme NETTO
        /// </summary>
        public double bgextern { get; set; }
        /// <summary>
        /// Finanzierungsbarwert/Finanzierungssumme BRUTTO
        /// </summary>
        public double bgexternbrutto { get; set; }
        /// <summary>
        /// Finanzierungsbarwert/Finanzierungssumme Umsatzsteuer
        /// </summary>
        public double bgexternust { get; set; }

        /// <summary>
        /// Barkaufpreis/Kreditbetrag/Kreditlimit exkl Zinsen, Ratenabsicherung, Steuern NETTO
        /// </summary>
        public double bgintern { get; set; }

        /// <summary>
        /// Barkaufpreis/Kreditbetrag/Kreditlimit exkl Zinsen, Ratenabsicherung, Steuern BRUTTO
        /// </summary>
        public double bginternbrutto { get; set; }

        /// <summary>
        /// Barkaufpreis/Kreditbetrag/Kreditlimit Umsatzsteuer
        /// </summary>
        public double bginternust { get; set; }

        /// <summary>
        /// Laufzeit
        /// </summary>
        public short lz { get; set; }
        /// <summary>
        /// Laufleistung bzw Jahreskilometer
        /// </summary>
        public long ll { get; set; }
        /// <summary>
        /// Zinssatz nominell Jahr
        /// </summary>
        public double zins { get; set; }
        /// <summary>
        /// Zinssatz effektiv Jahr
        /// </summary>
        public double zinseff { get; set; }
        /// <summary>
        /// Zinssatz Rap
        /// </summary>
        public double zinsrap { get; set; }
        /// <summary>
        /// Zinssatz Kundenzins bei Differenzleasing
        /// </summary>
        public double zinscust { get; set; }
        /// <summary>
        /// Sonderzahlung (1te Leasingrate, Anzahlung, Eintausch)
        /// </summary>
        public double sz { get; set; }
        /// <summary>
        /// Sonderzahlung Umsatzsteuer
        /// </summary>
        public double szUst { get; set; }
        /// <summary>
        /// Sonderzahlung Brutto
        /// </summary>
        public double szBrutto { get; set; }
        /// <summary>
        /// Restwert (Restrate, erhöhte letzte Rate, Blockrate)
        /// </summary>
        public double rw { get; set; }
        /// <summary>
        /// Restwert Umsatzsteuer
        /// </summary>
        public double rwUst { get; set; }
        /// <summary>
        /// Restwert Brutto
        /// </summary>
        public double rwBrutto { get; set; }
        /// <summary>
        /// Rate (Leasingrate, Kreditrate)
        /// </summary>
        public double rate { get; set; }
        /// <summary>
        /// Rate Umsatzsteuer
        /// </summary>
        public double rateUst { get; set; }
        /// <summary>
        /// Rate Brutto
        /// </summary>
        public double rateBrutto { get; set; }
        /// <summary>
        /// Kaution (Depot)
        /// </summary>
        public double depot { get; set; }
        /// <summary>
        /// Verrechungsbetrag
        /// </summary>
        public double verrechnung { get; set; }
        /// <summary>
        /// Verrechnung von Sonderzahlung und Kaution mit Auszahlungsbetrag
        /// </summary>
        public bool verrechnungFlag { get; set; }
        /// <summary>
        /// Auszahlungsbetrag
        /// </summary>
        public double auszahlung { get; set; }
        /// <summary>
        /// Auszahlungstyp (Überweisung, Cachkarte ...)
        /// </summary>
        public short auszahlungTyp { get; set; }
        /// <summary>
        /// Rückzahlungstyp (Überweisung, Abbuchung …)
        /// </summary>
        public short rueckzahlungTyp { get; set; }
        /// <summary>
        /// Zinskosten gesamt (berechnet)
        /// </summary>
        public double calcZinskosten { get; set; }
        /// <summary>
        /// Ratenabsicherung gesamt (berechnet)
        /// </summary>
        public double calcRsvgesamt { get; set; }
        /// <summary>
        /// Ratenabsicherung monatlich verzinst (berechnet)
        /// </summary>
        public double calcRsvmonat { get; set; }
        /// <summary>
        /// Zins auf Ratenabsicherung (berechnet)
        /// </summary>
        public double calcRsvzins { get; set; }
        /// <summary>
        /// Umsatzsteuer auf Zins (berechnet)
        /// </summary>
        public double calcUstzins { get; set; }

        /// <summary>
        /// Ratenabsicherung monatlich verzinst (berechnet für Rap min)
        /// </summary>
        public double calcRsvmonatMin { get; set; }
        /// <summary>
        /// Ratenabsicherung monatlich verzinst (berechnet für Rap max)
        /// </summary>
        public double calcRsvmonatMax { get; set; }
        /// <summary>
        /// Zinskosten gesamt (berechnet für Rap min)
        /// </summary>
        public double calcZinskostenMin { get; set; }
        /// <summary>
        /// Zinskosten gesamt (berechnet für Rap max)
        /// </summary>
        public double calcZinskostenMax { get; set; }
        /// <summary>
        /// Aufschub GESCH_MARK_AUFSCHUB
        /// </summary>
        public int aufschub { get; set; }
        /// <summary>
        /// Satzmehrkm OB_MARK_SATZMEHRKM
        /// </summary>
        public double satzmehrkm { get; set; }

        /// <summary>
        /// Benutzte Handelsgruppe der Kalkulation
        /// </summary>
        public long sysprhgrp { get; set; }

        /// <summary>
        /// Rate Brutto
        /// </summary>
        public double rateBruttoInklAbsicherung { get; set; }
        /// <summary>
        /// Erste Rate Brutto
        /// </summary>
        public double ersteRateBruttoInklAbsicherung { get; set; }

        /// <summary>
        /// Erstauszahlung Karte
        /// </summary>
        public double initLadung { get; set; }

       /// <summary>
       ///  Kundengruppe
       /// </summary>
        public long? calcPrkgroup { get; set; }

        /// <summary>
        /// Abo Periodizitaet
        /// </summary>
        public int aboppy { get; set; }

        /// <summary>
        /// Abo Betrag
        /// </summary>
        public double abobetrag { get; set; }

        /// <summary>
        /// Abo Beginn Gueltig ab (für Monat + Jahr)
        /// </summary>
        public DateTime? abobeginn { get; set; }

        /// <summary>
        /// Abo ende (für Monat + Jahr)
        /// </summary>eginn
        public DateTime? aboende { get; set; }

        /// <summary>
        /// Abo Beginn Tag
        /// </summary>
        public int aboauszltag { get; set; }

        /// <summary>
        /// Flag Budgetrechner wurde benutzt
        /// </summary>
        public int? flagopt1 { get; set; }

        /// <summary>
        /// Restwert RWA Netto
        /// </summary>
        public double rwBase { get; set; }

        /// <summary>
        /// Restwert RWA Brutto
        /// </summary>
        public double rwBaseBrutto { get; set; }

        /// <summary>
        /// Restwert Brutto Prozent
        /// </summary>
        public double rwKalkBruttoP { get; set; }

    }
}