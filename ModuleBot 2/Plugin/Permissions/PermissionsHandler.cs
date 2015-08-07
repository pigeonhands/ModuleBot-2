using MBotPlugin;
using ModuleBot_2.Plugin.Permissions.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBot_2.Plugin.Permissions
{
    public class PermissionsHandler : IPermissions
    {
        #region " Permissions "

        [Permission("Can trigger operations from chat text")]
        public bool CanUseChatTrigger { get; private set; } = false;

        [Permission("Can use twitch commands")]
        public bool CanAccessCommands { get; private set; } = false;

        #endregion
        public LoadedPlugin Parent { get; private set; }
        public OnExceptionDelegate OnException { get; set; }
        public HandlerBundle Handlers { get; private set; }

        public PermissionsHandler(LoadedPlugin _parent)
        {
            Parent = _parent;
            Handlers = new HandlerBundle();
        }

        public void UseCommands()
        {
            CanAccessCommands = true;
        }
        public Event.IChatTrigger UseChatTriggering()
        {
            if (CanUseChatTrigger)
                return Handlers.ChatTrigger;
            CanUseChatTrigger = true;
            Handlers.ChatTrigger = new ChatTriggerHandler();
            Handlers.ChatTrigger.OnException = OnException;
            return Handlers.ChatTrigger;
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PermissionAttribute : Attribute
    {
        public string Description { get; private set; }
        public PermissionAttribute(string _descrption)
        {
            Description = _descrption;
        }
    }

    public class HandlerBundle
    {
        public ChatTriggerHandler ChatTrigger { get; set; }
    }
}
