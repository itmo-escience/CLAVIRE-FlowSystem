﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Easis.Wfs.FlowSystemService.EventingService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://escience.ifmo.ru/easis/eventing", ConfigurationName="EventingService.IEventingBrokerService")]
    public interface IEventingBrokerService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://escience.ifmo.ru/easis/eventing/IEventingBrokerService/FireEvent", ReplyAction="http://escience.ifmo.ru/easis/eventing/IEventingBrokerService/FireEventResponse")]
        void FireEvent(Eventing.EventReport evt);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://escience.ifmo.ru/easis/eventing/IEventingBrokerService/Subscribe", ReplyAction="http://escience.ifmo.ru/easis/eventing/IEventingBrokerService/SubscribeResponse")]
        Eventing.SubscriptionId Subscribe(Eventing.SubscriptionRequest subscriptionRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://escience.ifmo.ru/easis/eventing/IEventingBrokerService/Unsubscribe", ReplyAction="http://escience.ifmo.ru/easis/eventing/IEventingBrokerService/UnsubscribeResponse" +
            "")]
        void Unsubscribe(Eventing.SubscriptionId subscriptionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://escience.ifmo.ru/easis/eventing/IEventingBrokerService/GetActiveSubscripti" +
            "ons", ReplyAction="http://escience.ifmo.ru/easis/eventing/IEventingBrokerService/GetActiveSubscripti" +
            "onsResponse")]
        Eventing.Subscription[] GetActiveSubscriptions();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEventingBrokerServiceChannel : Easis.Wfs.FlowSystemService.EventingService.IEventingBrokerService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EventingBrokerServiceClient : System.ServiceModel.ClientBase<Easis.Wfs.FlowSystemService.EventingService.IEventingBrokerService>, Easis.Wfs.FlowSystemService.EventingService.IEventingBrokerService {
        
        public EventingBrokerServiceClient() {
        }
        
        public EventingBrokerServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EventingBrokerServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EventingBrokerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EventingBrokerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void FireEvent(Eventing.EventReport evt) {
            base.Channel.FireEvent(evt);
        }
        
        public Eventing.SubscriptionId Subscribe(Eventing.SubscriptionRequest subscriptionRequest) {
            return base.Channel.Subscribe(subscriptionRequest);
        }
        
        public void Unsubscribe(Eventing.SubscriptionId subscriptionId) {
            base.Channel.Unsubscribe(subscriptionId);
        }
        
        public Eventing.Subscription[] GetActiveSubscriptions() {
            return base.Channel.GetActiveSubscriptions();
        }
    }
}
