using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    public class IncentivierungStatusDto
    {
        public double Finanzierungsvolumen { get; set; }
        public int AnzahlVertraege { get; set; }

        public double BonusTotal { get; set; }
        public double BonusWaitingForPayOut { get; set; }
    }
}