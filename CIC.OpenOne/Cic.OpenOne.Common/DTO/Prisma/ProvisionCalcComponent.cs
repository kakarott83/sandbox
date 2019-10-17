using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Provision Kalkulationskomponente
    /// </summary>
    public class ProvisionCalcComponent
    {
        /// <summary>
        /// Kalkulations komponenten Typ ID
        /// </summary>
        public ProvisionCalcComponentType type { get; set; }
        /// <summary>
        /// Wert der Kalkulationskomponente
        /// </summary>
        public double value { get; set; }
    }

    /// <summary>
    /// Defines different types of additional input-values needed for a provision calculation
    /// </summary>
    public enum ProvisionCalcComponentType
    {

    }
}
