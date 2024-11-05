using Browser.EDF;
using EdfBrowser.App.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EdfBrowser.App.View
{
    public partial class MenuView : UserControl
    {

        private readonly MenuViewModel m_menuViewModel;

        public MenuView(MenuViewModel menuViewModel)
        {
            InitializeComponent();

            m_menuViewModel = menuViewModel;

            BackColor = System.Drawing.Color.White;

            Load += MenuView_Load;
        }

        private void MenuView_Load(object sender, System.EventArgs e)
        {
            // Create Menu Controls
            MenuStrip menuStrip = new MenuStrip();
            menuStrip.Dock = DockStyle.Fill;

            // Add Menu Items
            m_menuViewModel.LoadMenuCommand.Execute(null);
            if (m_menuViewModel.Menus.Count() == 0) return;

            foreach (var menu in m_menuViewModel.Menus)
            {
                ToolStripMenuItem rootMenuItem = new ToolStripMenuItem(menu.Description);
                if (menu.MenuItems != null)
                {
                    foreach (var item in menu.MenuItems)
                    {
                        ToolStripMenuItem subMenuItem = new ToolStripMenuItem(item.Description);
                        subMenuItem.Click += OnSubMenuItemClick;
                        rootMenuItem.DropDownItems.Add(subMenuItem);
                    }
                }

                menuStrip.Items.Add(rootMenuItem);
            }

            // Add the Menu Controls to the UserControl
            Controls.Add(menuStrip);
        }

        private void OnSubMenuItemClick(object sender, EventArgs e)
        {
            var clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null) return;

            var strategy = StrategyFactory.GetStrategy(clickedItem.Text);
            if (strategy != null)
            {
                if (strategy is OpenStrategy openStrategy)
                {
                    openStrategy.FileSelected += OnFileSelected;
                }
            }

            strategy?.Execute(); // 如果命令存在则执行
        }

        private void OnFileSelected(object sender, string path)
        {
            using (Edf edf = new Edf(path))
            {
                edf.Open();

                EdfInfo edfInfo = new EdfInfo();
                edfInfo.FilePath = path;
                EdfLibHdr hdr = edf.EdfLibHdr;
                edfInfo.Subject = hdr.patient;
                edfInfo.Recording = hdr.recording;

                DateTime startDateTime = new DateTime(hdr.startdate_year, hdr.startdate_month, hdr.startdate_day, hdr.starttime_hour, hdr.starttime_minute, hdr.starttime_second);
                edfInfo.StartDateTime = startDateTime.ToString("yyyy-MMM-dd HH:mm:ss");

                long unit = hdr.datarecord_duration / EdfLibConstants.EDFLIB_TIME_DIMENSION;
                long numSamples = hdr.datarecords_in_file;
                int second = (int)(numSamples / unit);
                TimeSpan ts = new TimeSpan(0, 0, second);

                DateTime endDateTime = startDateTime + ts;
                edfInfo.EndDateTime = endDateTime.ToString("yyyy-MMM-dd HH:mm:ss");

                edfInfo.Duration = ts.ToString();


                Dictionary<string, int> signalInfo = new Dictionary<string, int>();
                for (int i = 0; i < hdr.edfsignals; i++)
                {
                    signalInfo[hdr.signalparam[i].label] = hdr.signalparam[i].smp_in_datarecord;
                }

                edfInfo.SignalInfo = signalInfo;

                EdfDashBoardView edfDashBoardView = new EdfDashBoardView(edfInfo);
                Form form = new Form();
                form.MinimumSize = new System.Drawing.Size(600, 600);
                form.Text = "Add Signal";

                form.Controls.Add(edfDashBoardView);

                edfDashBoardView.Dock = DockStyle.Fill;
                edfDashBoardView.Show();
                form.ShowDialog();
            }
        }

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

        public interface IStrategy
        {
            void Execute();
        }

        public class OpenStrategy : IStrategy
        {
            public event EventHandler<string> FileSelected;

            public void Execute()
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "EDF files (*.edf)|*.edf";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileSelected?.Invoke(this, openFileDialog.FileName);
                    }
                }
            }
        }

        public class CloseStrategy : IStrategy
        {
            public void Execute()
            {

            }
        }

        public static class StrategyFactory
        {
            public static IStrategy GetStrategy(string description)
            {
                switch (description)
                {
                    case "Open":
                        return new OpenStrategy();
                    case "Close":
                        return new CloseStrategy();
                    // 可以添加更多的选项
                    default:
                        return null; // 默认情况
                }
            }
        }
    }
}
