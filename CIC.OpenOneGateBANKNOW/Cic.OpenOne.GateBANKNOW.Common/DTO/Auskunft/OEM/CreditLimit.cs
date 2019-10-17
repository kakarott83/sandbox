using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateOEM.Service.DTO
{

    /// <summary>
    /// Dealer Credit Limit Information Structure
    /// </summary>
    public class CreditLimit
    {
        /// <summary>
        /// Dealer Code, 10 digits
        /// </summary>
        public String dealerNr { get; set; }
        /// <summary>
        /// Date
        /// </summary>
        public DateTime perDate { get; set; }
        /// <summary>
        /// SysPerson Dealer, optional
        /// </summary>
        public String dealerId { get; set; }
        /// <summary>
        /// Dealer Code, 10 digits
        /// </summary>
        public String dealerCode { get; set; }
        /// <summary>
        /// Dealer Name
        /// </summary>
        public String dealerName { get; set; }
        /// <summary>
        /// Credit Limit
        /// </summary>
        public decimal limit { get; set; }
        /// <summary>
        /// Operating amount
        /// </summary>
        public decimal? usage { get; set; }
        /// <summary>
        /// Available amount
        /// </summary>
        public decimal? saldo { get; set; }

        /// <summary>
        /// kind of vehicle
        /// 10=PKW
        /// 30=LCV (3.5t H350)
        /// </summary>
        public int divisionCode { get; set; }

        /// <summary>
        /// distribution chanel
        /// 10=New Cars
        /// 30=Company Cars
        /// </summary>
        public int distributionChannel { get; set; }
    }
}
