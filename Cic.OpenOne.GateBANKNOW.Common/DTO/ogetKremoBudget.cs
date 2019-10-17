using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Kremo Budgetcalculation Result Object
    /// </summary>
    public class ogetKremoBudget
    {       
        /// <summary>
        /// Budgetüberschuß pro Monat
        /// </summary>
        public double budget { get; set; }

        /// <summary>
        /// Personenkennzahlen 1. AS
        /// </summary>
        public PkzDto pkz1 { get; set; }

        /// <summary>
        /// Personenkennzahlen 2. AS
        /// </summary>
        public PkzDto pkz2 { get; set; }

        /// <summary>
        /// Berechnungsfaktoren für jede Laufzeit fuer übergebenes Produkt
        /// </summary>
        public List<KremoLaufzeitFaktorDto> faktoren { get; set; }

    }
    public class KremoLaufzeitFaktorDto
    {
        /// <summary>
        /// Laufzeit
        /// </summary>
        public int laufzeit { get; set; }
        /// <summary>
        /// Berechnungsfaktor
        /// </summary>
        public double faktor { get; set; }
    }
}