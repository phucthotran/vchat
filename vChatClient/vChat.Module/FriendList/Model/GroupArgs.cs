using System;

namespace vChat.Module.FriendList
{
    public class GroupArgs
    {
        public int groupId;
        public String name;

        public int GroupID
        {
            get { return groupId; }
        }

        public String Name
        {
            get { return name; }
        }

        public GroupArgs(int GroupID, String Name)
        {
            groupId = GroupID;
            name = Name;
        }
    }
}
