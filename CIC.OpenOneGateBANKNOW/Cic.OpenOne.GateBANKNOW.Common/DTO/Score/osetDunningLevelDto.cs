using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	public class osetDunningLevelDto : oBaseDto
    {


		/// <summary>
		/// customerReference 
		/// </summary>
		public long CustomerReference { get; set; }

		/// <summary>
		/// contractReference
		/// </summary>
		public long ContractReference { get; set; }
		//// DunningLevelDto
		///// <summary>
		///// Liste Persistenzobjekte dunningLevel
		///// </summary>
		//public ScoreDunningLevelDto dunningLevel { get; set; }
    }
}