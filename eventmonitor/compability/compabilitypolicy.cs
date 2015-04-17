using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor.Compability {
    /// <summary>
    /// Compability policy if the event is not supported in current version of windows.
    /// </summary>
    enum CompabilityPolicy {
        Unknown = 0,
        Expception = 1,
        Ignore = 2
    }
}
