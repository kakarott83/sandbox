using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.getPartnerZusatzdatenService.listAvailableNews"/> Methode
    /// </summary>
    public class olistAvailableNewsDto : oBaseDto
    {
        /// <summary>
        /// Array von News
        /// </summary>
        public AvailableNewsDto[] news
        {
            get;
            set;
        }
    }
}
