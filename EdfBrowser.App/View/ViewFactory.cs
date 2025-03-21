using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal class ViewFactory
    {
        private readonly Dictionary<Type, Type> _viewModelToViewMapping = new Dictionary<Type, Type>
        {
            { typeof(FileViewModel), typeof(FileView)},
            { typeof(SignalListViewModel), typeof(SignalListView)},
            { typeof(EdfPlotViewModel), typeof(EdfPlotView) }
        };

        private readonly IServiceScope _scope;

        public ViewFactory(IServiceScopeFactory factory)
        {
            _scope = factory.CreateScope();
        }

        internal BaseView CreateView(BaseViewModel vm)
        {
            Type currentViewModelType = vm?.GetType();

            if (currentViewModelType != null && _viewModelToViewMapping.ContainsKey(currentViewModelType))
            {
                Type viewType = _viewModelToViewMapping[currentViewModelType];

                if (_scope.ServiceProvider.GetRequiredService(viewType) is BaseView view)
                {
                    view.DataContext = vm;

                    if (view is EdfPlotView view2)
                        view2.InitializeChildrens();

                    return view;
                }
            }

            return null;
        }

        internal void Dispose() => _scope?.Dispose();

    }
}
