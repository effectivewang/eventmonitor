using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor.Querier.WMI {
    class WMIQueryHelper {
        public static string Simple(string wmiClass) {
            return String.Format("SELECT * FROM {0}", wmiClass);
        }
    }
}
