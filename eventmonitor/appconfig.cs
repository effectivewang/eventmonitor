using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventMonitor.Compability;

namespace EventMonitor {
    class AppConfig {
        public static CompabilityPolicy CompabilityPolicy { get { return CompabilityPolicy.Ignore; } }
    }
}
