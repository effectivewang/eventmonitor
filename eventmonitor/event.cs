using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace EventMonitor
{
    /// <summary>
    ///     Windows CIM Event.
    /// </summary>
    /// <see cref="https://msdn.microsoft.com/en-us/library/aa392738(v=vs.85).aspx"/>
    class Event
    {
        public ManagementBaseObject Object { get; private set; }
        public String Message { get; private set; }
        public EventType Type { get; private set; }
        public HashSet<string> Filters { get; private set; }

        public Event(EventType type, ManagementBaseObject obj, HashSet<string> filters) {
            Object = obj;
            Type = type;
            Filters = filters;
        }

        public Event(EventType type, String message) {
            Type = type;
            Message = message;
        }

        public override string ToString()
        {
            List<string> strings = new List<string>();
            if (Object != null)
            {
                foreach (PropertyData data in Object.Properties)
                {
                    if (Filters.Contains(data.Name))
                    {
                        strings.Add(String.Format("{0}:{1}", data.Name, data.Value));
                    }
                }
            }

            return String.Format("[{0}] {1} {2}", Type, String.Join(",", strings), Message);
        }
    }
}
