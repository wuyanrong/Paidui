using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Evt.Framework.Common
{
    /// <summary>
    /// 消息异常
    /// </summary>
    public class MessageException : Exception
    {
        private string _code = null;
        private string _detail = null;
        private bool _handled = false;

        /// <summary>
        /// 
        /// </summary>
        public string Code
        {
            get { return _code; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Detail
        {
            get { return _detail; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Handled
        {
            get { return _handled; }
            set { _handled = value; }
        }

        /// <summary>
        /// 初始化 System.Exception 类的新实例。
        /// </summary>
        public MessageException()
            : base()
        {
        }

        /// <summary>
        /// 使用指定的错误信息初始化 System.Exception 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的基本信息。</param>
        public MessageException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 使用指定的错误信息初始化 System.Exception 类的新实例。
        /// </summary>
        /// <param name="code">描述错误的编号。</param>
        /// <param name="message">描述错误的基本信息。</param>
        public MessageException(string code, string message)
            : base(message)
        {
            _code = code;
        }

        /// <summary>
        /// 使用指定的错误信息初始化 System.Exception 类的新实例。
        /// </summary>
        /// <param name="code">描述错误的编号。</param>
        /// <param name="message">描述错误的基本信息。</param>
        /// <param name="detail">描述错误的详细消息。</param>
        public MessageException(string code, string message, string detail)
            : base(message)
        {
            _code = code;
            _detail = detail;
        }

        /// <summary>
        /// 使用指定错误信息和对作为此异常原因的内部异常的引用来初始化 System.Exception 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的基本信息。</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个空引用</param>
        public MessageException(string message, string detail, Exception innerException)
            : base(message, innerException)
        {
            _detail = detail;
        }

        /// <summary>
        /// 用序列化数据初始化 System.Exception 类的新实例。
        /// </summary>
        /// <param name="info">它包含有关源或目标的上下文信息。</param>
        /// <param name="context">它存有有关所引发异常的序列化的对象数据。</param>
        protected MessageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
