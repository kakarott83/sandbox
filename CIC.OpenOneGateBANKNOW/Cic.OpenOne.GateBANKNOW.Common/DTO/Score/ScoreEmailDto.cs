using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	/// <summary>
	/// ScoreEmail (laut Def. Score DataFields.xlsx 20170602)
	/// </summary>
	public class ScoreEmailDto
	{
		/// <summary>
		/// reference
		/// </summary>
		public long reference { get; set; }
		
		/// <summary>
		/// emailAddress
		/// </summary>
		public string emailAddress { get; set; }
		
		/// <summary>
		/// emailType
		/// </summary>
		public string emailType { get; set; }
		
		/// <summary>
		/// emailIsPreferred
		/// Flag indicating whether the e-mail address is the preferred one for sending communication.
		/// rh: changed from string to bool (20170609)
		/// </summary>
		public bool emailIsPreferred { get; set; }
		
		/// <summary>
		/// validFrom
		/// </summary>
		public DateTime? validFrom { get; set; }
		
		/// <summary>
		/// validTo
		/// </summary>
		public DateTime? validTo { get; set; }

	}
}