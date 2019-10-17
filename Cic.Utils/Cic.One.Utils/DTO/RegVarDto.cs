using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.Common.DTO
{
    public class RegVarDto: EntityDto
    {
        public RegVarDto() { }
        public RegVarDto(RegVarDto dto)
        {
            this.sysRegVar = dto.sysRegVar;
            this.area = dto.area;
            this.bezeichnung = dto.bezeichnung;
            this.blobWert = dto.blobWert;
            this.chgdate = dto.chgdate;
            this.code = dto.code;
            this.path = dto.path;
            this.sysid = dto.sysid;
            this.sysRegSec = dto.sysRegSec;
            this.sysRegVar = dto.sysRegVar;
            this.syswfuser = dto.syswfuser;
            this.wert = dto.wert;
            this.wertTyp = dto.wertTyp;

        }
        
        /*Primärschlüssel */
        public long sysRegVar { get; set; }
        /*Reg */
        public long sysRegSec { get; set; }
        /*code */
        public string code { get; set; }
        /*Wert */
        public string wert { get; set; }
        /*bezeichnung */
        public string bezeichnung { get; set; }
        /*wertTyp */
        public int wertTyp { get; set; }
        /*Verweis zum blobWert */
        public string blobWert { get; set; }
        public string area { get; set; }

        /*path */
        public string completePath { get; set; }


        //dont transfer to client
        
        public string path { get; set; }

        public long sysid { get; set; }
        public long syswfuser { get; set; }
        public DateTime chgdate {get;set;}

        override public long getEntityId()
        {
            return sysRegVar;
        }
    }
}