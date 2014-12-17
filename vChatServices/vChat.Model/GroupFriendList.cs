using System;
using System.Collections.Generic;
using System.Text;
using vChat.Model.Entities;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace vChat.Model
{
    [DataContract(IsReference = true, Namespace = "http://vchat/GroupFriendList")]
    public class GroupFriendList
    {
        private ObservableCollection<FriendGroup> _FriendGroups;

        [DataMember]
        public ObservableCollection<FriendGroup> FriendGroups
        {
            get
            {
                if (_FriendGroups == null)
                {
                    _FriendGroups = new ObservableCollection<FriendGroup>();
                    return _FriendGroups;
                }

                return _FriendGroups;
            }
        }
    }
}
