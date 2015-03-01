using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventMonitor;
using System.Management;
using Microsoft.Win32;
using log4net;
using EventMonitor.Querier.WMI;

namespace EventMonitor.Querier.Registry {
    class InstalledSoftwareQuerier : EventQuerier {
        private ILog log = LogManager.GetLogger(typeof(InstalledSoftwareQuerier));

        public InstalledSoftwareQuerier(EventQueue queue)
            : base(queue, EventType.InstalledSoftware) {
        }

        public override void ExecuteQuery() {
            QueryByRegistry();
        }

        private void QueryByRegistry() {
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registry_key)) {
                foreach (string subkey_name in key.GetSubKeyNames()) {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name)) {
                        object obj = subkey.GetValue("DisplayName");
                        Enqueue(obj == null ? subkey_name : obj.ToString());
                    }
                }
            }
        }

    }
}
