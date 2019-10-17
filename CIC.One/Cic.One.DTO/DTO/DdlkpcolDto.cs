using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class DdlkpcolDto : EntityDto
    {
         /*Primärschlüssel */
        public long sysddlkpcol { get; set; }
        /*Verweis zur Rubrik */
        public long sysddlkprub { get; set; }
        /*Aktiv */
        public int activeFlag { get; set; }
        /*Rang für Sortierung der Anzeige (eindeutig zur Rubrik) */
        public int rank { get; set; }
        /*Bezeichnung */
        public String name { get; set; }
        /*Code */
        public String code { get; set; }
        /*Optionbox (1=Eingabe, 2=Liste) */
        public int type { get; set; }
        /*Optionbox (1=Text, 2=Ganzzahl, 3=Betrag, 4=Prozent, 5=Datum) */
        public int format { get; set; }
        /*Beschreibbar */
        public int editable { get; set; }
        /*Defaultwert */
        public String defaultValue { get; set; }


        override public long getEntityId()
        {
            return sysddlkpcol;
        }

        public DdlkpcolDto()
        {
        }
        public DdlkpcolDto(DdlkpcolDto other)
        {
            this.sysddlkpcol = other.sysddlkpcol;
            this.sysddlkprub = other.sysddlkprub;
            this.activeFlag = other.activeFlag;
            this.rank = other.rank;
            this.name = other.name;
            this.code = other.code;
            this.type = other.type;
            this.format = other.format;
            this.editable = other.editable;
            this.defaultValue = other.defaultValue;
        }
        
        public List<DdlkpposDto> selectItems { get; set; }
        /** the value for the user */
        public DdlkpsposDto value { get; set; }
        /* the id of the selected selectItem */
       // public long sysddlkppos { get; set; }
        //public String value { get; set; }
    }
}