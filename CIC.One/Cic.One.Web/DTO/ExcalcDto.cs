using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// External app create or update interface
    /// </summary>
    public class ExcalcDto
    {
        /// <summary>
        /// customer for the offer
        /// </summary>
        public ExcustomerDto customer { get; set; }
        /// <summary>
        /// calculation data for the offer
        /// </summary>
        public ExcalculationDto calculation { get; set; }
        /// <summary>
        /// object data for the offer
        /// </summary>
        public ExobjectDto obj { get; set; }
        
    }
}
