using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	/// <summary>
	/// WriteOff (noch KEINE Def.)
	/// </summary>
	public class ScoreWriteOffDto
	{
		/// <summary>
		/// contractReference
		/// </summary>
		public long ContractReference { get; set; }

		/// <summary>
		/// customerReference
		/// </summary>
		public long CustomerReference { get; set; }
	}
}