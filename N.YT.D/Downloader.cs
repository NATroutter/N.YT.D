using Pastel;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;
using YoutubeDLSharp.Options;

namespace N.YT.D {
    class Downloader {
        private readonly Color baseColor, highColor;

        private readonly Tests tests = new Tests();
        private readonly Utils utils = new Utils();

        public Downloader(Color baseColor, Color highColor) {
            this.baseColor = baseColor;
            this.highColor = highColor;
        }

        public async Task download(string link, Format form, string output, bool uselast) {
            var ytdl = new YoutubeDL();

            Console.Clear();
            Program.banner();
            Console.WriteLine(" ");
            Console.WriteLine("Loading data... ".Pastel(baseColor));

            var resp = await ytdl.RunVideoDataFetch(link);

            Console.Clear();
            Program.banner();
            Console.WriteLine(" ");
            Console.WriteLine("Output Folder: ".Pastel(baseColor) + output.Pastel(highColor));
            Console.WriteLine("Video link: ".Pastel(baseColor) + link.Pastel(highColor));
            Console.WriteLine("Format: ".Pastel(baseColor) + form.ToString().Pastel(highColor));
            Console.WriteLine(" ");

            VideoData data = resp.Data;
            if (data.Title != null && !String.IsNullOrEmpty(data.Title)) {
                Console.WriteLine("Title: ".Pastel(baseColor) + data.Title.Pastel(highColor));
            } else if (data.AltTitle != null && !String.IsNullOrEmpty(data.AltTitle)) {
                Console.WriteLine("Title(Alt): ".Pastel(baseColor) + data.AltTitle.Pastel(highColor));
            }
            if (data.Uploader != null && !String.IsNullOrEmpty(data.Uploader)) {
                Console.WriteLine("Uploader: ".Pastel(baseColor) + data.Uploader.Pastel(highColor));
            }
            if (data.UploadDate != null && !String.IsNullOrEmpty(data.UploadDate.ToString())) {
                Console.WriteLine("Date: ".Pastel(baseColor) + utils.DateFormat(data.UploadDate).Pastel(highColor));
            }
            if (data.ViewCount != null && !String.IsNullOrEmpty(data.ViewCount.ToString())) {
                Console.WriteLine("Views: ".Pastel(baseColor) + utils.numFormat(data.ViewCount).Pastel(highColor));
            }

            Console.WriteLine(" ");
            if (uselast) {
                await startExtract(ytdl, link, form, output, uselast);
                await Program.Work();
            } else {
                Console.Write($"Start Download (y/n): ".Pastel(baseColor));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Answear answear = tests.IsValidAnswear(Console.ReadLine());
                if (answear == Answear.YES) {
                    await startExtract(ytdl, link, form, output, uselast);
                    await Program.Work();
                } else {
                    await Program.Work();
                }
            }
        }

        private async Task startExtract(YoutubeDL ytdl, string link, Format form, string output, bool uselast) {
            Console.Write("Downloading... ".Pastel(baseColor));

            await extract(ytdl, link, form, output);

            if (!uselast) {
                Console.WriteLine(" ");
            }
            Console.WriteLine("Press any key to continue...".Pastel(baseColor));
            Console.ReadKey();
        }

        private async Task extract(YoutubeDL ytdl, string link, Format form, string output) {
            string ytdl_file = AppDomain.CurrentDomain.BaseDirectory + "youtube-dl.exe";
            string ffmpeg_file = AppDomain.CurrentDomain.BaseDirectory + "ffmpeg.exe";

            if (!tests.TestFile(false)) {
                Console.ReadKey();
                return;
            }

            using (var bar = new ProgressBar(highColor)) {
                var pros = new Progress<DownloadProgress>(p => bar.Report(p.Progress));
                var cts = new CancellationTokenSource();

                ytdl.YoutubeDLPath = ytdl_file;
                ytdl.FFmpegPath = ffmpeg_file;
                ytdl.OutputFolder = output;

                if (form == Format.WAV) {
                    await ytdl.RunAudioDownload(link, AudioConversionFormat.Wav, ct: cts.Token, progress: pros);
                } else if (form == Format.MP3) {
                    await ytdl.RunAudioDownload(link, AudioConversionFormat.Mp3, ct: cts.Token, progress: pros);
                } else if (form == Format.OGG) {
                    await ytdl.RunAudioDownload(link, AudioConversionFormat.Vorbis, ct: cts.Token, progress: pros);
                } else if (form == Format.MP4) {
                    await ytdl.RunVideoDownload(link, recodeFormat: VideoRecodeFormat.Mp4, ct: cts.Token, progress: pros);
                } else if (form == Format.WEBM) {
                    await ytdl.RunVideoDownload(link, recodeFormat: VideoRecodeFormat.Webm, ct: cts.Token, progress: pros);
                } else {
                    Console.WriteLine("Invalid Format!".Pastel(baseColor));
                    Console.ReadKey();
                    await Program.Work();
                }
            }
            Console.WriteLine("Done.".Pastel(highColor));
        }

    }
}
