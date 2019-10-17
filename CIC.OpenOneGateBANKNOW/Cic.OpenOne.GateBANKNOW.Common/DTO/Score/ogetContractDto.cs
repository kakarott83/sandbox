using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	public class ogetContractDto : oBaseDto
    {
		/// <summary>
		/// Liste Persistenzobjekte contract
		/// </summary>
		public ScoreContractDto Contract { get; set; }

    }
}