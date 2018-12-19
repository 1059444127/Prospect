using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class ExamineMethod : DALModel<ExamineMethod>
    {
        public static List<ExamineMethod> Methods;
        public ExamineMethod()
            : base("S_ExamineMethod")
        {
        }

        public void Insert()
        {
            CommonInsert(this, new string[] { "Name" });
            CommonSelectSingleRow(this,
                "Method_Key = (SELECT MAX(Method_Key) FROM S_ExamineMethod NOLOCK)", null);
        }

        public void Delete()
        {
            CommonDelete(this, "Method_Key = " + Method_Key);
        }

        public List<ExamineMethod> SelectAll()
        {
            if (Methods == null)
                Methods = CommonSelect(this, "1 = 1", null);
            return Methods;
        }
    }
}
