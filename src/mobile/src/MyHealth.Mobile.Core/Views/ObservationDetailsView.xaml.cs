using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealth.Mobile.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObservationDetailsView : ContentPage
    {
        public ObservationDetailsView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ObservationEditor.Focus();
            base.OnAppearing();
        }
    }
}
