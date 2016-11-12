# 360GHDrums2Midi
This program reads input from Guitar Hero drumkits (Xbox 360 only) and sends midi notes to a specified midi output (including velocity). It can be used to transform your plastic drumkit into a midi drumkit!

This was inspired by PS360 Midi Drummer (link unavailable), but it didn't work for me at all - so, I ended up deciding to do my own program.

It should work with Rock Band drumkits, but for those, one should use [rb2midi](http://www.mattgrounds.com/rb2midi/), which is tailored for those (including cymbal and accurate sensitivity support).

It is **NOT** compatible with PS3 or Wii kits, since this program uses XInput - however, one can try using [x360ce](http://www.x360ce.com/) to emulate a Xbox controller. If the buttons and axis are configured correctly on it, it should work flawlessy (including sensitivity), but it is untested, so I cannot assure it'll actually work.
## Requirements
This program needs a few things to work correctly. Those are:
  * .NET Framework
  * MidiYoke
  * Any DAW (Digital Audio Workstation)
    * FL Studio, Ableton and similar software are examples of DAWs.
  * ASIO4ALL

The last one isn't obligatory, but it is strongly recommended if you want to use your drumkit without audio lag.
## Usage
This assumes you already have the requirements installed.
  * Open up 360GHDrums2Midi.exe;
  * It'll list the available Midi output devices - memorize one of the MidiYoke ones (we'll be using "device 1 - Out to MidiYoke 1");
  * Close down the program;
  * Open up the settings.ini file that comes with the package;
  * Find the "outDevice = 0" line and change its value to the number of the device - in our case, 1 (so it should look like "outDevice = 1");
    * There are other settings that can be customized in this file - they'll be explained thoroughly in the next section.
  * Save the file;
  * Turn on and plugin your drumkit, then open 360GHDrums2Midi.exe again;
  * It'll already be reading your drumkit data and sending it to Out to MidiYoke 1 - to test it, just hit some pads at random, and its color and velocity should be shown on the screen;
    * If it isn't, check if your drumkit isn't controller 1 among Xbox controllers - if it isn't, open up settings.ini again, and change the "index = 1" line to the actual player index of your controller (2, 3 or 4), then save it and close.
  * Open your prefered DAW software;
  * Find the "Midi input" setting on it, and choose In from MidiYoke 1 (if you're using Out to MidiYoke 1);
  * In the DAW, open your favorite drums instrument/VSTi;
  * Hit the pads on your drumkit! It should be working! However, with a bit of audio latency, which can be quite bothersome;
  * To fix that, find the "audio output" setting on your DAW, and set it to ASIO4ALL;
    * Note that, while ASIO4ALL fixs the audio latency, it also makes the audio output exclusive to the software that uses it - so the DAW will be the only program on your computer that will actually output any audio. If you want to hear audio from other programs while having the DAW open, do **not** use ASIO4ALL (however, you'll still have audio latency).
  * Hit the pads again! It should still be working, but this time without the audio lag!
## Settings
The program also offers some custom settings, which can be altered on the settings.ini file. Those are:
* boostRed
  * Midi velocity values range from 0 to 127. This setting will add the specified value to the velocity of the midi note, when the red pad is hit (but it will never go over the 127 limit).
  * Default value is 20.
* boostYellow
  * Same as boostRed, but for the yellow cymbal.
* boostBlue
  * Same as boostRed, but for the blue pad.
* boostGreen
  * Same as boostRed, but for the green pad.
* boostOrange
  * Same as boostRed, but for the orange cymbal.
* boostKick
  * Same as boostRed, but for the kick pedal.
* midiRed
  * It is the midi note that will be sent when the red pad is hit. Read the [General MIDI Percussion Key Map](http://computermusicresource.com/GM.Percussion.KeyMap.html) to find the specific midi note you want.
  * Default value is 38
* midiYellow
  * Same as midiRed, but for the yellow cymbal.
  * Default value is 46
* midiBlue
  * Same as midiRed, but for the blue pad.
  * Default value is 45
* midiGreen
  * Same as midiRed, but for the green pad.
  * Default value is 43
* midiOrange
  * Same as midiRed, but for the orange cymbal.
  * Default value is 49
* midiKick
  * Same as midiRed, but for the kick pedal.
  * Default value is 36
* channel
  * Refers to the Midi channel to which the note will be sent to. Minimum value is 1, maximum is 16. Channel 10 is used for drums.
  * Default value is 10
* outDevice
  * Refers to the Midi output device to which the note will be sent to. The 0 value should send the note to the standard Microsoft GS Wavetable Synth, while bigger values will send the note to other Midi output devices.
  * Default value is 0
  * If the specified number is invalid (not a valid Midi output device id), the default value will be used.
* degree
  * The Guitar Hero drumkit sends its velocity in a linear fashion. If this setting is set to 1, the velocity will be unaffected. If it is set to 2 or bigger, the velocity difference will be smoother on the higher values, and harsher on the lower values (similar behaviour to electronic drumkits).
  * Default value is 1
  * Suggested values are 2 or 3
* index
  * Refers to the player index of the Xbox controller. Since XInput supports up to 4 controllers, the valid values are 1, 2, 3 and 4.
  * Default value is 1
  * If the specified number is invalid (smaller than 1 or bigger than 4), the default value will be used.
## License
This software is released under the MIT license. You're free to use it and modify it as you will, as long as you read and agree with its license.

This program uses [XInputDotNet](https://github.com/speps/XInputDotNet), which is under the MIT license, and [NAudio](https://github.com/naudio/NAudio/), which is under the Ms-PL license.