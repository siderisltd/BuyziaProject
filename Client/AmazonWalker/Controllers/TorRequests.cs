namespace AmazonWalker.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.Support.UI;

    public class TorRequests
    {
        public IWebDriver Driver { get; set; }
        public Process TorProcess { get; set; }
        public WebDriverWait Wait { get; set; }

        public void StartTor(string url)
        {
            //String torBinaryPath = @"C:\Users\Alex\Desktop\Tor Browser\Browser\firefox.exe";
            //this.TorProcess = new Process();
            //this.TorProcess.StartInfo.FileName = torBinaryPath;
            //this.TorProcess.StartInfo.Arguments = "-n";
            //this.TorProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //this.TorProcess.Start();

            this.TorProcess= new Process();
            //p.StartInfo = new ProcessStartInfo(@"C:\Users\n.mhoumadi\Downloads\Tor Browser\App\tor.exe", "ControlPort 9051 HashedControlPassword 16:09E39A6695DD2EAE60AC6BE15E1E348B1042FE3A6E34137C7E2F0C83B9");
            this.TorProcess.StartInfo = new ProcessStartInfo(@"C:\Users\Alex\Desktop\Tor Browser\Browser\TorBrowser\Tor\tor.exe", "ControlPort 9051 CircuitBuildTimeout 1");
            this.TorProcess.Start();

            FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("network.proxy.type", 1);
            profile.SetPreference("network.proxy.socks", "127.0.0.1");
            profile.SetPreference("network.proxy.socks_port", 9050);
            this.Driver = new FirefoxDriver(profile);
            this.Wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(30));


            this.Driver.Navigate().GoToUrl(url);
            //this.Driver.SwitchTo().Frame(this.Driver.FindElement(By.Id("product-description-iframe")));
            var expression = By.XPath("//*[@id='section_left']/div[2]");
            this.Wait.Until(x => x.FindElement(expression));
            var element = this.Driver.FindElement(expression);

            ChangeIdentity();
            this.Driver.Quit();

            //remove
            this.Driver = new FirefoxDriver(profile);
            this.Wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(30));
            this.Driver.Navigate().GoToUrl(url);
            this.Wait.Until(x => x.FindElement(expression));
            element = this.Driver.FindElement(expression);
            ChangeIdentity();
            this.Driver.Quit();
            this.Driver = new FirefoxDriver(profile);
            this.Wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(30));
            this.Driver.Navigate().GoToUrl(url);
            this.Wait.Until(x => x.FindElement(expression));
            element = this.Driver.FindElement(expression);
            ChangeIdentity();
            this.Driver.Quit();
            this.Driver = new FirefoxDriver(profile);
            this.Wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(30));
            this.Driver.Navigate().GoToUrl(url);
            this.Wait.Until(x => x.FindElement(expression));
            element = this.Driver.FindElement(expression);
            ChangeIdentity();
            this.Driver.Quit();
            this.Driver = new FirefoxDriver(profile);
            this.Wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(30));
            this.Driver.Navigate().GoToUrl(url);
            this.Wait.Until(x => x.FindElement(expression));
            element = this.Driver.FindElement(expression);
            ChangeIdentity();
            //

            if (!this.TorProcess.HasExited)
            {
                this.TorProcess.Kill();
            }
        }

        private void ChangeIdentity()
        {
            //auth problem....
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9051);//9051 both
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ip);
            }
            catch (SocketException e)
            {
                throw new ArgumentException("Cannot connect to server");
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