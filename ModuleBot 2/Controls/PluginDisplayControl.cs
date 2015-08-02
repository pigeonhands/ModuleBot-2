using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MBotPlugin;
using ModuleBot_2.Plugin;
using ModuleBot_2.Forms;
using System.IO;

namespace ModuleBot_2.Controls
{
    public partial class PluginDisplayControl : UserControl
    {
        LoadedPlugin Plugin;
        PluginInfomation Details;
        public PluginDisplayControl(LoadedPlugin _plugin)
        {
            Plugin = _plugin;
            Details = Plugin.Details;
            InitializeComponent();
            nameLabel.Text = Details.Name;
            descriptionLabel.Text = Details.Description;
            publisherLabel.Text = Details.Publisher;
            filenameLabel.Text = Path.GetFileName(Plugin.PluginAssembly.Location);
        }

        private void PluginDisplayControl_Load(object sender, EventArgs e)
        {
            permissionsButton.Text = string.Format("Permissions ({0})", Details.EventList.Length);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(Details.EventList.Length < 1)
            {
                MessageBox.Show("No events for this plugin");
                return;
            }
            using (PluginPermissionForms p = new PluginPermissionForms(Details))
            {
                p.ShowDialog();
            }
        }
    }
}
