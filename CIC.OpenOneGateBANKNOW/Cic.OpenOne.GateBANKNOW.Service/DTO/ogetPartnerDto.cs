using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	public class ogetPartnerDto : oBaseDto
    {
		/// <summary>
		/// Liste Persistenzobjekte person
		/// </summary>
		public ScoreCustomerDto person	{ get; set; }
	}
}