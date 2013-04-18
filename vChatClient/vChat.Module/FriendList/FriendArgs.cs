using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vChat.Module.FriendList
{
    public class FriendArgs
    {
        public int _UserID;
        public String _Username;
        public String _FirstName;
        public String _LastName;

        public int UserID
        {
            get { return _UserID; }
        }

        public String Username
        {
            get { return _Username; }
        }

        public String FirstName
        {
            get { return _FirstName; }
        }

        public String LastName
        {
            get { return _LastName; }
        }

        public FriendArgs(int UserID, String Username, String FirstName, String LastName)
        {
            _UserID = UserID;
            _Username = Username;
            _FirstName = FirstName;
            _LastName = LastName;
        }
    }
}
