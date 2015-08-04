using MBotPlugin;
using ModuleBot_2.Commands;
using ModuleBot_2.Controls.UserInputControls;
using ModuleBot_2.Plugin;
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
        private int userInputHeight = 0;
        private int DefaultHeight = 145;
        bool hasUserInput = false;

        Dictionary<Type, Type> TypeForm = new Dictionary<Type, Type>()
        {
            {typeof(string),  typeof(UserInput_String)},
            {typeof(int), typeof(UserInput_Integer) },
            {typeof(uint), typeof(UserInput_UnasignedInteger) },
            {typeof(bool), typeof(UserInput_Boolean) }
        };
        public AddCommandForm(CommandHandler[] _handlers)
        {
            handlers = _handlers;
            InitializeComponent();
            LoadUserInput();
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
            LoadUserInput(currentCommand.UserDataInput);
        }

        void LoadUserInput(UserData data = null)
        {
            UserInput[] UserData = SelectedHandler.Command.UserData;
            userInputHeight = 0;
            this.Height = DefaultHeight;
            UserInputPanel.Controls.Clear();
            if (UserData == null || UserData == null || UserData.Length < 1)
            {
                this.Height = DefaultHeight;
                UserInputPanel.Visible = false;
                hasUserInput = false;
                return;
            }
            this.Height += 10;
            foreach (var u in UserData)
            {
                if(TypeForm.ContainsKey(u.InputType))
                {
                    var tagData = new UserInputTagData(u.InputType, u.ID);

                    if (data != null)
                    {
                        object value = data.GetValue<object>(u.ID, null);
                        if (value != null && value.GetType() == u.InputType)
                            tagData.Value = value;
                    }

                    Control c = (Control)Activator.CreateInstance(TypeForm[u.InputType], tagData);
                    c.Width = UserInputPanel.Width;
                    c.Parent = UserInputPanel;
                    c.Location = new Point(0, userInputHeight);
                    
                    

                    userInputHeight += c.Height;
                    UserInputPanel.Controls.Add(c);
                }
            }
            hasUserInput = true;
            UserInputPanel.Visible = true;
            UserInputPanel.Height = userInputHeight;
            this.Height += userInputHeight;
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

            var objectData = new Dictionary<string, object>();

            if(hasUserInput)
            {
                foreach(Control c in UserInputPanel.Controls)
                {
                    UserInputTagData data = c.Tag as UserInputTagData;
                    if (data == null)
                        continue;
                    if (objectData.ContainsKey(data.InputID))
                        continue;
                    if(data.InputType == typeof(string) && string.IsNullOrEmpty((string)data.Value))
                    {
                        MessageBox.Show(string.Format("Enter a value for \"{0}\".", data.InputID));
                        return;
                    }
                    objectData.Add(data.InputID, data.Value);
                }
            }

            NewCommand = new RegisteredCommand(FlagTextbox.Text, SelectedHandler);
            NewCommand.IsModOnly = modOnly.Checked;
            NewCommand.FlagIsRegex = Isregex.Checked;
            NewCommand.FlagIsCaseSensitive = CaseSensitive.Checked;

            if (objectData.Count > 0)
                NewCommand.UserDataInput = new UserData(objectData);
            else
                NewCommand.UserDataInput = null;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void AddCommandForm_Load(object sender, EventArgs e)
        {
           // this.Height = DefaultHeight;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (PluginHandlerListForm phf = new PluginHandlerListForm(handlers))
            {
                if(phf.ShowDialog() == DialogResult.OK)
                {
                    LoadHandler(phf.SelectedHandler);
                    LoadUserInput();
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
