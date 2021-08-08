using Pastel;
using System;
using System.IO;
using System.Linq;

namespace N.YT.D {
    class Tests {

        public bool OldSettingsPresent() {
            if (IsValidString(Properties.Settings.Default.output, 5)) {
                return true;
            }
            return false;
        }

        public bool TestFile(bool silent) {
            string YTDL = AppDomain.CurrentDomain.BaseDirectory + "youtube-dl.exe";
            string FFMPEG = AppDomain.CurrentDomain.BaseDirectory + "ffmpeg.exe";
            bool status = true;
            if (!File.Exists(YTDL)) {
                if (!silent) {
                    Console.WriteLine(" ");
                    Console.WriteLine("Invalid installation!".Pastel(Program.baseColor));
                    Console.WriteLine("File doesn't exits: ".Pastel(Program.baseColor) + "youtube-dl.exe".Pastel(Program.highColor));
                }
                status = false;
            }
            if (!File.Exists(FFMPEG)) {
                if (!silent) {
                    Console.WriteLine(" ");
                    Console.WriteLine("Invalid installation!".Pastel(Program.baseColor));
                    Console.WriteLine("File doesn't exits: ".Pastel(Program.baseColor) + "ffmpeg.exe".Pastel(Program.highColor));
                }
                status = false;
            }
            return status;
        }

        public Format IsValidFormat(string str) {
            if (IsValidString(str, 3)) {
                if (Enumerable.Range(3, 4).Contains(str.Length)) {
                    switch (str.ToLower()) {
                        case "ogg": return Format.OGG;
                        case "mp3": return Format.MP3;
                        case "mp4": return Format.MP4;
                        case "webm": return Format.WEBM;
                    }
                }
            }
            return Format.INVALID;
        }

        public Answear IsValidAnswear(string str) {
            if (IsValidString(str, 1)) {
                if (str.Length == 1) {
                    if (str.ToLower() == "y") {
                        return Answear.YES;
                    } else if (str.ToLower() == "n") {
                        return Answear.NO;
                    }
                }
            }
            return Answear.INVALID;
        }

        public bool IsValidString(string str, int minLength) {
            if (!String.IsNullOrWhiteSpace(str) || !String.IsNullOrEmpty(str)) {
                if (str.Length >= minLength) {
                    return true;
                }
            }
            return false;
        }

        public bool IsValidDir(string str) {
            if (IsValidString(str, 3)) {
                if (str.Contains(@":\") || str.Contains(@":/")) {
                    return true;
                }
            }
            return false;

        }

        public bool IsValidLink(string link) {
            if (!String.IsNullOrWhiteSpace(link) || !String.IsNullOrEmpty(link)) {
                if (link.StartsWith("http://") || link.StartsWith("https://")) {
                    return true;
                }
            }
            return false;
        }

    }
}
