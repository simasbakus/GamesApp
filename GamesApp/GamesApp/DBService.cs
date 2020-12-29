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
        private FilterDefinition<Game> MonthFilter { get; set; }
        private FilterDefinition<Game> DivisionFilter { get; set; }

        public DBService()
        {
            MongoClient client;
            client = new MongoClient("/////////");
            this.DB = client.GetDatabase("/////////");
        }


        public Task<List<Game>> GetAllAsync()
        {
            this.MonthFilter = Builders<Game>.Filter.Exists(g => g.Id);
            this.DivisionFilter = Builders<Game>.Filter.Exists(g => g.Id);

            return RunQueryAsync();
        }


        public Task<List<Game>> GetByMonthOrAllAsync(bool showMonth, int newMonth)
        {
            // Reset of the filter
            this.MonthFilter = Builders<Game>.Filter.Exists(g => g.Id);

            if (showMonth)
            {
                this.CurMonth = newMonth == 0 ? this.CurMonth : this.CurMonth.AddMonths(newMonth);
                this.MonthFilter = Builders<Game>.Filter.Gte(g => g.Date, this.CurMonth)
                                   & Builders<Game>.Filter.Lt(g => g.Date, this.CurMonth.AddMonths(1));
            }

            return RunQueryAsync();
        }


        public Task<List<Game>> GetByDivisionOrAllAsync(List<Division> divisions)
        {
            // Reset of the filter
            this.DivisionFilter = Builders<Game>.Filter.Exists(g => g.Id);

            if ((divisions.Find(d => d.IsChecked == true) != null))
            {
                // needs to be false
                this.DivisionFilter = Builders<Game>.Filter.Eq(g => g.Id, 0);

                foreach (Division division in divisions.FindAll(d => d.IsChecked == true))
                {
                    this.DivisionFilter = this.DivisionFilter | Builders<Game>.Filter.Eq(g => g.Division, division.SearchTerm);
                }
            }

            return RunQueryAsync();
        }


        public Task<List<Game>> RefreshAsync()
        {
            return RunQueryAsync();
        }


        private Task<List<Game>> RunQueryAsync()
        {
            return DB.GetCollection<Game>("////////")
                        .Find(this.MonthFilter & this.DivisionFilter)
                        .Sort("{ Date: -1}")
                        .ToListAsync();
        }
    }
}
