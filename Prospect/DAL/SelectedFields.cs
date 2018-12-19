using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class SelectedFields : DALModel<SelectedFields>
    {

        public SelectedFields()
            : base("S_Selected_Fiedls")
        {
            Select();
        }

        public void Select()
        {
            CommonSelectSingleRow(this, "Type = 1 ", null);
        }

        public void SelectDefaut()
        {
            CommonSelectSingleRow(this, "Type = 0 ", null);
        }

        public void Update()
        {
            CommonUpdate(this, "Type = 1", null);
        }
    }
}
