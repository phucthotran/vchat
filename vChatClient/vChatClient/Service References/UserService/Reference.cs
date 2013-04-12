﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vChat.UserService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UserService.IUserService")]
    public interface IUserService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Info", ReplyAction="http://tempuri.org/IUserService/InfoResponse")]
        vChat.Model.Entities.Users Info(int UserID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/FindName", ReplyAction="http://tempuri.org/IUserService/FindNameResponse")]
        vChat.Model.Entities.Users FindName(string Username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/FriendList", ReplyAction="http://tempuri.org/IUserService/FriendListResponse")]
        vChat.Model.GroupFriendList FriendList(int UserID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/AddFriend", ReplyAction="http://tempuri.org/IUserService/AddFriendResponse")]
        vChat.Model.MethodInvokeResult AddFriend(int UserID, string FriendName, int GroupID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Login", ReplyAction="http://tempuri.org/IUserService/LoginResponse")]
        vChat.Model.MethodInvokeResult Login(string Username, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Signup", ReplyAction="http://tempuri.org/IUserService/SignupResponse")]
        vChat.Model.MethodInvokeResult Signup(string Username, string Password, string FirstName, string LastName, int QuestionID, string Answer, System.DateTime Birthdate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/UserExist", ReplyAction="http://tempuri.org/IUserService/UserExistResponse")]
        vChat.Model.MethodInvokeResult UserExist(string Username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/ChangePassword", ReplyAction="http://tempuri.org/IUserService/ChangePasswordResponse")]
        vChat.Model.MethodInvokeResult ChangePassword(int UserID, string OldPassword, string NewPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Deactive", ReplyAction="http://tempuri.org/IUserService/DeactiveResponse")]
        vChat.Model.MethodInvokeResult Deactive(int UserID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Reactive", ReplyAction="http://tempuri.org/IUserService/ReactiveResponse")]
        vChat.Model.MethodInvokeResult Reactive(int UserID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/GetConversations", ReplyAction="http://tempuri.org/IUserService/GetConversationsResponse")]
        vChat.Model.Entities.Conversation[] GetConversations(int UserID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/GetNewestConversations", ReplyAction="http://tempuri.org/IUserService/GetNewestConversationsResponse")]
        vChat.Model.Entities.Conversation[] GetNewestConversations(int UserID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUserServiceChannel : vChat.UserService.IUserService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UserServiceClient : System.ServiceModel.ClientBase<vChat.UserService.IUserService>, vChat.UserService.IUserService {
        
        public UserServiceClient() {
        }
        
        public UserServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UserServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public vChat.Model.Entities.Users Info(int UserID) {
            return base.Channel.Info(UserID);
        }
        
        public vChat.Model.Entities.Users FindName(string Username) {
            return base.Channel.FindName(Username);
        }
        
        public vChat.Model.GroupFriendList FriendList(int UserID) {
            return base.Channel.FriendList(UserID);
        }
        
        public vChat.Model.MethodInvokeResult AddFriend(int UserID, string FriendName, int GroupID) {
            return base.Channel.AddFriend(UserID, FriendName, GroupID);
        }
        
        public vChat.Model.MethodInvokeResult Login(string Username, string Password) {
            return base.Channel.Login(Username, Password);
        }
        
        public vChat.Model.MethodInvokeResult Signup(string Username, string Password, string FirstName, string LastName, int QuestionID, string Answer, System.DateTime Birthdate) {
            return base.Channel.Signup(Username, Password, FirstName, LastName, QuestionID, Answer, Birthdate);
        }
        
        public vChat.Model.MethodInvokeResult UserExist(string Username) {
            return base.Channel.UserExist(Username);
        }
        
        public vChat.Model.MethodInvokeResult ChangePassword(int UserID, string OldPassword, string NewPassword) {
            return base.Channel.ChangePassword(UserID, OldPassword, NewPassword);
        }
        
        public vChat.Model.MethodInvokeResult Deactive(int UserID) {
            return base.Channel.Deactive(UserID);
        }
        
        public vChat.Model.MethodInvokeResult Reactive(int UserID) {
            return base.Channel.Reactive(UserID);
        }
        
        public vChat.Model.Entities.Conversation[] GetConversations(int UserID) {
            return base.Channel.GetConversations(UserID);
        }
        
        public vChat.Model.Entities.Conversation[] GetNewestConversations(int UserID) {
            return base.Channel.GetNewestConversations(UserID);
        }
    }
}
