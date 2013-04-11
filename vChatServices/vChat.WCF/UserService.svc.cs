using System;
using System.Collections.Generic;
using System.ServiceModel;
using vChat.Lib.Serialize;
using vChat.Model;
using vChat.Model.Entities;

namespace vChat.WCF
{

    [ServiceBehavior]
    public class UserService : IUserService
    {
        private Business.UserProccess unc;

        public UserService()
        {
            unc = new Business.UserProccess();
        }

        /// <summary>
        /// Get user info by ID
        /// </summary>
        /// <param name="UserID">User ID</param>
        /// <returns>Return user info (Users object)</returns>
        /// <example>
        ///     Users u = Info(1);
        /// </example>
        [OperationBehavior]
        public Users Info(int UserID)
        {
            return unc.Info(UserID);
        }

        /// <summary>
        /// Get friend list of user
        /// </summary>
        /// <param name="UserID">User ID</param>
        /// <returns>Return user list (List<Users>)</returns>
        /// <example>
        ///     List<User> lstUser = FriendList(1);
        /// </example>
        [OperationBehavior]
        public GroupFriendList FriendList(int UserID)
        {
            return unc.FriendList(UserID);
        }

        /// <summary>
        /// Add new friend
        /// </summary>
        /// <param name="UserID">Current user id</param>
        /// <param name="FriendName">Username to be added as friend</param>
        /// <param name="GroupID">Assign new friend to a group</param>
        /// <example>
        ///     AddFriend(1, "phuctho", 2);
        /// </example>
        /// <returns></returns>
        [OperationBehavior]
        public MethodInvokeResult AddFriend(int UserID, String FriendName, int GroupID)
        {
            return unc.AddFriend(UserID, FriendName, GroupID);
        }

        /// <summary>
        /// Login to user account
        /// </summary>
        /// <param name="Username">Username (Min: 6, Max: 45)</param>
        /// <param name="Password">Password (Min: 6, Max: 45)</param>
        /// <returns>Status.SUCCESS or Status.FAIL</returns>
        /// <example>
        ///     Status st = Login("admin", "1234");
        /// </example>
        [OperationBehavior]
        public MethodInvokeResult Login(string Username, string Password)
        {
            return unc.Login(Username, Password);
        }

        /// <summary>
        /// Create new account
        /// </summary>
        /// <param name="Username">Username (Min: 6, Max: 45)</param>
        /// <param name="Password">Password (Min: 8, Max: 45)</param>
        /// <param name="FirstName">First Name (Min: 2, Max: 45)</param>
        /// <param name="LastName">Last Name (Min: 2, Max: 45)</param>
        /// <param name="QuestionID">Question ID</param>
        /// <param name="Answer">Answer (Min: 3, Max 50)</param>
        /// <param name="Birthdate"></param>
        /// <returns>Status.ERROR or Status.SUCCESS or Status.FAIL</returns>
        /// <example>
        ///     Status st = Signup("itexplore", "1234", "Phuc Tho", "Tran", 1, "Hoa Hong", new DateTime(1992, 4, 1));
        /// </example>
        [OperationBehavior]
        public MethodInvokeResult Signup(string Username, string Password, string FirstName, string LastName, int QuestionID, string Answer, DateTime Birthdate)
        {
            return unc.Signup(Username, Password, FirstName, LastName, QuestionID, Answer, Birthdate);
        }

        /// <summary>
        /// Check whether user exist or not
        /// </summary>
        /// <param name="Username">Username (Min: 6, Max: 45)</param>
        /// <returns>Status.SUCCESS or Status.FAIL</returns>
        /// <example>
        ///     Status st = UserExist("itexplore");
        /// </example>
        [OperationBehavior]
        public MethodInvokeResult UserExist(string Username)
        {
            return unc.UserExist(Username);
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="UserID">User ID</param>
        /// <param name="OldPassword">Old Password (Min: 8, Max: 45)</param>
        /// <param name="NewPassword">New Password (Min: 8, Max: 45)</param>
        /// <returns>Status.SUCCESS or Status.FAIL</returns>
        /// <example>
        ///     Status st = ChangePassword(1, "1234", "12345");
        /// </example>
        [OperationBehavior]
        public MethodInvokeResult ChangePassword(int UserID, string OldPassword, string NewPassword)
        {
            return unc.ChangePassword(UserID, OldPassword, NewPassword);
        }

        /// <summary>
        /// Deactive account
        /// </summary>
        /// <param name="UserID">User ID</param>
        /// <returns>Status.SUCCESS or Status.FAIL</returns>
        /// <example>
        ///     Status st = Deactive(1);
        /// </example>
        [OperationBehavior]
        public MethodInvokeResult Deactive(int UserID)
        {
            return unc.Deactive(UserID);
        }

        /// <summary>
        /// Reactive account
        /// </summary>
        /// <param name="UserID">User ID</param>
        /// <returns>Status.SUCCESS or Status.FAIL</returns>
        /// <example>
        ///     Status st = Reactive(1);
        /// </example>
        [OperationBehavior]
        public MethodInvokeResult Reactive(int UserID)
        {
            return unc.Reactive(UserID);
        }

        /// <summary>
        /// Get all user conversation
        /// </summary>
        /// <param name="UserID">User ID</param>
        /// <returns>List of Conversation</returns>
        /// <example>
        ///     List<Conversation> lstConv = GetConversations(1);
        /// </example>
        [OperationBehavior]
        public List<Conversation> GetConversations(int UserID)
        {
            return unc.GetConversations(UserID);
        }

        /// <summary>
        /// Get newest conversation
        /// </summary>
        /// <param name="UserID">User ID</param>
        /// <returns>List of Conversation</returns>
        /// <example>
        ///     List<Conversation> lstConv = GetNewestConversations(1);
        /// </example>
        [OperationBehavior]
        public List<Conversation> GetNewestConversations(int UserID)
        {
            return unc.GetNewestConversations(UserID);
        }
    }
}
