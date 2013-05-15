using System;
using System.Collections.Generic;
using System.Text;
using vChat.Model.Entities;

namespace vChat.Module.AddFriend
{
    public class AddFriendArgs
    {
        private String friendName;
        private String groupName;

        /// <summary>
        /// Tên bạn bè
        /// </summary>
        public String FriendName
        {
            get { return friendName; }
            set { friendName = value; }
        }

        /// <summary>
        /// Tên nhóm
        /// </summary>
        public String GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        public AddFriendArgs(String FriendName, String GroupName)
        {
            friendName = FriendName;
            groupName = GroupName;
        }
    }
}
