using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	public class ScorePostingDto
	{
		/// <summary>
		/// ContractReference
		/// </summary>
		public string ContractReference { get; set; }
		/// <summary>
		/// InvoiceID
		/// </summary>
		public long InvoiceID { get; set; }
		/// <summary>
		/// Posting amount
		/// </summary>
		public double Amount { get; set; }
		/// <summary>
		/// Posting currency
		/// </summary>
		public string Currency { get; set; }
		/// <summary>
		/// Posting Date 
		/// </summary>
		public DateTime? PostingDate { get; set; }
		/// <summary>
		/// Posting Type
		/// </summary>
		public string PostingCode { get; set; }
	}
}