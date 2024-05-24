using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class BankServer : IBank
    {
        public bool checkLogIn(string username, string password)
        {
            Console.WriteLine(username + " " + password);
            return Database.ExecuteScalarCommand($"select * from korisnik where email='{username}' and password='{password}'");
        }
    }
}
