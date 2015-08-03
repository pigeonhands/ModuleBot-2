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
    public partial class UserInput_UnasignedInteger : UserControl
    {
        UserInputTagData Data;
        public UserInput_UnasignedInteger(UserInputTagData _Data)
        {
            InitializeComponent();
            Data = _Data;
            numericUpDown1.Value = (int)Data.Value;
            groupBox1.Text = Data.InputID;
            this.Tag = Data;
        }

        private void UserInput_UnasignedInteger_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Data.Value = (uint)numericUpDown1.Value;
        }
    }
}
