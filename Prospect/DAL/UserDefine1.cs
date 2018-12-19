using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class UserDefine1: DALModel<UserDefine1>
    {
        public UserDefine1()
            : base("S_UserDef1")
        {

        }

        public void Insert()
        {
            CommonInsert(this, new string[] { "UserDef_Name" });
            CommonSelectSingleRow(this,
                "UserDef_Key = (SELECT MAX(UserDef_Key) FROM S_UserDef1 NOLOCK)", null);
        }

        public void Delete()
        {
            CommonDelete(this, "UserDef_Key = " + UserDef_Key);
        }

        public List<UserDefine1> SelectAll()
        {
            return CommonSelect(this, "1 = 1", null);
        }

    }
}
