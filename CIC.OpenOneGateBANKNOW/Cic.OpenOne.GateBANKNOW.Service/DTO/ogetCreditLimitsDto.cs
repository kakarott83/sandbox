using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// return value for getCreditLimits
    /// </summary>
    public class ogetCreditLimitsDto : oBaseDto
    {
        /// <summary>
        /// List of all Products where credit limit data is available
        /// </summary>
        public List<ProductCreditInfoDto> products { get; set; }
    }
    public class ProductCreditInfoDto
    {
        /// <summary>
        /// Product information
        /// </summary>
        public AvailableProduktDto product { get; set; }
        /// <summary>
        /// Credit Limit Information for all terms
        /// </summary>
        public List<ProductCreditLimitDto> creditLimits { get; set; }
    }
    public class ProductCreditLimitDto
    {
        /// <summary>
        /// Term
        /// </summary>
        public int laufzeit {get;set;}
        /// <summary>
        /// Credit Limit
        /// </summary>
        public double creditLimit {get;set;}
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
}
