﻿using System;
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
        /// Get group info by ID
        /// </summary>
        /// <param name="GroupID">Group ID</param>
        /// <returns>Return group info (FriendGroup object)</returns>
        [OperationBehavior]
        public FriendGroup GroupInfo(int GroupID)
        {
            return unc.GroupInfo(GroupID);
        }

        /// <summary>
        /// Find user by name
        /// </summary>
        /// <param name="Username">Username</param>
        /// <returns>Return user info (Users object)</returns>
        /// <example>
        ///     Users u = FindName("itexplore");
        /// </example>
        public Users FindName(String Username)
        {
            return unc.FindName(Username);
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

        [OperationBehavior]
        public List<Users> FriendRequests(int UserID)
        {
            return unc.FriendRequests(UserID);
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
        /// Add new group
        /// </summary>
        /// <param name="UserID">UserID of group's owner</param>
        /// <param name="Name">Group's name (Min: 1, Max: 45)</param>
        /// <param name="NewGroupID">Push new group id back to you</param>
        /// <returns>Status.SUCCESS, Status.FAIL</returns>
        [OperationBehavior]
        public MethodInvokeResult AddGroup(int UserID, String Name, out int NewGroupID)
        {
            return unc.AddGroup(UserID, Name, out NewGroupID);
        }

        /// <summary>
        /// Remove a group
        /// </summary>
        /// <param name="GroupID">GroupID of group to be removed</param>
        /// <param name="RemoveContact">Remove contact in group whether or not</param>
        /// <returns>Status.SUCCESS, Status.FAIL</returns>
        public MethodInvokeResult RemoveGroup(int GroupID, bool RemoveContact)
        {
            return unc.RemoveGroup(GroupID, RemoveContact);
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

        [OperationBehavior]
        public MethodInvokeResult LoginHash(string Username, string Password)
        {
            return unc.LoginHash(Username, Password);
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
        /// Accept a friend request
        /// </summary>
        /// <param name="UserID">UserID of user received request</param>
        /// <param name="FriendID">UserID of user raise request</param>
        /// <param name="GroupID">GroupID of group need to be added</param>
        /// <returns>Status.SUCCESS, Status.FAIL</returns>
        [OperationBehavior]
        public MethodInvokeResult AcceptFriendRequest(int UserID, int FriendID, int GroupID)
        {
            return unc.AcceptFriendRequest(UserID, FriendID, GroupID);
        }

        /// <summary>
        /// Ignore a friend request
        /// </summary>
        /// <param name="UserID">UserID of user received request</param>
        /// <param name="FriendID">UserID of user raise request</param>
        /// <param name="GroupID">GroupID of group need to be added</param>
        /// <returns>Status.SUCCESS, Status.FAIL</returns>
        [OperationBehavior]
        public MethodInvokeResult IgnoreFriendRequest(int UserID, int FriendID)
        {
            return unc.IgnoreFriendRequest(UserID, FriendID);
        }

        /// <summary>
        /// Move contact to new group
        /// </summary>
        /// <param name="UserID">UserID of user owned contact</param>
        /// <param name="FriendID">UserID of contact need to be moved</param>
        /// <param name="NewGroupID">GroupID of group move to</param>
        /// <returns>Status.SUCCESS, Status.FAIL</returns>
        public MethodInvokeResult MoveContact(int UserID, int FriendID, int NewGroupID)
        {
            return unc.MoveContact(UserID, FriendID, NewGroupID);
        }

        /// <summary>
        /// Remove contact from user's friendlist
        /// </summary>
        /// <param name="UserID">UserIf of user owned contact</param>
        /// <param name="FriendID">UserID of contact need to be removed</param>
        /// <returns>Status.SUCCESS, Status.FAIL</returns>
        public MethodInvokeResult RemoveContact(int UserID, int FriendID)
        {
            return unc.RemoveContact(UserID, FriendID);
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
