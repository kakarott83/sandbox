using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für getDealerID Methode
    /// </summary>
    public class ogetDealerDto : oBaseDto
    {
        /// <summary>
        /// perole
        /// </summary>
        public long sysPEROLE { get; set; }
    }
}