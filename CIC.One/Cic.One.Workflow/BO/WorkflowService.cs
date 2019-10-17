using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.One.DTO;
using System.Activities;
using Microsoft.VisualBasic.Activities;
using System.Activities.Statements;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO;
using CIC.Bas.Framework.Evaluate;
using System.Collections;
using System.Linq.Expressions;

namespace Cic.One.Workflow.BO
{
    /// <summary>
    /// Main Class for Evaluation on BOS
    /// used for VB and CAS evaluations
    /// 
    /// implementation for WF4
    /// 
    /// </summary>
    public class WorkflowService : IWorkflowService, IocBos
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private VisualBasicSettings vbSettings;
        private Cic.One.DTO.IWFVDao wfDao;//access to outside dao with wfvconfigs
        protected Cic.OpenOne.Common.BO.ICASBo casBo;

        public WorkflowService()
        {
            vbSettings = new VisualBasicSettings();
            //all namespaces supported by visual basic evaluations
            addImports(typeof(WorkflowContext), vbSettings.ImportReferences);
            addImports(typeof(WorkflowService), vbSettings.ImportReferences);
            addImports(typeof(AccountDto), vbSettings.ImportReferences);
            addImports(typeof(BOS), vbSettings.ImportReferences);
       
        }
        /// <summary>
        /// Sets the workflow Dao
        /// </summary>
        /// <param name="dao"></param>
        public void setWorkflowDao(Cic.One.DTO.IWFVDao dao)
        {
            this.wfDao = dao;
        }

        /// <summary>
        /// Sets the CAS BO
        /// </summary>
        /// <param name="dao"></param>
        public void setCASBo(Cic.OpenOne.Common.BO.ICASBo casBo)
        {
            this.casBo = casBo;
        }

        /// <summary>
        /// Processes a OL like vorgang with the wf4 workflow engine
        /// </summary>
        /// <param name="befehlszeile"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        /*public static List<String> processVorgang(String befehlszeile,  WorkflowContext ctx)
        {

            String workflowName = befehlszeile;
            Cic.One.Workflow.BO.Workflow wf = null;
            Activity act = Cic.One.Workflow.BO.WorkflowFactory.getInstance().getWorkflow(workflowName, 0);
           
            wf = new Cic.One.Workflow.BO.Workflow(act, ctx);
            ctx = wf.start();
            //default behaviour, add outputcommand to result-list if result list is empty
            if(ctx.results==null || ctx.results.Count==0)
            {
                ctx.results = new List<string>();
                ctx.results[0] = ctx.outputCommand;
            }
            return ctx.results;
        }*/

        /// <summary>
        /// Processes a workflow until
        ///  - finished
        ///  - or until bookmark reached (by UserIntarction-Activity)
        ///  
        /// Uses the given input context and returns the modified context
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        virtual
            public oprocessWorkflowDto processWorkflow(iprocessWorkflowDto input, oprocessWorkflowDto rval, Cic.One.DTO.IWFVDao dao, Cic.OpenOne.Common.BO.ICASBo casBo)
        {

            Cic.One.Workflow.BO.Workflow wf = null;
            this.wfDao = dao;
            this.casBo = casBo;
            Activity act = Cic.One.Workflow.BO.WorkflowFactory.getInstance().getWorkflow(input.workflowName, input.workflowVersion);
            if (input.workflowId == null || input.workflowId == Guid.Empty)//new workflow starts
            {
                if (input.workflowContext == null)
                    throw new Exception("New Workflows need a WorkflowContext!");
                wf = new Cic.One.Workflow.BO.Workflow(act, input.workflowContext);
                rval.workflowContext = wf.start();
                rval.workflowId = wf.wfid;
            }
            else//resume bookmark
            {
                wf = new Cic.One.Workflow.BO.Workflow(act, input.workflowId.Value);
                WorkflowContext context = null;
                if (input.overwriteContext)
                    context = input.workflowContext;

                rval.workflowContext = wf.resume(Cic.One.Workflow.Activities.UserInteraction.BOOKMARK_ID, context);
                rval.workflowId = wf.wfid;
            }
            if (wf.finished)
                rval.workflowId = null;// Guid.Empty;

            return rval;
        }

