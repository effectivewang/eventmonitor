using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using EventMonitor.Native.Installer;
using EventMonitor.Native;

namespace EventMonitor.Querier.API {
    class InstallerQuerier : EventQuerier {
        private static readonly ILog log = LogManager.GetLogger(typeof(InstallerQuerier));

        public InstallerQuerier(EventQueue queue)
            : base(queue, EventType.MsiEnumProducts) { }

        public override void ExecuteQuery() {
            StringBuilder productInfo = new StringBuilder(39); // https://msdn.microsoft.com/en-us/library/aa370101(v=vs.85).aspx
            StringBuilder productName = new StringBuilder(100);

            for (int i = 0; i < int.MaxValue; i++) {
                if (SystemErrorCodes.ERROR_NO_MORE_ITEMS == WsiHelper.MsiEnumProducts(i, productInfo)) {
                    break;
                }

                int size = -1;
                WsiHelper.MsiGetProductInfo(productInfo.ToString(), "ProductName", 
                    productName, ref size); 

                Enqueue(String.Format("IdentificationNumber: {0}\t{1}", productInfo, productName));
            }
        }

    }
}
