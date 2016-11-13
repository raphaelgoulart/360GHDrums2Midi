using XInputDotNetPure;
using System;
using NAudio.Midi;
namespace GHDrums2Midi {
    class Program {
        public static bool red = false, yellow = false, blue = false, green = false, orange = false, kick = false, isConnected = false, isNotConnected = false;
        public static int boostRed = 20, boostYellow = 20, boostBlue = 20, boostGreen = 20, boostOrange = 20, boostKick = 20;
        public static int midiRed = 38, midiYellow = 46, midiBlue = 45, midiGreen = 43, midiOrange = 49, midiKick = 36;
        public static int outDevice = 0, channel = 10, index = 1;
        public static double degree = 1;
        public static void Main(string[] args) {
            ListMidiOutDevices();
            ReadSettings();
            if (channel < 1) channel = 1; if (channel > 16) channel = 16;
            MidiOut midiOut;
            try {
                midiOut = new MidiOut(outDevice);
            } catch {
                Console.WriteLine("unable to use specified midi out device - using the default one instead.");
                outDevice = 0;
                midiOut = new MidiOut(0);
            }
            Console.WriteLine("using midi channel " + channel + " and midi out device " + outDevice + " - " + MidiOut.DeviceInfo(outDevice).ProductName);
            var playerindex = PlayerIndex.One;
            switch (index) {
                case 1: break;
                case 2: playerindex = PlayerIndex.Two; break;
                case 3: playerindex = PlayerIndex.Three; break;
                case 4: playerindex = PlayerIndex.Four; break;
                default: Console.WriteLine("unable to read player index value from settings - using default (player one) instead."); break;
            }
            Console.WriteLine("reading input from player " + playerindex.ToString().ToLower());
            Console.WriteLine();
            while (true) {
                GamePadState state = GamePad.GetState(playerindex, GamePadDeadZone.None);
                if (!state.IsConnected && !isNotConnected) { isNotConnected = true; isConnected = false; Console.WriteLine("controller not connected!!"); }
                if (state.IsConnected && !isConnected) { isConnected = true; isNotConnected = false; Console.WriteLine("controller connected!!"); }
                if (state.Buttons.B == ButtonState.Released) red = false;
                if (state.Buttons.Y == ButtonState.Released) yellow = false;
                if (state.Buttons.X == ButtonState.Released) blue = false;
                if (state.Buttons.A == ButtonState.Released) green = false;
                if (state.Buttons.RightShoulder == ButtonState.Released) orange = false;
                if (state.Buttons.LeftShoulder == ButtonState.Released) kick = false;
                if (state.Buttons.B == ButtonState.Pressed && !red) {
                    red = true; var velocity = EaseOut(Math.Floor(state.ThumbSticks.Left.Y * 120 + 15), degree) + boostRed;
                    if (velocity > 127 || velocity < 0) velocity = 127;
                    Console.WriteLine("red pad hit! velocity: " + velocity);
                    SendNote(midiRed, velocity, midiOut, channel);
                }
                if (state.Buttons.Y == ButtonState.Pressed && !yellow) {
                    yellow = true; var velocity = EaseOut(Math.Floor(state.ThumbSticks.Right.X * 30000) + 18, degree) + boostYellow;
                    if (velocity > 127 || velocity < 0) velocity = 127;
                    Console.WriteLine("yellow cymbal hit! velocity: " + velocity);
                    SendNote(midiYellow, velocity, midiOut, channel);
                }
                if (state.Buttons.X == ButtonState.Pressed && !blue) {
                    blue = true; var velocity = EaseOut(Math.Floor(state.ThumbSticks.Right.X * 120 + 15), degree) + boostBlue;
                    if (velocity > 127 || velocity < 0) velocity = 127;
                    Console.WriteLine("blue pad hit! velocity: " + velocity);
                    SendNote(midiBlue, velocity, midiOut, channel);
                }
                if (state.Buttons.A == ButtonState.Pressed && !green) {
                    green = true; var velocity = EaseOut(Math.Floor(state.ThumbSticks.Left.Y * 30000) + 16, degree) + boostGreen;
                    if (velocity > 127 || velocity < 0) velocity = 127;
                    Console.WriteLine("green pad hit! velocity: " + velocity);
                    SendNote(midiGreen, velocity, midiOut, channel);
                }
                if (state.Buttons.RightShoulder == ButtonState.Pressed && !orange) {
                    orange = true; var velocity = EaseOut(Math.Floor(state.ThumbSticks.Right.Y * 30000) + 16, degree) + boostOrange;
                    if (velocity > 127 || velocity < 0) velocity = 127;
                    Console.WriteLine("orange cymbal hit! velocity: " + velocity);
                    SendNote(midiOrange, velocity, midiOut, channel);
                }
                if (state.Buttons.LeftShoulder == ButtonState.Pressed && !kick) {
                    kick = true; var velocity = EaseOut(Math.Floor(state.ThumbSticks.Right.Y * 120 + 15), degree) + boostKick;
                    if (velocity > 127 || velocity < 0) velocity = 127;
                    Console.WriteLine("kick pedal hit! velocity: " + velocity);
                    SendNote(midiKick, velocity, midiOut, channel);
                }
            }
        }

