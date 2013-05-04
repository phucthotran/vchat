using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChat.Model.Entities;
using vChat.Service.UserService;

namespace vChat.Module.Avatar
{
    public partial class Avatar
    {
        private Users GetInfo(int UserID)
        {
            return this.Get<UserServiceClient>().Info(UserID);
        }
    }
}
