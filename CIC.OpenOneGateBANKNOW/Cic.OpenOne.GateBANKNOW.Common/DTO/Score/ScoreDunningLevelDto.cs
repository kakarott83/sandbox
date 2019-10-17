using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	/// <summary>
	/// ScoreDunningLevelDto (laut Def. Score DataFields.xlsx 20170606)
	/// </summary>
	public class ScoreDunningLevelDto
	{
		/// <summary>
		/// contractReference
		/// </summary>
		public string ContractReference { get; set; }

		/// <summary>
		/// Unique ID for each customer used as customer number in the BMW FS systems 
		/// </summary>
		public string CustomerReference { get; set; }

		/// <summary>
		/// InvoiceReference
		/// </summary>
		public string InvoiceReference { get; set; }

		/// <summary>
		/// Current Dunning Level of the contract
		/// </summary>
		public long DunningLevel { get; set; }

		///// <summary>
		///// Dunning Level of the Invoice
		///// </summary>
		//public long DunningLevelInvoice { get; set; }
		
		//// wird (eigentlich) NICHT an TM geliefert (V 1.6): 
		// Not in A: public long ContractStatusCollections { get; set; }
		
		// wird (eigentlich auch) NICHT an TM geliefert (V 1.6): 
		/// <summary>
		/// Fee assigned to a certain dunning activity
		/// </summary>
		public double DunningFee { get; set; }
		
		// Not in A: public long DunningActivityCounter { get; set; }

		/// <summary>
		/// Date of the just executed Dunning activity
		/// </summary>
		public DateTime? DunningActivityDate { get; set; }

		/// <summary>
		/// Indicates that a Dunning is in place (PRESUME: FLAG AS INT)
		/// </summary>
		public bool DunningHoldFlag { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		public DateTime? DunningHoldEndDate { get; set; }
		
		/// <summary>
		/// Date when the Dunning Episode has been completed in TM.
		/// </summary>
		public DateTime? DunningEpisodeEnd { get; set; }
		
	}
}