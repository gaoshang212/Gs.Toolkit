using System;
using System.Net.NetworkInformation;

namespace Gs.Toolkit.Extension
{
    public class PingEx
    {
        public static PingReply GetResponseTime(string p_host)
        {
            using (Ping ping = new Ping())
            {
                try
                {
                    var uri = new Uri(p_host);
                    var rp = ping.Send(uri.Host);
                    return rp;
                }
                catch (Exception ex)
                {
                    //   
                }

                return null;
            }
        }
    }
}