using System;
using System.Collections.Generic;
using MumblePlugin;

namespace MumblePluginTest
{
    public class PluginTest
    {
        public static void Main(string[] args)
        {
            string context = "";

            var state = new Dictionary<string, object>();
            var shortIntValues = new Dictionary<string, Int16?>();
            var textValues = new Dictionary<string, string>();
            var intValues = new Dictionary<string, int?>();
            var decimalValues = new Dictionary<string, decimal?>();
            var booleanValues = new Dictionary<string, Boolean?>();
            var extendedValues = new Dictionary<string, object>();

            VoiceAttackPlugin.VA_Init1(ref state, ref shortIntValues, ref textValues, ref intValues, ref decimalValues, ref booleanValues, ref extendedValues);

            Console.Write("Initialized");
            Console.ReadLine();

            Console.WriteLine("Sending a test message...");
            textValues["TextToSend"] = "This is a test message.";
            VoiceAttackPlugin.VA_Invoke1(context, ref state, ref shortIntValues, ref textValues, ref intValues, ref decimalValues, ref booleanValues, ref extendedValues);

            Console.Write("Invoked");
            Console.ReadLine();

            VoiceAttackPlugin.VA_Exit1(ref state);

            Console.Write("Exited");
            Console.ReadLine();
        }
    }
}
