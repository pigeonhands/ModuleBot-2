using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MBotPlugin;
using ModuleBot_2.Plugin;
using System.IO;
using System.Diagnostics;
using ModuleBot_2.Controls;
using ModuleBot_2.Forms;
using ModuleBot_2.Commands;
using System.Xml;

namespace ModuleBot_2
{
    public partial class MainWindow : Form
    {
        List<LoadedPlugin> PluginList = new List<LoadedPlugin>();
        List<CommandHandler> HandlerList = new List<CommandHandler>();
        List<PluginEvent.RawChatEvent> RawchatEventList = new List<PluginEvent.RawChatEvent>();
        HashSet<string> Moderators = new HashSet<string>();

        HashSet<string> PluginIDList = new HashSet<string>();
        IRCBot bot;
        public MainWindow()
        {
            InitializeComponent();

            DirectoryInfo PluginDirectory;
            try
            {
                PluginDirectory = new DirectoryInfo("Plugins");
                if (!PluginDirectory.Exists)
                    PluginDirectory.Create();
            }
            catch
            {
                MessageBox.Show("No access to \"Plugin\" Directory.");
                return;
            }
            foreach (FileInfo pluginFile in PluginDirectory.GetFiles("*.dll"))
            {
                try
                {
                    LoadedPlugin plugin = PluginHandler.LoadPlugin(pluginFile.FullName);

                    if (!PluginIDList.Add(plugin.PluginID))
                        throw new Exception("Duplicate id");

                    HandlerBundle handlers = new HandlerBundle();

                    handlers.Bot = new BotHandler(plugin);
                    handlers.Bot.OnException += OnException;
                    handlers.Bot.OnSayMessage += Bot_OnSayMessage;

                    handlers.UI = new BotUIHandler(plugin);
                    handlers.UI.OnTabAdd += UI_OnTabAdd;

                    foreach(IPluginEvent pEvent in plugin.Details.EventList)
                    {
                        Type eventType = pEvent.GetType();
                        if (eventType == typeof(PluginEvent.RawChatEvent))
                            RawchatEventList.Add((PluginEvent.RawChatEvent)pEvent);
                        if (eventType == typeof(PluginEvent.UseChatCommands))
                            plugin.CanUseCommands = true;
                    }

                    plugin.Initilze(handlers);


                    foreach (var command in plugin.Details.LoadedCommands)
                        HandlerList.Add(new CommandHandler(plugin, command));

                    PluginDisplayControl display = new PluginDisplayControl(plugin);
                    display.Parent = PluginDisplayPanel;
                    display.Width = PluginDisplayPanel.Width;
                    display.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                    display.Location = new Point(0, PluginList.Count * display.Height);
                    PluginDisplayPanel.Controls.Add(display);

                    PluginList.Add(plugin);
                }
                catch
                {
                    Debug.WriteLine("Error on file {0}", pluginFile.Name);
                }
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            bot = new IRCBot(new StreamDetails("BahNahNahBot", "oauth:ezb1az5khm015jx3gniq8kajxu07qr", "bahnahnah"));
            bot.OnDisconnect += Bot_OnDisconnect;
            bot.OnMessageRecieve += Bot_OnMessageRecieve;
            if(!bot.Start())
            {
                MessageBox.Show("Failed to start bot!");
            }
        }

        #region " Saving "

        public void SaveSettings()
        {
            using (XmlTextWriter xml = new XmlTextWriter("Modulebot.save", Encoding.UTF8))
            {
                xml.WriteStartDocument();

                xml.WriteStartElement("MBot");

                xml.WriteStartElement("commands");

                xml.WriteEndElement();//commands

                xml.WriteEndElement(); //MBot

                xml.WriteEndDocument();
            }
        }

        #endregion

        #region " Bot Callbacks "

        private void Bot_OnMessageRecieve(IRCBot sender, MBotMessage message)
        {
            bool hasParamiter = message.Text.Contains(" ");
            string uCmd = string.Empty;
            string[] Paramiters = new string[] {  };
            if (hasParamiter)
            {
                uCmd = message.Text.Split(' ')[0];
                Paramiters = message.Text.Split(new char[] {' '}, 2)[1].Split(' ');
            }
            else
            {
                uCmd = message.Text;
            }
            foreach(ListViewItem i in listView1.Items)
            {
                RegisteredCommand command = (RegisteredCommand)i.Tag;
                if (command.IsModOnly && !Moderators.Contains(message.Sender.ToLower()))
                    continue;
                if (command.Handler.Command.Paramiter == ParamiterType.Must && !hasParamiter)
                    continue;
                if (command.CheckFlag(message))
                    command.Execute(message.Sender, Paramiters);
            }
            foreach(var chatEvent in RawchatEventList)
                chatEvent.Execute(message.Sender, message.Text);

        }

        private void Bot_OnDisconnect(IRCBot sender)
        {
            MessageBox.Show("Bot lost connection.");
        }

        #endregion

        #region " Plugin Callbacks "

        private void UI_OnTabAdd(LoadedPlugin parent, PluginTab tab)
        {
            if(tab != null)
            {
                try
                {
                    TabPage t = new TabPage(tab.TabText);
                    tab.TabControl.Parent = t;
                    tab.TabControl.Dock = DockStyle.Fill;
                    t.Controls.Add(tab.TabControl);
                    mainTabControl.Controls.Add(t);
                }
                catch(Exception ex)
                {
                    OnException(parent, ex);
                }
            }
        }

        private void Bot_OnSayMessage(LoadedPlugin parent, string message, params object[] data)
        {
            try
            {
                if (message.StartsWith("/") && !parent.CanUseCommands)
                    throw new Exception("No permission for commands");
                bot.SendMessage(string.Format(message, data));
            }
            catch
            {

            }
            
        }

        private void OnException(LoadedPlugin parent, Exception ex)
        {
            Debug.WriteLine("[{0}] Exception: {1}", parent.Details.Name, ex.Message);
        }


        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            using (AddCommandForm acf = new AddCommandForm(HandlerList.ToArray()))
            {
                if(acf.ShowDialog() == DialogResult.OK)
                {
                    RegisteredCommand command = acf.NewCommand;
                    ListViewItem i = new ListViewItem(command.Flag);
                    i.SubItems.Add(command.Handler.Command.Name);
                    List<string> PropertyValues = new List<string>();

                    if (command.IsModOnly)
                        PropertyValues.Add("Mod Only");
                    if (command.FlagIsRegex)
                        PropertyValues.Add("Regex");
                    if (command.FlagIsCaseSensitive)
                        PropertyValues.Add("Case sensitive");

                    string properties = string.Join(", ", PropertyValues.ToArray());
                    if (string.IsNullOrEmpty(properties))
                        properties = "None";
                    i.SubItems.Add(properties);

                    i.Tag = command;
                    listView1.Items.Add(i);
                }
            }
        }

        private void addNewModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AddModeratorForm amf = new AddModeratorForm())
            {
                if(amf.ShowDialog() == DialogResult.OK)
                {
                    if (!Moderators.Add(amf.Username.ToLower()))
                        return;
                    ListViewItem i = new ListViewItem(amf.Username);
                    i.Tag = amf.Username;
                    modList.Items.Add(i);
                }
            }
        }

        private void selectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete selected moderators from the list?", "Remove moderators", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            foreach(ListViewItem i in modList.SelectedItems)
            {
                string username = (string)i.Tag;
                modList.Items.Remove(i);
                Moderators.Remove(username.ToLower());
            }
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete all moderators from the list?", "Remove moderators", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            foreach (ListViewItem i in modList.Items)
            {
                string username = (string)i.Tag;
                modList.Items.Remove(i);
                Moderators.Remove(username.ToLower());
            }
        }

        private void removeCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }
    }
}
