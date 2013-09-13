using System;
using System.Collections.Generic;
using System.Text;

namespace Evt.Framework.Common
{
    /// <summary>
    /// ��������
    /// </summary>
    [Serializable]
    public class ParameterCollection : List<Parameter>
    {
        /// <summary>
        /// ��Ӳ���
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
        public void Add(string name, object value)
        {
            Add(name, value, ParameterDirectionEnum.Input);
        }

        /// <summary>
        /// ��Ӳ���
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
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
		/// ��ʾ��������ϸ����
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
