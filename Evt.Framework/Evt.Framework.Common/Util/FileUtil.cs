using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Evt.Framework.Common
{
    /// <summary>
    /// 文件工具类
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// 获取文件的字节数组形式
        /// </summary>
        /// <param name="fileName">文件</param>
        /// <returns>字节数组</returns>
        public static byte[] GetFile(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] file;

            try
            {
                file = br.ReadBytes((int)fs.Length);
            }
            finally
            {
                br.Close();
                fs.Close();
                br = null;
                fs = null;
            }

            return file;
        }

        /// <summary>
        /// 保存文件到磁盘
        /// </summary>
        /// <param name="fileContent">文件内容</param>
        /// <param name="fileName">文件名称</param>
        public static void SeveFile(byte[] fileContent, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write);
            BinaryWriter br = new BinaryWriter(fs);

            try
            {
                br.Write(fileContent);
            }
            finally
            {
                br.Flush();
                br.Close();
                fs.Close();

                br = null;
                fs = null;
            }
        }
    }
}