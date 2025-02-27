using System;
using System.Runtime.InteropServices;

namespace EdfBrowser.EdfParser
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EdfInfo
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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public SignalInfo[] _signals;

        public DateTime StartDateTime { get; internal set; }

        internal DateTime ToDT(string date, string time)
        {
            string[] dateParts = date.Split('.');
            int day = Convert.ToInt32(dateParts[0]);
            int month = Convert.ToInt32(dateParts[1]);
            int year = Convert.ToInt32(dateParts[2]);
            year = year > 84 ? year + 1900 : year + 2000;

            string[] timeParts = time.Split('.');
            int hour = Convert.ToInt32(timeParts[0]);
            int minute = Convert.ToInt32(timeParts[1]);
            int second = Convert.ToInt32(timeParts[2]);

            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}

