using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// BMW AIDA2 InEquityForecast Data Object
    /// </summary>
    public class IefDto : EntityDto
    {
        public long sysVt { get; set; }
        public String vertrag { get; set; }
        public String vkName { get; set; }
        public String hdName { get; set; }
        public String hdOrt { get; set; }
        public String kdName { get; set; }

        /// <summary>
        /// In Equity Differenz Prozent
        /// </summary>
        public double iediffp { get; set; }
        public DateTime? ende { get; set; }
        public DateTime? erstzul { get; set; }
        public String vart { get; set; }
        public String marke { get; set; }
        public String modell { get; set; }
        public String konstellation { get; set; }
        public String serie { get; set; }

        public double sz { get; set; }
        public double depot { get; set; }
        public double rate { get; set; }
        public double rw { get; set; }
        public double eurotaxblau { get; set; }
        public double aufloesewert { get; set; }
        public double aufloesewert1 { get; set; }
        public double aufloesewert2 { get; set; }
        /// <summary>
        /// In Equity Differenz
        /// </summary>
        public double iediff { get; set; }
        public double bonusff { get; set; }

        override public long getEntityId()
        {
            return sysVt;
        }
        public override string getEntityBezeichnung()
        {
            return vertrag;
        }
    }
}
