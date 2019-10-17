using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.changeAntragService.setKontrollschild"/> Methode
    /// </summary>
    public class isetKontrollschildDto
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
        /// Zu setzende Kontrollschilde Nummer
        /// </summary>
        public string kontrollschild
        {
            get;
            set;
        }
    }
}
