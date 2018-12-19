using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class LastLogon : DALModel<LastLogon>
    {
        public  LastLogon()
            : base("S_Last_Logon")
        {
            Information.GetInformation();
        }

        public void Select()
        {
            CommonSelectSingleRow(this, "1=1", null);
        }

        public void Insert()
        {
            CommonInsert(this, null);
        }

        public void Delete()
        {
            CommonDelete(this, "1=1");
        }

    }
}
