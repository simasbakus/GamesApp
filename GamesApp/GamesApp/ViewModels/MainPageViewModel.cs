using GamesApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        DBService dbService = new DBService();
        public bool ShowMonth { get; set; } = false;
        public bool ShowFilter { get; set; } = false;
        public int CurMonth { get; set; } = 0;
        public string CurMonthStr { get; set; } = DateTime.Now.ToString("MMM");
        public List<Game> Games { get; set; }

        public ICommand ShowAllCommand { get; }
        public ICommand ChangeMonthCommand { get; }
        public ICommand ShowFilterCommand { get; }

        public MainPageViewModel()
        {
            ShowAllCommand = new Command(async () => await ChangeShowAll());
            ChangeMonthCommand = new Command<string>(async (newMonth) => await ChangeMonth(newMonth));
            ShowFilterCommand = new Command(ChangeShowFilter);
            // Get all the games, sort by Date
            this.Games = dbService.GetGames();
        }

        /// <summary>
        /// Method to change the main filter
        /// </summary>
        async Task ChangeShowAll()
        {
            this.ShowMonth = !this.ShowMonth;
            this.CurMonth = 0;
            this.CurMonthStr = DateTime.Now.ToString("MMM");

            // Get all the games, sort by Date
            this.Games = await dbService.GetGamesMainFilterAsync(this.ShowMonth, this.CurMonth);

            OnPropertyChanged(nameof(ShowMonth));
            OnPropertyChanged(nameof(Games));
        }

        async Task ChangeMonth(string newMonth)
        {

            this.CurMonth += int.Parse(newMonth);
            this.CurMonthStr = DateTime.Now.AddMonths(this.CurMonth).ToString("MMM");

            this.Games = await dbService.GetGamesMainFilterAsync(this.ShowMonth, this.CurMonth);

            OnPropertyChanged(nameof(Games));
            OnPropertyChanged(nameof(CurMonthStr));
        }

        void ChangeShowFilter()
        {
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
