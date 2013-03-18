using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace vChat.Model.Entities
{
    [DataContract(IsReference = true)]
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
        public String Answer { get; set; }

        [DataMember]
        public DateTime Birthdate { get; set; }

        [DataMember]
        public bool Deactive { get; set; }

        [DataMember]
        public Byte[] RowVersion { get; set; }

        [DataMember]
        public virtual Question Question { get; set; }

        [DataMember]
        public virtual IList<Conversation> SentMessage { get; set; }

        [DataMember]
        public virtual IList<Conversation> ReceivedMessage { get; set; }

        [DataMember]
        public virtual FriendGroup Group { get; set; }
    }
}