using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CIC.Bas.Modules.OpenBPE.ServiceAccess;
using CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto;
using CIC.Bas.Framework.Extensibility;
using CIC.Bas.Modules.OpenBPE.Storage.Models;
using Cic.OpenOne.Common.MediatorService;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using CIC.Bas.Modules.RuleEngine.Services;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Manages BPE access
    /// </summary>
    public class BPEBo
    {
        private static ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Starts a BPE Process
        /// </summary>
        /// <param name="processDefCode"></param>
        /// <param name="eventCode"></param>
        /// <param name="area"></param>
        /// <param name="areaId"></param>
        /// <param name="sysWfuser"></param>
        public static void createBPEProcess(String processDefCode, String eventCode, String area, long areaId, long sysWfuser)
        {
            //do nothing if not all codes given
            if (processDefCode == null || processDefCode.Trim().Length == 0
                || eventCode == null || eventCode.Trim().Length == 0
                )
                return;
            IMediatorService svc = new Cic.OpenOne.Common.MediatorService.MediatorServiceClient();
            var modifyHeadersBehavior = new ModifyHeadersBehavior();
            modifyHeadersBehavior.AddHeader("X-CIC-SYSWFUSER", "" + sysWfuser);
            ((Cic.OpenOne.Common.MediatorService.MediatorServiceClient)svc).Endpoint.Behaviors.Add(modifyHeadersBehavior);

            DispatchAndExecuteEventRequest request = new DispatchAndExecuteEventRequest();
            request.AreaName = area;
            request.AreaId = areaId;
            request.ProcessDefinitionCode = processDefCode.Trim();
            request.EventCode = eventCode;//CICeventname "START_WEB_PROCESS"
            request.UserId = sysWfuser;
            request.ProcessContext = new SubscriptionsDataDto();
            request.ProcessContext.Subscriptions = new SubscriptionDto[0];
            request.ProcessContext.Queues = new QueueDto[0];
            request.ProcessContext.Variables = new LookupVariableDto[0];
            //start BPE and set Context

            try
            {
                ResponseBase svcRval = checkError(svc.Execute(request));
                _log.Debug("BPE Job " + processDefCode + " created with Startevent " + eventCode + " for " + area + " id: " + areaId);
            }
            catch (Exception e)
            {
                _log.Error("BPE Job " + processDefCode + " start failed with Startevent " + eventCode + " for " + area + " id: " + areaId, e);
                throw new Exception("BPE Job " + processDefCode + " start failed with Startevent " + eventCode + " for " + area + " id: " + areaId + " MSG: " + e.Message);
            }

        }

        /// <summary>
        /// Check Error and return exception with message if error found
        /// </summary>
        /// <param name="resp"></param>
        private static ResponseBase checkError(ResponseBase resp)
        {
            if (resp is ErrorResponse)
            {
                ErrorResponse errInfo = (ErrorResponse)resp;
                throw new Exception(errInfo.Error);
            }
            return resp;
        }

        /// <summary>
        /// returns the value from record with given variablename
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String getQueueRecordValue(CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto rec, String id)
        {
            if (rec == null || rec.Values == null) return null;
            return (from f in rec.Values
                    where f.VariableName.Equals(id)
                    select f.Value).FirstOrDefault();
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
        public static List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> getQueueData(long sysid, String area, String[] queueName, String ruleSetCode, CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars, long syswfuser, List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> queueInput)
        {

            Cic.OpenOne.Common.MediatorService.MediatorServiceClient svc = new Cic.OpenOne.Common.MediatorService.MediatorServiceClient();
            var modifyHeadersBehavior = new ModifyHeadersBehavior();
            modifyHeadersBehavior.AddHeader("X-CIC-SYSWFUSER", "" + syswfuser);
            ((Cic.OpenOne.Common.MediatorService.MediatorServiceClient)svc).Endpoint.Behaviors.Add(modifyHeadersBehavior);


            svc.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SOAPLoggingBehaviour());
            ExecuteRuleSetRequest req = new ExecuteRuleSetRequest();
            req.Area = area;
            req.AreaId = sysid;
            req.RuleSetCode = ruleSetCode;
            req.UserId = syswfuser;
            req.Context = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto();
            req.Context.Variables = vars;


            if (!(queueInput == null || queueInput.Count == 0))
            {
                req.Context.Queues = queueInput.ToArray();
            }


            if (req.Context.Queues != null && req.Context.Queues.Count() > 0)
                queueName = (from f in req.Context.Queues
                             select f.Name).Distinct().ToArray();

            req.Context.Subscriptions = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto[queueName.Count()];
            int qnr = 0;

            foreach (String queue in queueName)
            {
                req.Context.Subscriptions[qnr] = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto();
                req.Context.Subscriptions[qnr].ObjectName = queue;
                req.Context.Subscriptions[qnr++].ObjectType = "Q";
            }


            try
            {
                ResponseBase svcRval = checkError(svc.Execute(req));
                if (svcRval is ExecuteRuleSetResponse)
                {
                    ExecuteRuleSetResponse resp = (ExecuteRuleSetResponse)svcRval;
                     
                    return resp.Context.Queues.ToList();
                }

            }
            catch (Exception e)
            {
                _log.Error("BPE getQueueData failed", e);
                throw new Exception("BPE getQueueData failed", e);
            }
            return null;
        }
    }

    public class BPEQueueRecord
    {

    }
    public class BPEQueue
    {
        private List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> queues = new List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto>();
        private List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto> qrecs = new List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto>();
        /*
         * List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> queues = new List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto>();
                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto qv = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto();
                    qv.Name = "qBUDGETIN";
                    queues.Add(qv);
                    queues.Add(new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto() { Name = "qBUDGETOUT" });
                    List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto> qrecs = new List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto>();

                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto rec = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto();
                    rec.Values = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto[4];
                    rec.Values[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    rec.Values[0].VariableName = "F01";
                    rec.Values[0].Value = prod.sysID.ToString();
                    rec.Values[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    rec.Values[1].VariableName = "F02";
                    rec.Values[1].Value = zinsen[0].ToString();
                    rec.Values[2] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    rec.Values[2].VariableName = "F03";
                    rec.Values[2].Value = lz.ToString();
                    rec.Values[3] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    rec.Values[3].VariableName = "F04";
                    rec.Values[3].Value = usekredit.ToString();
                    qrecs.Add(rec);

                    qv.Records = qrecs.ToArray();
         * */
        public BPEQueue()
        {

        }
        
        private Dictionary<String, List<BPEQueueRecordDto>> queueRecords = new Dictionary<string, List<BPEQueueRecordDto>>();

        public BPEQueueRecordDto addQueueRecord(String queueName)
        {
            if(!queueRecords.ContainsKey(queueName))
            {
                queueRecords[queueName] = new List<BPEQueueRecordDto>();
            }
            BPEQueueRecordDto rval = new BPEQueueRecordDto();
            queueRecords[queueName].Add(rval);
            return rval;
        }

        //bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_EINKOMMEN").addQueueRecordValue("F02", ""+input.budget1.pkz.einknetto);
        public void addQueue(String name)
        {
            queues.Add(new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto() { Name =name });
        }
        public List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> getQueues()
        {
            foreach(CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto queue in queues)
            {
                if (!queueRecords.ContainsKey(queue.Name)) continue;
                List<BPEQueueRecordDto> records = queueRecords[queue.Name];
                List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto> qrecdtos = new List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto>();
                foreach (BPEQueueRecordDto qrec in records)
                {
                    
                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto qrecord = qrec.buildRecord(); 
                    qrecdtos.Add(qrecord);
                }
                queue.Records = qrecdtos.ToArray();
            }
            return queues;
        }
    }
    public class BPEQueueRecordDto: CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto
    {
        private List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto> recvalues = new List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto>();
        public BPEQueueRecordDto()
        {

        }
        public BPEQueueRecordDto addQueueRecordValue(String name, String value)
        {

            CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto rec = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto();
            CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto recval = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
            recval.VariableName = name;
            recval.Value = value;
            recvalues.Add(recval);
            return this;
        }
        public CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto buildRecord()
        {
            CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto rval = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto();
            rval.Values = recvalues.ToArray();
            return rval;
        }

    }

    /// <summary>
    /// Allows to add additional headers to the WCF services.
    /// </summary>
    public class ModifyHeadersBehavior : IEndpointBehavior
    {
        private readonly ModifyHeadersInspector modifyHeadersInspector = new ModifyHeadersInspector();

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(this.modifyHeadersInspector);
        }

        public void AddHeader(string key, string value)
        {
            this.modifyHeadersInspector.AddHeader(key, value);
        }

        private class ModifyHeadersInspector : IClientMessageInspector
        {
            private readonly Dictionary<string, string> additionalHeaders = new Dictionary<string, string>();

            public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
            {
                HttpRequestMessageProperty httpRequestMessage;

                object httpRequestMessageObject;
                if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
                {
                    httpRequestMessage = (HttpRequestMessageProperty)httpRequestMessageObject;
                }
                else
                {
                    httpRequestMessage = new HttpRequestMessageProperty();
                    request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
                }

                foreach (var pair in this.additionalHeaders)
                {
                    httpRequestMessage.Headers[pair.Key] = pair.Value;
                }

                return null;
            }

            public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
            {
            }

            public void AddHeader(string key, string value)
            {
                this.additionalHeaders[key] = value;
            }
        }
    }
}