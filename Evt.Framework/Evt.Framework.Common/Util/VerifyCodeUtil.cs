//版权信息：版权所有(C) 2012，PAIDUI.CN
//变更历史：
//    姓名         日期          说明
//--------------------------------------------------------
//   技术部     2012/11/16       创建

using System;
using System.Drawing;
using System.IO;

namespace Evt.Framework.Common
{
    /// <summary>
    /// 验证码生成工具类
    /// </summary>
    public class VerifyCodeUtil
    {
        /// <summary>
        /// 产生纯数字随机字符串
        /// </summary>
        /// <param name="num">随机出几个字符</param>
        /// <returns>随机出的字符串</returns>
        public static string GenNumCode(int num)
        {
            string[] source = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string numCode = string.Empty;
            Random rd = new Random();
            int i;
            for (i = 0; i < num; i++)
            {
                numCode += source[rd.Next(0, source.Length)];
            }
            return numCode;
        }

        /// <summary>
        /// 产生随机字符串
        /// </summary>
        /// <param name="num">随机出几个字符</param>
        /// <returns>随机出的字符串</returns>
        public static string GenCode(int num)
        {
            string[] source = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string code = string.Empty;
            Random rd = new Random();
            int i;
            for (i = 0; i < num; i++)
            {
                code += source[rd.Next(0, source.Length)];
            }
            return code;
        }

        /// <summary>
        /// 生成图片（增加背景噪音线、前景噪音点）
        /// </summary>
        /// <param name="checkCode">随机出字符串</param>
        /// <returns>二进制流</returns>
        public static MemoryStream CreateCheckCodeImage(string checkCode)
        {
            //将字符串保存到Session中，以便需要时进行验证
            Bitmap image = new Bitmap((int)(checkCode.Length * 21.5), 22);
            Graphics g = Graphics.FromImage(image);
            System.IO.MemoryStream ms = null;
            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(Color.White);

                // 画图片的背景噪音线
                int i;
                for (i = 0; i < 10; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new Font("Arial", 14, FontStyle.Bold);
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2F, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            }
            catch
            {
                g.Dispose();
                image.Dispose();
            }
            return ms;
        }
    }
}
