using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
   
    public partial class Patient
    {
        #region Properties
        public string Patient_Number
        { get; set; }
        public string Name
        { get; set; }
        public string Case_Number
        { get; set; }
        public string Insurance_Number
        { get; set; }
        public string Sex
        { get; set; }
        public string Phone_Number
        { get; set; }
        public string Id_Number
        { get; set; }
        public int? Age
        { get; set; }
        public string AgeType
        { get; set; }
        public bool? IsPositive
        { get; set; }
        public string Contact_Address
        { get; set; }
        public string Patient_Type
        { get; set; }
        public DateTime? Examine_Time
        { get; set; }
        #endregion
    }
}
