using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Restwert DTO Ausgang
    /// </summary>
    public class ogetRestwertDto : oMessagingDto
    {
        /// <summary>
        /// Restwert
        /// </summary>
        public double Restwert
        {
            get;
            set;
        }
    }
}