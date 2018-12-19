using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;

namespace com.cloud.prospect
{
    public class Database
    {
        private static Database Instance = null;

        public string ConnectionStr
        { get; private set; }

        protected Database()
        {
        }

        public Database(string connStr)
        {
            ConnectionStr = connStr;
        }

        public static Database GetInstance(string conStr)
        {
            if (Instance == null)
            {
                Instance = new Database(conStr);
            }
            return Instance;
        }

        public virtual DataSet ExecuteDataSet(string cmd, CommandType cmdType)
        {
            return null;
        }

        public virtual DataSet ExecuteDataSet(string cmd, CommandType cmdType, SqlParameter[] cmdParams)
        {
            return null;
        }

        public virtual void ExecuteNonQuery(string cmd, CommandType cmdType, SqlParameter[] cmdParams)
        {

        }

        public virtual void ExecuteNonQuery(string cmd, CommandType cmdType)
        {

        }


    }
}
