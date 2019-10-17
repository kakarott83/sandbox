using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	/// <summary>
	/// ScorePhone (laut Def. Score DataFields.xlsx 20170606)
	/// </summary>
	public class ScorePhoneDto
	{
		/// <summary>
		/// phoneReference
		/// </summary>
		public long phoneReference { get; set; }
		
		/// <summary>
		/// phoneType
		/// </summary>
		public String phoneType { get; set; }
		
		/// <summary>
		/// phoneNumber
		/// </summary>
		public String phoneNumber { get; set; }
		
		/// <summary>
		/// validFrom
		/// </summary>
		public DateTime validFrom { get; set; }
		
		/// <summary>
		/// validTo
		/// </summary>
		public DateTime validTo { get; set; }

	}
}