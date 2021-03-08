using GamesApp.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GamesApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DIContainer.RegisterDependencies();

            Akavache.Registrations.Start("GamesApp");

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
