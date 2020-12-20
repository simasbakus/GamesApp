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
        public DateTime CurMonth { get; set; }
        public DBService()
        {
            MongoClient client;
            client = new MongoClient(////////);
            this.DB = client.GetDatabase("hockey");
        }

        public Task<List<Game>> GetGamesAllAsync()
        {
            return DB.GetCollection<Game>(////////)
                        .Find(g => true)      // get all
                        .Sort("{ Date: -1}")  // sort by Date
                        .ToListAsync();
        }

        public Task<List<Game>> GetGamesByMonthAsync(bool showMonth, int newMonth)
        {
            if (showMonth == true)
            {
                this.CurMonth = newMonth == 0 ? this.CurMonth : this.CurMonth.AddMonths(newMonth);
                return DB.GetCollection<Game>(///////)
                        .Find(g => g.Date >= this.CurMonth && g.Date < this.CurMonth.AddMonths(1))
                        .Sort("{ Date: -1}")
                        .ToListAsync();
            }
            else
            {
                return GetGamesAllAsync();
            }
        }

        public Task<List<Game>> GetGamesByDivisionAndMonthAsync(bool showMonth, int newMonth, List<Division> divisions)
        {
            // Check to see if any filters by division are needed
            if (divisions.Find(d => d.IsChecked == true) != null)
            {
                //Dummy filter needed to instansiate a variable 'filter'
                var filter = Builders<Game>.Filter.Eq(g => g.Id, 0);

                //Creates a filter for each element which has IsChecked = true
                foreach (Division division in divisions.FindAll(d => d.IsChecked == true))
                {
                    filter = filter | Builders<Game>.Filter.Eq(g => g.Division, division.SearchTerm);
                }

                if (showMonth == true)
                {
                    //Creates a filter for month if view in MainPage is by month
                    this.CurMonth = newMonth == 0 ? this.CurMonth : this.CurMonth.AddMonths(newMonth); 
                    filter = filter & Builders<Game>.Filter.Gte(g => g.Date, this.CurMonth) 
                                    & Builders<Game>.Filter.Lt(g => g.Date, this.CurMonth.AddMonths(1));
                }

                return DB.GetCollection<Game>(///////)
                        .Find(filter)
                        .Sort("{ Date: -1}")
                        .ToListAsync();
            }
            else
            {
                //If no element with IsChecked = true exist, no filter by division is needed, Games list is found only by month
                return GetGamesByMonthAsync(showMonth, newMonth);
            }
        }
    }
}
