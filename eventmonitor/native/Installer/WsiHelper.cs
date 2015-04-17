using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace EventMonitor.Native.Installer {
    /// <summary>
    /// Windows installer helper.
    /// </summary>
    /// <see cref="https://msdn.microsoft.com/en-us/library/aa369426(v=vs.85).aspx"/>
    class WsiHelper {
        [Flags]
        public enum MSIINSTALLCONTEXT : uint {
            MSIINSTALLCONTEXT_NONE = 0,
            MSIINSTALLCONTEXT_USERMANAGED = 1,
            MSIINSTALLCONTEXT_USERUNMANAGED = 2,
            MSIINSTALLCONTEXT_MACHINE = 4,
            MSIINSTALLCONTEXT_ALL = (MSIINSTALLCONTEXT_USERMANAGED | MSIINSTALLCONTEXT_USERUNMANAGED | MSIINSTALLCONTEXT_MACHINE),
        }

        public const string MSI_DLL = "msi.dll";

        [DllImport(MSI_DLL, SetLastError = true)]
        public static extern int MsiEnumProducts(int iProductIndex, StringBuilder lpProductBuf);

        [DllImport(MSI_DLL, EntryPoint = "MsiEnumProductsExW", CharSet = CharSet.Unicode,
           ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern uint MsiEnumProductsEx(
                   string szProductCode,
                   string szUserSid,
                   uint dwContext,
                   uint dwIndex,
                   string szInstalledProductCode,
                   out object pdwInstalledProductContext,
                   string szSid,
                   ref uint pccSid);

        [DllImport(MSI_DLL, CharSet = CharSet.Unicode)]
        public static extern Int32 MsiGetProductInfo(string product, string property, [Out] StringBuilder valueBuf, ref Int32 len);

        [DllImport(MSI_DLL, CharSet = CharSet.Unicode)]
        public static extern uint MsiEnumPatches(
           [In] String szProduct,
           [In] uint iPatchIndex,
           [Out] StringBuilder lpPatchBuf,
           [Out] StringBuilder lpTransformsBuf,
           ref uint pcchTransformsBuf
        );

        [DllImport(MSI_DLL, EntryPoint = "MsiEnumPatchesExW",
             CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        private static extern uint MsiEnumPatchesEx(string szProductCode,
            string szUserSid,
            uint dwContext,
            uint dwFilter,
            uint dwIndex,
            StringBuilder szPatchCode,
            StringBuilder szTargetProductCode,
            out object pdwTargetProductContext,
            StringBuilder szTargetUserSid,
            ref uint pcchTargetUserSid);

        [DllImport(MSI_DLL, SetLastError = true)]
        public static extern uint MsiGetPatchInfo(String szPatch, String szAttribute, [Out] StringBuilder lpValue, ref uint pcchValue);

        [DllImport(MSI_DLL, SetLastError = true)]
        static extern uint MsiGetPatchInfoEx(
            String szPatchCode,
            String szProductCode,
            String szUserSid, //pass 'null' to omit.
            MSIINSTALLCONTEXT dwContext,
            String szProperty,
            [Out] StringBuilder lpValue,
            ref uint pcchValue
        );

    }
}
