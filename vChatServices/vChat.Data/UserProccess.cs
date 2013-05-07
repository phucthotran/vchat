using System;
using System.Collections.Generic;
using vChat.Model;
using vChat.Model.Entities;
using vChat.Lib.Serialize;
using System.Linq;
using System.Collections.ObjectModel;

namespace vChat.Data
{
    public class UserProccess : IUserService
    {
        private Users UserTask;
        private Conversation ConversationTask;
        private FriendGroup GroupTask;
        private Question QuestionTask;
        private MethodInvokeResult SUCCESS, FAIL, INPUT_ERROR, UNHANDLE_ERROR;

        public UserProccess()
        {
            UserTask = new Users();
            ConversationTask = new Conversation();
            GroupTask = new FriendGroup();
            QuestionTask = new Question();
        }

        public Users Info(int UserID)
        {
            return UserTask.Get(UserID);
        }

        public FriendGroup GroupInfo(int GroupID)
        {
            return GroupTask.Get(GroupID) as FriendGroup;
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

        public List<Users> UnresponseFriendRequests(int UserID)
        {
            return UserTask.UnresponseRequests(UserID);
        }

        public MethodInvokeResult AddFriend(int UserID, String FriendName, int GroupID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Thêm bạn thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình thêm bạn bè. Vui lòng thử lại sau" };
            INPUT_ERROR = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = "Người dùng này không tồn tại. Vui lòng kiểm tra lại thông tin" };

            if (!UserTask.IsExist(FriendName))
                return INPUT_ERROR;

            Users userInfo = UserTask.Get(UserID) as Users;
            Users friendInfo = UserTask.GetByName(FriendName);
            FriendGroup groupInfo = GroupTask.Get(GroupID) as FriendGroup;

            return UserTask.AddFriend(userInfo, friendInfo, groupInfo) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult AddGroup(int UserID, String Name, ref int NewGroupID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Thêm nhóm thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình thêm nhóm. Vui lòng thử lại sau" };

            FriendGroup newGroup = new FriendGroup
            {
                Name = Name,
                Owner = new Users().Get(UserID) as Users
            };

            bool r = newGroup.New();
            NewGroupID = newGroup.GroupID;

            return r ? SUCCESS : FAIL;
        }

        public MethodInvokeResult RemoveGroup(int GroupID, bool RemoveContact)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Xóa nhóm thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình xóa nhóm. Vui lòng thử lại sau" };

            FriendGroup group = GroupTask.Get(GroupID) as FriendGroup;

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
                Question = new Question().Get(QuestionID) as Question,
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

        public MethodInvokeResult ChangeAvatar(int UserID, byte[] ImageBytes)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Cập nhập ảnh đại diện thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình cập nhập ảnh đại diện. Vui lòng thử lại sau" };

            Users userInfo = UserTask.Get(UserID) as Users;
            userInfo.Picture = ImageBytes;

            return userInfo.Update() ? SUCCESS : FAIL;
        }

        public MethodInvokeResult AnswerIsMatch(int UserID, int QuestionID, String Answer)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Câu hỏi và câu trả lời bí mật hợp lệ" };
            //FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình kiểm tra câu trả lời bí mật. Vui lòng thử lại sau" };
            INPUT_ERROR = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = "Sai câu hỏi hoặc câu trả lời bí mật. Vui lòng kiểm tra lại thông tin" };

            const int ANSWER_NOT_MATCH = 0, QUESTION_NOT_MATCH = -1;

            int result = UserTask.AnswerIsMatch(UserID, QuestionID, Answer);

            if (result == ANSWER_NOT_MATCH || result == QUESTION_NOT_MATCH)
                return INPUT_ERROR;

