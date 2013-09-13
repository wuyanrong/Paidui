using System;
using System.Collections.Generic;
using System.Text;

namespace Evt.Framework.Common
{
    /// <summary>
    /// 参数集合
    /// </summary>
    [Serializable]
    public class ParameterCollection : List<Parameter>
    {
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void Add(string name, object value)
        {
            Add(name, value, ParameterDirectionEnum.Input);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public void Add(string name, object value, ParameterDirectionEnum direction)
        {
            Parameter p = new Parameter();
            p.Name = name;
            p.Value = value;
            p.Direction = direction;

            this.Add(p);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public Parameter this[string name]
        {
            get 
            {
                foreach (Parameter p in this)
                {
                    if (p.Name == name)
                    {
                        return p;
                    }
                }
                return null;
            }
        }

		/// <summary>
		/// 显示参数的详细内容
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string ret = "";

			for (int i = 0; i < this.Count; i++)
			{
                ret += ", " + "Parameter Name/Value: " + this[i].Name + "/" + (this[i].Value == null ? "null" : this[i].Value.ToString());
			}

			if (ret.Length > 0)
			{
				ret = ret.Substring(1);
			}
			else
			{
				ret = base.ToString();
			}

			return ret;
		}
    }
}
