using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	public class igetContractDto
    {
		// public Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W002.executeRequest input { get; set; }

		/// <summary>
		/// customerReference 
		/// </summary>
		public long customerReference { get; set; }

		/// <summary>
		/// contractReference
		/// </summary>
		public long contractReference { get; set; }

	}
}