using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using EventMonitor.Querier;

namespace EventMonitor
{
    interface IEventQuerier
    {
        void Query();
    }

    abstract class EventQuerier : IEventQuerier{
        
        protected EventQueue Queue {get; private set;}
        public EventType Type { get; private set; }

        public EventQuerier(EventQueue queue, EventType type)
        {
            this.Queue = queue;
            this.Type = type;
        }

        public abstract void Query();

        protected void RunQuery(String queryString)
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher(queryString);
            ManagementObjectCollection collection = mos.Get();
            if (collection == null) return;

            foreach (ManagementObject mo in collection)
            {
                Queue.Enqueue(new Event(EventType.InstalledSoftware, mo, Filters));
            }
        }

        protected void SimpleQuery(String wmiClass) {
            RunQuery(QueryHelper.Simple(wmiClass));
        }

        protected virtual HashSet<string> Filters { get { return new HashSet<string>();  } }
    }
}
