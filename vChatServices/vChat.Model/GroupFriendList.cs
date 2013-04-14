using System;
using System.Collections.Generic;
using System.Text;
using vChat.Model.Entities;
using System.Runtime.Serialization;

namespace vChat.Model
{
    [DataContract(IsReference = true, Namespace = "http://vchat/GroupFriendList")]
    public class GroupFriendList
    {
        private List<FriendGroup> _FriendGroups;

        [DataMember]
        public List<FriendGroup> FriendGroups
        {
            get
            {
                if (_FriendGroups == null)
                {
                    _FriendGroups = new List<FriendGroup>();
                    return _FriendGroups;
                }

                return _FriendGroups;
            }
        }

        public override string ToString()
        {
            return "GroupFriendList";
        }
    }
}
