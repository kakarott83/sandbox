using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{

    /// <summary>
    /// Enumeration of Disclaimer Areas
    /// </summary>
    public enum DisclaimerArea
    {
        /// <summary>
        /// It
        /// </summary>
        IT,

        /// <summary>
        /// Angebot
        /// </summary>
        ANGEBOT,

        /// <summary>
        /// Antrag
        /// </summary>
        ANTRAG,


        /// <summary>
        /// Vertrag
        /// </summary>
        VT
    }
    

    /// <summary>
    /// InputParameter für icreateOrUpdateDisclaimerDto Methode
    /// </summary>
    public class icreateDisclaimerDto
    {
        /// <summary>
        /// Area to attach disclaimer to
        /// </summary>
        public DisclaimerArea area { get; set; }

        /// <summary>
        /// Id for Area
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// Disclaimer-Text presented to the user (translated
        /// </summary>
        public String inhalt { get; set; }

        public DisclaimerType disclaimerType { get; set; }
    }
}
