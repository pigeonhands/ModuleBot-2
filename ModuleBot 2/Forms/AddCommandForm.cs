using MBotPlugin;
using ModuleBot_2.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleBot_2.Forms
{
    public partial class AddCommandForm : Form
    {
        public RegisteredCommand NewCommand { get; private set; }
        private CommandHandler SelectedHandler = null;
        private CommandHandler[] handlers;
        public AddCommandForm(CommandHandler[] _handlers)
        {
            handlers = _handlers;
            InitializeComponent();
        }

        public AddCommandForm(CommandHandler[] _handlers, RegisteredCommand currentCommand)
        {
            InitializeComponent();
            this.Text = "Editing Command";
            handlers = _handlers;
            FlagTextbox.Text = currentCommand.Flag;
            modOnly.Checked = currentCommand.IsModOnly;
            Isregex.Checked = currentCommand.FlagIsRegex;
            CaseSensitive.Checked = currentCommand.FlagIsCaseSensitive;
            LoadHandler(currentCommand.Handler);
            
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (SelectedHandler == null)
            {
                MessageBox.Show("Select a command");
                return;
            }
            if (FlagTextbox.Text == string.Empty)
            {
                MessageBox.Show("Enter a flag");
                return;
            }
            NewCommand = new RegisteredCommand(FlagTextbox.Text, SelectedHandler);
            NewCommand.IsModOnly = modOnly.Checked;
            NewCommand.FlagIsRegex = Isregex.Checked;
            NewCommand.FlagIsCaseSensitive = CaseSensitive.Checked;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void AddCommandForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (PluginHandlerListForm phf = new PluginHandlerListForm(handlers))
            {
                if(phf.ShowDialog() == DialogResult.OK)
                {
                    LoadHandler(phf.SelectedHandler);
                }
            }
        }

        void LoadHandler(CommandHandler handler)
        {
            if (handler == null)
                return;
            SelectedHandler = handler;
            if (SelectedHandler == null)
                return;
            if (SelectedHandler.Command.Paramiter != ParamiterType.None)
                Isregex.Checked = false;
            Isregex.Enabled = (SelectedHandler.Command.Paramiter == ParamiterType.None);
            CommandName.Text = SelectedHandler.Command.Name;
        }
    }
}
