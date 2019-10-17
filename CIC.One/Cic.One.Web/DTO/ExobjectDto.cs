using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// External app object data
    /// </summary>
    public class ExobjectDto
    {
        public long Id { get; set; }
        public decimal FirstPayment { get; set; }
        public decimal LastPayment { get; set; }
        public int NumberOfMonths { get; set; }
        public decimal InterestRate { get; set; }
        public int PriceTaxMode { get; set; }
        public String Description { get; set; }
        public int IsOrderOnly { get; set; }
        public int ContractTypeId { get; set; }
        public int ObjectTypeId { get; set; }
        public decimal Price { get; set; }
        public decimal MonthlyPayment { get; set; }
        public String Thumbnail { get; set; }
        public int ResidualValueChartId { get; set; }
        public int Quantity { get; set; }
        public int Wear { get; set; }
        public decimal Vat { get; set; }
        public String Name { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public int IsPublished { get; set; }
        public int MarginChartId { get; set; }
        public int ProvisionChartId { get; set; }
        public int InterestRateChartId { get; set; }
        public int PriceCanBeEditedInClient { get; set; }
        public decimal Margin { get; set; }
        public decimal Provision { get; set; }
        public String Code { get; set; }
        public int Readonly { get; set; }
    }
}
