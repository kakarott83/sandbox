using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// External app calculation data
    /// </summary>
    public class ExcalculationDto
    {

        public long Id { get; set; }
        public DateTime? Date { get; set; }
        public String Description { get; set; }
        public decimal Price { get; set; }
        public decimal OneOffPayment { get; set; }
        public int Period { get; set; }
        public decimal LastPayment { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal InterestRate { get; set; }
        public int CompanyId { get; set; }
        public int CustomerId { get; set; }
        public int DealerId { get; set; }
        public String ContractTypeName { get; set; }
        public String ObjectTypeName { get; set; }
        public int BaseContractType { get; set; }
        public int BaseObjectType { get; set; }
        public decimal Vat { get; set; }
        public int Status { get; set; }
        public String ContractTypeCode { get; set; }
        public String ObjectTypeCode { get; set; }
        public DateTime? ExportDate { get; set; }
        public int ObjectId { get; set; }
        public String ObjectCode { get; set; }
        public int FinanceModeType { get; set; }


    }
}
