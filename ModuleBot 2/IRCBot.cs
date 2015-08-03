using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBot_2
{
    public delegate void OnDisconnectDelegate(IRCBot sender);
    public delegate void OnMessageRecieveDelegate(IRCBot sender, MBotMessage message);
    public class IRCBot
    {
        public TcpClient Client { get; private set; }
        public StreamCollection Streams { get; private set; }
        public StreamDetails Details { get; private set; }
        public Exception LastError { get; private set; }
        public byte[] StreamBuffer { get; private set; }
        public bool Connected { get; private set; }

        /// <summary>
        /// Delay between heartbeats in miliseconds
        /// </summary>
        public int HeartbeatInterval { get; private set; }

        public event OnDisconnectDelegate OnDisconnect;
        public event OnMessageRecieveDelegate OnMessageRecieve;
        public IRCBot(StreamDetails _details)
        {
            Details = _details;
            Connected = false;
            HeartbeatInterval = 30000;
            Client = new TcpClient();
        }

        public bool Start()
        {
            try
            {
                Client.Connect(Details.Server, Details.ServerPort);
                Streams = new StreamCollection(Client.GetStream());
                RawBuffer("PASS {0}", Details.Password);
                RawBuffer("NICK {0}", Details.Username);
                FlushBuffer();
                if (!Streams.InputStream.ReadLine().ToLower().Contains("welcome"))
                    return false;
                Connected = true;
                SendRaw("JOIN {0}", Details.Channel);
                HandleStreamReading();
                Heartbeat();
                return true;
            }
            catch (Exception ex)
            {
                LastError = ex;
                return false;
            }
        }

        private async void Heartbeat()
        {
            while (Streams.OutputStream != null && Streams.BaseStream != null && HeartbeatInterval > 0)
            {
                SendRaw("-HEARTBEAT");
                await Task.Delay(HeartbeatInterval);
            }
        }


        private async void HandleStreamReading()
        {
            string line = string.Empty;
            while ((line = await Streams.InputStream.ReadLineAsync()) != null)
            {
                string[] messageSegments = line.Split(new char[] { ' ' }, 4);
                if (messageSegments[0] == "PING")
                {
                    SendRaw("PONG {0}", messageSegments[1]);
                    continue;
                }
                if (messageSegments.Length != 4)
                    continue;
                MBotMessage _m = new MBotMessage(messageSegments[0].Split('!')[0].Replace(":", ""), messageSegments[3].Substring(1, messageSegments[3].Length - 1), line);
                if (_m.Sender == "tmi.twitch.tv" && _m.Text == "HEARTBEAT :Unknown command")
                    continue;
                if (OnMessageRecieve != null)
                    OnMessageRecieve(this, _m);
            }
            Stop();
        }


        #region " Sending "

        public void SendMessage(string MessageBody, params string[] args)
        {
            string message = string.Format(MessageBody, args);
            SendRaw("PRIVMSG {0} :{1}", Details.Channel, MessageBody);
        }

        public void SendRaw(string MessageBody, params string[] args)
        {
            string message = string.Format(MessageBody, args);
            Streams.OutputStream.WriteLine(message);
            Streams.OutputStream.FlushAsync();
        }

        public void RawBuffer(string MessageBody, params string[] args)
        {
            string message = string.Format(MessageBody, args);
            Streams.OutputStream.WriteLine(message);
        }
        public void FlushBuffer()
        {
            Streams.OutputStream.Flush();
        }

        public void FlushBufferAsync()
        {
            Streams.OutputStream.FlushAsync();
        }

        private void Stop()
        {
            lock (this)
            {
                if (!Connected)
                    return;
                Connected = false;
                if (OnDisconnect != null)
                    OnDisconnect(this);
                Streams.Close();
            }
        }

        #endregion

    }

    public class StreamCollection
    {
        public NetworkStream BaseStream { get; private set; }
        public StreamWriter OutputStream { get; private set; }
        public StreamReader InputStream { get; private set; }
        public StreamCollection(NetworkStream ns)
        {
            BaseStream = ns;
            OutputStream = new StreamWriter(ns);
            InputStream = new StreamReader(ns);
        }

        public void Close()
        {
            if (BaseStream != null)
                BaseStream.Dispose();
            if (OutputStream != null)
                OutputStream.Dispose();
            if (InputStream != null)
                InputStream.Dispose();
        }
    }

    public class MBotMessage
    {
        public string Sender { get; private set; }
        public string Text { get; private set; }
        public string RawText { get; private set; }
        public MBotMessage(string _sender, string _text, string _raw)
        {
            Sender = _sender;
            Text = _text;
            RawText = _raw;
        }
    }

    public class StreamDetails
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Server { get; private set; }
        public int ServerPort { get; private set; }
        public string Channel { get; private set; }
        public StreamDetails(string _username, string _password, string _channel)
        {
            Username = _username;
            Password = _password;
            Server = "irc.twitch.tv";
            ServerPort = 6667;
            Channel = _channel.ToLower();
            if (!Channel.StartsWith("#"))
                Channel = "#" + Channel;
        }
    }

}
