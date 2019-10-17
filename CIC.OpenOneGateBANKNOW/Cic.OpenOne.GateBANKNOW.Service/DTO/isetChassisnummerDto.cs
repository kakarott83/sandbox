using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.changeAntragService.setChassisnummer"/> Methode
    /// </summary>
    public class isetChassisnummerDto
    {
        /// <summary>
        /// ID des Antrags
        /// </summary>
        public long sysID
        {
            get;
            set;
        }

        /// <summary>
        /// Zu setzende Chassisnummer
        /// </summary>
        public string chassisnummer
        {
            get;
            set;
        }
    }
}