        public static int EaseOut(double velocity, double degree) {
            return (int)((1 - Math.Pow(1 - velocity / 127.0, degree)) * 127);
        }

        public static void ReadSettings() {
            if (System.IO.File.Exists("settings.ini")) {
                System.IO.StreamReader file = new System.IO.StreamReader("settings.ini");
                string line;
                while ((line = file.ReadLine()) != null) {
                    var data = line.Split('=');
                    int number;
                    switch (data[0].Trim().ToLower()) {
                        case "boostred":
                            if (Int32.TryParse(data[1].Trim(), out number)) boostRed = number;
                            else Console.WriteLine("unable to read boostRed value; using default value instead (20)");
                            break;
                        case "boostyellow":
                            if (Int32.TryParse(data[1].Trim(), out number)) boostYellow = number;
                            else Console.WriteLine("unable to read boostYellow value; using default value instead (20)");
                            break;
                        case "boostblue":
                            if (Int32.TryParse(data[1].Trim(), out number)) boostBlue = number;
                            else Console.WriteLine("unable to read boostBlue value; using default value instead (20)");
                            break;
                        case "boostgreen":
                            if (Int32.TryParse(data[1].Trim(), out number)) boostGreen = number;
                            else Console.WriteLine("unable to read boostGreen value; using default value instead (20)");
                            break;
                        case "boostorange":
                            if (Int32.TryParse(data[1].Trim(), out number)) boostOrange = number;
                            else Console.WriteLine("unable to read boostOrange value; using default value instead (20)");
                            break;
                        case "boostkick":
                            if (Int32.TryParse(data[1].Trim(), out number)) boostKick = number;
                            else Console.WriteLine("unable to read boostKick value; using default value instead (20)");
                            break;
                        case "midired":
                            if (Int32.TryParse(data[1].Trim(), out number)) midiRed = number;
                            else Console.WriteLine("unable to read midiRed value; using default value instead (38)");
                            break;
                        case "midiyellow":
                            if (Int32.TryParse(data[1].Trim(), out number)) midiYellow = number;
                            else Console.WriteLine("unable to read midiYellow value; using default value instead (46)");
                            break;
                        case "midiblue":
                            if (Int32.TryParse(data[1].Trim(), out number)) midiBlue = number;
                            else Console.WriteLine("unable to read midiBlue value; using default value instead (45)");
                            break;
                        case "midigreen":
                            if (Int32.TryParse(data[1].Trim(), out number)) midiGreen = number;
                            else Console.WriteLine("unable to read midiGreen value; using default value instead (43)");
                            break;
                        case "midiorange":
                            if (Int32.TryParse(data[1].Trim(), out number)) midiOrange = number;
                            else Console.WriteLine("unable to read midiOrange value; using default value instead (49)");
                            break;
                        case "midikick":
                            if (Int32.TryParse(data[1].Trim(), out number)) midiKick = number;
                            else Console.WriteLine("unable to read midiKick value; using default value instead (36)");
                            break;
                        case "channel":
                            if (Int32.TryParse(data[1].Trim(), out number)) channel = number;
                            else Console.WriteLine("unable to read channel value; using default value instead (1)");
                            break;
                        case "outdevice":
                            if (Int32.TryParse(data[1].Trim(), out number)) outDevice = number;
                            else Console.WriteLine("unable to read outDevice value; using default value instead (0)");
                            break;
                        case "degree":
                            if (Int32.TryParse(data[1].Trim(), out number)) degree = number;
                            else Console.WriteLine("unable to read degree value; using default value instead (1)");
                            break;
                        case "index":
                            if (Int32.TryParse(data[1].Trim(), out number)) index = number;
                            else Console.WriteLine("unable to read player index value; using default value instead (1)");
                            break;
                        default: break;
                    }
                }
                file.Close();
            } else {
                Console.WriteLine("settings.ini file not found - using default settings.");
            }
        }

        public static void ListMidiOutDevices() {
            Console.WriteLine("listing midi out devices:");
            for (int i = 0; i < MidiOut.NumberOfDevices; i++) {
                Console.WriteLine("\tdevice " + i + " - " + MidiOut.DeviceInfo(i).ProductName);
            }
            Console.WriteLine();
        }

        public static void SendNote(int note, int velocity, MidiOut midiOut, int channel) {
            midiOut.Send(MidiMessage.StartNote(note, velocity, channel).RawData);
            midiOut.Send(MidiMessage.StopNote(note, velocity, channel).RawData);
        }
    }
}