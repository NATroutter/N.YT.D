using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace N.YT.D {
    class Updater {

        public async Task Update(string updateURL) {

            WebClient client = new WebClient();
            string CurrentLoc = AppDomain.CurrentDomain.BaseDirectory;

            Uri uri = new Uri(updateURL + "NYTDsetup.exe");
            await client.DownloadFileTaskAsync(uri, CurrentLoc + "NYTDsetup.exe");


            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = CurrentLoc + "NYTDsetup.exe";
            Process.Start(startInfo);
            Environment.Exit(0);
        }

        public async Task<bool> UpToDate(string updateURL) {

            WebClient client = new WebClient();
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo VersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            Uri uri = new Uri(updateURL + "version.txt");
            string NewVersion = await client.DownloadStringTaskAsync(uri);

            if (NewVersion.Contains(VersionInfo.ProductVersion)) {
                return true;
            }
            return false;
        }

    }
}
