using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Neupreis auslesen DTO
    /// </summary>
    public class ogetNeupreisDto:oMessagingDto
    {
        /// <summary>
        /// Neupreis
        /// </summary>
        public double Neupreis
        {
            get;
            set;
        }
    }
}