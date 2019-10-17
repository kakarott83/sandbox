using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.getPartnerZusatzdatenService.listAvailableAlerts"/> Methode
    /// </summary>
    public class olistAvailableAlertsDto : oBaseDto
    {
        /// <summary>
        /// Array von Alerts
        /// </summary>
        public AvailableAlertsDto[] alerts
        {
            get;
            set;
        }
    }
}
