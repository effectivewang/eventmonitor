using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace EventMonitor {
    class WMIEvent : Event {
        public ManagementBaseObject Object { get; private set; }
        public string[] AttributesToDisplay { get; private set; }

        public WMIEvent(EventType type, ManagementBaseObject obj, string[] attrsToDisplay)
            : base(type, null) {
            Object = obj;
            AttributesToDisplay = attrsToDisplay;

            BuildMessage();
        }

        private void BuildMessage() {
            List<string> strings = new List<string>();
            if (Object != null) {
                foreach (PropertyData data in Object.Properties) {
                    if (AttributesToDisplay == null || AttributesToDisplay.Contains(data.Name)) {
                        strings.Add(String.Format("{0}: {1}", data.Name, data.Value));
                    }
                }
            }

            Message = String.Format("{0} {1}", String.Join("\t", strings.ToArray()), Message);
        }
    }
}