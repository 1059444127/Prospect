using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class Doctor : DALModel<Doctor>
    {
        public Doctor()
            : base("S_Doctor")
        {

        }

        public void Select()
        {
            CommonSelectSingleRow(this, "Doctor_Number = " + Doctor_Number, null);
        }

        public bool IsExist()
        {
            if (CommonSelect(this, "Name = '" + Name + "'", null).Count > 0)
                return true;
            else
                return false;
        }

        public void Insert()
        {
            CommonInsert(this, new string[] { "Department_Number", "Name", "Profile", "Account_Name", "Password" });
            CommonSelectSingleRow(this,
                "Doctor_Number = (SELECT MAX(Doctor_Number) FROM S_Doctor NOLOCK)", null);
        }

        public void Delete()
        {
            CommonDelete(this, "Doctor_Number = " + Doctor_Number);
        }

        public List<Doctor> SelectAll()
        {
            return CommonSelect(this, "1 = 1", null);
        }

        public List<Doctor> SelectApplyDoctors()
        {
            return CommonSelect(this, "Profile = '申请医生' AND Department_Number = " + Department_Number, null);
        }

        public List<Doctor> SelectDiagnosisDoctors()
        {
            return CommonSelect(this, "Profile = '诊断医生' AND Department_Number = " + Department_Number, null);
        }

        public List<Doctor> SelectRecordDoctors()
        {
            return CommonSelect(this, "Profile = '记录医生' AND Department_Number = " + Department_Number, null);
        }

        public List<Doctor> SelectDepartmentDoctor()
        {
            return CommonSelect(this, "Department_Number = " + Department_Number, null);
        }


        public void UpdatePassword()
        {
            CommonUpdate(this, "Doctor_Number = " + Doctor_Number, new string[] { "Password" });
        }
    }
}
