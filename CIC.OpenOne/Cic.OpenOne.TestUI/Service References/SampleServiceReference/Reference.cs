﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cic.OpenOne.TestUI.SampleServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SampleServiceReference.ISampleService")]
    public interface ISampleService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/sampleMethod", ReplyAction="http://tempuri.org/ISampleService/sampleMethodResponse")]
        Cic.OpenOne.Service.DTO.oSampleMethodDto sampleMethod(Cic.OpenOne.Service.DTO.iSampleMethodDto sampleParameter);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISampleServiceChannel : Cic.OpenOne.TestUI.SampleServiceReference.ISampleService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SampleServiceClient : System.ServiceModel.ClientBase<Cic.OpenOne.TestUI.SampleServiceReference.ISampleService>, Cic.OpenOne.TestUI.SampleServiceReference.ISampleService {
        
        public SampleServiceClient() {
        }
        
        public SampleServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SampleServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SampleServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SampleServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Cic.OpenOne.Service.DTO.oSampleMethodDto sampleMethod(Cic.OpenOne.Service.DTO.iSampleMethodDto sampleParameter) {
            return base.Channel.sampleMethod(sampleParameter);
        }
    }
}
