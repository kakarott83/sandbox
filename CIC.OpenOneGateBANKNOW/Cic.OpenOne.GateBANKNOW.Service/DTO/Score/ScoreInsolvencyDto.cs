using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	/// <summary>
	/// ScoreInsolvency (laut Def. Score DataFields.xlsx 20170606)
	/// </summary>
	public class ScoreInsolvencyDto
	{

		/// <summary>
		/// InsolvencyReference
		/// Reference number insolvency
		/// Anm. rh: sollte eine number sein
		/// </summary>
		public String InsolvencyReference { get; set; }

		/// <summary>
		/// InsolvencyStartDate
		/// </summary>
		public DateTime InsolvencyStartDate { get; set; }

	}
}