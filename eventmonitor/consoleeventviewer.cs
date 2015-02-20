using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor
{
    class ConsoleEventViewer : EventViewer
    {
        private EventQueue queue;

        public ConsoleEventViewer(EventQueue queue) {
            this.queue = queue;    
        }

        public void View(EventType type) {
            while (!queue.IsEmpty) {
                Event e = null;
                if (!queue.TryDequeue(out e))
                {
                    continue;
                }

                if ((e.Type & type) == EventType.Unknown) {
                    continue;
                }

                if (queue.TryDequeue(out e))
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
