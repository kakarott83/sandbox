using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Schnittstelle für Rundungsklasse (BO)
    /// </summary>
    public interface IRounding
    {
        /// <summary>
        /// Auf die angegebene Nachkommastelle runden
        /// </summary>
        /// <param name="value">Eingangswert</param>
        /// <param name="digits">nachkommastelle</param>
        /// <returns>Gerundeter Ausgangswert</returns>
        double Round(double value, int digits);

        /// <summary>
        /// Auf zwei Nachkommastellen zum nächsten vielfachen von fünf der zweiten Nachkommastelle runden
        /// </summary>
        /// <param name="value">Eingangssumme</param>
        /// <returns>Gerundete Summe</returns>
        double RoundCHF(double value);

        /// <summary>
        /// Netto aus dem Brutto errechnen
        /// </summary>
        /// <param name="grossValue">Bruttowert</param>
        /// <param name="taxRate">Steuerfaktor in Prozent</param>
        /// <returns>Nettowert</returns>
        double getNetValue(double grossValue, double taxRate);

        /// <summary>
        /// Bruttowert aus dem Nettowert errechnen
        /// </summary>
        /// <param name="netValue">Nettowert der Summe</param>
        /// <param name="taxRate">Steuerfaktor in Prozent</param>
        /// <returns>Bruttowert</returns>
        double getGrossValue(double netValue, double taxRate);

        /// <summary>
        /// Steuer aus dem Nettowert errechnen
        /// </summary>
        /// <param name="netValue">Nettowert der Summe</param>
        /// <param name="taxRate">Steuerfaktor in Prozent</param>
        /// <returns>Steuerwert</returns>
        double getTaxValue(double netValue, double taxRate);
    }
}
