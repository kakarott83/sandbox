using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.Common.DTO
{
    public class RegDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysReg { get; set; }
        /*code */
        public string code { get; set; }
        /*bezeichnung */
        public string bezeichnung { get; set; }
        /*Verweis zum Wfuser */
        public long sysWfuser { get; set; }
        /*Wert */
        public string wert { get; set; }

        override public long getEntityId()
        {
            return sysReg;
        }
    }
}