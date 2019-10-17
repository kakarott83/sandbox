using AutoMapper;
using Cic.One.DTO;
using Cic.One.DTO.BN;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;
//using Cic.OpenOne.GateBANKNOW.Common.BO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

using CIC.Bas.Modules.OpenBPE.ServiceAccess;
using CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto;
using CIC.Bas.Framework.Extensibility;
using CIC.Bas.Modules.OpenBPE.Storage.Models;
using Cic.OpenOne.Common.MediatorService;

namespace Cic.One.Workflow.BO
{
    /// <summary>
    /// BPE Implementation of a Workflow Service
    /// 
    /// later implementations should implement the interface and use bpe evaluate and not extend the cas-service version
    /// </summary>
    public class BPEWorkflowService : WorkflowService, IocBos
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static String QUERYEVALCODE = "select expression from BPEVALDEF where evaluatecode=:code and expressionlang in (0,2)";
        // and eventcode=:eventcode
        private static String QUERYLISTENER = "select sysbplistener from bplistener,bpprocdef where isusertask=1 and bplistener.sysbpprocdef=bpprocdef.sysbpprocdef and bplistener.oltable=:area and sysoltable=:sysid and bpprocdef.processdefcode=:proccode and (bplistener.supervisorflags>=16 or supervisorflags=0) order by bplistener.sysbplistener";
        private static String QUERYLISTENERINFO = @"select bplistener.sysbplistener, bpprocdef.description,nvl(ev.description,nvl(evt.description,bplistener.eventcode)) stepdescription,ev.evaluatecode,evt.eventcode,bpprocdef.processdefcode from bplistener , bpevaldef ev, bpeventdef evt,bpprocdef
                        where bplistener.evaluatecode=ev.evaluatecode(+) and bplistener.eventcode=evt.eventcode(+) and bplistener.sysbpprocdef=bpprocdef.sysbpprocdef and bplistener.sysbplistener=:sysbplistener";
        private static String VALIDATELANE = "SELECT 1 FROM bproleusr a1, bplane a2 WHERE a1.syswfuser = :syswfuser AND a1.namebprole = a2.namebprole AND a2.namebplane = :namebplane";

        private Cic.One.DTO.IWFVDao wfDao;//access to outside dao with wfvconfigs
        private QueueDto[] lookups;//lookups from current wf

        /// <summary>
        /// convert a long to a guid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid toGuid(long value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
        /// <summary>
        /// convert a guid to a long
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static long fromGuid(Guid g)
        {
            byte[] bytes = g.ToByteArray();
            return BitConverter.ToInt64(bytes, 0);
        }


