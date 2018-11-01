using System;
using System.Collections.Generic;

namespace UserAPI.Models
{
    public partial class TableUser
    {
        public int Userid { get; set; }
        public string x_auth_token{ get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Hashedpassword { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public int Postnummer { get; set; }
        public string Stad { get; set; }
        public DateTime Registered { get; set; }
    }
}
