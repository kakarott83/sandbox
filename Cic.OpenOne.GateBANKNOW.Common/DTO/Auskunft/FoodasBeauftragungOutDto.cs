using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Foodas return structure for Beauftragung WS
    /// </summary>
    public class FoodasBeauftragungOutDto : FoodasOutDto
    {
        public String auftragsnummer { get; set; }
    }
}
