using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace com.cloud.prospect
{
    public partial class ExamineInformation : DALModel<ExamineInformation>
    {
        public ExamineInformation() : base("A_Examine_Infomation") { }

        public void SelectSingleRow(string condition)
        {
            CommonSelectSingleRow(this, condition, null);
        }

        public void DeleteByPK()
        {
            if (String.IsNullOrEmpty(Patient_Number)) return;
            string condtion = " Patient_Number = '" + Patient_Number + "'";
            CommonDelete(this,condtion);
        }

        public void SelectByPK()
        {
            if (String.IsNullOrEmpty(Patient_Number)) return;
            string condtion = " Patient_Number = '" + Patient_Number + "'";
            SelectSingleRow(condtion);
        }

        public void Update()
        {
            if (String.IsNullOrEmpty(Patient_Number)) return;
            string condition = "Patient_Number = '" + Patient_Number + "'";
            CommonUpdate(this, condition, null);
        }

        public void Insert()
        {
            CommonDelete(this, "Patient_Number = '" + Patient_Number + "'");
            CommonInsert(this, null);
        }

        public List<ExamineInformation> Select(string condition)
        {
            return CommonSelect(this, condition, null);
        }

        public void ChangePK()
        {
            database.ExecuteNonQuery("Update A_Examine_Infomation WITH(ROWLOCK) SET Patient_Number='" + Patient_Number + "A' WHERE Patient_Number='" + Patient_Number + "'", CommandType.Text);
        }
    }
}
