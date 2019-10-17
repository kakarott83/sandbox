using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	public class igetPartnerDto
    {
		// ToDo rh: define executeRequest
		// public Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W002.executeRequest input { get; set; }

		///// <summary>
		///// strSysPerson Id As String?
		///// </summary>
		//public string strSysPerson { get; set; }
		
		/// <summary>
		/// sysPerson Id
		/// </summary>
		public long sysPerson { get; set; }
	}
}