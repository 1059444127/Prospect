using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;

namespace com.cloud.prospect
{
    public class DatabaseFactory
    {
        public static Database CreateDatabase()
        {
            Database database = null;
            try
            {
                string name = ConfigurationManager.AppSettings["DatabaseName"];
                string conStr = ConfigurationManager.ConnectionStrings[name].ConnectionString;
                string type = ConfigurationManager.AppSettings["DatabaseType"];
                switch (type)
                {
                    case "SQLServer":
                        database = SQLServer.GetInstance(conStr);
                        break;
                }
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, "程序数据库配置出错！程序将终止。");
            }
            return database;
        }

        public static Database CreateRemoteDatabase()
        {
            Database database = null;
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["Server"].ConnectionString;
                string type = ConfigurationManager.AppSettings["DatabaseType"];
                switch (type)
                {
                    case "SQLServer":
                        database = SQLServer.GetRemoteInstance(conStr);
                        break;
                }
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);
                return null;
            }
            return database;
        }
    }
}
