using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	/// <summary>
	/// ScorePhone (laut Def. Score DataFields.xlsx 20170606)
	/// </summary>
	public class ScorePhoneDto
	{
		/// <summary>
		/// phoneReference (eig. person:sysperson)
		/// </summary>
		public long PhoneReference { get; set; }
		
		/// <summary>
		/// phoneType
		/// </summary>
		public string PhoneType { get; set; }
		
		/// <summary>
		/// phoneNumber (OL: person.ptelefon/telefon/handy)
		/// OL: if Person.telefon > '', sonst if Person.handy > '', sonst Person.petelefon
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Phone_Valid_From
		/// neu ab V 2.8
		/// </summary>
		public DateTime? Phone_Valid_From { get; set; }

		/// <summary>
		/// Phone_Valid_To
		/// neu ab V 2.8
		/// </summary>
		public DateTime? Phone_Valid_To { get; set; }

	}
}