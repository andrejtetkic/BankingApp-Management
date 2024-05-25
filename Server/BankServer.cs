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
        public User AuthenticateUser(string username, string password)
        {
            
            string sqlQuery = $"select * from korisnik where email='{username}' and password='{password}'";
            List<Dictionary<string, object>> accounts = Database.ExecuteSelectCommand(sqlQuery);
            if (accounts.Count() > 0)
            {
                return new User(accounts[0]);

            }

            return null;
        }
    }
}
