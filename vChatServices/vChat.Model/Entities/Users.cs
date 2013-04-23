using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Collections.ObjectModel;

namespace vChat.Model.Entities
{
    [DataContract(IsReference = true, Namespace = "http://vchat/entities/Users")]
    public class Users : IDbModel
    {
        [DataMember]        
        public int UserID { get; set; }

        [DataMember]
        public String Username { get; set; }

        [DataMember]
        public String Password { get; set; }

        [DataMember]
        public String FirstName { get; set; }

        [DataMember]
        public String LastName { get; set; }

        [DataMember]
        public byte[] Picture { get; set; }

        [DataMember]
        public String Answer { get; set; }

        [DataMember]
        public DateTime Birthdate { get; set; }

        [DataMember]
        public bool Deactive { get; set; }

        [IgnoreDataMember]
        public Byte[] RowVersion { get; set; }

        [IgnoreDataMember]
        public virtual Question Question { get; set; }

        [IgnoreDataMember]
        public virtual ObservableCollection<Conversation> SentMessage { get; set; }

        [IgnoreDataMember]
        public virtual ObservableCollection<Conversation> ReceivedMessage { get; set; }
                
        // Fake friends list. Use for mapping on database purpose only
        [IgnoreDataMember]
        public virtual ObservableCollection<FriendMap> FriendsFake { get; set; }

        [IgnoreDataMember]
        public virtual ObservableCollection<FriendMap> Friends { get; set; }

        public override int GetHashCode()
        {
            return 333;
        }

        public override bool Equals(object obj)
        {
            if (obj is Users)
            {
                Users compareObj = (Users)obj;

                if (compareObj.UserID == this.UserID)
                    return true;
            }

            return false;
        }
    }
}