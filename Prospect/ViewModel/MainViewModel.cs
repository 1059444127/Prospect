using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Reflection;
using System.Threading;
using System.Collections.ObjectModel;


namespace com.cloud.prospect
{

    public enum StartUpWindow
    {
        UCMain = 0,
        AddPatient = 1,
        UpdatePatient = 2,
        UCSearch = 3,
        SimpleTemplate1 = 4,
        SimpleTemplate2 = 5
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        #region Members
        Patient patient;
        ExamineInformation examineInfo;
        public static List<BodyArea> lstBody;
        #endregion

        #region Construction
        public MainViewModel(StartUpWindow window, string patientNumber)
        {
            switch (window)
            {
                case StartUpWindow.UCMain:
                    InitUCMain();
                    break;
                case StartUpWindow.AddPatient:
                    InitAddPatient();

                    break;
                case StartUpWindow.UpdatePatient:
                    InitUpdatePatient(patientNumber);
                    break;
                case StartUpWindow.UCSearch:
                    break;
            }
        }
        public MainViewModel() { }
        #endregion

        #region Properties
        #region patient
        public string Patient_Number
        {
            get
            {
                return patient.Patient_Number;
            }
            set
            {
                if (value != patient.Patient_Number)
                {
                    patient.Patient_Number = value;
                    RaisePropertyChanged("Patient_Number");
                }
            }
        }
        public string Name
        {
            get
            {
                return patient.Name;
            }
            set
            {
                if (value != patient.Name)
                {
                    patient.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public string Case_Number
        {
            get
            {
                return patient.Case_Number;
            }
            set
            {
                if (value != patient.Case_Number)
                {
                    patient.Case_Number = value;
                    RaisePropertyChanged("Case_Number");
                }
            }
        }
        public string Insurance_Number
        {
            get
            {
                return patient.Insurance_Number;
            }
            set
            {
                if (value != patient.Insurance_Number)
                {
                    patient.Insurance_Number = value;
                    RaisePropertyChanged("Insurance_Number");
                }
            }
        }
        public string Sex
        {
            get
            {
                return patient.Sex;
            }
            set
            {
                if (value != patient.Sex)
                {
                    patient.Sex = value;
                    RaisePropertyChanged("Sex");
                }
            }
        }
        public string Phone_Number
        {
            get
            {
                return patient.Phone_Number;
            }
            set
            {
                if (value != patient.Phone_Number)
                {
                    patient.Phone_Number = value;
                    RaisePropertyChanged("Phone_Number");
                }
            }
        }
        public string Id_Number
        {
            get
            {
                return patient.Id_Number;
            }
            set
            {
                if (value != patient.Id_Number)
                {
                    patient.Id_Number = value;
                    RaisePropertyChanged("Id_Number");
                }
            }
        }
        public bool? IsPositive
        {
            get
            {
                return patient.IsPositive;
            }
            set
            {
                if (value != patient.IsPositive)
                {
                    patient.IsPositive = value;
                    RaisePropertyChanged("IsPositive");
                }
            }
        }
        public int? Age
        {
            get
            {
                return patient.Age;
            }
            set
            {
                if (value != patient.Age)
                {
                    patient.Age = value;
                    RaisePropertyChanged("Age");
                }
            }
        }
        public string AgeType
        {
            get
            {
                return patient.AgeType;
            }
            set
            {
                if (value != patient.AgeType)
                {
                    patient.AgeType = value;
                    RaisePropertyChanged("Age");
                }
            }
        }
        public string Contact_Address
        {
            get
            {
                return patient.Contact_Address;
            }
            set
            {
                if (value != patient.Contact_Address)
                {
                    patient.Contact_Address = value;
                    RaisePropertyChanged("Contact_Address");
                }
            }
        }
        public string Patient_Type
        {
            get
            {
                return patient.Patient_Type;
            }
            set
            {
                if (value != patient.Patient_Type)
                {
                    patient.Patient_Type = value;
                    RaisePropertyChanged("Patient_Type");
                }
            }
        }
        public DateTime? Examine_Time
        {
            get
            {
                return patient.Examine_Time;
            }
            set
            {
                if (value != patient.Examine_Time)
                {
                    patient.Examine_Time = value;
                    RaisePropertyChanged("Examine_Time");
                }
            }
        }
        #endregion
        #region ExamineInformation
        public string Apply_Department
        {
            get
            {
                return examineInfo.Apply_Department;
            }
            set
            {
                if (value != examineInfo.Apply_Department)
                {
                    examineInfo.Apply_Department = value;
                    RaisePropertyChanged("Apply_Department");
                }
            }
        }
        public string Ultrasound_Number
        {
            get
            {
                return examineInfo.Ultrasound_Number;
            }
            set
            {
                if (value != examineInfo.Ultrasound_Number)
                {
                    examineInfo.Ultrasound_Number = value;
                    RaisePropertyChanged("Ultrasound_Number");
                }
            }
        }
        public string Apply_Doctor
        {
            get
            {
                return examineInfo.Apply_Doctor;
            }
            set
            {
                if (value != examineInfo.Apply_Doctor)
                {
                    examineInfo.Apply_Doctor = value;
                    RaisePropertyChanged("Apply_Doctor");
                }
            }
        }
        public string Record_Doctor
        {
            get
            {
                return examineInfo.Record_Doctor;
            }
            set
            {
                if (value != examineInfo.Record_Doctor)
                {
                    examineInfo.Record_Doctor = value;
                    RaisePropertyChanged("Record_Doctor");
                }
            }
        }
        public string Diagnosis_Doctor
        {
            get
            {
                return examineInfo.Diagnosis_Doctor;
            }
            set
            {
                if (value != examineInfo.Diagnosis_Doctor)
                {
                    examineInfo.Diagnosis_Doctor = value;
                    RaisePropertyChanged("Diagnosis_Doctor");
                }
            }
        }
        public string Clinic_Number
        {
            get
            {
                return examineInfo.Clinic_Number;
            }
            set
            {
                if (value != examineInfo.Clinic_Number)
                {
                    examineInfo.Clinic_Number = value;
                    RaisePropertyChanged("Clinic_Number");
                }
            }
        }
        public string Hospitalized_Number
        {
            get
            {
                return examineInfo.Hospitalized_Number;
            }
            set
            {
                if (value != examineInfo.Hospitalized_Number)
                {
                    examineInfo.Hospitalized_Number = value;
                    RaisePropertyChanged("Hospitalized_Number");
                }
            }
        }
        public string Examine_Method
        {
            get
            {

                return examineInfo.Examine_Method;

            }
            set
            {
                if (value != examineInfo.Examine_Method)
                {
                    examineInfo.Examine_Method = value;
                    RaisePropertyChanged("Examine_Method");
                }
            }
        }
        public string Device_Number
        {
            get
            {
                return examineInfo.Device_Number;
            }
            set
            {
                if (value != examineInfo.Device_Number)
                {
                    examineInfo.Device_Number = value;
                    RaisePropertyChanged("Device_Number");
                }
            }
        }
        public string Sickbed_Number
        {
            get
            {
                return examineInfo.Sickbed_Number;
            }
            set
            {
                if (value != examineInfo.Sickbed_Number)
                {
                    examineInfo.Sickbed_Number = value;
                    RaisePropertyChanged("Sickbed_Number");
                }
            }
        }
        public string Host_Name
        {
            get
            {
                return examineInfo.Host_Name;
            }
            set
            {
                if (value != examineInfo.Host_Name)
                {
                    examineInfo.Host_Name = value;
                    RaisePropertyChanged("Host_Name");
                }
            }
        }
        public string Charge1
        {
            get
            {
                if (!default(float?).Equals(examineInfo.Charge1))
                    return ((float)examineInfo.Charge1).ToString("F2");
                else
                    return "";
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    Decimal charge = 0;
                    if (Decimal.TryParse(value, out charge))
                    {
                        examineInfo.Charge1 = charge;
                    }
                }
                else
                {
                    examineInfo.Charge1 = default(Decimal?);
                }
                RaisePropertyChanged("Charge1");
            }
        }
        public string Charge2
        {
            get
            {
                if (!default(float?).Equals(examineInfo.Charge2))
                    return ((float)examineInfo.Charge2).ToString("F2");
                else
                    return "";
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    Decimal charge = 0;
                    if (Decimal.TryParse(value, out charge))
                    {
                        examineInfo.Charge2 = charge;
                    }
                }
                else
                {
                    examineInfo.Charge2 = default(Decimal?);
                }
                RaisePropertyChanged("Charge2");
            }
        }
        public string Exists_Image
        {
            get
            {
                return examineInfo.Exists_Image;
            }
            set
            {
                if (value != examineInfo.Exists_Image)
                {
                    examineInfo.Exists_Image = value;
                    RaisePropertyChanged("Exists_Image");
                }
            }
        }
        public string Image_Quality
        {
            get
            {
                return examineInfo.Image_Quality;
            }
            set
            {
                if (value != examineInfo.Image_Quality)
                {
                    examineInfo.Image_Quality = value;
                    RaisePropertyChanged("Image_Quality");
                }
            }
        }
        public string Clinical_Diagnosis
        {
            get
            {
                return examineInfo.Clinical_Diagnosis;
            }
            set
            {
                if (value != examineInfo.Clinical_Diagnosis)
                {
                    examineInfo.Clinical_Diagnosis = value;
                    RaisePropertyChanged("Clinical_Diagnosis");
                }
            }
        }
        public string Patient_Source
        {
            get
            {
                return examineInfo.Patient_Source;
            }
            set
            {
                if (value != examineInfo.Patient_Source)
                {
                    examineInfo.Patient_Source = value;
                    RaisePropertyChanged("Patient_Source");
                }
            }
        }
        public string Examine_Test
        {
            get
            {
                return examineInfo.Examine_Test;
            }
            set
            {
                if (value != examineInfo.Examine_Test)
                {
                    examineInfo.Examine_Test = value;
                    RaisePropertyChanged("Examine_Test");
                }
            }
        }
        public string Examine_Part
        {
            get
            {
                return examineInfo.Examine_Part;
            }
            set
            {
                if (value != examineInfo.Examine_Part)
                {
                    examineInfo.Examine_Part = value;
                    RaisePropertyChanged("Examine_Part");
                }
            }
        }

        public string User_Defined1
        {
            get
            {
                return examineInfo.User_Defined1;
            }
            set
            {
                if (value != examineInfo.User_Defined1)
                {
                    examineInfo.User_Defined1 = value;
                    RaisePropertyChanged("User_Defined1");
                }
            }
        }
        public string User_Defined2
        {
            get
            {
                return examineInfo.User_Defined2;
            }
            set
            {
                if (value != examineInfo.User_Defined2)
                {
                    examineInfo.User_Defined2 = value;
                    RaisePropertyChanged("User_Defined2");
                }
            }
        }
        public string Ultrasound_Finding
        {
            get
            {
                return examineInfo.Ultrasound_Finding;
            }
            set
            {
                if (value != examineInfo.Ultrasound_Finding)
                {
                    examineInfo.Ultrasound_Finding = value;
                    RaisePropertyChanged("Ultrasound_Finding");
                }
            }
        }
        public string Ultrasound_Prompt
        {
            get
            {
                return examineInfo.Ultrasound_Prompt;
            }
            set
            {
                if (value != examineInfo.Ultrasound_Prompt)
                {
                    examineInfo.Ultrasound_Prompt = value;
                    RaisePropertyChanged("Ultrasound_Prompt");
                }
            }
        }
        #endregion
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Method
        void InitUCMain()
        {
            patient = new Patient();
            patient.GetLatestRecord();
            examineInfo = new ExamineInformation()
            {
                Patient_Number = patient.Patient_Number
            };
            examineInfo.SelectByPK();
        }
        void InitAddPatient()
        {
            patient = new Patient()
            {
                AgeType = "岁",
                Patient_Type = "普通",
                Examine_Time = DateTime.Now
            };
            patient.CreatePatientNumber();
            patient.Case_Number = patient.Patient_Number;
            examineInfo = new ExamineInformation()
            {
                Patient_Number = patient.Patient_Number,
                Ultrasound_Number = patient.Patient_Number,
                Patient_Source = "门诊"
            };
            if (lstBody == null)
                lstBody = new BodyArea().SelectUSModality();
        }
        public void ChangePatientNumber(string number)
        {
            patient.Patient_Number = number;
            examineInfo.Patient_Number = number;
            examineInfo.Ultrasound_Number = number;
        }
        void InitUpdatePatient(string patientNumber)
        {
            patient = new Patient()
            {
                Patient_Number = patientNumber
            };
            patient.SelectByPK();
            examineInfo = new ExamineInformation()
            {
                Patient_Number = patient.Patient_Number
            };
            examineInfo.SelectByPK();
        }
        void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = Interlocked.CompareExchange(ref PropertyChanged, null, null);
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public Patient AddNewPatient()
        {
            Patient patient2 = new Patient()
            {
                AgeType = "岁",
                Patient_Type = "普通",
                Examine_Time = DateTime.Now
            };
            patient2.CreatePatientNumber();
            patient2.Case_Number = patient2.Patient_Number;
            ExamineInformation examineInfo2 = new ExamineInformation()
            {
                Patient_Number = patient2.Patient_Number,
                Ultrasound_Number = patient2.Patient_Number,
                Patient_Source = "门诊"
            };
            patient2.Insert();
            examineInfo2.Insert();
            return patient2;
        }
        public void RefreshUI()
        {
            PropertyInfo[] properties = typeof(MainViewModel).GetProperties();
            for (int i = 0; i < properties.Count(); i++)
            {
                RaisePropertyChanged(properties[i].Name);
            }
        }
        public void Update()
        {
            if (patient != null && !String.IsNullOrEmpty(patient.Patient_Number))
            {
                patient.Update();
                if (examineInfo == null || String.IsNullOrEmpty(examineInfo.Patient_Number))
                {
                    examineInfo = new ExamineInformation();
                    examineInfo.Patient_Number = patient.Patient_Number;
                }
                examineInfo.Update();

            }
        }
        public void Insert()
        {
            if (patient != null && !String.IsNullOrEmpty(patient.Patient_Number))
            {
                patient.Insert();
                if (examineInfo != null && !String.IsNullOrEmpty(examineInfo.Patient_Number))
                    examineInfo.Insert();
            }
        }
        public void SelectByKey(string patientNumber)
        {
            if (patient != null)
            {
                patient = new Patient();
                patient.Patient_Number = patientNumber;
                patient.SelectByPK();
                if (examineInfo != null)
                {
                    examineInfo = new ExamineInformation();
                    examineInfo.Patient_Number = patientNumber;
                    examineInfo.SelectByPK();
                }
            }
        }
        public void DeleteByKey(string patientNumber)
        {
            if (patient != null)
            {
                patient.Patient_Number = patientNumber;
                patient.DeleteByPK();
                if (examineInfo != null)
                {
                    examineInfo.Patient_Number = patientNumber;
                    examineInfo.DeleteByPK();
                }
            }
        }
        public void GetFirstRecord()
        {
            //patient = new Patient();
            patient.GetFirstRecord();
            examineInfo = new ExamineInformation()
            {
                Patient_Number = patient.Patient_Number
            };
            examineInfo.SelectByPK();
        }

        public void GetNextRecord()
        {
            patient.GetNextRecord();
            examineInfo = new ExamineInformation()
            {
                Patient_Number = patient.Patient_Number
            };
            examineInfo.SelectByPK();
        }

        public void GetLastRecord()
        {
            patient.GetLastRecord();
            examineInfo = new ExamineInformation()
            {
                Patient_Number = patient.Patient_Number
            };
            examineInfo.SelectByPK();
        }

        public void GetLatestRecord()
        {
            // patient = new Patient();
            patient.GetLatestRecord();
            examineInfo = new ExamineInformation()
            {
                Patient_Number = patient.Patient_Number
            };
            examineInfo.SelectByPK();
        }
        public List<BodyArea> GetListBodyArea()
        {
            return lstBody;
        }
        public void SaveImage()
        {

        }
        public PatientImage CreateNewImage()
        {
            PatientImage img = new PatientImage();
            img.Patient_Number = Patient_Number;
            if (Config.CompressPic)
            {
                img.Image_Format = "jpg";
            }
            else
            {
                img.Image_Format = "bmp";
            }
            img.Image_Type = "Image";
            string baseDir = Config.PatientImageBaseDirectory + @"\" + ((DateTime)Examine_Time).ToString("yyyyMMdd") + @"\"
                + Patient_Number + @"\";
            img.CreateNewImage(baseDir);
            return img;
        }
        public PatientImage CreateNewImage(string patientNumber, DateTime examineTime)
        {
            PatientImage img = new PatientImage();
            img.Patient_Number = patientNumber;
            img.Image_Format = "bmp";
            img.Image_Type = "Image";
            string baseDir = Config.PatientImageBaseDirectory + @"\" + examineTime.ToString("yyyyMMdd") + @"\"
                + patientNumber + @"\";
            img.CreateNewImage(baseDir);
            return img;
        }
        public PatientImage CreateNewVideo(string format)
        {
            PatientImage img = new PatientImage()
            {
                Patient_Number = Patient_Number,
                Image_Format = format,
                Image_Type = "Video"
            };
            string baseDir = Config.PatientImageBaseDirectory + @"\" + ((DateTime)Examine_Time).ToString("yyyyMMdd") + @"\"
                + Patient_Number + @"\";
            img.CreateNewImage(baseDir);
            return img;
        }
        public PatientImage CreateNewVideo(string patientNumber,DateTime examineTime, string format)
        {
            PatientImage img = new PatientImage()
            {
                Patient_Number = patientNumber,
                Image_Format = format,
                Image_Type = "Video"
            };
            string baseDir = Config.PatientImageBaseDirectory + @"\" + examineTime.ToString("yyyyMMdd") + @"\"
                + patientNumber + @"\";
            img.CreateNewImage(baseDir);
            return img;
        }

        public PatientImage SelectTopOneImage()
        {
            PatientImage img = new PatientImage()
            {
                Patient_Number = Patient_Number
            };
            List<PatientImage> imgs =  img.GetPatientImages();
            if (imgs.Count > 0)
                return imgs[0];
            else
                return null;
        }

        public PatientImage DeletePatientImages()
        {
            PatientImage img = new PatientImage()
            {
                Patient_Number = Patient_Number
            };
            img.DeletePatientImages();
            return img;
        }
        public List<PatientImage> GetPatientImages(string patientNumber)
        {
            PatientImage img = new PatientImage()
            {
                Patient_Number = patientNumber
            };
            return img.GetPatientImages();
        }
        public PatientImage AddImage(string baseDir, string format, string type)
        {
            PatientImage img = new PatientImage()
            {
                Patient_Number = Patient_Number,
                Image_Format = format,
                Image_Type = type
            };
            img.CreateNewImage(baseDir);
            return img;
        }

        public ObservableCollection<Search> QuerySearchInfo(string condition, int maxnum)
        {
            ObservableCollection<Search> result = null;
            try
            {
                Search app = new Search();
                result = app.Select(condition, maxnum);
            }
            catch (Exception e)
            {

                ErrorHandle.HandleException(e, false);
            }
            return result;
        }

        public string QueryServerInfo(string patient_number)
        {
            Patient patient = new Patient()
            {
                Patient_Number = patient_number
            };
            patient.GetPatientInfoFromServer();
            if (patient.Patient_Number == null)
            {
                MessageBox.Show("服务器无此病人信息！");
                return null;
            }
            ExamineInformation examineInfo = new ExamineInformation()
            {
                Patient_Number = patient.Patient_Number,
                Ultrasound_Number = patient.Patient_Number,
                Patient_Source = "门诊"
            };
            int result = patient.CheckIsExistsOrError();
            if (result == 0)
            {
                patient.Insert();
                examineInfo.Insert();
            }
            return patient.Patient_Number;
        }

        public ObservableCollection<Search> RefreshServerInfo()
        {
            Search app = new Search();
            return app.SearchPatientsFromServer();
        }

        public ObservableCollection<Search> GetCurrentSearchInfo()
        {
            Search app = new Search();
            return app.Select("A.Patient_Number = '" + patient.Patient_Number + "'", 0);
        }
        #endregion


    }
}
