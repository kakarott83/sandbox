using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of CREFO
    /// </summary>
    public class ogetCREFODetailDto : oBaseDto
    {
        public byte[] data { get;set;}
        public String kundenrating { get;set;}
        public String description1 { get; set; }
        public String description2 { get; set; }
    }
}