using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Web;

using Evt.Framework.Common;

namespace Evt.Framework.Http
{
    public class HttpFile
    {
        private string fileName;
        private string fieldName;
        private byte[] data;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        public HttpFile(string fileName, string fieldName, bool isGzipCompress)
        {
            this.fileName = fileName;
            this.fieldName = fieldName;

            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    byte[] inBytes = new byte[stream.Length];
                    stream.Read(inBytes, 0, inBytes.Length);
                    //data = inBytes;
                    data = isGzipCompress ? CompressUtil.CompressByGzip(inBytes) : inBytes;
                }
            }
            catch (Exception ex)
            {
                throw new HttpClientException(ex.Message, string.Format("执行HttpFile({0}, {1}, {3})失败", fileName, fieldName, isGzipCompress), ex);
            }
        }

        public HttpFile(byte[] data, string fileName, string fieldName, bool isGzipCompress)
        {
            //this.data = data;
            this.data = isGzipCompress ? CompressUtil.CompressByGzip(data) : data;
            this.fileName = fileName;
            this.fieldName = fieldName;
        }
        
    }
}
