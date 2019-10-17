using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	public class ScorePostingDto
	{
		/// <summary>
		/// ContractReference
		/// </summary>
		public long contractReference { get; set; }
		/// <summary>
		/// Posting amount
		/// </summary>
		public double amount { get; set; }
		/// <summary>
		/// Posting currency
		/// </summary>
		public String currency { get; set; }
		/// <summary>
		/// Posting Date 
		/// </summary>
		public DateTime postingDate { get; set; }
		/// <summary>
		/// Posting Type
		/// </summary>
		public String postingCode { get; set; }
	}
}