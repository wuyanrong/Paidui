//版权信息：版权所有(C) 2012，PAIDUI.CN
//变更历史：
//    姓名         日期          说明
//--------------------------------------------------------
//   技术部     2012/10/18       创建

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace Evt.Framework.Common
{
    /// <summary>
    /// 压缩和解压缩工具类
    /// 支持对字节流进行gzip 和 deflate两种方式的压缩和解压缩
    /// </summary>
    public class CompressUtil
    {
        /// <summary>
        /// 压缩二进制数据
        /// </summary>
        /// <param name="input">待压缩的数据</param>
        /// <returns>压缩后的数据</returns>
        public static byte[] CompressBinary(byte[] input)
        {
            Deflater compressor = new Deflater(Deflater.DEFAULT_COMPRESSION);

            //设置待压缩数据并开始压缩
            compressor.SetInput(input);
            compressor.Finish();

            using (MemoryStream ms = new MemoryStream(input.Length))
            {
                byte[] buffer = new byte[1024];

                //等待压缩完成
                while (!compressor.IsFinished)
                {
                    int count = compressor.Deflate(buffer);
                    ms.Write(buffer, 0, count);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// 解压缩二进制数据
        /// </summary>
        /// <param name="input">待解压缩的数据</param>
        /// <returns>压缩前的原数据</returns>
        public static byte[] DecompressBinary(byte[] input)
        {
            Inflater decompressor = new Inflater();
            decompressor.SetInput(input);

            using (MemoryStream ms = new MemoryStream(input.Length))
            {
                int bufferLength = 1024;
                if (input.Length < bufferLength)
                {
                    bufferLength = input.Length;
                }
                byte[] buffer = new byte[bufferLength];

                while (!decompressor.IsFinished)
                {
                    int count = decompressor.Inflate(buffer);
                    ms.Write(buffer, 0, count);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// 使用Zip方式压缩字符串
        /// </summary>
        /// <param name="str">待压缩字符串</param>
        /// <returns>压缩后的字符串</returns>
        public static string ZipCompressString(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
            zip.Write(buffer, 0, buffer.Length);
            zip.Close();
            ms.Position = 0;
            byte[] zipBuffer = new byte[ms.Length];
            ms.Read(zipBuffer, 0, zipBuffer.Length);
            ms.Close();
            string zipstr = Convert.ToBase64String(zipBuffer);
            return zipstr;
        }

        /// <summary>
        /// 使用Zip方式解压字符串
        /// </summary>
        /// <param name="str">压缩后字符串</param>
        /// <returns>原始字符串</returns>
        public static string ZipDeCompressString(string str)
        {
            byte[] buffer = Convert.FromBase64String(str);
            MemoryStream ms = new MemoryStream();
            ms.Write(buffer, 0, buffer.Length);
            ms.Position = 0;
            GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
            byte[] zipBuffer = new byte[1024];
            MemoryStream ms2 = new MemoryStream();
            while (true)
            {
                int bytesRead = zip.Read(zipBuffer, 0, zipBuffer.Length);
                if (bytesRead == 0)
                {
                    break;
                }
                ms2.Write(zipBuffer, 0, bytesRead);
            }
            zip.Close();
            string zipstr = Encoding.UTF8.GetString(ms2.ToArray(), 0, (int)ms2.Length);
            return zipstr;
        }

        /// <summary>
        /// 使用GZip方式解压字节流
        /// </summary>
        /// <param name="data">压缩后字节流</param>
        /// <returns>原始字节流</returns>
        public static byte[] DecompressByGzip(byte[] data)
        {
            using (MemoryStream streamOutput = new MemoryStream())
            {
                try
                {
                    using (GZipStream gZipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
                    {
                        byte[] readBuffer = new byte[1024];
                        int bytesRead;
                        while ((bytesRead = gZipStream.Read(readBuffer, 0, readBuffer.Length)) != 0)
                        {
                            streamOutput.Write(readBuffer, 0, bytesRead);
                        }
                        gZipStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new MessageException(String.Format("CompressUtil.DecompressGzip method exception，{0}", ex.Message));
                }

                return streamOutput.ToArray();
            }
        }

        /// <summary>
        /// 使用GZip方式压缩字节流
        /// </summary>
        /// <param name="data">原始字节流</param>
        /// <returns>压缩后字节流</returns>
        public static byte[] CompressByGzip(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress, true))
                {
                    byte[] compressedData = null;
                    try
                    {
                        gZipStream.Write(data, 0, data.Length);
                        gZipStream.Close();

                        stream.Position = 0;
                        compressedData = stream.ToArray();
                        stream.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new MessageException(String.Format("CompressUtil.CompressByGzip method exception，{0}", ex.Message));
                    }

                    return compressedData;
                }
            }
        }

        /// <summary>
        /// 使用Deflate方式解压字节流
        /// </summary>
        /// <param name="data">压缩后字节流</param>
        /// <returns>原始字节流</returns>
        public static byte[] DecompressByDeflate(byte[] data)
        {
            //Request.Headers["Content-Encoding"] = "gzip, deflate"

            using (MemoryStream streamInput = new MemoryStream())
            {
                byte[] decompressBuffer = null;
                try
                {
                    using (DeflateStream streamOutput = new DeflateStream(new MemoryStream(data), CompressionMode.Decompress, true))
                    {
                        streamOutput.Flush();
                        int nSize = 1024;
                        int offset = 0;
                        decompressBuffer = new byte[data.Length * 10]; //假设10倍的压缩率
                        while (true)
                        {
                            int bytesRead = streamOutput.Read(decompressBuffer, offset, nSize);
                            if (bytesRead == 0) break;
                            offset += bytesRead;
                        }  
                        streamOutput.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new MessageException(String.Format("CompressUtil.DecompressByDeflate method exception，{0}", ex.Message));
                }

                return decompressBuffer;
            }
        }

        /// <summary>
        /// 使用Deflate方式压缩字节流
        /// </summary>
        /// <param name="data">原始字节流</param>
        /// <returns>压缩后字节流</returns>
        public static byte[] CompressByDeflate(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Compress, true))
                {
                    byte[] compressedData = null;
                    try
                    {
                        deflateStream.Write(data, 0, data.Length);
                        deflateStream.Close();

                        stream.Position = 0;
                        compressedData = stream.ToArray();
                        stream.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new MessageException(String.Format("CompressUtil.CompressByDeflate method exception，{0}", ex.Message));
                    }

                    return compressedData;
                }
            }
        }   

    }
}
