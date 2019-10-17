using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Defines an interface to call a webservice from an eaihot
    /// </summary>
    public interface IEaiBOSAdapter
    {
        void processEaiHot(IEaihotDao dao, EaihotDto eai);
    }
}
