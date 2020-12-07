using GamesApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GamesApp
{
    class DBService
    {
        public IMongoDatabase DB;
        public DBService()
        {
            MongoClient client = new MongoClient(///////);
            this.DB = client.GetDatabase(////////);
        }

        public List<Game> GetGames()
        {
            return DB.GetCollection<Game>(////////)
                        .Find(g => true)      // get all
                        .Sort("{ Date: -1}")  // sort by Date
                        .ToList();
        }

        /// <summary>
        /// Get all the documents from DataBase collection
        /// </summary>
        /// <returns>A list of documents based on Model Games</returns>
        public Task<List<Game>> GetGamesAsync()
        {
            return DB.GetCollection<Game>(////////)
                        .Find(g => true)      // get all
                        .Sort("{ Date: -1}")  // sort by Date
                        .ToListAsync();
        }

        /// <summary>
        /// Get documents from DataBase collection based on showAll value
        /// </summary>
        /// <param name="showMonth">False to show all, True to show current month's</param>
        /// <returns>A list of documents based on Model Games</returns>
        public Task<List<Game>> GetGamesMainFilterAsync(bool showMonth, int curMonth)
        {
            DateTime selectedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(curMonth);
            if (showMonth == true)
            {
                return DB.GetCollection<Game>(////////)
                        .Find(g => g.Date >= selectedMonth && g.Date < selectedMonth.AddMonths(1))  // get this month's
                        .Sort("{ Date: -1}")  // sort by Date
                        .ToListAsync();
            }
            else
            {
                return GetGamesAsync();
            }
        }
    }
}
