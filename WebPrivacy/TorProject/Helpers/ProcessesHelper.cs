namespace TorProject.Helpers
{
    using System.Diagnostics;
    using System.Linq;
    using Config;

    internal class ProcessesHelper
    {
        internal Process CreateTorProcess()
        {
            Process torProcess = new Process();
            torProcess.StartInfo = new ProcessStartInfo(Constants.TOR_PHYSICAL_PATH, Constants.TOR_CONTROL_PORT_CONFIGURATION);
            torProcess.Start();

            return torProcess;
        }

        internal void EnsurePrivoxyIsStarted()
        {
            if (!ProcessesHelper.IsProcessOpen(Constants.PRIVOXY_PROCESS_NAME))
            {
                Process privoxyProcess = new Process();
                privoxyProcess.StartInfo = new ProcessStartInfo(Constants.PRIVOXY_PHYSICAL_PATH);
                privoxyProcess.StartInfo.WorkingDirectory = Constants.PRIVOXY_WORKING_DIRECTORY;
                privoxyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                privoxyProcess.Start();
            }
        }

        private static bool IsProcessOpen(string name)
        {
            Process[] pname = Process.GetProcessesByName(name);
            if (pname.Length != 0)
            {
                return true;
            }
            return false;
        }
    }
}
