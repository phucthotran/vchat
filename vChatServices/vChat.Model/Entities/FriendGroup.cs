using System;
using System.Runtime.Serialization;

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
