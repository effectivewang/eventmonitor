using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor.Querier.WMI {
    class CustomWMIQuery : WMIEventQuerier {
        public String QueryString { get; private set; }

        public CustomWMIQuery(EventQueue globalQueue, String queryString) 
            : base(globalQueue, EventType.CustomWMIQuery) {
            QueryString = queryString;
        }

        public override void ExecuteQuery() {
            RunQuery("root\\cimv2", QueryString);
        }
    }
}
