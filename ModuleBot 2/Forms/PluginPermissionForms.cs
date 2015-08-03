using MBotPlugin;
using ModuleBot_2.Plugin;
using ModuleBot_2.Plugin.Permissions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleBot_2.Forms
{
    public partial class PluginPermissionForms : Form
    {
        PermissionsHandler Permissions;
        public PluginPermissionForms(PermissionsHandler _permissions)
        {
            if (_permissions == null)
            {
                this.DialogResult = DialogResult.Abort;
                return;
            }
            Permissions = _permissions;
            InitializeComponent();
            this.Text = string.Format("Permissions for \"{0}\"", Permissions.Parent.Details.Name);
        }

        private void PluginPermissionForms_Load(object sender, EventArgs e)
        {
            foreach(PropertyInfo property in typeof(PermissionsHandler).GetProperties())
            {
                PermissionAttribute pAtt = property.GetCustomAttribute(typeof(PermissionAttribute)) as PermissionAttribute;
                if (pAtt == null)
                    continue;
                if((bool)property.GetValue(Permissions))
                    permissionsList.Items.Add(pAtt.Description);
            }
        }
    }
}
