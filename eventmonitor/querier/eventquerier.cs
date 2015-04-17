using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using EventMonitor.Querier;
using log4net;

namespace EventMonitor {
    interface IEventQuerier {
        void ExecuteQuery();
    }

    abstract class EventQuerier : IEventQuerier {
        private List<EventQuerier> children = new List<EventQuerier>();

        protected EventQueue Queue { get; private set; }
        public EventType Type { get; private set; }
        public List<EventQuerier> Children {
            get {
                return children;
            }
        }

        public EventQuerier(EventQueue queue, EventType type) {
            this.Queue = queue;
            this.Type = type;
        }

        public void Query() {
            this.ExecuteQuery();

            foreach (EventQuerier child in Children) {
                child.Query();
            }
        }

        protected void Enqueue(String message) {
            Queue.Enqueue(new Event(Type, message));
        }

        protected void Enqueue(String messageFormat, params object[] args) {
            Enqueue(String.Format(messageFormat, args));
        }

        public abstract void ExecuteQuery();
    }
}
