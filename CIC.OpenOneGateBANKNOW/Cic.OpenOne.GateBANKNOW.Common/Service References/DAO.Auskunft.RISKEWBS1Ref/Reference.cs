﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DAO.Auskunft.RISKEWBS1Ref.ICicService")]
    public interface ICicService {
        
        // CODEGEN: Generating message contract since the operation CalculateWB is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICicService/CalculateWB", ReplyAction="http://tempuri.org/ICicService/CalculateWBResponse")]
        Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.CalculateWBResponse CalculateWB(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.SsInputParams request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICicService/Test", ReplyAction="http://tempuri.org/ICicService/TestResponse")]
        string Test();
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SsInputParams", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class SsInputParams {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public long SysAuskunft;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.IO.Stream input;
        
        public SsInputParams() {
        }
        
        public SsInputParams(long SysAuskunft, System.IO.Stream input) {
            this.SysAuskunft = SysAuskunft;
            this.input = input;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CalculateWBResponse {
        
        public CalculateWBResponse() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICicServiceChannel : Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.ICicService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CicServiceClient : System.ServiceModel.ClientBase<Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.ICicService>, Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.ICicService {
        
        public CicServiceClient() {
        }
        
        public CicServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CicServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CicServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CicServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.CalculateWBResponse Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.ICicService.CalculateWB(Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.SsInputParams request) {
            return base.Channel.CalculateWB(request);
        }
        
        public void CalculateWB(long SysAuskunft, System.IO.Stream input) {
            Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.SsInputParams inValue = new Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.SsInputParams();
            inValue.SysAuskunft = SysAuskunft;
            inValue.input = input;
            Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.CalculateWBResponse retVal = ((Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.RISKEWBS1Ref.ICicService)(this)).CalculateWB(inValue);
        }
        
        public string Test() {
            return base.Channel.Test();
        }
    }
}
