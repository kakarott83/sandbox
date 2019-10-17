using Cic.OpenOne.Common.DTO.Prisma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class icheckKrRiskByIdDto
    {
		public long sysid {get;set;}
        public long sysPEROLE
        {
            get;
            set;
        }
        public prKontextDto kontext { get; set; }
    }
}
