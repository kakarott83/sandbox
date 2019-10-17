using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	public class oset2ndDDDto : oBaseDto
    {
		///// <summary>
		///// Liste Persistenzobjekte 2nd Direct Debit
		///// </summary>
		//public ScoreDDebitDto ddebit { get; set; }

		// public Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W002.executeRequest input { get; set; }
		/// <summary>
		/// contractRefernce
		/// </summary>
		public long ContractReference { get; set; }

	}
}