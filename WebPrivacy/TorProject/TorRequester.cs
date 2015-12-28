namespace TorProject
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    //TODO: CHECK AND FIX !!!
    public class TorRequester
    {
        public static void MakeRequest()
        {
            Process p = new Process();
            //p.StartInfo = new ProcessStartInfo(@"C:\Users\n.mhoumadi\Downloads\Tor Browser\App\tor.exe", "ControlPort 9051 HashedControlPassword 16:09E39A6695DD2EAE60AC6BE15E1E348B1042FE3A6E34137C7E2F0C83B9");
            p.StartInfo =
                new ProcessStartInfo(@"C:\Program Files (x86)\TorProject\Tor Browser\Browser\TorBrowser\Tor\tor.exe",
                    "ControlPort 9051 CircuitBuildTimeout 10");

            //p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();

            Thread.Sleep(5000);

            Regex regex = new Regex("\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}", RegexOptions.Multiline);


            HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://whatismyipaddress.com/");

            request.Proxy = new WebProxy("127.0.0.1:8118");
            request.KeepAlive = false;

            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            {

                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                {

                    //webBrowser1.DocumentText = reader.ReadToEnd();

                    //Regex regex = new Regex("value=\"([0-9]*)\\.([0-9]*)\\.([0-9]*)\\.([0-9]*)\"");

                    string contenu = reader.ReadToEnd();
                    Console.WriteLine(regex.Match(contenu).Groups[0].Value);
                    Console.WriteLine(reader.ToString());

                }

            }

            p.Kill();
            Console.ReadLine();
        }
    }
}
