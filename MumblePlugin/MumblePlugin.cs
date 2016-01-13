using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MumbleSharp;
using MumbleSharp.Model;

namespace MumblePlugin
{
    public class VoiceAttackPlugin
    {
        private static string addr, name, pass;
        private static int port;

        private static ConsoleMumbleProtocol protocol;
        private static MumbleConnection connection;
        private static Thread t;

        // -------------------

        public static string VA_DisplayName()
        {
            return "Mumble Client Plugin";
        }

        public static string VA_DisplayInfo()
        {
            return "Sends messages to mumble chat.";
        }

        public static Guid VA_Id()
        {
            return new Guid("{4c6f0ee1-7bc6-4da6-9adc-386b3e91df81}");
        }
        
        public static void VA_Init1(ref Dictionary<string, object> state, ref Dictionary<string, Int16?> shortIntValues, ref Dictionary<string, string> textValues, ref Dictionary<string, int?> intValues, ref Dictionary<string, decimal?> decimalValues, ref Dictionary<string, Boolean?> booleanValues, ref Dictionary<string, object> extendedValues)
        {
            LoadConfig();
            
            protocol = new ConsoleMumbleProtocol();

            try {
                connection = new MumbleConnection(new IPEndPoint(Dns.GetHostAddresses(addr).First(a => a.AddressFamily == AddressFamily.InterNetwork), port), protocol);
                connection.Connect(name, pass, new string[0], addr);

                t = new Thread(a => UpdateLoop(connection)) { IsBackground = true };
                t.Start();

                //When localuser is set it means we're really connected
                while (!protocol.ReceivedServerSync)
                {
                }
            }
            catch (Exception e)
            {
                string[] lines = { "Error", e.Message };
                System.IO.File.WriteAllLines(@"C:\Users\Christian\Desktop\WriteLines.txt", lines);
            }
            
            Channel c = null;

            foreach (var channel in protocol.Channels.ToArray())
            {
                if (channel.Name == "Illuminati")
                {
                    c = channel;
                    break;
                }
            }

            if (c != null)
                protocol.LocalUser.Channel = c;
            

            textValues.Add("VolumeAmount", "");
            textValues.Add("AudioURL", "");
        }

        public static void VA_Exit1(ref Dictionary<string, object> state)
        {
            t.Abort();
            connection.Close();
        }

        public static void VA_Invoke1(String context, ref Dictionary<string, object> state, ref Dictionary<string, Int16?> shortIntValues, ref Dictionary<string, string> textValues, ref Dictionary<string, int?> intValues, ref Dictionary<string, decimal?> decimalValues, ref Dictionary<string, Boolean?> booleanValues, ref Dictionary<string, object> extendedValues)
        {
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
            FileInfo serverConfigFile = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\VoiceAttack\\Apps\\Mumble\\server.txt");
            if (serverConfigFile.Exists)
            {
                using (StreamReader reader = new StreamReader(serverConfigFile.OpenRead()))
                {
                    addr = reader.ReadLine();
                    port = int.Parse(reader.ReadLine());
                    name = reader.ReadLine();
                    pass = reader.ReadLine();
                }
            }
            else
            {
                addr = "idle.negroserver.com";
                port = 64738;
                name = ".AI";
                pass = "";
            }
        }
    }
}