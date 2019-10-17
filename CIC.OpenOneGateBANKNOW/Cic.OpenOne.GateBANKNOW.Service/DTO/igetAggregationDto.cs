using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Aggregation external interface input structure
    /// </summary>
    public class igetAggregationDto
    {
        public Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W003.executeRequest req { get; set; }
    }
}