using System;
using System.Collections.Generic;
using vChat.Model;
using vChat.Model.Entities;
using vChat.Lib.Serialize;

namespace vChat.Data
{
    public class UserProccess : IUserService
    {
        private Users UserTask;
        private Conversation ConversationTask;
        private MethodInvokeResult SUCCESS, FAIL, INPUT_ERROR, UNHANDLE_ERROR;

        public UserProccess()
        {
            UserTask = new Users();
            ConversationTask = new Conversation();
        }

        public XmlTextObject Info(int UserID)
        {
            return ObjectSerialize<Users>.ParseToXml(UserTask.Get(UserID));
        }

        public XmlTextObject FriendList(int UserID)
        {
            return ObjectSerialize<List<Users>>.ParseToXml(UserTask.FriendList(UserID));
        }

        public MethodInvokeResult Login(string Username, string Password)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Login Success" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Login Fail" };

            return UserTask.IsAvailable(Username, Password) ? SUCCESS : FAIL;
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

            return new_user.Update() ? SUCCESS : FAIL;
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
