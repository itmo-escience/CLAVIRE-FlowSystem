﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.208
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WFStarter.WFS {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Notification", Namespace="http://escience.ifmo.ru/nano/services/")]
    [System.SerializableAttribute()]
    public partial class Notification : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WFStarter.WFS.EventReport EventField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WFStarter.WFS.SubscriptionId SubscriptionIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TagField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public WFStarter.WFS.EventReport Event {
            get {
                return this.EventField;
            }
            set {
                if ((object.ReferenceEquals(this.EventField, value) != true)) {
                    this.EventField = value;
                    this.RaisePropertyChanged("Event");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public WFStarter.WFS.SubscriptionId SubscriptionId {
            get {
                return this.SubscriptionIdField;
            }
            set {
                if ((object.ReferenceEquals(this.SubscriptionIdField, value) != true)) {
                    this.SubscriptionIdField = value;
                    this.RaisePropertyChanged("SubscriptionId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Tag {
            get {
                return this.TagField;
            }
            set {
                if ((object.ReferenceEquals(this.TagField, value) != true)) {
                    this.TagField = value;
                    this.RaisePropertyChanged("Tag");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="EventReport", Namespace="http://escience.ifmo.ru/nano/services/")]
    [System.SerializableAttribute()]
    public partial class EventReport : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BodyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SchemeUriField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SourceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime TimestampField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TopicField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Body {
            get {
                return this.BodyField;
            }
            set {
                if ((object.ReferenceEquals(this.BodyField, value) != true)) {
                    this.BodyField = value;
                    this.RaisePropertyChanged("Body");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SchemeUri {
            get {
                return this.SchemeUriField;
            }
            set {
                if ((object.ReferenceEquals(this.SchemeUriField, value) != true)) {
                    this.SchemeUriField = value;
                    this.RaisePropertyChanged("SchemeUri");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Source {
            get {
                return this.SourceField;
            }
            set {
                if ((object.ReferenceEquals(this.SourceField, value) != true)) {
                    this.SourceField = value;
                    this.RaisePropertyChanged("Source");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Timestamp {
            get {
                return this.TimestampField;
            }
            set {
                if ((this.TimestampField.Equals(value) != true)) {
                    this.TimestampField = value;
                    this.RaisePropertyChanged("Timestamp");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Topic {
            get {
                return this.TopicField;
            }
            set {
                if ((object.ReferenceEquals(this.TopicField, value) != true)) {
                    this.TopicField = value;
                    this.RaisePropertyChanged("Topic");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="SubscriptionId", Namespace="http://escience.ifmo.ru/nano/services/")]
    [System.SerializableAttribute()]
    public partial class SubscriptionId : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Value {
            get {
                return this.ValueField;
            }
            set {
                if ((this.ValueField.Equals(value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="JobRequest", Namespace="http://schemas.datacontract.org/2004/07/Easis.Wfs.FlowSystemService")]
    [System.SerializableAttribute()]
    public partial class JobRequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FlowExecutionPropertiesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ScriptField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ScriptDataContextField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FlowExecutionProperties {
            get {
                return this.FlowExecutionPropertiesField;
            }
            set {
                if ((object.ReferenceEquals(this.FlowExecutionPropertiesField, value) != true)) {
                    this.FlowExecutionPropertiesField = value;
                    this.RaisePropertyChanged("FlowExecutionProperties");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Script {
            get {
                return this.ScriptField;
            }
            set {
                if ((object.ReferenceEquals(this.ScriptField, value) != true)) {
                    this.ScriptField = value;
                    this.RaisePropertyChanged("Script");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ScriptDataContext {
            get {
                return this.ScriptDataContextField;
            }
            set {
                if ((object.ReferenceEquals(this.ScriptDataContextField, value) != true)) {
                    this.ScriptDataContextField = value;
                    this.RaisePropertyChanged("ScriptDataContext");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WFS.IFlowSystemService")]
    public interface IFlowSystemService {
        
        // CODEGEN: Generating message contract since the wrapper namespace (http://escience.ifmo.ru/nano/services/) of message NotifyRequest does not match the default value (http://tempuri.org/)
        [System.ServiceModel.OperationContractAttribute(Action="http://escience.ifmo.ru/nano/services/Notify", ReplyAction="http://escience.ifmo.ru/nano/services/INotifiable/NotifyResponse")]
        WFStarter.WFS.NotifyResponse Notify(WFStarter.WFS.NotifyRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFlowSystemService/PushJob", ReplyAction="http://tempuri.org/IFlowSystemService/PushJobResponse")]
        int PushJob(WFStarter.WFS.JobRequest jobRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFlowSystemService/GetStatus", ReplyAction="http://tempuri.org/IFlowSystemService/GetStatusResponse")]
        string GetStatus(int wfId);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Notify", WrapperNamespace="http://escience.ifmo.ru/nano/services/", IsWrapped=true)]
    public partial class NotifyRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://escience.ifmo.ru/nano/services/", Order=0)]
        public WFStarter.WFS.Notification notification;
        
        public NotifyRequest() {
        }
        
        public NotifyRequest(WFStarter.WFS.Notification notification) {
            this.notification = notification;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="NotifyResponse", WrapperNamespace="http://escience.ifmo.ru/nano/services/", IsWrapped=true)]
    public partial class NotifyResponse {
        
        public NotifyResponse() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFlowSystemServiceChannel : WFStarter.WFS.IFlowSystemService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FlowSystemServiceClient : System.ServiceModel.ClientBase<WFStarter.WFS.IFlowSystemService>, WFStarter.WFS.IFlowSystemService {
        
        public FlowSystemServiceClient() {
        }
        
        public FlowSystemServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FlowSystemServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FlowSystemServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FlowSystemServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WFStarter.WFS.NotifyResponse WFStarter.WFS.IFlowSystemService.Notify(WFStarter.WFS.NotifyRequest request) {
            return base.Channel.Notify(request);
        }
        
        public void Notify(WFStarter.WFS.Notification notification) {
            WFStarter.WFS.NotifyRequest inValue = new WFStarter.WFS.NotifyRequest();
            inValue.notification = notification;
            WFStarter.WFS.NotifyResponse retVal = ((WFStarter.WFS.IFlowSystemService)(this)).Notify(inValue);
        }
        
        public int PushJob(WFStarter.WFS.JobRequest jobRequest) {
            return base.Channel.PushJob(jobRequest);
        }
        
        public string GetStatus(int wfId) {
            return base.Channel.GetStatus(wfId);
        }
    }
}