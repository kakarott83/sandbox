using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ObtypDto : TreeDto
    {
        public long sysobtyp{get;set;}
        public String fzart { get; set; }
        public String bezeichnung { get; set; }
        public String marke { get; set; }
        public String modell { get; set; }
        public String schwacke { get; set; }
        public String beschreibung { get; set; }
        public String typ { get; set; }
        public String typengenehmigung { get; set; }
        public String hsn { get; set; }
        public String tsn { get; set; }
        public String kommnr { get; set; }
        public double neupreisbrutto { get; set; }
        public double neupreisnetto { get; set; }
        public double leistung { get; set; }
        public long baujahr { get; set; }
        public long baubisjahr { get; set; }
        public int baumonat { get; set; }
        public int bgn { get; set; }

        public String bezeichnung2 { get; set; }
        public String bezeichnung3 { get; set; }
        public override long getEntityId()
        {
            return sysobtyp;
        }
    }
}
