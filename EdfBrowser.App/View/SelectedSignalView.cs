using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal partial class SignalListView : UserControl
    {
        private class BindableListView : ListView
        {
            private class CollectionChangedManager
            {
                private readonly BindableListView _owner;

                internal CollectionChangedManager(BindableListView owner)
                {
                    _owner = owner;
                }

                internal void Subscribe()
                {
                    if (_owner.DataSource is INotifyCollectionChanged notifyCollection)
                    {
                        notifyCollection.CollectionChanged += OnCollectionChanged;
                    }
                }

                internal void UnSubscribe()
                {
                    if (_owner.DataSource is INotifyCollectionChanged notifyCollection)
                    {
                        notifyCollection.CollectionChanged -= OnCollectionChanged;
                    }

                    _owner.DataSource = null;
                }

                private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
                {
                    if (_owner.InvokeRequired)
                    {
                        _owner.Invoke(new Action(() => OnCollectionChanged(sender, e)));
                        return;
                    }

                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            foreach (object newItem in e.NewItems)
                                _owner.AddListViewItem(newItem);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            foreach (object oldItem in e.OldItems)
                                _owner.RemoveListViewItem(oldItem);
                            break;
                        case NotifyCollectionChangedAction.Reset:
                        case NotifyCollectionChangedAction.Move:
                            _owner.BindData(); // 重新绑定所有数据
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            //foreach (object newItem in e.NewItems)
                            //    UpdateListViewItem(e.NewStartingIndex + , e.NewItems[0]); // 只更新修改的行
                            break;
                    }
                }
            }

            private readonly CollectionChangedManager _manager;
            private object _dataSource;

            internal BindableListView()
            {
                _manager = new CollectionChangedManager(this);

                View = View.Details;
                FullRowSelect = true;
            }

            internal object DataSource
            {
                get { return _dataSource; }
                set
                {
                    if (value != _dataSource)
                    {
                        _manager.UnSubscribe();
                        _dataSource = value;
                        BindData();
                        _manager.Subscribe();
                    }
                }
            }

            private void RemoveListViewItem(object oldItem)
            {
                foreach (ListViewItem listViewItem in Items)
                {
                    // 比较 ListViewItem 的 Tag 属性与 item，如果匹配，则移除
                    if (Equals(listViewItem.Tag, oldItem))
                    {
                        Items.Remove(listViewItem);
                        break;
                    }
                }
            }

            private void UpdateListViewItem(int index, object item)
            {
                if (index < 0 || index >= Items.Count)
                    return;

                var listViewItem = Items[index];
                for (int i = 0; i < Columns.Count; i++)
                {
                    string columnName = Columns[i].Text;

                    if (columnName == "Index")
                    {
                        listViewItem.SubItems[i].Text = (index + 1).ToString();
                    }
                    else
                    {
                        var propInfo = item.GetType().GetProperty(columnName,
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        listViewItem.SubItems[i].Text = propInfo?.GetValue(item)?.ToString() ?? "";
                    }
                }
            }

            private void AddListViewItem(object item)
            {
                if (Columns.Count == 0)
                    return;

                string[] values = new string[Columns.Count];

                for (int i = 0; i < values.Length; i++)
                {
                    string columnName = Columns[i].Text;

                    if (columnName == "Index")
                    {
                        values[i] = (Items.Count + 1).ToString();
                        continue;
                    }

                    PropertyInfo prop = item.GetType().GetProperty(columnName,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    values[i] = prop?.GetValue(item)?.ToString() ?? "";
                }

                ListViewItem viewItem = new ListViewItem(values);
                viewItem.Tag = item;

                Items.Add(viewItem);
            }

            private void BindData()
            {
                Items.Clear();
                if (_dataSource is IEnumerable list)
                {
                    foreach (object item in list)
                    {
                        AddListViewItem(item);
                    }
                }

            }
        }

        private readonly SignalListViewModel _selectedSignalViewModel;

        private readonly TableLayoutPanel _mainLayout;
        private readonly FlowLayoutPanel _operatorLayout;

        private readonly BindableListView _allListView;
        private readonly BindableListView _selectedListView;

        private readonly Button _addButton;
        private readonly Button _removeButton;

        private readonly Panel _buttonPanel;

        private readonly Button _completeButton;

        internal SignalListView(SignalListViewModel selectedSignalViewModel)
        {
            InitializeComponent();

            _selectedSignalViewModel = selectedSignalViewModel;

            _mainLayout = new TableLayoutPanel();
            _mainLayout.RowCount = 2;
            _mainLayout.ColumnCount = 3;
            _mainLayout.Dock = DockStyle.Fill;

            _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            _operatorLayout = new FlowLayoutPanel();
            _operatorLayout.FlowDirection = FlowDirection.TopDown;
            _operatorLayout.WrapContents = false;
            _operatorLayout.AutoSize = true;
            //_operatorLayout.Padding = new Padding(0, _mainLayout.Height / 2, 0, _mainLayout.Height / 2);
            _operatorLayout.Dock = DockStyle.Fill;

            _allListView = new BindableListView();
            _allListView.Dock = DockStyle.Fill;
            _allListView.View = View.Details;
            _allListView.FullRowSelect = true;
            _allListView.GridLines = true;
            _allListView.Columns.Clear();
            _allListView.Columns.Add("Index", 50, HorizontalAlignment.Left);
            _allListView.Columns.Add("Label", 200, HorizontalAlignment.Left);
            _allListView.Columns.Add("SampleRate", 200, HorizontalAlignment.Left);
            _allListView.DataSource = _selectedSignalViewModel.SignalItems;

            _selectedListView = new BindableListView();
            _selectedListView.Dock = DockStyle.Fill;
            _selectedListView.View = View.Details;
            _selectedListView.FullRowSelect = true;
            _selectedListView.GridLines = true;
            _selectedListView.Columns.Clear();
            _selectedListView.Columns.Add("Index", 50, HorizontalAlignment.Left);
            _selectedListView.Columns.Add("Label", 200, HorizontalAlignment.Left);
            _selectedListView.Columns.Add("SampleRate", 200, HorizontalAlignment.Left);
            _selectedListView.DataSource = _selectedSignalViewModel.SelectedSignalItems;

            _addButton = new Button();
            _addButton.AutoSize = true;
            _addButton.Text = "Add->";
            _addButton.Click += OnAddSignal;
            _removeButton = new Button();
            _removeButton.AutoSize = true;
            _removeButton.Text = "<-Remove";
            _removeButton.Click += OnRemoveSignal;

            _operatorLayout.Controls.Add(_addButton);
            _operatorLayout.Controls.Add(_removeButton);


            _buttonPanel = new Panel();
            _buttonPanel.AutoSize = true;
            _buttonPanel.Dock = DockStyle.Fill;

            _completeButton = new Button();
            _completeButton.AutoSize = true;
            _completeButton.Text = "Complete";
            _completeButton.Click += OnCompleted;

            _buttonPanel.Controls.Add(_completeButton);


            _mainLayout.Controls.Add(_allListView, 0, 0);
            _mainLayout.Controls.Add(_operatorLayout, 1, 0);
            _mainLayout.Controls.Add(_selectedListView, 2, 0);
            _mainLayout.Controls.Add(_buttonPanel, 2, 1);


            Controls.Add(_mainLayout);
        }

        private void OnAddSignal(object sender, EventArgs e)
        {
            // 获取所有选中的项
            var selectedItems = _allListView.SelectedItems
                .Cast<ListViewItem>()
                .Select(item => item.Tag as SignalItem)
                .Where(item => item != null)
                .ToList();

            if (selectedItems.Any())
            {
                // 一次性将所有选中的项添加到视图模型
                _selectedSignalViewModel.AddSignalCommand.Execute(selectedItems);
            }
        }

        private void OnRemoveSignal(object sender, EventArgs e)
        {
            // 获取所有选中的项
            var selectedItems = _selectedListView.SelectedItems
                .Cast<ListViewItem>()
                .Select(item => item.Tag as SignalItem)
                .Where(item => item != null)
                .ToList();

            if (selectedItems.Any())
            {
                // 一次性将所有选中的项添加到视图模型
                _selectedSignalViewModel.RemoveSignalCommand.Execute(selectedItems);
            }
        }


        private void OnCompleted(object sender, EventArgs e)
        {
            _selectedSignalViewModel.CompletedCommand.Execute(null);
        }
    }
}
