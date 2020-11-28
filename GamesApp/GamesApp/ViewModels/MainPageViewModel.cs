using GamesApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GamesApp.ViewModels
{
    public class MainPageViewModel
    {
        DBService dbService = new DBService();
        public List<Game> Games { get; }

        public MainPageViewModel()
        {
            // Get all the games, sort by Date descending
            this.Games = dbService.GetGames(-1);
        }
    }
}
