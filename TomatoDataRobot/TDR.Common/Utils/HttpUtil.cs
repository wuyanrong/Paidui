using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace TDR.Common.Utils
{
    public class HttpUtil
    {
        /// <summary>
        /// Post数据到URI
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="header"></param>
        /// <param name="bodyParam"></param>
        /// <returns></returns>
        public static string Post(string uri, WebHeaderCollection header, Dictionary<string, string> bodyParam)
        {
            string param = string.Empty;
            foreach (var item in bodyParam)
            {
                param += item.Key + "=" + item.Value + "&";
            }
            param = param.Remove(param.Length - 1); //删除最后一个多余的&符号
            byte[] para = Encoding.UTF8.GetBytes(param);
            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Headers = header;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = para.Length;
            using (System.IO.Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(para, 0, para.Length);
            }
            string result = string.Empty;
            using (WebResponse response = request.GetResponse())
            {
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// 发起Get的请求，参数拼接在URL上
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = 5000;
            request.ServicePoint.ConnectionLeaseTimeout = 5000;
            request.ServicePoint.MaxIdleTime = 5000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string result = string.Empty;
            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader readStream = new StreamReader(resStream, Encoding.UTF8);
                result = readStream.ReadToEnd();
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }
    }
}
