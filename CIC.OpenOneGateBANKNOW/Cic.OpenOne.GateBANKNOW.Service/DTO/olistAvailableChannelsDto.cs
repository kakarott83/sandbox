using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.getPartnerZusatzdatenService.listAvailableChannels"/> Methode
    /// </summary>
    public class olistAvailableChannelsDto : oBaseDto
    {
        /// <summary>
        /// Array von Channels
        /// </summary>
        public DropListDto[] channels
        {
            get;
            set;
        }
    }
}
