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
	public class oLoggedInListDto : oBaseDto
    {
		public List<CicLogDto> loggedInUsers { get; set; }
    }
}