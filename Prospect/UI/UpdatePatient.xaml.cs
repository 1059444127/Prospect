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
    public partial class UpdatePatient : Window
    {
        #region Members
        MainViewModel mainViewModle;
        string Patient_Number;
        #endregion

        public UpdatePatient(string patientNumber)
        {
            InitializeComponent();
            Patient_Number = patientNumber;
            InitViewModel();
        }

        #region Event
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
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            mainViewModle.Update();
            this.Close();
            ((ProspectWin)this.Owner).RefreshPatientInformation(mainViewModle.Patient_Number);
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Method
        void InitViewModel()
        {
            mainViewModle = new MainViewModel(StartUpWindow.UpdatePatient, Patient_Number);
            this.DataContext = mainViewModle;

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
        }
        #endregion

        

       

       
    }
}
