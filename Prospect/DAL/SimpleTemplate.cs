using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace com.cloud.prospect
{
    public partial class SimpleTemplate : DALModel<SimpleTemplate>
    {
        public SimpleTemplate()
            : base("S_Template_Simple")
        {

        }

        public void Insert()
        {
            CommonInsert(this, null);
        }

        public void Delete()
        {
            CommonDelete(this, "Template_Number = " + Template_Number);
        }

        public void Update()
        {
            CommonUpdate(this, "Template_Number = " + Template_Number, new string[] { "Name", "Description", "Type" });
        }


        public ObservableCollection<SimpleTemplate> LoadTemplate1()
        {
            return new ObservableCollection<SimpleTemplate>(CommonSelect(this, "Type = '1'", null));
        }

        public ObservableCollection<SimpleTemplate> LoadTemplate2()
        {
            return new ObservableCollection<SimpleTemplate>(CommonSelect(this, "Type = '2'", null));
        }
    }
}
