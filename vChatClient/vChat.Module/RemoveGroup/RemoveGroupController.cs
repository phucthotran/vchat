using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChat.Model;
using vChat.Service.UserService;
using vChat.Model.Entities;

namespace vChat.Module.RemoveGroup
{
    public partial class RemoveGroup
    {
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
    }
}
