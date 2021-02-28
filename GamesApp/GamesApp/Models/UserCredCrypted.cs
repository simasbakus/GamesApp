using System;
using System.Collections.Generic;
using System.Text;

namespace GamesApp.Models
{
    class UserCredCrypted
    {
        public string Username { get; set; }
        public string UsernameIV { get; set; }
        public string Password { get; set; }
        public string PasswordIV { get; set; }
    }
}
