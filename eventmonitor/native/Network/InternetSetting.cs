using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace EventMonitor.Native {
    /// <summary>
    /// Query inseret setting.
    /// </summary>
    /// <see cref="https://msdn.microsoft.com/en-us/library/aa385384(v=vs.85).aspx"/>
    /// <seealso cref="https://msdn.microsoft.com/en-us/library/aa385146(v=vs.85).aspx"/>
    class InternetSetting {
        const int INTERNET_OPTION_VERSION = 40;
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct InternetPerConnOptionList {
            /// <summary>
            /// size of the InternetPerConnOptionList struct
            /// </summary>
            public int dwSize;
            /// <summary>
            /// Connection name to set/query options
            /// </summary>
            public IntPtr szConnection;
            /// <summary>
            /// Number of options to set/query
            /// </summary>
            public int dwOptionCount;
            /// <summary>
            /// On error, which option failed
            /// </summary>
            public int dwOptionError;
            public IntPtr options;
        }

        /// <summary>
        /// TODO:: Implment this!
        /// </summary>
        public struct INTERNET_PER_CONN_OPTION {
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNET_VERSION_INFO {
            public int dwMajorVersion;
            public int dwMinorVersion;
        };

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetQueryOption(IntPtr hInternet, uint dwOption, char[] lpBuffer, ref int lpdwBufferLength);
    }
}
