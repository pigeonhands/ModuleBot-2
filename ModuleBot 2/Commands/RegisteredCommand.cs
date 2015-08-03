using MBotPlugin;
using ModuleBot_2.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModuleBot_2.Commands
{
    public class RegisteredCommand
    {
        public string Flag { get; set; }
        public bool FlagIsRegex { get; set; }
        public bool FlagIsCaseSensitive { get; set; }
        public bool IsModOnly { get; set; }
        public CommandHandler Handler { get; set; }
        public OnExceptionDelegate OnException { get; set; }

        public RegisteredCommand(string _flag, CommandHandler h)
        {
            OnException = h.Parent.GetExceptionCallback();
            Flag = _flag;
            Handler = h;
            FlagIsRegex = false;
            FlagIsCaseSensitive = false;
            IsModOnly = false;
        }
        public void Execute(string sender, string[] paramiters)
        {
            if (!Handler.Parent.Enabled)
                return;
            try
            {
                Handler.Command.Execute(sender, paramiters);
            }
            catch(Exception ex)
            {
                if (OnException != null)
                    OnException(Handler.Parent, ex);
            }
        }
        public bool CheckFlag(MBotMessage m)
        {
            try
            {
                string cmp = m.Text;

                if (!FlagIsCaseSensitive)
                    cmp = cmp.ToLower();
                bool ParamiterExists = cmp.Contains(" ");
                string paramBase = ParamiterExists ? cmp.Split(' ')[0] : cmp;

                if (FlagIsRegex && Handler.Command.Paramiter == ParamiterType.None)
                {
                    return Regex.Match(cmp, Flag).Success;
                }
                else
                {
                    switch (Handler.Command.Paramiter)
                    {
                        case ParamiterType.Must:
                            if (!ParamiterExists)
                                return false;
                            return paramBase == Flag;
                        case ParamiterType.Optional:
                            return cmp == Flag || paramBase == Flag;
                        case ParamiterType.None:
                            return cmp == Flag;
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                if (OnException != null)
                    OnException(Handler.Parent, ex);
                return false;
            }
        }
    }
}
