using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.One.Workflow.BO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.One.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.One.Workflow.DAO;
using System.Data.EntityClient;
using Devart.Data.Oracle;


using CIC.Bas.Modules.OpenBPE.ServiceAccess;
using CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto;
using CIC.Bas.Framework.Extensibility;
using CIC.Bas.Modules.RuleEngine.Services;
using Cic.OpenOne.Common.MediatorService;
using CIC.Bas.Modules.OpenBPE.Storage.Models;
using AutoMapper;

/// <summary>
/// BOS Access Class for evaluated expressions
/// </summary>
public class BOS
{
    private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static readonly String COMMAND_WORKFLOW = "WORKFLOW";
    private static readonly String COMMAND_CAS = "CAS";
    private static readonly String COMMAND_OLSTART = "OLSTART";
    private static readonly String COMMAND_DASHBOARD = "DASHBOARD";

    /// <summary>
    /// result will be evaluated on GUI client
    /// </summary>
    /// <param name="syscode">syscode of the wfv-entry</param>
    /// <returns></returns>
    public static String fire(String syscode)
    {
        return fire(syscode, null);
    }

    /// <summary>
    /// fetches the popup-definition from the named workflow queue, fills the gui-definition in WorkflowContext (as configured in the wfv) - through ioc (like BPEWorkflowService)
    /// and commands the java-GUI to show as popup
    /// </summary>
    /// <param name="wfv"></param>
    /// <param name="queue"></param>
    /// <param name="wctx"></param>
    /// <returns></returns>
    public static String showPopup(String wfv, String queue, WorkflowContext wctx)
    {
        if (wctx.iocBos!=null)
            wctx.iocBos.showPopup(wfv, queue, wctx);

        return "jv:GUI.showPopup(\"" + wfv + "\")";
    }


    /// <summary>
    /// Adds a Message for the user with title, text, severty
    /// </summary>
    /// <param name="title"></param>
    /// <param name="text"></param>
    /// <param name="type"></param>
    /// <param name="wctx"></param>
    /// <returns></returns>
    public static String showMessage(String title, String text, int type, WorkflowContext wctx)
    {
        wctx.messages = new List<WorkflowMessageDto>();
        WorkflowMessageDto message = new WorkflowMessageDto();
        message.message = text;
        message.type = type;
        message.title = title;
        wctx.messages.Add(message);
        return "";
    }

     /// <summary>
    /// shows the gui in the WEB GUI and extracts the queue-name into the workflowcontext
    /// </summary>
    /// <param name="wfv"></param>
    /// <param name="popup"></param>
    /// <param name="queue"></param>
    /// <param name="wctx"></param>
    /// <returns></returns>
    public static String showGUI(String wfv, String popup, String queue,String popupQueue, WorkflowContext wctx)
    {
        return showGUI(wfv, popup, queue, popupQueue, wctx, null, 0);
    }

    /// <summary>
    /// shows the gui in the WEB GUI and extracts the queue-name into the workflowcontext
    /// </summary>
    /// <param name="wfv"></param>
    /// <param name="popup"></param>
    /// <param name="queue"></param>
    /// <param name="wctx"></param>
    /// <returns></returns>
    public static String showGUI(String wfv, String popup, String queue,String popupQueue, WorkflowContext wctx, String area, long sysid)
    {
        if (wctx.contextInternal == null)
            wctx.contextInternal = new ContextVariableDto[0];

        List<ContextVariableDto> vars = wctx.contextInternal.ToList();
        ContextVariableDto v = (from a in wctx.contextInternal
                            where a.key.Equals("QUEUE")
                            select a).FirstOrDefault();
        if (v == null)
        { 
            v = new ContextVariableDto();
            v.key = "QUEUE";
            v.group = "BPE";
            vars.Add(v);
        }
        v.value = queue;

        wctx.contextInternal = vars.ToArray();
        if (wctx.iocBos != null)
        {
            wctx.iocBos.showPopup(wfv, queue, wctx);
        }
        if (area == null)
        {
            area = wctx.area;
            try{
                sysid = long.Parse(wctx.areaid);
            }catch(Exception e)
            {

            }
        }
        
        if (popup == null || popup.Length == 0)
            return "jv:GUI.showGUI(\"" + wfv + "\",null,\"" + area + "\"," + sysid + ")";

        if (wctx.iocBos != null)
        {
            wctx.iocBos.showPopup(popup, popupQueue, wctx);
        }

        return "jv:GUI.showGUI(\"" + wfv + "\",\"" + popup + "\",\""+area+"\","+sysid+")";
    }

