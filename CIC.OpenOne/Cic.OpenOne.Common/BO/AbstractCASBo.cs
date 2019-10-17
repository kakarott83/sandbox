using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    public abstract class AbstractCASBo : ICASBo
    {
        /// <summary>
        /// Validates CAS Connection
        /// </summary>
        /// <returns></returns>
        public abstract bool validateConnection();

        /// <summary>
        /// Evaluates the given clarion expressions
        /// </summary>
        /// <param name="eval"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public abstract CASEvaluateResult[] getEvaluation(iCASEvaluateDto eval, long syswfuser);
       
    }
}