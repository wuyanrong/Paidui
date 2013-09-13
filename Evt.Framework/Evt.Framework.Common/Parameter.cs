using System;
using System.Collections.Generic;
using System.Text;

namespace Evt.Framework.Common
{
    /// <summary>
    /// 参数
    /// </summary>
    [Serializable]
    public class Parameter
    {
        private string _name = null;
        private object _value = null;
        private ParameterDirectionEnum _direction = ParameterDirectionEnum.Input;

        /// <summary>
        /// 构造器
        /// </summary>
        public Parameter()
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="parameterValue">参数值</param>
        public Parameter(string Name, object Value)
        {
            _name = Name;
            _value = Value;
        }

        /// <summary>
        /// 参数名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// 参数方向
        /// </summary>
        public ParameterDirectionEnum Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

    }
}
