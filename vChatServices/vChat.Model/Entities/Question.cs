using System;
using System.Runtime.Serialization;

namespace vChat.Model.Entities
{
    [DataContract(IsReference = true)]
    public class Question : IDbModel
    {
        [DataMember]
        public int QuestionID { get; set; }

        [DataMember]
        public String Content { get; set; }

        [DataMember]
        public Byte[] RowVersion { get; set; }
    }
}