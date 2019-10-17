using System;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.CASKernel;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// BO for accessing Clarion 
    /// </summary>
    public class CASBo : AbstractCASBo
    {
        public static String OP = "_f('CRMOP')";
        public static String OP_VT = "_f('CRMOP_VT')";
        public static String OBLIGO ="_f('CRMOBLIGO')";
        public static String OBLIGOKWG = "_f('CRMOBLIGOKWG')";

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Clarion Eval Service BO
        /// </summary>
        public CASBo()
        {
        }

        public override bool validateConnection()
        {
            try
            {
                CASKernelClient client = new CASKernelClient();

                CASEvaluate ieval = new CASEvaluate();
                ieval.Area = "PERSON";
                ieval.SysWfuser = 1;
                ieval.Mandant = 0;
                ieval.ExprList = new Expression[1];
                ieval.IDList = new ID[1];

                Expression exp = new Expression();
                exp.Value = "1";
                ieval.ExprList[0] = exp;

                ID id = new ID();
                id.SysID = 0;
                ieval.IDList[0] = id;

                CASEvaluateOutput output = client.CicEval(ieval);
                if ("0".Equals(output.ReturnMessage.RetCode))
                    return true;
                else return false;
            }
            catch (Exception e)
            {
                _log.Error("CAS DISABLED, restart BOS to retry. Will automatically retry upon next CAS-Call in 1 Minute.", e);
                return false;
            }
        }

        /// <summary>
        /// Evaluates the given clarion expressions
        /// </summary>
        /// <param name="eval"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public override CASEvaluateResult[] getEvaluation(iCASEvaluateDto eval, long syswfuser)
        {
            CASKernelClient client = null;
            if (eval.url == null)
                client = new CASKernelClient();
            else client = new CASKernelClient("WSHttpBinding_CASKernel", eval.url);

            CASEvaluate ieval = new CASEvaluate();
            ieval.Area = eval.area;
            ieval.SysWfuser = syswfuser;
            ieval.Mandant = 0;
            int elen = eval.expression != null ? eval.expression.Length : 0;
            int plen = eval.param != null ? eval.param.Length : 0;
            int ilen = eval.sysID != null ? eval.sysID.Length : 0;
            ieval.ExprList = new Expression[elen];
            ieval.IDList = new ID[ilen];
            ieval.ParamList = new Parameter[plen];
            ieval.ExecID = eval.execID;
            if(eval.expression!=null)
            for (int i = 0; i < eval.expression.Length; i++)
            {
                Expression exp = new Expression();
                exp.Value = eval.expression[i];
                ieval.ExprList[i] = exp;
            }
            for (int i = 0; i < eval.sysID.Length; i++)
            {
                ID id = new ID();
                id.SysID = eval.sysID[i];
                ieval.IDList[i] = id;
            }
            if (eval.param != null)
            for (int i = 0; i < eval.param.Length; i++)
            {
                Parameter par = new Parameter();
                par.Value = eval.param[i];
                ieval.ParamList[i] = par;
            }

            _log.Debug("Calling CAS with "+_log.dumpObject(eval)+" == "+_log.dumpObject(ieval));
            
            CASEvaluateOutput output = client.CicEval(ieval);
            _log.Debug("CAS returned with " + _log.dumpObject(output));
            


            if (output.OutputList != null && output.OutputList.Length > 0)
            {
                CASEvaluateResult[] rval = new CASEvaluateResult[output.OutputList.Length];
                String oneError = null;
                for (int i = 0; i < rval.Length; i++)
                {
                    CASEvaluateResult er = new CASEvaluateResult();
                    rval[i] = er;

                    er.sysID = output.OutputList[i].SysID;
                    er.clarionResult = new String[output.OutputList[i].ResultList.Length];
                    for (int t = 0; t < er.clarionResult.Length; t++)
                    {
                        er.clarionResult[t] = output.OutputList[i].ResultList[t].ClarionResult;
                        _log.Debug("Result " + t + "=" + er.clarionResult[t]);
                        if (oneError == null)
                            oneError = er.clarionResult[t];
                    }
                }
                if (!"0".Equals(output.ReturnMessage.RetCode))
                    throw new ArgumentException("CAS didnt return 0 but " + output.ReturnMessage.RetCode + " and " + output.ReturnMessage.RetMessage + ". One Result was " + oneError);

                return rval;
            }
            if (!"0".Equals(output.ReturnMessage.RetCode))
                throw new ArgumentException("CAS didnt return 0 but " + output.ReturnMessage.RetCode + " and " + output.ReturnMessage.RetMessage);

            return new CASEvaluateResult[0];
        }
    }
}