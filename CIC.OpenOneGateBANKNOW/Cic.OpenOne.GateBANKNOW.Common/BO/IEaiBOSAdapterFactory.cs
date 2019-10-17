using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Interface for an adapter to adapt bo-methods to eai-calls with code CALL_BOS
    /// Every implementing class must be manually registered in EaihotBo!
    /// 
    /// </summary>
    public interface IEaiBOSAdapterFactory
    {
        IEaiBOSAdapter getEaiBOSAdapter(String method);
    }
}