        /// <summary>
        /// adds the imports for the VB Evaluation
        /// </summary>
        /// <param name="type"></param>
        /// <param name="imports"></param>
        private static void addImports(Type type, ISet<VisualBasicImportReference> imports)
        {
            if (type.IsPrimitive || type == typeof(void) || type.Namespace == "System")
                return;

            var wasAdded = imports.Add(new VisualBasicImportReference { Assembly = type.Assembly.GetName().Name, Import = type.Namespace });

            if (!wasAdded)
                return;

            if (type.BaseType != null)
                addImports(type.BaseType, imports);

            foreach (var interfaceType in type.GetInterfaces())
                addImports(interfaceType, imports);

            foreach (var property in type.GetProperties())
                addImports(property.PropertyType, imports);

            foreach (var method in type.GetMethods())
            {
                addImports(method.ReturnType, imports);

                foreach (var parameter in method.GetParameters())
                    addImports(parameter.ParameterType, imports);

                if (method.IsGenericMethod)
                {
                    foreach (var genericArgument in method.GetGenericArguments())
                        addImports(genericArgument, imports);
                }
            }

            if (type.IsGenericType)
            {
                foreach (var genericArgument in type.GetGenericArguments())
                    addImports(genericArgument, imports);
            }
        }

        /// <summary>
        /// Evaluates a VB Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private String evaluateVB(String expression)
        {
            WorkflowContext ctx = null;
            return evaluateVB(expression, null, null, ref ctx);
        }

        /// <summary>
        /// Evaluates a VB Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <param name="wctx">WorkflowContext object</param>
        /// <returns></returns>
        private String evaluateVB(String expression, String area, String areaid, ref WorkflowContext wctx)
        {

            if (wctx == null)
                wctx = new WorkflowContext();


            wctx.area = area;
            wctx.areaid = areaid;
            wctx.iocBos = this;
            return evaluateVB(expression, ref wctx);
        }

