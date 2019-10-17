using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.getPartnerZusatzdatenService.getKam"/> Methode
    /// </summary>
    public class ogetKamDto : oBaseDto
    {
        /// <summary>
        /// Name des Key Account Managers
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Vorname des Key Account Managers
        /// </summary>
        public string vorname
        {
            get;
            set;
        }

        /// <summary>
        /// Telefon Direktwahl des Key Account Managers
        /// </summary>
        public string telefon
        {
            get;
            set;
        }
    }
}
