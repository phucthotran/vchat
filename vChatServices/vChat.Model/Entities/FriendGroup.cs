﻿using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace vChat.Model.Entities
{
    [DataContract(IsReference = true, Namespace = "http://vchat/entities/FriendGroup")]
    public class FriendGroup : IDbModel
    {
        [DataMember]
        public int GroupID { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public virtual Users Owner { get; set; }

        private ObservableCollection<Users> _Friends;
        
        [DataMember]
        public ObservableCollection<Users> Friends
        {
            get
            {
                if (_Friends == null)
                {
                    _Friends = new ObservableCollection<Users>();
                    return _Friends;
                }

                return _Friends;
            }
            set { _Friends = value; }
        }

        [IgnoreDataMember]
        public Byte[] RowVersion { get; set; }

        public override int GetHashCode()
        {
            return 222;
        }

        public override bool Equals(object obj)
        {
            if (obj is FriendGroup)
            {
                FriendGroup compareObj = (FriendGroup)obj;

                if (compareObj.GroupID == this.GroupID)
                    return true;
            }

            return false;
        }
    }
}
