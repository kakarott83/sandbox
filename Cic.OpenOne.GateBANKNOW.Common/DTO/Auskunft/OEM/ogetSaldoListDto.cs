using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateOEM.Service.DTO
{
    /// <summary>
    /// Holds all saldo informations
    /// </summary>
    public class ogetSaldoListDto:oBaseDto
    {
        public List<SaldoInfo> salden { get; set; }
    }
}
