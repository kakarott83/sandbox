using System;

namespace Cic.OpenOne.GateOEM.Service.DTO
{
    /// <summary>
    /// Calculation Information
    /// </summary>
    public class CalculationDto
    {


        /// <summary>
        /// Verweis zum Produkt
        /// </summary>
        public long sysprproduct { get; set; }
        /// <summary>
        /// Produkt-Bezeichnung
        /// </summary>
        public String prProductName { get; set; }

        /// <summary>
        /// Verweis zur Nutzungsart (privat, geschäftlich, demo)
        /// </summary>
        public long sysobusetype { get; set; }
        /// <summary>
        /// Nutzungsart-Bezeichnung
        /// </summary>
        public String obUseTypeName { get; set; }

        /// <summary>
        /// Verweis zur Währung bei Express Auszhlung auf Karte (kann in Euro ausbezahlt werden)
        /// </summary>
        public long sysCurrency { get; set; }
        /// <summary>
        /// Währung-Bezeichnung
        /// </summary>
        public String currencyName { get; set; }

        /// <summary>
        /// Finanzierungsbarwert/Finanzierungssumme NETTO
        /// </summary>
        public double cashvalueExtern { get; set; }
        /// <summary>
        /// Finanzierungsbarwert/Finanzierungssumme BRUTTO
        /// </summary>
        public double cashvalueExternPretax { get; set; }
        /// <summary>
        /// Finanzierungsbarwert/Finanzierungssumme Umsatzsteuer
        /// </summary>
        public double cashvalueExternTurnovertax { get; set; }

        /// <summary>
        /// Barkaufpreis/Kreditbetrag/Kreditlimit exkl Zinsen, Ratenabsicherung, Steuern NETTO
        /// </summary>
        public double cashvalueIntern { get; set; }

        /// <summary>
        /// Barkaufpreis/Kreditbetrag/Kreditlimit exkl Zinsen, Ratenabsicherung, Steuern BRUTTO
        /// </summary>
        public double cashvalueInternPretax { get; set; }

        /// <summary>
        /// Barkaufpreis/Kreditbetrag/Kreditlimit Umsatzsteuer
        /// </summary>
        public double cashvalueInternTurnovertax { get; set; }
        /// <summary>
        /// Mietvorauszahlung
        /// </summary>
        public double downpayment { get; set; }
        /// <summary>
        /// Laufzeit
        /// </summary>
        public short duration { get; set; }
        /// <summary>
        /// Laufleistung bzw Jahreskilometer
        /// </summary>
        public long mileage { get; set; }
        /// <summary>
        /// Zinssatz nominell Jahr
        /// </summary>
        public double interestPerAnnum { get; set; }
        /// <summary>
        /// Zinssatz effektiv Jahr
        /// </summary>
        public double actualInterestRate { get; set; }

        /// <summary>
        /// Sonderzahlung (1te Leasingrate, Anzahlung, Eintausch)
        /// </summary>
        public double specialpayment { get; set; }
        /// <summary>
        /// Sonderzahlung Umsatzsteuer
        /// </summary>
        public double specialpaymentTurnovertax { get; set; }
        /// <summary>
        /// Sonderzahlung Brutto
        /// </summary>
        public double specialpaymentPretax { get; set; }
        /// <summary>
        /// Restwert (Restrate, erhöhte letzte Rate, Blockrate)
        /// </summary>
        public double decliningbalance { get; set; }
        /// <summary>
        /// Restwert Umsatzsteuer
        /// </summary>
        public double decliningbalanceTurnovertax { get; set; }
        /// <summary>
        /// Restwert Brutto
        /// </summary>
        public double decliningbalancePretax { get; set; }
        /// <summary>
        /// Rate (Leasingrate, Kreditrate)
        /// </summary>
        public double rate { get; set; }
        /// <summary>
        /// Rate Umsatzsteuer
        /// </summary>
        public double rateTurnovertax { get; set; }
        /// <summary>
        /// Rate Brutto
        /// </summary>
        public double ratePretax { get; set; }
        /// <summary>
        /// Kaution (Depot)
        /// </summary>
        public double deposit { get; set; }

        /// <summary>
        /// Auszahlungsbetrag
        /// </summary>
        public double amountPaid { get; set; }

        /// <summary>
        /// Zinskosten gesamt (berechnet)
        /// </summary>
        public double interestChargesTotal { get; set; }
        /// <summary>
        /// Ratenabsicherung gesamt (berechnet)
        /// </summary>
        public double paymentInsuranceTotal { get; set; }

        /// <summary>
        /// Satzmehrkm OB_MARK_SATZMEHRKM
        /// </summary>
        public double standardMileageAmount { get; set; }



    }

}