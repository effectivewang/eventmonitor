using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventMonitor.Register;

namespace EventMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Waiting for retriving events ...");

            EventQueue globalQueue = new EventQueue();
            // Register Events
            EventQuerier register = new InstalledSoftwareQuerier(globalQueue, EventType.InstalledSoftware);
            register.Query();

            EventViewer viewer = new ConsoleEventViewer(globalQueue);
            viewer.View(EventType.InstalledSoftware);

            Console.ReadKey();
        }
    }
}
