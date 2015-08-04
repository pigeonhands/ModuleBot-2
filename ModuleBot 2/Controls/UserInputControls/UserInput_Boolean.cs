using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModuleBot_2.Plugin;

namespace ModuleBot_2.Controls.UserInputControls
{
    public partial class UserInput_Boolean : UserControl
    {
        UserInputTagData Data;
        bool set = false;
        public UserInput_Boolean(UserInputTagData _Data)
        {
            InitializeComponent();
            Data = _Data;
            set = (bool)Data.Value;
            button1.Text = set ? "On" : "Off";
            groupBox1.Text = Data.InputID;
            this.Tag = Data;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            set = !set;
            Data.Value = set;
            button1.Text = set ? "On" : "Off";
        }
    }
}
