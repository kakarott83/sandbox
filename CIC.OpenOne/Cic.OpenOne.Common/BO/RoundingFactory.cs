using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// RundingsFactory 
    /// </summary>
    public class RoundingFactory
    {
        /// <summary>
        /// Rundung erzeugen
        /// </summary>
        /// <returns>Rundungsklasse</returns>
        public static IRounding createRounding()
        {
            return new Rounding();
        }
        
    }
}
