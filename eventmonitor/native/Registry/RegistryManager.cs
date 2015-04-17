using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Permissions;
using System.Text;

namespace EventMonitor.Native.Registry {
    public enum RegHive : uint {
        HKEY_CLASSES_ROOT = 0x80000000,
        HKEY_CURRENT_USER = 0x80000001,
        HKEY_LOCAL_MACHINE = 0x80000002,
        HKEY_USERS = 0x80000003,
        HKEY_CURRENT_CONFIG = 0x80000005,
    }

    public enum RegType : uint {
        REG_SZ = 1,
        REG_EXPAND_SZ = 2,
        REG_BINARY = 3,
        REG_DWORD = 4,
        REG_MULTI_SZ = 7
    }

    /// <summary>
    /// Manipulate system registry keys and values.
    /// <see cref="https://msdn.microsoft.com/en-us/library/aa393664(VS.85).aspx"/> 
    /// </summary>
    class RegistryManager {
        private ManagementClass mc;
        public RegistryManager() {
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = ImpersonationLevel.Impersonate;
            options.Authentication = AuthenticationLevel.Unchanged;
            options.EnablePrivileges = true;
            options.Username = null;
            options.Password = null;

            ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\default", Environment.MachineName), options);
            ManagementPath path = new ManagementPath("StdRegProv");
            
            mc = new ManagementClass(scope, path, null);
        }

        public String GetStringValue(RegHive regHive, String subKeyName, String valueName) {
            ManagementBaseObject inParams = mc.GetMethodParameters("GetStringValue");
            ManagementBaseObject outParams;
            inParams["hDefKey"] = regHive;
            inParams["sSubKeyName"] = subKeyName;
            inParams["sValueName"] = valueName;

            String retVal = String.Empty;

            outParams = mc.InvokeMethod("GetStringValue", inParams, null);
            if (Convert.ToUInt32(outParams["ReturnValue"]) == 0) {
                retVal = outParams["sValue"].ToString();
            }
            return retVal;
        }

        public object GetValue(RegHive regHive, String subKey, String valueName) {
            RegType type = GetValueType(regHive, subKey, valueName);

            ManagementBaseObject inParams = mc.GetMethodParameters("GetStringValue");
            inParams["hDefKey"] = regHive;
            inParams["sSubKeyName"] = subKey;
            inParams["sValueName"] = valueName;

            ManagementBaseObject outParams = null; object retVal = null;
            if (type == RegType.REG_BINARY) {
                outParams = mc.InvokeMethod("GetBinaryValue", inParams, null);
                if ((uint)outParams["ReturnValue"] == 0) {
                    retVal = (byte)outParams["uValue"];
                }
            } else if (type == RegType.REG_DWORD) {
                outParams = mc.InvokeMethod("GetDWORDValue", inParams, null);
                if ((uint)outParams["ReturnValue"] == 0) {
                    retVal = outParams["uValue"];
                }
            } else if (type == RegType.REG_EXPAND_SZ) {
                outParams = mc.InvokeMethod("GetExpandedStringValue", inParams, null);
                if ((uint)outParams["ReturnValue"] == 0) {
                    retVal = outParams["sValue"];
                }
            } else if (type == RegType.REG_MULTI_SZ) {
                outParams = mc.InvokeMethod("GetMultiStringValue", inParams, null);
                if ((uint)outParams["ReturnValue"] == 0) {
                    retVal = outParams["sValue"];
                }
            } else if (type == RegType.REG_SZ) {
                outParams = mc.InvokeMethod("GetBinaryValue", inParams, null);
                if ((uint)outParams["ReturnValue"] == 0) {
                    retVal = outParams["sValue"];
                }
            }

            return retVal;
        }

        public RegType GetValueType(RegHive regHive, String subKey, String valueName) {
            ManagementBaseObject inParams = mc.GetMethodParameters("EnumValues");
            inParams["hDefKey"] = regHive;
            inParams["sSubKeyName"] = subKey;

            ManagementBaseObject outParams = mc.InvokeMethod("EnumValues", inParams, null);
            if (Convert.ToUInt32(outParams["ReturnValue"]) == 0) {
                String[] names = (String[])outParams["sNames"];
                int[] types = (int[])outParams["Types"];

                for (int i = 0; i < names.Length; i++) {
                    if (names[i] == valueName) {
                        return (RegType)types[i];
                    }
                }
            }

            return RegType.REG_SZ; // return default
        }

        public String[] EnumKeys(RegHive regHive, String subKey) {
            ManagementBaseObject inParams = mc.GetMethodParameters("EnumKey");
            ManagementBaseObject outParams;

            inParams["hDefKey"] = regHive;
            inParams["sSubKeyName"] = subKey;

            outParams = mc.InvokeMethod("EnumKey", inParams, null);
            String[] values = (String[])outParams.Properties["sNames"].Value;
             return values;
        }

        public String[] EnumValues(RegHive regHive, String subKey) {
            ManagementBaseObject inParams = mc.GetMethodParameters("EnumValues");
            ManagementBaseObject outParams;

            inParams["hDefKey"] = regHive;
            inParams["sSubKeyName"] = subKey;
            outParams = mc.InvokeMethod("EnumValues", inParams, null);

            return (String[])outParams.Properties["sNames"].Value;
        }
    }
}
