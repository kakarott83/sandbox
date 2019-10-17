using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	/// <summary>
	/// ScoreDunningLevelDto (laut Def. Score DataFields.xlsx 20170606)
	/// </summary>
	public class ScoreDunningLevelDto
	{
		/// <summary>
		/// customerReference 
		/// </summary>
		public String CustomerReference { get; set; }

		/// <summary>
		/// contractReference
		/// </summary>
		public String ContractReference { get; set; }

		public String InvoiceReference { get; set; }
		public long ContractStatusCollections { get; set; }
		public double DunningFee { get; set; }
		public long DunningActivityCounter { get; set; }
		public DateTime DunningActivityDate { get; set; }
		public bool DunningHoldFlag { get; set; }
		public DateTime DunningHoldEndDate { get; set; }
	}
}