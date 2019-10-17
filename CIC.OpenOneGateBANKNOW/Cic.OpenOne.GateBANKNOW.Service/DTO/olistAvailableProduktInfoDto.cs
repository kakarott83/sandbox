using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Output for Method listAvailablePpoduktInfo containing products and its parameters
    /// </summary>
    public class olistAvailableProduktInfoDto : oBaseDto
    {
        /// <summary>
        /// Array von Produktinfos
        /// </summary>
        public AvailableProduktInfoDto[] produktinfos
        {
            get;
            set;
        }
    }
}
