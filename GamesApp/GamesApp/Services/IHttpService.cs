using GamesApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GamesApp.Services
{
    public interface IHttpService
    {
        Task<List<Game>> GetGames(string divisions = "");
        Task<List<Game>> GetMonthGames(string date, string divisions = "");
        Task<bool> CheckToken();
        Task GetToken();
    }
}
