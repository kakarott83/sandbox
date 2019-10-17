using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;


namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Interface for Clarion Eval Service
    /// </summary>
    public interface ICASBo
    {
        /// <summary>
        /// Validates CAS Connection
        /// </summary>
        /// <returns></returns>
        bool validateConnection();

        /// <summary>
        /// Evaluates the given clarion expressions
        /// </summary>
        /// <param name="eval"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        CASEvaluateResult[] getEvaluation(iCASEvaluateDto eval, long syswfuser);
    }
}