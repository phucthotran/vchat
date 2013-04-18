using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vChat.Module.FriendList
{
    public class GroupArgs
    {
        public int _GroupID;
        public String _Name;

        public int GroupID
        {
            get { return _GroupID; }
        }

        public String Name
        {
            get { return _Name; }
        }

        public GroupArgs(int GroupID, String Name)
        {
            _GroupID = GroupID;
            _Name = Name;
        }
    }
}
