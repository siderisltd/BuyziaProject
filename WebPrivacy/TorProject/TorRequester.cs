namespace TorProject
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Ports;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Microsoft.Win32;

    //TODO: CHECK AND FIX !!!

    public class TorRequester
    {

        string lastHtml = string.Empty;

        //private void Exec(string url)
        //{
        //    string html = string.Empty;


        //    var th = new Thread(() =>
        //    {
        //            WebBrowser wb = new WebBrowser();
        //            wb.DocumentCompleted += (sndr, e) =>
        //            {
        //                var documentElement = wb.Document.GetElementsByTagName("html")[0];

        //                if (html == documentElement.OuterHtml)
        //                {
        //                    lastHtml = documentElement.OuterHtml;
        //                    Application.ExitThread();
        //                }

        //                html = documentElement.OuterHtml;

        //                wb.Navigate(url);
        //                Thread.Sleep(1000);
        //            };
        //            wb.Navigate(url);
        //    });
        //    th.SetApartmentState(ApartmentState.STA);
        //    th.Start();
        //    th.Join();
        //}


        //public async Task<string> MakeDynamicRequest(string url)
        //{
        //    var cts = new CancellationTokenSource(10000); // cancel in 10s
        //    //var html = await this.LoadDynamicPage(url, cts.Token);
        //    return Exec(url);
        //}

        //public Task<T> StartSTATask<T>(Func<T> func)
        //{
        //    var tcs = new TaskCompletionSource<T>();
        //    Thread thread = new Thread(() =>
        //    {
        //        try
        //        {
        //            tcs.SetResult(func());
        //        }
        //        catch (Exception e)
        //        {
        //            tcs.SetException(e);
        //        }
        //    });
        //    thread.SetApartmentState(ApartmentState.STA);
        //    thread.Start();
        //    return tcs.Task;
        //}

        //private async Task<string> LoadDynamicPage(string url, CancellationToken token)
        //{
        //    this.StartSTATask<WebBrowser>(yx => new WebBrowser());
        //    var res = await Task.Run(() => new WebBrowser());
        //    var webBrowser = res;

        //    // navigate and await DocumentCompleted
        //    var tcs = new TaskCompletionSource<bool>();
        //    WebBrowserDocumentCompletedEventHandler handler = (s, arg) =>
        //                                                      {
        //                                                          tcs.TrySetResult(true);
        //                                                      };

        //    using (token.Register(() => tcs.TrySetCanceled(), useSynchronizationContext: true))
        //    {
        //        webBrowser.DocumentCompleted += handler;
        //        try
        //        {
        //            webBrowser.Navigate(url);
        //            await tcs.Task; // wait for DocumentCompleted
        //        }
        //        finally
        //        {
        //            webBrowser.DocumentCompleted -= handler;
        //        }
        //    }

        //    // get the root element

        //    var documentElement = webBrowser.Document.GetElementsByTagName("html")[0];
        //    // poll the current HTML for changes asynchronosly
        //    var html = documentElement.OuterHtml;
        //    while (true)
        //    {
        //        // wait asynchronously, this will throw if cancellation requested
        //        await Task.Delay(500, token);

        //        // continue polling if the WebBrowser is still busy
        //        if (webBrowser.IsBusy)
        //            continue;

        //        var htmlNow = documentElement.OuterHtml;
        //        if (html == htmlNow)
        //            break; // no changes detected, end the poll loop

        //        html = htmlNow;
        //    }

        //    // consider the page fully rendered 
        //    token.ThrowIfCancellationRequested();
        //    return html;
        //}

        //public async Task<string> MakeCompleteRequest(string url)
        //{
        //    var resp = await MakeDynamicRequest(url);

        //    return resp;
        //}

        public string MakeRequest(string url)
        {
            //TODO: ADD PRIVOXY'S PROCESS ALSO

            string content;

            Process p = new Process();
            //p.StartInfo = new ProcessStartInfo(@"C:\Users\n.mhoumadi\Downloads\Tor Browser\App\tor.exe", "ControlPort 9051 HashedControlPassword 16:09E39A6695DD2EAE60AC6BE15E1E348B1042FE3A6E34137C7E2F0C83B9");
            p.StartInfo = new ProcessStartInfo(@"C:\Users\Alex\Desktop\Tor Browser\Browser\TorBrowser\Tor\tor.exe", "ControlPort 9051 CircuitBuildTimeout 1");
            p.Start();

            Regex regex = new Regex("\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}", RegexOptions.Multiline);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/6.0;)";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:38.0) Gecko/20100101 Firefox/38.0";

            request.Accept = "text/html,application/xhtml+xml";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en");
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            //request.Proxy = new WebProxy("");
            request.KeepAlive = false;
            request.Timeout = 15000;

            //this.Exec("127.0.0.1:8118", url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    content = reader.ReadToEnd();
                    var first = regex.Match(content);
                    var ip = first.Groups[0].Value;
                    //Thread.Sleep(10000);

                    this.RefreshTor();
                }
            }

            if (!p.HasExited) p.Kill();

            return content;
        }

        //private void SetProxy(string Proxy)
        //{
        //    string key = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
        //    RegistryKey RegKey = Registry.CurrentUser.OpenSubKey(key, true);
        //    RegKey.SetValue("ProxyServer", Proxy);
        //    RegKey.SetValue("ProxyEnable", 1);
        //}

        public void CheckIfBlocked(HttpWebResponse response, string ypURL)
        {
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                Console.WriteLine("Getting Blocked");
                this.RefreshTor();
                //make new request to the same url

                //check for forbidden if it is true you are blocked
            }
        }

        public void RefreshTor()
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9051);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ip);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect to server.");
                RefreshTor();
                return;
            }

            server.Send(Encoding.ASCII.GetBytes("AUTHENTICATE \"butt\"\n"));
            byte[] data = new byte[1024];
            int receivedDataLength = server.Receive(data);
            string stringData = Encoding.ASCII.GetString(data, 0, receivedDataLength);

            if (stringData.Contains("250"))
            {
                server.Send(Encoding.ASCII.GetBytes("SIGNAL NEWNYM\r\n"));
                data = new byte[1024];
                receivedDataLength = server.Receive(data);
                stringData = Encoding.ASCII.GetString(data, 0, receivedDataLength);
                if (!stringData.Contains("250"))
                {
                    Console.WriteLine("Unable to signal new user to server.");
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                    RefreshTor();
                }
            }
            else
            {
                Console.WriteLine("Unable to authenticate to server.");
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                RefreshTor();
            }
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }
    }
}
