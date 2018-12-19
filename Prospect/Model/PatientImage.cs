using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class PatientImage
    {
        #region Properties
        public string Patient_Number
        { get; set; }
        public int? Image_Number
        { get; set; }
        public string Image_Path
        {
            get;set;
        }
        public string Image_Format
        { get; set; }
        public string Image_Type
        { get; set; }
        public DateTime? Capture_Date
        { get; set; }
        #endregion
    }
}
