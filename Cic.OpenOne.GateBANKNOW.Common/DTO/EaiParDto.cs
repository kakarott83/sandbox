using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// EAIPAR Dto
    /// </summary>
    public class EaiparDto
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public long SYSEAIPAR { get; set; }

        /// <summary>
        /// CODE
        /// </summary>
        public String CODE { get; set; }

        /// <summary>
        /// DESCRIPTION
        /// </summary>
        public String DESCRIPTION { get; set; }

        /// <summary>
        /// PARAMFILE
        /// </summary>
        public String PARAMFILE { get; set; }

    }
}
