//版权信息：版权所有(C) 2012，PAIDUI.CN
//变更历史：
//    姓名         日期          说明
//--------------------------------------------------------
//   技术部     2012/11/19       创建

using System;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Evt.Framework.Common
{
    /// <summary>
    /// SMS服务返回结果Model
    /// </summary>
    [Serializable]
    class ResponseModel
    {
        /// <summary>
        /// 返回状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string data { get; set; }
    }

    /// <summary>
    /// SMS服务工具类
    /// </summary>
    public class SMSUtil
    {
        /// <summary>
        /// 实例化
        /// </summary>
        private static SMSUtil _instance = new SMSUtil();

        /// <summary>
        /// 手机号表达式
        /// </summary>
        private static string MOBILE_PATTERN = @"^1[3,4,5,8]{1}[0-9]{1}[0-9]{8}$";

        /// <summary>
        /// 单例
        /// </summary>
        public static SMSUtil Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 发送短信操作方法、支持给一个手机发送短信
        /// </summary>
        /// <param name="mobile">手机号码,不能为空</param>
        /// <param name="content">短信内容，不能为空。长度不限，系统自动分截。</param>
        /// <param name="time">定时发送时间，可为空,为空表示即时发送,格式为2012-11-05 11:30:23</param>
        /// <param name="appsign">与服务端约定的项目标识，用于在服务端验证有效性，在服务器根据项目标映射成对应短信扩展号。</param>
        /// <returns>发送是否成功</returns>
        public bool SendSMS(string mobile, string content, string time, string appsign)
        {
            //检查参数
            if (mobile.Length != 11 || !Regex.IsMatch(mobile, MOBILE_PATTERN))
            {
                //throw new MessageException("待发送的手机号码格式不对。");
                return false;
            }

            if (string.IsNullOrEmpty(content))
            {
                //throw new MessageException("短信发送内容不能为空。");
                return false;
            }

            DateTime dt = ConvertUtil.ToDateTime(time);
            if (dt == DateTime.MinValue)
            {
                // throw new MessageException("短信定时发送时间不是有效时间值。");
                return false;
            }
            if (dt <= DateTime.Now)
            {
                // throw new MessageException("短信定时发送时间不能小于当前系统时间。");
                return false;
            }

            if (string.IsNullOrEmpty(appsign))
            {
                //throw new MessageException("项目标识不能为空。");
                return false;
            }

            //http发送sms
            string responseData = HttpRequest(mobile, content, time, appsign);
            ResponseModel resultModel = new JavaScriptSerializer().Deserialize<ResponseModel>(responseData);

            if (resultModel.status == 0)//处理失败
            {
                LogUtil.Warn(resultModel.data);
                //throw new Exception(resultModel.data);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 短信发送Http通道
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <param name="time"></param>
        /// <param name="appsign"></param>
        /// <returns></returns>
        private string HttpRequest(string mobile, string content, string time, string appsign)
        {
            string responseData = string.Empty;

            //Init http发送
            HttpWebRequest request = null;
            request = WebRequest.Create("http://" + ConfigUtil.SMSHttpUrl) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = string.Empty;
            request.Timeout = 20000;
            Encoding encoding = Encoding.GetEncoding("utf-8");

            StreamReader responseReader = null;
            Stream responseStream = null;

            //POST数据
            string buffer = string.Format("mobile={0}&content={1}&datetime={2}&appsign={3}", mobile, content, time, appsign);
            byte[] data = encoding.GetBytes(buffer);
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            try
            {
                responseStream = request.GetResponse().GetResponseStream();
                responseReader = new StreamReader(responseStream);
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
            }
            finally
            {
                if (responseStream != null)
                {
                    responseStream.Close();
                    responseStream = null;
                }
                responseReader.Close();
                responseReader = null;
                request = null;
            }

            return responseData;
        }
    }
}
