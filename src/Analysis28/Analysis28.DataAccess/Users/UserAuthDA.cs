using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Analysis28.Entity.Users;
using Common.Utility.DataAccess;

namespace Analysis28.DataAccess.Users
{
    public class UserAuthDA
    {
        public static List<UsersInfo> GetDemoData(int sysNo)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Demo");
            cmd.SetParameterValue("@SysNo", sysNo);
            return cmd.ExecuteEntityList<UsersInfo>();
        }
    }
}
