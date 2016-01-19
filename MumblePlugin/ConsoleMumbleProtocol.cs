//Copyright(c) 2013 Martin Evans

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using MumbleProto;
using MumbleSharp;
using MumbleSharp.Audio;
using MumbleSharp.Audio.Codecs;
using MumbleSharp.Model;
using NAudio.Wave;

namespace MumblePlugin
{
    /// <summary>
    /// A test mumble protocol. Currently just prints the name of whoever is speaking, as well as printing messages it receives
    /// </summary>
    public class ConsoleMumbleProtocol
        : BasicMumbleProtocol
    {
        /*
        readonly Dictionary<User, AudioPlayer> _players = new Dictionary<User, AudioPlayer>();
        
        public override void EncodedVoice(byte[] data, uint userId, long sequence, IVoiceCodec codec, SpeechTarget target)
        {
            User user = Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
                Console.WriteLine(user.Name + " is speaking. Seq" + sequence);

            base.EncodedVoice(data, userId, sequence, codec, target);
        }
        */
        protected override void UserJoined(User user)
        {
            base.UserJoined(user);

            //_players.Add(user, new AudioPlayer(user.Voice));
        }

        protected override void UserLeft(User user)
        {
            base.UserLeft(user);

            //_players.Remove(user);
        }

        public override void ServerConfig(ServerConfig serverConfig)
        {
            base.ServerConfig(serverConfig);

            //Console.WriteLine(serverConfig.welcome_text);
        }

        protected override void ChannelMessageReceived(ChannelMessage message)
        {
            if (message.Channel.Equals(LocalUser.Channel))
                //Console.WriteLine(string.Format("{0} (channel message): {1}", message.Sender.Name, message.Text));

            base.ChannelMessageReceived(message);
        }

        protected override void PersonalMessageReceived(PersonalMessage message)
        {
            //Console.WriteLine(string.Format("{0} (personal message): {1}", message.Sender.Name, message.Text));

            base.PersonalMessageReceived(message);
        }
        /*
        private class AudioPlayer
        {
            private readonly WaveOut _playbackDevice = new WaveOut();

            public AudioPlayer(IWaveProvider provider)
            {
                _playbackDevice.Init(provider);
                _playbackDevice.Play();

                _playbackDevice.PlaybackStopped += (sender, args) => Console.WriteLine("Playback stopped: " + args.Exception);
            }
        }
        */
    }
}
