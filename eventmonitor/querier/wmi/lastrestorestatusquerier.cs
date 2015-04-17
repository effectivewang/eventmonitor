using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using log4net;

namespace EventMonitor.Querier.WMI {
    class LastRestoreStatusQuerier : WMIEventQuerier {
        private ILog log = LogManager.GetLogger(typeof(LastRestoreStatusQuerier));
        enum RestoreStatus : int {
            Failed = 0, Success = 1, Interrrupted = 2
        }

        public LastRestoreStatusQuerier(EventQueue queue) 
            : base(queue, EventType.LastRestoreStatus){
        }

        public override void ExecuteQuery() {
            ManagementClass mgmtClass = new ManagementClass("root\\DEFAULT", "SystemRestore",
     new ObjectGetOptions());
            object val = mgmtClass.InvokeMethod("GetLastRestoreStatus", null);
            Enqueue(String.Format("GetLastRestoreStatus: {0}", (RestoreStatus)(uint)val));
        }
    }
}
