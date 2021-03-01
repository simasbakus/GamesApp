using GamesApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GamesApp.Services
{
    class RepositoryGames : IRepositoryGames
    {
        private readonly IHttpService _httpService;

        public RepositoryGames(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Game>> GetGames(List<Division> divisions)
        {
            string divisionsQuery = GetDivisionsQuery(divisions);

            if (divisionsQuery != "")
                return await _httpService.GetGames(divisionsQuery);

            return await _httpService.GetGames();
        }

        public async Task<List<Game>> GetMonthGames(string date, List<Division> divisions)
        {
            string divisionsQuery = GetDivisionsQuery(divisions);

            if (divisionsQuery != "")
                return await _httpService.GetMonthGames(date, divisionsQuery);

            return await _httpService.GetMonthGames(date);
        }

        private string GetDivisionsQuery(List<Division> divisions)
        {
            string divisionsStr = "";
            foreach (Division division in divisions.FindAll(d => d.IsChecked == true))
            {
                divisionsStr += $"{division.SearchTerm},";
            }

            return divisionsStr;
        }
    }
}
