using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{    
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchAntragService.getAntragDetail"/> Methode
    /// </summary>
    public class igetAntragDetailDto
    {
        /// <summary>
        /// Antrags Id
        /// </summary>
        public long sysantrag
        {
            get;
            set;
        }

        /// <summary>
        /// Clienttyp (10=B2B, 20=MA, 30=B2C, 50=ONE)
        /// </summary>
        public int? wsclient 
        { 
            get; 
            set; 
        }
    }
}