        /// <summary>
        /// Evaluates the given expression inside a implicit WF4 Workflow as VB Expression
        /// The Workflow has access to the given Context
        /// The Workflow will have the expression from the overwritten ctx.inputCommand
        /// The Workflow will return the result in ctx.outputCommand
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private String evaluateVB(String expression, ref WorkflowContext ctx)
        {
            ctx.inputCommand = expression;//IMPORTANT - we use this trick to execute exactly this string inside the workflow!
            Assign<String> a = new Assign<String>();
            if (ctx.inputCommand != null)
                ctx.inputCommand = ctx.inputCommand.Trim().Replace("\n", "").Replace("\r", "");

            a.Value = new InArgument<String>(new VisualBasicValue<String>("(" + ctx.inputCommand + ").ToString()"));
            a.To = new VisualBasicReference<String>("input.outputCommand");
            DynamicActivity ab = new DynamicActivity
            {
                Properties =
               {
                   new DynamicActivityProperty
                    {
                    Name = "input",
                    Type = typeof(InOutArgument<WorkflowContext>)
                    }
               },
                Implementation = () => a

            };
            VisualBasic.SetSettings(ab, vbSettings);
            Cic.One.Workflow.BO.Workflow wf = new Cic.One.Workflow.BO.Workflow(ab, ctx, false);
            ctx = wf.start();
            //_log.Debug(_log.dumpObject(ctx));
            return ctx.outputCommand;
        }

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
        public String evaluate(String expression, String area, long sysId, ref WorkflowContext ctx)
        {
           

            String rval = evaluateInternal(expression, area, sysId,ref ctx);
            if(rval!=null&&rval.Length>3)
            {
                String prefix = rval.Substring(0, 3).ToLower();
                if(prefix.IndexOf("c#:")==0||prefix.IndexOf("cw:")==0||prefix.IndexOf("cn:")==0||prefix.IndexOf("vb:")==0)
                {
                    return evaluateInternal(rval, area, sysId,ref ctx);
                }
            }
 
            return rval;
        }
        public String evaluateInternal(String expression, String area, long sysId, ref WorkflowContext ctx)
        {
            if (expression == null || expression.Length == 0)
            {
                return expression;
            }

            if (expression.Length < 4)
            {
                return expression;
            }
            if(expression.Trim().EndsWith(";"))
            {
                expression = expression.Substring(0, expression.LastIndexOf(";"));
            }

            String prefix = expression.Substring(0, 3).ToLower();
            switch (prefix)
            {
                case ("c#:"):
                    return evaluateVB(expression.Substring(3), area, sysId.ToString(), ref ctx);

                case ("vb:"):
                    return evaluateVB(expression.Substring(3), area, sysId.ToString(), ref ctx);

                case ("cw:")://clarion for windows
                    return evaluateCAS(expression.Substring(3), area, sysId.ToString(), ref ctx);
                case ("cn:")://bas evaluate
                    return evaluateBAS(expression.Substring(3), area, sysId.ToString(), ref ctx);

                default:
                    return expression;

            }
        }
        public static void test()
        {
            Cic.One.Workflow.BO.WorkflowService ws = new Cic.One.Workflow.BO.WorkflowService();
            WorkflowContext wc = new WorkflowContext();
            wc.entities = new EntityContainer();
            wc.entities.angebot = new AngebotDto();
            wc.entities.angebot.attribut = "TEST";
            wc.entities.angebot.sysAntrag = 33;

            wc.entities.angob = new AngobDto();
            wc.entities.angob.ahkBrutto = 83838.22;

            ws.evaluateCAS("ANGEBOT:ATTRIBUT", "TEST", "0", ref wc);
        }
        /// <summary>
        /// Evaluates the input via CAS
        /// returns the first Result as String
        /// puts all Results into the WorkflowContext
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        private String evaluateCAS(String casFunction, String area, String areaid, ref WorkflowContext ctx)
        {

            if (casFunction == null || casFunction.Length == 0) return "";

            try
            {
                var evaluator = SimpleClarionExpressionEvaluatorFactory.Create();
                evaluator.Bind("GEBIET", new ClarionString(area));
                evaluator.Bind("GEBIETID", new ClarionString(areaid));
                evaluator.Bind("AREA", new ClarionString(area));
                evaluator.Bind("AREAID", new ClarionString(areaid));
                evaluator.RegisterGlobalVariable(area, "SYSID", true, new ClarionNumber(areaid));
                evaluator.RegisterGlobalVariable(area, "SYS" + area, true, new ClarionNumber(areaid));

                evaluator.RegisterGlobalVariable("WF", "USERID", true, new ClarionNumber("" + ctx.sysWFUSER));

                if(ctx!=null && ctx.context!=null && ctx.context.Count()>0)
                {
                    foreach(ContextVariableDto ctxvar in ctx.context)
                    {
                        if (ctxvar.group == null) continue;
                        if (ctxvar.key == null) continue;
                        evaluator.RegisterGlobalVariable(ctxvar.group, ctxvar.key, true, new ClarionString(ctxvar.value!=null?ctxvar.value:""));
                    }
                }



                if(ctx.entities!=null)
                {
                    try
                    {
                        foreach (PropertyInfo pi in getProperties(ctx.entities))
                        {
                            object entityObj = pi.GetValue(ctx.entities);
                            if (entityObj == null) continue;
                            foreach (PropertyInfo pif in getProperties(entityObj))
                            {
                                object entityFieldValue = pif.GetValue(entityObj);
                                if (entityFieldValue == null) continue;
                                ClarionObject co = null;
                                if (entityFieldValue is String)
                                    co = new ClarionString(entityFieldValue.ToString());
                                else if (entityFieldValue is Boolean)
                                    co = new ClarionBool((bool)entityFieldValue);
                                else if (entityFieldValue is Decimal)
                                    co = new ClarionDecimal((decimal)entityFieldValue);
                                else if (entityFieldValue is Double)
                                    co = new ClarionReal((double)entityFieldValue);
                                else if (entityFieldValue is Int64)
                                    co = new ClarionString("" + (long)entityFieldValue);
                                else if (entityFieldValue is Int32)
                                    co = new ClarionNumber((int)entityFieldValue);
                                else if (entityFieldValue is Int16)
                                    co = new ClarionNumber((short)entityFieldValue);
                                else continue;
                                evaluator.RegisterGlobalVariable(pi.Name.ToUpper(), pif.Name.ToUpper(), true, co);
                            }

                        }
                    }catch(Exception excm)
                    {
                        _log.Warn("Clarion Simple Expression Mapping all values failed " + excm.Message, excm);
                    }
                    
                }
                ClarionString cs = evaluator.Evaluate(casFunction);
                if(evaluator.EvaluateErrorHandler.GetErrorCode()!=0)
                {
                    throw new Exception(evaluator.EvaluateErrorHandler.GetErrorCode()+": "+evaluator.EvaluateErrorHandler.GetError()+" for "+casFunction);
                }
                return cs.ToString();
            }catch(Exception e)
            {
                _log.Info("Clarion Simple Expression skipped, using cas: " + e.Message);
            }

            //Clarion evaluate test
            if (casFunction.IndexOf("test(") > -1)
            {
                return "ev_" + Substring(casFunction, "test('", "')");
            }

            ICASBo bo = new CASBo();
            iCASEvaluateDto iEval = new iCASEvaluateDto();
            iEval.area = area;
            iEval.sysID = new long[1];
            iEval.sysID[0] = long.Parse(((areaid == null || areaid == "") ? "0" : areaid));
            iEval.expression = new String[1];
            iEval.expression[0] = casFunction;
            //iEval.url//TODO optional via activity

            return evaluateCAS(iEval, ref ctx);
        }

