using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	public class ScoreDDebitDto
	{
		/// <summary>
		/// ContractReference
		/// </summary>
		public string ContractReference { get; set; }
		/// <summary>
		/// DirectDebitID
		/// </summary>
		public long DirectDebitID { get; set; }

		// NOT USED (ab V 1.8)
		///// <summary>
		///// Direct Debit amount
		///// </summary>
		//public double Amount { get; set; }
		///// <summary>
		///// Direct Debit currency
		///// </summary>
		//public string Currency { get; set; }
		///// <summary>
		///// Direct Debit Type
		///// </summary>
		//public string DDType { get; set; }
		///// <summary>
		///// Direct Debit InvoiceReference
		///// </summary>
		//public string DDInvoiceReference { get; set; }
		///// <summary>
		///// Direct Debit Date 
		///// </summary>
		//public DateTime? DDDate { get; set; }
	}
}