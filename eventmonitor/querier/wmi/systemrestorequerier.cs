using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor.Querier.WMI {
    class SystemRestoreQuerier : WMIEventQuerier {
        public SystemRestoreQuerier(EventQueue queue) 
            : base(queue, EventType.SystemRestore){
        }

        public override void ExecuteQuery() {
            RunQuery("root\\DEFAULT", WMIQueryHelper.Simple("SystemRestore"));
        }
    }
}
