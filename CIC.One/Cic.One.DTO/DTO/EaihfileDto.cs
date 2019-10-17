using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// EaihfileDto
    /// </summary>
    public class EaihfileDto : EntityDto 
    {

        public long syseaihfile { get; set; }
        public long syseaihot { get; set; }
        public String code { get; set; }
        public byte[] eaihfile { get; set; }
        public int? inputflag { get; set; }
        public int? readflag { get; set; }
        public String sourcefilename { get; set; }
        public String sourcepathspec { get; set; }
        public String targetfilename { get; set; }
        public String targetpathspec { get; set; }


        public override long getEntityId()
        {
            return syseaihfile;
        }
    }
}