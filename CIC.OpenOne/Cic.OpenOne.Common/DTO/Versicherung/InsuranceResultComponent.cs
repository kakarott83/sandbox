using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Versicherung
{
    /// <summary>
    /// Versicherung Kalkulationskomponenten-Ergebnis
    /// </summary>
    public class InsuranceResultComponent
    {
        /// <summary>
        /// Versicherungs Ergebnis Komponente
        /// </summary>
        /// <param name="type">Parameter</param>
        /// <param name="value">Wert</param>
        public InsuranceResultComponent(InsuranceResultComponentType type, double value)
        {
            this.type = type;
            this.value = value;
        }

        /// <summary>
        /// Kalkulations komponenten Typ ID
        /// </summary>
        public InsuranceResultComponentType type { get; set; }

        /// <summary>
        /// Wert der Kalkulationskomponente
        /// </summary>
        public double value { get; set; }
    }

    /// <summary>
    /// Defines different types of additional output-values returned by some insurance calculations
    /// </summary>
    public enum InsuranceResultComponentType
    {
        /// <summary>
        /// rsvMonat
        /// </summary>
        RsvMonat,
        /// <summary>
        /// RsvGesamt
        /// </summary>
        RsvGesamt,
        /// <summary>
        /// RsvBarwertAddition
        /// </summary>
        RsvBarwertAddition,
        /// <summary>
        /// Zinskosten
        /// </summary>
        RsvZins
    }
}
