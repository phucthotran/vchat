using System;
using System.Collections.Generic;
using vChat.Model;
using vChat.Model.Entities;
using vChat.Data;
using vChat.Lib;
using vChat.Lib.Serialize;
using vChat.Business.Validations;
using System.Collections.ObjectModel;

namespace vChat.Business
{
    public class UserProccess : IUserService
    {
        private Data.UserProccess unc;

        public UserProccess()
        {
            unc = new Data.UserProccess();            
        }

        public Users FindName(String Username)
        {
            try
            {
                ValidationController.Prepare();

                Username.RequiredArgument("Username")
                    .NotNull() //throw ArgumentNullException
                    .Between(6, 45);

                ValidationController.Validate(); //throw ValidateException

                return unc.FindName(Username);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (ValidateException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public Users Info(int UserID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.Info(UserID);
            }
            catch (ValidateException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public FriendGroup GroupInfo(int GroupID)
        {
            try
            {
                ValidationController.Prepare();

                GroupID.RequiredArgumentWithStruct("GroupID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.GroupInfo(GroupID);
            }
            catch (ValidateException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public GroupFriendList FriendList(int UserID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.FriendList(UserID);
            }
            catch (ValidateException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public List<Users> FriendRequests(int UserID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.FriendRequests(UserID);
            }
            catch (ValidateException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public List<Users> UnresponseFriendRequests(int UserID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.UnresponseFriendRequests(UserID);
            }
            catch (ValidateException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MethodInvokeResult AddFriend(int UserID, String FriendName, int GroupID)
        {
            try
            {
                ValidationController.Prepare();

                FriendName.RequiredArgument("FriendName")
                    .NotNull() //throw ArgumentNullException
                    .Between(6, 45);
                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                GroupID.RequiredArgumentWithStruct("GroupID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.AddFriend(UserID, FriendName, GroupID);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }                        
        }

        public MethodInvokeResult AddGroup(int UserID, String Name, ref int NewGroupID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                Name.RequiredArgument("Name")
                    .NotNull() //throw ArgumentNullException
                    .Between(1, 45);

                ValidationController.Validate(); //throw ValidateException

                return unc.AddGroup(UserID, Name, ref NewGroupID);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }         
        }

        public MethodInvokeResult RemoveGroup(int GroupID, bool RemoveContact)
        {
            try
            {
                ValidationController.Prepare();

                GroupID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.RemoveGroup(GroupID, RemoveContact);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }            
        }

        public MethodInvokeResult Login(string Username, string Password)
        {
            try
            {
                ValidationController.Prepare();

                Username.RequiredArgument("Username")
                    .NotNull() //throw ArguemntNullException
                    .Between(6, 45);

                Password.RequiredArgument("Password").NotNull().Between(8, 45);

                ValidationController.Validate(); //throw ValidateException

                return unc.Login(Username, MD5Encrypt.Hash(Password));
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }            
        }

        public MethodInvokeResult LoginHash(string Username, string Password)
        {
            try
            {
                ValidationController.Prepare();

                Username.RequiredArgument("Username")
                    .NotNull() //throw ArguemntNullException
                    .Between(6, 45);

                Password.RequiredArgument("Password").NotNull().Between(8, 45);

                ValidationController.Validate(); //throw ValidateException

                return unc.LoginHash(Username, Password);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }            
        }

        public MethodInvokeResult Signup(string Username, string Password, string FirstName, string LastName, int QuestionID, string Answer, DateTime Birthdate)
        {
            try
            {
                ValidationController.Prepare();

                Username.RequiredArgument("Username")
                    .NotNull() //throw ArgumentNullException
                    .Between(6, 45);

                Password.RequiredArgument("Password").NotNull().Between(8, 45);
                LastName.RequiredArgument("LastName").NotNull().Between(2, 45);
                FirstName.RequiredArgument("FirstName").NotNull().Between(2, 45);
                Answer.RequiredArgument("Answer").NotNull().Between(2, 50);
                Birthdate.RequiredArgumentWithStruct("Birthdate").NotNull();

                ValidationController.Validate(); //throw ValidateException

                return unc.Signup(Username, MD5Encrypt.Hash(Password), FirstName, LastName, QuestionID, Answer, Birthdate);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }            
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }            
        }

        public MethodInvokeResult UserExist(string Username)
        {
            try
            {
                ValidationController.Prepare();

                Username.RequiredArgument("Username")
                    .NotNull() //throw ArgumentNullException
                    .Between(6, 45);

                ValidationController.Validate(); //throw ValidateException

                return unc.UserExist(Username);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }            
        }

        public MethodInvokeResult ChangeAvatar(int UserID, byte[] ImageBytes)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                ImageBytes.RequiredArgument("ImageBytes")
                    .NotNull(); //throw ArgumentNullException                    

                ValidationController.Validate(); //throw ValidateException

                return unc.ChangeAvatar(UserID, ImageBytes);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }            
        }

        public MethodInvokeResult AnswerIsMatch(int UserID, int QuestionID, String Answer)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                QuestionID.RequiredArgumentWithStruct("QuestionID").BeginFrom(1);
                Answer.RequiredArgument("ImageBytes")
                    .NotNull() //throw ArgumentNullException
                    .Between(2, 50);

                ValidationController.Validate(); //throw ValidateException

                return unc.AnswerIsMatch(UserID, QuestionID, Answer);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            } 
        }

        public MethodInvokeResult ChangePassword(int UserID, string OldPassword, string NewPassword)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                OldPassword.RequiredArgument("OldPassword")
                    .NotNull() //throw ArgumentNullException
                    .Between(8, 45);

                NewPassword.RequiredArgument("NewPassword").NotNull().Between(8, 45);

                ValidationController.Validate(); //throw ValidateException

                return unc.ChangePassword(UserID, OldPassword, MD5Encrypt.Hash(NewPassword));
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }            
        }

        public MethodInvokeResult AccountRecovery(int UserID, String NewPassword)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                NewPassword.RequiredArgument("NewPassword")
                    .NotNull() //throw ArgumentNullException
                    .Between(8, 45);

                ValidationController.Validate(); //throw ValidateException

                return unc.AccountRecovery(UserID, MD5Encrypt.Hash(NewPassword));
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }
        }

        public MethodInvokeResult ChangeUserInfo(int UserID, String FirstName, String LastName, int QuestionID, String Answer, DateTime Birthdate)
        {
            try
            {
                ValidationController.Prepare();

                FirstName.RequiredArgument("FirstName").NotNull().Between(2, 45);
                LastName.RequiredArgument("LastName").NotNull().Between(2, 45);                
                Answer.RequiredArgument("Answer").NotNull().Between(2, 50);
                QuestionID.RequiredArgumentWithStruct("QuestionID").BeginFrom(1);
                Birthdate.RequiredArgumentWithStruct("Birthdate").NotNull();

                ValidationController.Validate(); //throw ValidateException

                return unc.ChangeUserInfo(UserID, FirstName, LastName, QuestionID, Answer, Birthdate);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }
        }

        public MethodInvokeResult AcceptFriendRequest(int UserID, int FriendID, int GroupID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                FriendID.RequiredArgumentWithStruct("FriendID").BeginFrom(1);
                GroupID.RequiredArgumentWithStruct("GroupID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.AcceptFriendRequest(UserID, FriendID, GroupID);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }            
        }

        public MethodInvokeResult IgnoreFriendRequest(int UserID, int FriendID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                FriendID.RequiredArgumentWithStruct("FriendID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.IgnoreFriendRequest(UserID, FriendID);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }            
        }

        public MethodInvokeResult MoveContact(int UserID, int FriendID, int NewGroupID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                FriendID.RequiredArgumentWithStruct("FriendID").BeginFrom(1);
                NewGroupID.RequiredArgumentWithStruct("NewGroupID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.MoveContact(UserID, FriendID, NewGroupID);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }            
        }

        public MethodInvokeResult RemoveContact(int UserID, int FriendID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                FriendID.RequiredArgumentWithStruct("FriendID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.RemoveContact(UserID, FriendID);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }            
        }

        public MethodInvokeResult AlreadyMakeFriend(int UserID, int FriendID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                FriendID.RequiredArgumentWithStruct("FriendID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.AlreadyMakeFriend(UserID, FriendID);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }
        }

        public MethodInvokeResult Deactive(int UserID)
        {
            return unc.Deactive(UserID);
        }

        public MethodInvokeResult Reactive(int UserID)
        {
            return unc.Reactive(UserID);
        }

        public List<Question> GetAllQuestion()
        {
            try
            {
                return unc.GetAllQuestion();
            }
            catch(Exception)
            {
                return null;
            }
        }

        public MethodInvokeResult SaveConversation(int UserID, int FriendID, String Content, DateTime Time, ref int ConversationID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                FriendID.RequiredArgumentWithStruct("FriendID").BeginFrom(1);
                Content.RequiredArgument("Content").NotNull();  //throw ArgumentNullException
                Time.RequiredArgumentWithStruct("Time").NotNull();

                ValidationController.Validate(); //throw ValidateException

                return unc.SaveConversation(UserID, FriendID, Content, Time, ref ConversationID);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }
        }

        public MethodInvokeResult MarkAsReadConversation(int ConversationID)
        {
            try
            {
                ValidationController.Prepare();

                ConversationID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.MarkAsReadConversation(ConversationID);
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }
        }

        public List<Conversation> GetConversations(int UserID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.GetConversations(UserID);
            }
            catch (ValidateException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public List<Conversation> GetConversations(int UserID, int BeginIndex, int EndIndex)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                BeginIndex.RequiredArgumentWithStruct("BeginIndex").BeginFrom(1);
                BeginIndex.RequiredArgumentWithStruct("EndIndex").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.GetConversations(UserID, BeginIndex, EndIndex);
            }
            catch (ValidateException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public List<Conversation> GetConversationBetween(int UserID, int FriendID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                FriendID.RequiredArgumentWithStruct("FriendID").BeginFrom(1); 

                ValidationController.Validate(); //throw ValidateException

                return unc.GetConversationBetween(UserID, FriendID);
            }
            catch (ValidateException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }             
        }

        public List<Conversation> GetConversationBetween(int UserID, int FriendID, int BeginIndex, int EndIndex)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                FriendID.RequiredArgumentWithStruct("FriendID").BeginFrom(1);
                BeginIndex.RequiredArgumentWithStruct("BeginIndex").BeginFrom(1);
                BeginIndex.RequiredArgumentWithStruct("EndIndex").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.GetConversationBetween(UserID, FriendID, BeginIndex, EndIndex);
            }
            catch (ValidateException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Conversation> GetNewestConversations(int UserID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.GetNewestConversations(UserID);
            }
            catch (ValidateException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public List<Conversation> GetNewestConversations(int UserID, int BeginIndex, int EndIndex)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                BeginIndex.RequiredArgumentWithStruct("BeginIndex").BeginFrom(1);
                BeginIndex.RequiredArgumentWithStruct("EndIndex").BeginFrom(1);

                ValidationController.Validate(); //throw ValidateException

                return unc.GetNewestConversations(UserID, BeginIndex, EndIndex);
            }
            catch (ValidateException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }            
        }
    }
}
