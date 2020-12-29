using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GamesApp.Models
{
    public class Game
    {
        private int _Id { get; set; }
        public int Id
        {
            get => _Id;
            set
            {
                _Id = value;
                UrlLink = "http://m.hockey.lt/#/rezultatai/rungtynes/" + _Id.ToString();
            }
        }
        public string DateStr { get; private set; }
        public DateTime Date
        {
            get => DateTime.Parse(DateStr);
            set { DateStr = value.ToString("yyyy/MM/dd HH:mm"); }
        }
        public string Teams { get; set; }
        public string Division { get; set; }
        public string Position { get; set; }
        public string UrlLink { get; set; }
    }
}
