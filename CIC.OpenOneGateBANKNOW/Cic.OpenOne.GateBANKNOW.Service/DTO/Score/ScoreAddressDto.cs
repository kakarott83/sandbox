using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
	/// <summary>
	/// ScoreAddress (laut Def. Score DataFields.xlsx 20170602)
	/// </summary>
	public class ScoreAddressDto
	{
		
		/// <summary>
		/// customerAddress_ID 
		/// </summary>
		public long customerAddress_ID { get; set; }

		
		/// <summary>
		/// addressType
		/// </summary>
		public String addressType { get; set; }
		
		/// <summary>
		/// street
		/// </summary>
		public String street { get; set; }
		
		/// <summary>
		/// street_2ndLine
		/// </summary>
		public String street_2ndLine { get; set; }
		
		/// <summary>
		/// houseNo
		/// </summary>
		public String houseNo { get; set; }
		
		/// <summary>
		/// poBox
		/// </summary>
		public String poBox { get; set; }
		
		/// <summary>
		/// postCode
		/// </summary>
		public String postCode { get; set; }
		
		/// <summary>
		/// city
		/// </summary>
		public String city { get; set; }
		
		/// <summary>
		/// country
		/// </summary>
		public String country { get; set; }
		
		/// <summary>
		/// region
		/// </summary>
		public String region { get; set; }
	}
}