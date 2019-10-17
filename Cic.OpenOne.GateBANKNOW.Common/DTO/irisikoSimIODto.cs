using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für Risikoprüfung Simulation
    /// </summary>
    public class irisikoSimIODto
    {
        /// <summary>
        /// Antrag SYSID
        /// </summary>
        public long sysid { get; set; }


        /// <summary>
        /// Getter/Setter credit ammount
        /// </summary>
        public decimal? Finanzierungsbetrag { get; set; }
        /// <summary>
        /// Getter/Setter Security deposit
        /// </summary>
        public decimal? Kaution { get; set; }
        /// <summary>
        /// Getter/Setter Downpay first rate
        /// </summary>
        public decimal? Anzahlung_ErsteRate { get; set; }
        /// <summary>
        /// Getter/Setter contract period
        /// </summary>
        public decimal? Laufzeit { get; set; }

        /// <summary>
        /// Getter/Setter Interest rate
        /// </summary>
        public decimal? Zinssatz { get; set; }


        /// <summary>
        /// Getter/Setter terminal value
        /// </summary>
        public decimal? Restwert { get; set; }
        /// <summary>
        /// Getter/Setter Rate
        /// </summary>
        public decimal? Rate { get; set; }

        /// <summary>
        /// Getter/Setter terminal value Eurotax
        /// </summary>
        public decimal? Restwert_Eurotax { get; set; }

        /// <summary>
        /// Getter/Setter Terminal value BankNow
        /// </summary>
        public decimal? Restwert_Banknow { get; set; }

        /// <summary>
        /// Getter/Setter  Expected_Loss
        /// </summary>
        public decimal? Expected_Loss { get; set; }

        /// <summary>
        /// Getter/Setter  Expected_Loss_Prozent
        /// </summary>
        public decimal? Expected_Loss_Prozent { get; set; }

        /// <summary>
        /// Getter/Setter Expected_Loss_LGD
        /// </summary>
        public decimal? Expected_Loss_LGD { get; set; }

        /// <summary>
        /// Getter/Setter Profitabilitaet_Prozent
        /// </summary>   
        public decimal? Profitabilitaet_Prozent { get; set; }

    }
}
