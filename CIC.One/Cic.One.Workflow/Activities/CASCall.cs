using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Tracking;
using Cic.One.DTO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;

namespace Cic.One.Workflow.Activities
{
    /// <summary>
    /// Activity that adds a Message-String in the workflow-contexts message list
    /// </summary>
    public sealed class CASCall<Tout> : NativeActivity<Tout> 
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [RequiredArgument]
        public InArgument<WorkflowContext> wfcontext { get; set; }
        [RequiredArgument]
        public InArgument<String> expression { get; set; }
        

        protected override void Execute(NativeActivityContext context)
        {
            try
            { 
                WorkflowContext ctx = wfcontext.Get(context);

                String casFunction = expression.Get(context);

                ICASBo bo = new CASBo();
                iCASEvaluateDto iEval = new iCASEvaluateDto();
                iEval.area = ctx.area;
                iEval.sysID = new long[1];
                iEval.sysID[0] = long.Parse((ctx.areaid == null ? "0": ctx.areaid));
                iEval.expression = new String[1];
                iEval.expression[0] = casFunction;
                //iEval.url//TODO optional via activity
                _log.Debug("Now starting CAS...");
            
                CASEvaluateResult[] result = bo.getEvaluation(iEval, ctx.sysWFUSER);
                if (result != null && result.Length > 0)
                {
                    String rval = result[0].clarionResult[0];
                    Result.Set(context, CASEvaluateResult.ChangeType(rval, typeof(Tout)));
                }
            }
            catch (Exception e)
            {
                _log.Error("CAS Failure", e);
            }

        }
       

       
    }
}
