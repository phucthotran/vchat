using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Data
{
    [Serializable()]
    public class Command
    {
        /// <summary>
        /// Đối tượng dùng để thực thi method tương ứng với CommandType
        /// </summary>
        public object Invoker { get; set; }
        public CommandMetadata Metadata { get; private set; }
        public CommandType Type { get; private set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public Command(CommandType type, CommandMetadata data)
        {
            this.Type = type;
            this.Metadata = data;
        }
        public string GetDataString()
        {
            return this.Metadata.ToString();
        }
        public string GetTypeString()
        {
            return this.Type.ToString();
        }
        public override string ToString()
        {
            return (this.GetTypeString() + " - " + this.GetDataString());
        }
    }
}
