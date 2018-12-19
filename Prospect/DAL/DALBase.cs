using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using com.cloud.prospect;
using System.Xml.Serialization;
using System.Reflection;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace com.cloud.prospect
{
    public class DALBase
    {
        protected static Database database;
        protected static Database remoteDB;

        public DALBase()
        {
            if (database == null)
                database = DatabaseFactory.CreateDatabase();
            if (remoteDB == null)
                remoteDB = DatabaseFactory.CreateRemoteDatabase();
        }

        public DataSet ExecuteDataSet(string cmd, CommandType cmdType)
        {
            DataSet ds = null;
            try
            {
                ds = database.ExecuteDataSet(cmd, cmdType);
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e,false);
            }
            return ds;
        }

        public DataSet ExecuteDataSet(string cmd, CommandType cmdType, SqlParameter[] cmdParams)
        {
            DataSet ds = null;
            try
            {
                ds = database.ExecuteDataSet(cmd, cmdType, cmdParams);
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e,false);
            }
            return ds;
        }
    }
}
