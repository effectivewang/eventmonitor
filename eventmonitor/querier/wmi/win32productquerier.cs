using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor.Querier.WMI {
    /// <summary>
    /// Querier for Win32_Product, can only get current user's installed softwares.
    /// </summary>
    /// <remarks>
    /// Time-Consuming Opeartion.
    /// Reference: https://msdn.microsoft.com/en-us/library/aa394378(v=vs.85).aspx
    /// </remarks>
    class Win32ProductQuerier : WMIEventQuerier {
        protected override string[] Attributes { get { return new string[] { "IdentifyingNumber", "Caption" }; } }
        public Win32ProductQuerier(EventQueue queue)
            : base(queue, EventType.Win32Product) {
        }

        public override void ExecuteQuery() {
            RunQuery(WMIQueryHelper.Simple("Win32_Product"));
        }
    }
}
