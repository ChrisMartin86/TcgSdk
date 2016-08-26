using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TcgSdk.Common
{
    internal class ITcgCardRequest<T>
    {
        private string method = "GET";
        private IDictionary<string, string> dict = new Dictionary<string, string>();

        /// <summary>
        /// The URL of the api
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// The parameters to filter results by. Key should be parameter name, value should be parameter value.
        /// </summary>
        public IDictionary<string, string> Parameters { get { return dict; } set { dict = value; } }
        /// <summary>
        /// The method to use. Usually "GET"
        /// </summary>
        public string Method { get { return method; } set { method = value; } }
        /// <summary>
        /// Get the deserialized response from the API.
        /// </summary>
        /// <returns>An ITcgCardResponse containing the deserialized cards.</returns>
        public ITcgCardResponse<T> GetResponse()
        {
            try
            {
                return JsonConvert.DeserializeObject<ITcgCardResponse<T>>(getHttpResponseString(buildRequestUrl(), "GET"));
            }
            catch (Exception e)
            {
                throw new Exception("There was a problem deserializing the response.", e);
            }
        }
        /// <summary>
        /// Build the full request URL from the base url (URL) and the parameter dictionary (Parameters).
        /// </summary>
        /// <returns>string containing full URL for request.</returns>
        private string buildRequestUrl()
        {
            StringBuilder urlSb = new StringBuilder(Url);

            if (null != Parameters)
            {
                if (Url.Substring(Url.Length - 1, 1) != "?")
                {
                    urlSb.Append("?");
                }

                foreach (KeyValuePair<string, string> item in Parameters)
                {
                    urlSb.Append(string.Format("{0}={1}&", item.Key, item.Value));
                }

                urlSb.Remove(urlSb.Length - 1, 1);
            }

            return urlSb.ToString();
        }

        /// <summary>
        /// Get the JSON response string from the requested URL
        /// </summary>
        /// <param name="url">The URL to make the request to</param>
        /// <param name="method">The http method to use</param>
        /// <returns>JSON response</returns>
        private static string getHttpResponseString(string url, string method)
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
