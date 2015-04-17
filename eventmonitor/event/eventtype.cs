using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EventMonitor
{
    enum EventType : int
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        [Description("All softwares read from registry")]
        InstalledSoftware,
        [Description("Softwares installed by curent user and time-consuming opeartion")]
        Win32Product,
        [Description("MsiEnumProducts with windows installer")]
        MsiEnumProducts,
        [Description("Security center components' status")]
        SecurityCenter,
        [Description("HotFixes")]
        HoxFix,
        [Description("System restore points")]
        SystemRestore,
        [Description("Last time restore status")]
        LastRestoreStatus,
        [Description("Firewall setting")]
        Firewall,
        [Description("Server features installed on server")]
        ServerFeature,
        [Description("Custom WMI Query (eg. SELECT * FROM Win32_Product)")]
        CustomWMIQuery
    }
}
