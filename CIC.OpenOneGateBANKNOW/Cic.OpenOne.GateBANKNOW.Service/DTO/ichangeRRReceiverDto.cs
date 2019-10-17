using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für getBuchwertService.changeRRReceiver Methode
    /// </summary>
    public class ichangeRRReceiverDto
    {
        /// <summary>
        /// SysID des Antrags
        /// </summary>
        public long sysid { get; set; }
    }

}
