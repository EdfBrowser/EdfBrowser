using System.Collections.Generic;

namespace EdfBrowser.Models
{
    public class EdfInfo
    {
        private string m_filePath;
        private string m_subject;
        private string m_recording;
        private string m_startDateTime;
        private string m_endDateTime;
        private string m_duration;

        private Dictionary<string, int> m_signalInfo;

        public string FilePath { get => m_filePath; set => m_filePath = value; }
        public string Subject { get => m_subject; set => m_subject = value; }
        public string Recording { get => m_recording; set => m_recording = value; }
        public string StartDateTime { get => m_startDateTime; set => m_startDateTime = value; }
        public string EndDateTime { get => m_endDateTime; set => m_endDateTime = value; }
        public string Duration { get => m_duration; set => m_duration = value; }
        public Dictionary<string, int> SignalInfo { get => m_signalInfo; set => m_signalInfo = value; }
    }
}
