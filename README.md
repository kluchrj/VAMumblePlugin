# VAMumblePlugin
A Voice Attack plugin that connects to a mumble server using [MumbleSharp](https://github.com/martindevans/MumbleSharp).

Currently only allows voice attack to send chat messages.


## Installation

1. Download the .zip from the releases area.
2. Extract the contents, and place them in your voice attack folder. See the following structure:

  ```
  Program Files (x86)
  └── VoiceAttack
      └── Apps
          └── MumblePlugin
              ├── MumblePlugin.dll
              ├── MumbleSharp.dll
              ├── protobuf-net.dll
              └── server.ini
  ```
3. Configure `server.ini` to connect to your server.
4. Enable plugin support in VoiceAttack. You can find this under `Settings -> General -> Enable Plugin Support`.
5. Restart VoiceAttack.
6. Create a VoiceAttack profile.
7. Add a new command.
  * Click `Other -> Advanced -> Set a Text Value`.
  * Set the `Variable Name` to `TextToSend`.
  * Under `Set Text Value To`, click `Text` and input the message you want to send to the server.
  * Click `OK`.
  * Click `Other -> Advanced -> Execute an External Plugin Function`.
  * Change the dropdown to `Mumble Client Plugin`.
  * Under `Text Variables`, add `TextToSend`.
  * Hit `OK`, and configure the command to execute however you like.
8. Done!


## Troubleshooting

Errors are logged to `log.txt` in the plugin directory. Check this if VoiceAttack is crashing on startup or not connecting to your mumble server.
