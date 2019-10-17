using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Dto for Rahmen (RVT-Table) Entity
    /// </summary>
    public class RahmenDto:EntityDto
    {
        public long sysrvt { get; set; }
        public List<RvtPosDto> positionen { get; set; }

        public String rahmen { get; set; }
        public String beschreibung { get; set; }
        public int nettingflag { get; set; }
        public long sysperson { get; set; }
        public DateTime? beginn { get; set; }
        public DateTime? ende { get; set; }
        public int aktivkz { get; set; }
        public long sysls { get; set; }
        public double zinsaufab { get; set; }
        public String evalzinsaufab { get; set; }
        public long kmtoleranz { get; set; }
        public String evalkmtoleranz { get; set; }
        public long mikmbegrenz { get; set; }
        public String evalmikmbegrenz { get; set; }
        public double mehrkm { get; set; }
        public String evalmehrkm { get; set; }
        public double mehrkmp { get; set; }
        public String evalmehrkmp { get; set; }
        public double minderkm { get; set; }
        public String evalminderkm { get; set; }
        public double minderkmp { get; set; }
        public String evalminderkmp { get; set; }
        public long sysit { get; set; }
        public DateTime? creationdate { get; set; }
        public double penalty { get; set; }
        public int courseinstalm { get; set; }
        public long sysbn { get; set; }
        public int szamort { get; set; }
        public double riskvalue { get; set; }
        public int addcharge { get; set; }
        public long limitvalue { get; set; }
        public int pmtlimit { get; set; }
        public int pmttype { get; set; }
        public int increaserate { get; set; }
        public long sysvart { get; set; }
        public long syswaehrung { get; set; }
        public String zustand { get; set; }
        public DateTime? zustandam { get; set; }
        
        override public long getEntityId()
        {
            return sysrvt;
        }

        override public String getEntityBezeichnung()
        {
            return beschreibung;
        }

        /// <summary>
        /// Is used in frontend in BO, to determine whether its created via copy 
        /// </summary>
        public int isCopyFlag { get; set; }
    }
}