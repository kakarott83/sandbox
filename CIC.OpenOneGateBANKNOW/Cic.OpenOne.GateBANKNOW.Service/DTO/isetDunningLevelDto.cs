using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	public class isetDunningLevelDto
	{
		// public Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W002.executeRequest input { get; set; }
		// DunningLevelDto
		/// <summary>
		/// Liste Persistenzobjekte dunningLevel
		/// </summary>
		public ScoreDunningLevelDto dunningLevel { get; set; }		
		
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