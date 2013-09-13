using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Evt.Framework.Common
{
    /// <summary>
    /// Assembly工具类
    /// </summary>
    public class AssemblyUtil
    {
        private static Hashtable _assembly = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 获取程序集的Type
        /// </summary>
        /// <example>
		/// object a = Utility.GetAssemblyInstance("assembly1", "assembly1.class1");
        /// </example>
        /// <param name="assemblyString"></param>
        /// <returns></returns>
        public static object GetAssemblyInstance(string assemblyName, string fullClassName)
        {
            Assembly assem = null;

			if (_assembly.ContainsKey(assemblyName))
            {
				assem = (Assembly)_assembly[assemblyName];
            }
            else
            {
				assem = Assembly.Load(assemblyName);
				_assembly[assemblyName] = assem;
            }

			return assem.CreateInstance(fullClassName);
        }

		/// <summary>
		/// Clear cached assembly
		/// </summary>
		public static void Clear()
		{
			_assembly.Clear();
		}
    }
}
