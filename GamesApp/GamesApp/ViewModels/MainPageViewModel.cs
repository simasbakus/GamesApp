using GamesApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        DBService dbService = new DBService();
        public bool ShowAll { get; set; } = true;
        public List<Game> Games { get; set; }
        public ICommand ShowAllCommand { get; }

        public MainPageViewModel()
        {
            ShowAllCommand = new Command(ChangeShowAll);
            // Get all the games, sort by Date
            this.Games = dbService.GetGames();
        }

        /// <summary>
        /// Method to change the main filter
        /// </summary>
        void ChangeShowAll()
        {
            this.ShowAll = !this.ShowAll;

            // Get all the games, sort by Date
            this.Games = dbService.GetGamesMainFilter(this.ShowAll);

            OnPropertyChanged(nameof(ShowAll));
            OnPropertyChanged(nameof(Games));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
