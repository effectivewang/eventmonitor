using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor
{
    [Flags]
    enum EventType : int
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Installed software
        /// </summary>
        InstalledSoftware = 1,
        /// <summary>
        /// Action messages
        /// </summary>
        ActionCenter = 2,
        /// <summary>
        /// Pathces update
        /// </summary>
        PatchUpdate = 4
    }
}
