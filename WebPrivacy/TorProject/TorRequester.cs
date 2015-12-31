namespace TorProject
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;
    using AmazonParser.Models;
    using AmazonParser.SeleniumExtensions;
    using Config;
    using Helpers;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.Support.UI;

    public class TorRequester
    {
        public void BrowserRequest(string url)
        {
            var processesHelper = new ProcessesHelper();
            processesHelper.EnsurePrivoxyIsStarted();
            Process torProcess = processesHelper.CreateTorProcess();

            FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("network.proxy.type", 1);
            profile.SetPreference("network.proxy.socks", Constants.LOCALHOST);
            profile.SetPreference("network.proxy.socks_port", Constants.NETWORK_PROXY_SOCKS_PORT);

            ICustomWebDriver driver = new CustomFirefoxDriver(profile);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Constants.WEB_DRIVER_WAIT_SECONDS));

            ItemDetailsObject result = driver.ParseAmazonProduct(url, wait);
            //driver.Navigate().GoToUrl(url);
            ////switch to IFrameContent
            ////this.Driver.SwitchTo().Frame(this.Driver.FindElement(By.Id("product-description-iframe")));
            //var whatIsMyIpComExpression = By.XPath("//*[@id='section_left']/div[2]");
            //Wait.Until(x => x.FindElement(whatIsMyIpComExpression));
            //var element = driver.FindElement(whatIsMyIpComExpression);

            this.ChangeIdentity();
            driver.Quit();

            //For more requests with the same driver

            //this.Driver = new FirefoxDriver(profile);
            //this.Wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(30));
            //this.Driver.Navigate().GoToUrl(url);
            //this.Wait.Until(x => x.FindElement(expression));
            //element = this.Driver.FindElement(expression);
            //ChangeIdentity();
            //this.Driver.Quit();

            if (!torProcess.HasExited) { torProcess.Kill(); }
        }

        public string Get(string url)
        {
            var processesHelper = new ProcessesHelper();
            processesHelper.EnsurePrivoxyIsStarted();
            Process torProcess = processesHelper.CreateTorProcess();

            Regex ipRegex = new Regex("\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}", RegexOptions.Multiline);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/6.0;)";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:38.0) Gecko/20100101 Firefox/38.0";

            request.Accept = "text/html,application/xhtml+xml";
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en");
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            request.Proxy = new WebProxy(Constants.PRIVOXY_WEB_PROXY);
            request.KeepAlive = false;
            request.Timeout = Constants.REQUEST_TIMEOUT_MS;

            string content;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    content = reader.ReadToEnd();
                    var first = ipRegex.Match(content);
                    var ip = first.Groups[0].Value;

                    this.ChangeIdentity();
                }
            }

            if (!torProcess.HasExited) torProcess.Kill();

            return content;
        }

        //public void CheckIfBlocked(HttpWebResponse response, string ypURL)
        //{
        //    if (response.StatusCode == HttpStatusCode.Forbidden)
        //    {
        //        Console.WriteLine("Getting Blocked");
        //        this.RefreshTor();
        //        //make new request to the same url

        //        //check for forbidden if it is true you are blocked
        //    }
        //}

        private void ChangeIdentity()
        {
            //auth problem....
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(Constants.LOCALHOST), Constants.TOR_IDENTITY_PORT);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ip);
            }
            catch (SocketException e)
            {
                throw new ArgumentException("Cannot connect to server");
            }

            server.Send(Encoding.ASCII.GetBytes(Constants.TOR_AUTH_COMMAND));
            byte[] data = new byte[1024];
            int receivedDataLength = server.Receive(data);
            string stringData = Encoding.ASCII.GetString(data, 0, receivedDataLength);

            if (stringData.Contains(Constants.TOR_OK_CODE))
            {
                server.Send(Encoding.ASCII.GetBytes(Constants.TOR_CHANGE_IDENTITY_COMMAND));
                data = new byte[1024];
                receivedDataLength = server.Receive(data);
                stringData = Encoding.ASCII.GetString(data, 0, receivedDataLength);
                if (!stringData.Contains(Constants.TOR_OK_CODE))
                {
                    Console.WriteLine("Unable to signal new user to server.");
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                    throw new ArgumentException("Unable to change identity");
                }
            }
            else
            {
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                throw new ArgumentException("Unable to connect the server! Maybe authentication- see the last stringData");
            }
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }
    }
}
