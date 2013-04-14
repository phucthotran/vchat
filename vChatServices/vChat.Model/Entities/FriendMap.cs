using System;
using System.Runtime.Serialization;

namespace vChat.Model.Entities
{
    [DataContract(IsReference = true, Namespace = "http://vchat/entities/FriendMap")]
    public class FriendMap : IDbModel
    {
        [DataMember]
        public int FriendMapID { get; set; }

        //[DataMember]
        //public int UserUserID { get; set; }

        //[DataMember]
        //public int FriendUserID { get; set; }

        [DataMember]
        public virtual Users User { get; set; }

        [DataMember]
        public virtual Users Friend { get; set; }

        [DataMember]
        public virtual FriendGroup FriendGroup { get; set; }

        [DataMember]
        public bool IsAvailable { get; set; }

        [IgnoreDataMember]
        public Byte[] RowVersion { get; set; }

        public override int GetHashCode()
        {
            return 111;
        }

        public override bool Equals(object obj)
        {
            if (obj is FriendMap)
            {
                FriendMap compareObj = (FriendMap)obj;

                if (compareObj.FriendMapID == this.FriendMapID)
                    return true;
            }

            return false;
        }

        public override string ToString()
        {
            return "FriendMap";
        }
    }
}
