using Akavache;
using GamesApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

        public async Task<List<Game>> GetGames(List<Division> divisions, bool forceRefresh = false)
        {
            List<Game> games;
            
            string divisionsQuery = GetDivisionsQuery(divisions);

            // if no division filters are active try get games from cache
            if (divisionsQuery == "" && !forceRefresh)
            {
                games = await GetFromCache<List<Game>>("games");

                if (games != null)
                    return games;
            }

            // ignore and remove cached games if method called with forceRefresh: true
            if (forceRefresh)
                await _cache.InvalidateAllObjects<List<Game>>();

            string token = await GetFromCache<string>("token");

            games = await _httpService.GetGames(token, divisionsQuery);

            if (games == null)
            {
                // if results from API where null check Token and try again
                await CheckAuthentication();

                token = await GetFromCache<string>("token");

                games = await _httpService.GetGames(token, divisionsQuery);
            }
            
            if (divisionsQuery == "" && games.Count > 0)
                await _cache.InsertObject("games", games, DateTimeOffset.Now.AddMinutes(5));

            return games;
        }

        public async Task<List<Game>> GetMonthGames(string date, List<Division> divisions, bool forceRefresh = false)
        {
            List<Game> games;

            string divisionsQuery = GetDivisionsQuery(divisions);

            // if no division filters are active try get games from cache
            if (divisionsQuery == "" && !forceRefresh)
            {
                games = await GetFromCache<List<Game>>($"games{date}");

                if (games != null)
                    return games;
            }

            // ignore and remove cached games if method called with forceRefresh: true
            if (forceRefresh)
                await _cache.InvalidateAllObjects<List<Game>>();

            string token = await GetFromCache<string>("token");

            games = await _httpService.GetMonthGames(token, date, divisionsQuery);

            if (games == null)
            {
                // if results from API where null check Token and try again
                await CheckAuthentication();

                token = await GetFromCache<string>("token");

                games = await _httpService.GetMonthGames(token, date, divisionsQuery);
            }

            if (divisionsQuery == "" && games.Count > 0)
                await _cache.InsertObject($"games{date}", games, DateTimeOffset.Now.AddMinutes(5));

            return games;
        }

        public async Task CheckAuthentication()
        {
            string token = await GetFromCache<string>("token");

            //Check if token exists in cache or if existing token is still valid
            if (token == null || await _httpService.CheckToken(token) == false)
            {
                token = await _httpService.GetToken();
                await _cache.InsertObject("token", token, DateTimeOffset.Now.AddMinutes(10));
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
