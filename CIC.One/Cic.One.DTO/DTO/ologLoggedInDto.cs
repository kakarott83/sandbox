using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// 
    /// </summary>
	public class ologLoggedInDto : oBaseDto
    {
        public long syswfuser { get; set; }
		public long sysperole { get; set; }
		public String wfuserCode { get; set; }

    }
}