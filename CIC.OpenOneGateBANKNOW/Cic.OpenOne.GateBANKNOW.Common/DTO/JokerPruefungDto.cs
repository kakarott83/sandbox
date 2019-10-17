using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class JokerPruefungDto
    {
        public List<JokerProductMap> jokerproductsmap { set; get; }
        public List<AvailableProduktDto> products { set; get; }
    }
}
