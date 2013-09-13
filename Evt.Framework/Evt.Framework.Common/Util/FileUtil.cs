using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Evt.Framework.Common
{
    /// <summary>
    /// �ļ�������
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// ��ȡ�ļ����ֽ�������ʽ
        /// </summary>
        /// <param name="fileName">�ļ�</param>
        /// <returns>�ֽ�����</returns>
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
        /// �����ļ�������
        /// </summary>
        /// <param name="fileContent">�ļ�����</param>
        /// <param name="fileName">�ļ�����</param>
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