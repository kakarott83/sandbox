using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	/// <summary>
	/// ScoreArrangementDto (laut Def. Score DataFields.xlsx 20170606)
	/// </summary>
	public class ScoreArrangementDto
	{

		/// <summary>
		/// customerReference 
		/// </summary>
		public String CustomerReference { get; set; }

		/// <summary>
		/// contractReference
		/// </summary>
		public String ContractReference { get; set; }

		public bool ArrangementStatusFlag { get; set; }
		public bool ArrangementCancellationFlag { get; set; }
		public String ArrangementCancellationReason { get; set; }
		public DateTime ArrangementStartDate { get; set; }
		public DateTime ArrangementEndDate { get; set; }
		public bool PtPStatusFlag { get; set; }
		public bool PtPCancellationFlag { get; set; }
		public String PtPCancellationReason { get; set; }
		public DateTime PtPStartDate { get; set; }
		public DateTime PtPEndDate { get; set; }
		

	}
}