//版权信息：版权所有(C) 2012，PAIDUI.CN
//变更历史：
//    姓名         日期          说明
//--------------------------------------------------------
//   技术部     2012/11/16       创建

using System;
using System.Text;
using System.IO;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Evt.Framework.Common
{
    /// <summary>
    /// Security工具类，提高加密解密、
    /// </summary>
    public class SecurityUtil
    {
        #region 加密解密种子/密钥定义

        /// <summary>
        /// 加密Salt种子
        /// </summary>
        private static string ENCRYPT_SALT = "paidui888";

        /// <summary>
        /// DES加解密的密钥(必须为8位)
        /// </summary>
        private static string DES_KEY = "8ERDPDW1";

        #endregion

        #region MD5加密

        /// <summary>
        /// 获取MD5加密后Hash字符串
        /// </summary>
        /// <param name="strOriginal">待加密字符串</param>
        /// <returns>MD5加密后Hash字符串</returns>
        public static string GetMD5Hash(string strOriginal)
        {
            //MD5 md5Hash = MD5.Create();
            //byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(strOriginal));
            //StringBuilder sbList = new StringBuilder();
            //foreach (byte d in data)
            //{
            //    sbList.Append(d.ToString("x2")); //格式化十六进制显示格式，e.g.10和26显示为0x0A、0x1A
            //}
            //return sbList.ToString();

            return FormsAuthentication.HashPasswordForStoringInConfigFile(strOriginal, "MD5");
        }

        /// <summary>
        /// 获取MD5及Salt加密后Hash字符串
        /// </summary>
        /// <param name="strOriginal">待加密字符串</param>
        /// <param name="strSalt">Salte种子字符串</param>
        /// <returns>MD5加密后Hash字符串</returns>
        public static string GetMD5Hash(string strOriginal, string strSalt)
        {
            //如果调用未给Salt值,则默认
            if (string.IsNullOrEmpty(strSalt))
            {
                strSalt = ENCRYPT_SALT;
            }
            strOriginal = strOriginal + strSalt;

            return GetMD5Hash(strOriginal);
        }

        #endregion

        #region DES加密/解密

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString">待加密的明文</param>
        /// <returns>加密后的密文</returns>
        public static string DESEncrypt(string encryptString)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(encryptString);
            des.Key = ASCIIEncoding.ASCII.GetBytes(DES_KEY);
            des.IV = ASCIIEncoding.ASCII.GetBytes(DES_KEY);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString">待解密的密文</param>
        /// <returns>解密后的明文</returns>
        public static string DESDecrypt(string decryptString)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[decryptString.Length / 2];
            for (int x = 0; x < decryptString.Length / 2; x++)
            {
                int i = Convert.ToInt32(decryptString.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(DES_KEY);
            des.IV = ASCIIEncoding.ASCII.GetBytes(DES_KEY);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return Encoding.ASCII.GetString(ms.ToArray());
        }

        #endregion

        #region SHA1加密

        /// <summary>
        /// 使用 SHA1 对输入字符串进行加密
        /// </summary>
        /// <param name="inputString">待加密字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string ConvertToSHA1(string inputString)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(inputString, "SHA1");
        }

        #endregion
    }
}
