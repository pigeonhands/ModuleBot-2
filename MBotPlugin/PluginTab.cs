using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MBotPlugin
{
    public class PluginTab
    {
        public PluginTab(string text, Control pControl)
        {
            TabText = text;
            TabControl = pControl;
        }
        public string TabText { get; private set; }
        public Control TabControl { get; private set; }
    }
}
