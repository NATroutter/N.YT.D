using Pastel;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace N.YT.D {
    class Program {

        public static string version;
        public static string UpdateURL = "http://natroutter.net/projects/N.YT.D/update/";
        public static Color baseColor = Color.FromArgb(231, 63, 52);
        public static Color highColor = Color.FromArgb(231, 159, 52);

        public static Tests tests = new Tests();
        public static Utils util = new Utils();
        public static Updater updater = new Updater();
        public static Downloader downloader = new Downloader(baseColor, highColor);


        static async Task Main(string[] args) {
            string Dir = AppDomain.CurrentDomain.BaseDirectory;
            string installer = Dir + "/NYTDsetup.exe";

            if (File.Exists(installer)) {
                File.Delete(installer);
            }

            if (!await updater.UpToDate(UpdateURL)) {
                DialogResult result = MessageBox.Show("New update aviable!\nDo you want to update now?", "N.YT.D", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes) {
                    Console.WriteLine("Updating...".Pastel(baseColor));
                    await updater.Update(UpdateURL);
                }
                Environment.Exit(0);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            version = fvi.ProductVersion.Remove(fvi.ProductVersion.Length - 4);

            Console.Title = "N.YT.D - NATroutter's Youtube Downloader - Version: " + version;

            await Work();

        }


        public static async Task Work() {
            Console.Clear();
            banner();

            if (!tests.TestFile(false)) {
                Console.ReadKey();
                return;
            }

            Console.Write("Youtube Link: ".Pastel(baseColor));
            Console.ForegroundColor = ConsoleColor.Yellow;
            string link = Console.ReadLine();
            if (!tests.IsValidLink(link)) {
                await invalidInput();
                return;
            }


            if (tests.OldSettingsPresent()) {
                Console.WriteLine(" ");
                Console.Write("Use Last Settings (y/n): ".Pastel(baseColor));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Answear answear = tests.IsValidAnswear(Console.ReadLine());
                if (answear == Answear.YES) {
                    Format fo = util.GetFormat(Properties.Settings.Default.format);
                    await downloader.download(link, fo, Properties.Settings.Default.output, true);
                    return;
                } else if (answear == Answear.INVALID) {
                    await invalidInput();
                }
            }


            Console.WriteLine(" ");
            Console.Write("Select Format (".Pastel(baseColor) + "wav".Pastel(highColor) + "/".Pastel(baseColor) + "ogg".Pastel(highColor) + "/".Pastel(baseColor) + "mp3".Pastel(highColor) + "/".Pastel(baseColor) + "mp4".Pastel(highColor) + "/".Pastel(baseColor) + "webm".Pastel(highColor) + "): ".Pastel(baseColor));
            Console.ForegroundColor = ConsoleColor.Yellow;
            string format = Console.ReadLine();
            Format form = tests.IsValidFormat(format);
            if (form != Format.INVALID) {
                Properties.Settings.Default.format = form.ToString();
                Properties.Settings.Default.Save();
            } else {
                await invalidInput();
            }


            Console.WriteLine(" ");
            Console.Write("Output Folder: ".Pastel(baseColor));
            Console.ForegroundColor = ConsoleColor.Yellow;
            string output = Console.ReadLine();
            bool oput = tests.IsValidDir(output);
            if (oput) {
                Properties.Settings.Default.output = output;
                Properties.Settings.Default.Save();
            } else {
                await invalidInput();
            }

            await downloader.download(link, form, output, false);
            return;
        }


        public static async Task invalidInput() {
            Console.Clear();
            banner();
            Console.WriteLine("Invalid input!".Pastel(baseColor));
            Console.WriteLine("Press any key to continie...".Pastel(baseColor));
            Console.ReadKey();
            await Work();
        }


        public static void banner() {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{@"__/\\\\\_____/\\\________/\\\________/\\\__/\\\\\\\\\\\\\\\________/\\\\\\\\\\\\____".Pastel(baseColor)}");
            Console.WriteLine($"{@" _\/\\\\\\___\/\\\_______\///\\\____/\\\/__\///////\\\/////________\/\\\////////\\\__".Pastel(baseColor)}");
            Console.WriteLine($"{@"  _\/\\\/\\\__\/\\\_________\///\\\/\\\/__________\/\\\_____________\/\\\______\//\\\_".Pastel(baseColor)}");
            Console.WriteLine($"{@"   _\/\\\//\\\_\/\\\___________\///\\\/____________\/\\\_____________\/\\\_______\/\\\_".Pastel(baseColor)}");
            Console.WriteLine($"{@"    _\/\\\\//\\\\/\\\_____________\/\\\_____________\/\\\_____________\/\\\_______\/\\\_".Pastel(baseColor)}");
            Console.WriteLine($"{@"     _\/\\\_\//\\\/\\\_____________\/\\\_____________\/\\\_____________\/\\\_______\/\\\_".Pastel(baseColor)}");
            Console.WriteLine($"{@"      _\/\\\__\//\\\\\\_____________\/\\\_____________\/\\\_____________\/\\\_______/\\\__".Pastel(baseColor)}");
            Console.WriteLine($"{@"       _\/\\\___\//\\\\\__/\\\_______\/\\\_____________\/\\\________/\\\_\/\\\\\\\\\\\\/___".Pastel(baseColor)}");
            Console.WriteLine($"{@"        _\///_____\/////__\///________\///______________\///________\///__\////////////_____".Pastel(baseColor)}");
            Console.WriteLine($"{@"                                   NATroutter's YouTube Downloader".Pastel(baseColor)}");
            Console.WriteLine(" ");
            Console.WriteLine($"                                     {"Made with <3 by: ".Pastel(baseColor)}{"NATroutter".Pastel(highColor)}");
            Console.WriteLine($"                                   {"Website: ".Pastel(baseColor)}{"https://NATroutter.net".Pastel(highColor)}");
            Console.WriteLine($"                                             {"Version: ".Pastel(baseColor)}{version.Pastel(highColor)}");
            Console.WriteLine(" ");
        }

    }
}
