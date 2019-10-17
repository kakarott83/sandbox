using System;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Keeps information about Eigenablöse Vorvertrag
    /// </summary>
    public class EigenAblInfo
    {
        /// <summary>
        /// VERSSUMME
        /// </summary>
        public double VERSSUMME { get; set; }

        /// <summary>
        /// VARTCODE
        /// </summary>
        public String VARTCODE { get; set; }

        /// <summary>
        /// returns the PPI Ablösefaktor
        /// 1 if VART == KREDIT_CLASSIC and VERSSUMME>0
        /// 0 otherwise
        /// </summary>
        /// <returns></returns>
        public double getPPI()
        {
            if (VERSSUMME > 0 && "KREDIT_CLASSIC".Equals(VARTCODE)) return 1.0;
            return 0;
        }
    }
}