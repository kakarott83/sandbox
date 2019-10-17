using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.Service.Services.DdOl.DTO
{
    public class VsTypPosDto
    {
        public long sysVSTypPos { get; set; }
        public long sysVSTyp { get; set; }
        public int Rang { get; set; }
        public String Bezeichnung { get; set; }
        public String Beschreibung { get; set; }
        public String Codemethod { get; set; }
        public int Basis { get; set; }
        public int Dimension1 { get; set; }
        public int Dimension2 { get; set; }
        public decimal AnteilLS { get; set; }
        public decimal AnteilVS { get; set; }
        public long SYSKORRTYP1 { get; set; }
        public long SYSKORRTYP2 { get; set; }
        public long sysVG1 { get; set; }
        public long sysQuote1 { get; set; }
        public long sysSteuer { get; set; }
        public int Activeflag { get; set; }

    }
}