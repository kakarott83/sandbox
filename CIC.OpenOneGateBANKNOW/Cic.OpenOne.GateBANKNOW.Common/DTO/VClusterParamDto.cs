using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class VClusterParamDto
    {
        public string v_cluster;
        public ParamDto v_el_betrag;
        public ParamDto v_el_prozent;
        public ParamDto v_prof;
    }
}
