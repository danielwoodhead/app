﻿using MyHealth.Mobile.Core.ViewModels;
using MyHealth.Mobile.Core.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealth.Mobile.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : TabbedPage
    {
        public MainView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<MainViewModel, int>(this, MessageKeys.ChangeTab, (sender, arg) =>
            {
                switch (arg)
                {
                    case 0:
                        CurrentPage = ObservationsView;
                        break;
                    //case 1:
                    //    CurrentPage = ProfileView;
                    //    break;
                    //case 2:
                    //    CurrentPage = BasketView;
                    //    break;
                    //case 3:
                    //    CurrentPage = CampaignView;
                    //    break;
                }
            });

            await ((ObservationsViewModel)ObservationsView.BindingContext).InitializeAsync(null);
            //await ((BasketViewModel)BasketView.BindingContext).InitializeAsync(null);
            //await ((ProfileViewModel)ProfileView.BindingContext).InitializeAsync(null);
            //await ((CampaignViewModel)CampaignView.BindingContext).InitializeAsync(null);
        }
    }
}