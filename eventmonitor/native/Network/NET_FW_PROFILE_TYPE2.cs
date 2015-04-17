using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor.Native.Network {
    /// <summary>
    /// Type of windows firewall profile.
    /// <seealso cref="https://msdn.microsoft.com/en-us/library/windows/desktop/aa366303(v=vs.85).aspx"/>
    /// </summary>
    enum NET_FW_PROFILE_TYPE2 : uint {
        NET_FW_PROFILE2_DOMAIN   = 0x0001,
        NET_FW_PROFILE2_PRIVATE  = 0x0002,
        NET_FW_PROFILE2_PUBLIC   = 0x0004,
        NET_FW_PROFILE2_ALL      = 0x7FFFFFFF
    }
}