            //else (result == ANSWER_MATCH)            
            return SUCCESS;
        }
        
        public MethodInvokeResult ChangePassword(int UserID, string OldPassword, string NewPassword)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Đổi mật khẩu thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình đổi mật khẩu. Vui lòng thử lại sau" };
            INPUT_ERROR = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = "Sai mật khẩu hiện tại. Vui lòng kiểm tra lại thông tin" };

            Users userInfo = UserTask.Get(UserID) as Users;

            String oldPwdFromDB = userInfo.Password;

            bool matchPwd = OldPassword.Equals(oldPwdFromDB);

            if (!matchPwd)
                return INPUT_ERROR;

            userInfo.Password = NewPassword;            

            return userInfo.Update() ? SUCCESS : FAIL;
        }

        public MethodInvokeResult ChangeUserInfo(int UserID, String FirstName, String LastName, int QuestionID, String Answer, DateTime Birthdate)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Thay đổi thông tin cá nhân thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình thay đổi thông tin cá nhân. Vui lòng thử lại sau" };

            Users userInfo = UserTask.Get(UserID);
            userInfo.FirstName = FirstName;
            userInfo.LastName = LastName;
            userInfo.Question = QuestionTask.Get(QuestionID);
            userInfo.Answer = Answer;
            userInfo.Birthdate = Birthdate;

            return userInfo.Update() ? SUCCESS : FAIL;
        }

        public MethodInvokeResult AcceptFriendRequest(int UserID, int FriendID, int GroupID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Xử lý yêu cầu kết bạn thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình xử lý yêu cầu kết bạn. Vui lòng thử lại sau" };

            Users user = UserTask.Get(UserID) as Users;
            Users friend = UserTask.Get(FriendID) as Users;
            FriendGroup group = GroupTask.Get(GroupID) as FriendGroup;

            return UserTask.RequestProcess(user, friend, group, true, false) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult IgnoreFriendRequest(int UserID, int FriendID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Xử lý yêu cầu kết bạn thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình xử lý yêu cầu kết bạn. Vui lòng thử lại sau" };

            Users user = UserTask.Get(UserID) as Users;
            Users friend = UserTask.Get(FriendID) as Users;

            return UserTask.RequestProcess(user, friend, null, false, true) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult MoveContact(int UserID, int FriendID, int NewGroupID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Di chuyển danh sách bạn bè thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình di chuyển danh sách bạn bè. Vui lòng thử lại sau" };

            Users user = UserTask.Get(UserID) as Users;
            Users friend = UserTask.Get(FriendID) as Users;
            FriendGroup group = GroupTask.Get(NewGroupID) as FriendGroup;

            return UserTask.MoveContact(user, friend, group) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult RemoveContact(int UserID, int FriendID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Xóa bạn bè thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình xóa bạn bè. Vui lòng thử lại sau" };

            Users user = UserTask.Get(UserID) as Users;
            Users friend = UserTask.Get(FriendID) as Users;

            return UserTask.RemoveContact(user, friend) ? SUCCESS : FAIL;
        }

        public MethodInvokeResult AlreadyMakeFriend(int UserID, int FriendID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Kiểm tra tình trạng bạn bè thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình kiểm tra tình trạng bạn bè. Vui lòng thử lại sau" };

            Users user = UserTask.Get(UserID) as Users;
            Users friend = UserTask.Get(FriendID) as Users;

            return UserTask.AlreadyMakeFriend(user, friend) ? SUCCESS : FAIL;
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

        public List<Question> GetAllQuestion()
        {
            return QuestionTask.GetAll();
        }

        public MethodInvokeResult SaveConversation(int UserID, int FriendID, String Content, DateTime Time, ref int ConversationID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Lưu đoạn hội thoại thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình lưu đoạn hội thoại. Vui lòng thử lại sau" };

            Users sendBy = UserTask.Get(UserID);
            Users sendTo = UserTask.Get(FriendID);

            Conversation newConversation = new Conversation();
            newConversation.Message = Content;
            newConversation.SentBy = sendBy;
            newConversation.SendTo = sendTo;
            newConversation.Time = Time;

            bool r = newConversation.New();

            ConversationID = newConversation.ConversationID;

            return r ? SUCCESS : FAIL;
        }

        public MethodInvokeResult MarkAsReadConversation(int ConversationID)
        {
            SUCCESS = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.SUCCESS, Message = "Đánh dấu đoạn hội thoại thành công" };
            FAIL = new MethodInvokeResult { Status = MethodInvokeResult.RESULT.FAIL, Message = "Có lỗi trong quá trình đánh dấu đoạn hội thoại thành công" };

            Conversation conversation = ConversationTask.Get(ConversationID);
            conversation.IsRead = true;

            return conversation.Update() ? SUCCESS : FAIL;
        }

        public List<Conversation> GetConversations(int UserID)
        {
            return ConversationTask.GetByUser(UserID);
        }

        public List<Conversation> GetConversations(int UserID, int BeginIndex, int EndIndex)
        {
            return ConversationTask.GetByUser(UserID, BeginIndex, EndIndex);
        }

        public List<Conversation> GetConversationBetween(int UserID, int FriendID)
        {
            return ConversationTask.GetConversationBetween(UserID, FriendID);
        }

        public List<Conversation> GetConversationBetween(int UserID, int FriendID, int BeginIndex, int EndIndex)
        {
            return ConversationTask.GetConversationBetween(UserID, FriendID, BeginIndex, EndIndex);
        }

        public List<Conversation> GetNewestConversations(int UserID)
        {
            return ConversationTask.GetNewestByUser(UserID);
        }

        public List<Conversation> GetNewestConversations(int UserID, int BeginIndex, int EndIndex)
        {
            return ConversationTask.GetNewestByUser(UserID, BeginIndex, EndIndex);
        }
    }
}