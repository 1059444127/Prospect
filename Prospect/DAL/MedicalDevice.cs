using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class MedicalDevice : DALModel<MedicalDevice>
    {
        public MedicalDevice()
            : base("S_Device")
        {

        }

        public void Insert()
        {
            CommonInsert(this, new string[] { "Device_Name" });
            CommonSelectSingleRow(this,
                "Device_Key = (SELECT MAX(Device_Key) FROM S_Device NOLOCK)", null);
        }

        public void Delete()
        {
            CommonDelete(this, "Device_Key = " + Device_Key);
        }

        public List<MedicalDevice> SelectAll()
        {
            return CommonSelect(this, "1 = 1", null);
        }
       
    }
}
