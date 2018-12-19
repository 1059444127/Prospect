using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    [Serializable]
    public partial class ExamineInformation
    {
        #region Properties
        public string Patient_Number
        { get; set; }
        public string Apply_Department
        { get; set; }
        public string Ultrasound_Number
        { get; set; }
        public string Apply_Doctor
        { get; set; }
        public string Record_Doctor
        { get; set; }
        public string Diagnosis_Doctor
        { get; set; }
        public string Clinic_Number
        { get; set; }
        public string Hospitalized_Number
        { get; set; }
        public string Examine_Method
        { get; set; }
        public string Device_Number
        { get; set; }
        public string Sickbed_Number
        { get; set; }
        public string Host_Name
        { get; set; }
        public decimal? Charge1
        { get; set; }
        public decimal? Charge2
        { get; set; }
        public string Exists_Image
        { get; set; }
        public string Image_Quality
        { get; set; }
        public string Clinical_Diagnosis
        { get; set; }
        public string Patient_Source
        { get; set; }
        public string Examine_Test
        { get; set; }
        public string Examine_Part
        { get; set; }
        public string User_Defined1
        { get; set; }
        public string User_Defined2
        { get; set; }
        public string Ultrasound_Finding
        { get; set; }
        public string Ultrasound_Prompt
        { get; set; }
        #endregion
    }
}
