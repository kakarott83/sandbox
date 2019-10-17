using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für deleteAntrag Methode
    /// </summary>
    public class ideleteAntragDto
    {
        /// <summary>
        /// ID des Antrags
        /// </summary>
        public long sysID
        {
            get;
            set;
        }
    }
}
