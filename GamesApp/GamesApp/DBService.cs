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
            this.DB = client.GetDatabase(///////);
        }

        /// <summary>
        /// Get all the documents from DataBase collection
        /// </summary>
        /// <param name="sorter">1 for ascending sort, -1 for descending sort</param>
        /// <returns>A list of documents based on Model Games</returns>
        public List<Game> GetGames(int sorter)
        {
            return DB.GetCollection<Game>(//////)
                        .Find(g => true)                  // get all
                        .Sort("{ Date: " + sorter + "}")  // sort by Date
                        .ToList();
        }
    }
}
