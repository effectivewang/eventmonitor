using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace EventMonitor
{
    class Event
    {
        public String Message { get; protected set; }
        public EventType Type { get; private set; }

        public Event(EventType type, String message) {
            Type = type;
            Message = message;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
