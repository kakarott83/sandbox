//IMPORTANT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//THIS WAS BUILD WITH
//svcutil https://bnr14dev1.cic.muc/BAS/MediatorService.svc

//the following namespaces then have to be commented out because they exist already in referenced dlls:
//namespace CIC.Bas.Framework.Evaluate
//namespace CIC.Monitoring.Model

//the serviceclient was encapsulated within the namespace
//namespace Cic.OpenOne.Common.MediatorService

//The ServiceContractAttribute ConfigurationName must be renamed to 
//[System.ServiceModel.ServiceContractAttribute(ConfigurationName = "MediatorService.IMediatorService")]
namespace CIC.Bas.Framework.Extensibility
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RequestBase", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.Extensibility")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Common.ResetCacheRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Framework.Extensibility.Requests.ResetCacheWithDelayRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.DocumentViewer.Requests.ShowDocumentViewerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.DocumentViewer.Requests.MailMergeRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.Localization.Model.GetLocalizationRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.Localization.Model.ShowTranslationManagerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.NT.Handlers.Vorgang.FireVorgangRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.NT.Handlers.Synchronization.GetSysWfTableRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.CASClient.CheckServerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.CASClient.EvaluateAllRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.CASClient.EvaluateRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.CASClient.Testing.TestRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.CASClient.Server.ServerResetCacheRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.Debugger.Requests.StartDebugServerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.Debugger.Requests.StopDebugServerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.RuleEngine.Services.ExecuteRuleSetRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.VotingRule.DesignVotingRuleRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.FlowRule.DesignFlowRuleRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.RuleEngine.Wpf.DesignExcelRuleRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.RuleEngine.Wpf.ExportRuleRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.RuleEngine.Wpf.ExportRuleSetRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.RuleEngine.Wpf.ImportRuleRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.RuleEngine.Wpf.ImportRuleSetRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.OpenWorkbookRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.AddNewWorkbookRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.AddNewSheetRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.RemoveSheetRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SelectSheetRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.GetSheetNamesRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.GetSheetNameRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetSheetNameRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.GetCellValueRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellValueRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellFormulaRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellTextRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellFromClipboardRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetRowValuesRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetColumnValuesRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetTextOrientationRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellStyleRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellFormatRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellBorderStyleRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellAlignmentRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellProtectionRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetHeaderRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetFooterRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetHeaderImageRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetFooterImageRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.AutoFitRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetSheetProtectionRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SaveAsRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.CloseRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.CleanUpProcessRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.DispatchAndExecuteEventCoreRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.DispatchAndExecuteListenerCoreRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.InitializeContextCoreRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.RestoreProcessRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.SaveContextCoreRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.Manager.ShowCaseStepsViewerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.Manager.ShowUserActionsWindowRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.Manager.ShowManagerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.Manager.ShowViewerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Printing.Model.GetAllPrintersRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Printing.Model.PrintRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.WebViewer.Requests.WebViewerCloseRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.WebViewer.Requests.WebViewerChangeUrlRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.WebViewer.Requests.WebViewerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.WebViewer.Requests.WebViewerCloseResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.SLA.PlusTimeRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.SLA.SLAChangeRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.SLA.SLACreateRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.SLA.TimeDiffRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Security.ShowLoginWindowRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Security.Permissions.PermitRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Security.Permissions.PermitExtendedRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.PdfViewer.Requests.PdfViewerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.DispatchAndExecuteEventRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.DispatchAndExecuteListenerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.GetInstanceTimestampRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.GetListenersRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.GetProcessContextRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.PermissionCheckRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.SearchListenersRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.SetProcessContextRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.KNEManager.ShowKNEManagerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.JobFlowDesigner.ShowJobFlowDesignerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.FormulaManager.ShowManagerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.EvaluateService.EvaluateRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.DocUniManager.ShowEditorRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.DocUniManager.ShowManagerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.DeepLinkService.DTO.DeepLinkServiceStartRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.DeepLinkService.DTO.DeepLinkServiceGetStateRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.DeepLinkService.DTO.DeepLinkServiceStopRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Iso8061Service.Iso8061Request))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.ScriptsManager.ShowManagerRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Query.Model.CustomizeQueryRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Query.Model.ShowQueryRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Payments.Model.ImportCamtRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Payments.Model.ExportPain008Request))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Payments.Model.ExportPain001Request))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.ExposedApi.ExecuteScriptRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.ExposedApi.ExecuteFormulaRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Evaluate.EvaluateFunctionsRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Evaluate.StopWatchRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Evaluate.DelayRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Evaluate.BindProceduresStatistics.GetBindProceduresStatisticsRequest))]
    public partial class RequestBase : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResponseBase", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.Extensibility")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Framework.Extensibility.SuccessResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Framework.Extensibility.ErrorResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Framework.Extensibility.ErrorResponseExtended))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Framework.Extensibility.Requests.ResetCacheWithDelayResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.DocumentViewer.Requests.ShowDocumentViewerResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.Localization.Model.GetLocalizationResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.CASClient.CheckServerResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.CASClient.EvaluateAllResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.CASClient.EvaluateResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.CASClient.Testing.TestResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.RuleEngine.Services.ExecuteRuleSetResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.RestoreProcessResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Printing.Model.GetAllPrintersResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.WebViewer.Requests.WebViewerResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.SLA.PlusTimeResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.SLA.SLAChangeResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.SLA.SLACreateResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.SLA.TimeDiffResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Security.Permissions.PermitResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.DispatchAndExecuteEventResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.DispatchAndExecuteListenerResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.GetInstanceTimestampResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.GetListenersResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.GetProcessContextResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.PermissionCheckResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.ServiceAccess.SearchListenersResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.EvaluateService.EvaluateResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.DeepLinkService.DTO.DeepLinkServiceStartResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.DeepLinkService.DTO.DeepLinkServiceGetStateResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.DeepLinkService.DTO.DeepLinkServiceStopResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Iso8061Service.Iso8061Response))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.ExposedApi.ExecuteScriptResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.ExposedApi.ExecuteFormulaResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Evaluate.EvaluateFunctionsResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Evaluate.StopWatchResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Evaluate.DelayResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Evaluate.BindProceduresStatistics.GetBindProcedureStatisticsResponse))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.PersonBrowser.CustomizationWindowResponse))]
    public partial class ResponseBase : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SuccessResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.Extensibility")]
    public partial class SuccessResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string SuccessField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Success
        {
            get
            {
                return this.SuccessField;
            }
            set
            {
                this.SuccessField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ErrorResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.Extensibility")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Framework.Extensibility.ErrorResponseExtended))]
    public partial class ErrorResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string ErrorField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Error
        {
            get
            {
                return this.ErrorField;
            }
            set
            {
                this.ErrorField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ErrorResponseExtended", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.Extensibility")]
    public partial class ErrorResponseExtended : CIC.Bas.Framework.Extensibility.ErrorResponse
    {
        
        private string FullExceptionField;
        
        private bool LogoutField;
        
        private string MessageField;
        
        private string StackTraceField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FullException
        {
            get
            {
                return this.FullExceptionField;
            }
            set
            {
                this.FullExceptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Logout
        {
            get
            {
                return this.LogoutField;
            }
            set
            {
                this.LogoutField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message
        {
            get
            {
                return this.MessageField;
            }
            set
            {
                this.MessageField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StackTrace
        {
            get
            {
                return this.StackTraceField;
            }
            set
            {
                this.StackTraceField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.Common
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResetCacheRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Common")]
    public partial class ResetCacheRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
}
namespace CIC.Bas.Framework.Extensibility.Requests
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResetCacheWithDelayRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.Extensibility.Requests")]
    public partial class ResetCacheWithDelayRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResetCacheWithDelayResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.Extensibility.Requests")]
    public partial class ResetCacheWithDelayResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string MessageField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message
        {
            get
            {
                return this.MessageField;
            }
            set
            {
                this.MessageField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.DocumentViewer.Requests
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowDocumentViewerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.DocumentViewer.Requests")]
    public partial class ShowDocumentViewerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string[] AllowedAreasField;
        
        private string[] AllowedFontsField;
        
        private bool CreateNewDocumentField;
        
        private string CsvDelimiterField;
        
        private string CsvEncodingField;
        
        private string CsvMailmergePathField;
        
        private string DocumentPathField;
        
        private int MaxDocumentPagesField;
        
        private bool OpenInMdiField;
        
        private bool OpenProtectedViewField;
        
        private string SessionIdField;
        
        private string TitleField;
        
        private string WindowTypeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] AllowedAreas
        {
            get
            {
                return this.AllowedAreasField;
            }
            set
            {
                this.AllowedAreasField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] AllowedFonts
        {
            get
            {
                return this.AllowedFontsField;
            }
            set
            {
                this.AllowedFontsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool CreateNewDocument
        {
            get
            {
                return this.CreateNewDocumentField;
            }
            set
            {
                this.CreateNewDocumentField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CsvDelimiter
        {
            get
            {
                return this.CsvDelimiterField;
            }
            set
            {
                this.CsvDelimiterField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CsvEncoding
        {
            get
            {
                return this.CsvEncodingField;
            }
            set
            {
                this.CsvEncodingField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CsvMailmergePath
        {
            get
            {
                return this.CsvMailmergePathField;
            }
            set
            {
                this.CsvMailmergePathField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DocumentPath
        {
            get
            {
                return this.DocumentPathField;
            }
            set
            {
                this.DocumentPathField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int MaxDocumentPages
        {
            get
            {
                return this.MaxDocumentPagesField;
            }
            set
            {
                this.MaxDocumentPagesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool OpenInMdi
        {
            get
            {
                return this.OpenInMdiField;
            }
            set
            {
                this.OpenInMdiField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool OpenProtectedView
        {
            get
            {
                return this.OpenProtectedViewField;
            }
            set
            {
                this.OpenProtectedViewField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SessionId
        {
            get
            {
                return this.SessionIdField;
            }
            set
            {
                this.SessionIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Title
        {
            get
            {
                return this.TitleField;
            }
            set
            {
                this.TitleField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WindowType
        {
            get
            {
                return this.WindowTypeField;
            }
            set
            {
                this.WindowTypeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MailMergeRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.DocumentViewer.Requests")]
    public partial class MailMergeRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string CsvDelimiterField;
        
        private string CsvEncodingField;
        
        private string CsvMailmergePathField;
        
        private string DocumentExportPathField;
        
        private string DocumentPathField;
        
        private bool ExportPdfAsPdfAField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CsvDelimiter
        {
            get
            {
                return this.CsvDelimiterField;
            }
            set
            {
                this.CsvDelimiterField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CsvEncoding
        {
            get
            {
                return this.CsvEncodingField;
            }
            set
            {
                this.CsvEncodingField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CsvMailmergePath
        {
            get
            {
                return this.CsvMailmergePathField;
            }
            set
            {
                this.CsvMailmergePathField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DocumentExportPath
        {
            get
            {
                return this.DocumentExportPathField;
            }
            set
            {
                this.DocumentExportPathField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DocumentPath
        {
            get
            {
                return this.DocumentPathField;
            }
            set
            {
                this.DocumentPathField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ExportPdfAsPdfA
        {
            get
            {
                return this.ExportPdfAsPdfAField;
            }
            set
            {
                this.ExportPdfAsPdfAField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowDocumentViewerResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.DocumentViewer.Requests")]
    public partial class ShowDocumentViewerResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string[] AreasField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Areas
        {
            get
            {
                return this.AreasField;
            }
            set
            {
                this.AreasField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.Localization.Model
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetLocalizationRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.Localization.Model")]
    public partial class GetLocalizationRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string ContextField;
        
        private string TextField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Context
        {
            get
            {
                return this.ContextField;
            }
            set
            {
                this.ContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Text
        {
            get
            {
                return this.TextField;
            }
            set
            {
                this.TextField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowTranslationManagerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.Localization.Model")]
    public partial class ShowTranslationManagerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetLocalizationResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.Localization.Model")]
    public partial class GetLocalizationResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string LocalizedTextField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LocalizedText
        {
            get
            {
                return this.LocalizedTextField;
            }
            set
            {
                this.LocalizedTextField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.NT.Handlers.Vorgang
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FireVorgangRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.NT.Handlers.Vorgang")]
    public partial class FireVorgangRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto ContextField;
        
        private string FireAreaField;
        
        private long FireAreaIdField;
        
        private string SyncAreaField;
        
        private long SyncAreaIdField;
        
        private long SysWfUserField;
        
        private System.Nullable<long> SysWfgField;
        
        private long SysWfvField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto Context
        {
            get
            {
                return this.ContextField;
            }
            set
            {
                this.ContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FireArea
        {
            get
            {
                return this.FireAreaField;
            }
            set
            {
                this.FireAreaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long FireAreaId
        {
            get
            {
                return this.FireAreaIdField;
            }
            set
            {
                this.FireAreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SyncArea
        {
            get
            {
                return this.SyncAreaField;
            }
            set
            {
                this.SyncAreaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long SyncAreaId
        {
            get
            {
                return this.SyncAreaIdField;
            }
            set
            {
                this.SyncAreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long SysWfUser
        {
            get
            {
                return this.SysWfUserField;
            }
            set
            {
                this.SysWfUserField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> SysWfg
        {
            get
            {
                return this.SysWfgField;
            }
            set
            {
                this.SysWfgField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long SysWfv
        {
            get
            {
                return this.SysWfvField;
            }
            set
            {
                this.SysWfvField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.NT.Handlers.Synchronization
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetSysWfTableRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.NT.Handlers.Synchronizati" +
        "on")]
    public partial class GetSysWfTableRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string CodeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Code
        {
            get
            {
                return this.CodeField;
            }
            set
            {
                this.CodeField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.CASClient
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CheckServerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.CASClient")]
    public partial class CheckServerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string HostField;
        
        private int PortField;
        
        private long UserIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Host
        {
            get
            {
                return this.HostField;
            }
            set
            {
                this.HostField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Port
        {
            get
            {
                return this.PortField;
            }
            set
            {
                this.PortField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EvaluateAllRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.CASClient")]
    public partial class EvaluateAllRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private CIC.Bas.Modules.CASClient.EvaluateRequest RequestField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.CASClient.EvaluateRequest Request
        {
            get
            {
                return this.RequestField;
            }
            set
            {
                this.RequestField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EvaluateRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.CASClient")]
    public partial class EvaluateRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string AreaField;
        
        private long[] AreaIdsField;
        
        private string[] ExpressionsField;
        
        private long MandantField;
        
        private long ProcessInstanceIdField;
        
        private long UserIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Area
        {
            get
            {
                return this.AreaField;
            }
            set
            {
                this.AreaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long[] AreaIds
        {
            get
            {
                return this.AreaIdsField;
            }
            set
            {
                this.AreaIdsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Expressions
        {
            get
            {
                return this.ExpressionsField;
            }
            set
            {
                this.ExpressionsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Mandant
        {
            get
            {
                return this.MandantField;
            }
            set
            {
                this.MandantField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ProcessInstanceId
        {
            get
            {
                return this.ProcessInstanceIdField;
            }
            set
            {
                this.ProcessInstanceIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CheckServerResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.CASClient")]
    public partial class CheckServerResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private CIC.Bas.Modules.CASClient.CheckServerResult ResultField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.CASClient.CheckServerResult Result
        {
            get
            {
                return this.ResultField;
            }
            set
            {
                this.ResultField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EvaluateAllResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.CASClient")]
    public partial class EvaluateAllResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private CIC.Bas.Modules.CASClient.EvaluateResponse[] ResponsesField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.CASClient.EvaluateResponse[] Responses
        {
            get
            {
                return this.ResponsesField;
            }
            set
            {
                this.ResponsesField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EvaluateResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.CASClient")]
    public partial class EvaluateResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private CIC.Bas.Modules.CASClient.AreaResult[] AreaResultsField;
        
        private string ReturnCodeField;
        
        private string ReturnMessageField;
        
        private long TransactionIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.CASClient.AreaResult[] AreaResults
        {
            get
            {
                return this.AreaResultsField;
            }
            set
            {
                this.AreaResultsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ReturnCode
        {
            get
            {
                return this.ReturnCodeField;
            }
            set
            {
                this.ReturnCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ReturnMessage
        {
            get
            {
                return this.ReturnMessageField;
            }
            set
            {
                this.ReturnMessageField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long TransactionId
        {
            get
            {
                return this.TransactionIdField;
            }
            set
            {
                this.TransactionIdField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CheckServerResult", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.CASClient")]
    public enum CheckServerResult : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Busy = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotExists = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EvaluateSuccess = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EvaluateFail = 3,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AreaResult", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.CASClient")]
    public partial class AreaResult : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private long AreaIdField;
        
        private string[] ResultsField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long AreaId
        {
            get
            {
                return this.AreaIdField;
            }
            set
            {
                this.AreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Results
        {
            get
            {
                return this.ResultsField;
            }
            set
            {
                this.ResultsField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.CASClient.Testing
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TestRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.CASClient.Testing")]
    public partial class TestRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private int[] NumbersField;
        
        private string[] StringsField;
        
        private bool ThrowExceptionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int[] Numbers
        {
            get
            {
                return this.NumbersField;
            }
            set
            {
                this.NumbersField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Strings
        {
            get
            {
                return this.StringsField;
            }
            set
            {
                this.StringsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ThrowException
        {
            get
            {
                return this.ThrowExceptionField;
            }
            set
            {
                this.ThrowExceptionField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TestResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.CASClient.Testing")]
    public partial class TestResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private int[] NumbersField;
        
        private string[] StringsField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int[] Numbers
        {
            get
            {
                return this.NumbersField;
            }
            set
            {
                this.NumbersField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Strings
        {
            get
            {
                return this.StringsField;
            }
            set
            {
                this.StringsField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.CASClient.Server
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServerResetCacheRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.CASClient.Server")]
    public partial class ServerResetCacheRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
}
namespace CIC.Bas.Modules.Debugger.Requests
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StartDebugServerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.Debugger.Requests")]
    public partial class StartDebugServerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string UrlField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Url
        {
            get
            {
                return this.UrlField;
            }
            set
            {
                this.UrlField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StopDebugServerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.Debugger.Requests")]
    public partial class StopDebugServerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
}
namespace CIC.Bas.Modules.RuleEngine.Services
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ExecuteRuleSetRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.RuleEngine.Services")]
    public partial class ExecuteRuleSetRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string AreaField;
        
        private long AreaIdField;
        
        private CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto ContextField;
        
        private string RuleSetCodeField;
        
        private long UserIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Area
        {
            get
            {
                return this.AreaField;
            }
            set
            {
                this.AreaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long AreaId
        {
            get
            {
                return this.AreaIdField;
            }
            set
            {
                this.AreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto Context
        {
            get
            {
                return this.ContextField;
            }
            set
            {
                this.ContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RuleSetCode
        {
            get
            {
                return this.RuleSetCodeField;
            }
            set
            {
                this.RuleSetCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ExecuteRuleSetResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.RuleEngine.Services")]
    public partial class ExecuteRuleSetResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto ContextField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto Context
        {
            get
            {
                return this.ContextField;
            }
            set
            {
                this.ContextField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.VotingRule
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DesignVotingRuleRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.VotingRule")]
    public partial class DesignVotingRuleRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string FactsField;
        
        private long RuleIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Facts
        {
            get
            {
                return this.FactsField;
            }
            set
            {
                this.FactsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long RuleId
        {
            get
            {
                return this.RuleIdField;
            }
            set
            {
                this.RuleIdField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.FlowRule
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DesignFlowRuleRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.FlowRule")]
    public partial class DesignFlowRuleRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long RuleIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long RuleId
        {
            get
            {
                return this.RuleIdField;
            }
            set
            {
                this.RuleIdField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.RuleEngine.Wpf
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DesignExcelRuleRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.RuleEngine.Wpf")]
    public partial class DesignExcelRuleRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long RuleIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long RuleId
        {
            get
            {
                return this.RuleIdField;
            }
            set
            {
                this.RuleIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ExportRuleRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.RuleEngine.Wpf")]
    public partial class ExportRuleRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long RuleIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long RuleId
        {
            get
            {
                return this.RuleIdField;
            }
            set
            {
                this.RuleIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ExportRuleSetRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.RuleEngine.Wpf")]
    public partial class ExportRuleSetRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long RuleSetIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long RuleSetId
        {
            get
            {
                return this.RuleSetIdField;
            }
            set
            {
                this.RuleSetIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ImportRuleRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.RuleEngine.Wpf")]
    public partial class ImportRuleRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ImportRuleSetRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.RuleEngine.Wpf")]
    public partial class ImportRuleSetRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
}
namespace CIC.Bas.Modules.SpreadsheetProcessing.Requests
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="OpenWorkbookRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class OpenWorkbookRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string WorkbookPathField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WorkbookPath
        {
            get
            {
                return this.WorkbookPathField;
            }
            set
            {
                this.WorkbookPathField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AddNewWorkbookRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class AddNewWorkbookRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SessionRequestBase", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.AddNewSheetRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.RemoveSheetRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SelectSheetRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.GetSheetNamesRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.GetSheetNameRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetSheetNameRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.GetCellValueRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellValueRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellFormulaRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellTextRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellFromClipboardRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetRowValuesRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetColumnValuesRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetTextOrientationRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellStyleRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellFormatRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellBorderStyleRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellAlignmentRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellProtectionRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetHeaderRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetFooterRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetHeaderImageRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetFooterImageRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.AutoFitRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetSheetProtectionRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SaveAsRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.CloseRequest))]
    public partial class SessionRequestBase : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string SessionIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SessionId
        {
            get
            {
                return this.SessionIdField;
            }
            set
            {
                this.SessionIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AddNewSheetRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class AddNewSheetRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private string SheetNameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SheetName
        {
            get
            {
                return this.SheetNameField;
            }
            set
            {
                this.SheetNameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RemoveSheetRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class RemoveSheetRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private int IndexField;
        
        private string SheetNameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Index
        {
            get
            {
                return this.IndexField;
            }
            set
            {
                this.IndexField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SheetName
        {
            get
            {
                return this.SheetNameField;
            }
            set
            {
                this.SheetNameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SelectSheetRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SelectSheetRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private int IndexField;
        
        private string SheetNameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Index
        {
            get
            {
                return this.IndexField;
            }
            set
            {
                this.IndexField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SheetName
        {
            get
            {
                return this.SheetNameField;
            }
            set
            {
                this.SheetNameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetSheetNamesRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class GetSheetNamesRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetSheetNameRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class GetSheetNameRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetSheetNameRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetSheetNameRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private string SheetNameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SheetName
        {
            get
            {
                return this.SheetNameField;
            }
            set
            {
                this.SheetNameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CellRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.GetCellValueRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellValueRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellFormulaRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellTextRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellFromClipboardRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetRowValuesRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetColumnValuesRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetTextOrientationRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellStyleRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellFormatRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellBorderStyleRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellAlignmentRequest))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.SpreadsheetProcessing.Requests.SetCellProtectionRequest))]
    public partial class CellRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private CIC.Bas.Modules.SpreadsheetProcessing.Model.CellIndex CellIndexField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.SpreadsheetProcessing.Model.CellIndex CellIndex
        {
            get
            {
                return this.CellIndexField;
            }
            set
            {
                this.CellIndexField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetCellValueRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class GetCellValueRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
        
        private CIC.Bas.Modules.SpreadsheetProcessing.Model.CellReturnValue CellReturnValueField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.SpreadsheetProcessing.Model.CellReturnValue CellReturnValue
        {
            get
            {
                return this.CellReturnValueField;
            }
            set
            {
                this.CellReturnValueField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetCellValueRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetCellValueRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
        
        private string FormatField;
        
        private string ValueField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Format
        {
            get
            {
                return this.FormatField;
            }
            set
            {
                this.FormatField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value
        {
            get
            {
                return this.ValueField;
            }
            set
            {
                this.ValueField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetCellFormulaRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetCellFormulaRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
        
        private string FormulaField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Formula
        {
            get
            {
                return this.FormulaField;
            }
            set
            {
                this.FormulaField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetCellTextRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetCellTextRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
        
        private string TextField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Text
        {
            get
            {
                return this.TextField;
            }
            set
            {
                this.TextField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetCellFromClipboardRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetCellFromClipboardRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetRowValuesRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetRowValuesRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
        
        private char DelimiterField;
        
        private string ValuesField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public char Delimiter
        {
            get
            {
                return this.DelimiterField;
            }
            set
            {
                this.DelimiterField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Values
        {
            get
            {
                return this.ValuesField;
            }
            set
            {
                this.ValuesField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetColumnValuesRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetColumnValuesRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
        
        private char DelimiterField;
        
        private string ValuesField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public char Delimiter
        {
            get
            {
                return this.DelimiterField;
            }
            set
            {
                this.DelimiterField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Values
        {
            get
            {
                return this.ValuesField;
            }
            set
            {
                this.ValuesField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetTextOrientationRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetTextOrientationRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
        
        private int RotationField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Rotation
        {
            get
            {
                return this.RotationField;
            }
            set
            {
                this.RotationField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetCellStyleRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetCellStyleRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
        
        private string BackgroundColorField;
        
        private char DelimiterField;
        
        private string FontFamilyField;
        
        private double FontSizeField;
        
        private CIC.Bas.Modules.SpreadsheetProcessing.Services.CellFontStyle FontStyleField;
        
        private string ForegroundColorField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BackgroundColor
        {
            get
            {
                return this.BackgroundColorField;
            }
            set
            {
                this.BackgroundColorField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public char Delimiter
        {
            get
            {
                return this.DelimiterField;
            }
            set
            {
                this.DelimiterField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FontFamily
        {
            get
            {
                return this.FontFamilyField;
            }
            set
            {
                this.FontFamilyField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double FontSize
        {
            get
            {
                return this.FontSizeField;
            }
            set
            {
                this.FontSizeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.SpreadsheetProcessing.Services.CellFontStyle FontStyle
        {
            get
            {
                return this.FontStyleField;
            }
            set
            {
                this.FontStyleField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ForegroundColor
        {
            get
            {
                return this.ForegroundColorField;
            }
            set
            {
                this.ForegroundColorField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetCellFormatRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetCellFormatRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
        
        private string FormatField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Format
        {
            get
            {
                return this.FormatField;
            }
            set
            {
                this.FormatField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetCellBorderStyleRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetCellBorderStyleRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
        
        private DevExpress.Spreadsheet.BorderLineStyle BorderLineStyleField;
        
        private CIC.Bas.Modules.SpreadsheetProcessing.Services.CellBorderOrientation BorderOrientationField;
        
        private string CellBorderColorField;
        
        private char DelimiterField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public DevExpress.Spreadsheet.BorderLineStyle BorderLineStyle
        {
            get
            {
                return this.BorderLineStyleField;
            }
            set
            {
                this.BorderLineStyleField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.SpreadsheetProcessing.Services.CellBorderOrientation BorderOrientation
        {
            get
            {
                return this.BorderOrientationField;
            }
            set
            {
                this.BorderOrientationField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CellBorderColor
        {
            get
            {
                return this.CellBorderColorField;
            }
            set
            {
                this.CellBorderColorField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public char Delimiter
        {
            get
            {
                return this.DelimiterField;
            }
            set
            {
                this.DelimiterField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetCellAlignmentRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetCellAlignmentRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
        
        private DevExpress.Spreadsheet.SpreadsheetHorizontalAlignment CellHorizontalAlignmentField;
        
        private DevExpress.Spreadsheet.SpreadsheetVerticalAlignment CellVerticalAlignmentField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public DevExpress.Spreadsheet.SpreadsheetHorizontalAlignment CellHorizontalAlignment
        {
            get
            {
                return this.CellHorizontalAlignmentField;
            }
            set
            {
                this.CellHorizontalAlignmentField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public DevExpress.Spreadsheet.SpreadsheetVerticalAlignment CellVerticalAlignment
        {
            get
            {
                return this.CellVerticalAlignmentField;
            }
            set
            {
                this.CellVerticalAlignmentField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetCellProtectionRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetCellProtectionRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.CellRequest
    {
        
        private int LockedField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Locked
        {
            get
            {
                return this.LockedField;
            }
            set
            {
                this.LockedField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetHeaderRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetHeaderRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private CIC.Bas.Modules.SpreadsheetProcessing.Model.SpreadsheetSection SheetSectionField;
        
        private string TextField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.SpreadsheetProcessing.Model.SpreadsheetSection SheetSection
        {
            get
            {
                return this.SheetSectionField;
            }
            set
            {
                this.SheetSectionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Text
        {
            get
            {
                return this.TextField;
            }
            set
            {
                this.TextField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetFooterRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetFooterRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private CIC.Bas.Modules.SpreadsheetProcessing.Model.SpreadsheetSection SheetSectionField;
        
        private string TextField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.SpreadsheetProcessing.Model.SpreadsheetSection SheetSection
        {
            get
            {
                return this.SheetSectionField;
            }
            set
            {
                this.SheetSectionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Text
        {
            get
            {
                return this.TextField;
            }
            set
            {
                this.TextField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetHeaderImageRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetHeaderImageRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private string ImagePathField;
        
        private CIC.Bas.Modules.SpreadsheetProcessing.Model.SpreadsheetSection SheetSectionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ImagePath
        {
            get
            {
                return this.ImagePathField;
            }
            set
            {
                this.ImagePathField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.SpreadsheetProcessing.Model.SpreadsheetSection SheetSection
        {
            get
            {
                return this.SheetSectionField;
            }
            set
            {
                this.SheetSectionField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetFooterImageRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetFooterImageRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private string ImagePathField;
        
        private CIC.Bas.Modules.SpreadsheetProcessing.Model.SpreadsheetSection SheetSectionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ImagePath
        {
            get
            {
                return this.ImagePathField;
            }
            set
            {
                this.ImagePathField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.SpreadsheetProcessing.Model.SpreadsheetSection SheetSection
        {
            get
            {
                return this.SheetSectionField;
            }
            set
            {
                this.SheetSectionField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AutoFitRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class AutoFitRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private CIC.Bas.Modules.SpreadsheetProcessing.Model.AutoFitOrientation AutoFitOrientationField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.SpreadsheetProcessing.Model.AutoFitOrientation AutoFitOrientation
        {
            get
            {
                return this.AutoFitOrientationField;
            }
            set
            {
                this.AutoFitOrientationField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetSheetProtectionRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SetSheetProtectionRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private int LockedField;
        
        private string PasswordField;
        
        private CIC.Bas.Modules.SpreadsheetProcessing.Services.CellProtectOptions ProtectionOptionsField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Locked
        {
            get
            {
                return this.LockedField;
            }
            set
            {
                this.LockedField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Password
        {
            get
            {
                return this.PasswordField;
            }
            set
            {
                this.PasswordField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.SpreadsheetProcessing.Services.CellProtectOptions ProtectionOptions
        {
            get
            {
                return this.ProtectionOptionsField;
            }
            set
            {
                this.ProtectionOptionsField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SaveAsRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class SaveAsRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private string WorkbookPathField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WorkbookPath
        {
            get
            {
                return this.WorkbookPathField;
            }
            set
            {
                this.WorkbookPathField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CloseRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Req" +
        "uests")]
    public partial class CloseRequest : CIC.Bas.Modules.SpreadsheetProcessing.Requests.SessionRequestBase
    {
        
        private System.Nullable<bool> SaveField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<bool> Save
        {
            get
            {
                return this.SaveField;
            }
            set
            {
                this.SaveField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenBPE
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CleanUpProcessRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE")]
    public partial class CleanUpProcessRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long ProcessInstanceIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ProcessInstanceId
        {
            get
            {
                return this.ProcessInstanceIdField;
            }
            set
            {
                this.ProcessInstanceIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DispatchAndExecuteEventCoreRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE")]
    public partial class DispatchAndExecuteEventCoreRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long EventIdField;
        
        private long UserIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long EventId
        {
            get
            {
                return this.EventIdField;
            }
            set
            {
                this.EventIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DispatchAndExecuteListenerCoreRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE")]
    public partial class DispatchAndExecuteListenerCoreRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private System.Nullable<long> InstanceTimestampField;
        
        private long ListenerIdField;
        
        private long UserIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> InstanceTimestamp
        {
            get
            {
                return this.InstanceTimestampField;
            }
            set
            {
                this.InstanceTimestampField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ListenerId
        {
            get
            {
                return this.ListenerIdField;
            }
            set
            {
                this.ListenerIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="InitializeContextCoreRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE")]
    public partial class InitializeContextCoreRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long ProcessInstanceIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ProcessInstanceId
        {
            get
            {
                return this.ProcessInstanceIdField;
            }
            set
            {
                this.ProcessInstanceIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RestoreProcessRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE")]
    public partial class RestoreProcessRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long ProcessInstanceIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ProcessInstanceId
        {
            get
            {
                return this.ProcessInstanceIdField;
            }
            set
            {
                this.ProcessInstanceIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SaveContextCoreRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE")]
    public partial class SaveContextCoreRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long ProcessInstanceIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ProcessInstanceId
        {
            get
            {
                return this.ProcessInstanceIdField;
            }
            set
            {
                this.ProcessInstanceIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RestoreProcessResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE")]
    public partial class RestoreProcessResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private System.Nullable<long> RestoreListenerIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> RestoreListenerId
        {
            get
            {
                return this.RestoreListenerIdField;
            }
            set
            {
                this.RestoreListenerIdField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenBPE.Manager
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowCaseStepsViewerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.Manager")]
    public partial class ShowCaseStepsViewerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string AreaField;
        
        private System.Nullable<long> AreaIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Area
        {
            get
            {
                return this.AreaField;
            }
            set
            {
                this.AreaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> AreaId
        {
            get
            {
                return this.AreaIdField;
            }
            set
            {
                this.AreaIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowUserActionsWindowRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.Manager")]
    public partial class ShowUserActionsWindowRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private CIC.Bas.Modules.OpenBPE.Actions.UserAction ActionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenBPE.Actions.UserAction Action
        {
            get
            {
                return this.ActionField;
            }
            set
            {
                this.ActionField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowManagerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.Manager")]
    public partial class ShowManagerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowViewerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.Manager")]
    public partial class ShowViewerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private System.Nullable<long> ProcessDefinitionIdField;
        
        private System.Nullable<long> ProcessInstanceIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> ProcessDefinitionId
        {
            get
            {
                return this.ProcessDefinitionIdField;
            }
            set
            {
                this.ProcessDefinitionIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> ProcessInstanceId
        {
            get
            {
                return this.ProcessInstanceIdField;
            }
            set
            {
                this.ProcessInstanceIdField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.Printing.Model
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetAllPrintersRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Printing." +
        "Model")]
    public partial class GetAllPrintersRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PrintRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Printing." +
        "Model")]
    public partial class PrintRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private int CopiesField;
        
        private string DocumentPathField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.Printing.Model.PrinterDescription[] PrintersField;
        
        private bool ShowDialogField;
        
        private bool UseDefaultPrinterField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Copies
        {
            get
            {
                return this.CopiesField;
            }
            set
            {
                this.CopiesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DocumentPath
        {
            get
            {
                return this.DocumentPathField;
            }
            set
            {
                this.DocumentPathField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Printing.Model.PrinterDescription[] Printers
        {
            get
            {
                return this.PrintersField;
            }
            set
            {
                this.PrintersField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ShowDialog
        {
            get
            {
                return this.ShowDialogField;
            }
            set
            {
                this.ShowDialogField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool UseDefaultPrinter
        {
            get
            {
                return this.UseDefaultPrinterField;
            }
            set
            {
                this.UseDefaultPrinterField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PrinterDescription", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Printing." +
        "Model")]
    public partial class PrinterDescription : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string NameField;
        
        private System.Nullable<int> PageFromField;
        
        private System.Nullable<int> PageToField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> PageFrom
        {
            get
            {
                return this.PageFromField;
            }
            set
            {
                this.PageFromField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> PageTo
        {
            get
            {
                return this.PageToField;
            }
            set
            {
                this.PageToField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetAllPrintersResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Printing." +
        "Model")]
    public partial class GetAllPrintersResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string[] PrinterNamesField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] PrinterNames
        {
            get
            {
                return this.PrinterNamesField;
            }
            set
            {
                this.PrinterNamesField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.WebViewer.Requests
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WebViewerCloseRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.WebViewer" +
        ".Requests")]
    public partial class WebViewerCloseRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string SessionIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SessionId
        {
            get
            {
                return this.SessionIdField;
            }
            set
            {
                this.SessionIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WebViewerChangeUrlRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.WebViewer" +
        ".Requests")]
    public partial class WebViewerChangeUrlRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string SessionIdField;
        
        private string UrlField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SessionId
        {
            get
            {
                return this.SessionIdField;
            }
            set
            {
                this.SessionIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Url
        {
            get
            {
                return this.UrlField;
            }
            set
            {
                this.UrlField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WebViewerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.WebViewer" +
        ".Requests")]
    public partial class WebViewerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private bool OpenInMdiField;
        
        private string SessionIdField;
        
        private string TitleField;
        
        private string UrlField;
        
        private string WindowTypeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool OpenInMdi
        {
            get
            {
                return this.OpenInMdiField;
            }
            set
            {
                this.OpenInMdiField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SessionId
        {
            get
            {
                return this.SessionIdField;
            }
            set
            {
                this.SessionIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Title
        {
            get
            {
                return this.TitleField;
            }
            set
            {
                this.TitleField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Url
        {
            get
            {
                return this.UrlField;
            }
            set
            {
                this.UrlField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WindowType
        {
            get
            {
                return this.WindowTypeField;
            }
            set
            {
                this.WindowTypeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WebViewerCloseResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.WebViewer" +
        ".Requests")]
    public partial class WebViewerCloseResponse : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string OpenUrlField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OpenUrl
        {
            get
            {
                return this.OpenUrlField;
            }
            set
            {
                this.OpenUrlField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WebViewerResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.WebViewer" +
        ".Requests")]
    public partial class WebViewerResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string SessionIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SessionId
        {
            get
            {
                return this.SessionIdField;
            }
            set
            {
                this.SessionIdField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.SLA
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PlusTimeRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.SLA")]
    public partial class PlusTimeRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string LookupNameField;
        
        private long PeroleIdField;
        
        private double PlusTimeField;
        
        private int StartDateField;
        
        private long StartTimeField;
        
        private long WorktimeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LookupName
        {
            get
            {
                return this.LookupNameField;
            }
            set
            {
                this.LookupNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long PeroleId
        {
            get
            {
                return this.PeroleIdField;
            }
            set
            {
                this.PeroleIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double PlusTime
        {
            get
            {
                return this.PlusTimeField;
            }
            set
            {
                this.PlusTimeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StartDate
        {
            get
            {
                return this.StartDateField;
            }
            set
            {
                this.StartDateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long StartTime
        {
            get
            {
                return this.StartTimeField;
            }
            set
            {
                this.StartTimeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Worktime
        {
            get
            {
                return this.WorktimeField;
            }
            set
            {
                this.WorktimeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SLAChangeRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.SLA")]
    public partial class SLAChangeRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private System.DateTime ChangeDateField;
        
        private string MethodField;
        
        private long SlaIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime ChangeDate
        {
            get
            {
                return this.ChangeDateField;
            }
            set
            {
                this.ChangeDateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Method
        {
            get
            {
                return this.MethodField;
            }
            set
            {
                this.MethodField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long SlaId
        {
            get
            {
                return this.SlaIdField;
            }
            set
            {
                this.SlaIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SLACreateRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.SLA")]
    public partial class SLACreateRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string AreaField;
        
        private long AreaIdField;
        
        private long MetricIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Area
        {
            get
            {
                return this.AreaField;
            }
            set
            {
                this.AreaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long AreaId
        {
            get
            {
                return this.AreaIdField;
            }
            set
            {
                this.AreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long MetricId
        {
            get
            {
                return this.MetricIdField;
            }
            set
            {
                this.MetricIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TimeDiffRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.SLA")]
    public partial class TimeDiffRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private int EndDateField;
        
        private long EndTimeField;
        
        private long PeroleIdField;
        
        private int StartDateField;
        
        private long StartTimeField;
        
        private long WorktimeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int EndDate
        {
            get
            {
                return this.EndDateField;
            }
            set
            {
                this.EndDateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long EndTime
        {
            get
            {
                return this.EndTimeField;
            }
            set
            {
                this.EndTimeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long PeroleId
        {
            get
            {
                return this.PeroleIdField;
            }
            set
            {
                this.PeroleIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StartDate
        {
            get
            {
                return this.StartDateField;
            }
            set
            {
                this.StartDateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long StartTime
        {
            get
            {
                return this.StartTimeField;
            }
            set
            {
                this.StartTimeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Worktime
        {
            get
            {
                return this.WorktimeField;
            }
            set
            {
                this.WorktimeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PlusTimeResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.SLA")]
    public partial class PlusTimeResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private double ResponseField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Response
        {
            get
            {
                return this.ResponseField;
            }
            set
            {
                this.ResponseField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SLAChangeResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.SLA")]
    public partial class SLAChangeResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private long SlaStepIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long SlaStepId
        {
            get
            {
                return this.SlaStepIdField;
            }
            set
            {
                this.SlaStepIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SLACreateResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.SLA")]
    public partial class SLACreateResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private long SlaIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long SlaId
        {
            get
            {
                return this.SlaIdField;
            }
            set
            {
                this.SlaIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TimeDiffResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.SLA")]
    public partial class TimeDiffResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private long DifferenceField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Difference
        {
            get
            {
                return this.DifferenceField;
            }
            set
            {
                this.DifferenceField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.Security
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowLoginWindowRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Security")]
    public partial class ShowLoginWindowRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.Security.Permissions
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PermitRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Security." +
        "Permissions")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenLeaseCommon.Security.Permissions.PermitExtendedRequest))]
    public partial class PermitRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string FunctionCodeField;
        
        private string ModuleCodeField;
        
        private char PermissionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FunctionCode
        {
            get
            {
                return this.FunctionCodeField;
            }
            set
            {
                this.FunctionCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ModuleCode
        {
            get
            {
                return this.ModuleCodeField;
            }
            set
            {
                this.ModuleCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public char Permission
        {
            get
            {
                return this.PermissionField;
            }
            set
            {
                this.PermissionField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PermitExtendedRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Security." +
        "Permissions")]
    public partial class PermitExtendedRequest : CIC.Bas.Modules.OpenLeaseCommon.Security.Permissions.PermitRequest
    {
        
        private long UserIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PermitResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Security." +
        "Permissions")]
    public partial class PermitResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private bool HasPermissionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool HasPermission
        {
            get
            {
                return this.HasPermissionField;
            }
            set
            {
                this.HasPermissionField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.PdfViewer.Requests
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PdfViewerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.PdfViewer" +
        ".Requests")]
    public partial class PdfViewerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string ControlsEnabledField;
        
        private string SourcePathField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ControlsEnabled
        {
            get
            {
                return this.ControlsEnabledField;
            }
            set
            {
                this.ControlsEnabledField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SourcePath
        {
            get
            {
                return this.SourcePathField;
            }
            set
            {
                this.SourcePathField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenBPE.ServiceAccess
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DispatchAndExecuteEventRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class DispatchAndExecuteEventRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long AreaIdField;
        
        private string AreaNameField;
        
        private string EventCodeField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.SubscriptionsDataDto ProcessContextField;
        
        private string ProcessDefinitionCodeField;
        
        private long UserIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long AreaId
        {
            get
            {
                return this.AreaIdField;
            }
            set
            {
                this.AreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AreaName
        {
            get
            {
                return this.AreaNameField;
            }
            set
            {
                this.AreaNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EventCode
        {
            get
            {
                return this.EventCodeField;
            }
            set
            {
                this.EventCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.SubscriptionsDataDto ProcessContext
        {
            get
            {
                return this.ProcessContextField;
            }
            set
            {
                this.ProcessContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProcessDefinitionCode
        {
            get
            {
                return this.ProcessDefinitionCodeField;
            }
            set
            {
                this.ProcessDefinitionCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DispatchAndExecuteListenerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class DispatchAndExecuteListenerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long InstanceTimestampField;
        
        private long ListenerIdField;
        
        private bool OverwriteProcessContextField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.SubscriptionsDataDto ProcessContextField;
        
        private long UserIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long InstanceTimestamp
        {
            get
            {
                return this.InstanceTimestampField;
            }
            set
            {
                this.InstanceTimestampField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ListenerId
        {
            get
            {
                return this.ListenerIdField;
            }
            set
            {
                this.ListenerIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool OverwriteProcessContext
        {
            get
            {
                return this.OverwriteProcessContextField;
            }
            set
            {
                this.OverwriteProcessContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.SubscriptionsDataDto ProcessContext
        {
            get
            {
                return this.ProcessContextField;
            }
            set
            {
                this.ProcessContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetInstanceTimestampRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class GetInstanceTimestampRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long ProcessInstanceIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ProcessInstanceId
        {
            get
            {
                return this.ProcessInstanceIdField;
            }
            set
            {
                this.ProcessInstanceIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetListenersRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class GetListenersRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long AreaIdField;
        
        private string AreaNameField;
        
        private string ProcessDefinitionCodeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long AreaId
        {
            get
            {
                return this.AreaIdField;
            }
            set
            {
                this.AreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AreaName
        {
            get
            {
                return this.AreaNameField;
            }
            set
            {
                this.AreaNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProcessDefinitionCode
        {
            get
            {
                return this.ProcessDefinitionCodeField;
            }
            set
            {
                this.ProcessDefinitionCodeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetProcessContextRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class GetProcessContextRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long ListenerIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ListenerId
        {
            get
            {
                return this.ListenerIdField;
            }
            set
            {
                this.ListenerIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PermissionCheckRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class PermissionCheckRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string LaneNameField;
        
        private long UserIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LaneName
        {
            get
            {
                return this.LaneNameField;
            }
            set
            {
                this.LaneNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SearchListenersRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class SearchListenersRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private CIC.Bas.Modules.OpenBPE.ServiceAccess.SearchListenersFilter FilterField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenBPE.ServiceAccess.SearchListenersFilter Filter
        {
            get
            {
                return this.FilterField;
            }
            set
            {
                this.FilterField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SetProcessContextRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class SetProcessContextRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.SubscriptionsDataDto ProcessContextField;
        
        private long ProcessInstanceIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.SubscriptionsDataDto ProcessContext
        {
            get
            {
                return this.ProcessContextField;
            }
            set
            {
                this.ProcessContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ProcessInstanceId
        {
            get
            {
                return this.ProcessInstanceIdField;
            }
            set
            {
                this.ProcessInstanceIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SearchListenersFilter", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class SearchListenersFilter : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private System.Nullable<long> AreaIdFromField;
        
        private System.Nullable<long> AreaIdToField;
        
        private string AreaNameField;
        
        private System.Nullable<System.DateTime> CreatedAtFromField;
        
        private System.Nullable<System.DateTime> CreatedAtToField;
        
        private System.Nullable<long> CreatedByUserIdField;
        
        private System.Nullable<long> IsUserTaskField;
        
        private string LaneNameField;
        
        private System.Nullable<CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerState> ListenerStateField;
        
        private CIC.Bas.Modules.OpenBPE.ServiceAccess.SearchListenersColumn OrderByColumnField;
        
        private System.Nullable<int> PageNumberField;
        
        private System.Nullable<int> PageSizeField;
        
        private System.Nullable<long> ProcessInstanceIdFromField;
        
        private System.Nullable<long> ProcessInstanceIdToField;
        
        private CIC.Bas.Framework.Core.Extensions.SortOrder SortOrderField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> AreaIdFrom
        {
            get
            {
                return this.AreaIdFromField;
            }
            set
            {
                this.AreaIdFromField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> AreaIdTo
        {
            get
            {
                return this.AreaIdToField;
            }
            set
            {
                this.AreaIdToField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AreaName
        {
            get
            {
                return this.AreaNameField;
            }
            set
            {
                this.AreaNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> CreatedAtFrom
        {
            get
            {
                return this.CreatedAtFromField;
            }
            set
            {
                this.CreatedAtFromField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> CreatedAtTo
        {
            get
            {
                return this.CreatedAtToField;
            }
            set
            {
                this.CreatedAtToField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> CreatedByUserId
        {
            get
            {
                return this.CreatedByUserIdField;
            }
            set
            {
                this.CreatedByUserIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> IsUserTask
        {
            get
            {
                return this.IsUserTaskField;
            }
            set
            {
                this.IsUserTaskField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LaneName
        {
            get
            {
                return this.LaneNameField;
            }
            set
            {
                this.LaneNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerState> ListenerState
        {
            get
            {
                return this.ListenerStateField;
            }
            set
            {
                this.ListenerStateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenBPE.ServiceAccess.SearchListenersColumn OrderByColumn
        {
            get
            {
                return this.OrderByColumnField;
            }
            set
            {
                this.OrderByColumnField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> PageNumber
        {
            get
            {
                return this.PageNumberField;
            }
            set
            {
                this.PageNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> PageSize
        {
            get
            {
                return this.PageSizeField;
            }
            set
            {
                this.PageSizeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> ProcessInstanceIdFrom
        {
            get
            {
                return this.ProcessInstanceIdFromField;
            }
            set
            {
                this.ProcessInstanceIdFromField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> ProcessInstanceIdTo
        {
            get
            {
                return this.ProcessInstanceIdToField;
            }
            set
            {
                this.ProcessInstanceIdToField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.Core.Extensions.SortOrder SortOrder
        {
            get
            {
                return this.SortOrderField;
            }
            set
            {
                this.SortOrderField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SearchListenersColumn", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public enum SearchListenersColumn : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Id = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EventCode = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AreaName = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AreaId = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ProcessId = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ListenerDefinitionId = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ProcessDefinitionId = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ProcessInstanceId = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        IsUserTask = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FlowNodeId = 9,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Priority = 10,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ListenerState = 11,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ExecutionHost = 12,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UserId = 13,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        TokenId = 14,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LaneName = 15,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CreatedAt = 16,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ProcessInstanceChildId = 17,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EvaluateCode = 18,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EventDefinitionCode = 19,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EnablePersistence = 20,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DispatchAndExecuteEventResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class DispatchAndExecuteEventResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private CIC.Bas.Modules.OpenBPE.ServiceAccess.ExecutionReport ExecutionReportField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenBPE.ServiceAccess.ExecutionReport ExecutionReport
        {
            get
            {
                return this.ExecutionReportField;
            }
            set
            {
                this.ExecutionReportField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DispatchAndExecuteListenerResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class DispatchAndExecuteListenerResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private CIC.Bas.Modules.OpenBPE.ServiceAccess.ExecutionReport ExecutionReportField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenBPE.ServiceAccess.ExecutionReport ExecutionReport
        {
            get
            {
                return this.ExecutionReportField;
            }
            set
            {
                this.ExecutionReportField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetInstanceTimestampResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class GetInstanceTimestampResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private long InstanceTimestampField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long InstanceTimestamp
        {
            get
            {
                return this.InstanceTimestampField;
            }
            set
            {
                this.InstanceTimestampField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetListenersResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class GetListenersResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerModel[] ListenersField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerModel[] Listeners
        {
            get
            {
                return this.ListenersField;
            }
            set
            {
                this.ListenersField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetProcessContextResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class GetProcessContextResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.SubscriptionsDataDto ProcessContextField;
        
        private long ProcessInstanceIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.SubscriptionsDataDto ProcessContext
        {
            get
            {
                return this.ProcessContextField;
            }
            set
            {
                this.ProcessContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ProcessInstanceId
        {
            get
            {
                return this.ProcessInstanceIdField;
            }
            set
            {
                this.ProcessInstanceIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PermissionCheckResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class PermissionCheckResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private bool HasPermissionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool HasPermission
        {
            get
            {
                return this.HasPermissionField;
            }
            set
            {
                this.HasPermissionField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SearchListenersResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class SearchListenersResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerModel[] ListenersField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerModel[] Listeners
        {
            get
            {
                return this.ListenersField;
            }
            set
            {
                this.ListenersField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ExecutionReport", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.ServiceAccess")]
    public partial class ExecutionReport : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private long AreaIdField;
        
        private string AreaNameField;
        
        private string EventCodeField;
        
        private CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerModel[] ListenersField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.SubscriptionsDataDto ProcessContextField;
        
        private string ProcessDefinitionCodeField;
        
        private CIC.Bas.Modules.OpenBPE.Storage.Models.ProcessInstanceModel ProcessInstanceField;
        
        private long UserIdField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long AreaId
        {
            get
            {
                return this.AreaIdField;
            }
            set
            {
                this.AreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AreaName
        {
            get
            {
                return this.AreaNameField;
            }
            set
            {
                this.AreaNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EventCode
        {
            get
            {
                return this.EventCodeField;
            }
            set
            {
                this.EventCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerModel[] Listeners
        {
            get
            {
                return this.ListenersField;
            }
            set
            {
                this.ListenersField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.SubscriptionsDataDto ProcessContext
        {
            get
            {
                return this.ProcessContextField;
            }
            set
            {
                this.ProcessContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProcessDefinitionCode
        {
            get
            {
                return this.ProcessDefinitionCodeField;
            }
            set
            {
                this.ProcessDefinitionCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenBPE.Storage.Models.ProcessInstanceModel ProcessInstance
        {
            get
            {
                return this.ProcessInstanceField;
            }
            set
            {
                this.ProcessInstanceField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.KNEManager
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowKNEManagerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.KNEManage" +
        "r")]
    public partial class ShowKNEManagerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.JobFlowDesigner
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowJobFlowDesignerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.JobFlowDe" +
        "signer")]
    public partial class ShowJobFlowDesignerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private long IdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.FormulaManager
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowManagerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.FormulaMa" +
        "nager")]
    public partial class ShowManagerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.EvaluateService
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EvaluateRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.EvaluateS" +
        "ervice")]
    public partial class EvaluateRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string AreaField;
        
        private long AreaIdField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.EvaluateService.EvaluateExpression[] ExpressionsField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.EvaluateService.EvaluateLanguage LanguageField;
        
        private long UserIdField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.EvaluateService.VariableDefinition[] VariablesField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Area
        {
            get
            {
                return this.AreaField;
            }
            set
            {
                this.AreaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long AreaId
        {
            get
            {
                return this.AreaIdField;
            }
            set
            {
                this.AreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.EvaluateService.EvaluateExpression[] Expressions
        {
            get
            {
                return this.ExpressionsField;
            }
            set
            {
                this.ExpressionsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.EvaluateService.EvaluateLanguage Language
        {
            get
            {
                return this.LanguageField;
            }
            set
            {
                this.LanguageField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.EvaluateService.VariableDefinition[] Variables
        {
            get
            {
                return this.VariablesField;
            }
            set
            {
                this.VariablesField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EvaluateExpression", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.EvaluateS" +
        "ervice")]
    public partial class EvaluateExpression : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string ExpressionField;
        
        private string GuidField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Expression
        {
            get
            {
                return this.ExpressionField;
            }
            set
            {
                this.ExpressionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Guid
        {
            get
            {
                return this.GuidField;
            }
            set
            {
                this.GuidField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EvaluateLanguage", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.EvaluateS" +
        "ervice")]
    public enum EvaluateLanguage : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Clarion = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DotNet = 1,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="VariableDefinition", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.EvaluateS" +
        "ervice")]
    public partial class VariableDefinition : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private bool IsReadOnlyField;
        
        private string NameField;
        
        private string PrefixField;
        
        private string ValueField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsReadOnly
        {
            get
            {
                return this.IsReadOnlyField;
            }
            set
            {
                this.IsReadOnlyField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Prefix
        {
            get
            {
                return this.PrefixField;
            }
            set
            {
                this.PrefixField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value
        {
            get
            {
                return this.ValueField;
            }
            set
            {
                this.ValueField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EvaluateResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.EvaluateS" +
        "ervice")]
    public partial class EvaluateResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private CIC.Bas.Modules.OpenLeaseCommon.EvaluateService.EvaluateExpressionResult[] ResultsField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.EvaluateService.EvaluateExpressionResult[] Results
        {
            get
            {
                return this.ResultsField;
            }
            set
            {
                this.ResultsField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EvaluateExpressionResult", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.EvaluateS" +
        "ervice")]
    public partial class EvaluateExpressionResult : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string GuidField;
        
        private string ResultField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Guid
        {
            get
            {
                return this.GuidField;
            }
            set
            {
                this.GuidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Result
        {
            get
            {
                return this.ResultField;
            }
            set
            {
                this.ResultField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.DocUniManager
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowEditorRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.DocUniMan" +
        "ager")]
    public partial class ShowEditorRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string AreaField;
        
        private string DescriptionField;
        
        private System.Nullable<int> DocSubtypeField;
        
        private System.Nullable<int> DocSubtypeFormatField;
        
        private System.Nullable<int> DocTypeField;
        
        private string NameField;
        
        private int RankField;
        
        private System.Nullable<long> SysIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Area
        {
            get
            {
                return this.AreaField;
            }
            set
            {
                this.AreaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                this.DescriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> DocSubtype
        {
            get
            {
                return this.DocSubtypeField;
            }
            set
            {
                this.DocSubtypeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> DocSubtypeFormat
        {
            get
            {
                return this.DocSubtypeFormatField;
            }
            set
            {
                this.DocSubtypeFormatField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> DocType
        {
            get
            {
                return this.DocTypeField;
            }
            set
            {
                this.DocTypeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Rank
        {
            get
            {
                return this.RankField;
            }
            set
            {
                this.RankField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> SysId
        {
            get
            {
                return this.SysIdField;
            }
            set
            {
                this.SysIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowManagerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.DocUniMan" +
        "ager")]
    public partial class ShowManagerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.DeepLinkService.DTO
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DeepLinkServiceStartRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.DeepLinkS" +
        "ervice.DTO")]
    public partial class DeepLinkServiceStartRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private int PortField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Port
        {
            get
            {
                return this.PortField;
            }
            set
            {
                this.PortField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DeepLinkServiceGetStateRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.DeepLinkS" +
        "ervice.DTO")]
    public partial class DeepLinkServiceGetStateRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private int PortField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Port
        {
            get
            {
                return this.PortField;
            }
            set
            {
                this.PortField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DeepLinkServiceStopRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.DeepLinkS" +
        "ervice.DTO")]
    public partial class DeepLinkServiceStopRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private int PortField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Port
        {
            get
            {
                return this.PortField;
            }
            set
            {
                this.PortField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DeepLinkServiceStartResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.DeepLinkS" +
        "ervice.DTO")]
    public partial class DeepLinkServiceStartResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string ErrorMessageField;
        
        private bool IsSuccessfulField;
        
        private string ListenUrlField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorMessage
        {
            get
            {
                return this.ErrorMessageField;
            }
            set
            {
                this.ErrorMessageField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsSuccessful
        {
            get
            {
                return this.IsSuccessfulField;
            }
            set
            {
                this.IsSuccessfulField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ListenUrl
        {
            get
            {
                return this.ListenUrlField;
            }
            set
            {
                this.ListenUrlField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DeepLinkServiceGetStateResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.DeepLinkS" +
        "ervice.DTO")]
    public partial class DeepLinkServiceGetStateResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string ErrorMessageField;
        
        private string StateField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorMessage
        {
            get
            {
                return this.ErrorMessageField;
            }
            set
            {
                this.ErrorMessageField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string State
        {
            get
            {
                return this.StateField;
            }
            set
            {
                this.StateField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DeepLinkServiceStopResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.DeepLinkS" +
        "ervice.DTO")]
    public partial class DeepLinkServiceStopResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string ErrorMessageField;
        
        private bool IsSuccessfulField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorMessage
        {
            get
            {
                return this.ErrorMessageField;
            }
            set
            {
                this.ErrorMessageField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsSuccessful
        {
            get
            {
                return this.IsSuccessfulField;
            }
            set
            {
                this.IsSuccessfulField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.Iso8061Service
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Iso8061Request", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Iso8061Se" +
        "rvice")]
    public partial class Iso8061Request : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string ActionField;
        
        private string Parameter1Field;
        
        private string Parameter2Field;
        
        private string Parameter3Field;
        
        private string Parameter4Field;
        
        private string SubActionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Action
        {
            get
            {
                return this.ActionField;
            }
            set
            {
                this.ActionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter1
        {
            get
            {
                return this.Parameter1Field;
            }
            set
            {
                this.Parameter1Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter2
        {
            get
            {
                return this.Parameter2Field;
            }
            set
            {
                this.Parameter2Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter3
        {
            get
            {
                return this.Parameter3Field;
            }
            set
            {
                this.Parameter3Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter4
        {
            get
            {
                return this.Parameter4Field;
            }
            set
            {
                this.Parameter4Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SubAction
        {
            get
            {
                return this.SubActionField;
            }
            set
            {
                this.SubActionField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Iso8061Response", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Iso8061Se" +
        "rvice")]
    public partial class Iso8061Response : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string ResultField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Result
        {
            get
            {
                return this.ResultField;
            }
            set
            {
                this.ResultField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.ScriptsManager
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowManagerRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.ScriptsMa" +
        "nager")]
    public partial class ShowManagerRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.Query.Model
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CustomizeQueryRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Query.Mod" +
        "el")]
    public partial class CustomizeQueryRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string CustomizationXmlField;
        
        private bool LoadXmlFromDbField;
        
        private string QueryNameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CustomizationXml
        {
            get
            {
                return this.CustomizationXmlField;
            }
            set
            {
                this.CustomizationXmlField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool LoadXmlFromDb
        {
            get
            {
                return this.LoadXmlFromDbField;
            }
            set
            {
                this.LoadXmlFromDbField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string QueryName
        {
            get
            {
                return this.QueryNameField;
            }
            set
            {
                this.QueryNameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ShowQueryRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Query.Mod" +
        "el")]
    public partial class ShowQueryRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string CustomizationXmlField;
        
        private string P1Field;
        
        private string P2Field;
        
        private string P3Field;
        
        private string P4Field;
        
        private string P5Field;
        
        private string P6Field;
        
        private bool PreviewField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.Query.Model.QueryMode QueryModeField;
        
        private string QueryNameField;
        
        private bool ReEvalField;
        
        private bool ResetViewField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CustomizationXml
        {
            get
            {
                return this.CustomizationXmlField;
            }
            set
            {
                this.CustomizationXmlField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string P1
        {
            get
            {
                return this.P1Field;
            }
            set
            {
                this.P1Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string P2
        {
            get
            {
                return this.P2Field;
            }
            set
            {
                this.P2Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string P3
        {
            get
            {
                return this.P3Field;
            }
            set
            {
                this.P3Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string P4
        {
            get
            {
                return this.P4Field;
            }
            set
            {
                this.P4Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string P5
        {
            get
            {
                return this.P5Field;
            }
            set
            {
                this.P5Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string P6
        {
            get
            {
                return this.P6Field;
            }
            set
            {
                this.P6Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Preview
        {
            get
            {
                return this.PreviewField;
            }
            set
            {
                this.PreviewField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Query.Model.QueryMode QueryMode
        {
            get
            {
                return this.QueryModeField;
            }
            set
            {
                this.QueryModeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string QueryName
        {
            get
            {
                return this.QueryNameField;
            }
            set
            {
                this.QueryNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ReEval
        {
            get
            {
                return this.ReEvalField;
            }
            set
            {
                this.ReEvalField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ResetView
        {
            get
            {
                return this.ResetViewField;
            }
            set
            {
                this.ResetViewField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="QueryMode", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Query.Mod" +
        "el")]
    public enum QueryMode : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SelectMode = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EditMode = 2,
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.Payments.Model
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ImportCamtRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Payments." +
        "Model")]
    public partial class ImportCamtRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string FilePathField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.Payments.BbImportType ImportTypeField;
        
        private bool MultipleImportField;
        
        private bool OneBbposForEveryTxDtlField;
        
        private long SysWfexecField;
        
        private long SysWfuserField;
        
        private long SysbbtypField;
        
        private long SyslsField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FilePath
        {
            get
            {
                return this.FilePathField;
            }
            set
            {
                this.FilePathField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Payments.BbImportType ImportType
        {
            get
            {
                return this.ImportTypeField;
            }
            set
            {
                this.ImportTypeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool MultipleImport
        {
            get
            {
                return this.MultipleImportField;
            }
            set
            {
                this.MultipleImportField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool OneBbposForEveryTxDtl
        {
            get
            {
                return this.OneBbposForEveryTxDtlField;
            }
            set
            {
                this.OneBbposForEveryTxDtlField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long SysWfexec
        {
            get
            {
                return this.SysWfexecField;
            }
            set
            {
                this.SysWfexecField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long SysWfuser
        {
            get
            {
                return this.SysWfuserField;
            }
            set
            {
                this.SysWfuserField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Sysbbtyp
        {
            get
            {
                return this.SysbbtypField;
            }
            set
            {
                this.SysbbtypField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Sysls
        {
            get
            {
                return this.SyslsField;
            }
            set
            {
                this.SyslsField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ExportPain008Request", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Payments." +
        "Model")]
    public partial class ExportPain008Request : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string FilePathField;
        
        private string MessageIdField;
        
        private long SysPmtGrpField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FilePath
        {
            get
            {
                return this.FilePathField;
            }
            set
            {
                this.FilePathField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MessageId
        {
            get
            {
                return this.MessageIdField;
            }
            set
            {
                this.MessageIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long SysPmtGrp
        {
            get
            {
                return this.SysPmtGrpField;
            }
            set
            {
                this.SysPmtGrpField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ExportPain001Request", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Payments." +
        "Model")]
    public partial class ExportPain001Request : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string FilePathField;
        
        private string MessageIdField;
        
        private long SysPmtGrpField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FilePath
        {
            get
            {
                return this.FilePathField;
            }
            set
            {
                this.FilePathField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MessageId
        {
            get
            {
                return this.MessageIdField;
            }
            set
            {
                this.MessageIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long SysPmtGrp
        {
            get
            {
                return this.SysPmtGrpField;
            }
            set
            {
                this.SysPmtGrpField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.ExposedApi
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ExecuteScriptRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.ExposedAp" +
        "i")]
    public partial class ExecuteScriptRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string AreaField;
        
        private long AreaIdField;
        
        private CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto ContextField;
        
        private string Parameter1Field;
        
        private string Parameter2Field;
        
        private string Parameter3Field;
        
        private string Parameter4Field;
        
        private string Parameter5Field;
        
        private string ScriptCodeField;
        
        private string StartBeanCodeField;
        
        private long UserIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Area
        {
            get
            {
                return this.AreaField;
            }
            set
            {
                this.AreaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long AreaId
        {
            get
            {
                return this.AreaIdField;
            }
            set
            {
                this.AreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto Context
        {
            get
            {
                return this.ContextField;
            }
            set
            {
                this.ContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter1
        {
            get
            {
                return this.Parameter1Field;
            }
            set
            {
                this.Parameter1Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter2
        {
            get
            {
                return this.Parameter2Field;
            }
            set
            {
                this.Parameter2Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter3
        {
            get
            {
                return this.Parameter3Field;
            }
            set
            {
                this.Parameter3Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter4
        {
            get
            {
                return this.Parameter4Field;
            }
            set
            {
                this.Parameter4Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter5
        {
            get
            {
                return this.Parameter5Field;
            }
            set
            {
                this.Parameter5Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ScriptCode
        {
            get
            {
                return this.ScriptCodeField;
            }
            set
            {
                this.ScriptCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StartBeanCode
        {
            get
            {
                return this.StartBeanCodeField;
            }
            set
            {
                this.StartBeanCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ExecuteFormulaRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.ExposedAp" +
        "i")]
    public partial class ExecuteFormulaRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string AreaField;
        
        private long AreaIdField;
        
        private CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto ContextField;
        
        private string FormulaNameField;
        
        private string Parameter1Field;
        
        private string Parameter2Field;
        
        private string Parameter3Field;
        
        private string Parameter4Field;
        
        private string Parameter5Field;
        
        private long UserIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Area
        {
            get
            {
                return this.AreaField;
            }
            set
            {
                this.AreaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long AreaId
        {
            get
            {
                return this.AreaIdField;
            }
            set
            {
                this.AreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto Context
        {
            get
            {
                return this.ContextField;
            }
            set
            {
                this.ContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FormulaName
        {
            get
            {
                return this.FormulaNameField;
            }
            set
            {
                this.FormulaNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter1
        {
            get
            {
                return this.Parameter1Field;
            }
            set
            {
                this.Parameter1Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter2
        {
            get
            {
                return this.Parameter2Field;
            }
            set
            {
                this.Parameter2Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter3
        {
            get
            {
                return this.Parameter3Field;
            }
            set
            {
                this.Parameter3Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter4
        {
            get
            {
                return this.Parameter4Field;
            }
            set
            {
                this.Parameter4Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Parameter5
        {
            get
            {
                return this.Parameter5Field;
            }
            set
            {
                this.Parameter5Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ExecuteScriptResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.ExposedAp" +
        "i")]
    public partial class ExecuteScriptResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto ContextField;
        
        private string ScriptResultField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto Context
        {
            get
            {
                return this.ContextField;
            }
            set
            {
                this.ContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ScriptResult
        {
            get
            {
                return this.ScriptResultField;
            }
            set
            {
                this.ScriptResultField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ExecuteFormulaResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.ExposedAp" +
        "i")]
    public partial class ExecuteFormulaResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto ContextField;
        
        private string FormulaResultField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto Context
        {
            get
            {
                return this.ContextField;
            }
            set
            {
                this.ContextField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FormulaResult
        {
            get
            {
                return this.FormulaResultField;
            }
            set
            {
                this.FormulaResultField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.Evaluate
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EvaluateFunctionsRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate")]
    public partial class EvaluateFunctionsRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string ExpressionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Expression
        {
            get
            {
                return this.ExpressionField;
            }
            set
            {
                this.ExpressionField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StopWatchRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate")]
    public partial class StopWatchRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string NameField;
        
        private string OperationField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Operation
        {
            get
            {
                return this.OperationField;
            }
            set
            {
                this.OperationField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DelayRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate")]
    public partial class DelayRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string MiliSecondsField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MiliSeconds
        {
            get
            {
                return this.MiliSecondsField;
            }
            set
            {
                this.MiliSecondsField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EvaluateFunctionsResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate")]
    public partial class EvaluateFunctionsResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string ResultField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Result
        {
            get
            {
                return this.ResultField;
            }
            set
            {
                this.ResultField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StopWatchResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate")]
    public partial class StopWatchResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string ResultField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Result
        {
            get
            {
                return this.ResultField;
            }
            set
            {
                this.ResultField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DelayResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate")]
    public partial class DelayResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string MiliSecondsField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MiliSeconds
        {
            get
            {
                return this.MiliSecondsField;
            }
            set
            {
                this.MiliSecondsField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.Evaluate.BindProceduresStatistics
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetBindProceduresStatisticsRequest", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate." +
        "BindProceduresStatistics")]
    public partial class GetBindProceduresStatisticsRequest : CIC.Bas.Framework.Extensibility.RequestBase
    {
        
        private string ProcedureNamesField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProcedureNames
        {
            get
            {
                return this.ProcedureNamesField;
            }
            set
            {
                this.ProcedureNamesField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetBindProcedureStatisticsResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate." +
        "BindProceduresStatistics")]
    public partial class GetBindProcedureStatisticsResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private int[] CountPerProcedureField;
        
        private string StatisticsField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int[] CountPerProcedure
        {
            get
            {
                return this.CountPerProcedureField;
            }
            set
            {
                this.CountPerProcedureField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Statistics
        {
            get
            {
                return this.StatisticsField;
            }
            set
            {
                this.StatisticsField = value;
            }
        }
    }
}
namespace CIC.Bas.Framework.OpenLease.Subscriptions
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SubscriptionsDataDto", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.OpenLease.Subscriptions" +
        "")]
    public partial class SubscriptionsDataDto : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto[] QueuesField;
        
        private CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto[] SubscriptionsField;
        
        private CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] VariablesField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto[] Queues
        {
            get
            {
                return this.QueuesField;
            }
            set
            {
                this.QueuesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto[] Subscriptions
        {
            get
            {
                return this.SubscriptionsField;
            }
            set
            {
                this.SubscriptionsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] Variables
        {
            get
            {
                return this.VariablesField;
            }
            set
            {
                this.VariablesField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="QueueDto", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.OpenLease.Subscriptions" +
        "")]
    public partial class QueueDto : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string NameField;
        
        private CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto[] RecordsField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto[] Records
        {
            get
            {
                return this.RecordsField;
            }
            set
            {
                this.RecordsField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SubscriptionDto", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.OpenLease.Subscriptions" +
        "")]
    public partial class SubscriptionDto : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string ObjectKeyField;
        
        private string ObjectNameField;
        
        private string ObjectTypeField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ObjectKey
        {
            get
            {
                return this.ObjectKeyField;
            }
            set
            {
                this.ObjectKeyField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ObjectName
        {
            get
            {
                return this.ObjectNameField;
            }
            set
            {
                this.ObjectNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ObjectType
        {
            get
            {
                return this.ObjectTypeField;
            }
            set
            {
                this.ObjectTypeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LookupVariableDto", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.OpenLease.Subscriptions" +
        "")]
    public partial class LookupVariableDto : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string LookupVariableNameField;
        
        private string ValueField;
        
        private string VariableNameField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LookupVariableName
        {
            get
            {
                return this.LookupVariableNameField;
            }
            set
            {
                this.LookupVariableNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value
        {
            get
            {
                return this.ValueField;
            }
            set
            {
                this.ValueField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string VariableName
        {
            get
            {
                return this.VariableNameField;
            }
            set
            {
                this.VariableNameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="QueueRecordDto", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.OpenLease.Subscriptions" +
        "")]
    public partial class QueueRecordDto : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto[] ValuesField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto[] Values
        {
            get
            {
                return this.ValuesField;
            }
            set
            {
                this.ValuesField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="QueueRecordValueDto", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.OpenLease.Subscriptions" +
        "")]
    public partial class QueueRecordValueDto : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string ValueField;
        
        private string VariableNameField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value
        {
            get
            {
                return this.ValueField;
            }
            set
            {
                this.ValueField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string VariableName
        {
            get
            {
                return this.VariableNameField;
            }
            set
            {
                this.VariableNameField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.SpreadsheetProcessing.Model
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CellIndex", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Mod" +
        "el")]
    public partial class CellIndex : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int ColumnIndexField;
        
        private string ExcelIndexField;
        
        private int RowIndexField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ColumnIndex
        {
            get
            {
                return this.ColumnIndexField;
            }
            set
            {
                this.ColumnIndexField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ExcelIndex
        {
            get
            {
                return this.ExcelIndexField;
            }
            set
            {
                this.ExcelIndexField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RowIndex
        {
            get
            {
                return this.RowIndexField;
            }
            set
            {
                this.RowIndexField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CellReturnValue", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Mod" +
        "el")]
    public enum CellReturnValue : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Value = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DateTime = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Text = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Formula = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        All = 10,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SpreadsheetSection", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Mod" +
        "el")]
    public enum SpreadsheetSection : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Left = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Center = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Right = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AutoFitOrientation", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Mod" +
        "el")]
    public enum AutoFitOrientation : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Horizontal = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Vertical = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Both = 2,
    }
}
namespace CIC.Bas.Modules.SpreadsheetProcessing.Services
{
    using System;
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.FlagsAttribute()]
    [System.Runtime.Serialization.DataContractAttribute(Name="CellFontStyle", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Ser" +
        "vices")]
    public enum CellFontStyle : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        None = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Bold = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Italic = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Strikethrough = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Underline = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DoubleUnderline = 16,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.FlagsAttribute()]
    [System.Runtime.Serialization.DataContractAttribute(Name="CellBorderOrientation", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Ser" +
        "vices")]
    public enum CellBorderOrientation : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        None = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Left = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Top = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Right = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Bottom = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DiagonalDown = 16,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DiagonalUp = 32,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        InsideHorizontal = 64,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        InsideVertical = 128,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.FlagsAttribute()]
    [System.Runtime.Serialization.DataContractAttribute(Name="CellProtectOptions", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.SpreadsheetProcessing.Ser" +
        "vices")]
    public enum CellProtectOptions : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        None = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Default = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DeleteRows = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        InsertRows = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DeleteColumns = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        InsertColumns = 16,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FormatCells = 32,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FormatColumns = 64,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FormatRows = 128,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Filtering = 256,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Sorting = 512,
    }
}
namespace DevExpress.Spreadsheet
{
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BorderLineStyle", Namespace="http://schemas.datacontract.org/2004/07/DevExpress.Spreadsheet")]
    public enum BorderLineStyle : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        None = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Thin = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Medium = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Dashed = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Dotted = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Thick = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Double = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Hair = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        MediumDashed = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DashDot = 9,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        MediumDashDot = 10,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DashDotDot = 11,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        MediumDashDotDot = 12,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SlantDashDot = 13,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SpreadsheetHorizontalAlignment", Namespace="http://schemas.datacontract.org/2004/07/DevExpress.Spreadsheet")]
    public enum SpreadsheetHorizontalAlignment : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        General = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Left = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Center = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Right = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Fill = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Justify = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CenterContinuous = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Distributed = 7,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SpreadsheetVerticalAlignment", Namespace="http://schemas.datacontract.org/2004/07/DevExpress.Spreadsheet")]
    public enum SpreadsheetVerticalAlignment : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Top = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Center = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Bottom = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Justify = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Distributed = 4,
    }
}
namespace CIC.Bas.Modules.OpenBPE.Actions
{
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserAction", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.Actions")]
    public enum UserAction : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RestoreProcess = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DeleteProcess = 1,
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SubscriptionsDataDto", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate." +
        "Compatibility.BusinessProcessing.Subscriptions.Dto")]
    public partial class SubscriptionsDataDto : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.QueueDto[] QueuesField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.SubscriptionDto[] SubscriptionsField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto[] VariablesField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.QueueDto[] Queues
        {
            get
            {
                return this.QueuesField;
            }
            set
            {
                this.QueuesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.SubscriptionDto[] Subscriptions
        {
            get
            {
                return this.SubscriptionsField;
            }
            set
            {
                this.SubscriptionsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto[] Variables
        {
            get
            {
                return this.VariablesField;
            }
            set
            {
                this.VariablesField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="QueueDto", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate." +
        "Compatibility.BusinessProcessing.Subscriptions.Dto")]
    public partial class QueueDto : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string NameField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.QueueRecordDto[] RecordsField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.QueueRecordDto[] Records
        {
            get
            {
                return this.RecordsField;
            }
            set
            {
                this.RecordsField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SubscriptionDto", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate." +
        "Compatibility.BusinessProcessing.Subscriptions.Dto")]
    public partial class SubscriptionDto : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string ObjectKeyField;
        
        private string ObjectNameField;
        
        private string ObjectTypeField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ObjectKey
        {
            get
            {
                return this.ObjectKeyField;
            }
            set
            {
                this.ObjectKeyField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ObjectName
        {
            get
            {
                return this.ObjectNameField;
            }
            set
            {
                this.ObjectNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ObjectType
        {
            get
            {
                return this.ObjectTypeField;
            }
            set
            {
                this.ObjectTypeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LookupVariableDto", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate." +
        "Compatibility.BusinessProcessing.Subscriptions.Dto")]
    public partial class LookupVariableDto : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string LookupVariableNameField;
        
        private string ValueField;
        
        private string VariableNameField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LookupVariableName
        {
            get
            {
                return this.LookupVariableNameField;
            }
            set
            {
                this.LookupVariableNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value
        {
            get
            {
                return this.ValueField;
            }
            set
            {
                this.ValueField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string VariableName
        {
            get
            {
                return this.VariableNameField;
            }
            set
            {
                this.VariableNameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="QueueRecordDto", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate." +
        "Compatibility.BusinessProcessing.Subscriptions.Dto")]
    public partial class QueueRecordDto : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.QueueRecordValueDto[] ValuesField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.QueueRecordValueDto[] Values
        {
            get
            {
                return this.ValuesField;
            }
            set
            {
                this.ValuesField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="QueueRecordValueDto", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Evaluate." +
        "Compatibility.BusinessProcessing.Subscriptions.Dto")]
    public partial class QueueRecordValueDto : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string ValueField;
        
        private string VariableNameField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value
        {
            get
            {
                return this.ValueField;
            }
            set
            {
                this.ValueField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string VariableName
        {
            get
            {
                return this.VariableNameField;
            }
            set
            {
                this.VariableNameField = value;
            }
        }
    }
}
namespace CIC.Bas.Modules.OpenBPE.Storage.Models
{
    using System.Runtime.Serialization;
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ListenerState", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.Storage.Models")]
    public enum ListenerState : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WaitingForBeProcessed = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReadyForDispatch = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Dispatching = 10,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReadyToExecute = 100,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Executing = 110,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Skipped = 120,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Executed = 130,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Error = 999,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProcessInstanceModel", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.Storage.Models")]
    public partial class ProcessInstanceModel : CIC.Bas.Modules.OpenBPE.Storage.Models.ModelBase
    {
        
        private System.Nullable<long> AreaIdField;
        
        private string AreaNameField;
        
        private System.Nullable<long> CaseIdField;
        
        private System.Nullable<long> CreatorIdField;
        
        private System.Nullable<System.DateTime> DeletedAtField;
        
        private CIC.Bas.Modules.OpenBPE.Storage.Models.ProcessInstanceState ExecutionStatusField;
        
        private System.Nullable<long> FlowNodeIdField;
        
        private System.Nullable<bool> IsDeletedField;
        
        private System.Nullable<bool> IsDepreciatedField;
        
        private System.Nullable<long> LastActiveTimestampField;
        
        private System.Nullable<long> ParentIdField;
        
        private System.Nullable<long> PriorityField;
        
        private string ProcessDefinitionCodeField;
        
        private long ProcessDefinitionIdField;
        
        private System.Nullable<long> ProcessIdField;
        
        private System.Nullable<bool> ShouldBeKilledField;
        
        private long TimestampField;
        
        private long TokenSequenceField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> AreaId
        {
            get
            {
                return this.AreaIdField;
            }
            set
            {
                this.AreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AreaName
        {
            get
            {
                return this.AreaNameField;
            }
            set
            {
                this.AreaNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> CaseId
        {
            get
            {
                return this.CaseIdField;
            }
            set
            {
                this.CaseIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> CreatorId
        {
            get
            {
                return this.CreatorIdField;
            }
            set
            {
                this.CreatorIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> DeletedAt
        {
            get
            {
                return this.DeletedAtField;
            }
            set
            {
                this.DeletedAtField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenBPE.Storage.Models.ProcessInstanceState ExecutionStatus
        {
            get
            {
                return this.ExecutionStatusField;
            }
            set
            {
                this.ExecutionStatusField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> FlowNodeId
        {
            get
            {
                return this.FlowNodeIdField;
            }
            set
            {
                this.FlowNodeIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<bool> IsDeleted
        {
            get
            {
                return this.IsDeletedField;
            }
            set
            {
                this.IsDeletedField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<bool> IsDepreciated
        {
            get
            {
                return this.IsDepreciatedField;
            }
            set
            {
                this.IsDepreciatedField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> LastActiveTimestamp
        {
            get
            {
                return this.LastActiveTimestampField;
            }
            set
            {
                this.LastActiveTimestampField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> ParentId
        {
            get
            {
                return this.ParentIdField;
            }
            set
            {
                this.ParentIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> Priority
        {
            get
            {
                return this.PriorityField;
            }
            set
            {
                this.PriorityField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProcessDefinitionCode
        {
            get
            {
                return this.ProcessDefinitionCodeField;
            }
            set
            {
                this.ProcessDefinitionCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ProcessDefinitionId
        {
            get
            {
                return this.ProcessDefinitionIdField;
            }
            set
            {
                this.ProcessDefinitionIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> ProcessId
        {
            get
            {
                return this.ProcessIdField;
            }
            set
            {
                this.ProcessIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<bool> ShouldBeKilled
        {
            get
            {
                return this.ShouldBeKilledField;
            }
            set
            {
                this.ShouldBeKilledField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Timestamp
        {
            get
            {
                return this.TimestampField;
            }
            set
            {
                this.TimestampField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long TokenSequence
        {
            get
            {
                return this.TokenSequenceField;
            }
            set
            {
                this.TokenSequenceField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ListenerModel", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.Storage.Models")]
    public partial class ListenerModel : CIC.Bas.Modules.OpenBPE.Storage.Models.ModelBase
    {
        
        private long AreaIdField;
        
        private string AreaNameField;
        
        private System.Nullable<System.DateTime> CreatedAtField;
        
        private bool EnablePersistenceField;
        
        private string EvaluateCodeField;
        
        private string EventCodeField;
        
        private string EventDefinitionCodeField;
        
        private string ExecutionHostField;
        
        private System.Nullable<long> FlowNodeIdField;
        
        private System.Nullable<bool> IsUserTaskField;
        
        private string LaneNameField;
        
        private System.Nullable<long> ListenerDefinitionIdField;
        
        private System.Nullable<long> PriorityField;
        
        private long ProcessDefinitionIdField;
        
        private System.Nullable<long> ProcessIdField;
        
        private System.Nullable<long> ProcessInstanceChildIdField;
        
        private System.Nullable<long> ProcessInstanceIdField;
        
        private CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerReasons ReasonsField;
        
        private System.Nullable<CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerState> StateField;
        
        private System.Nullable<CIC.Bas.Modules.OpenBPE.Storage.Models.SupervisorTypes> SupervisorTypesField;
        
        private System.Nullable<long> TokenIdField;
        
        private System.Nullable<long> UserIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long AreaId
        {
            get
            {
                return this.AreaIdField;
            }
            set
            {
                this.AreaIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AreaName
        {
            get
            {
                return this.AreaNameField;
            }
            set
            {
                this.AreaNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> CreatedAt
        {
            get
            {
                return this.CreatedAtField;
            }
            set
            {
                this.CreatedAtField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool EnablePersistence
        {
            get
            {
                return this.EnablePersistenceField;
            }
            set
            {
                this.EnablePersistenceField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EvaluateCode
        {
            get
            {
                return this.EvaluateCodeField;
            }
            set
            {
                this.EvaluateCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EventCode
        {
            get
            {
                return this.EventCodeField;
            }
            set
            {
                this.EventCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EventDefinitionCode
        {
            get
            {
                return this.EventDefinitionCodeField;
            }
            set
            {
                this.EventDefinitionCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ExecutionHost
        {
            get
            {
                return this.ExecutionHostField;
            }
            set
            {
                this.ExecutionHostField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> FlowNodeId
        {
            get
            {
                return this.FlowNodeIdField;
            }
            set
            {
                this.FlowNodeIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<bool> IsUserTask
        {
            get
            {
                return this.IsUserTaskField;
            }
            set
            {
                this.IsUserTaskField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LaneName
        {
            get
            {
                return this.LaneNameField;
            }
            set
            {
                this.LaneNameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> ListenerDefinitionId
        {
            get
            {
                return this.ListenerDefinitionIdField;
            }
            set
            {
                this.ListenerDefinitionIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> Priority
        {
            get
            {
                return this.PriorityField;
            }
            set
            {
                this.PriorityField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ProcessDefinitionId
        {
            get
            {
                return this.ProcessDefinitionIdField;
            }
            set
            {
                this.ProcessDefinitionIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> ProcessId
        {
            get
            {
                return this.ProcessIdField;
            }
            set
            {
                this.ProcessIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> ProcessInstanceChildId
        {
            get
            {
                return this.ProcessInstanceChildIdField;
            }
            set
            {
                this.ProcessInstanceChildIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> ProcessInstanceId
        {
            get
            {
                return this.ProcessInstanceIdField;
            }
            set
            {
                this.ProcessInstanceIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerReasons Reasons
        {
            get
            {
                return this.ReasonsField;
            }
            set
            {
                this.ReasonsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerState> State
        {
            get
            {
                return this.StateField;
            }
            set
            {
                this.StateField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<CIC.Bas.Modules.OpenBPE.Storage.Models.SupervisorTypes> SupervisorTypes
        {
            get
            {
                return this.SupervisorTypesField;
            }
            set
            {
                this.SupervisorTypesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> TokenId
        {
            get
            {
                return this.TokenIdField;
            }
            set
            {
                this.TokenIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> UserId
        {
            get
            {
                return this.UserIdField;
            }
            set
            {
                this.UserIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ModelBase", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.Storage.Models")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.Storage.Models.ProcessInstanceModel))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CIC.Bas.Modules.OpenBPE.Storage.Models.ListenerModel))]
    public partial class ModelBase : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private long IdField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.FlagsAttribute()]
    [System.Runtime.Serialization.DataContractAttribute(Name="ListenerReasons", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.Storage.Models")]
    public enum ListenerReasons : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        None = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        StartEvent = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        IncorrectSupervisor = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NoLanePermission = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WaitingForEvent = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PersistenceMode = 16,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RestorePoint = 32,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.FlagsAttribute()]
    [System.Runtime.Serialization.DataContractAttribute(Name="SupervisorTypes", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.Storage.Models")]
    public enum SupervisorTypes : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Generic = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FatClient = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        JobClient = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        OpenLeaseClient = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DatabaseTrigger = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Service = 16,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProcessInstanceState", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenBPE.Storage.Models")]
    public enum ProcessInstanceState : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReadyToExecute = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Executing = 10,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Killed = 20,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Executed = 30,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Error = 99,
    }
}
namespace CIC.Bas.Framework.Core.Extensions
{
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SortOrder", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Framework.Core.Extensions")]
    public enum SortOrder : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Ascending = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Descending = 1,
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.Payments
{
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BbImportType", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.Payments")]
    public enum BbImportType : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Camt53 = 100,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Camt54 = 101,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Camt52 = 102,
    }
}
namespace CIC.Bas.Modules.OpenLeaseCommon.PersonBrowser
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CustomizationWindowResponse", Namespace="http://schemas.datacontract.org/2004/07/CIC.Bas.Modules.OpenLeaseCommon.PersonBro" +
        "wser")]
    public partial class CustomizationWindowResponse : CIC.Bas.Framework.Extensibility.ResponseBase
    {
        
        private string ContentField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Content
        {
            get
            {
                return this.ContentField;
            }
            set
            {
                this.ContentField = value;
            }
        }
    }
}


namespace Cic.OpenOne.Common.MediatorService
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "MediatorService.IMediatorService")]
    public interface IMediatorService
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IMediatorService/Execute", ReplyAction = "http://tempuri.org/IMediatorService/ExecuteResponse")]
        CIC.Bas.Framework.Extensibility.ResponseBase Execute(CIC.Bas.Framework.Extensibility.RequestBase request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IMediatorService/Execute", ReplyAction = "http://tempuri.org/IMediatorService/ExecuteResponse")]
        System.Threading.Tasks.Task<CIC.Bas.Framework.Extensibility.ResponseBase> ExecuteAsync(CIC.Bas.Framework.Extensibility.RequestBase request);
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMediatorServiceChannel : IMediatorService, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MediatorServiceClient : System.ServiceModel.ClientBase<IMediatorService>, IMediatorService
    {

        public MediatorServiceClient()
        {
        }

        public MediatorServiceClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public MediatorServiceClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public MediatorServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public MediatorServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public CIC.Bas.Framework.Extensibility.ResponseBase Execute(CIC.Bas.Framework.Extensibility.RequestBase request)
        {
            return base.Channel.Execute(request);
        }

        public System.Threading.Tasks.Task<CIC.Bas.Framework.Extensibility.ResponseBase> ExecuteAsync(CIC.Bas.Framework.Extensibility.RequestBase request)
        {
            return base.Channel.ExecuteAsync(request);
        }
    }
}
