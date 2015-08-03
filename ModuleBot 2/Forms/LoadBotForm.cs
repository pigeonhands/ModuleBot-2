using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleBot_2.Forms
{
    public partial class LoadBotForm : Form
    {
        public StreamDetails BotLogin { get; private set; }
        public LoadBotForm()
        {
            InitializeComponent();
        }

        private void LoadBotForm_Load(object sender, EventArgs e)
        {
            if (File.Exists("ModuleBot.login"))
            {
                try
                {
                    string[] settings = File.ReadAllLines("ModuleBot.login");
                    usernameTextbox.Text = settings[0];
                    oauthTextbox.Text = settings[1];
                    channelTextbox.Text = settings[2];
                }
                catch
                {

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string[] settings = new string[3];
                settings[0] = usernameTextbox.Text;
                settings[1] = oauthTextbox.Text;
                settings[2] = channelTextbox.Text;
                File.WriteAllLines("ModuleBot.login", settings);
            }
            catch
            { }

            BotLogin = new StreamDetails(usernameTextbox.Text, oauthTextbox.Text, channelTextbox.Text);
            this.DialogResult = DialogResult.OK;
        }
    }
}
