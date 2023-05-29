using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AngelBroking
{
    public static class Helpers
    {
        public static byte[] DecodeBase64Byte(this string value)
        {
            var valueBytes = System.Convert.FromBase64String(value);
            return valueBytes;
        }

        public static string GetLocalIPAddress()
        {
            string ClientLocalIP = "";
            try
            {
                IPHostEntry Host = default(IPHostEntry);
                string Hostname = null;
                Hostname = System.Environment.MachineName;
                Host = Dns.GetHostEntry(Hostname);
                foreach (IPAddress IP in Host.AddressList)
                {
                    if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ClientLocalIP = Convert.ToString(IP);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ClientLocalIP;
        }
        public static string GetPublicIPAddress()
        {
            String address = "";
            try
            {
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    address = stream.ReadToEnd();
                }
                int first = address.IndexOf("Address: ") + 9;
                int last = address.LastIndexOf("</body>");
                address = address.Substring(first, last - first);
            }
            catch (Exception ex)
            {
            }
            return address;
        }
        public static string GetPublicIPAddress2()
        {
            String address = "";
            try
            {
                WebRequest request = WebRequest.Create("https://api.ipify.org/");
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    address = stream.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
            }
            return address;
        }
        public static PhysicalAddress GetMacAddress()
        {
            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // Only consider Ethernet network interfaces
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        nic.OperationalStatus == OperationalStatus.Up)
                    {
                        return nic.GetPhysicalAddress();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
