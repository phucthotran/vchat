using System;
using System.Collections.Generic;
using System.ServiceModel;
using vChat.Lib.Serialize;
using vChat.Model.Entities;
using System.Collections.ObjectModel;

namespace vChat.Model
{
    [ServiceContract]    
    public interface IUserService
    {
        [OperationContract]
        Users Info(int UserID);

        [OperationContract]
        FriendGroup GroupInfo(int GroupID);

        [OperationContract]
        Users FindName(String Username);

        [OperationContract]
        GroupFriendList FriendList(int UserID);

        [OperationContract]
        List<Users> FriendRequests(int UserID);

        [OperationContract]
        List<Users> UnresponseFriendRequests(int UserID);

        [OperationContract]
        MethodInvokeResult AddFriend(int UserID, String FriendName, int GroupID);

        [OperationContract]
        MethodInvokeResult AddGroup(int UserID, String Name, ref int NewGroupID);

        [OperationContract]
        MethodInvokeResult RemoveGroup(int GroupID, bool RemoveContact);

        [OperationContract]
        MethodInvokeResult Login(String Username, String Password);

        [OperationContract]
        MethodInvokeResult LoginHash(String Username, String Password);

        [OperationContract]
        MethodInvokeResult Signup(String Username, String Password, String FirstName, String LastName, int QuestionID, String Answer, DateTime Birthdate);

        [OperationContract]
        MethodInvokeResult UserExist(String Username);

        [OperationContract]
        MethodInvokeResult ChangeAvatar(int UserID, byte[] ImageBytes);

        [OperationContract]
        MethodInvokeResult AnswerIsMatch(int UserID, int QuestionID, String Answer);

        [OperationContract]
        MethodInvokeResult ChangePassword(int UserID, String OldPassword, String NewPassword);

        [OperationContract]
        MethodInvokeResult ChangeUserInfo(int UserID, String FirstName, String LastName, int QuestionID, String Answer, DateTime Birthdate);

        [OperationContract]
        MethodInvokeResult AcceptFriendRequest(int UserID, int FriendID, int GroupID);

        [OperationContract]
        MethodInvokeResult IgnoreFriendRequest(int UserID, int FriendID);

        [OperationContract]
        MethodInvokeResult MoveContact(int UserID, int FriendID, int NewGroupID);

        [OperationContract]
        MethodInvokeResult RemoveContact(int UserID, int FriendID);

        [OperationContract]
        MethodInvokeResult AlreadyMakeFriend(int UserID, int FriendID);

        [OperationContract]
        MethodInvokeResult Deactive(int UserID);

        [OperationContract]
        MethodInvokeResult Reactive(int UserID);

        [OperationContract]
        List<Question> GetAllQuestion();

        [OperationContract]
        MethodInvokeResult SaveConversation(int UserID, int FriendID, String Content, DateTime Time, ref int ConversationID);

        [OperationContract]
        MethodInvokeResult MarkAsReadConversation(int ConversationID);

        [OperationContract(Name = "GetConversations")]
        List<Conversation> GetConversations(int UserID);

        [OperationContract(Name = "GetConversationsByRange")]
        List<Conversation> GetConversations(int UserID, int BeginIndex, int EndIndex);

        [OperationContract(Name = "GetConversationBetween")]
        List<Conversation> GetConversationBetween(int UserID, int FriendID);

        [OperationContract(Name = "GetConversationBetweenByRange")]
        List<Conversation> GetConversationBetween(int UserID, int FriendID, int BeginIndex, int EndIndex);

        [OperationContract(Name = "GetNewestConversations")]
        List<Conversation> GetNewestConversations(int UserID);

        [OperationContract(Name = "GetNewestConversationsByRange")]
        List<Conversation> GetNewestConversations(int UserID, int BeginIndex, int EndIndex);
    }
}
