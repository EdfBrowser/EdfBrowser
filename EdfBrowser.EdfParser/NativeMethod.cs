using System;
using System.Runtime.InteropServices;

namespace EdfBrowser.EdfParser
{
    internal static class NativeMethod
    {
    
        [DllImport("../edflib", EntryPoint = "edf_open", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr EdfOpen(
            string filepath);

        [DllImport("../edflib", EntryPoint = "edf_close", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int EdfClose(
            IntPtr handle);

        [DllImport("../edflib", EntryPoint = "edf_read_header", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int EdfReadHeader(
            IntPtr handle,
        IntPtr ptr);

        [DllImport("../edflib", EntryPoint = "edf_read_signal_data", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int EdfReadSignalData(
            IntPtr hadnle,
        IntPtr ptr,
        uint signalIndex,
        uint startRecord = 0,
        uint recordCount = 0);

    }
}

