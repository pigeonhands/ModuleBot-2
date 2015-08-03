using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBotPlugin
{
    public interface IPlugin
    {
        void PluginLoad(IBot BotHandler, IBotUI UIHandler, IPermissions PermissionsHandler);
        PluginInfomation PluginDetails { get; }
    }
}
