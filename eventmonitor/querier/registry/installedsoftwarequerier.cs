using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventMonitor;
using System.Management;
using Microsoft.Win32;
using log4net;
using EventMonitor.Querier.WMI;
using System.Diagnostics;
using System.Security.AccessControl;
using EventMonitor.Native.Registry;
using System.Threading;
using EventMonitor.Native.Account;
using System.Text.RegularExpressions;

namespace EventMonitor.Querier.Registry {
    class InstalledSoftwareQuerier : EventQuerier {
        private const String KB_PATTERN = "KB\\d+$";

        private ILog log = LogManager.GetLogger(typeof(InstalledSoftwareQuerier));
        private String[] Properties = new String[] { "DisplayName", "DisplayVersion", "Publisher", "InstallLocation",
                                                     "UninstallString", "QuietUninstallString" };

        private Dictionary<String, String> SoftwareDict = new Dictionary<String, String>();
        public InstalledSoftwareQuerier(EventQueue queue)
            : base(queue, EventType.InstalledSoftware) {
        }

        public override void ExecuteQuery() {
            QueryByRegistry();
        }

        private void QueryByRegistry() {
            EnumerateKey(RegHive.HKEY_LOCAL_MACHINE, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\");
            if (Environment.OSVersion.Version.Major >= 6) {
                EnumerateKey(RegHive.HKEY_LOCAL_MACHINE, @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\");
            }

            if (!String.IsNullOrEmpty(User.Current.SID)) {
                EnumerateKey(RegHive.HKEY_USERS, 
                    String.Format("{0}\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\", User.Current.SID));
            }

            Enqueue();
        }

        private void EnumerateKey(RegHive regHive,String registryKey) {
            RegistryManager rm = new RegistryManager();
            String[] subKeyNames = rm.EnumKeys(regHive, registryKey);
            if (subKeyNames == null) {
                return;
            }

            foreach (String keyName in subKeyNames) {
                if (Regex.IsMatch(keyName, KB_PATTERN)) {
                    continue;
                }

                String fullKeyName = registryKey + keyName;
                String displayName = rm.GetStringValue(regHive, fullKeyName, "DisplayName");
                if (String.IsNullOrEmpty(displayName)) {
                    continue;
                }

                object systemComponent = rm.GetValue(regHive, fullKeyName, "SystemComponent");
                if (systemComponent != null && (uint)systemComponent == 1) {
                    continue;
                }

                String[] values = new String[Properties.Length];
                for (int i = 0; i < Properties.Length; i++) {
                    String valueName = Properties[i];
                    String value = rm.GetStringValue(regHive, fullKeyName, valueName);
                    if (String.IsNullOrEmpty(valueName)) {
                        value = "N/A";
                    }
                    values[i] = value;
                }
                
                if (!SoftwareDict.ContainsKey(keyName)) {
                    SoftwareDict[keyName] = String.Join("\t", values);
                }
            }
        }

        private void Enqueue() {
            List<String> messages = SoftwareDict.Values.ToList();
            messages.Sort();

            foreach (var msg in messages) {
                Enqueue(msg);
            }
        }

    }
}
