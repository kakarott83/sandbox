using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// ZustandDto
    /// </summary>
    public class ZustandDto
    {
        /// <summary>
        /// Zustand (Status)
        /// </summary>
        public String zustand { get; set; }
        /// <summary>
        /// Zustandsübergang am
        /// </summary>
        public DateTime zustandAm { get; set; }
    }
}
