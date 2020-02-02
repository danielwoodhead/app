using System;
using System.Globalization;
using System.Reflection;
using MyApp.Services.Api;
using MyApp.Services.Navigation;
using MyApp.Services.Notes;
using MyApp.Services.Settings;
using TinyIoC;
using Xamarin.Forms;

namespace MyApp.ViewModels.Base
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
            _container = new TinyIoCContainer();

            // View models - by default, TinyIoC will register concrete classes as multi-instance.
            _container.Register<MainViewModel>();
            _container.Register<NotesViewModel>();

            // Services - by default, TinyIoC will register interface registrations as singletons.
            _container.Register<IApiClient, ApiClient>();
            _container.Register<INavigationService, NavigationService>();
            _container.Register<INotesService, NotesService>();
            _container.Register<ISettingsService, SettingsService>();
        }

        public static void UpdateDependencies(bool useMockServices)
        {
            if (useMockServices)
            {
                _container.Register<INotesService, NotesMockService>();
            }
            else
            {
                _container.Register<INotesService, NotesService>();
            }
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
