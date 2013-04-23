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
            return UserTask.Get<Users>(UserID);
        }

        public FriendGroup GroupInfo(int GroupID)
        {
            return GroupTask.Get<FriendGroup>(GroupID);
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
            return UserTask.Requests(UserID);
        }

        public MethodInvokeResult AddFriend(int UserID, String FriendName, int GroupID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Thêm bạn thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình thêm bạn bè. Vui lòng thử lại sau" };
            INPUT_ERROR = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = "Người dùng này không tồn tại. Vui lòng kiểm tra lại thông tin" };

            if (!UserTask.IsExist(FriendName))
                return INPUT_ERROR;

            Users userInfo = UserTask.Get<Users>(UserID);
            Users friendInfo = UserTask.GetByName(FriendName);
            FriendGroup groupInfo = GroupTask.Get<FriendGroup>(GroupID);

            return UserTask.AddFriend(userInfo, friendInfo, groupInfo) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult AddGroup(int UserID, String Name, out int NewGroupID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Thêm nhóm thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình thêm nhóm. Vui lòng thử lại sau" };

            FriendGroup newGroup = new FriendGroup
            {
                Name = Name,
                Owner = new Users().Get<Users>(UserID)
            };

            bool r = newGroup.New();
            NewGroupID = newGroup.GroupID;

            return r ? SUCCESS : FAIL;
        }

        public MethodInvokeResult RemoveGroup(int GroupID, bool RemoveContact)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Xóa nhóm thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình xóa nhóm. Vui lòng thử lại sau" };

            FriendGroup group = GroupTask.Get<FriendGroup>(GroupID);

            return UserTask.RemoveGroup(group, RemoveContact) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult Login(string Username, string Password)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Đăng nhập thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình đăng nhập. Vui lòng thử lại sau" };

            return UserTask.IsAvailable(Username, Password) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult LoginHash(string Username, string Password)
        {
            return Login(Username, Password);
        }

        public MethodInvokeResult Signup(string Username, string Password, string FirstName, string LastName, int QuestionID, string Answer, DateTime Birthdate)
        {            
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Đăng ký thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình đăng ký. Vui lòng thử lại sau" };
            INPUT_ERROR = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = "Người dùng đã tồn tại trong hệ thống" };

            if (UserTask.IsExist(Username))
                return INPUT_ERROR;

            Users newUser = new Users
            {
                Username = Username,
                Password = Password,
                FirstName = FirstName,
                LastName = LastName,
                Question = new Question().Get<Question>(QuestionID),
                Answer = Answer,
                Birthdate = Birthdate
            };

            return newUser.New() ? SUCCESS : FAIL;
        }

        public MethodInvokeResult UserExist(string Username)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Người dùng đã tồn tại trong hệ thống" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Người dùng chưa tồn tại trong hệ thống" };

            return UserTask.IsExist(Username) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult ChangePassword(int UserID, string OldPassword, string NewPassword)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Đổi mật khẩu thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình đổi mật khẩu. Vui lòng thử lại sau" };
            INPUT_ERROR = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = "Sai mật khẩu hiện tại. Vui lòng kiểm tra lại thông tin" };

            Users userInfo = UserTask.Get<Users>(UserID);

            String oldPwdFromDB = userInfo.Password;

            bool matchPwd = OldPassword.Equals(oldPwdFromDB);

            if (!matchPwd)
                return INPUT_ERROR;

            userInfo.Password = NewPassword;            

            return userInfo.Update() ? SUCCESS : FAIL;
        }

        public MethodInvokeResult AcceptFriendRequest(int UserID, int FriendID, int GroupID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Xử lý yêu cầu kết bạn thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình xử lý yêu cầu kết bạn. Vui lòng thử lại sau" };

            Users user = UserTask.Get<Users>(UserID);
            Users friend = UserTask.Get<Users>(FriendID);
            FriendGroup group = GroupTask.Get<FriendGroup>(GroupID);

            return UserTask.RequestProcess(user, friend, group, true, false) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult IgnoreFriendRequest(int UserID, int FriendID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Xử lý yêu cầu kết bạn thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình xử lý yêu cầu kết bạn. Vui lòng thử lại sau" };

            Users user = UserTask.Get<Users>(UserID);
            Users friend = UserTask.Get<Users>(FriendID);

            return UserTask.RequestProcess(user, friend, null, false, true) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult MoveContact(int UserID, int FriendID, int NewGroupID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Di chuyển danh sách bạn bè thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình di chuyển danh sách bạn bè. Vui lòng thử lại sau" };

            Users user = UserTask.Get<Users>(UserID);
            Users friend = UserTask.Get<Users>(FriendID);
            FriendGroup group = GroupTask.Get<FriendGroup>(NewGroupID);

            return UserTask.MoveContact(user, friend, group) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult RemoveContact(int UserID, int FriendID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Xóa bạn bè thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình xóa bạn bè. Vui lòng thử lại sau" };

            Users user = UserTask.Get<Users>(UserID);
            Users friend = UserTask.Get<Users>(FriendID);

            return UserTask.RemoveContact(user, friend) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult Deactive(int UserID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Vô hiệu hóa tài khoản thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình vô hiệu hóa tài khoản. Vui lòng thử lại sau" };

            return UserTask.Deactive(UserID, true) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult Reactive(int UserID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Kích hoạt lại tài khoản thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình kích hoạt lại tài khoản. Vui lòng thử lại sau" };

            return UserTask.Deactive(UserID, false) ? SUCCESS : FAIL;
        }

        public List<Conversation> GetConversations(int UserID)
        {
            return ConversationTask.GetByUser(UserID);
        }

        public List<Conversation> GetNewestConversations(int UserID)
        {
            return ConversationTask.GetNewestByUser(UserID);
        }
    }
}