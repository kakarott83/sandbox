using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für createAntrag Methode
    /// </summary>
    public class icreateAntragDto
    {
        /// <summary>
        /// Antrag
        /// </summary>
        public AntragDto antrag
        {
            get;
            set;
        }

        /// <summary>
        /// Must be set to reset the CRIF Status (e.g. when customer name changed)
        /// </summary>
        public int CRIFReset { get; set; }
    }
}
