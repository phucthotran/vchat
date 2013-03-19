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

        public XmlTextObject Info(int UserID)
        {
            if (UserID == 0)
                return null;

            return unc.Info(UserID);
        }

        public XmlTextObject FriendList(int UserID)
        {
            if (UserID == 0)
                return null;

            return unc.FriendList(UserID);
        }

        public MethodInvokeResult Login(string Username, string Password)
        {
            try
            {                          
                Username.RequiredArgument("Username")
                    .NotNull() //throw ArgumentNullException
                    .Between(8, 45); //throw ArgumentException

                Password.RequiredArgument("Password").NotNull().Between(8, 45);
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (Exception ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.UNHANDLE_ERROR, Message = String.Format("Unhandle error occurs: {0}", ex.Message), Exception = new ExceptionInfo(ex) };
            }

            return unc.Login(Username, MD5Encrypt.Hash(Password));
        }

        public MethodInvokeResult Signup(string Username, string Password, string FirstName, string LastName, int QuestionID, string Answer, DateTime Birthdate)
        {
            try
            {            
                Username.RequiredArgument("Username")
                    .NotNull() //throw ArgumentNullException
                    .Between(8, 45); //throw ArgumentException

                Password.RequiredArgument("Password").NotNull().Between(8, 45);
                LastName.RequiredArgument("LastName").NotNull().Between(2, 45);
                FirstName.RequiredArgument("FirstName").NotNull().Between(2, 45);
                Answer.RequiredArgument("Answer").NotNull().Between(2, 50);
                Birthdate.RequiredArgumentWithStruct("Birthdate").NotNull();
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentException ex)
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
                Username.RequiredArgument("Username")
                    .NotNull() //throw ArgumentNullException
                    .Between(8, 45); //throw ArgumentException
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentException ex)
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
                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1); //throw ArgumentException

                OldPassword.RequiredArgument("OldPassword")
                    .NotNull() //throw ArgumentNullException
                    .Between(8, 45); //throw ArgumentException

                NewPassword.RequiredArgument("NewPassword").NotNull().Between(8, 45);
            }
            catch (ArgumentNullException ex)
            {
                return new MethodInvokeResult { Status = MethodInvokeResult.RESULT.INPUT_ERROR, Message = ex.Message, Exception = new ExceptionInfo(ex) };
            }
            catch (ArgumentException ex)
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

        public XmlTextObject GetConversations(int UserID)
        {
            try
            {
                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1); //throw ArgumentException
            }
            catch (ArgumentException ex)
            {
                return null;
            }

            return unc.GetConversations(UserID);
        }

        public XmlTextObject GetNewestConversations(int UserID)
        {
            try
            {
                UserID.RequiredArgumentWithStruct("UserID").BeginFrom(1); //throw ArgumentException
            }
            catch (ArgumentException ex)
            {
                return null;
            }

            return unc.GetNewestConversations(UserID);
        }
    }
}
