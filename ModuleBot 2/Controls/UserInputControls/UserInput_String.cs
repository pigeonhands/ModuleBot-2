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
        public UserInput_String(UserInputTagData _Data)
        {
            InitializeComponent();
            Data = _Data;
            if(!string.IsNullOrEmpty(Data.Value as string))
            {
                richTextBox1.Text = (string)Data.Value;
            }
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
            Data.Value = richTextBox1.Text;
        }

        private void UserInput_String_Load(object sender, EventArgs e)
        {

        }
    }
}
