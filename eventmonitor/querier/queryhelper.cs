using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor.Querier
{
    class QueryHelper
    {
        public static string Simple(string wmiClass) {
            

            return String.Format("SELECT * FROM {0}", wmiClass);
        }
    }
}
