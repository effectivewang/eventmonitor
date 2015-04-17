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
            Dictionary<string, PropertyData> dict = new Dictionary<string, PropertyData>();
            if (Object != null) {
                foreach (PropertyData data in Object.Properties) {
                    if (HasAttributes || AttributesToDisplay.Contains(data.Name)) {
                        dict.Add(data.Name, data);
                    }
                }
            }

            StringBuilder msgBuilder = new StringBuilder();
            if (HasAttributes) {
                foreach (String name in AttributesToDisplay) {
                    msgBuilder.AppendFormat("{0}: {1}\t", name, GetValue(dict[name].Value));
                }
            } else {
                foreach (var item in dict) {
                    msgBuilder.AppendFormat("{0}: {1}\t", item.Key, GetValue(item.Value.Value));
                }
            }

            Message = msgBuilder.ToString();
        }

        private String GetValue(Object value) {
            if (String.IsNullOrEmpty(value.ToString())) {
                value = "N/A";
            }
            return value.ToString();
        }

        private bool HasAttributes { get { return AttributesToDisplay != null && AttributesToDisplay.Length > 0; } }
    }
}