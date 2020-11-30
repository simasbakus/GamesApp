using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GamesApp.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string DateStr { get; private set; }
        public DateTime Date
        {
            get { return DateTime.Parse(DateStr); }
            set { DateStr = value.ToString("yyyy/MM/dd HH:mm"); }
        }
        public string Teams { get; set; }
        public string Division { get; set; }
        public string Position { get; set; }
    }
}