    /// <summary>
    /// shows the gui in the WEB GUI and extracts the queue-name into the workflowcontext
    /// </summary>
    /// <param name="wfv"></param>
    /// <param name="popup"></param>
    /// <param name="queue"></param>
    /// <param name="wctx"></param>
    /// <returns></returns>
    public static String showGUI(String wfv, String popup, String queue, WorkflowContext wctx)
    {
        if (wctx.contextInternal == null)
            wctx.contextInternal = new ContextVariableDto[0];

        List<ContextVariableDto> vars = wctx.contextInternal.ToList();
        ContextVariableDto v = (from a in wctx.contextInternal
                                where a.key.Equals("QUEUE")
                                select a).FirstOrDefault();
        if (v == null)
        {
            v = new ContextVariableDto();
            v.key = "QUEUE";
            v.group = "BPE";
            vars.Add(v);
        }
        v.value = queue;

        wctx.contextInternal = vars.ToArray();
        if (wctx.iocBos != null)
        {
            wctx.iocBos.showPopup(wfv, queue, wctx);
        }
        if(popup==null||popup.Length==0)
            return "jv:GUI.showGUI(\"" + wfv + "\",null)";

        if (wctx.iocBos != null)
        {
            wctx.iocBos.showPopup(popup, queue, wctx);
        }

        return "jv:GUI.showGUI(\"" + wfv + "\",\"" + popup + "\")";
    }
   

    /// <summary>
    /// fires a vorgang on the gui
    ///  - gui-evaluation with vb:BOS.fire('xy',input) causes this method to be called
    ///    the returnvalue will be evaluated as BeanShell-Code in the GUI
    ///    
    /// Depending on the wfv vorgang type (WORKFLOW|DASHBOARD|CAS| xy) it will invoke a method in the GUI
    /// WORKFLOW: GUI.launchWF
    /// DASHBOARD: GUI.openView
    /// CAS:
    /// xy: GUI.openView
    /// </summary>
    /// <param name="syscode"></param>
    /// <param name="ctx"></param>
    /// <returns></returns>
    public static String fire(String syscode, WorkflowContext wctx)
    {
        return fire(syscode, wctx, null, 0);
    }
    public static String fire(String syscode,String area, long areaId)
    {
        return fire(syscode, null, area, areaId);
    }
    /// <summary>
    /// fires a vorgang on the gui
    ///  - gui-evaluation with vb:BOS.fire('xy',input) causes this method to be called
    ///    the returnvalue will be evaluated as BeanShell-Code in the GUI
    ///    
    /// Depending on the wfv vorgang type (WORKFLOW|DASHBOARD|CAS| xy) it will invoke a method in the GUI
    /// WORKFLOW: GUI.launchWF
    /// DASHBOARD: GUI.openView
    /// CAS:
    /// xy: GUI.openView
    /// </summary>
    /// <param name="syscode"></param>
    /// <param name="ctx"></param>
    /// <returns></returns>
    public static String fire(String syscode, WorkflowContext wctx, String area, long areaId)
    {
        WorkflowDao wfd = new WorkflowDao();
        //load wfv by syscode, get befehlszeile
        String syscodeOrg = syscode;
        syscode = syscode.ToLower();
        
        String befehlszeile = wfd.getBefehlszeile(syscode);
        if (befehlszeile == null)
        {
            _log.Debug("WFV not found in DB: " + syscode);
            return "jv:GUI.openView(\"" + syscodeOrg + "\",\"" + area + "\"," + areaId + ")";
        }
        befehlszeile = befehlszeile.ToUpper();
        _log.Debug("called BOS.fire for " + syscode + ", processing " + befehlszeile);
        if (COMMAND_WORKFLOW.Equals(befehlszeile))
        {
            return "jv:GUI.launchWF(\"" + syscode + "\")";
        }
        if (COMMAND_DASHBOARD.Equals(befehlszeile))
        {
            return "jv:GUI.openView(\"" + syscode + "\",\"" + area + "\"," + areaId + ")";
        }
        if (COMMAND_CAS.Equals(befehlszeile))
        {
            String einrichtung = wfd.getWfvEinrichtung(syscode);
            if (wctx == null)
            {
                _log.Warn("called BOS.fire() Vorgang for CAS without mandatory second input-Parameter! Parameter Replacement inactive! Usage: BOS.fire('xy',input)");
                wctx = new WorkflowContext();
                wctx.entities = new EntityContainer();
            }

            {
                HtmlReportBo bo = new HtmlReportBo(new StringHtmlTemplateDao(null));
                einrichtung = bo.ReplaceText(einrichtung, wctx, true);
            }


            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(einrichtung);
            iCASEvaluateDto iEval = Cic.OpenOne.Common.Util.Serialization.XMLDeserializer.objectFromXml<iCASEvaluateDto>(bytes, "UTF-8");
            IWorkflowService ws = WorkflowServiceFactory.getInstance().getWorkflowService();
            return ws.evaluateCAS(iEval, ref wctx);
        }
        if (COMMAND_OLSTART.Equals(befehlszeile))
        {

            return deepLink("OLSTART", wctx);

        }
        //default behaviour:
        return "jv:GUI.openView(\"" + syscode + "\",\""+area+"\","+areaId+")";

    }

