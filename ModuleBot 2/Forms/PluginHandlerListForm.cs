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
    public partial class PluginHandlerListForm : Form
    {
        public CommandHandler SelectedHandler { get; private set; }
        CommandHandler[] handlers;
        public PluginHandlerListForm(CommandHandler[] handlerList)
        {
            InitializeComponent();
            handlers = handlerList;
            LoadPlugins("", true);
        }

        void LoadPlugins(string search, bool checkDesc)
        {
            listView1.Items.Clear();
            foreach (CommandHandler handler in handlers)
            {
                if (handler.Command.Name.ToLower().Contains(search.ToLower()) || (checkDesc && handler.Command.Description.ToLower().Contains(search)))
                {
                    ListViewItem i = new ListViewItem(handler.Command.Name);
                    i.SubItems.Add(handler.Command.Description);
                    i.SubItems.Add(handler.Command.Paramiter.ToString());
                    i.SubItems.Add(handler.Parent.Details.Name);
                    i.Tag = handler;
                    listView1.Items.Add(i);
                }
            }
        }

        private void PluginHandlerListForm_Load(object sender, EventArgs e)
        {

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count < 0)
                return;
            CommandHandler handler = (CommandHandler)listView1.SelectedItems[0].Tag;
            SelectedHandler = handler;
            this.DialogResult = DialogResult.OK;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadPlugins(textBox1.Text, checkBox1.Checked);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            LoadPlugins(textBox1.Text, checkBox1.Checked);
        }
    }
}
