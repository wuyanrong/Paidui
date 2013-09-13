using System;
using System.Collections.Generic;
using System.Text;

namespace Evt.Framework.Common
{
    /// <summary>
    /// ����
    /// </summary>
    [Serializable]
    public class Parameter
    {
        private string _name = null;
        private object _value = null;
        private ParameterDirectionEnum _direction = ParameterDirectionEnum.Input;

        /// <summary>
        /// ������
        /// </summary>
        public Parameter()
        {
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="parameterName">������</param>
        /// <param name="parameterValue">����ֵ</param>
        public Parameter(string Name, object Value)
        {
            _name = Name;
            _value = Value;
        }

        /// <summary>
        /// ������
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// ����ֵ
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public ParameterDirectionEnum Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

    }
}
