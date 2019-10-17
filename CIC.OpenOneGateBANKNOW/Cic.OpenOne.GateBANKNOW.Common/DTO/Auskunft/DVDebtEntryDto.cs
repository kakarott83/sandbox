using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// DV Debt Entry Data Transfer Objects
    /// </summary>
    public class DVDebtEntryDto
    {
        /// <summary>
        /// Getter/Setter Risc Class
        /// </summary>
        public int RiskClass { get; set; }

        /// <summary>
        /// Getter/Setter Date of Opening
        /// </summary>
        public string dateOpen { get; set; }

        /// <summary>
        /// Getter/Setter Date of Closing
        /// </summary>
        public string dateClose { get; set; }

        /// <summary>
        /// Getter/Setter Ammount
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Getter/Setter Ammount at opening
        /// </summary>
        public int AmountOpen { get; set; }

        /// <summary>
        /// Getter/Setter Type of Debt
        /// </summary>
        public int DebtType { get; set; }

        /// <summary>
        /// Getter/Setter Payment state
        /// 1=in Bearbeitung, 2=ausgebucht, 3=bezahlt, 4=unbekannt
        /// </summary>
        public int PaymentStatus { get; set; }

        /// <summary>
        /// Getter/Setter Origin
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Getter/Setter Text
        /// </summary>
        public string Text { get; set; }
    }
}
