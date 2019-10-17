using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Versicherung
{
    /// <summary>
    /// Versicherung Kalkulationskomponente
    /// </summary>
    public class InsuranceCalcComponent
    {
        /// <summary>
        /// Versicherungsberechnungs komponente
        /// </summary>
        /// <param name="type">typ</param>
        /// <param name="value">Wert</param>
        public InsuranceCalcComponent(InsuranceCalcComponentType type, double value)
        {
            this.type = type;
            this.value = value;

        }

        /// <summary>
        /// Kalkulations komponenten Typ ID
        /// </summary>
        public InsuranceCalcComponentType type { get; set; }

        /// <summary>
        /// Wert der Kalkulationskomponente
        /// </summary>
        public double value { get; set; }
    }

    /// <summary>
    /// Defines different types of additional input-values needed for a insurance calculation
    /// </summary>
    public enum InsuranceCalcComponentType
    {
        /// <summary>
        /// Laufzeit
        /// </summary>
        Laufzeit,
        /// <summary>
        /// Bruttorate, ungerunded
        /// </summary>
        RateBruttoUnrounded,
        /// <summary>
        /// Zins
        /// </summary>
        Zins,
        /// <summary>
        /// Zins
        /// </summary>
        Aufschub
    }
}
