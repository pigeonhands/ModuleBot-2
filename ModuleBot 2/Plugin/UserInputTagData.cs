using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBot_2.Plugin
{
    public delegate void SetDataDelegate(object data);
    public class UserInputTagData
    {
        public Type InputType { get; private set; }
        public string InputID { get; private set; }
        public object Value { get; set; }
        public UserInputTagData(Type t, string id)
        {
            InputType = t;
            InputID = id;
        }
    }
}
