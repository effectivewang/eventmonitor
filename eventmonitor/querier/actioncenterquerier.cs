using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor.Querier
{
    class ActionCenterQuerier : EventQuerier
    {
        public ActionCenterQuerier(EventQueue queue, EventType type) 
            : base(queue, type){ 
        }

        public override void Query()
        {
            SimpleQuery("win32_quickfixengineering");
        }
    }
}
