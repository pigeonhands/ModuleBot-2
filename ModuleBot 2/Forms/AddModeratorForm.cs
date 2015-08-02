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
    public partial class AddModeratorForm : Form
    {
        public string Username
        {
            get; private set;
        }
        public AddModeratorForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == string.Empty)
            {
                MessageBox.Show("Enter a username");
                return;
            }
            Username = textBox1.Text;
            
            DialogResult dr = MessageBox.Show(string.Format("Are you sure? This will gave the twitch user \"{0}\" Access to all admin commands in modulebot", Username), "Confirm", MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Cancel)
                return;
            if (dr == DialogResult.No)
                this.DialogResult = DialogResult.No;
            if (dr == DialogResult.Yes)
                this.DialogResult = DialogResult.OK;
        }
    }
}
