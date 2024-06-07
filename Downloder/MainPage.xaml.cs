using System;
using Xamarin.Forms;

namespace Downloder
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (await DependencyService.Get<IFileService>().SaveFileToLocalStorage())
            {
                await DisplayAlert("", "Successfully downloded", "OK");
            }
            else
            {
                await DisplayAlert("", "Downlod failed", "OK");
            }
        }
    }
}
