using System;
using System.Collections.Generic;
using System.ServiceModel;
using vChat.Lib.Serialize;
using vChat.Model.Entities;

namespace vChat.Model
{
    [ServiceContract]    
    public interface IUserService
    {
        [OperationContract]
        XmlTextObject Info(int UserID);

        [OperationContract]
        XmlTextObject FriendList(int UserID);

        [OperationContract]
        MethodInvokeResult Login(String Username, String Password);

        [OperationContract]
        MethodInvokeResult Signup(String Username, String Password, String FirstName, String LastName, int QuestionID, String Answer, DateTime Birthdate);

        [OperationContract]
        MethodInvokeResult UserExist(String Username);

        [OperationContract]
        MethodInvokeResult ChangePassword(int UserID, String OldPassword, String NewPassword);

        [OperationContract]
        MethodInvokeResult Deactive(int UserID);

        [OperationContract]
        MethodInvokeResult Reactive(int UserID);

        [OperationContract]
        List<Conversation> GetConversations(int UserID);

        [OperationContract]
        List<Conversation> GetNewestConversations(int UserID);
    }
}
