using System;

namespace vChat.Module.FriendList
{
    public class FriendArgs
    {
        private int userId;
        private String username;
        private String firstName;
        private String lastName;

        public int UserID
        {
            get { return userId; }
        }

        public String Username
        {
            get { return username; }
        }

        public String FirstName
        {
            get { return firstName; }
        }

        public String LastName
        {
            get { return lastName; }
        }

        public FriendArgs(int UserID, String Username, String FirstName, String LastName)
        {
            userId = UserID;
            username = Username;
            firstName = FirstName;
            lastName = LastName;
        }
    }
}
