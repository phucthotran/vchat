using System;

namespace vChat.Module.FriendList
{
    /// <summary>
    /// Thông tin nhóm
    /// </summary>
    public class GroupArgs
    {
        public int groupId;
        public String name;

        /// <summary>
        /// ID của nhóm
        /// </summary>
        public int GroupID
        {
            get { return groupId; }
        }

        /// <summary>
        /// Tên nhóm
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        /// <summary>
        /// Khởi tạo thông tin nhóm
        /// </summary>
        /// <param name="GroupID">ID của nhóm</param>
        /// <param name="Name">Tên nhóm</param>
        public GroupArgs(int GroupID, String Name)
        {
            groupId = GroupID;
            name = Name;
        }
    }
}
