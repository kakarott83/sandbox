using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	public class osetPostingDto : oBaseDto
    {
		///// <summary>
		///// Liste Persistenzobjekte Posting
		///// </summary>
		//public ScorePostingDto posting { get; set; }

		// public Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W002.executeRequest input { get; set; }
		/// <summary>
		/// contractReference 
		/// </summary>
		public string ContractReference { get; set; }


	}
}