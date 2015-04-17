using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventMonitor.Native.Registry {
    class RegistryHelper {

        [DllImport("advapi32.dll")]
        public static extern int RegLoadKey(uint hKey, string lpSubKey, string lpFile);
        [DllImport("advapi32.dll")]
        public static extern int RegUnLoadKey(uint hKey, string lpSubKey);
        [DllImport("advapi32.dll")]
        public static extern int OpenProcessToken(int ProcessHandle, int DesiredAccess, ref int tokenhandle);
        [DllImport("kernel32.dll")]
        public static extern int GetCurrentProcess();
        [DllImport("advapi32.dll")]
        public static extern int AdjustTokenPrivileges(int tokenhandle, int disableprivs, [MarshalAs(UnmanagedType.Struct)]ref TOKEN_PRIVILEGES Newstate, int bufferlength, int PreivousState, int Returnlength);
        [DllImport("advapi32.dll")]
        public static extern int LookupPrivilegeValue(string lpsystemname, string lpname, [MarshalAs(UnmanagedType.Struct)] ref LUID lpLuid);

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID {
            public int LowPart;
            public int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES {
            public LUID Luid;
            public int Attributes;
            public int PrivilegeCount;
        }

        public static int LoadHiveOffline() {

            if (!File.Exists(string.Format(@"{0}\windows\system32\config\system", Environment.SystemDirectory))) {
                return -1;
            }

            int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
            int SE_PRIVILEGE_ENABLED = 0x00000002;
            int TOKEN_QUERY = 0x00000008;
            int token = 0;
            int retval = 0;
            //uint HKU = 0x80000003;
            uint HKLM = 0x80000002;
            string SE_BACKUP_NAME = "SeBackupPrivilege";
            string SE_RESTORE_NAME = "SeRestorePrivilege";

            string offlineSystemHive = string.Format(@"{0}\windows\system32\config\SYSTEM", Main.GLOBALDRIVELETTER);
            string offlineSoftwareHive = string.Format(@"{0}\windows\system32\config\SOFTWARE", Main.GLOBALDRIVELETTER);
            string offlineBCDHive = GetBCDPath();

            LUID RestoreLuid = new LUID();
            LUID BackupLuid = new LUID();

            TOKEN_PRIVILEGES TP = new TOKEN_PRIVILEGES();
            TOKEN_PRIVILEGES TP2 = new TOKEN_PRIVILEGES();

            retval = OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref token);
            retval = LookupPrivilegeValue(null, SE_RESTORE_NAME, ref RestoreLuid);
            retval = LookupPrivilegeValue(null, SE_BACKUP_NAME, ref BackupLuid);

            TP.PrivilegeCount = 1;
            TP.Attributes = SE_PRIVILEGE_ENABLED;
            TP.Luid = RestoreLuid;
            TP2.PrivilegeCount = 1;
            TP2.Attributes = SE_PRIVILEGE_ENABLED;
            TP2.Luid = BackupLuid;

            retval = AdjustTokenPrivileges(token, 0, ref TP, 1024, 0, 0);
            retval = AdjustTokenPrivileges(token, 0, ref TP2, 1024, 0, 0);

            if (RegLoadKey(HKLM, AWSTEMPSOFTWARE, offlineSoftwareHive).Equals(0)) {
                RegLoadKey(HKLM, AWSTEMPBCD, offlineBCDHive);
                return RegLoadKey(HKLM, AWSTEMPSYSTEM, offlineSystemHive);
            }

            return -1;
        }

        public static void UnloadSystemHive() {
            GC.Collect(); // collects all unused memory
            GC.WaitForPendingFinalizers(); // wait until GC has finished its work
            GC.Collect();

            System.Threading.Thread.Sleep(400);
            uint HKLM = 0x80000002;
            int x = RegUnLoadKey(HKLM, AWSTEMPSYSTEM);
            int y = RegUnLoadKey(HKLM, AWSTEMPSOFTWARE);
            int z = RegUnLoadKey(HKLM, AWSTEMPBCD);
            Console.WriteLine(x + y + z);
        }

    }
}
