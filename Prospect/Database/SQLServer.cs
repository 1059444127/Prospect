using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;

namespace com.cloud.prospect
{
    public class SQLServer : Database
    {
        private static SQLServer Instance = null;
        private static SQLServer RemoteInstance = null;

        public static SQLServer GetInstance(string connStr)
        {
            if (Instance == null)
            {
                Instance = new SQLServer(connStr);
            }
            return Instance;
        }

        public static SQLServer GetRemoteInstance(string connStr)
        {
            if (RemoteInstance == null)
            {
                RemoteInstance = new SQLServer(connStr);
            }
            return RemoteInstance;
        }

        private SQLServer()
        {
        }

        private SQLServer(string connStr)
            : base(connStr)
        {
             
        }

        public override DataSet ExecuteDataSet(string cmd, System.Data.CommandType cmdType)
        {
            return SqlHelper.ExecuteDataset(ConnectionStr, cmdType, cmd);
        }

        public override DataSet ExecuteDataSet(string cmd, CommandType cmdType, SqlParameter[] cmdParams)
        {
            return SqlHelper.ExecuteDataset(ConnectionStr, cmdType, cmd, cmdParams);
        }

        public override void ExecuteNonQuery(string cmd, CommandType cmdType)
        {
            SqlHelper.ExecuteNonQuery(ConnectionStr, cmdType, cmd);
        }

        public override void ExecuteNonQuery(string cmd, CommandType cmdType, SqlParameter[] cmdParams)
        {
            SqlHelper.ExecuteNonQuery(ConnectionStr, cmdType, cmd, cmdParams);
        }
    }
}
