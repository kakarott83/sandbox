using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
	public class iLoggedInLogPropertiesDto
    {
		/// <summary>
		/// sessionTimeoutMin
		/// </summary>
		public int sessionTimeoutMin { get; set; }

		/// <summary>
		/// appId
		/// </summary>
		public long? appId { get; set; }

		/// <summary>
		/// hostID
		/// </summary>
		public string hostID { get; set; }

		/// <summary>
		/// userSource
		/// </summary>
		public string userSource { get; set; }

	}
}