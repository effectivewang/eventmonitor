using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace EventMonitor.Native.SecurityCenter
{
    /// <summary>
    /// Windows Security Center Helper.
    /// </summary>
    /// <see ref="https://msdn.microsoft.com/en-us/library/bb432507%28v=VS.85%29.aspx?f=255&MSPPError=-2147217396"/>
    class WscHelper
    {
        public static readonly int NOT_RUNNING = 1;

        public static string GetStatus(int result) {
            return result == NOT_RUNNING ? "Not Running" : "Running";
        }

        /// <summary>
        /// </summary>
        /// <param name="reserved"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        /// <remarks>
        /// HRESULT WINAPI WscRegisterForChanges(
        /// _In_   LPVOID Reserved,
        /// _Out_  PHANDLE phCallbackRegistration,
        /// _In_   LPTHREAD_START_ROUTINE lpCallbackAddress,
        /// _In_   PVOID pContext
        /// );
        /// </remarks>
        [DllImport("wscapi.dll", EntryPoint = "WscGetSecurityProviderHealth", CallingConvention=CallingConvention.StdCall)]
        public static extern int GetSecurityProviderHealth([In] WSC_SECURITY_PROVIDER provider,
                                                    [Out] out WSC_SECURITY_PROVIDER_HEALTH health);
    }
}
