using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EventMonitor {
    class ReflectionHelper {
        /// <summary>
        /// Get all public properity map.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, Object> GetPropertyMap(object obj) {
            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            if (obj == null) {
                return dict;
            }

            PropertyInfo[] props = obj.GetType().GetProperties();
            if (props == null) {
                return dict;
            }

            foreach (PropertyInfo p in props) {
                object val = p.GetValue(obj, null);
                dict[p.Name] = val;
            }
            return dict;
        }
    }
}
