using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace EventMonitor.Native.Registry {
    class Advapi {
        enum RootKey : uint { 
            HKEY_CLASSES_ROOT,
            HKEY_CURRENT_CONFIG,
            HKEY_CURRENT_USER,
            HKEY_LOCAL_MACHINE,
            HKEY_USERS
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegOpenKeyA(RootKey key, string lpSubKey, ref int phkResult);

        [DllImport("advapi32.dll", EntryPoint = "RegEnumKeyEx")]
        public static extern int RegEnumKeyEx(UIntPtr hkey,
            RootKey key,
            StringBuilder lpName,
            ref uint lpcbName,
            IntPtr reserved,
            IntPtr lpClass,
            IntPtr lpcbClass,
            out long lpftLastWriteTime);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW", SetLastError = true)]
        public static extern int RegQueryValueEx(
               RootKey key,
               string lpValueName,
               int lpReserved,
               out uint lpType,
               System.Text.StringBuilder lpData,
               ref uint lpcbData);
    }
}