    /// <summary>
    /// Opens a deeplink from the GUI
    ///  target can be either extern (URL) or OpenLease (special url with registry handler for onex: prefix)
    /// </summary>
    /// <param name="wctx"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public static String deepLink(String code, WorkflowContext wctx)
    {
        return deepLink(code, wctx, 0);
    }

    /// <summary>
    /// Opens a deeplink from the GUI
    ///  target can be either extern (URL) or OpenLease (special url with registry handler for onex: prefix)
    /// </summary>
    /// <param name="wctx"></param>
    /// <param name="code"></param>
    /// <param name="continueWorkflow">if 1 then continue BPE with SAVECLIENT, when 1 also send goInitsequence()</param>
    /// <returns></returns>
    public static String deepLink(String code, WorkflowContext wctx, int continueWorkflow)
    {
        WorkflowDao wfd = new WorkflowDao();
        try
        {
            String rval = "";
            

            wctx.continueWF = continueWorkflow ==1;

            DeepLnkDto link = wfd.getDeepLink(code);
            if (link == null)
            {
                _log.Warn("Deeplink-Code nicht vorhanden: " + code + " - Funktion nicht vorhanden");
                rval= "jv:GUI.showMessage(\"Funktion nicht vorhanden\")";
            }
            else if (link.AREA!=null && !link.AREA.ToUpper().Split('|').Contains(wctx.area.ToUpper()))
            {
                _log.Warn("Deeplink-Area inkonsistent mit Übergabe für Code " + code +": "+ link.AREA + "!=" + wctx.area);
                rval = "jv:GUI.showMessage(\"Aufruf inkonsistent\")";
            }
            else if (!wfd.isDeepLinkDataAvailable(wctx.area, wctx.areaid))
            {
                _log.Warn("Deeplink Datensatz nicht gefunden für Code " + code + ": area: " + wctx.area + " id:" + wctx.areaid);
                rval = "jv:GUI.showMessage(\"Datensatz nicht gefunden\")";
            }

            //if (link.CLIENTTYPE != (int)DeepLinkType.WEB) return "jv:GUI.showMessage(\"Cannot open DeepLink: " + code + " not useable from OneWeb - wrong clienttype\")";//wrong clienttype
            else if (link.TARGETTYPE == (int)DeepLinkType.UNKNOWN)
            {
                _log.Warn("Deeplink fehlerhaft konfiguriert - kein targettype für Code " + code + ": area: " + wctx.area + " id:" + wctx.areaid);
                rval = "jv:GUI.showMessage(\"Funktion nicht vollständig konfiguriert\")";//wrong targettype
            }

            
            //when targettype=web we have to open the gui in oneWeb
            else if (link.TARGETTYPE == (int)DeepLinkType.WINDOWS)
            {
                String expr = link.getParsedExpression(wctx);
                
                String guid = wfd.execOlStart(wctx.area, long.Parse(wctx.areaid), wctx.sysWFUSER, code, expr, wctx,continueWorkflow,link.USEINBOXFLAG,link.SYSDEEPLNK);
                if (link.USEINBOXFLAG == 0)//Deeplink öffnen
                {
                    List<ContextVariableDto> cvars = new List<ContextVariableDto>();
                    if (wctx.context != null && wctx.context.Length > 0)
                        cvars = wctx.context.ToList();

                    string hostname = "";
                    try
                    {
                        hostname = Cic.One.Utils.Util.Config.ConfigUtil.GetCustomSetting(@"applicationSettings/Cic.OpenOne.Common.Properties.Config", "OpenLeaseConnectionStringSID").Trim();
                    } catch (Exception)
                    {
                        _log.Warn("applicationSettings/Cic.OpenOne.Common.Properties.Config/OpenLeaseConnectionStringSID missing in WebConf. Needed for external Deeplinks into CIC");
                        try
                        {
                            hostname = Cic.One.Utils.Util.Config.ConfigUtil.GetCustomSetting(@"applicationSettings/Cic.OpenOne.Common.Properties.Config", "OpenLeaseConnectionStringDataSource").Trim();
                            _log.Warn("Using "+hostname+" now");
                        }
                        catch (Exception)
                        {
                            

                        }
                    }

                    ContextVariableDto cvar = new ContextVariableDto();
                    cvar.value = guid;//always as second {{$object.context[1].value}}
                    cvars.Insert(0, cvar);
                    cvar = new ContextVariableDto();
                    cvar.value = hostname;//always on top {{$object.context[0].value}}
                    cvars.Insert(0, cvar);


                    wctx.context = cvars.ToArray();
                    String lnkParam = link.ALTERNATEBASISURL;
                    if (lnkParam == null || lnkParam.Length == 0)//use some default if nothing found
                        lnkParam = "w=1&s=:p01&v=Frontend&g=:p02";
                    String cmdLine = link.getParsedExpression(wctx, lnkParam);

                    rval = "jv:GUI.openLink(\"onex7:" + cmdLine + "\",1)";//g=" + guid + "&s=" + hostname + "&w=1&v="+Uri.EscapeDataString(link.ALTERNATEBASISURL)+"\")";
                    _log.Debug("Showing OL as Deeplink: " + rval);
                }
                else
                {
                    rval = "jv:GUI.showMessage(\"OL Aktion kann ausgeführt werden\")";
                }
                    
               
            }
            //when targettype=extern we have to call javascript in new tab
            else if (link.TARGETTYPE == (int)DeepLinkType.EXTERN )
            {
                String expr = link.getParsedExpression(wctx,link.PARAMEXPRESSION);
                String prefix = "";
                if (link.ALTERNATEBASISURL != null)
                {
                    String[] cfgs = link.ALTERNATEBASISURL.Split('/');
                    prefix = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(cfgs[1], cfgs[2], "", cfgs[0]);
                }
                
                rval = "jv:GUI.openLink(\"" + prefix + expr + "\")";
            }
            else if (link.TARGETTYPE == (int)DeepLinkType.IFRAME)
            {
                String expr = link.getParsedExpression(wctx, link.PARAMEXPRESSION);
                String prefix = "";
                if (link.ALTERNATEBASISURL != null)
                {
                    String[] cfgs = link.ALTERNATEBASISURL.Split('/');
                    prefix = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(cfgs[1], cfgs[2], "", cfgs[0]);
                }

                rval = prefix + expr;
            }
            else if (link.TARGETTYPE == (int)DeepLinkType.WEB)//Target OneWeb, we already originate from One Web else we wouldnt be here, so just force gui to show index.xhtml and execute some command
            {
                String expr = link.getParsedExpression(wctx);
                String prefix = "";
                if(link.ALTERNATEBASISURL==null)
                    link.ALTERNATEBASISURL = "SETUP/DEEPLINK/DEFAULTURL";
                if (link.ALTERNATEBASISURL != null)
                {
                    link.ALTERNATEBASISURL =  link.ALTERNATEBASISURL.Replace('\\','/');
                    String[] cfgs = link.ALTERNATEBASISURL.Split('/');
                    prefix = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(cfgs[1], cfgs[2], "", cfgs[0]);
                }
                int noframe = 0;
                if (link.CLIENTTYPE == (int)DeepLinkType.EXTERN && link.USEINTERNALVIEWER==1)
                    noframe=1;
                rval = "jv:GUI.openRedirectIntern(\"" + expr.Replace("\"","\\\"") + "\","+noframe+")";
                return rval;//no continuing here, we already destinate in cic one!
            }
            else rval = "jv:GUI.showMessage(\"Cannot open DeepLink: " + code + " has wrong targettype\")";

            if (continueWorkflow == 2)
                rval += ";jv:GUI.goInitsequence()";
            return rval;
        }
        catch (Exception e)
        {
            return "jv:GUI.showMessage(\"Cannot open DeepLink: " + e.Message + "\")";
        }
    }
    
