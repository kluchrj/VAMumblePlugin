using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

using MumbleSharp;

namespace MumblePlugin
{
    public class VoiceAttackPlugin
    {
        private static string addr, name, pass, chan;
        private static int port;
        private static bool configLoaded;

        private static ConsoleMumbleProtocol protocol;
        private static MumbleConnection connection;
        private static Thread t;

        private static string CurrentDirectory;

        // -------------------

        public static string VA_DisplayName()
        {
            return "Mumble Client Plugin";
        }

        public static string VA_DisplayInfo()
        {
            return "A mumble client that can send messages to a server.";
        }

        public static Guid VA_Id()
        {
            return new Guid("{4c6f0ee1-7bc6-4da6-9adc-386b3e91df81}");
        }
        
        public static void VA_Init1(ref Dictionary<string, object> state, ref Dictionary<string, Int16?> shortIntValues, ref Dictionary<string, string> textValues, ref Dictionary<string, int?> intValues, ref Dictionary<string, decimal?> decimalValues, ref Dictionary<string, Boolean?> booleanValues, ref Dictionary<string, object> extendedValues)
        {
            CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            LoadConfig();

            if (!configLoaded)
                return;
            
            try
            {
                protocol = new ConsoleMumbleProtocol();
                connection = new MumbleConnection(new IPEndPoint(Dns.GetHostAddresses(addr).First(a => a.AddressFamily == AddressFamily.InterNetwork), port), protocol);
                connection.Connect(name, pass, new string[0], addr);

                t = new Thread(a => UpdateLoop(connection)) { IsBackground = true };
                t.Start();

                //When localuser is set it means we're really connected
                while (!protocol.ReceivedServerSync)
                {
                }

                if (chan != "")
                {
                    foreach (var c in protocol.Channels.ToArray().Where(u => u.Name == chan.Trim()))
                    {
                        protocol.LocalUser.Move(c);
                    }
                }
            }
            catch (Exception e)
            {
                LogMessage("Error: " + e.Message);
            }
        }

        public static void VA_Exit1(ref Dictionary<string, object> state)
        {
            try
            {
                t.Abort();
                connection.Close();
            }
            catch { }
        }

        public static void VA_Invoke1(String context, ref Dictionary<string, object> state, ref Dictionary<string, Int16?> shortIntValues, ref Dictionary<string, string> textValues, ref Dictionary<string, int?> intValues, ref Dictionary<string, decimal?> decimalValues, ref Dictionary<string, Boolean?> booleanValues, ref Dictionary<string, object> extendedValues)
        {
            if (connection == null || connection.State != ConnectionStates.Connected)
            {
                LogMessage("Error: Not processing command, unable to connect");
                return;
            }

            string text;

            if (textValues.TryGetValue("TextToSend", out text))
            {
                protocol.LocalUser.Channel.SendMessage(text, false);

                textValues["TextToSend"] = null;
            }
        }

        // -------------------
        
        private static void UpdateLoop(MumbleConnection connection)
        {
            while (connection.State != ConnectionStates.Disconnected)
            {
                connection.Process();
            }
        }

        private static void LoadConfig()
        {
            IniFile ini = new IniFile(CurrentDirectory + @"\server.ini");

            if (File.Exists(CurrentDirectory + @"\server.ini"))
            {
                addr = ini.IniReadValue("General", "Hostname");
                if (!int.TryParse(ini.IniReadValue("General", "Port"), out port))
                {
                    port = 64738;
                    LogMessage("Warning: Could not read port info, using 64738");
                }

                name = ini.IniReadValue("General", "Username");
                pass = ini.IniReadValue("General", "Password");
                chan = ini.IniReadValue("General", "Channel");

                configLoaded = true;
            }
            else
            {
                LogMessage("Error: Could not read server configuration file, generating a new one");

                ini.IniWriteValue("General", "Hostname", "localhost");
                ini.IniWriteValue("General", "Port", "64738");
                ini.IniWriteValue("General", "Username", ".VABot");
                ini.IniWriteValue("General", "Password", "");
                ini.IniWriteValue("General", "Channel", "");

                configLoaded = false;
            }
        }

        private static void LogMessage(string Message)
        {
            FileInfo LogFile = new FileInfo(CurrentDirectory + @"\log.txt");

            if (LogFile.Directory.Exists)
                using (var writer = LogFile.AppendText())
                {
                    writer.WriteLine(String.Format("[{0}] {1}", DateTime.Now, Message));
                }
        }
    }
}