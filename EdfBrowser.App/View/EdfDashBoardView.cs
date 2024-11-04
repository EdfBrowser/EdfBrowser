using System.Drawing;
using System.Windows.Forms;

namespace EdfBrowser.App.View
{
    public partial class EdfDashBoardView : UserControl
    {
        public EdfDashBoardView()
        {
            InitializeComponent();

            TableLayoutPanel mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.RowCount = 3;
            mainPanel.ColumnCount = 1;
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            ListBox pathPanel = CreatePathPanel();
            mainPanel.Controls.Add(pathPanel, 0, 0);

            TableLayoutPanel infoPanel = CreateInfoPanel();
            mainPanel.Controls.Add(infoPanel, 0, 1);

            TableLayoutPanel operatePanel = CreateOperatePanel();
            mainPanel.Controls.Add(operatePanel, 0, 2);

            Controls.Add(mainPanel);
        }

        private TableLayoutPanel CreateOperatePanel()
        {
            TableLayoutPanel operatePanel = new TableLayoutPanel();
            operatePanel.Dock = DockStyle.Fill;
            operatePanel.ColumnCount = 3;
            operatePanel.RowCount = 1;

            // 设置列宽
            operatePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));  // 左侧列表
            operatePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100)); // 中间按钮列，固定宽度
            operatePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));  // 右侧列表

            // 左侧列表 (ListBox 或 ListView)
            ListBox leftListBox = new ListBox();
            leftListBox.Dock = DockStyle.Fill;
            operatePanel.Controls.Add(leftListBox, 0, 0);

            // 中间按钮
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.FlowDirection = FlowDirection.TopDown;

            // 添加按钮
            Button addButton = new Button { Text = "Add->", Width = 80 };
            Button subtractButton = new Button { Text = "Subtract->", Width = 80 };
            Button removeButton = new Button { Text = "Remove<-", Width = 80 };
            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(subtractButton);
            buttonPanel.Controls.Add(removeButton);
            operatePanel.Controls.Add(buttonPanel, 1, 0);

            // 右侧列表 (ListBox 或 ListView)
            ListBox rightListBox = new ListBox();
            rightListBox.Dock = DockStyle.Fill;
            operatePanel.Controls.Add(rightListBox, 2, 0);
            return operatePanel;
        }

        private TableLayoutPanel CreateInfoPanel()
        {
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.RowCount = 3; // 根据实际信息项数量
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150)); // 第一列宽度
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); // 第二列占满

            tableLayoutPanel.Controls.Add(new Label { Text = "Subject:" }, 0, 0);
            tableLayoutPanel.Controls.Add(new Label { Text = "X M 30-OCT-2021", AutoSize = true }, 1, 0);

            tableLayoutPanel.Controls.Add(new Label { Text = "Recording:" }, 0, 1);
            tableLayoutPanel.Controls.Add(new Label { Text = "Startdate 31-OCT-2021...", AutoSize = true }, 1, 1);

            tableLayoutPanel.Controls.Add(new Label { Text = "Start / End / Duration:" }, 0, 2);
            tableLayoutPanel.Controls.Add(new Label { Text = "7:39:01 / 8:53:39 / 1:14:38", AutoSize = true }, 1, 2);
            return tableLayoutPanel;
        }

        private ListBox CreatePathPanel()
        {
            // Top Panel to display edf file path.
            ListBox listbox = new ListBox();
            listbox.Dock = DockStyle.Fill;
            listbox.BackColor = Color.White;
            listbox.BorderStyle = BorderStyle.FixedSingle;

            return listbox;
        }
    }
}
