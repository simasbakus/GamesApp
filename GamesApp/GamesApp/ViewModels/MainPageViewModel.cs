using Akavache;
using GamesApp.Models;
using GamesApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        /* ------------------ Variables ------------------ */
        private readonly IRepositoryGames _repository;

        public bool ShowMonth { get; set; } = false;
        public bool ShowFilter { get; set; } = false;
        private DateTime CurMonth { get; set; }
        public string CurMonthStr { get; set; }
        public string ActiveFilters { get; set; }
        public bool ActiveFiltersVisible { get; set; } = false;
        public bool IsRefreshing { get; set; }

        bool _IsLoading { get; set; }
        public bool IsLoading
        {
            get => _IsLoading;
            set
            {
                _IsLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        List<Game> _Games { get; set; }
        public List<Game> Games
        {
            get => _Games;
            set
            {
                _Games = value;
                OnPropertyChanged(nameof(Games));
            }
        }

        public List<Division> Divisions { get; set; } = new List<Division>
        {
            new Division("NLRL", false, "Lietuvos \u010Dempionatas"),
            new Division("BHL", false, "Baltijos ledo ritulio lyga - D. Kasparai\u010Dio grup\u0117"),
            new Division("U18", false, "Jaunimo lyga"),
            new Division("U15", false, "U15"),
            new Division("U13", false, "U13"),
            new Division("U11", false, "U11 A, U11 B")
        };

        /* ------------------ Commands ------------------ */
        public IAsyncCommand ShowAllCommand { get; }
        public IAsyncCommand<string> ChangeMonthCommand { get; }
        public IAsyncCommand FilterBtnCommand { get; }
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand LoadDataCommand { get; }
        public IAsyncCommand CheckTokenCommand { get; }



        public MainPageViewModel(IRepositoryGames repository)
        {
            _repository = repository;
            Games = new List<Game>();
            ShowAllCommand = new AsyncCommand(ToggleShowAll, allowsMultipleExecutions: false);
            ChangeMonthCommand = new AsyncCommand<string>((newMonth) => ChangeMonth(newMonth), allowsMultipleExecutions: false);
            FilterBtnCommand = new AsyncCommand(FilterBtnPressed, allowsMultipleExecutions: false);
            RefreshCommand = new AsyncCommand(ExecuteRefresh, allowsMultipleExecutions: false);
            LoadDataCommand = new AsyncCommand(LoadInitialData, allowsMultipleExecutions: false);
            CheckTokenCommand = new AsyncCommand(CheckToken, allowsMultipleExecutions: false);

        }

        private async Task ToggleShowAll()
        {
            IsLoading = true;
            ShowMonth = !ShowMonth;
            CurMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            CurMonthStr = CurMonth.ToString("MMM");

            try
            {
                if (ShowMonth)
                    Games = await _repository.GetMonthGames(CurMonth.ToString("yyyy-MM"), Divisions);
                else
                    Games = await _repository.GetGames(Divisions);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepetion caught: " + e.Message);
                string message = "Something went wrong!";
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    message = "No internet connection!";
                await App.Current.MainPage.DisplayAlert("Alert", message, "Ok");
            }

            IsLoading = false;
            OnPropertyChanged(nameof(ShowMonth));
        }

        private async Task ChangeMonth(string newMonth)
        {
            IsLoading = true;
            CurMonth = CurMonth.AddMonths(int.Parse(newMonth));
            CurMonthStr = CurMonth.ToString("MMM");

            try
            {
                Games = await _repository.GetMonthGames(CurMonth.ToString("yyyy-MM"), Divisions);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepetion caught: " + e.Message);
                string message = "Something went wrong!";
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    message = "No internet connection!";
                await App.Current.MainPage.DisplayAlert("Alert", message, "Ok");
            }

            IsLoading = false;
            OnPropertyChanged(nameof(CurMonthStr));
        }

        private async Task FilterBtnPressed()
        {
            if (ShowFilter == true)
            {
                IsLoading = true;

                if (Divisions.Find(d => d.IsChecked == false) == null)
                {
                    //Check if all filter options ar checked, if so - all values are reset to initial ones
                    foreach (Division division in Divisions)
                    {
                        division.IsChecked = false;
                    }
                }

                try
                {
                    if (ShowMonth)
                        Games = await _repository.GetMonthGames(CurMonth.ToString("yyyy-MM"), Divisions);
                    else
                        Games = await _repository.GetGames(Divisions);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Excepetion caught: " + e.Message);
                    string message = "Something went wrong!";
                    if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                        message = "No internet connection!";
                    await App.Current.MainPage.DisplayAlert("Alert", message, "Ok");
                }



                // Resets active ilters string
                ActiveFilters = "Active filters: ";
                foreach (Division division in Divisions.FindAll(d => d.IsChecked == true))
                {
                    ActiveFilters = ActiveFilters + division.Label + " | ";
                }

                ActiveFiltersVisible = Divisions.Find(d => d.IsChecked == true) != null;

                IsLoading = false;
                OnPropertyChanged(nameof(ActiveFilters));
                OnPropertyChanged(nameof(ActiveFiltersVisible));
            }
            ShowFilter = !ShowFilter;

            OnPropertyChanged(nameof(ShowFilter));
        }

        private async Task ExecuteRefresh()
        {
            IsRefreshing = true;
            OnPropertyChanged(nameof(IsRefreshing));

            try
            {
                if (ShowMonth)
                    Games = await _repository.GetMonthGames(CurMonth.ToString("yyyy-MM"), Divisions);
                else
                    Games = await _repository.GetGames(Divisions);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepetion caught: " + e.Message);
                string message = "Something went wrong!";
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    message = "No internet connection!";
                await App.Current.MainPage.DisplayAlert("Alert", message, "Ok");
            }

            IsRefreshing = false;
            OnPropertyChanged(nameof(IsRefreshing));
        }

        public async Task LoadInitialData()
        {
            IsLoading = true;

            try
            {
                Games = await _repository.GetGames(Divisions);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepetion caught: " + e.Message);
                string message = "Something went wrong!";
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    message = "No internet connection!";
                await App.Current.MainPage.DisplayAlert("Alert", message, "Ok");
            }

            IsLoading = false;

        }

        private async Task CheckToken()
        {
            try
            {
                await _repository.CheckAuthentication();
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepetion caught: " + e.Message);
                string message = "Something went wrong!";
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                    message = "No internet connection!";
                await App.Current.MainPage.DisplayAlert("Alert", message, "Ok");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
