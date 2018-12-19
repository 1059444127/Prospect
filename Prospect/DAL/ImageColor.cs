using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace com.cloud.prospect
{
    public partial class ImageColor : DALModel<ImageColor>
    {
        public ImageColor()
            : base("S_Image_Color")
        {
            Red = 0;
            Green = 0;
            Blue = 0;
        }

        public void Insert()
        {
            CommonInsert(this, new string[] { "Name", "Red", "Green", "Blue" });
        }

        public void SelectLatestColor()
        {
            CommonSelectSingleRow(this, "Unique_Number = (SELECT MAX(Unique_Number) FROM S_Image_Color NOLOCK)", null);
        }

        public void Delete()
        {
            CommonDelete(this, "Unique_Number = " + Unique_Number);
        }
        public void Update()
        {
            CommonUpdate(this, "Unique_Number = " + Unique_Number, new string[] { "Name", "Red", "Green", "Blue" });
        }
        public ObservableCollection<ImageColor> SelectAll()
        {
            return new ObservableCollection<ImageColor>(CommonSelect(this, "1=1", null));;
        }
    }
}
