using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	public class osetArrangementDto : oBaseDto
    {
		//// ArrangementDto
		///// <summary>
		///// Liste Persistenzobjekte arrangement
		///// </summary>
		//public ScoreArrangementDto arrangement { get; set; }
		
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