using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.One.DTO
{
    /// <summary>
    /// Output for calculation of all rate prices
    /// </summary>
    public class ocalcRateDto : oBaseDto
    {
        public double rate { get; set; }//rate netto
        public double rw { get; set; }//kalk rw gesamt, basis für ratenberechnung
        public double rwP { get; set; }//kalk rw Prozentsatz gesamt, basis für ratenberechnung
        public double novabonusmalus { get; set; }
        public double novasonderminderung { get; set; }
        public double novasteuervorteil { get; set; }
        public double nova { get; set; }
        public double rgg { get; set; }
        public double sonderAustP { get; set; }
        public double crvP { get; set; }
        public double sfbaseP { get; set; }
        public double crv { get; set; }
        public double sfbase { get; set; }
        public double zins { get; set; } //the zins used for calculation
    }
}