using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventMonitor.Native.Network;
using log4net;
using NetFwTypeLib;

namespace EventMonitor.Querier.API {
    class FirewallQuerier : EventQuerier {
        private static readonly ILog log = LogManager.GetLogger(typeof(FirewallQuerier));

        public FirewallQuerier(EventQueue gloablQueue) : base(gloablQueue, EventType.Firewall) { }

        public override void ExecuteQuery() {
            if (!QueryNew()) {
                QueryLegacy();
            }
        }

        private void QueryLegacy() {
            Type fwMgrType = System.Type.GetTypeFromProgID("HNetCfg.FwMgr");

            INetFwMgr mgr = (INetFwMgr)Activator.CreateInstance(fwMgrType);
            bool isEnabled = mgr.LocalPolicy.CurrentProfile.FirewallEnabled;

            INetFwOpenPorts ports = (INetFwOpenPorts)mgr.LocalPolicy.CurrentProfile.GloballyOpenPorts;
            Enqueue("Firewall Enabled:\t{0}", isEnabled);
            if (ports == null || ports.Count < 1) {                
                return;
            }

            foreach (INetFwOpenPort p in ports) {
                string protocol = numToProtocol(p.Protocol);

                Enqueue("Name:\t{0}", p.Name);
                Enqueue("Name:\t{0}", p.Enabled);
                Enqueue("LocalPort:\t{0}", p.Port);
                Enqueue("Protocol:\t{0}", p.Protocol);
                Enqueue("RemoteAddress:\t{0}", p.RemoteAddresses);
            }
        }

        private bool QueryNew() {
            INetFwPolicy2 fwPolicy2;

            Type tNetFwPolicy2 = System.Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            if (tNetFwPolicy2 == null) {
                return false;
            }

            fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
            int CurrentProfiles = fwPolicy2.CurrentProfileTypes;

            bool isPublic = CurrentProfiles == (uint)NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_PUBLIC;
            Enqueue("Is Firwall Public:\t{0}", isPublic);
            if (isPublic) {
                INetFwRules rulesList = fwPolicy2.Rules;
                Enqueue("Name\tEnabled\tProtocol\tAction\tLocalPort\tRemtePorts\tServiceName\tRemoteAddress\tIcmpTypesAndCodes");
                foreach (INetFwRule2 rule in rulesList) {
                    string protocol = numToProtocol(rule.Protocol);

                    Enqueue("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}",
                        rule.Name, rule.Enabled, protocol, rule.Action, rule.LocalPorts,
                        rule.RemotePorts, rule.serviceName, rule.RemoteAddresses, rule.IcmpTypesAndCodes);
                }
            }
            return true;
        }

        private string numToProtocol(object p) {
            string obj = p.ToString();
            if (obj.Equals("null")) {
                return obj;
            }

            int protocolNumber = -1;
            if (!int.TryParse(obj, out protocolNumber)) {
                return obj;
            }
            switch (protocolNumber) {
                case 6:
                    return "tcp";
                case 17:
                    return "udp";
                case 1:
                    return "icmp";
                default:
                    return "other";
            }
        }
    }
}
