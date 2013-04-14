using System;
using System.Collections.Generic;
using vChat.Model;
using vChat.Model.Entities;
using vChat.Data;
using vChat.Lib;
using vChat.Lib.Serialize;
using vChat.Business.Validations;

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
                    .NotNull() //throw ArgumentNullExption
                    .Between(6, 45);

                ValidationController.Validate();
            }
            catch (ValidateException)
            {
                return null;
            }

            return unc.FindName(Username);
        }

        public Users Info(int UserID)
        {
            if (UserID == 0)
                return null;

            return unc.Info(UserID);
        }

        public GroupFriendList FriendList(int UserID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                ValidationController.Validate();
            }
            catch (ValidateException)
            {
                return null;
            }

            return unc.FriendList(UserID);
        }

        public MethodInvokeResult AddFriend(int UserID, String FriendName, int GroupID)
        {
            try
            {
                ValidationController.Prepare();

                FriendName.RequiredArgument("FriendName")
                    .NotNull() //throw ArguemntNullException
                    .Between(6, 45);
                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);
                GroupID.RequiredArgumentWithStruct("GroupID").BeginFrom(1);

                ValidationController.Validate();
            }
            catch (ValidateException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Errors = ex.Errors, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }

            return unc.AddFriend(UserID, FriendName, GroupID);
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

                ValidationController.Validate();
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

            return unc.Login(Username, MD5Encrypt.Hash(Password));
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

                ValidationController.Validate();
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

            return unc.LoginHash(Username, Password);
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

                ValidationController.Validate();
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

            return unc.Signup(Username, MD5Encrypt.Hash(Password), FirstName, LastName, QuestionID, Answer, Birthdate);
        }

        public MethodInvokeResult UserExist(string Username)
        {
            try
            {
                ValidationController.Prepare();

                Username.RequiredArgument("Username")
                    .NotNull() //throw ArgumentNullException
                    .Between(6, 45);

                ValidationController.Validate();
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

            return unc.UserExist(Username);
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

                ValidationController.Validate();
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

            return unc.ChangePassword(UserID, MD5Encrypt.Hash(OldPassword), MD5Encrypt.Hash(NewPassword));
        }

        public MethodInvokeResult Deactive(int UserID)
        {
            return unc.Deactive(UserID);
        }

        public MethodInvokeResult Reactive(int UserID)
        {
            return unc.Reactive(UserID);
        }

        public List<Conversation> GetConversations(int UserID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                ValidationController.Validate();
            }
            catch (ValidateException)
            {
                return null;
            }

            return unc.GetConversations(UserID);
        }

        public List<Conversation> GetNewestConversations(int UserID)
        {
            try
            {
                ValidationController.Prepare();

                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1);

                ValidationController.Validate();
            }
            catch (ValidateException)
            {
                return null;
            }

            return unc.GetNewestConversations(UserID);
        }
    }
}
