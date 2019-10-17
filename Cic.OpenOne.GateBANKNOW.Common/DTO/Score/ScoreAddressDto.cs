using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
	/// <summary>
	/// ScoreAddress (laut Def. Score DataFields.xlsx 20170602)
	/// </summary>
	public class ScoreAddressDto
	{
		
		/// <summary>
		/// customerAddress_ID 
		/// </summary>
		public long AddressID { get; set; }

		/// <summary>
		/// AddressType
		/// neu ab V 2.8
		/// </summary>
		public string AddressType { get; set; }

		/// <summary>
		/// street
		/// </summary>
		public string Street { get; set; }
		
		/// <summary>
		/// postCode
		/// </summary>
		public string PostCode { get; set; }
		
		/// <summary>
		/// city
		/// </summary>
		public string City { get; set; }
		
		/// <summary>
		/// country
		/// </summary>
		public long Country { get; set; }
	}
}