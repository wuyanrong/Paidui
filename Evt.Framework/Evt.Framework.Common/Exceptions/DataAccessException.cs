using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Evt.Framework.Common
{
	/// <summary>
	/// ���ݷ����쳣
	/// </summary>
	public class DataAccessException : Exception
	{
        private string _detail = null;

        /// <summary>
        /// �쳣����ϸ��Ϣ
        /// </summary>
        public string Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }

        /// <summary>
        /// ��ʼ�� System.Exception �����ʵ����
        /// </summary>
        public DataAccessException()
            : base()
        {
        }

        /// <summary>
        /// ʹ��ָ���Ĵ�����Ϣ��ʼ�� System.Exception �����ʵ����
        /// </summary>
        /// <param name="message">�����������Ϣ��</param>
        public DataAccessException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// ʹ��ָ��������Ϣ�Ͷ���Ϊ���쳣ԭ����ڲ��쳣����������ʼ�� System.Exception �����ʵ����
        /// </summary>
        /// <param name="message">�����������Ϣ��</param>
        /// <param name="innerException">���µ�ǰ�쳣���쳣�����δָ���ڲ��쳣������һ��������</param>
        public DataAccessException(string message, string detail)
            : base(message)
        {
            _detail = detail;
        }

        /// <summary>
        /// ʹ��ָ��������Ϣ�Ͷ���Ϊ���쳣ԭ����ڲ��쳣����������ʼ�� System.Exception �����ʵ����
        /// </summary>
        /// <param name="message">�����������Ϣ��</param>
        /// <param name="description">��ϸ��Ϣ</param>
        /// <param name="innerException">���µ�ǰ�쳣���쳣�����δָ���ڲ��쳣������һ��������</param>
        public DataAccessException(string message, string detail, Exception innerException)
            : base(message, innerException)
        {
            _detail = detail;
        }

        /// <summary>
        /// �����л����ݳ�ʼ�� System.Exception �����ʵ����
        /// </summary>
        /// <param name="info">�������й�Դ��Ŀ�����������Ϣ��</param>
        /// <param name="context">�������й��������쳣�����л��Ķ������ݡ�</param>
        protected DataAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
	}
}
