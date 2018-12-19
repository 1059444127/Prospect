using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class UserDefine2 : DALModel<UserDefine2>
    {
        public UserDefine2()
            : base("S_UserDef2")
        {

        }

        public void Insert()
        {
            CommonInsert(this, new string[] { "UserDef_Name" });
            CommonSelectSingleRow(this,
                "UserDef_Key = (SELECT MAX(UserDef_Key) FROM S_UserDef2 NOLOCK)", null);
        }

        public void Delete()
        {
            CommonDelete(this, "UserDef_Key = " + UserDef_Key);
        }

        public List<UserDefine2> SelectAll()
        {
            return CommonSelect(this, "1 = 1", null);
        }

    }
}
