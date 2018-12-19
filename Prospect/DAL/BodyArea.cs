using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class BodyArea : DALModel<BodyArea>
    {
        public BodyArea()
            : base("S_BodyArea")
        {
        }

        public void Insert()
        {
            CommonInsert(this, new string[] { "Modality_Type", "BodyArea_Name", "Charge1", "Charge2" });
            CommonSelectSingleRow(this, 
                "BodyArea_Number = (SELECT MAX(BodyArea_Number) FROM S_BodyArea NOLOCK)",null);
        }

        public void Delete()
        {
            CommonDelete(this, "BodyArea_Number = " + BodyArea_Number);
        }

        public List<BodyArea> SelectUSModality()
        {
            return CommonSelect(this, "Modality_Type='US'", null);
        }


    }
}
