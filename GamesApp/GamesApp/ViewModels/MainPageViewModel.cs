using GamesApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        // Test purpose --------------------------
        Stopwatch stopwatch = new Stopwatch();
        //----------------------------------------

        DBService       dbService { get; set; }
        public bool     ShowMonth { get; set; } = false;
        public bool     ShowFilter { get; set; } = false;
        public string   CurMonthStr { get; set; }
        public string   ActiveFilters { get; set; }
        public bool     ActiveFiltersVisible { get; set; } = false;
        public string   Message { get; set; }

        private List<Game> _Games { get; set; }
        public List<Game> Games
        {
            get { return _Games; }
            set
            {
                _Games = value;
                this.Message = "No games found!";

                OnPropertyChanged(nameof(Games));
            }
        }
        public List<Division>   Divisions { get; set; } = new List<Division>
        {
            new Division("NLRL", false, "Lietuvos \u010Dempionatas"),
            new Division("BHL", false, "Baltijos ledo ritulio lyga - D. Kasparai\u010Dio grup\u0117"),
            new Division("U18", false, "Jaunimo lyga"),
            new Division("U15", false, "U15"),
            new Division("U13", false, "U13"),
            new Division("U11", false, "U11")
        };

        public ICommand ShowAllCommand { get; }
        public ICommand ChangeMonthCommand { get; }
        public ICommand FilterBtnCommand { get; }

        public MainPageViewModel()
        {
            ShowAllCommand = new Command(async () => await ToggleShowAll());
            ChangeMonthCommand = new Command<string>(async (newMonth) => await ChangeMonth(newMonth));
            FilterBtnCommand = new Command(async () => await FilterBtnPressed());

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                this.Message = "No network access!";
            }

            try
            {
                this.dbService = new DBService();
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepetion caught: " + e.Message);
            }

            // Get all the games, sort by Date
            try
            {
                this.Games = dbService.GetGamesAllAsync().Result;
            }
            catch (Exception e)
            {
                this.Games = new List<Game>();
                Console.WriteLine("Excepetion caught: " + e.Message);
                this.Message = "Something went wrong!";
            }
        }

        async Task ToggleShowAll()
        {
            this.ShowMonth = !this.ShowMonth;

            dbService.CurMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            try
            {
                this.Games = await dbService.GetGamesByDivisionAndMonthAsync(this.ShowMonth, 0, this.Divisions);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepetion caught: " + e.Message);
                this.Message = "Something went wrong!";
            }

            this.CurMonthStr = dbService.CurMonth.ToString("MMM");

            OnPropertyChanged(nameof(Divisions));
            OnPropertyChanged(nameof(ShowMonth));
        }

        async Task ChangeMonth(string newMonth)
        {
            try
            {
                this.Games = await dbService.GetGamesByDivisionAndMonthAsync(this.ShowMonth, int.Parse(newMonth), this.Divisions);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepetion caught: " + e.Message);
                this.Message = "Something went wrong!";
            }

            this.CurMonthStr = dbService.CurMonth.ToString("MMM");

            OnPropertyChanged(nameof(CurMonthStr));
        }

        async Task FilterBtnPressed()
        {
            if (this.ShowFilter == true)
            {
                if (this.Divisions.Find(d => d.IsChecked == false) == null)
                {
                    //Check if all filter options ar checked, if so - all values are reset to initial ones
                    foreach (Division division in this.Divisions)
                    {
                        division.IsChecked = false;
                    }
                }

                try
                {
                    this.Games = await dbService.GetGamesByDivisionAndMonthAsync(this.ShowMonth, 0, this.Divisions);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Excepetion caught: " + e.Message);
                    this.Message = "Something went wrong!";
                }



                // Resets active ilters string
                this.ActiveFilters = "Active filters: ";
                foreach (Division division in this.Divisions.FindAll(d => d.IsChecked == true))
                {
                    this.ActiveFilters = this.ActiveFilters + division.Label + " | ";
                }

                this.ActiveFiltersVisible = this.Divisions.Find(d => d.IsChecked == true) != null;

                OnPropertyChanged(nameof(ActiveFilters));
                OnPropertyChanged(nameof(ActiveFiltersVisible));
            }
            this.ShowFilter = !this.ShowFilter;

            OnPropertyChanged(nameof(ShowFilter));
        }



        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
