using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBotPlugin
{
    public interface IBot
    {
        void SayMessage(string message, params object[] data);

    }
}
