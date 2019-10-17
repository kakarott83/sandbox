using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
	public class ilogLoggedInDto
    {
        /// <summary>
		/// sysperole
        /// </summary>
		public long sysperole
        {
            get;
            set;
        }

		/// <summary>
		/// timeOutMin (in Minutes)
		/// </summary>
		public int timeOutMin
		{
			get;
			set;
		}

		/// <summary>
		/// appId 
		/// </summary>
		public long appId
		{
			get;
			set;
		}
		
		/// <summary>
		/// hostID
		/// </summary>
		public string hostID
		{
			get;
			set;
		}
		/// <summary>
		/// userSource
		/// </summary>
		public string userSource
		{
			get;
			set;
		}

        /// <summary>
		/// wfuserCode
        /// </summary>
		public string wfuserCode
        {
            get;
            set;
        }

    }
}