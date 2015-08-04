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
    public partial class UserInput_String : UserControl
    {
        UserInputTagData Data;
        bool set = false;
        public UserInput_String(UserInputTagData _Data)
        {
            InitializeComponent();
            Data = _Data;
            if(!string.IsNullOrEmpty(Data.Value as string))
            {
                set = (bool)Data.Value;
            }
            button1.Text = set ? "On" : "Off";
            groupBox1.Text = Data.InputID;
            this.Tag = Data;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void UserInput_String_Load(object sender, EventArgs e)
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
