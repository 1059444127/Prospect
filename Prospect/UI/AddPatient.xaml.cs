using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for AddPatient.xaml
    /// </summary>
    public partial class AddPatient : Window
    {
        #region Members
        MainViewModel mainViewModle;
        #endregion

        public AddPatient()
        {
            InitializeComponent();
            InitViewModel();
        }

        #region Event
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            mainViewModle.ChangePatientNumber(mainViewModle.Case_Number);
            mainViewModle.Insert();
            this.Close();
            ((ProspectWin)this.Owner).RefreshPatientInformation(mainViewModle.Patient_Number);
        }
        void cbxBodyArea_CheckedChanged(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object obj in wplCheckPart.Children)
            {
                Canvas canvas = (Canvas)obj;
                CheckBox cbx = (CheckBox)canvas.Children[0];
                if (cbx.IsChecked == true)
                {
                    sb.Append(cbx.Content.ToString());
                    sb.Append(" ");
                }
            }
            if (sb.Length > 0)
            {
                sb = sb.Remove(sb.Length - 1, 1);
            }
            mainViewModle.Examine_Part = sb.ToString();
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            mainViewModle.Patient_Number = default(string);
            mainViewModle.Name = default(string);
            mainViewModle.Insurance_Number = default(string);
            mainViewModle.Sex = default(string);
            mainViewModle.Phone_Number = default(string);
            mainViewModle.Id_Number = default(string);
            mainViewModle.Age = default(int?);
            mainViewModle.AgeType = default(string);
            mainViewModle.Contact_Address = default(string);
            mainViewModle.Patient_Type = default(string);
            mainViewModle.Apply_Department = default(string);
            mainViewModle.Apply_Doctor = default(string);
            mainViewModle.Record_Doctor = default(string);
            mainViewModle.Diagnosis_Doctor = default(string);
            mainViewModle.Clinic_Number = default(string);
            mainViewModle.Hospitalized_Number = default(string);
            mainViewModle.Examine_Method = null;
            mainViewModle.Device_Number = default(string);
            mainViewModle.Sickbed_Number = default(string);
            mainViewModle.Host_Name = default(string);
            mainViewModle.Charge1 = default(string);
            mainViewModle.Charge2 = default(string);
            mainViewModle.Exists_Image = default(string);
            mainViewModle.Image_Quality = default(string);
            mainViewModle.Clinical_Diagnosis = default(string);
            mainViewModle.Examine_Test = default(string);
            mainViewModle.Examine_Part = default(string);
            mainViewModle.User_Defined1 = default(string);
            mainViewModle.User_Defined2 = default(string);
            mainViewModle.Ultrasound_Finding = default(string);
            mainViewModle.Ultrasound_Prompt = default(string);
            foreach (object obj in wplCheckPart.Children)
            {
                Canvas canvas = (Canvas)obj;
                CheckBox cbx = (CheckBox)canvas.Children[0];
                cbx.IsChecked = false;
            }
        }
        private void cbxDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxDepartment.SelectedItem != null)
            {
                Department dept = (Department)cbxDepartment.SelectedItem;
                Doctor doctor = new Doctor()
                {
                    Department_Number = dept.Department_Number
                };
                cbxApplyDoctor.ItemsSource = new ObservableCollection<Doctor>(doctor.SelectApplyDoctors());
                cbxApplyDoctor.DisplayMemberPath = "Name";
                cbxApplyDoctor.SelectedValuePath = "Doctor_Number";
            }
        }
        #endregion

        #region Method
        void InitViewModel()
        {
           
            Address addr = new Address();
            cbxAddress.ItemsSource = new ObservableCollection<Address>(addr.SelectAll());
            cbxAddress.DisplayMemberPath = "DetailAddress";
            cbxAddress.SelectedValuePath = "DetailAddress";

            ExamineMethod method = new ExamineMethod();
            cbxMethod.ItemsSource = new ObservableCollection<ExamineMethod>(method.SelectAll());
            cbxMethod.DisplayMemberPath = "Name";
            cbxMethod.SelectedValuePath = "Name";

            UserDefine1 def1 = new UserDefine1();
            cbxUserDef1.ItemsSource = new ObservableCollection<UserDefine1>(def1.SelectAll());
            cbxUserDef1.DisplayMemberPath = "UserDef_Name";
            cbxUserDef1.SelectedValuePath = "UserDef_Name";


            UserDefine2 def2 = new UserDefine2();
            cbxUserDef2.ItemsSource = new ObservableCollection<UserDefine2>(def2.SelectAll());
            cbxUserDef2.DisplayMemberPath = "UserDef_Name";
            cbxUserDef2.SelectedValuePath = "UserDef_Name";

            MedicalDevice dev = new MedicalDevice();
            cbxDevice.ItemsSource = new ObservableCollection<MedicalDevice>(dev.SelectAll());
            cbxDevice.DisplayMemberPath = "Device_Name";
            cbxDevice.SelectedValuePath = "Device_Name";

            Diagnosis diagnosis = new Diagnosis();
            cbxDiagnosis.ItemsSource = new ObservableCollection<Diagnosis>(diagnosis.SelectAll());
            cbxDiagnosis.DisplayMemberPath = "Diagnosis_Name";
            cbxDiagnosis.SelectedValuePath = "Diagnosis_Name";

            Department dept = new Department();
            cbxDepartment.ItemsSource = new ObservableCollection<Department>(dept.SelectAll());
            cbxDepartment.DisplayMemberPath = "Name";
            cbxDepartment.SelectedValuePath = "Name";

            mainViewModle = new MainViewModel(StartUpWindow.AddPatient, null);
            this.DataContext = mainViewModle;
            foreach (BodyArea obj in mainViewModle.GetListBodyArea())
            {
                CheckBox cbx = new CheckBox();
                cbx.Content = obj.BodyArea_Name;
                cbx.Width = 300;
                cbx.FontSize = 12;
                cbx.Checked += new RoutedEventHandler(cbxBodyArea_CheckedChanged);
                cbx.Unchecked += new RoutedEventHandler(cbxBodyArea_CheckedChanged);
                Canvas canvas = new Canvas();
                canvas.Height = 17;
                canvas.Width = 250;
                canvas.Children.Add(cbx);
                canvas.Style = FindResource("ChangeColorCanvasStyle") as Style;
                Canvas.SetTop(cbx, 2);

                wplCheckPart.Children.Add(canvas);
            }
        }
        #endregion






    }
}
