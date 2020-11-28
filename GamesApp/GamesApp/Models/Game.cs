using System;
using System.Collections.Generic;
using System.Text;

namespace GamesApp.Models
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Teams { get; set; }
        public string Division { get; set; }
        public string Position { get; set; }
    }
}
