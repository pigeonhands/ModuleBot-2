using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBotPlugin
{
    public class UserInput
    {
        public string ID { get; private set; }
        public Type InputType { get; private set; }
        public UserInput(string _id, Type ty)
        {
            ID = _id;
            InputType = ty;
        }
        public static UserInput Create(string ID, Type InputType)
        {
            return new UserInput(ID, InputType);
        }
    }

    public interface IUserData
    {
        t GetValue<t>(string id, t DefaultValue);
    }
}
