using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class UserSession
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public int Role { get; set; } // e.g., "Admin" 1 or "User" 0
    }

    

}
