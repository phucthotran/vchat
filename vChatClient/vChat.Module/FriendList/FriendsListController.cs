using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChat.Model;
using vChat.Service.UserService;

namespace vChat.Module.FriendList
{
    public partial class FriendsList
    {
        private GroupFriendList FriendList(int UserID)
        {
            return this.Get<UserServiceClient>().FriendList(UserID);
        }
    }
}
