using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace TcgSdk.System
{
    public class ApiCommunicator
    {
        public static string GetHttpResponseString(string url, string method)
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);

            webRequest.Method = method;

            using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }

                }
            }
        }
    }
}
