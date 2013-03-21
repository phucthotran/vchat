using System;
using System.Runtime.Serialization;

namespace vChat.Model.Entities
{
    [DataContract(IsReference = true, Namespace = "http://vchat/entities/Conversation")]
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

        [IgnoreDataMember]
        public Byte[] RowVersion { get; set; }

        public override int GetHashCode()
        {
            return 555;
        }

        public override bool Equals(object obj)
        {
            if (obj is Conversation)
            {
                Conversation compareObj = (Conversation)obj;

                if (compareObj.ConversationID == this.ConversationID)
                    return true;
            }

            return false;
        }
    }
}