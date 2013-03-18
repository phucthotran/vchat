using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace vChat.Model.Entities
{
    [DataContract(IsReference = true)]
    public class FriendGroup : IDbModel
    {
        [DataMember]
        public int GroupID { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public virtual Users Owner { get; set; }
    }
}
