using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Transferobjekt für OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.AuskunftService.EurotaxGetValuation"/> Methode
    /// </summary>
    [System.CLSCompliant(false)]
    public class oEurotaxGetValuationDto : oBaseDto
    {
        /// <summary>
        /// Transferobjekt EurotaxGetValuation
        /// </summary>
        public EurotaxGetValuationDto EurotaxGetValuationDto
        {
            get;
            set;
        }
    }
}
