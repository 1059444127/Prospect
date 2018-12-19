using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class Diagnosis : DALModel<Diagnosis>
    {
        public Diagnosis()
            : base("S_Diagnosis")
        {

        }

        public void Insert()
        {
            CommonInsert(this, new string[] { "Diagnosis_Name" });
            CommonSelectSingleRow(this,
                "Diagnosis_Key = (SELECT MAX(Diagnosis_Key) FROM S_Diagnosis NOLOCK)", null);
        }

        public void Delete()
        {
            CommonDelete(this, "Diagnosis_Key = " + Diagnosis_Key);
        }

        public List<Diagnosis> SelectAll()
        {
            return CommonSelect(this, "1 = 1", null);
        }
    }
}
