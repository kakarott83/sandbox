using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für processAntragToVertrag Methode
    /// </summary>
    public class oprocessAntragToVertragDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Vertrag
        /// </summary>
        public VertragDto vertrag
        {
            get;
            set;
        }
    }
}
