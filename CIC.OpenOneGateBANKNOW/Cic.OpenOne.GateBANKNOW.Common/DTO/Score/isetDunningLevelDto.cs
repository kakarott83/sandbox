using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	public class isetDunningLevelDto
	{
		// public Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W002.executeRequest input { get; set; }
		// DunningLevelDto
		/// <summary>
		/// Liste Persistenzobjekte dunningLevel
		/// </summary>
		public ScoreDunningLevelDto DunningLevel { get; set; }		
		
		///// <summary>
		///// customerReference 
		///// </summary>
		//public long customerReference { get; set; }

		///// <summary>
		///// contractReference
		///// </summary>
		//public long contractReference { get; set; }
	}
}