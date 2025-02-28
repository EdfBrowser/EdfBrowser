using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace EdfBrowser.App
{
    // 扩展方法：将命令绑定到控件事件
    public static class CommandExtensions
    {
        public static void BindCommand(this Control control, ICommand command, EventHandler eventHandler,
            Func<object> parameterProvider = null)
        {
            eventHandler += (s, e) =>
            {
                var param = parameterProvider?.Invoke() ?? e;
                if (command.CanExecute(param))
                {
                    command.Execute(param);
                }
            };

            // 动态更新控件启用状态
            command.CanExecuteChanged += (s, e) =>
            {
                control.Enabled = command.CanExecute(parameterProvider?.Invoke());
            };
            // 初始状态
            control.Enabled = command.CanExecute(parameterProvider?.Invoke());
        }
    }
}
