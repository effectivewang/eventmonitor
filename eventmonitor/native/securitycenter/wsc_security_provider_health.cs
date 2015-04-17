using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace EventMonitor.Native.SecurityCenter
{
    enum WSC_SECURITY_PROVIDER_HEALTH
    {
        WSC_SECURITY_PROVIDER_HEALTH_GOOD, // Green pillar in English locales
        WSC_SECURITY_PROVIDER_HEALTH_NOTMONITORED, // Yellow pillar in English locales
        WSC_SECURITY_PROVIDER_HEALTH_POOR,  // Red pillar in English locales
        WSC_SECURITY_PROVIDER_HEALTH_SNOOZE, // Yellow pillar in English locales
    }
}
