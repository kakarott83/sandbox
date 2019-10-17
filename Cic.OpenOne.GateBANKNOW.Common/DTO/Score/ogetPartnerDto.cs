using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	public class ogetPartnerDto : oBaseDto
    {
		/// <summary>
		/// Persistenzobjekt Person
		/// </summary>
		public ScoreCustomerDto Person	{ get; set; }
	}
}