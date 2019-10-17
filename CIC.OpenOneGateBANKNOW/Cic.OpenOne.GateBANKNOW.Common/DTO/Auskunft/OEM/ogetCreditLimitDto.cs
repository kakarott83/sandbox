using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateOEM.Service.DTO
{
    /// <summary>
    /// Holds all credit limit informations
    /// </summary>
    public class ogetCreditLimitDto:oBaseDto
    {
        public List<CreditLimit> creditLimits { get; set; }
    }
}
