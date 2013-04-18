using System;
using System.Collections.Generic;
using System.Text;
using vChat.Model.Entities;

namespace vChat.Module.AddFriend
{
    public class AddFriendArgs
    {
        private String _FriendName;
        private String _GroupName;

        public String FriendName
        {
            get { return _FriendName; }
            set { _FriendName = value; }
        }

        public String GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; }
        }

        public AddFriendArgs(String FriendName, String GroupName)
        {
            _FriendName = FriendName;
            _GroupName = GroupName;
        }
    }
}
