using System;
using System.Runtime.InteropServices;

namespace EdfBrowser.EdfParser
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct HeaderInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)]
        public char[] _patientID;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)]
        public char[] _recordingID;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public char[] _startDate;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public char[] _startTime;


        [MarshalAs(UnmanagedType.U4)]
        public uint _recordCount;

        [MarshalAs(UnmanagedType.R8)]
        public double _recordDuration;

        [MarshalAs(UnmanagedType.U4)]
        public uint _signalCount;

        public SignalInfo[] _signals;
    }
}

