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
        [Description("All softwares installed on machine, read from registry")]
        InstalledSoftware = 1,
        [Description("Softwares installed by curent user and time-consuming opeartion")]
        Win32Product = 2,
        [Description("Security center components' status")]
        SecurityCenter = 3,
        [Description("HotFixes")]
        HoxFix = 4,
        [Description("System restore points")]
        SystemRestore = 5,
        [Description("Last time restore status")]
        LastRestoreStatus = 6,
        [Description("Features installed on server")]
        ServerFeature = 7
    }
}
