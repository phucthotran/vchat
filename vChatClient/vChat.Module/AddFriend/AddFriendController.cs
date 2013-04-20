using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChat.Model;
using vChat.Service.UserService;
using System.Windows;
using vChat.Model.Entities;

namespace vChat.Module.AddFriend
{
    public partial class AddFriend
    {
        private GroupFriendList FriendList(int UserID)
        {
            return this.Get<UserServiceClient>().FriendList(UserID);
        }

        private bool AddNewGroup(int UserID, String Name, out int NewGroupID)
        {
            MethodInvokeResult result = this.Get<UserServiceClient>().AddGroup(out NewGroupID, UserID, Name);

            switch (result.Status)
            {
                case MethodInvokeResult.RESULT.SUCCESS:
                    return true;

                case MethodInvokeResult.RESULT.FAIL:
                    String errors = String.Join(",", result.Errors);
                    MessageBox.Show("Please fix following errors: \n {0}", errors);
                    return false;
            }

            return false;
        }

        private bool AddNewFriend(int UserID, String FriendName, int GroupID)
        {
            MethodInvokeResult result = this.Get<UserServiceClient>().AddFriend(UserID, FriendName, GroupID);

            switch (result.Status)
            {
                case MethodInvokeResult.RESULT.INPUT_ERROR:
                    MessageBox.Show(result.Message);
                    return false;

                case MethodInvokeResult.RESULT.SUCCESS:
                    return true;

                case MethodInvokeResult.RESULT.FAIL:
                    String errors = String.Join(",", result.Errors);
                    MessageBox.Show("Please fix following errors: \n {0}", errors);
                    return false;
            }

            return false;
        }
    }
}
