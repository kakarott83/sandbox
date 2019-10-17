using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.changeAntragService.setAblieferdatum"/> Methode
    /// </summary>
    public class isetAblieferdatumDto
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
        /// Zu setzendes Ablieferdatum (Auslieferungsdatum)
        /// </summary>
        public DateTime ablieferdatum
        {
            get;
            set;
        }
    }
}
