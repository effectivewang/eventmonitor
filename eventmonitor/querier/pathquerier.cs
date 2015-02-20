using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor.Querier
{
    class PathQuerier : EventQuerier
    {
        public PathQuerier(EventQueue queue, EventType type) 
            : base(queue, type){ 
        }

        public override void Query()
        {
            SimpleQuery("win32_quickfixengineering");
            SimpleQuery("SMS_SoftwareUpdae");
        }
    }
}
