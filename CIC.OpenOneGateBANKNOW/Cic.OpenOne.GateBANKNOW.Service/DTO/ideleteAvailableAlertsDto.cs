using System;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// ideleteAvailableAlertsDto
    /// </summary>
    public class ideleteAvailableAlertsDto
    {
        /// <summary>
        /// isoCode
        /// </summary>
        public String isoCode { get; set; }

        /// <summary>
        /// sysIds
        /// </summary>
        public long[] sysIds { get; set; }

        /// <summary>
        /// returnList
        /// </summary>
        public bool returnList { get; set; }
    }
}