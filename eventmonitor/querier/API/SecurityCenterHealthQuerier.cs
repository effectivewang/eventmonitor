using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventMonitor.Native.SecurityCenter;
using System.Diagnostics;
using log4net;

namespace EventMonitor.Querier {
    /// <summary>
    /// Querier for action center.
    /// </summary>
    /// <see cref="https://msdn.microsoft.com/en-us/library/bb963845(VS.85).aspx"/>
    class SecurityCenterHealthQuerier : EventQuerier {
        private ILog log = LogManager.GetLogger(typeof(SecurityCenterHealthQuerier));

        public SecurityCenterHealthQuerier(EventQueue queue)
            : base(queue, EventType.SecurityCenter) {
        }

        public override void ExecuteQuery() {
            WSC_SECURITY_PROVIDER_HEALTH health = WSC_SECURITY_PROVIDER_HEALTH.WSC_SECURITY_PROVIDER_HEALTH_GOOD;
            int result = WscHelper.GetSecurityProviderHealth(WSC_SECURITY_PROVIDER.WSC_SECURITY_PROVIDER_ALL, out health);

            if (result >= 0 && health == WSC_SECURITY_PROVIDER_HEALTH.WSC_SECURITY_PROVIDER_HEALTH_GOOD) {
                log.Info("All security providers are good.");
                return;
            }

            QueryProviders();
        }

        private void QueryProviders() {
            Array array = Enum.GetValues(typeof(WSC_SECURITY_PROVIDER));
            foreach (WSC_SECURITY_PROVIDER provider in array) {
                if (provider == WSC_SECURITY_PROVIDER.WSC_SECURITY_PROVIDER_ALL ||
                    provider == WSC_SECURITY_PROVIDER.WSC_SECURITY_PROVIDER_NONE) {
                    continue;
                }
                WSC_SECURITY_PROVIDER_HEALTH health = WSC_SECURITY_PROVIDER_HEALTH.WSC_SECURITY_PROVIDER_HEALTH_GOOD;
                int res = WscHelper.GetSecurityProviderHealth(provider, out health);

                Debug.WriteLine(String.Format("Provider: {0}\tstatus {1}\t{2}", provider, health, WscHelper.GetStatus(res)));
                log.InfoFormat("Provider: {0}\tstatus {1}\t{2}", provider, health, WscHelper.GetStatus(res));
                Queue.Enqueue(new Event(Type, String.Format("{0}:\t{1}\t{2}", provider, health, WscHelper.GetStatus(res))));
            }
        }
    }
}
