using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für saveKalkulation Methode
    /// </summary>
    public class isaveKalkulationDto
    {
        /// <summary>
        /// Persistenzobjekt Kalkulation
        /// </summary>
        public AngAntVarDto kalkualtion
        {
            get;
            set;
        }
    }
}
