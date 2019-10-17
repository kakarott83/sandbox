using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.GateWKT.DTO
{
    /// <summary>
    /// Input for calculating wartung reparatur, maintenance, fuelprice
    /// </summary>
    public class icalcServicesDto
    {
        public long sysangob { get; set; }
        public long sysobtyp { get; set; }
        public int lz { get; set; }
        public long ll { get; set; }
        public double avgConsumption { get; set; }
        public int fuelCode { get; set; }
        public double wrAufschlag { get; set; }
        public bool wrfix { get; set; }
    }
}