using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateOEM.Service.DTO
{
    /// <summary>
    /// Saldo Information Structure
    /// </summary>
    public class SaldoInfo
    {
        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime createDate { get; set; }
        /// <summary>
        /// Date of last change
        /// </summary>
        public DateTime changeDate { get; set; }
        /// <summary>
        /// Saldo soll
        /// </summary>
        public decimal saldo_debit { get; set; }
        /// <summary>
        /// Saldo Ist
        /// </summary>
        public decimal saldo_current { get; set; }
        /// <summary>
        /// Dealer Code, 10 digits
        /// </summary>
        public String dealerNr { get; set; }
        /// <summary>
        /// Vehicle information VIN
        /// </summary>
        public String fznr { get; set; }
        /// <summary>
        /// Vehicle KFZ Briefnr 
        /// </summary>
        public String brief { get; set; }
        /// <summary>
        /// Invoice Date
        /// </summary>
        public DateTime invoiceDate { get; set; }
        /// <summary>
        /// Invoice Number
        /// </summary>
        public String invoiceNr { get; set; }
        /// <summary>
        /// Invoice Amount
        /// </summary>
        public decimal invoiceAmount { get; set; }
        /// <summary>
        /// FFS Condition keys
        /// </summary>
        public String conditionKey { get; set; }

    }

}
