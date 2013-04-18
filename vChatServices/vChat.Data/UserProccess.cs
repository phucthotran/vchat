using System;
using System.Collections.Generic;
using vChat.Model;
using vChat.Model.Entities;
using vChat.Lib.Serialize;
using System.Linq;

namespace vChat.Data
{
    public class UserProccess : IUserService
    {
        private Users UserTask;
        private Conversation ConversationTask;
        private FriendGroup GroupTask;
        private MethodInvokeResult SUCCESS, FAIL, INPUT_ERROR, UNHANDLE_ERROR;

        public UserProccess()
        {
            UserTask = new Users();
            ConversationTask = new Conversation();
            GroupTask = new FriendGroup();
        }

        public Users Info(int UserID)
        {
            return UserTask.Get(UserID);
        }

        public Users FindName(String Username)
        {
            return UserTask.GetByName(Username);
        }

        public GroupFriendList FriendList(int UserID)
        {
            return UserTask.FriendList(UserID);
        }

        public List<Users> FriendRequests(int UserID)
        {
            return UserTask.FriendRequests(UserID);
        }

        public MethodInvokeResult AddFriend(int UserID, String FriendName, int GroupID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Add friend Success" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Add friend Fail" };
            INPUT_ERROR = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = "User not yet exist" };

            if (!UserTask.IsExist(FriendName))
                return INPUT_ERROR;

            Users user_info = UserTask.Get(UserID);
            Users friend_info = UserTask.GetByName(FriendName);
            FriendGroup group_info = GroupTask.Get(GroupID) as FriendGroup;

            return UserTask.AddFriend(user_info, friend_info, group_info) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult AddGroup(int UserID, String Name, out int NewGroupID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Add group Success" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Add group Fail" };

            FriendGroup new_group = new FriendGroup
            {
                Name = Name,
                Owner = new Users().Get(UserID)
            };

            bool r = new_group.New();
            NewGroupID = new_group.GroupID;

            return r ? SUCCESS : FAIL;
        }

        public MethodInvokeResult Login(string Username, string Password)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Login Success" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Login Fail" };

            return UserTask.IsAvailable(Username, Password) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult LoginHash(string Username, string Password)
        {
            return Login(Username, Password);
        }

        public MethodInvokeResult Signup(string Username, string Password, string FirstName, string LastName, int QuestionID, string Answer, DateTime Birthdate)
        {            
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Signup Success" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Sign Fail" };
            INPUT_ERROR = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = "User already exist" };

            if (UserTask.IsExist(Username))
                return INPUT_ERROR;

            Users new_user = new Users
            {
                Username = Username,
                Password = Password,
                FirstName = FirstName,
                LastName = LastName,
                Question = new Question().Get(QuestionID) as Question,
                Answer = Answer,
                Birthdate = Birthdate
            };

            return new_user.New() ? SUCCESS : FAIL;
        }

        public MethodInvokeResult UserExist(string Username)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "User already exist" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "User not yet exist" };

            return UserTask.IsExist(Username) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult ChangePassword(int UserID, string OldPassword, string NewPassword)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Update success" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Update fail" };
            INPUT_ERROR = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = "Old Password and New Password not match" };

            Users user_info = UserTask.Get(UserID) as Users;

            String oldPwdFromDB = user_info.Password;

            bool r = false;
            bool matchPwd = OldPassword.Equals(oldPwdFromDB);

            if (!matchPwd)
                return INPUT_ERROR;
            
            user_info.Password = NewPassword;
            r = user_info.Update();

            return r ? SUCCESS : FAIL;
        }

        public MethodInvokeResult AcceptFriendRequest(int UserID, int FriendID, int GroupID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Process friend requests success" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Process friend requests fail" };

            Users user = UserTask.Get(UserID);
            Users friend = UserTask.Get(FriendID);
            FriendGroup group = GroupTask.Get(GroupID) as FriendGroup;

            return UserTask.ReponseFriendRequest(user, friend, group, true, false) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult IgnoreFriendRequest(int UserID, int FriendID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Process friend requests success" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Process friend requests fail" };

            Users user = UserTask.Get(UserID);
            Users friend = UserTask.Get(FriendID);            

            return UserTask.ReponseFriendRequest(user, friend, null, false, true) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult MoveContact(int UserID, int FriendID, int NewGroupID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Move contact requests success" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Move contact requests fail" };

            Users user = UserTask.Get(UserID);
            Users friend = UserTask.Get(FriendID);
            FriendGroup group = GroupTask.Get(NewGroupID) as FriendGroup;

            return UserTask.MoveContact(user, friend, group) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult RemoveContact(int UserID, int FriendID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Remove contact requests success" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Remove contact requests fail" };

            Users user = UserTask.Get(UserID);
            Users friend = UserTask.Get(FriendID);

            return UserTask.RemoveContact(user, friend) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult Deactive(int UserID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Deactive account success" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Deactive account fail" };

            return UserTask.DeactiveAccount(UserID, true) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult Reactive(int UserID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Reactive account success" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Reactive account fail" };

            return UserTask.DeactiveAccount(UserID, false) ? SUCCESS : FAIL;
        }

        public List<Conversation> GetConversations(int UserID)
        {
            return ConversationTask.GetConversations(UserID);
        }

        public List<Conversation> GetNewestConversations(int UserID)
        {
            return ConversationTask.GetNewestConversations(UserID);
        }
    }
}
