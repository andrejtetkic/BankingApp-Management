﻿using Common;
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
        public PrivilagesEnum Role { get; set; }
    }

    

}
