using System;
using System.Collections.Generic;

namespace Luxubu.Models
{
    public partial class Users
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
