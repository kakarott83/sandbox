using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	public class ScoreDDebitDto
	{
		/// <summary>
		/// ContractReference
		/// </summary>
		public long contractReference { get; set; }
		/// <summary>
		/// DirectDebitID
		/// </summary>
		public long DirectDebitID { get; set; }
		/// <summary>
		/// Direct Debit amount
		/// </summary>
		public double amount { get; set; }
		/// <summary>
		/// Direct Debit currency
		/// </summary>
		public String currency { get; set; }
		/// <summary>
		/// Direct Debit Type
		/// </summary>
		public String ddType { get; set; }
		/// <summary>
		/// Direct Debit InvoiceReference
		/// </summary>
		public String ddInvoiceReference { get; set; }
		/// <summary>
		/// Direct Debit Date 
		/// </summary>
		public DateTime ddDate { get; set; }
	}
}