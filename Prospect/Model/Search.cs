using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class Search
    {
        #region Patient
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
        public bool? IsPositive
        { get; set; }
        public string AgeType
        { get; set; }
        public string Contact_Address
        { get; set; }
        public string Patient_Type
        { get; set; }
        public DateTime? Examine_Time
        { get; set; }
        public string Examine_Time_String
        {
            get
            {
                return Examine_Time == default(DateTime?) ? null : ((DateTime)Examine_Time).ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                DateTime time = new DateTime();
                DateTime.TryParse(value,out time);
                Examine_Time = (DateTime?)time;
            }
        }
        #endregion
        #region ExamineInformation
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
