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
using System.Xml.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace ModuleBot_2
{
    public partial class MainWindow : Form
    {
        List<LoadedPlugin> PluginList = new List<LoadedPlugin>();
        Dictionary<string, CommandHandler> HandlerList = new Dictionary<string, CommandHandler>();
        HashSet<string> Moderators = new HashSet<string>();

        HashSet<string> PluginIDList = new HashSet<string>();
        IRCBot bot;
        public MainWindow()
        {
            using (LoadBotForm lbf = new LoadBotForm())
            {
                if(lbf.ShowDialog() != DialogResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }
                bot = new IRCBot(lbf.BotLogin);
            }
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
                    plugin.OnException += Plugin_OnException;
                    if (!PluginIDList.Add(plugin.PluginID))
                        throw new Exception("Duplicate id");

                    HandlerBundle handlers = new HandlerBundle();

                    handlers.Bot = new BotHandler(plugin);
                    handlers.Bot.OnException += OnException;
                    handlers.Bot.OnSayMessage += Bot_OnSayMessage;

                    handlers.UI = new BotUIHandler(plugin);
                    handlers.UI.OnTabAdd += UI_OnTabAdd;

                    plugin.Initilze(handlers);


                    foreach (var command in plugin.Details.LoadedCommands)
                    {
                        var handler = new CommandHandler(plugin, command);
                        if (!HandlerList.ContainsKey(handler.ID))
                        {
                            HandlerList.Add(handler.ID, handler);
                        }
                    }
                       

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

                LoadSettings();
            }
        }

        private void Plugin_OnException(LoadedPlugin parent, Exception ex)
        {
            string PluginName = "Unknowen";
            if(parent != null)
            {
                PluginName = parent.Details.Name;
            }
            var exceptionItem = new ListViewItem(PluginName);
            exceptionItem.SubItems.Add(ex.Message);
            pluginExceptionList.Items.Add(exceptionItem);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
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
            try
            {

                using (XmlTextWriter xml = new XmlTextWriter("Modulebot.save", Encoding.UTF8))
                {
                    xml.Formatting = Formatting.Indented;
                    xml.WriteStartDocument();

                    xml.WriteStartElement("MBot");

                    xml.WriteStartElement("commands");

                    foreach (ListViewItem lvItem in listView1.Items)
                    {
                        var command = (RegisteredCommand)lvItem.Tag;
                        xml.WriteStartElement("Registered");

                        xml.WriteAttributeString("ID", command.Handler.ID);
                        xml.WriteAttributeString("Flag", command.Flag);
                        xml.WriteAttributeString("IsRegex", command.FlagIsRegex ? "1" : "0");
                        xml.WriteAttributeString("ModOnly", command.IsModOnly ? "1" : "0");
                        xml.WriteAttributeString("CaseSensistve", command.FlagIsCaseSensitive ? "1" : "0");

                        xml.WriteStartElement("UserData");

                        foreach(var ud in command.UserDataInput.ObjectData)
                        {
                            xml.WriteStartElement("object");
                            xml.WriteAttributeString("Name", ud.Key);
                            xml.WriteAttributeString("Value", SerilizeObject(ud.Value));
                            xml.WriteEndElement();//data
                        }

                        xml.WriteEndElement();//UserData

                        xml.WriteEndElement();//Registered
                    }
                    xml.WriteEndElement();//commands

                    xml.WriteStartElement("Mods");

                    foreach (ListViewItem i in modList.Items)
                    {
                        xml.WriteStartElement("User");
                        xml.WriteAttributeString("Name", (string)i.Tag);
                        xml.WriteEndElement();//User
                    }

                    xml.WriteEndElement();//commands

                    xml.WriteEndElement(); //MBot

                    xml.WriteEndDocument();
                }

            }
            catch
            {
                MessageBox.Show("Failed to save settings");
            }
        }

        public void LoadSettings()
        {
            try
            {
                XDocument xDoc = XDocument.Load("Modulebot.save");
                var main = xDoc.Element("MBot");
                foreach(var commandElement in main.Descendants("commands").Descendants("Registered"))
                {
                    string ID = commandElement.Attribute("ID").Value;
                    if(HandlerList.ContainsKey(ID))
                    {
                        var handler = HandlerList[ID];
                        var Flag = commandElement.Attribute("Flag").Value;
                        var Isregex = commandElement.Attribute("IsRegex").Value;
                        var ModOnly = commandElement.Attribute("ModOnly").Value;
                        var CaseSensitive = commandElement.Attribute("CaseSensistve").Value;

                        var registeredCommand = new RegisteredCommand(Flag, handler);

                        var data = new Dictionary<string, object>();

                        foreach(var UserDataElement in commandElement.Descendants("UserData").Descendants("object"))
                        {
                            string name = UserDataElement.Attribute("Name").Value;
                            if (data.ContainsKey(name))
                                continue;
                            object o = DeserilizeObject(UserDataElement.Attribute("Value").Value);
                            if (o == null)
                                continue;
                            data.Add(name, o);
                        }
                        registeredCommand.UserDataInput = new UserData(data);

                        registeredCommand.FlagIsRegex = (Isregex == "1");
                        registeredCommand.IsModOnly = (ModOnly == "1");
                        registeredCommand.FlagIsCaseSensitive = (CaseSensitive == "1");

                        AddCommandToList(registeredCommand);
                    }
                }

                foreach (var commandElement in main.Descendants("Mods").Descendants("User"))
                {
                    ListViewItem mod = new ListViewItem(commandElement.Attribute("Name").Value);
                    modList.Items.Add(mod);
                }
            }
            catch
            {
                MessageBox.Show("Failed to load settings");
            }
        }

        private string SerilizeObject(object o)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, o);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch
            {
                return "";
            }
        }
        private object DeserilizeObject(string o)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(o)))
                {
                    return bf.Deserialize(ms);
                }
            }
            catch
            {
                return null;
            }
        }


        #endregion

        #region " Bot Callbacks "

        private void Bot_OnMessageRecieve(IRCBot sender, MBotMessage message)
        {
            bool hasParamiter = message.Text.Contains(" ");
            string uCmd = string.Empty;
            string[] Parameters = new string[] {  };
            if (hasParamiter)
            {
                uCmd = message.Text.Split(' ')[0];
                Parameters = message.Text.Split(new char[] {' '}, 2)[1].Split(' ');
            }
            else
            {
                uCmd = message.Text;
            }
            foreach(ListViewItem i in listView1.Items)
            {
                RegisteredCommand command = (RegisteredCommand)i.Tag;
                if (command == null)
                    continue;
                if (command.IsModOnly && !Moderators.Contains(message.Sender.ToLower()))
                    continue;
                if (command.Handler.Command.Paramiter == ParamiterType.Must && !hasParamiter)
                    continue;
                if (command.CheckFlag(message))
                    command.Execute(message.Sender, Parameters);
            }

            foreach(var plugin in PluginList)
            {
                if (plugin.Permissions.CanUseChatTrigger)
                    plugin.Permissions.Handlers.ChatTrigger.CheckTrigger(plugin, message.Sender, message.Text);
            }
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
            catch(Exception ex)
            {
                OnExceptionDelegate oEx = parent.GetExceptionCallback();
                if (oEx != null)
                    oEx(parent, ex);
            }
            
        }

        private void OnException(LoadedPlugin parent, Exception ex)
        {
            Debug.WriteLine("[{0}] Exception: {1}", parent.Details.Name, ex.Message);
        }


        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            using (AddCommandForm acf = new AddCommandForm(HandlerList.Values.ToArray()))
            {
                if(acf.ShowDialog() == DialogResult.OK)
                {
                    RegisteredCommand command = acf.NewCommand;
                    AddCommandToList(command);
                }
            }
        }

        void AddCommandToList(RegisteredCommand command)
        {
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
            SaveSettings();
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
                    SaveSettings();
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
            SaveSettings();
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
            SaveSettings();
        }

        private void removeCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            listView1.Items.Remove(listView1.SelectedItems[0]);
            SaveSettings();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            var item = listView1.SelectedItems[0];
            var command = (RegisteredCommand)item.Tag;
            using (AddCommandForm acf = new AddCommandForm(HandlerList.Values.ToArray(), command))
            {
                if(acf.ShowDialog() == DialogResult.OK)
                {
                    command = acf.NewCommand;
                    item.SubItems[1].Text = command.Handler.Command.Name;
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
                    item.SubItems[2].Text = properties;
                    item.Tag = command;
                    SaveSettings();
                }
            }
        }
    }
}
