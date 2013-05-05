using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChat.Model;
using vChat.Service.UserService;
using vChat.Model.Entities;
using System.Windows;

namespace vChat.Module.FriendList
{
    public partial class FriendsList
    {
        private GroupFriendList FriendList(int UserID)
        {
            return this.Get<UserServiceClient>().FriendList(UserID);
        }

        private List<Users> FriendRequests(int UserID)
        {
            return this.Get<UserServiceClient>().FriendRequests(UserID).ToList();
        }

        private void AcceptRequest(int UserID, int FriendID, int GroupID)
        {
            MethodInvokeResult result = this.Get<UserServiceClient>().AcceptFriendRequest(UserID, FriendID, GroupID);
            Helper.ShowMessage(result);
        }

        private void IgnoreRequest(int UserID, int FriendID)
        {
            MethodInvokeResult result = this.Get<UserServiceClient>().IgnoreFriendRequest(UserID, FriendID);
            Helper.ShowMessage(result);
        }

        private void MoveContact(int UserID, int FriendID, int NewGroupID)
        {
            MethodInvokeResult result = this.Get<UserServiceClient>().MoveContact(UserID, FriendID, NewGroupID);
            Helper.ShowMessage(result);
        }

        private void RemoveContact(int UserID, int FriendID)
        {
            MethodInvokeResult result = this.Get<UserServiceClient>().RemoveContact(UserID, FriendID);
            Helper.ShowMessage(result);
        }

        private FriendGroup GetGroup(int GroupID)
        {
            return this.Get<UserServiceClient>().GroupInfo(GroupID);
        }

        private bool AddNewGroup(int UserID, String Name, ref int NewGroupID)
        {
            MethodInvokeResult result = this.Get<UserServiceClient>().AddGroup(UserID, Name, ref NewGroupID);

            Helper.ShowMessage(result);

            if (result.Status == MethodInvokeResult.RESULT.SUCCESS)
                return true;
            //else
            return false;
        }

        private void RemoveGroup(int GroupID, bool RemoveContact)
        {
            MethodInvokeResult result = this.Get<UserServiceClient>().RemoveGroup(GroupID, RemoveContact);
            Helper.ShowMessage(result);
        }
    }
}
