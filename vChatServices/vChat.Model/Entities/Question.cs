using System;
using System.Runtime.Serialization;

namespace vChat.Model.Entities
{
    [DataContract(IsReference = true, Namespace = "http://vchat/entities/Question")]    
    public class Question : IDbModel
    {
        [DataMember]
        public int QuestionID { get; set; }

        [DataMember]
        public String Content { get; set; }

        [IgnoreDataMember]
        public Byte[] RowVersion { get; set; }

        public override int GetHashCode()
        {
            return 444;
        }

        public override bool Equals(object obj)
        {
            if (obj is Question)
            {
                Question compareObj = (Question)obj;

                if (compareObj.QuestionID == this.QuestionID)
                    return true;
            }

            return false;
        }
    }
}