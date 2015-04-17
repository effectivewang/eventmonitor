using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventMonitor.Querier;
using System.Diagnostics;
using EventMonitor.Querier.Registry;
using EventMonitor.Querier.WMI;
using System.ComponentModel;
using System.Reflection;
using EventMonitor.Viewer;
using EventMonitor.Querier.API;
using System.Windows.Forms;
using System.Threading;

namespace EventMonitor {
    class Program {
        static void Main(string[] args) {
            RunConsole();
        }

        private static void RunApplication() {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }

        private static void RunConsole() {
            for (byte i = 0; i < Byte.MaxValue; i++) {
                EventType type = DisplaySelection();

                EventQueue globalQueue = new EventQueue();
                Query(globalQueue, type);

                EventViewer viewer = new ConsoleEventViewer(globalQueue);
                viewer.View(type);

                Console.WriteLine();
            }
        }
        private static EventType DisplaySelection() {
            Console.WriteLine("Please select following option (example: 1): ");
            Array array = Enum.GetValues(typeof(EventType));
            for (int i = 1; i < array.Length; i++) {
                EventType type = (EventType)array.GetValue(i);
                FieldInfo field = typeof(EventType).GetField(type.ToString());
                DescriptionAttribute[] attrs = (DescriptionAttribute[])field.GetCustomAttributes(
                                                    typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    Console.WriteLine(String.Format("{0}. {1} ({2})", i, type, attrs[0].Description));
                else
                    Console.WriteLine(String.Format("{0}. {1}", i, type));
            }

            Console.Write("Select: ");  String input = Console.ReadLine();
            EventType selectedType = EventType.Unknown;

            try {
                selectedType = (EventType)Enum.Parse(typeof(EventType), input);
            } catch (Exception) {
                selectedType = EventType.Unknown;
            }
            if (Enum.IsDefined(typeof(EventType), selectedType) && selectedType != EventType.Unknown) {
                return selectedType;
            } else {
                Console.WriteLine();
                selectedType = DisplaySelection();
            }

            return selectedType;
        }

        private static void Query(EventQueue globalQueue, EventType type) {
            EventQuerier register = null;

            if (type == EventType.InstalledSoftware)
                register = new InstalledSoftwareQuerier(globalQueue);
            else if (type == EventType.Win32Product)
                register = new Win32ProductQuerier(globalQueue);
            else if (type == EventType.MsiEnumProducts)
                register = new InstallerQuerier(globalQueue);
            else if (type == EventType.HoxFix)
                register = new HotFixQuerier(globalQueue);
            else if (type == EventType.SecurityCenter)
                register = new SecurityCenterHealthQuerier(globalQueue);
            else if (type == EventType.SystemRestore)
                register = new SystemRestoreQuerier(globalQueue);
            else if (type == EventType.LastRestoreStatus)
                register = new LastRestoreStatusQuerier(globalQueue);
            else if (type == EventType.Firewall)
                register = new FirewallQuerier(globalQueue);
            else if (type == EventType.CustomWMIQuery)
                register = CreateCustomWMIQuerier(globalQueue);
            else
                throw new ArgumentOutOfRangeException();

            register.ExecuteQuery();
        }

        private static EventQuerier CreateCustomWMIQuerier(EventQueue globalQueue) {
            Console.Write("Query String: ");
            String queryString = Console.ReadLine();

            return new CustomWMIQuery(globalQueue, queryString);
        }
    }
}
