using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleCommands.Controls
{
    public partial class RawChatLog : UserControl
    {
        public RawChatLog()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void NewMessage(string sender, string message)
        {
            richTextBox1.Text += string.Format("<{0}> {1}\n", sender, message);
        }
    }
}
