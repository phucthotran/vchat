using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Core.Data
{
    public class CommandExecuter : Dictionary<CommandType, Action<CommandResponse>>
    {
        public void Set(CommandType type, Action<CommandResponse> action)
        {
            this.Add(type, action);
        }

        public Action<CommandResponse> Get(CommandType type)
        {
            return this[type];
        }
    }

    public class CommandResponse
    {
        public string TargetUser { get; private set; }
        public object[] Params { get; private set; }
        public CommandResponse(Command cmd)
        {
            this.TargetUser = cmd.Metadata.TargetUser;
            this.Params = cmd.Metadata.Datas;
        }
    }
}
