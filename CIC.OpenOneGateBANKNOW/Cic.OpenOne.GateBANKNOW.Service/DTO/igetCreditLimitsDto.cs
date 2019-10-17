using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für getCreditLimits Methode
    /// </summary>
    public class igetCreditLimitsDto
    {
        /// <summary>
        /// id for determining kremo-data
        /// </summary>
        public long sysantrag { get; set; }

        /// <summary>
        /// additional filter for vart
        /// </summary>
        public long sysvart { get; set; }

        /// <summary>
        /// Produktkontext
        /// </summary>
        public prKontextDto kontext
        {
            get;
            set;
        }
    }
}
