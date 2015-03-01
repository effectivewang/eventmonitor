using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace EventMonitor.Querier.WMI {
    class HotFixQuerier : WMIEventQuerier {
        private ILog log = LogManager.GetLogger(typeof(HotFixQuerier));

        /// <summary>
        ///  Filter properties to display.
        /// </summary>
        /// <example>
        ///  Caption:http://support.microsoft.com/?kbid=3019215,CSName:COMPUTER-NAME,
        ///  Description:Security Update,FixComments:,HotFixID:KB3019215,InstallDate:,
        ///  InstalledBy:COMPUTER-NAME\USER-NAME,InstalledOn:1/19/2015,Name:,ServicePackInEffect:,Status:
        /// </example>
        protected override string[] Attributes { get { return new string[] { "Caption", "HotFixID" }; } }

        public HotFixQuerier(EventQueue queue)
            : base(queue, EventType.HoxFix) {
        }

        public override void ExecuteQuery() {
            RunQuery(WMIQueryHelper.Simple("Win32_QuickFixEngineering"));
        }
    }
}
