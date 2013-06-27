using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SMP.Client.Core
{
    public static class Utils
    {
        public static string GetIP()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            foreach (var item in addr)
            {
                if (item.ToString().Contains("."))
                {
                    return item.ToString();
                }
            }

            return addr[addr.Length - 1].ToString();
        }
    }
}
