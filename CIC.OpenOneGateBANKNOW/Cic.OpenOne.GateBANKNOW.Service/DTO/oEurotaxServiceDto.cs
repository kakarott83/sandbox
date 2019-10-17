using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Transferobjekt für OutputParameter für Cic.OpenOne.GateBANKNOW.Service.AuskunftService-Methoden
    /// </summary>
    public class oEurotaxServiceDto : oBaseDto
    {
        /// <summary>
        /// Return value from Eurotax Auskunft Services
        /// </summary>
        public bool Output
        {
            get;
            set;
        }
    }
}
