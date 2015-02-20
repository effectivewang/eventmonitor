using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace EventMonitor
{
    /// <summary>
    /// Queue of <see cref="Event"/>
    /// </summary>
    class EventQueue : ConcurrentQueue<Event>
    {
        
    }
}
