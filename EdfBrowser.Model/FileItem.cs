using System;

namespace EdfBrowser.Model
{
    // 数据模型
    public class FileItem
    {
        public FileItem(string title, string subtitle, DateTime time)
        {
            Title = title;
            Subtitle = subtitle;
            Time = time;
        }

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Path => Subtitle + Title;
        public DateTime Time { get; set; }
    }
}
