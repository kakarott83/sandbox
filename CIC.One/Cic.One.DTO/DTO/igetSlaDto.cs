using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.One.DTO
{
   /// <summary>
   /// LA details
   /// </summary>
    public class igetSlaDto
    {
		///// isoLangCode of Ang/Ant-User
		public String isoCode { get; set; }

		/// <summary>
		/// ID of Ang/Ant
		/// </summary>
		public long sysid
		{
			get;
			set;
		}
		
		///// <summary>
		///// sysctlang of Ang/Ant
		///// </summary>
		//public long sysctlang
		//{
		//	get;
		//	set;
		//}
	}
}
