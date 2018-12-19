using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class Parameter : DALModel<Parameter>
    {
        public Parameter()
            : base("S_Parameter")
        {

        }

        public Parameter(string tablename)
            : base(tablename)
        {

        }

        public void Update()
        {
            CommonDelete(this, "Parameter_Name = '" + Parameter_Name + "'");
            CommonInsert(this, null);
        }

        public List<Parameter> SelectAll()
        {
            return CommonSelect(this, "1=1", null);
        }

        public void InsertLastConfig()
        {
            database.ExecuteNonQuery("USP_Common_Insert_Last_Config", System.Data.CommandType.StoredProcedure);
        }
    }
}
