﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://guardean.net/webservices/workflow/wkf_status_update/types")]
    public partial class systemMessageType : ValidatableDataContract {
        
        private string messageField;
        
        private string codeField;
        
        private string[] parameterField;
        
        private systemMessageType causeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
                this.RaisePropertyChanged("code");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("parameter", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string[] parameter {
            get {
                return this.parameterField;
            }
            set {
                this.parameterField = value;
                this.RaisePropertyChanged("parameter");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public systemMessageType cause {
            get {
                return this.causeField;
            }
            set {
                this.causeField = value;
                this.RaisePropertyChanged("cause");
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://guardean.net/webservices/workflow/wkf_status_update/types")]
    public partial class executeResponse : ValidatableDataContract {
        
        private bool statusResponseField;
        
        private string statusResponseTextField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public bool statusResponse {
            get {
                return this.statusResponseField;
            }
            set {
                this.statusResponseField = value;
                this.RaisePropertyChanged("statusResponse");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string statusResponseText {
            get {
                return this.statusResponseTextField;
            }
            set {
                this.statusResponseTextField = value;
                this.RaisePropertyChanged("statusResponseText");
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://guardean.net/webservices/workflow/wkf_status_update/types")]
    public partial class TechnicalInformationStruct : ValidatableDataContract {
        
        private string applicationIDField;
        
        private string externalReferenceField;
        
        private string statusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string applicationID {
            get {
                return this.applicationIDField;
            }
            set {
                this.applicationIDField = value;
                this.RaisePropertyChanged("applicationID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string externalReference {
            get {
                return this.externalReferenceField;
            }
            set {
                this.externalReferenceField = value;
                this.RaisePropertyChanged("externalReference");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
                this.RaisePropertyChanged("status");
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://guardean.net/webservices/workflow/wkf_status_update/types")]
    public partial class BusinessInformationStruct : ValidatableDataContract {
        
        private System.DateTime statusUpdateDateTimeField;
        
        private string statusUpdateDescriptionField;
        
        private string statusUpdateEditorField;
        
        private string statusUpdateSystemField;
        
        private string statusUpdateTextField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public System.DateTime statusUpdateDateTime {
            get {
                return this.statusUpdateDateTimeField;
            }
            set {
                this.statusUpdateDateTimeField = value;
                this.RaisePropertyChanged("statusUpdateDateTime");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string statusUpdateDescription {
            get {
                return this.statusUpdateDescriptionField;
            }
            set {
                this.statusUpdateDescriptionField = value;
                this.RaisePropertyChanged("statusUpdateDescription");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string statusUpdateEditor {
            get {
                return this.statusUpdateEditorField;
            }
            set {
                this.statusUpdateEditorField = value;
                this.RaisePropertyChanged("statusUpdateEditor");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string statusUpdateSystem {
            get {
                return this.statusUpdateSystemField;
            }
            set {
                this.statusUpdateSystemField = value;
                this.RaisePropertyChanged("statusUpdateSystem");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string statusUpdateText {
            get {
                return this.statusUpdateTextField;
            }
            set {
                this.statusUpdateTextField = value;
                this.RaisePropertyChanged("statusUpdateText");
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://guardean.net/webservices/workflow/wkf_status_update/types")]
    public partial class executeRequest : ValidatableDataContract {
        
        private BusinessInformationStruct businessInformationField;
        
        private TechnicalInformationStruct technicalInformationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public BusinessInformationStruct businessInformation {
            get {
                return this.businessInformationField;
            }
            set {
                this.businessInformationField = value;
                this.RaisePropertyChanged("businessInformation");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public TechnicalInformationStruct technicalInformation {
            get {
                return this.technicalInformationField;
            }
            set {
                this.technicalInformationField = value;
                this.RaisePropertyChanged("technicalInformation");
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://guardean.net/webservices/workflow/wkf_status_update", ConfigurationName="GuardeanStatusUpdateServiceReference.Wkf_Status_UpdatePortType")]
    public interface Wkf_Status_UpdatePortType {
        
        // CODEGEN: Generating message contract since the operation execute is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://guardean.net/webservices/workflow/wkf_status_update/execute", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.systemMessageType), Action="http://guardean.net/webservices/workflow/wkf_status_update/execute", Name="systemMessage", Namespace="http://guardean.net/webservices/workflow/wkf_status_update/types")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeResponse1 execute(Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://guardean.net/webservices/workflow/wkf_status_update/execute", ReplyAction="*")]
        System.Threading.Tasks.Task<Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeResponse1> executeAsync(Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest1 request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class executeRequest1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://guardean.net/webservices/workflow/wkf_status_update/types", Order=0)]
        public Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest executeRequest;
        
        public executeRequest1() {
        }
        
        public executeRequest1(Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest executeRequest) {
            this.executeRequest = executeRequest;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class executeResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://guardean.net/webservices/workflow/wkf_status_update/types", Order=0)]
        public Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeResponse executeResponse;
        
        public executeResponse1() {
        }
        
        public executeResponse1(Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeResponse executeResponse) {
            this.executeResponse = executeResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface Wkf_Status_UpdatePortTypeChannel : Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.Wkf_Status_UpdatePortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Wkf_Status_UpdatePortTypeClient : System.ServiceModel.ClientBase<Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.Wkf_Status_UpdatePortType>, Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.Wkf_Status_UpdatePortType {
        
        public Wkf_Status_UpdatePortTypeClient() {
        }
        
        public Wkf_Status_UpdatePortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Wkf_Status_UpdatePortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Wkf_Status_UpdatePortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Wkf_Status_UpdatePortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeResponse1 Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.Wkf_Status_UpdatePortType.execute(Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest1 request) {
            return base.Channel.execute(request);
        }
        
        public Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeResponse execute(Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest executeRequest) {
            Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest1 inValue = new Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest1();
            inValue.executeRequest = executeRequest;
            Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeResponse1 retVal = ((Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.Wkf_Status_UpdatePortType)(this)).execute(inValue);
            return retVal.executeResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeResponse1> Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.Wkf_Status_UpdatePortType.executeAsync(Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest1 request) {
            return base.Channel.executeAsync(request);
        }
        
        public System.Threading.Tasks.Task<Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeResponse1> executeAsync(Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest executeRequest) {
            Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest1 inValue = new Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest1();
            inValue.executeRequest = executeRequest;
            return ((Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.Wkf_Status_UpdatePortType)(this)).executeAsync(inValue);
        }
    }
}
