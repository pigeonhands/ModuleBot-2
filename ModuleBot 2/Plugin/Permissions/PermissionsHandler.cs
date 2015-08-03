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

        [Permission("Access to raw chat")]
        public bool CanAccessRawChat { get; private set; }

        [Permission("Can use twitch commands")]
        public bool CanAccessCommands { get; private set; }

        #endregion
        public LoadedPlugin Parent { get; private set; }
        public OnExceptionDelegate OnException { get; set; }
        public HandlerBundle Handlers { get; private set; }

        public PermissionsHandler(LoadedPlugin _parent)
        {
            Parent = _parent;
            Handlers = new HandlerBundle();
        }

        public void AccessCommands()
        {
            CanAccessCommands = true;
        }

        public Event.IRawChatHandler AccessRawChat()
        {
            if (CanAccessRawChat)
                return Handlers.RawChat;
            CanAccessRawChat = true;
            Handlers.RawChat = new RawChatHandler();
            Handlers.RawChat.OnException = OnException;
            return Handlers.RawChat;
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
        public RawChatHandler RawChat { get; set; }
    }
}
