using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor
{
    interface EventViewer
    {
        void View(EventType type);
    }
}
