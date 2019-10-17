using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.Common.DTO
{
    public class RegSecDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysRegSec { get; set; }
        /*Reg */
        public long sysReg { get; set; }
        /*code */
        public string code { get; set; }
        /*Wert */
        public string wert { get; set; }
        /*bezeichnung */
        public string bezeichnung { get; set; }
        /*codeRegTyp */
        public string codeRegTyp { get; set; }
        /*Verweis zum RegSecParente */
        public long sysRegSecParente { get; set; }
        

        override public long getEntityId()
        {
            return sysRegSec;
        }
    }
}