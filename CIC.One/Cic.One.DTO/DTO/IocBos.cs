using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    /// <summary>
    /// Provides access to functionality from inside wf4
    /// this is the pointer in workflowcontext (the only variable available inside wf4)
    /// </summary>
    public interface IocBos
    {
        /// <summary>
        /// fetches the popup-definition from the named workflow queue, fills the gui-definition in WorkflowContext (as configured in the wfv)
        /// 
        /// </summary>
        /// <param name="wfv"></param>
        /// <param name="queue"></param>
        /// <param name="wctx"></param>
        void showPopup(String wfv, String queue, WorkflowContext wctx);
    }
}
