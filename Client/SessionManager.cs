using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class SessionManager
    {
        private static UserSession _currentUser;

        public static void SetUserSession(UserSession user)
        {
            _currentUser = user;
        }

        public static UserSession GetCurrentUser()
        {
            return _currentUser;
        }

        public static bool IsAdmin()
        {
            return _currentUser?.Role == PrivilagesEnum.Admin;
        }
    }
}
