using GamesApp.Models;
using GamesApp.Services;
using GamesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GamesApp
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = DIContainer.Resolve<MainPageViewModel>();
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((CollectionView)sender).SelectedItem != null)
            {
                Game selectedItem = ((CollectionView)sender).SelectedItem as Game;
                await Browser.OpenAsync(selectedItem.UrlLink, BrowserLaunchMode.SystemPreferred);

                ((CollectionView)sender).SelectedItem = null;
            }
        }

        protected override void OnAppearing()
        {
            if (_viewModel.LoadDataCommand.CanExecute(null))
                _viewModel.LoadDataCommand.Execute(null);

            base.OnAppearing();
        }

    }
}
