using GamesApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GamesApp.Services
{
    public interface IRepositoryGames
    {
        Task<List<Game>> GetGames(List<Division> divisions);
        Task<List<Game>> GetMonthGames(string date, List<Division> divisions);
    }
}
