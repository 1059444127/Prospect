using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace com.cloud.prospect
{
    public class StatisticReport : DALModel<Address>
    {
        public DateTime? StartDate
        { get; set; }
        public DateTime? EndDate
        { get; set; }


        public StatisticReport()
            : base("")
        {
        }

        public DataSet GetDataSet()
        {
            SqlParameter[] cmdParams = new SqlParameter[2];
            cmdParams[0] = new SqlParameter("@StartDate", SqlDbType.DateTime);
            cmdParams[0].Value = StartDate;
            cmdParams[1] = new SqlParameter("@EndDate", SqlDbType.DateTime);
            cmdParams[1].Value = EndDate;
            return database.ExecuteDataSet("[USP_Common_Statistic_Select]", CommandType.StoredProcedure, cmdParams);
        }
    }
}
