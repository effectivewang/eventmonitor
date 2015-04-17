using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor.Querier.WMI {
    class ServerFeatureQuerier : WMIEventQuerier {

        public ServerFeatureQuerier(EventQueue queue) 
            : base(queue, EventType.ServerFeature){
        }

        public override void ExecuteQuery() {
            RunQuery(WMIQueryHelper.Simple("Win32_ServerFeature"));
        }
    }
}
