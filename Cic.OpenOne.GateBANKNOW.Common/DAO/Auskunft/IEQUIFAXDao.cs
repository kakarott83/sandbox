using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    public interface IEQUIFAXDao
    {
        EQUIFAXOutDto requestRiskData(AuskunftCFGDto config, EQUIFAXInDto input);
    }
}