    /// <summary>
    /// Launches a new BPE Workflow from GUI
    /// </summary>
    /// <param name="processDefCode"></param>
    /// <param name="eventCode"></param>
    /// <param name="area"></param>
    /// <param name="areaId"></param>
    /// <param name="wctx"></param>
    /// <returns></returns>
    public static String launchWF(String processDefCode, String eventCode, String area, long areaId, WorkflowContext wctx)
    {
        launchWF(processDefCode, eventCode, area, areaId, wctx.sysWFUSER,null);
        return "";
    }
    /// <summary>
    /// Launches a new Workflow
    /// </summary>
    /// <param name="processDefCode"></param>
    /// <param name="eventCode"></param>
    /// <param name="area"></param>
    /// <param name="areaId"></param>
    /// <param name="sysWfuser"></param>
    /// <returns>the sysbplistener, if a user-task was invoked, 0 otherwise</returns>
    public static long launchWF(String processDefCode, String eventCode, String area, long areaId,long sysWfuser, LookupVariableDto[] vars) 
    {

        IMediatorService svc = new Cic.OpenOne.Common.MediatorService.MediatorServiceClient();
        
        DispatchAndExecuteEventRequest request = new DispatchAndExecuteEventRequest();
        request.AreaName = area;
        request.AreaId = areaId;
        
        request.ProcessDefinitionCode = processDefCode;
        
        request.EventCode = eventCode;
        request.UserId = sysWfuser;
        request.ProcessContext = new SubscriptionsDataDto();
        request.ProcessContext.Subscriptions = new SubscriptionDto[0];
        request.ProcessContext.Queues = new QueueDto[0];
        if (vars == null) vars = new LookupVariableDto[0];
        request.ProcessContext.Variables =vars;
        
        try
        {
            ResponseBase svcRval = BPEWorkflowService.checkError(svc.Execute(request));
            _log.Debug("BPE Job " + processDefCode + " created with Startevent " + eventCode + " for " + area + " id: " + areaId);
            try
            {
                DispatchAndExecuteEventResponse resp = (DispatchAndExecuteEventResponse)svcRval;
                ListenerModel lm = resp.ExecutionReport.Listeners.Where(a => a.IsUserTask == true).FirstOrDefault();
                return lm.Id;
            }catch(Exception)
            {
                return 0;
            }
            
        }
        catch (Exception e)
        {
            _log.Error("BPE Job " + processDefCode + " start failed with Startevent " + eventCode + " for " + area + " id: " + areaId, e);
            throw new Exception("BPE Job " + processDefCode + " start failed with Startevent " + eventCode + " for " + area + " id: " + areaId);
        }
    }
    /// <summary>
    /// Tries to find a  listener for the process for the area/id for the eventCode
    /// </summary>
    /// <param name="processDefCode"></param>
    /// <param name="eventCode"></param>
    /// <param name="area"></param>
    /// <param name="areaId"></param>
    /// <param name="wctx"></param>
    /// <returns>1 for success</returns>
    public static long sendEvent(String processDefCode, String eventCode, String area, long areaId, WorkflowContext wctx) 
    {

        using (PrismaExtended context = new PrismaExtended())
        {
            List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = areaId });
            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "proccode", Value = processDefCode });
            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "event", Value = eventCode });
            long sysbplistener = context.ExecuteStoreQuery<long>("select sysbplistener from bplistener,bpprocdef where isusertask=1 and bplistener.eventcode=:event and  bplistener.sysbpprocdef=bpprocdef.sysbpprocdef and bplistener.oltable=:area and sysoltable=:sysid and bpprocdef.processdefcode=:proccode and (bplistener.supervisorflags>=16 or supervisorflags=0) order by bplistener.sysbplistener", pars.ToArray()).FirstOrDefault();
            if (sysbplistener > 0)
            {
                sendEvent(sysbplistener, wctx.sysWFUSER, new LookupVariableDto[0]);
                return 1;
            }
            return 0;

        }
    }

    /// <summary>
    /// Continues the listener with the given vars
    /// </summary>
    /// <param name="sysBpListener"></param>
    /// <param name="sysWfuser"></param>
    /// <param name="vars"></param>
    public static void sendEvent(long sysBpListener, long sysWfuser, LookupVariableDto[] vars) 
    {
        try
        {
            IMediatorService svc = new Cic.OpenOne.Common.MediatorService.MediatorServiceClient();

            GetProcessContextRequest gctxreq = new GetProcessContextRequest();
            gctxreq.ListenerId = sysBpListener;
            GetProcessContextResponse presp = (GetProcessContextResponse)BPEWorkflowService.checkError(svc.Execute(gctxreq));
            SubscriptionsDataDto context = presp.ProcessContext;
            context.Variables = vars;
            //update context
            //updateProcessContextVariables(input.workflowContext, context);

            long timestamp = 0;
            GetInstanceTimestampRequest reqtime = new GetInstanceTimestampRequest();
            reqtime.ProcessInstanceId = presp.ProcessInstanceId;
            //get current changestamp
            ResponseBase svcRval = BPEWorkflowService.checkError(svc.Execute(reqtime));
            if (svcRval is GetInstanceTimestampResponse)
            {
                GetInstanceTimestampResponse resp = (GetInstanceTimestampResponse)svcRval;
                timestamp = resp.InstanceTimestamp;
            }
            DispatchAndExecuteListenerRequest request = new DispatchAndExecuteListenerRequest();
            request.ListenerId = sysBpListener;
            request.UserId = sysWfuser;
            request.ProcessContext = context;
            request.OverwriteProcessContext = true;
            request.InstanceTimestamp = timestamp;

            //continue BPE
            svcRval = BPEWorkflowService.checkError(svc.Execute(request));
        }
        catch (Exception e)
        {
            _log.Error("BPE Job " + sysBpListener + " sendEvent failed", e);
            throw new Exception("BPE Job " + sysBpListener + " sendEvent failed", e);
        }
    }

    /// <summary>
    /// Fetches the given Queue  for the given ruleset
    /// </summary>
    /// <param name="sysid"></param>
    /// <param name="area"></param>
    /// <param name="queueName"></param>
    /// <param name="ruleSetCode"></param>
    /// <param name="vars"></param>
    /// <returns></returns>
    public static QueueResultDto getQueueData(long sysid, String area, String[] queueName, String ruleSetCode, CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars, long syswfuser)
    {

        Cic.OpenOne.Common.MediatorService.MediatorServiceClient svc = new Cic.OpenOne.Common.MediatorService.MediatorServiceClient();
        svc.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SOAPLoggingBehaviour());
        ExecuteRuleSetRequest req = new ExecuteRuleSetRequest();
        req.Area = area;
        req.AreaId = sysid;
        req.RuleSetCode = ruleSetCode;
        req.UserId = syswfuser;
        req.Context = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto();
        req.Context.Subscriptions = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto[queueName.Count()];
        int qnr=0;
        foreach(String queue in queueName)
        {
            req.Context.Subscriptions[qnr] = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto();
            req.Context.Subscriptions[qnr].ObjectName=queue;
            req.Context.Subscriptions[qnr++].ObjectType="Q";
        }
        req.Context.Variables = vars;
        
        try
        {
            ResponseBase svcRval = BPEWorkflowService.checkError(svc.Execute(req));
            if (svcRval is ExecuteRuleSetResponse)
            {
                ExecuteRuleSetResponse resp = (ExecuteRuleSetResponse)svcRval;
                QueueResultDto result = new QueueResultDto();
                result.queues = new List<Cic.One.DTO.Mediator.QueueDto>();
                foreach (CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto queue in resp.Context.Queues)
                {
                    result.queues.Add(Mapper.Map<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto, Cic.One.DTO.Mediator.QueueDto>(queue));
                }
                return result;
            }

        }
        catch (Exception e)
        {
            _log.Error("BPE getQueueData failed", e);
            throw new Exception("BPE getQueueData failed",e);
        }
        return null;
    }

    public static CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto getFormulaData(long sysid, String area, String formulaName, CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars, CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto[] subscriptions, CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto[] queues, long syswfuser)
    {

        Cic.OpenOne.Common.MediatorService.MediatorServiceClient svc = new Cic.OpenOne.Common.MediatorService.MediatorServiceClient();
        svc.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SOAPLoggingBehaviour());
        CIC.Bas.Modules.OpenLeaseCommon.ExposedApi.ExecuteFormulaRequest req = new CIC.Bas.Modules.OpenLeaseCommon.ExposedApi.ExecuteFormulaRequest();
        req.Area = area;
        req.AreaId = sysid;
        req.FormulaName = formulaName;
        req.Parameter1 = "p1";
        req.Parameter2 = "p2";
        req.Parameter3 = "p3";
        req.Parameter4 = "p4";
        req.Parameter5 = "p5";
        req.UserId = syswfuser;

        req.Context = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto();
        req.Context.Subscriptions = subscriptions;
        req.Context.Variables = vars;
        req.Context.Queues = queues;
        try
        {
            ResponseBase svcRval = BPEWorkflowService.checkError(svc.Execute(req));
            if (svcRval is CIC.Bas.Modules.OpenLeaseCommon.ExposedApi.ExecuteFormulaResponse)
            {
                CIC.Bas.Modules.OpenLeaseCommon.ExposedApi.ExecuteFormulaResponse resp = (CIC.Bas.Modules.OpenLeaseCommon.ExposedApi.ExecuteFormulaResponse)svcRval;
                return resp.Context;
            }

        }
        catch (Exception e)
        {
            _log.Error("BPE getFormulaData failed", e);
            throw new Exception("BPE getFormulaData failed", e);
        }
        return null;
    }

    /// <summary>
    /// Updates the BPE Queue
    /// </summary>
    /// <param name="sysid"></param>
    /// <param name="area"></param>
    /// <param name="queueName"></param>
    /// <param name="ruleSetCode"></param>
    /// <param name="queue"></param>
    /// <param name="vars"></param>
    /// <returns></returns>
    public static Cic.One.DTO.Mediator.QueueDto setQueueData(long sysid, String area, String queueName, String ruleSetCode, Cic.One.DTO.Mediator.QueueDto queue, CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars, long syswfuser)
    {

        IMediatorService svc = new Cic.OpenOne.Common.MediatorService.MediatorServiceClient();
        ExecuteRuleSetRequest req = new ExecuteRuleSetRequest();
        req.Area = area;
        req.AreaId = sysid;
        req.UserId = syswfuser;
        req.RuleSetCode = ruleSetCode;
        req.Context = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto();
        req.Context.Subscriptions = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto[1];
        req.Context.Subscriptions[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto();
        req.Context.Subscriptions[0].ObjectName = queueName;
        req.Context.Subscriptions[0].ObjectType = "Q";
        req.Context.Variables = vars;
        req.Context.Queues = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto[1];
        req.Context.Queues[0] = Mapper.Map<Cic.One.DTO.Mediator.QueueDto,CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto>(queue);
        try
        {
            ResponseBase svcRval = BPEWorkflowService.checkError(svc.Execute(req));
            if (svcRval is ExecuteRuleSetResponse)
            {
                ExecuteRuleSetResponse resp = (ExecuteRuleSetResponse)svcRval;
                CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto queueR = resp.Context.Queues[0];
                return Mapper.Map<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto, Cic.One.DTO.Mediator.QueueDto>(queueR);
            }

        }
        catch (Exception e)
        {
            _log.Error("BPE getQueueData failed", e);
            throw new Exception("BPE getQueueData failed", e);
        }
        return null;
    }


    /// <summary>
    /// DEPRECATED, now via wfv customization and type WORKFLOW, command is BOS.fire('xy')
    /// </summary>
    /// <param name="syscode"></param>
    /// <returns></returns>
    public static String launchWF(String syscode)
    {

        return "jv:GUI.launchWF(\"" + syscode + "\")";
    }

    public static String getAlerts(WorkflowContext wctx)
    {
        WorkflowDao wfd = new WorkflowDao();
        return wfd.getAlerts(wctx.sysWFUSER,wctx.isocode);
        
    }
}

