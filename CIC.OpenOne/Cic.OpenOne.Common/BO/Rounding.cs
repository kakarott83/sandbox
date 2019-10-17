using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Runding BO
    /// </summary>
    public class Rounding :IRounding
    {

        /// <summary>
        /// Auf zwei Nachkommastellen zum nächsten vielfachen von fünf der zweiten Nachkommastelle runden
        /// </summary>
        /// <param name="value">Eingangssumme</param>
        /// <returns>Gerundete Summe</returns>
        public double RoundCHF(double value)
        {
            //Round to 2 places after coma
            return System.Math.Round(value * 20, 0) / 20;
        }



        /// <summary>
        /// Auf die angegebene Nachkommastelle runden
        /// </summary>
        /// <param name="value">Eingangswert</param>
        /// <param name="digits">nachkommastelle</param>
        /// <returns>Gerundeter Ausgangswert</returns>
        public double Round(double value, int digits)
        {
            return System.Math.Round(value, digits);
        }

        /// <summary>
        /// Netto aus dem Brutto errechnen
        /// </summary>
        /// <param name="grossValue">Bruttowert</param>
        /// <param name="taxRate">Steuerfaktor in Prozent</param>
        /// <returns>Nettowert</returns>
        public double getNetValue(double grossValue, double taxRate)
        {
            return (grossValue * 100) / (100 + taxRate);
        }

        /// <summary>
        /// Steuer aus dem Nettowert errechnen
        /// </summary>
        /// <param name="netValue">Nettowert der Summe</param>
        /// <param name="taxRate">Steuerfaktor in Prozent</param>
        /// <returns>Steuerwert</returns>
        public double getTaxValue(double netValue, double taxRate)
        {
            return (netValue / 100) *taxRate;
        }

        /// <summary>
        /// Bruttowert aus dem Nettowert errechnen
        /// </summary>
        /// <param name="netValue">Nettowert der Summe</param>
        /// <param name="taxRate">Steuerfaktor in Prozent</param>
        /// <returns>Bruttowert</returns>
        public double getGrossValue(double netValue, double taxRate)
        {
            return netValue * (100 + taxRate) / 100;
        }

    }
}
