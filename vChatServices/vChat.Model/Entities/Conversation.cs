using System;
using System.Runtime.Serialization;

namespace vChat.Model.Entities
{
    [DataContract(IsReference = true)]
    public class Conversation : IDbModel
    {
        [DataMember]
        public int ConversationID { get; set; }

        [DataMember]
        public String Message { get; set; }

        [DataMember]
        public DateTime Time { get; set; }

        [DataMember]
        public bool IsRead { get; set; }

        [DataMember]
        public virtual Users SentBy { get; set; }

        [DataMember]
        public virtual Users SendTo { get; set; }

        [DataMember]
        public Byte[] RowVersion { get; set; }
    }
}