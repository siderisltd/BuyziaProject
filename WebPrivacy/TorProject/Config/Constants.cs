namespace TorProject.Config
{
    using System;

    internal class Constants
    {
        internal const int TOR_IDENTITY_PORT = 9051;

        internal const int NETWORK_PROXY_SOCKS_PORT = 9050;

        internal const string TOR_CHANGE_IDENTITY_COMMAND = "SIGNAL NEWNYM\r\n";

        internal const string TOR_OK_CODE = "250";

        internal const string TOR_AUTH_COMMAND = "AUTHENTICATE \"butt\"\n";

        internal const int REQUEST_TIMEOUT_MS = 15000;

        internal const string PRIVOXY_WEB_PROXY = "127.0.0.1:8118";

        internal const int WEB_DRIVER_WAIT_SECONDS = 30;

        internal const string LOCALHOST = "127.0.0.1";

        internal const string TOR_PROCESS_NAME = "tor";

        internal const string TOR_PHYSICAL_PATH = @"C:\Users\Alex\Desktop\Tor Browser\Browser\TorBrowser\Tor\tor.exe";

        internal const string TOR_CONTROL_PORT_CONFIGURATION = "ControlPort 9051 CircuitBuildTimeout 1";

        internal const string PRIVOXY_PROCESS_NAME = "privoxy";

        internal const string PRIVOXY_PHYSICAL_PATH = @"C:\Users\Alex\Desktop\BuyziaProjectRepo\privoxy\privoxy.exe";

        internal const string PRIVOXY_WORKING_DIRECTORY = @"C:\Users\Alex\Desktop\BuyziaProjectRepo\privoxy\";
    }
}