        private IEnumerable getProperties(object o)
        {
            PropertyInfo[] pis = o.GetType().GetProperties();
            var order = from pi in pis
                        select pi;
            return order.OrderBy(r => r.Name);
        }

        /// <summary>
        /// Evaluates the input via CAS
        /// returns the first Result as String
        /// puts all Results into the WorkflowContext
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        private String evaluateBAS(String casFunction, String area, String areaid, ref WorkflowContext ctx)
        {

            if (casFunction == null || casFunction.Length == 0) return "";

           

            //Clarion evaluate test
            if (casFunction.IndexOf("test(") > -1)
            {
                return "ev_" + Substring(casFunction, "test('", "')");
            }
          
            ICASBo bo = new CASBo();
            iCASEvaluateDto iEval = new iCASEvaluateDto();
            iEval.area = area;
            iEval.sysID = new long[1];
            iEval.sysID[0] = long.Parse(((areaid == null || areaid == "") ? "0" : areaid));
            iEval.expression = new String[1];
            iEval.expression[0] = casFunction;
            //iEval.url//TODO optional via activity

            return evaluateCAS(iEval, ref ctx);
        }

        /// <summary>
        /// takes a substring between two anchor strings (or the end of the string if that anchor is null)
        /// </summary>
        /// <param name="this">a string</param>
        /// <param name="from">an optional string to search after</param>
        /// <param name="until">an optional string to search before</param>
        /// <param name="comparison">an optional comparison for the search</param>
        /// <returns>a substring based on the search</returns>
        private static string Substring(String str, string from = null, string until = null, StringComparison comparison = StringComparison.InvariantCulture)
        {
            var fromLength = (from ?? string.Empty).Length;
            var startIndex = !string.IsNullOrEmpty(from)
                ? str.IndexOf(from, comparison) + fromLength
                : 0;

            if (startIndex < fromLength) { throw new ArgumentException("from: Failed to find an instance of the first anchor"); }

            var endIndex = !string.IsNullOrEmpty(until)
            ? str.IndexOf(until, startIndex, comparison)
            : str.Length;

            if (endIndex < 0) { throw new ArgumentException("until: Failed to find an instance of the last anchor"); }

            var subString = str.Substring(startIndex, endIndex - startIndex);
            return subString;
        }
        /// <summary>
        /// Evaluates the input via CAS
        /// returns the first Result as String
        /// puts all Results into the WorkflowContext
        /// </summary>
        /// <param name="iEval"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public String evaluateCAS(iCASEvaluateDto iEval, ref WorkflowContext ctx)
        {

            ICASBo bo = new CASBo();

            _log.Debug("Now starting CAS...");
            long syswfuser = 0;
            if (ctx != null)
                syswfuser = ctx.sysWFUSER;
            if (iEval.param == null)
                iEval.param = new string[0];
            CASEvaluateResult[] result = bo.getEvaluation(iEval, syswfuser);

            if (result != null && result.Length > 0)
            {
                if (ctx != null)
                    ctx.casResults = result;

                String rval = result[0].clarionResult[0];
                return rval;

            }
            return "";
        }

        /// <summary>
        /// Evaluates all given expressions 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public String[] evaluate(iEvalExpressionDto input)
        {
            List<String> results = new List<String>();

            foreach (String expression in input.expression)
            {
                results.Add(evaluate(expression, input.area, input.sysID, ref input.context));
            }
            return results.ToArray();
        }

        /// <summary>
        /// fetches the popup-definition from the named workflow queue, fills the gui-definition in WorkflowContext (as configured in the wfv)
        /// 
        /// </summary>
        /// <param name="wfv"></param>
        /// <param name="queue"></param>
        /// <param name="wctx"></param>
        virtual public void showPopup(string wfv, string queueName, WorkflowContext wctx)
        {
            if (wctx.context == null)
                wctx.context = new ContextVariableDto[0];

            
            wctx.inputfields = new List<Viewfield>();
            if (wfDao == null) return;
            WfvEntry entry = wfDao.getWfvEntry(wfv);
            if (entry != null && entry.customentry != null && entry.customentry.viewmeta != null) //if the wfv was found, use the fields there for the gui to display
            {
                foreach (Viewfield vf in entry.customentry.viewmeta.fields)
                {
                    Viewfield nvf = vf.clone();

                    wctx.inputfields.Add(nvf);
                }
            }
          

        }
    }
}
