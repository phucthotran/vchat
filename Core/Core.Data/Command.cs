using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Data
{
    [Serializable()]
    public class Command
    {
        public CommandMetadata Metadata { get; private set; }
        public CommandType Type { get; private set; }
        public string TargetUser { get; set; }
        public Command(CommandType type, string targetUser, CommandMetadata data)
        {
            this.Type = type;
            this.TargetUser = targetUser;
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
