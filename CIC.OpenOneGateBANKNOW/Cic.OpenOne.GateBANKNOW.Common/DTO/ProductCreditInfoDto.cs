using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class ProductCreditInfoDto
    {
        public AvailableProduktDto product { get; set; }
        public List<ProductCreditLimitDto> creditLimits { get; set; }
    }

    public class ProductCreditLimitDto
    {
        /// <summary>
        /// Term
        /// </summary>
        public int laufzeit { get; set; }
        /// <summary>
        /// Credit Limit
        /// </summary>
        public double creditLimit { get; set; }
        /// <summary>
        /// monatliche Rate
        /// </summary>
        public double rate { get; set; }
        /// <summary>
        /// Provisionsbetrag
        /// </summary>
        public double provision { get; set; }
        /// <summary>
        /// Provisions-Prozentsatz
        /// </summary>
        public double provisionp { get; set; }
        /// <summary>
        /// Flag Ratenabsicherung ja/nein
        /// </summary>
        public int ratenAbsicherung { get; set; }
        /// <summary>
        /// Status OK / NOK
        /// </summary>
        public String status { get; set; }
    }
    /// <summary>
    /// Used for fetching productcreditlimitdto from db
    /// </summary>
    public class ProductCreditLimitFetchDto : ProductCreditLimitDto
    {
        public long sysprproduct { get; set; }
    }
}
