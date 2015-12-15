﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Easis.Wfs.FlowSystemService.MonitoringFacade {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ObjectId", Namespace="http://schemas.datacontract.org/2004/07/MongoDB.Bson")]
    [System.SerializableAttribute()]
    public partial struct ObjectId : System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int _incrementField;
        
        private int _machineField;
        
        private short _pidField;
        
        private int _timestampField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int _increment {
            get {
                return this._incrementField;
            }
            set {
                if ((this._incrementField.Equals(value) != true)) {
                    this._incrementField = value;
                    this.RaisePropertyChanged("_increment");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int _machine {
            get {
                return this._machineField;
            }
            set {
                if ((this._machineField.Equals(value) != true)) {
                    this._machineField = value;
                    this.RaisePropertyChanged("_machine");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public short _pid {
            get {
                return this._pidField;
            }
            set {
                if ((this._pidField.Equals(value) != true)) {
                    this._pidField = value;
                    this.RaisePropertyChanged("_pid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int _timestamp {
            get {
                return this._timestampField;
            }
            set {
                if ((this._timestampField.Equals(value) != true)) {
                    this._timestampField = value;
                    this.RaisePropertyChanged("_timestamp");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WhereStatement", Namespace="http://schemas.datacontract.org/2004/07/Easis.Monitoring.MonitoringService")]
    [System.SerializableAttribute()]
    public partial class WhereStatement : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ColumnField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Easis.Wfs.FlowSystemService.MonitoringFacade.WhereOperator[] operatorsField;
        
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
        public string Column {
            get {
                return this.ColumnField;
            }
            set {
                if ((object.ReferenceEquals(this.ColumnField, value) != true)) {
                    this.ColumnField = value;
                    this.RaisePropertyChanged("Column");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Easis.Wfs.FlowSystemService.MonitoringFacade.WhereOperator[] operators {
            get {
                return this.operatorsField;
            }
            set {
                if ((object.ReferenceEquals(this.operatorsField, value) != true)) {
                    this.operatorsField = value;
                    this.RaisePropertyChanged("operators");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="WhereOperator", Namespace="http://schemas.datacontract.org/2004/07/Easis.Monitoring.MonitoringService")]
    [System.SerializableAttribute()]
    public partial class WhereOperator : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Easis.Wfs.FlowSystemService.MonitoringFacade.WhereOperation OperationField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] ValuesField;
        
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
        public Easis.Wfs.FlowSystemService.MonitoringFacade.WhereOperation Operation {
            get {
                return this.OperationField;
            }
            set {
                if ((this.OperationField.Equals(value) != true)) {
                    this.OperationField = value;
                    this.RaisePropertyChanged("Operation");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Values {
            get {
                return this.ValuesField;
            }
            set {
                if ((object.ReferenceEquals(this.ValuesField, value) != true)) {
                    this.ValuesField = value;
                    this.RaisePropertyChanged("Values");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WhereOperation", Namespace="http://schemas.datacontract.org/2004/07/Easis.Monitoring.MonitoringService")]
    public enum WhereOperation : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        GREATER = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        GREATEREQUAL = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LOWER = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LOWEREQUAL = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EQUAL = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        IN = 5,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="OrderStatement", Namespace="http://schemas.datacontract.org/2004/07/Easis.Monitoring.MonitoringService")]
    [System.SerializableAttribute()]
    public partial class OrderStatement : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ColumnField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Easis.Wfs.FlowSystemService.MonitoringFacade.DirectionOperation DirectionField;
        
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
        public string Column {
            get {
                return this.ColumnField;
            }
            set {
                if ((object.ReferenceEquals(this.ColumnField, value) != true)) {
                    this.ColumnField = value;
                    this.RaisePropertyChanged("Column");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Easis.Wfs.FlowSystemService.MonitoringFacade.DirectionOperation Direction {
            get {
                return this.DirectionField;
            }
            set {
                if ((this.DirectionField.Equals(value) != true)) {
                    this.DirectionField = value;
                    this.RaisePropertyChanged("Direction");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DirectionOperation", Namespace="http://schemas.datacontract.org/2004/07/Easis.Monitoring.MonitoringService")]
    public enum DirectionOperation : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ASC = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DESC = 1,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JobSubset", Namespace="http://schemas.datacontract.org/2004/07/Easis.Monitoring.MonitoringService")]
    [System.SerializableAttribute()]
    public partial class JobSubset : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Easis.Wfs.FlowSystemService.MonitoringFacade.ShortJobDescription[] JobsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long TotalCountField;
        
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
        public Easis.Wfs.FlowSystemService.MonitoringFacade.ShortJobDescription[] Jobs {
            get {
                return this.JobsField;
            }
            set {
                if ((object.ReferenceEquals(this.JobsField, value) != true)) {
                    this.JobsField = value;
                    this.RaisePropertyChanged("Jobs");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long TotalCount {
            get {
                return this.TotalCountField;
            }
            set {
                if ((this.TotalCountField.Equals(value) != true)) {
                    this.TotalCountField = value;
                    this.RaisePropertyChanged("TotalCount");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ShortJobDescription", Namespace="http://schemas.datacontract.org/2004/07/Easis.Monitoring.MonitoringService")]
    [System.SerializableAttribute()]
    public partial class ShortJobDescription : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CommentField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double DurationInSecField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorCommentField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorExceptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> FinishedAtField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int NumberOfStepsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] PackagesUsedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> PushedAtField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] ResourcesUsedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> StartedAtField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Easis.Common.DataContracts.JobState StateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string VerboseErrorCommentField;
        
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
        public string Comment {
            get {
                return this.CommentField;
            }
            set {
                if ((object.ReferenceEquals(this.CommentField, value) != true)) {
                    this.CommentField = value;
                    this.RaisePropertyChanged("Comment");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double DurationInSec {
            get {
                return this.DurationInSecField;
            }
            set {
                if ((this.DurationInSecField.Equals(value) != true)) {
                    this.DurationInSecField = value;
                    this.RaisePropertyChanged("DurationInSec");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorComment {
            get {
                return this.ErrorCommentField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorCommentField, value) != true)) {
                    this.ErrorCommentField = value;
                    this.RaisePropertyChanged("ErrorComment");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorException {
            get {
                return this.ErrorExceptionField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorExceptionField, value) != true)) {
                    this.ErrorExceptionField = value;
                    this.RaisePropertyChanged("ErrorException");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> FinishedAt {
            get {
                return this.FinishedAtField;
            }
            set {
                if ((this.FinishedAtField.Equals(value) != true)) {
                    this.FinishedAtField = value;
                    this.RaisePropertyChanged("FinishedAt");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ID {
            get {
                return this.IDField;
            }
            set {
                if ((object.ReferenceEquals(this.IDField, value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int NumberOfSteps {
            get {
                return this.NumberOfStepsField;
            }
            set {
                if ((this.NumberOfStepsField.Equals(value) != true)) {
                    this.NumberOfStepsField = value;
                    this.RaisePropertyChanged("NumberOfSteps");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] PackagesUsed {
            get {
                return this.PackagesUsedField;
            }
            set {
                if ((object.ReferenceEquals(this.PackagesUsedField, value) != true)) {
                    this.PackagesUsedField = value;
                    this.RaisePropertyChanged("PackagesUsed");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> PushedAt {
            get {
                return this.PushedAtField;
            }
            set {
                if ((this.PushedAtField.Equals(value) != true)) {
                    this.PushedAtField = value;
                    this.RaisePropertyChanged("PushedAt");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] ResourcesUsed {
            get {
                return this.ResourcesUsedField;
            }
            set {
                if ((object.ReferenceEquals(this.ResourcesUsedField, value) != true)) {
                    this.ResourcesUsedField = value;
                    this.RaisePropertyChanged("ResourcesUsed");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> StartedAt {
            get {
                return this.StartedAtField;
            }
            set {
                if ((this.StartedAtField.Equals(value) != true)) {
                    this.StartedAtField = value;
                    this.RaisePropertyChanged("StartedAt");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Easis.Common.DataContracts.JobState State {
            get {
                return this.StateField;
            }
            set {
                if ((this.StateField.Equals(value) != true)) {
                    this.StateField = value;
                    this.RaisePropertyChanged("State");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
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
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string VerboseErrorComment {
            get {
                return this.VerboseErrorCommentField;
            }
            set {
                if ((object.ReferenceEquals(this.VerboseErrorCommentField, value) != true)) {
                    this.VerboseErrorCommentField = value;
                    this.RaisePropertyChanged("VerboseErrorComment");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MonitoringFacade.IMonitoringService")]
    public interface IMonitoringService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMonitoringService/GetActualDataInJson", ReplyAction="http://tempuri.org/IMonitoringService/GetActualDataInJsonResponse")]
        string GetActualDataInJson(string infoPath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMonitoringService/GetJobInfoInJson", ReplyAction="http://tempuri.org/IMonitoringService/GetJobInfoInJsonResponse")]
        string GetJobInfoInJson(System.Guid jid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMonitoringService/GetResourcesInfoInJson", ReplyAction="http://tempuri.org/IMonitoringService/GetResourcesInfoInJsonResponse")]
        string GetResourcesInfoInJson();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMonitoringService/GetResourcesInfo", ReplyAction="http://tempuri.org/IMonitoringService/GetResourcesInfoResponse")]
        Easis.Common.DataContracts.ResourceDescription GetResourcesInfo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMonitoringService/GetJobInfo", ReplyAction="http://tempuri.org/IMonitoringService/GetJobInfoResponse")]
        Easis.Common.DataContracts.JobDescription GetJobInfo(System.Guid jid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMonitoringService/PutJobResultInBson", ReplyAction="http://tempuri.org/IMonitoringService/PutJobResultInBsonResponse")]
        void PutJobResultInBson(byte[] jr);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMonitoringService/PutJobResult", ReplyAction="http://tempuri.org/IMonitoringService/PutJobResultResponse")]
        void PutJobResult(Easis.Common.DataContracts.JobDescription jd);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMonitoringService/GetHistoryInJson", ReplyAction="http://tempuri.org/IMonitoringService/GetHistoryInJsonResponse")]
        string GetHistoryInJson(string infoPath, System.DateTime from, System.DateTime to);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMonitoringService/SelectFinishedJobs", ReplyAction="http://tempuri.org/IMonitoringService/SelectFinishedJobsResponse")]
        Easis.Wfs.FlowSystemService.MonitoringFacade.JobSubset SelectFinishedJobs(Easis.Wfs.FlowSystemService.MonitoringFacade.WhereStatement[] where, Easis.Wfs.FlowSystemService.MonitoringFacade.OrderStatement[] order, int page, int limit);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMonitoringServiceChannel : Easis.Wfs.FlowSystemService.MonitoringFacade.IMonitoringService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MonitoringServiceClient : System.ServiceModel.ClientBase<Easis.Wfs.FlowSystemService.MonitoringFacade.IMonitoringService>, Easis.Wfs.FlowSystemService.MonitoringFacade.IMonitoringService {
        
        public MonitoringServiceClient() {
        }
        
        public MonitoringServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MonitoringServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MonitoringServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MonitoringServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetActualDataInJson(string infoPath) {
            return base.Channel.GetActualDataInJson(infoPath);
        }
        
        public string GetJobInfoInJson(System.Guid jid) {
            return base.Channel.GetJobInfoInJson(jid);
        }
        
        public string GetResourcesInfoInJson() {
            return base.Channel.GetResourcesInfoInJson();
        }
        
        public Easis.Common.DataContracts.ResourceDescription GetResourcesInfo() {
            return base.Channel.GetResourcesInfo();
        }
        
        public Easis.Common.DataContracts.JobDescription GetJobInfo(System.Guid jid) {
            return base.Channel.GetJobInfo(jid);
        }
        
        public void PutJobResultInBson(byte[] jr) {
            base.Channel.PutJobResultInBson(jr);
        }
        
        public void PutJobResult(Easis.Common.DataContracts.JobDescription jd) {
            base.Channel.PutJobResult(jd);
        }
        
        public string GetHistoryInJson(string infoPath, System.DateTime from, System.DateTime to) {
            return base.Channel.GetHistoryInJson(infoPath, from, to);
        }
        
        public Easis.Wfs.FlowSystemService.MonitoringFacade.JobSubset SelectFinishedJobs(Easis.Wfs.FlowSystemService.MonitoringFacade.WhereStatement[] where, Easis.Wfs.FlowSystemService.MonitoringFacade.OrderStatement[] order, int page, int limit) {
            return base.Channel.SelectFinishedJobs(where, order, page, limit);
        }
    }
}
