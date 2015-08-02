using MBotPlugin;
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
    public partial class PluginPermissionForms : Form
    {
        PluginInfomation Details;
        public PluginPermissionForms(PluginInfomation _details)
        {
            Details = _details;
            InitializeComponent();
            this.Text = string.Format("Permissions for \"{0}\"", Details.Name);
        }

        private void PluginPermissionForms_Load(object sender, EventArgs e)
        {
            foreach(IPluginEvent ev in Details.EventList)
            {
                permissionsList.Items.Add(ev.Description);
            }
        }
    }
}
