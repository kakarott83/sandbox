using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public interface IDisclaimerBo
    {
        void createDisclaimer(String area, DisclaimerType dt, long sysid, long syswfuser, string inhalt);
    }
}
