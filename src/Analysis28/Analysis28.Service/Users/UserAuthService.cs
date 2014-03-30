using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Analysis28.Entity.Users;
using Analysis28.DataAccess.Users;

namespace Analysis28.Service.Users
{
    public class UserAuthService
    {
        public static List<UsersInfo> GetDemoData(int sysNo)
        {
            return UserAuthDA.GetDemoData(sysNo);
        }
    }
}
