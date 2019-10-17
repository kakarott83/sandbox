using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Schnitstelle für Buchwert BO
    /// </summary>
    public interface IBuchwertBo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputBw"></param>
        /// <returns></returns>
        ogetBuchwertDto getBuchwert(igetBuchwertDto inputBw);

        /// <summary>
        ///indikativen Buchwertberechnung erlaubt? //BRN9 CR 29 p.9
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        bool isBuchwertCalculationAllowed(long sysid);
        
    }
}
