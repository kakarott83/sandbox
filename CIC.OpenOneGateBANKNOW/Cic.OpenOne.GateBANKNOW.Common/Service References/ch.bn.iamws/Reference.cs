﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="B2BResponseContract", Namespace="http://schemas.datacontract.org/2004/07/IAMWS")]
    [System.SerializableAttribute()]
    public partial class B2BResponseContract : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BStatusContract StatusField;
        
        private Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BContract[] UsersField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BStatusContract Status {
            get {
                return this.StatusField;
            }
            set {
                if ((object.ReferenceEquals(this.StatusField, value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BContract[] Users {
            get {
                return this.UsersField;
            }
            set {
                if ((object.ReferenceEquals(this.UsersField, value) != true)) {
                    this.UsersField = value;
                    this.RaisePropertyChanged("Users");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="B2BStatusContract", Namespace="http://schemas.datacontract.org/2004/07/IAMWS")]
    [System.SerializableAttribute()]
    public partial class B2BStatusContract : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string MessageField;
        
        private string SuccessField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Success {
            get {
                return this.SuccessField;
            }
            set {
                if ((object.ReferenceEquals(this.SuccessField, value) != true)) {
                    this.SuccessField = value;
                    this.RaisePropertyChanged("Success");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="B2BContract", Namespace="http://schemas.datacontract.org/2004/07/IAMWS")]
    [System.SerializableAttribute()]
    public partial class B2BContract : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string ApplicationIdField;
        
        private string CreatedField;
        
        private string LanguageField;
        
        private string MailField;
        
        private string MobileField;
        
        private string ModifiedField;
        
        private string PasswordField;
        
        private string StatusField;
        
        private string TokenField;
        
        private string UserIdField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string ApplicationId {
            get {
                return this.ApplicationIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ApplicationIdField, value) != true)) {
                    this.ApplicationIdField = value;
                    this.RaisePropertyChanged("ApplicationId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Created {
            get {
                return this.CreatedField;
            }
            set {
                if ((object.ReferenceEquals(this.CreatedField, value) != true)) {
                    this.CreatedField = value;
                    this.RaisePropertyChanged("Created");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Language {
            get {
                return this.LanguageField;
            }
            set {
                if ((object.ReferenceEquals(this.LanguageField, value) != true)) {
                    this.LanguageField = value;
                    this.RaisePropertyChanged("Language");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Mail {
            get {
                return this.MailField;
            }
            set {
                if ((object.ReferenceEquals(this.MailField, value) != true)) {
                    this.MailField = value;
                    this.RaisePropertyChanged("Mail");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Mobile {
            get {
                return this.MobileField;
            }
            set {
                if ((object.ReferenceEquals(this.MobileField, value) != true)) {
                    this.MobileField = value;
                    this.RaisePropertyChanged("Mobile");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Modified {
            get {
                return this.ModifiedField;
            }
            set {
                if ((object.ReferenceEquals(this.ModifiedField, value) != true)) {
                    this.ModifiedField = value;
                    this.RaisePropertyChanged("Modified");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Password {
            get {
                return this.PasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.PasswordField, value) != true)) {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Status {
            get {
                return this.StatusField;
            }
            set {
                if ((object.ReferenceEquals(this.StatusField, value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Token {
            get {
                return this.TokenField;
            }
            set {
                if ((object.ReferenceEquals(this.TokenField, value) != true)) {
                    this.TokenField = value;
                    this.RaisePropertyChanged("Token");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string UserId {
            get {
                return this.UserIdField;
            }
            set {
                if ((object.ReferenceEquals(this.UserIdField, value) != true)) {
                    this.UserIdField = value;
                    this.RaisePropertyChanged("UserId");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ch.bn.iamws.IB2BOL")]
    public interface IB2BOL {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IB2BOL/GetUser", ReplyAction="http://tempuri.org/IB2BOL/GetUserResponse")]
        Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BResponseContract GetUser(string ApplicationId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IB2BOL/SetUser", ReplyAction="http://tempuri.org/IB2BOL/SetUserResponse")]
        Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BResponseContract SetUser(Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BContract user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IB2BOL/CreateUser", ReplyAction="http://tempuri.org/IB2BOL/CreateUserResponse")]
        Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BResponseContract CreateUser(Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BContract user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IB2BOL/SetPassword", ReplyAction="http://tempuri.org/IB2BOL/SetPasswordResponse")]
        Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BStatusContract SetPassword(string ApplicationId, string OldPassword, string NewPassword);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IB2BOLChannel : Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.IB2BOL, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class B2BOLClient : System.ServiceModel.ClientBase<Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.IB2BOL>, Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.IB2BOL {
        
        public B2BOLClient() {
        }
        
        public B2BOLClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public B2BOLClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public B2BOLClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public B2BOLClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BResponseContract GetUser(string ApplicationId) {
            return base.Channel.GetUser(ApplicationId);
        }
        
        public Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BResponseContract SetUser(Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BContract user) {
            return base.Channel.SetUser(user);
        }
        
        public Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BResponseContract CreateUser(Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BContract user) {
            return base.Channel.CreateUser(user);
        }
        
        public Cic.OpenOne.GateBANKNOW.Common.ch.bn.iamws.B2BStatusContract SetPassword(string ApplicationId, string OldPassword, string NewPassword) {
            return base.Channel.SetPassword(ApplicationId, OldPassword, NewPassword);
        }
    }
}
