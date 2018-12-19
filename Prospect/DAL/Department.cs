using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace com.cloud.prospect
{
   public partial class Department: DALModel<Department>
    {
       public static List<Department> departments;
       public Department()
           : base("S_Department")
        {

        }

        public void Insert()
        {
            CommonInsert(this, new string[] { "Name" });
            CommonSelectSingleRow(this,
                "Department_Number = (SELECT MAX(Department_Number) FROM S_Department NOLOCK)", null);
        }

        public void Delete()
        {
            CommonDelete(this, "Department_Number = " + Department_Number);
        }

        public List<Department> SelectAll()
        {
            if (departments==null)
                departments = CommonSelect(this, "1 = 1", null);
            return departments;
        }

    }
}
