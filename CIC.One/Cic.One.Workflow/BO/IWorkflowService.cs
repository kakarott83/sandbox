using Cic.One.DTO;
using Cic.One.Workflow.DAO;
using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.Workflow.BO
{
    public interface IWorkflowService
    {
        /// <summary>
        /// Processes a workflow until
        ///  - finished
        ///  - or until bookmark reached (by UserIntarction-Activity)
        ///  
        /// Uses the given input context and returns the modified context
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        oprocessWorkflowDto processWorkflow(iprocessWorkflowDto input, oprocessWorkflowDto rval, Cic.One.DTO.IWFVDao dao, Cic.OpenOne.Common.BO.ICASBo casBo);


        /// <summary>
        /// evaluates the expression for the given area and areaid
        /// supports the following languages with the given prefix:
        /// c#:   = csharp
        /// vb:   = visual basic
        /// cw:   = clarion
        ///       = return the expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        String evaluate(String expression, String area, long sysId, ref WorkflowContext ctx);

        /// <summary>
        /// Evaluates the input via CAS
        /// returns the first Result as String
        /// puts all Results into the WorkflowContext
        /// </summary>
        /// <param name="iEval"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        String evaluateCAS(iCASEvaluateDto iEval, ref WorkflowContext ctx);

        /// <summary>
        /// Evaluates all given expressions 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        String[] evaluate(iEvalExpressionDto input);

    }
}
