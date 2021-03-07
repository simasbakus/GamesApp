using Akavache;
using GamesApp.Models;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesApp.Services
{
    class RepositoryGames : IRepositoryGames
    {
        private readonly IHttpService _httpService;
        private readonly IBlobCache _cache;

        public RepositoryGames(IHttpService httpService)
        {
            _httpService = httpService;
            _cache = BlobCache.LocalMachine;
        }

        public async Task<List<Game>> GetGames(List<Division> divisions)
        {
            string divisionsQuery = GetDivisionsQuery(divisions);

            string token = await GetFromCache<string>("token");

            List<Game> result;
            if (divisionsQuery != "")
                result = await _httpService.GetGames(token, divisionsQuery);
            else
                result = await _httpService.GetGames(token);

            if (result != null)
                return result;

            // if results from API where null check Token and re-call the procedure
            await CheckAuthentication();
            return await GetGames(divisions);
        }

        public async Task<List<Game>> GetMonthGames(string date, List<Division> divisions)
        {
            string divisionsQuery = GetDivisionsQuery(divisions);

            await CheckAuthentication();

            string token = await GetFromCache<string>("token");

            List<Game> result;
            if (divisionsQuery != "")
                result = await _httpService.GetMonthGames(token, date, divisionsQuery);
            else
                result = await _httpService.GetMonthGames(token, date);

            if (result != null)
                return result;

            // if results from API where null check Token and re-call the procedure
            await CheckAuthentication();
            return await GetMonthGames(date, divisions);
        }

        public async Task CheckAuthentication()
        {
            string token = await GetFromCache<string>("token");

            //Check if token exists in cache or if existing token is still valid
            if (token == null || await _httpService.CheckToken(token) == false)
            {
                token = await _httpService.GetToken();
                await _cache.InsertObject("token", token, DateTimeOffset.Now.AddMinutes(20));
            }
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

        private async Task<T> GetFromCache<T>(string key)
        {
            try
            {
                T t = await _cache.GetObject<T>(key);
                return t;
            }
            catch (KeyNotFoundException)
            {
                return default;
            }
        }
    }
}
