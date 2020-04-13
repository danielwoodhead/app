using System;
using System.Globalization;
using System.Reflection;
using MyHealth.Mobile.Core.Services.Http;
using MyHealth.Mobile.Core.Services.Identity;
using MyHealth.Mobile.Core.Services.Navigation;
using MyHealth.Mobile.Core.Services.Observations;
using MyHealth.Mobile.Core.Services.Settings;
using MyHealth.Mobile.Core.Services.SystemTime;
using TinyIoC;
using Xamarin.Forms;

namespace MyHealth.Mobile.Core.ViewModels.Base
{
    public static class ViewModelLocator
    {
        private static TinyIoCContainer _container;

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(ViewModelLocator.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(ViewModelLocator.AutoWireViewModelProperty, value);
        }

        static ViewModelLocator()
        {
            _container = TinyIoCContainer.Current;

            // View models - by default, TinyIoC will register concrete classes as multi-instance.
            _container.Register<MainViewModel>();
            _container.Register<ObservationsViewModel>();

            // Services - by default, TinyIoC will register interface registrations as singletons.
            _container.Register<IHttpClientFactory, HttpClientFactory>();
            _container.Register<IIdentityService, IdentityService>();
            _container.Register<INavigationService, NavigationService>();
            _container.Register<IObservationsService, ObservationsService>();
            _container.Register<ISettingsService, SettingsService>();
            _container.Register<ISystemTimeService, SystemTimeService>();

            _container.Register<UserAccessTokenHandler>();
        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;
            if (view == null)
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }
            var viewModel = _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}
