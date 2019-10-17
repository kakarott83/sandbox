using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Foodas Event return value data row
    /// </summary>
    public class FoodasEventOutDto : FoodasOutDto
    {
        public List<FoodasEventOutDataDto> events {get;set;}
    }
}
