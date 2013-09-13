using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Web;
using System.Collections.Specialized;

using Evt.Framework.Common;

namespace Evt.Framework.Http
{
    public class HttpRequest
    {
        private System.Web.HttpRequest request;

        public NameValueCollection Form
        {
            get { return this.request.Form; }
        }

        public NameValueCollection ServerVariables
        {
            get { return this.request.ServerVariables; }
        }

        public HttpFileCollection Files
        {
            get { return this.request.Files; }
        }

        /// <summary>
        /// 构造新的Evt.Framework.Http.HttpRequest实例
        /// </summary>
        /// <param name="request">HTTP请求对象</param>
        public HttpRequest(System.Web.HttpRequest req)
        {
            if (req != null)
            {
                this.request = req;
            }
        }

        /// <summary>
        /// 返回HTTP请求上传文件流
        /// </summary>
        /// <param name="fieldname">HTTP请求中文件的域名</param>
        /// <returns>HTTP请求内容流</returns>
        public Stream GetStream(string fieldname)
        {
            HttpPostedFile gzipdata = this.request.Files[fieldname]; //e.g. fieldname="gziplogdata"
            MemoryStream mstream = null;

            try
            {
                Stream stream = (gzipdata != null) ? gzipdata.InputStream : null;
                byte[] inBytes = new byte[stream.Length];
                stream.Read(inBytes, 0, inBytes.Length);
                stream.Close();

                byte[] data = inBytes;// CompressUtil.DecompressByGzip(inBytes);
                if (this.request.Form["isgzip"].ToString().Equals("1"))
                {
                    data = CompressUtil.DecompressByGzip(inBytes);
                }

                mstream = new MemoryStream();
                mstream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                throw new HttpClientException(ex.Message, "执行GetStream失败", ex);
            }

            return mstream;
        }

        /// <summary>
        /// 返回HTTP请求上传文件的字节数组
        /// </summary>
        /// <param name="fieldname">HTTP请求内容中上传文件的域名</param>
        /// <returns>HTTP请求内容的字节数组</returns>
        public byte[] GetBytes(string fieldname)
        {
            MemoryStream mstream = (MemoryStream)GetStream(fieldname);
            return (mstream != null) ? mstream.ToArray() : null;
        }

        /// <summary>
        /// 把HTTP请求上传文件保存到文件
        /// 如果指定的文件存在,它会被覆盖
        /// </summary>
        /// <param name="fieldname">HTTP请求内容中上传文件的域名</param>
        /// <param name="fileName">要保存的文件完整路径</param>
        /// <returns>是否向目标文件写入了数据</returns>
        public bool SaveAsFile(string fieldname, string fileName)
        {
            byte[] data = GetBytes(fieldname);
            bool result = false;

            try
            {
                using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)))
                    writer.Write(data);
                result = true;
            }
            catch (Exception ex)
            {
                throw new HttpClientException(ex.Message, "执行SaveAsFile失败", ex);
            }

            return result;
        }
    }
}
