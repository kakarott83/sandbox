using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	/// <summary>
	/// ScoreArrangementDto (laut Def. Score DataFields.xlsx 20170606)
	/// </summary>
	public class ScoreArrangementDto
	{
		/// <summary>
		/// customerReference 
		/// </summary>
		public string CustomerReference { get; set; }

		/// <summary>
		/// contractReference
		/// </summary>
		public string ContractReference { get; set; }

		/// <summary>
		/// InvoiceReference
		/// </summary>
		public string InvoiceReference { get; set; }
		
		
		/// <summary>
		/// Attribut indicating whether an arrangement has been kept or broken. 
		/// Can also be used to indicate that an arrangement has just been activated. (Flag As String!)
		/// </summary>
		public string ArrangementStatusFlag { get; set; }

		///// <summary>
		///// Indicates whether Arrangement has been cancelled by user. (Flag As Int!)
		///// </summary>
		//public int ArrangementCancellationFlag { get; set; }

		/// <summary>
		/// Note about reason for cancelling the arrangement.
		/// </summary>
		public string ArrangementCancellationReason { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? ArrangementStartDate { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? ArrangementEndDate { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string ArrangementDetails { get; set; }

		/// <summary>
		/// Attribut indicating whether an arrangement has been kept or broken. 
		/// Can also be used to indicate that an arrangement has just been activated.
		/// </summary>
		public string PtPStatusFlag { get; set; }

		///// <summary>
		///// Indicates whether Arrangement has been cancelled by user.	(Flag As String!)
		///// </summary>
		//public int PtPCancellationFlag { get; set; }

		/// <summary>
		/// Note about reason for cancelling the arrangement.
		/// </summary>
		public string PtPCancellationReason { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? PtPStartDate { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? PtPEndDate { get; set; }

	}
}