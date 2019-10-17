using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// NKK Tilgungsplan
    /// </summary>
    public class NkkabschlDto : EntityDto
    {

        /// <summary>
        /// Sys ID
        /// </summary>
        public long sysnkkabschl { get; set; }

        override public long getEntityId()
        {
            return sysnkkabschl;
        }

        override public String getEntityBezeichnung()
        {
            return text;
        }


        public long sysnkk { get; set; }
        public DateTime? von { get; set; }
        public DateTime? bis { get; set; }
        public double vsaldo { get; set; }
        public int vsollhaben { get; set; }
        public double saldo { get; set; }
        public int sollhaben { get; set; }
        public double saldomin { get; set; }
        public double saldomax { get; set; }
        public double saldoavg { get; set; }
        public int storno { get; set; }
        public DateTime? stornoam { get; set; }
        public int flagendabr { get; set; }
        public long sysrun { get; set; }

        //von nkkabpos
        public double intrate { get; set; }
        public double betrag { get; set; }
        public String text { get; set; }

    }
}