using MBotPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBot_2.Plugin
{
    public class UserData : IUserData
    {
        public Dictionary<string, object> ObjectData { get; private set; }
        public UserData(Dictionary<string, object> data)
        {
            ObjectData = data;
        }

        public t GetValue<t>(string id, t DefaultValue)
        {
            if (ObjectData.ContainsKey(id))
                return (t)ObjectData[id];
            else
                return DefaultValue;
        }
    }
}