        /// <summary>
        /// Processes a workflow until
        ///  - finished
        ///  - or until bookmark reached (by UserIntarction-Activity)
        ///  
        /// Uses the given input context and returns the modified context
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        override
        public oprocessWorkflowDto processWorkflow(iprocessWorkflowDto input, oprocessWorkflowDto rval, Cic.One.DTO.IWFVDao dao, Cic.OpenOne.Common.BO.ICASBo casBo)
        {
            if (input.workflowContext.areaid == null || input.workflowContext.areaid.Length == 0)
                input.workflowContext.areaid = "0";
            try
            {
                long.Parse(input.workflowContext.areaid);
            }catch(Exception)
            {
                //we have some invalid nonsense in areaid, make a number
                input.workflowContext.areaid = "0";
            }
            if (input.workflowContext.area == null || input.workflowContext.area.Length == 0)
                input.workflowContext.area = "SYSTEM";
            
            //----------------
            try
            {
                /*if ("ANGEBOT".Equals(input.workflowContext.area.ToUpper()) && long.Parse(input.workflowContext.areaid) == 0)
                {
                    IAngAntBo bo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createAngAntBo();
                    Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto tmpAng = new Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto();
                    tmpAng.erfassungsclient = 10;
                    tmpAng.angAntVars = new List<OpenOne.GateBANKNOW.Common.DTO.AngAntVarDto>();
                    tmpAng = bo.createOrUpdateAngebot(tmpAng, input.workflowContext.sysPEROLE);
                    input.workflowContext.areaid = "" + tmpAng.sysid;
                    BNAngebotDto angebotOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto,BNAngebotDto>(tmpAng);
                    input.workflowContext.entities.BNAngebot = angebotOutput;
                }
                else if ("ANTRAG".Equals(input.workflowContext.area.ToUpper()) && long.Parse(input.workflowContext.areaid) == 0)
                {
                    IAngAntBo bo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createAngAntBo();
                    Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto tmpAnt = new Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto();
                    tmpAnt.erfassungsclient = 10;
                    
                    tmpAnt = bo.createOrUpdateAntrag(tmpAnt, input.workflowContext.sysPEROLE);
                    input.workflowContext.areaid = "" + tmpAnt.sysid;
                    BNAntragDto antragOutput = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto, BNAntragDto>(tmpAnt);
                    input.workflowContext.entities.BNAntrag = antragOutput;

                }
                else */if ("WFEXEC".Equals(input.workflowContext.area.ToUpper()) && long.Parse(input.workflowContext.areaid) == 0)
                {
                    using (DdOwExtended ctx = new DdOwExtended())
                    {
                        WFEXEC ex = new WFEXEC();
                        ctx.AddToWFEXEC(ex);
                        ctx.SaveChanges();
                        input.workflowContext.areaid = "" + ex.SYSWFEXEC;
                       
                    }

                }
            }catch(Exception e)
            {
                _log.Error("Cannot create a new ANGEBOT for BPE", e);
            }
            //----------------

            rval.workflowId = null;
            rval.workflowContext = input.workflowContext;
            rval.workflowContext.messages = new List<WorkflowMessageDto>();
            this.wfDao = dao;
            this.casBo = casBo;
            input.workflowContext.iocBos = this;
            long areaid = long.Parse(input.workflowContext.areaid);
            try
            {
                IMediatorService svc = new Cic.OpenOne.Common.MediatorService.MediatorServiceClient();
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    if (input.workflowId == null && areaid>0)
                    {
                    
                            List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                            //pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "eventcode", Value = input.workflowContext.inputCommand });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = input.workflowContext.area });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = areaid });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "proccode", Value = input.workflowName });

                            long wfid = ctx.ExecuteStoreQuery<long>(QUERYLISTENER, pars.ToArray()).FirstOrDefault();
                            if (wfid > 0)
                            {
                                input.workflowId = toGuid(wfid);
                                input.sysbplistener = wfid;
                                input.workflowContext.inputCommand = null;//avoid endless loop
                            }
                            else
                            {
                                //we could not find the bplistener by area/id/proccode but we where given the exact listener id, so it must have been ended meanwhile
                                if(input.workflowContext.bplistener!=null && input.workflowContext.bplistener.sysbplistener>0)
                                {
                                    WorkflowMessageDto msg = new WorkflowMessageDto();
                                    msg.title = "Hinweis";
                                    msg.type = 0;
                                    msg.message = "Der Prozess wurde bereits automatisch beendet";
                                    rval.workflowContext.messages.Add(msg);
                                    rval.workflowContext.outputCommand += "jv:GUI.goLastEntityDetail()";
                                    return rval;
                                }
                            }
                    }
                    
                    //if we have a listener (process has already been started), check if there is an evaluatecode present or not
                    if ((input.workflowContext.inputCommand == null || input.workflowContext.inputCommand.Length==0) && input.workflowId.HasValue)
                    {
                        long sysbplistener = fromGuid(input.workflowId.Value);
                        String evalcode= ctx.ExecuteStoreQuery<String>("select evaluatecode from bplistener where sysbplistener="+sysbplistener,null).FirstOrDefault();
                        if (evalcode == null)//Process waits for event
                        {
                            String eventcode = ctx.ExecuteStoreQuery<String>("select eventcode from bplistener where sysbplistener=" + sysbplistener, null).FirstOrDefault();
                            input.workflowContext.inputCommand = eventcode;
                        }
                    }
                }

                if (input.workflowId == null)//NEUER BPE Prozess
                {
                    
                    DispatchAndExecuteEventRequest request = new DispatchAndExecuteEventRequest();
                    request.AreaName = input.workflowContext.area;
                    request.AreaId = areaid;
                    request.ProcessDefinitionCode = input.workflowName;//aus tabelle bpprocdef
                    request.EventCode = input.workflowContext.inputCommand;//CICeventname "START_WEB_PROCESS"
                    request.UserId = input.workflowContext.sysWFUSER;
                    request.ProcessContext = new SubscriptionsDataDto();
                    request.ProcessContext.Subscriptions = new SubscriptionDto[0];
                    request.ProcessContext.Queues = new QueueDto[0];
                    request.ProcessContext.Variables = new LookupVariableDto[0];
                    updateProcessContextVariables(input.workflowContext, request.ProcessContext);

                    //start BPE and set Context
                    ResponseBase svcRval = checkError(svc.Execute(request));

                    //Handle Response
                    if (svcRval is DispatchAndExecuteEventResponse)
                    {
                        DispatchAndExecuteEventResponse resp = (DispatchAndExecuteEventResponse)svcRval;
                        ListenerModel lm = resp.ExecutionReport.Listeners.Where(a=>a.IsUserTask==true).FirstOrDefault();
                        if (lm == null)
                        {
                            WorkflowMessageDto msg = new WorkflowMessageDto();
                            msg.title = "Error";
                            msg.type = 2;
                            msg.message = "No BPListener available for BPE Workflow " + input.workflowName + " StartEvent: " + input.workflowContext.inputCommand;
                            rval.workflowContext.messages.Add(msg);
                            rval.workflowContext.outputCommand = "jv:GUI.goBackOrInitsequence()";
                            return rval;
                        }
                        
                        rval.workflowId = toGuid(lm.Id);
                        rval.sysbplistener = lm.Id;
                        LookupVariableDto[] lookups = resp.ExecutionReport.ProcessContext.Variables;
                      
                        fillContextFromMediator(rval, lookups);
                        if(!checkLane(rval,input.workflowContext.sysWFUSER, lm.LaneName))
                            return rval;
                        WorkflowContext wctx = input.workflowContext;
                        wctx.area = lm.AreaName;
                        wctx.areaid = "" + lm.AreaId;
                        wctx.sysbpprocinst = lm.ProcessInstanceId.GetValueOrDefault();
                        using (PrismaExtended ctx = new PrismaExtended())
                        {
                            List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = lm.EvaluateCode });

                            rval.workflowContext.outputCommand = ctx.ExecuteStoreQuery<String>(QUERYEVALCODE, pars.ToArray()).FirstOrDefault();
                            _log.Debug("Workflow using evalcode " + lm.EvaluateCode + " as expression " + rval.workflowContext.outputCommand);
                            this.lookups = resp.ExecutionReport.ProcessContext.Queues;
                            execAdditionalCommands(lookups, ref wctx);
                            rval.workflowContext.outputCommand = evaluate(rval.workflowContext.outputCommand, input.workflowContext.area, long.Parse(input.workflowContext.areaid), ref wctx);
                        }
                        fillButtons(rval, resp.ExecutionReport.ProcessContext.Queues);
                        updateListenerInfo(lm.Id,rval);
                    }

                }
                //event-driven eg button sends a command
                else if (input.workflowContext.inputCommand != null && input.workflowContext.inputCommand.Length > 0)
                {
                    //get current context
                    GetProcessContextRequest gctxreq = new GetProcessContextRequest();
                    gctxreq.ListenerId = fromGuid(input.workflowId.Value);
                    GetProcessContextResponse presp = (GetProcessContextResponse)checkError(svc.Execute(gctxreq));
                    SubscriptionsDataDto context = presp.ProcessContext;
                    //update context
                    updateProcessContextVariables(input.workflowContext, context);

                    long timestamp = 0;
                    GetInstanceTimestampRequest reqtime = new GetInstanceTimestampRequest();
                    reqtime.ProcessInstanceId = presp.ProcessInstanceId;
                    //get current changestamp
                    ResponseBase svcRval = checkError(svc.Execute(reqtime));
                    if (svcRval is GetInstanceTimestampResponse)
                    {
                        GetInstanceTimestampResponse resp = (GetInstanceTimestampResponse)svcRval;
                        timestamp=resp.InstanceTimestamp;
                    }
                    DispatchAndExecuteListenerRequest request = new DispatchAndExecuteListenerRequest();
                    request.ListenerId = fromGuid(input.workflowId.Value);
                    request.UserId = input.workflowContext.sysWFUSER;
                    request.ProcessContext = context;
                    request.OverwriteProcessContext = true;
                    request.InstanceTimestamp = timestamp;
                    
                    //continue BPE
                    svcRval = checkError(svc.Execute(request));

                    //Handle Response
                    if (svcRval is DispatchAndExecuteListenerResponse)
                    {
                        DispatchAndExecuteListenerResponse resp = (DispatchAndExecuteListenerResponse)svcRval;
                        ListenerModel lm = resp.ExecutionReport.Listeners.Where(a => a.IsUserTask == true).FirstOrDefault();
                        if (lm == null || lm.EvaluateCode == null)
                        {
                            if (resp.ExecutionReport.ProcessInstance.IsDeleted.HasValue && resp.ExecutionReport.ProcessInstance.IsDeleted.Value == true)
                            {
                                WorkflowMessageDto msg = new WorkflowMessageDto();
                                msg.title = "Hinweis";
                                msg.type = 0;
                                msg.message = "Der Prozess wurde erfolgreich beendet";
                                rval.workflowContext.messages.Add(msg);
                                rval.workflowContext.outputCommand += "jv:GUI.goBackOrInitsequence()";
                                return rval;
                            }


                            if (lm == null || lm.EvaluateCode == null)
                            {
                                WorkflowMessageDto msg = new WorkflowMessageDto();
                                msg.title = "Hinweis";
                                msg.type = 0;
                                msg.message = "Der Prozess wurde beendet";//mit Status "+resp.ExecutionReport.ProcessInstance.ExecutionStatus+" abgeschlossen";
                                rval.workflowContext.messages.Add(msg);
                                rval.workflowContext.outputCommand += "jv:GUI.goBackOrInitsequence()";
                                return rval;
                            }
                        }
                        rval.workflowId = toGuid(lm.Id);
                        rval.sysbplistener = lm.Id;
                        LookupVariableDto[] lookups = resp.ExecutionReport.ProcessContext.Variables;
                        
                        fillContextFromMediator(rval, lookups);

                        WorkflowContext wctx = input.workflowContext;
                        wctx.area = lm.AreaName;
                        wctx.areaid = ""+lm.AreaId;
                        wctx.sysbpprocinst = lm.ProcessInstanceId.GetValueOrDefault();
                        if (lm.EvaluateCode != null)
                        {
                            if(!checkLane(rval,input.workflowContext.sysWFUSER,lm.LaneName))
                                return rval;
                            using (PrismaExtended ctx = new PrismaExtended())
                            {
                                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = lm.EvaluateCode });

                                rval.workflowContext.outputCommand = ctx.ExecuteStoreQuery<String>(QUERYEVALCODE, pars.ToArray()).FirstOrDefault();
                                _log.Debug("Workflow using evalcode " + lm.EvaluateCode + " as expression " + rval.workflowContext.outputCommand);
                                this.lookups = resp.ExecutionReport.ProcessContext.Queues;
                                execAdditionalCommands(lookups, ref wctx);
                                rval.workflowContext.outputCommand = evaluate(rval.workflowContext.outputCommand, input.workflowContext.area, long.Parse(input.workflowContext.areaid), ref wctx);
                            }
                        }
                        fillButtons(rval, resp.ExecutionReport.ProcessContext.Queues);
                        updateListenerInfo(lm.Id,rval);
                    }
                }
                else//wf already running, dont continue, just display current state (inputcommand empty)
                {
                    GetProcessContextRequest gctxreq = new GetProcessContextRequest();
                    gctxreq.ListenerId = fromGuid(input.workflowId.Value);
                    GetProcessContextResponse presp = (GetProcessContextResponse)checkError(svc.Execute(gctxreq));

                    ListenerModel lm = null;

                    using (PrismaExtended ctx = new PrismaExtended())
                    {
                        lm = ctx.ExecuteStoreQuery<ListenerModel>("select sysbplistener Id,bplistener.oltable areaname, sysoltable areaid, bplistener.sysbpprocinst ProcessInstanceId,bplistener.namebplane LaneName, bplistener.evaluatecode EvaluateCode from bplistener where  isusertask=1 and sysbplistener=" + gctxreq.ListenerId, null).FirstOrDefault();
                    }
                   
                    
                    //get current context
                   /* GetListenersRequest request = new GetListenersRequest();
                    request.AreaName = input.workflowContext.area;
                    request.AreaId = long.Parse(input.workflowContext.areaid);
                    request.ProcessDefinitionCode = input.workflowName;//aus tabelle bpprocdef
                    ResponseBase svcRval = (ResponseBase)checkError(svc.Execute(request));
                    */
                    //Handle Response
                    //if (svcRval is GetListenersResponse)
                    {
                       // GetListenersResponse resp = (GetListenersResponse)svcRval;
                        //ListenerModel lm = resp.Listeners.Where(a => a.IsUserTask == true).FirstOrDefault();



                        if (lm == null || lm.EvaluateCode == null)
                        {
                            WorkflowMessageDto msg = new WorkflowMessageDto();
                            msg.title = "Hinweis";
                            msg.type = 0;
                            msg.message = "Der Prozess wurde abgeschlossen";
                            rval.workflowContext.messages.Add(msg);
                            rval.workflowContext.outputCommand = "jv:GUI.goBackOrInitsequence()";
                            return rval;
                        }
                        rval.workflowId = toGuid(lm.Id);
                        rval.sysbplistener = lm.Id;
                        //update the bplistener incoming variables with evtl. additional variables coming from gui, like when
                        //jv:GUI._L("PVAR","GEBIET","ANGEBOT") & jv:GUI._L("PVAR","GEBIETID",11181) & jv:GUI.continueWF("tprc_WFA_NFP_allg_Toolbar","REMINDER",634) 
                        updateProcessContextVariables(input.workflowContext, presp.ProcessContext);
                        LookupVariableDto[] lookups = presp.ProcessContext.Variables;
                        
                        fillContextFromMediator(rval, lookups);

                        WorkflowContext wctx = input.workflowContext;
                        wctx.area = lm.AreaName;
                        wctx.areaid = "" + lm.AreaId;
                        wctx.sysbpprocinst = lm.ProcessInstanceId.GetValueOrDefault();
                        if (lm.EvaluateCode != null)
                        {
                            if (!checkLane(rval, input.workflowContext.sysWFUSER, lm.LaneName))
                                return rval;
                            using (PrismaExtended ctx = new PrismaExtended())
                            {
                                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = lm.EvaluateCode });

                                rval.workflowContext.outputCommand = ctx.ExecuteStoreQuery<String>(QUERYEVALCODE, pars.ToArray()).FirstOrDefault();
                                _log.Debug("Workflow using evalcode " + lm.EvaluateCode + " as expression " + rval.workflowContext.outputCommand);
                                this.lookups = presp.ProcessContext.Queues;
                                execAdditionalCommands(lookups,ref wctx);
                                rval.workflowContext.outputCommand = evaluate(rval.workflowContext.outputCommand, input.workflowContext.area, long.Parse(input.workflowContext.areaid), ref wctx);
                            }
                        }
                        fillButtons(rval, presp.ProcessContext.Queues);
                        updateListenerInfo(lm.Id,rval);
                    }
                }


            }
            catch (Exception ex)
            {
                WorkflowMessageDto msg = new WorkflowMessageDto();
                msg.title = "Error";
                msg.type = 3;
                msg.message = "Prozessverarbeitungsfehler: " + ex.Message;
                _log.Error("Failure communicating with BPE", ex);
                rval.workflowContext.messages.Add(msg);
            }
            if(rval.workflowContext.continueWF)
            {
                input.workflowContext.continueWF = false;
                input.workflowContext.inputCommand = "SAVECLIENT";
                String oc = input.workflowContext.outputCommand;
                oprocessWorkflowDto rval2= processWorkflow(input, rval, dao,casBo);
                rval2.workflowContext.outputCommand = oc + ";" + rval2.workflowContext.outputCommand;
                return rval2;
            }
            return rval;
        }

        /// <summary>
        /// Check Error and return exception with message if error found
        /// </summary>
        /// <param name="resp"></param>
        public static ResponseBase checkError(ResponseBase resp)
        {
            if (resp is ErrorResponse)
            {
                ErrorResponse errInfo = (ErrorResponse)resp;
                throw new Exception(errInfo.Error);
            }
            return resp;
        }

        /// <summary>
        /// Validates the workflows current lane against the user requesting the workflow
        /// </summary>
        /// <param name="resp"></param>
        /// <param name="syswfuser"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool checkLane(oprocessWorkflowDto resp,long syswfuser, String name)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "namebplane", Value = name });
              
                long found = ctx.ExecuteStoreQuery<long>(VALIDATELANE, pars.ToArray()).FirstOrDefault();
                if (found == 0)
                {
                    WorkflowMessageDto msg = new WorkflowMessageDto();
                    msg.title = "Hinweis";
                    msg.type = 0;
                    msg.message = "Prozess-Lane ausserhalb Ihrer Berechtigungen";
                    resp.workflowContext.messages.Add(msg);
                    resp.workflowContext.outputCommand = "jv:GUI.goBack()";
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Fills the workflowcontext variable list from lookup variables from service
        /// </summary>
        /// <param name="rval"></param>
        /// <param name="lookups"></param>
        private static void fillContextFromMediator(oprocessWorkflowDto rval, LookupVariableDto[] lookups)
        {
            if (lookups == null) return;
            rval.workflowContext.context = new ContextVariableDto[lookups.Length];
            int i = 0;
            foreach (LookupVariableDto v in lookups)
            {
                if (v == null) continue;
                if ("WEB_VARS".Equals(v.VariableName))
                {
                    if ("CMDS".Equals(v.LookupVariableName))
                    {
                        continue;//dont return this variable
                    }
                }

                
                rval.workflowContext.context[i] = new ContextVariableDto();
                rval.workflowContext.context[i].group = v.VariableName;
                rval.workflowContext.context[i].key = v.LookupVariableName;
                rval.workflowContext.context[i].value = v.Value;
                i++;
            }
        }

        /// <summary>
        /// Updates the info about the current listener
        /// </summary>
        /// <param name="rval"></param>
        private static void updateListenerInfo(long sysbplistener,oprocessWorkflowDto rval)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                    
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysbplistener", Value = sysbplistener });
                            
                rval.workflowContext.bplistener = ctx.ExecuteStoreQuery<BPListenerDto>(QUERYLISTENERINFO, pars.ToArray()).FirstOrDefault();
           }
        }
        /// <summary>
        /// Fills the gui workflow-buttons from a queue named WEB_ACTIONS or the queue given in the BOS.showGUI -command
        /// 
        /// must be called after a evaluate (BPE EvaluateCode which might change the currently used queue
        /// 
        /// QUEUE-Variables needed:
        /// DESC: F02
        /// ICON: F03
        /// TEXT: F04
        /// COMMAND: F05
        /// RANG: F06
        /// ENABLED/DISABLED: F08
        /// 
        /// </summary>
        /// <param name="rval"></param>
        /// <param name="lookups"></param>
        private static void fillButtons(oprocessWorkflowDto rval, QueueDto[] lookups)
        {

            rval.workflowContext.buttons = null;
            String queueName = "WEB_ACTIONS";
            if (rval.workflowContext.contextInternal != null)
            {
                //allow setting a different queuename from BOS.showGUI, when context QUEUE ist set
                String testqueue = (from a in rval.workflowContext.contextInternal
                             where a.key.Equals("QUEUE")
                             select a.value).FirstOrDefault();
                if (testqueue != null && testqueue.Length > 0)
                    queueName = testqueue;
            }
            if(lookups!=null)
            {
                QueueDto queue = (from s in lookups
                                  where s.Name.Equals(queueName)
                                  select s).FirstOrDefault();
                if (queue != null)
                {
                    QueueRecordDto[] records = queue.Records;
                    List<WorkflowButtonDto> buttons = new List<WorkflowButtonDto>();

                    foreach (QueueRecordDto rec in records)//every button
                    {
                        QueueRecordValueDto[] values = rec.Values;
                        WorkflowButtonDto button = new WorkflowButtonDto();
                        button.area = rval.workflowContext.area;
                        button.areaid = rval.workflowContext.areaid;
                        
                        button.command = getQueueField("F05", rec.Values);
                        button.icon = getQueueField("F03", rec.Values);
                        button.text = getQueueField("F04", rec.Values);
                        button.desc3 = getQueueField("F02", rec.Values);//SEGMENT
                        String tmp = getQueueField("F08", rec.Values);
                        button.disabled = "1".Equals(tmp) ? 1 : 0;
                        button.type = 5;//default all are toolbaritems
                        if (button.desc3!=null && button.desc3.Equals("COMMAND"))
                            button.type = 0;
                        button.persist = 0;
                        tmp = getQueueField("F06", rec.Values);
                        button.rang = 0;
                        try
                        {
                            if(tmp!=null && tmp.Length>0)
                                button.rang = Int16.Parse(tmp);
                        }catch(Exception)
                        {
                            _log.Warn("BPE Button from Queue "+queueName+" has wrong number in F06 for command "+button.command);
                        }


                        buttons.Add(button);
                    }
                    rval.workflowContext.buttons = buttons.OrderBy(a => a.rang).ToList();
                }
            }
            if (rval.workflowContext.buttons == null)
                rval.workflowContext.buttons = new List<WorkflowButtonDto>();

            WorkflowButtonDto tb = (from t in rval.workflowContext.buttons
                                        where t.type==0
                                        select t).FirstOrDefault();
           /* if (tb == null)//always deliver a command button for save
            {
                WorkflowButtonDto button = new WorkflowButtonDto();
                button.area = rval.workflowContext.area;
                button.areaid = rval.workflowContext.areaid;

                button.command = "SAVECLIENT";
                button.icon = "";
                button.text = "Speichern";
                button.disabled = 0;
                button.type = 0;

                button.persist = 0;
                button.rang = 0;
                rval.workflowContext.buttons.Add(button);
            }*/
        }

        /// <summary>
        /// Returns the value of a certain field of a queuerecord
        /// </summary>
        /// <param name="id"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        private static String getQueueField(String id, QueueRecordValueDto[] records)
        {
            QueueRecordValueDto val = (from s in records
                                       where s.VariableName.Equals(id)
                                       select s).FirstOrDefault();
            if (val == null) return null;
            return val.Value;
        }

        /// <summary>
        /// Creates an Object with fixed Parameter-Names from a record
        /// Mapping: F10->P01
        ///          F11->P02
        ///          ...
        ///          F19->P10
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private EvalParamDto fillFromQueue(QueueRecordValueDto[] records)
        {
            EvalParamDto rval = new EvalParamDto();
            foreach(QueueRecordValueDto rec in records)
            {
                if ("F10".Equals(rec.VariableName))
                    rval.P01 = rec.Value;
                if ("F11".Equals(rec.VariableName))
                    rval.P02 = rec.Value;
                if ("F12".Equals(rec.VariableName))
                    rval.P03 = rec.Value;
                if ("F13".Equals(rec.VariableName))
                    rval.P04 = rec.Value;
                if ("F14".Equals(rec.VariableName))
                    rval.P05 = rec.Value;
                if ("F15".Equals(rec.VariableName))
                    rval.P06 = rec.Value;
                if ("F16".Equals(rec.VariableName))
                    rval.P07 = rec.Value;
                if ("F17".Equals(rec.VariableName))
                    rval.P08 = rec.Value;
                if ("F18".Equals(rec.VariableName))
                    rval.P09 = rec.Value;
                if ("F19".Equals(rec.VariableName))
                    rval.P10 = rec.Value;
            }
            return rval;
        }

        private void execAdditionalCommands(LookupVariableDto[] lookups, ref WorkflowContext ctx)
        {
            if (lookups == null) return;

            String oc = ctx.outputCommand;//remember, will be overridden by evaluate...
            foreach (LookupVariableDto v in lookups)
            {
                if (v == null) continue;
                if("WEB_VARS".Equals(v.VariableName))
                {
                    if("CMDS".Equals(v.LookupVariableName))
                    {
                        String cmds = v.Value;
                        if (cmds == null || cmds.Length == 0) return;
                        String[] cmdarr = cmds.Split(';');
                        foreach(String cmd in cmdarr)
                        {
                            try
                            {
                                evaluate(cmd, ctx.area, long.Parse(ctx.areaid), ref ctx);
                            }catch(Exception exc)
                            {
                                _log.Error("Error in BPE WEB_VARS/CMDS Command: " + cmd, exc);
                            }
                        }
                        break;
                    }
                }
            }
            ctx.outputCommand = oc;//restore command
        }

        /// <summary>
        /// Updates the Variables of the processContext
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="subCtx"></param>
        private static void updateProcessContextVariables(WorkflowContext ctx, SubscriptionsDataDto subCtx)
        {
            List<LookupVariableDto> vars = null;
            if (ctx.context != null)
            {
                if (subCtx.Variables == null)
                    subCtx.Variables = new LookupVariableDto[ctx.context.Length];
                vars = new List<LookupVariableDto>(subCtx.Variables);

                foreach (ContextVariableDto v in ctx.context)
                {
                    if (v == null) continue;
                    if (v.key == null) continue;
                    if (v.group == null) continue;//vars without group not supported

                    String grp = v.group;


                    List<LookupVariableDto> lvars = (from sv in vars
                                              where sv.VariableName != null && sv.VariableName.Equals(grp)
                                                                             && sv.LookupVariableName.Equals(v.key)
                                              select sv).ToList();
                    if (lvars == null || lvars.Count == 0)
                    {
                        LookupVariableDto lvar = new LookupVariableDto();
                        lvar.VariableName = v.group;
                        lvar.LookupVariableName = v.key;
                        lvar.Value = v.value;
                        vars.Add(lvar);
                    }
                    else
                    {
                        foreach(LookupVariableDto lvar in lvars)
                        {
                            lvar.Value = v.value;
                        }
                    }
                }
            }
            if (subCtx.Variables == null)
            { 
                subCtx.Variables = new LookupVariableDto[1];
            }
            if(vars==null)
                vars = new List<LookupVariableDto>(subCtx.Variables);

            //Remove all, might be more than one command in there?!
            List<LookupVariableDto> lvarcmds = (from sv in vars
                                         where sv.VariableName.Equals("WEB_VARS")
                                                                     && (sv.LookupVariableName.Equals("CMD"))
                                         select sv).ToList();
            foreach(LookupVariableDto lv in lvarcmds)
            {
                vars.Remove(lv);
            }
            //always set the workflowcontext inputcommand into a lookupvariable as command
            LookupVariableDto lvarcmd = (from sv in vars
                                         where sv.VariableName.Equals("WEB_VARS")
                                                                     && sv.LookupVariableName.Equals("CMD")
                                      select sv).FirstOrDefault();
            if (lvarcmd == null)
            {
                lvarcmd = new LookupVariableDto();
                lvarcmd.VariableName = "WEB_VARS";
                lvarcmd.LookupVariableName = "CMD";
                lvarcmd.Value = ctx.inputCommand;
                vars.Add(lvarcmd);
            }
            else
                lvarcmd.Value = ctx.inputCommand;
            

            subCtx.Variables = vars.ToArray();
        }

        /// <summary>
        /// fetches the popup-definition from the named workflow queue, fills the gui-definition in WorkflowContext (as configured in the wfv)
        /// 
        /// program-flow:
        ///  * BPE returns an evaluateCommand vb:BOS.showPopup()....
        ///  * this method is called and the wfv-definition from the eval is used to read all field-definitions from the wfvconfig
        ///  * when the queue is filled, the corresponding wfv-values are replaced with the queue-variables, if queue variables are filled (e.g. if F04 is empty in queue, the value from the wfv is used)
        ///  * BAS will be called to evaluate the following fields, when they begin with cw:
        ///     * LABEL, VALUE
        ///     * Queue-Records F10-F19 will be put into P01-P10 and can be used in the label/values expression with the ${{object.PXY}} notation
        ///     * e.g. a Queue var F10 will be in P01 and can be used this way in the expression in a label:
        ///        label="cw:test('Name',${{object.P01}})"
        ///     * the clarion expression will be evaluated inside the area/id of the workflow, so common clarion-variables like VT:SYSID will also be available!
        ///     
        ///  
        ///  * the filled fields are returned to the gui workflow-context
        ///  * the gui renders a gviewdetail with the fields
        /// 
        /// </summary>
        /// <param name="wfv"></param>
        /// <param name="queue"></param>
        /// <param name="wctx"></param>
        override
        public void showPopup(string wfv, string queueName, WorkflowContext wctx)
        {
            if (wctx.context == null)
                wctx.context = new ContextVariableDto[0];

            WfvEntry entry = wfDao.getWfvEntry(wfv);
            wctx.inputfields = new List<Viewfield>();
            if (entry == null || entry.customentry == null || entry.customentry.viewmeta == null) return;

            //if the wfv was found, use the fields there for the gui to display
             
            foreach (Viewfield vf in entry.customentry.viewmeta.fields)
            {
                Viewfield nvf = vf.clone();

                wctx.inputfields.Add(nvf);
            }
            
            /*
             * Werte für Übergabe in Queue:
                F01 Field Unique Id (e.g. s01)
                F02 Field Name (e.g. NAME)
                F03 Field type dropdown|radioset|multiselect|multidropdown|textfield|textarea
                F04 Field label
                F05 Field Value/s (auch Multiline html)
                F06 XPRO Code für lookup
                F07 Value Type (String Long, DateTime, Int Double)
                F08 translate flag
                F09 readonly flag
             * */
            if (lookups != null && queueName!=null)
            {
                QueueDto queue = (from s in lookups
                                  where s.Name.Equals(queueName)
                                  select s).FirstOrDefault();
                if (queue != null)//use the data defined in the queue to update the viewfields, or add new fields if not yet defined in wfventry
                {
                    QueueRecordDto[] records = queue.Records;
                    
                    foreach (QueueRecordDto rec in records)//every gui item
                    {
                        QueueRecordValueDto[] values = rec.Values;
                        //F01 == Field id
                        String id = getQueueField("F01", rec.Values);
                        Viewfield vf = (from t in wctx.inputfields
                                        where t.id.Equals(id)
                                        select t).FirstOrDefault();
                        //create an object from the queuerecords holding P01-P10 for replacement in the wfc configured expressions
                        EvalParamDto ep = fillFromQueue(rec.Values);
                        bool addnewInput = false;
                        if(vf==null)
                        {
                            vf = new Viewfield();
                            vf.attr = new ViewFieldAttributes();
                            addnewInput = true;
                            
                        }
                        vf.ep = ep;
                        try
                        {
                            if (vf.value == null)
                                vf.value = new ViewValue();
                            String tmp = getQueueField("F02", rec.Values);
                            if (tmp != null && tmp.Length > 0)
                                vf.attr.field = tmp;

                            tmp = getQueueField("F03", rec.Values);
                            if (tmp != null && tmp.Length > 0)
                                vf.attr.viewtype = tmp;
                            if ("dropdown".Equals(vf.attr.viewtype))
                                vf.attr.viewtype = "xpro";
                            if (vf.attr.viewtype == null)
                                throw new Exception("F03 not filled with correct ViewType: " + tmp);

                            tmp = getQueueField("F04", rec.Values);
                            if (tmp != null && tmp.Length > 0)
                            {
                                //tmp = varReplacer.ReplaceText(tmp, ep, true);
                                vf.attr.label = tmp;
                            }

                            //code
                            tmp = getQueueField("F06", rec.Values);
                            if (tmp != null && tmp.Length > 0)
                                vf.attr.code = tmp;
                            //Value Type
                            tmp = getQueueField("F07", rec.Values);
                            if (tmp != null && tmp.Length > 0)
                                vf.attr.type = tmp;
                            if (vf.attr.type == null)
                                throw new Exception("F07 not filled with correct ValueType: " + tmp);
                            
                            tmp = getQueueField("F08", rec.Values);
                            if (tmp != null && tmp.Length > 0)
                                vf.attr.translate = int.Parse(tmp);

                            tmp = getQueueField("F09", rec.Values);
                            if (tmp != null && tmp.Length > 0)
                                vf.attr.ro = int.Parse(tmp);

                            //value
                            tmp = getQueueField("F05", rec.Values);
                            if (tmp != null && tmp.Length > 0)
                            {
                                vf.setStringValue(tmp);
                                //tmp = varReplacer.ReplaceText(tmp, ep, true);
                                //vf.fillFromObject(tmp);
                            }
                            if (addnewInput)
                                wctx.inputfields.Add(vf);
                        }catch(Exception e)
                        {
                            _log.Warn("BPE wanted to add a generic field with id " + id + " for vorgang "+wfv+" which was not configured correctly in the queue.",e);
                        }
                    }
                }
            }
            EvaluateViewFieldsData(wctx);
            EvaluateViewFields(casBo, wctx);
        }

        /// <summary>
        /// Replaces the label and values with data from the ${object.} notation
        /// </summary>
        /// <param name="wctx"></param>
        public static void EvaluateViewFieldsData(WorkflowContext wctx)
        {
            HtmlReportBo varReplacer = new HtmlReportBo(new StringHtmlTemplateDao(null));
            foreach (Viewfield vf in wctx.inputfields)
            {
                if (vf.ep == null)
                {
                    vf.ep = new EvalParamDto();//needed to replace the object.Fx with empty values
                    vf.ep.P01 = "";
                    vf.ep.P02 = "";
                    vf.ep.P03 = "";
                    vf.ep.P04 = "";
                    vf.ep.P05 = "";
                    vf.ep.P06 = "";
                    vf.ep.P07 = "";
                    vf.ep.P08 = "";
                    vf.ep.P09 = "";
                    vf.ep.P10 = "";
                }
                
                //special mapping from BAS  Queue Record Fields F10-F20 to this temporary object
                {
                    vf.attr.label = varReplacer.ReplaceText(vf.attr.label, vf.ep, true, true);
                    if (vf.value != null && vf.value.s != null)//when current viewfield string-value is set, try to process it and convert to the real value type
                    {
                        String tmp = varReplacer.ReplaceText(vf.value.s, vf.ep, true,true);
                        vf.setStringValue(tmp);
                        try { vf.fillFromObject(tmp); }
                        catch (Exception) { }
                    }
                }

                vf.attr.label = varReplacer.ReplaceText(vf.attr.label, wctx, true);
                if (vf.value != null && vf.value.s != null)//when current viewfield string-value is set, try to process it and convert to the real value type
                {
                    String tmp = varReplacer.ReplaceText(vf.value.s, wctx, true,false);
                    vf.setStringValue(tmp);
                    try { vf.fillFromObject(tmp); }
                    catch (Exception) { }
                }
            }
        }


        /// <summary>
        /// evaluates all gview field labels/values in the wctx.inputfields fields
        /// needs wctx.area/areaid/sysWFUSER be set, too!
        /// </summary>
        /// <param name="casBo"></param>
        /// <param name="wctx"></param>
        public static void EvaluateViewFields(ICASBo casBo, WorkflowContext wctx)
        {
            //CAS evaluations:
            if (casBo != null)
            {
                List<String> expressions = new List<String>();
                //build a expression-list for every field and supported attribute
                foreach (Viewfield vf in wctx.inputfields)
                {
                    if (vf.attr.label.IndexOf("cw:") == 0 && vf.attr.label.IndexOf("{{$") < 0)
                        expressions.Add(vf.attr.label.Substring(3));
                    if (vf.value != null && vf.value.s != null && vf.value.s.IndexOf("cw:") == 0 && vf.value.s.IndexOf("{{$") < 0)
                        expressions.Add(vf.value.s.Substring(3));
                }

                iCASEvaluateDto ieval = new iCASEvaluateDto();
                ieval.area = wctx.area;
                ieval.expression = expressions.ToArray();
                ieval.sysID = new long[] { long.Parse(wctx.areaid) };
                CASEvaluateResult[] er = null;
                if (expressions.Count > 0)
                {
                    try
                    {
                        er = casBo.getEvaluation(ieval, wctx.sysWFUSER);
                        //repeate result expression-list for every field and supported attribute
                        int idx = 0;
                        foreach (Viewfield vf in wctx.inputfields)
                        {
                            if (vf.attr.label.IndexOf("cw:") == 0)
                            {
                                if (er[0].clarionResult[idx] != null && !"".Equals(er[0].clarionResult[idx]))
                                {
                                    vf.attr.label = er[0].clarionResult[idx];
                                }
                                idx++;
                            }
                            if (vf.value != null && vf.value.s != null && vf.value.s.IndexOf("cw:") == 0)
                            {
                                if (er[0].clarionResult[idx] != null && !"".Equals(er[0].clarionResult[idx]))
                                {
                                    vf.setStringValue(er[0].clarionResult[idx]);
                                    try { vf.fillFromObject(er[0].clarionResult[idx]); }
                                    catch (Exception) { }
                                    
                                }
                                idx++;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        _log.Warn("CAS-Evaluation for BPE Popup failed for " + wctx.area + "/" + wctx.areaid, e);
                    }
                }
            }
        }

        /// <summary>
        /// Returns multiple evaluations for the area/id and user
        /// </summary>
        /// <param name="casBo"></param>
        /// <param name="area"></param>
        /// <param name="areaId"></param>
        /// <param name="sysWfUser"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static List<String> EvaluateMultiple(ICASBo casBo, String area, long areaId, long sysWfUser, List<String> expressions)
        {
            List<String> rval = new List<String>();
            //CAS evaluations:
            if (casBo != null)
            {
                
                iCASEvaluateDto ieval = new iCASEvaluateDto();
                ieval.area = area;
                ieval.expression = expressions.ToArray();
                ieval.sysID = new long[] { areaId };
                CASEvaluateResult[] er = null;
                if (expressions.Count > 0)
                {
                    try
                    {
                        er = casBo.getEvaluation(ieval, sysWfUser);
                        //repeate result expression-list for every field and supported attribute
                        int idx = 0;
                        foreach (String exp in expressions)
                        {
                            rval.Add(er[0].clarionResult[idx++]);
                        }

                    }
                    catch (Exception e)
                    {
                        _log.Warn("CAS-Evaluation for Multiple Expressions failed for " + area + "/" + areaId, e);
                    }
                }
            }
            return rval;
        }
    }
}
