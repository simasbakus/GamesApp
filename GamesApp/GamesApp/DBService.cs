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
            this.DB = client.GetDatabase(/////////);
        }

        /// <summary>
        /// Get all the documents from DataBase collection
        /// </summary>
        /// <param name="sorter">1 for ascending sort, -1 for descending sort</param>
        /// <returns>A list of documents based on Model Games</returns>
        public List<Game> GetGames()
        {
            return DB.GetCollection<Game>(/////////)
                        .Find(g => true)      // get all
                        .Sort("{ Date: -1}")  // sort by Date
                        .ToList();
        }

        /// <summary>
        /// Get documents from DataBase collection based on showAll value
        /// </summary>
        /// <param name="showAll">True to show all, False to show current month's</param>
        /// <returns>A list of documents based on Model Games</returns>
        public List<Game> GetGamesMainFilter(bool showAll)
        {
            if (showAll == false)
            {
                return DB.GetCollection<Game>(////////)
                        .Find(g => g.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
                                    && g.Date < new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1))  // get this month's
                        .Sort("{ Date: -1}")                                                                 // sort by Date
                        .ToList();
            }
            else
            {
                return this.GetGames();
            }
        }
    }
}
