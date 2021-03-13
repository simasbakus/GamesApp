using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GamesApp.Models
{
    [Serializable()]
    public class Game
    {
        private int Id { get; set; }
        public string UrlLink { get; set; }
        public string DateStr { get; set; }
        public DateTime Date
        {
            get => DateTime.Parse(DateStr);
            set { DateStr = value.ToString("yyyy/MM/dd HH:mm"); }
        }
        public string Teams { get; set; }
        public string Division { get; set; }
        public string Position { get; set; }
    }
}
