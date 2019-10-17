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
    public class ologinEaiHotDto : oBaseDto
    {
        public long sysvlm { get;set;}
        public long syswfuser { get; set; }
        public String wfuserCode { get; set; }
        public String vlmCode { get; set; }
        public String execExpression { get; set; }
    }
}