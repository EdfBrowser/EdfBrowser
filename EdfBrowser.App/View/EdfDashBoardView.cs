using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EdfBrowser.App.View
{
    public partial class EdfDashBoardView : UserControl
    {
        private MenuView.EdfInfo m_edfInfo;

        public EdfDashBoardView(MenuView.EdfInfo edfInfo)
        {
            InitializeComponent();

            m_edfInfo = edfInfo;

            Load += EdfDashBoardView_Load;
        }

        private void EdfDashBoardView_Load(object sender, System.EventArgs e)
        {
            AddControls();
        }

        private void AddControls()
        {
            Controls.Add(CreateButtonPanel());
            Controls.Add(CreateSignalInfoPanel());
            Controls.Add(CreateInfoPanel());
            Controls.Add(CreateLabelPanel(m_edfInfo.FilePath));
        }

        private Panel CreateLabelPanel(string text)
        {
            var label = CreateLabel(text);
            label.Dock = DockStyle.Fill;
            label.BackColor = Color.White;
            label.BorderStyle = BorderStyle.FixedSingle;

            var panel = new Panel();
            panel.Dock = DockStyle.Top;
            panel.Controls.Add(label);
            return panel;
        }

        private Panel CreateSignalInfoPanel()
        {
            Panel container = new Panel();
            container.Dock = DockStyle.Fill;

            GroupBox groupBox = new GroupBox();
            groupBox.Text = "Signals";
            groupBox.Dock = DockStyle.Fill;

            ListBox signalListBox = CreateSignalListBox();
            groupBox.Controls.Add(signalListBox);

            container.Controls.Add(groupBox);
            return container;
        }
        private ListBox CreateSignalListBox()
        {
            ListBox signalListBox = new ListBox();
            signalListBox.Dock = DockStyle.Fill;

            signalListBox.SelectionMode = SelectionMode.MultiExtended;

            signalListBox.BeginUpdate();

            int i = 1;
            StringBuilder sbuilder = new StringBuilder();

            foreach (var item in m_edfInfo.SignalInfo)
            {
                signalListBox.Items.Add(FormatSignalInfo(i++, item));
            }

            signalListBox.EndUpdate();


            return signalListBox;
        }

        private string FormatSignalInfo(int index, KeyValuePair<string, int> item)
        {
            return string.Format("{0,-4}{1,-20}{2,5}HZ", index, item.Key, item.Value);
        }

        private FlowLayoutPanel CreateButtonPanel()
        {
            FlowLayoutPanel container = new FlowLayoutPanel();
            container.Dock = DockStyle.Bottom;
            container.FlowDirection = FlowDirection.LeftToRight;

            Button selectedButton = new Button();
            selectedButton.Text = "Selected";

            container.Controls.Add(selectedButton);

            return container;
        }


        private Label CreateLabel(string text, bool autosize = false)
        {
            Label label = new Label();
            label.Text = text;
            label.AutoSize = autosize;
            if (autosize)
            {
                label.Size = CreateGraphics().MeasureString(text, Font).ToSize();
            }

            return label;
        }

        private Panel CreateInfoPanel()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"Subject: ", m_edfInfo.Subject },
                {"Recording: ", m_edfInfo.Recording },
                {"Start Time: ", m_edfInfo.StartDateTime },
                {"End Time: " , m_edfInfo.EndDateTime},
                {"Duration: ", m_edfInfo.Duration },
            };

            Panel panel = new Panel();
            panel.Dock = DockStyle.Top;

            int lastHeight = 0;
            foreach (var entry in dict)
            {
                AddLabelPairToPanel(panel, entry.Key, entry.Value, ref lastHeight);
            };

            panel.Height = lastHeight;

            return panel;
        }

        private void AddLabelPairToPanel(Panel panel, string key, string value, ref int lastHeight)
        {
            var keyLabel = CreateLabel(key);
            keyLabel.Location = new Point(0, lastHeight);

            var valueLabel = CreateLabel(value, true);
            valueLabel.Location = new Point(keyLabel.Width, lastHeight);

            panel.Controls.Add(keyLabel);
            panel.Controls.Add(valueLabel);

            lastHeight += keyLabel.Height;
        }
    }
}
