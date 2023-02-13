﻿using AntDesign.Sample.ViewModels;
using AntDesign.Sample.Views;

namespace AntDesign.Sample.Routers;
public class MainRoutingViewLocator : IMainRoutingViewLocator
{
    public MainRoutingViewLocator(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
        _router = new();
        _mapViewModelViews = new();
        _mapRoutingTokenCallBack = new();
        _mapRoutingViewViewModels = new();
    }

    readonly IServiceCollection _serviceCollection;
    readonly RoutingState _router;

    bool _isMake = false;
    IScreen _screen = default!;

    readonly Dictionary<Type, Type> _mapViewModelViews;
    readonly Dictionary<string, (Type view, Type viewModel)> _mapRoutingViewViewModels;
    readonly Dictionary<string, Func<string>?> _mapRoutingTokenCallBack;

    public RoutingState Make(IScreen screen)
    {
        _isMake = true;
        _screen = screen;
        return _router;
    }

    public bool AddRouter<TView, TViewModel>(string token = nameof(TViewModel), Func<string>? routerNameCallBack = default)
        where TView : IViewFor
        where TViewModel : ViewModelRoutableBase
    {
        _serviceCollection.AddScoped<OverviewView>();
        _serviceCollection.AddSingleton<OverviewViewModel>();

        _mapViewModelViews.TryAdd(typeof(OverviewView), typeof(OverviewViewModel));
        _mapRoutingTokenCallBack.TryAdd(token, routerNameCallBack);
        _mapRoutingViewViewModels.TryAdd(token, (typeof(OverviewView), typeof(OverviewViewModel)));

        return true;
    }

    public bool Navigate(string token)
    {
        if (!_isMake)
            return false;

        if (!_mapRoutingViewViewModels.TryGetValue(token, out var view_viewModel))
            return false;

        if (view_viewModel.viewModel is null)
            return false;

        var serviceProvider = _serviceCollection.BuildServiceProvider();
        var viewModel = serviceProvider.GetRequiredService(view_viewModel.viewModel);
        if (viewModel is not ViewModelRoutableBase routableViewModel)
            return false;

        routableViewModel.HostScreen = _screen;
        _router.Navigate.Execute(routableViewModel);
        return true;
    }

    public bool NavigateWithView<TView>() where TView : IViewFor
    {
        if (!_isMake)
            return false;

        var viewModelType = _mapViewModelViews.FirstOrDefault(pair => pair.Value == typeof(TView));
        if (viewModelType.Key is null)
            return false;

        var serviceProvider = _serviceCollection.BuildServiceProvider();
        var viewModel = serviceProvider.GetRequiredService(viewModelType.Key);
        if (viewModel is not ViewModelRoutableBase routableViewModel)
            return false;

        routableViewModel.HostScreen = _screen;
        _router.Navigate.Execute(routableViewModel);
        return true;
    }

    public bool NavigateWithViewModel<TViewModel>() where TViewModel : ViewModelRoutableBase
    {
        if (!_isMake)
            return false;

        var serviceProvider = _serviceCollection.BuildServiceProvider();
        var viewModel = serviceProvider.GetRequiredService<TViewModel>();

        viewModel.HostScreen = _screen;
        _router.Navigate.Execute(viewModel);
        return true;
    }

    public bool GoBack()
    {
        _router.NavigateBack.Execute();
        return true;
    }

    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
    {
        if (viewModel is null)
            return default;

        var serviceProvider = _serviceCollection.BuildServiceProvider();

        IViewFor? view = default;
        switch (viewModel.GetType().Name)
        {
            case nameof(OverviewViewModel):
                view = serviceProvider.GetService<OverviewView>();
                break;
            default:
                break;
        }

        if (view is not null)
            view.ViewModel = viewModel;

        return view;
    }

    public ObservableCollection<Router> Routers()
    {
        var collection = new ObservableCollection<Router>();
        foreach (var viewModel in _mapRoutingViewViewModels)
        {
            var pair = _mapRoutingTokenCallBack.FirstOrDefault(pair => pair.Key == viewModel.Key);
            var func = pair.Value;
            var router = new Router(viewModel.Key, func)
            {
                ViewType = viewModel.Value.view,
                ViewModelType = viewModel.Value.viewModel,
            };

            collection.Add(router);
        }

        return collection;
    }
}
