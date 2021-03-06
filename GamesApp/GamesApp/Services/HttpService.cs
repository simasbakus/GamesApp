using GamesApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GamesApp.Services
{
    class HttpService : IHttpService
    {
        // Temp crypted user credentials
        private readonly UserCredCrypted UserCred = new UserCredCrypted()
        {
            Username = "qsVTiAZsV4CxheSxcHjFYw==",
            UsernameIV = "Dxuf0iTVKyS/OZAJEza0nQ==",
            Password = "08KB5EWOvbiQyOmdSAASyQ==",
            PasswordIV = "mVKqOY76FY+QDz+22hKvYA=="
        };

        //Valid untill 03-15 18:35
        private string Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjYwMjc4MGQyNjE0NTQ0YTc2NGI5MzY1NSIsIm5iZiI6MTYxNDYxNjUyMSwiZXhwIjoxNjE1ODI2MTIxLCJpYXQiOjE2MTQ2MTY1MjF9.8ApLMvw6A2SPhbcUhO98LFuBbAp-Mjmqseln1WfqazM";

        private readonly HttpClient _httpClient = new HttpClient();

        public HttpService()
        {
            _httpClient.BaseAddress = new Uri("https://hockeygamesapi.azurewebsites.net");
        }

        public async Task<List<Game>> GetGames(string divisions = "")
        {
            if (await CheckToken() == false)
                await GetToken();

            UriBuilder path = new UriBuilder()
            {
                Scheme = "",
                Host = "",
                Path = "Games",
                Query = divisions != "" ? $"Divisions={divisions}" : ""
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, path.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                var games = JsonConvert.DeserializeObject<List<Game>>(content);

                return games;
            }
            return null;
        }

        public async Task<List<Game>> GetMonthGames(string date, string divisions = "")
        {
            if (await CheckToken() == false)
                await GetToken();

            UriBuilder path = new UriBuilder()
            {
                Scheme = "",
                Host = "",
                Path = $"Games/{date}",
                Query = divisions != "" ? $"Divisions={divisions}" : ""
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, path.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<Game>>(content);
            }
            return null;
        }

        public async Task<bool> CheckToken()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "Authenticate");
            //Instead of geting from var get from cache
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

        public async Task GetToken()
        {
            string serializedUserCred = JsonConvert.SerializeObject(UserCred);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "Authenticate")
            {
                Content = new StringContent(serializedUserCred)
            };

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Instead of var set token to cache
                Token = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
