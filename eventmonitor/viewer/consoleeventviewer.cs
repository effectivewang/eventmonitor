using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace EventMonitor.Viewer
{
    class ConsoleEventViewer : EventViewer
    {
        private EventQueue queue;

        public ConsoleEventViewer(EventQueue queue) {
            this.queue = queue;    
        }

        public void View(EventType type) {
            int total = queue.Count;
            Console.WriteLine(String.Format("Total Event Number: {0}", total));
            for (int i = 0; i < total; i++ )
            {
                Event e = queue.Dequeue();
                if (e.Type != type)
                {
                    continue;
                }

                Console.WriteLine(e);
            }
        }
    }
}
