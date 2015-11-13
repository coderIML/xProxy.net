using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace xProxy
{
    public class ADSL
    {
        private static void Connect(string connectionName, string user, string pass)
        {
            string arg = string.Format("rasdial \"{0}\" {1} {2}", connectionName, user, pass);
            InvokeCmd(arg);
        }

        private static void Disconnect(string connectionName)
        {
            string arg = string.Format("rasdial \"{0}\" /disconnect", connectionName);
            InvokeCmd(arg);
        }

        private static string InvokeCmd(string cmdArgs)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(cmdArgs);
            p.StandardInput.WriteLine("exit");

            return p.StandardOutput.ReadToEnd();
        }
        public static bool ReConnectNet(string connectionName, string user, string pass)
        {
            while (true)
            {
                Disconnect(connectionName);
                Thread.Sleep(4000);
                Connect(connectionName, user, pass);
                for (int i = 0; i < 4; i++)
                {
                    Thread.Sleep(3000);
                    if (Ping("pop.163.com"))
                    {
                        return true;
                    }
                }
                continue;
            }
        }

        public static bool Ping(string ip)
        {

            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();

            System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();

            options.DontFragment = true;

            string data = "Test Data!";

            byte[] buffer = Encoding.ASCII.GetBytes(data);

            int timeout = 1000; // Timeout 时间，单位：毫秒
            System.Net.NetworkInformation.PingReply reply = null;
            try
            {
                reply = p.Send(ip, timeout, buffer, options);
            }
            catch
            {
                return false;
            }
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)

                return true;

            else

                return false;

        }
    }
}
