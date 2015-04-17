using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace EventMonitor.Querier.Registry {
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
    abstract class RegistryQuerier : EventQuerier {
        private ManagementClass mc;

        public RegistryQuerier(EventQueue queue, EventType eventType)
            : base(queue, eventType) {
            ManagementScope scope = new ManagementScope("root\\DEFAULT");
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

        public String[] EnumKeys(RegHive regHive, String subKey) {
            ManagementBaseObject inParams = mc.GetMethodParameters("EnumKey");
            ManagementBaseObject outParams;

            inParams["hDefKey"] = regHive;
            inParams["sSubKeyName"] = subKey;

            outParams = mc.InvokeMethod("EnumKey", inParams, null);
            return (String[])outParams.Properties["sNames"].Value;
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
