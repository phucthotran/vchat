using System;

namespace vChat.Module.FriendList
{
    /// <summary>
    /// Thông tin bạn bè
    /// </summary>
    public class FriendArgs
    {
        private int userId;
        private String username;
        private String firstName;
        private String lastName;

        /// <summary>
        /// ID của user
        /// </summary>
        public int UserID
        {
            get { return userId; }
        }

        /// <summary>
        /// Tên tài khoản
        /// </summary>
        public String Username
        {
            get { return username; }
        }

        /// <summary>
        /// Tên và tên đệm
        /// </summary>
        public String FirstName
        {
            get { return firstName; }
        }

        /// <summary>
        /// Họ
        /// </summary>
        public String LastName
        {
            get { return lastName; }
        }

        /// <summary>
        /// Khởi tạo thông tin bạn bè
        /// </summary>
        /// <param name="UserID">ID của user</param>
        /// <param name="Username">Tên tài khoản</param>
        /// <param name="FirstName">Tên và tên đêm</param>
        /// <param name="LastName">Họ</param>
        public FriendArgs(int UserID, String Username, String FirstName, String LastName)
        {
            userId = UserID;
            username = Username;
            firstName = FirstName;
            lastName = LastName;
        }
    }
}
