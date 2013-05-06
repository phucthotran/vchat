using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChat.Service.UserService;
using System.Windows;
using vChat.Model;
using vChat.Model.Entities;

namespace vChat.Module.AddFriend
{
    public partial class AddFriend
    {
        private Users GetUser(int UserID)
        {
            return this.Get<UserServiceClient>().Info(UserID);
        }

        private Users FindUser(String Username)
        {
            return this.Get<UserServiceClient>().FindName(Username);
        }

        private bool AlreadyAdded(int UserID, int FriendID)
        {            
            MethodInvokeResult result = this.Get<UserServiceClient>().AlreadyMakeFriend(UserID, FriendID);

            if (result.Status == MethodInvokeResult.RESULT.SUCCESS)
                return true;

            return false;
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

        private bool AddNewFriend(int UserID, String FriendName, int GroupID)
        {
            MethodInvokeResult result = this.Get<UserServiceClient>().AddFriend(UserID, FriendName, GroupID);

            Helper.ShowMessage(result);

            if (result.Status == MethodInvokeResult.RESULT.SUCCESS)
                return true;
            //else
            return false;
        }
    }
}
