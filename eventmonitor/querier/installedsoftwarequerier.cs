using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventMonitor;
using System.Management;
using Microsoft.Win32;

namespace EventMonitor.Register
{
    class InstalledSoftwareQuerier : EventQuerier
    {
        protected override HashSet<string> Filters
        {
            get
            {
                return new HashSet<string>(new string[] { "Caption" } );
            }
        }
        public InstalledSoftwareQuerier(EventQueue queue, EventType type) 
            : base(queue, type){ 
        }

        public override void Query()
        {
            //SimpleQuery("Win32_Product");
            // RunQuery("SELECT * FROM Win32_ApplicationService");
            // RunQuery("SELECT * FROM Win32_SoftwareFeature");

            // RunQuery("SELECT * FROM Win32_InstalledSoftwareElement");
            // RunQuery("SELECT * FROM Win32_ActionCheck");
            // RunQuery("SELECT * FROM Win32_ServerFeature");
            // SimpleQuery("Win32_InstalledSoftwareElement");

            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        object obj = subkey.GetValue("DisplayName");
                        Queue.Enqueue(new Event(EventType.InstalledSoftware, obj == null ? subkey.Name : obj.ToString()));
                    }
                }
            }
        }
    }
}
