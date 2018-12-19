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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows.Controls;
using System.Collections.ObjectModel;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for UCSearch.xaml
    /// </summary>
    public partial class UCSearch : UserControl
    {

        public delegate void SelectPatientEventHandler(object sender, string patientNumber);
        public event SelectPatientEventHandler SelectPatientEvent;
        public delegate void DoubleClickPatientEventHandler(object sender, EventArgs args);
        public event DoubleClickPatientEventHandler DoubleClickPatientEvent;
        private bool IsQueryFromServer = false;


        MainViewModel mainViewModel;

        public Window Owner
        { get; set; }

        public UCSearch(MainViewModel model)
        {
            InitializeComponent();
            InitViewModel(model);
            ResetColumns();
        }

        #region Event
        private void tbkSystemConfig_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SystemSetting window = new SystemSetting();
            window.Owner = this.Owner;
            window.ShowInTaskbar = false;
            window.ShowDialog();
        }
        private void dgMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (IsQueryFromServer)
                {
                    Search search = (Search)dgMain.SelectedItem;
                    if (search == null) return;
                    Patient patient = new Patient()
                    {
                        Patient_Number = search.Patient_Number,
                        Name = search.Name,
                        Case_Number = search.Case_Number,
                        Insurance_Number = search.Insurance_Number,
                        Sex = search.Sex,
                        Phone_Number = search.Phone_Number,
                        Id_Number = search.Id_Number,
                        Age = search.Age,
                        AgeType = search.AgeType,
                        Contact_Address = search.Contact_Address,
                        Patient_Type = search.Patient_Type,
                        Examine_Time = search.Examine_Time
                    };
                    if (patient.CheckIsExistsOrError() == 0)
                    {
                        patient.Insert();
                        ExamineInformation exam = new ExamineInformation()
                        {
                            Patient_Number = search.Patient_Number,
                            Ultrasound_Number = search.Patient_Number,
                            Patient_Source = "门诊"
                        };
                        exam.DeleteByPK();
                        exam.Insert();
                    }
                }
                if (dgMain.SelectedItem != null)
                {
                    Search search = (Search)dgMain.SelectedItem;
                    string patientNumber = search.Patient_Number;
                    OnPatientSelect(patientNumber);
                    tbxPatientInfo.Text = "【" + search.Name + "】:" + search.Sex + ";"
                        + search.Age.ToString() + "岁;检查部位：" + search.Examine_Part;
                }
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1,false);
            }

        }
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            IsQueryFromServer = false;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"Examine_Time >=");
            sb.Append(String.IsNullOrEmpty(dteStart.Text) || cbxTime.IsChecked == true
                ? "'1900-01-01' AND " :
                 "'" + dteStart.SelectedDate.Value.ToString("yyyy-MM-dd").Replace("'", "''") + "' AND ");
            sb.Append(@"Examine_Time <= ");
            sb.Append(String.IsNullOrEmpty(dteEnd.Text) || cbxTime.IsChecked == true
                ? "'2100-01-01' AND " :
                "'" + dteEnd.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd").Replace("'", "''") + "' AND ");
            if (!String.IsNullOrEmpty(cbxPositive.Text)
                && !cbxPositive.Text.Equals("所有"))
            {
                if (cbxPositive.Text.Equals("阳性"))
                    sb.Append("IsPositive = 1 AND ");
                else if (cbxPositive.Text.Equals("阴性"))
                    sb.Append("IsPositive = 0 AND ");
            }

            sb.Append(String.IsNullOrEmpty(tbxPatientId.Text)
               ? "" : "A.Patient_Number = '" + tbxPatientId.Text.Replace("'", "''") + "' AND ");
            sb.Append(String.IsNullOrEmpty(tbxName.Text)
               ? "" : "Name = '" + tbxName.Text.Replace("'", "''") + "' AND ");
            sb.Append(String.IsNullOrEmpty(cbxSex.Text)
               ? "" : "Sex = '" + cbxSex.Text.Replace("'", "''") + "' AND ");
            sb.Append(String.IsNullOrEmpty(cbxPatientSrc.Text)
               ? "" : "Patient_Source = '" + cbxPatientSrc.Text.Replace("'", "''") + "' AND ");
            sb.Append(String.IsNullOrEmpty(cbxDepartment.Text)
              ? "" : "Apply_Department = '" + cbxDepartment.Text.Replace("'", "''") + "' AND ");
            if (cbxDiagnosisDoctor.SelectedValue == null)
                cbxDiagnosisDoctor.SelectedValue = cbxDiagnosisDoctor.Text;
            sb.Append(String.IsNullOrEmpty(cbxDiagnosisDoctor.Text)
              ? "" : "Diagnosis_Doctor = " + cbxDiagnosisDoctor.Text.ToString().Replace("'", "''") + "' AND ");
            sb.Append(String.IsNullOrEmpty(cbxDevice.Text)
              ? "" : "Device_Number = '" + cbxDevice.Text.Replace("'", "''") + "' AND ");
            ObservableCollection<Search> result = mainViewModel.QuerySearchInfo(sb.ToString() + "1=1 ", cbxTopNumber.IsChecked == true ? 100 : 0);
            dgMain.ItemsSource = result;

            tbkStatistics.Text = "共【" + result.Count().ToString() + "】条记录---自定义查询";
        }
        private void btnQueryServer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsQueryFromServer = true;
                if (String.IsNullOrEmpty(tbxPatientId.Text))
                {
                    MessageBox.Show("请先录入超声号！");
                    return;
                }
                string Patient_Number = mainViewModel.QueryServerInfo(tbxPatientId.Text);
                ObservableCollection<Search> result = mainViewModel.QuerySearchInfo("A.Patient_Number = '" + Patient_Number + "'", cbxTopNumber.IsChecked == true ? 100 : 0);
                dgMain.ItemsSource = result;
                tbkStatistics.Text = "共【" + result.Count().ToString() + "】条记录---自定义查询";
            }
            catch (Exception e1)
            {

                ErrorHandle.HandleException(e1, false);
            }
           

        }

        private void btnRefreshServer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsQueryFromServer = true;
                ObservableCollection<Search> result = mainViewModel.RefreshServerInfo();
                dgMain.ItemsSource = result;
                tbkStatistics.Text = "共【" + result.Count().ToString() + "】条记录---服务器查询";
            }
            catch (Exception e1)
            {

                ErrorHandle.HandleException(e1, false);
            }
            
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdData.Height = this.Height - 200;
        }
        private void dgMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            OnDoubleClickPatient();
        }
        private void tbxToday_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<Search> result = mainViewModel.QuerySearchInfo("Examine_Time BETWEEN '"
                + DateTime.Now.ToString("yyyy-MM-dd") + "' AND ' " + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'", 0);
            dgMain.ItemsSource = result;
        }
        private void tbkYesterday_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<Search> result = mainViewModel.QuerySearchInfo("Examine_Time BETWEEN '"
               + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' AND ' " + DateTime.Now.ToString("yyyy-MM-dd") + "'", 0);
            dgMain.ItemsSource = result;
        }
        private void tbkTop100_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<Search> result = mainViewModel.QuerySearchInfo("1=1", 100);
            dgMain.ItemsSource = result;
        }
        private void tbkLatset3days_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<Search> result = mainViewModel.QuerySearchInfo("Examine_Time BETWEEN '"
               + DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + "' AND ' " + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'", 0);
            dgMain.ItemsSource = result;
        }
        private void tbxTodayAllDepartment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<Search> result = mainViewModel.QuerySearchInfo("Examine_Time BETWEEN '"
                + DateTime.Now.ToString("yyyy-MM-dd") + "' AND ' " + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'", 0);
            dgMain.ItemsSource = result;
        }

        private void tbkYesterdayAllDepartment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<Search> result = mainViewModel.QuerySearchInfo("Examine_Time BETWEEN '"
              + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' AND ' " + DateTime.Now.ToString("yyyy-MM-dd") + "'", 0);
            dgMain.ItemsSource = result;
        }

        private void tbkLatset3daysDepartment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ObservableCollection<Search> result = mainViewModel.QuerySearchInfo("Examine_Time BETWEEN '"
              + DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + "' AND ' " + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "'", 0);
            dgMain.ItemsSource = result;
        }

        private void tbkTop100AllDepartment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObservableCollection<Search> result = mainViewModel.QuerySearchInfo("1=1", 100);
                dgMain.ItemsSource = result;
            }
            catch (Exception e1)
            {
                
              
            }
            
        }
        private void tbkFieldConfig_MouseLeftButtonDown(object sender,
            MouseButtonEventArgs e)
        {
            FieldItems window = new FieldItems();
            window.Owner = this.Owner;
            window.ShowInTaskbar = false;
            window.ShowDialog();
            ResetColumns();
        }
        private void cbxDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbxDepartment.SelectedItem != null)
                {
                    Department dept = (Department)cbxDepartment.SelectedItem;
                    Doctor doctor = new Doctor()
                    {
                        Department_Number = dept.Department_Number
                    };
                    cbxDiagnosisDoctor.ItemsSource = new ObservableCollection<Doctor>(doctor.SelectDiagnosisDoctors());
                    cbxDiagnosisDoctor.DisplayMemberPath = "Name";
                    cbxDiagnosisDoctor.SelectedValuePath = "Doctor_Number";
                }
            }
            catch (Exception e1)
            {

                ErrorHandle.HandleException(e1, false);
            }
            
        }
        private void tbxPatientId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnQueryServer_Click(sender, e);
        }
        #endregion

        #region Method
        void OnPatientSelect(string patientNumber)
        {
            SelectPatientEventHandler handler = System.Threading.Interlocked.CompareExchange(ref SelectPatientEvent, null, null);
            if (handler != null)
            {
                handler(this, patientNumber);
            }
        }
        void OnDoubleClickPatient()
        {
            DoubleClickPatientEventHandler handler = System.Threading.Interlocked.CompareExchange(ref DoubleClickPatientEvent, null, null);
            if (handler != null)
            {
                handler(this, null);
            }
        }
        void InitViewModel(MainViewModel model)
        {
            try
            {
                mainViewModel = model;
                grdMain.DataContext = mainViewModel;
                tbkStatistics.Text = "共【1】条记录---某个病人";
                tbxPatientInfo.Text = "【】:;0岁;检查部位：";
                dgMain.ItemsSource = mainViewModel.GetCurrentSearchInfo();



                MedicalDevice dev = new MedicalDevice();
                cbxDevice.ItemsSource = new ObservableCollection<MedicalDevice>(dev.SelectAll());
                cbxDevice.DisplayMemberPath = "Device_Name";
                cbxDevice.SelectedValuePath = "Device_Name";

                com.cloud.prospect.Department dept = new com.cloud.prospect.Department();
                cbxDepartment.ItemsSource = new ObservableCollection<Department>(dept.SelectAll());
                cbxDepartment.DisplayMemberPath = "Name";
                cbxDepartment.SelectedValuePath = "Name";
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);  
              
            }
           

        }
        public void ResetColumns()
        {
            foreach (Microsoft.Windows.Controls.DataGridTextColumn colunm in dgMain.Columns)
            {
                if (Config.SelectedColunms.Case_Number != true)
                    dgMain.Columns[0].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[0].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Ultrasound_Number != true)
                    dgMain.Columns[1].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[1].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Name != true)
                    dgMain.Columns[2].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[2].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Sex != true)
                    dgMain.Columns[3].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[3].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Age != true)
                {
                    dgMain.Columns[4].Visibility = System.Windows.Visibility.Hidden;
                    dgMain.Columns[5].Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    dgMain.Columns[4].Visibility = System.Windows.Visibility.Visible;
                    dgMain.Columns[5].Visibility = System.Windows.Visibility.Visible;
                }
                if (Config.SelectedColunms.Id_Number != true)
                    dgMain.Columns[6].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[6].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Insurance_Number != true)
                    dgMain.Columns[7].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[7].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Phone_Number != true)
                    dgMain.Columns[8].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[8].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Contact_Address != true)
                    dgMain.Columns[9].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[9].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Patient_Type != true)
                    dgMain.Columns[10].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[10].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Examine_Time != true)
                    dgMain.Columns[11].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[11].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Clinic_Number != true)
                    dgMain.Columns[12].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[12].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Hospitalized_Number != true)
                    dgMain.Columns[13].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[13].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Sickbed_Number != true)
                    dgMain.Columns[14].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[14].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.User_Defined1 != true)
                    dgMain.Columns[15].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[15].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.User_Defined2 != true)
                    dgMain.Columns[16].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[16].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Apply_Department != true)
                    dgMain.Columns[17].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[17].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Apply_Doctor != true)
                    dgMain.Columns[18].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[18].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Examine_Part != true)
                    dgMain.Columns[19].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[19].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Device_Number != true)
                    dgMain.Columns[20].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[20].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Host_Name != true)
                    dgMain.Columns[21].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[21].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Examine_Test != true)
                    dgMain.Columns[22].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[22].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Examine_Method != true)
                    dgMain.Columns[23].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[23].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Exists_Image != true)
                    dgMain.Columns[24].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[24].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Image_Quality != true)
                    dgMain.Columns[25].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[25].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Charge1 != true)
                    dgMain.Columns[26].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[26].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.Diagnosis_Doctor != true)
                    dgMain.Columns[27].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[27].Visibility = System.Windows.Visibility.Visible;
                if (Config.SelectedColunms.IsPositive != true)
                    dgMain.Columns[28].Visibility = System.Windows.Visibility.Hidden;
                else
                    dgMain.Columns[28].Visibility = System.Windows.Visibility.Visible;

            }
        }
        public void SearchCurrentInfo()
        {
            try
            {
                ObservableCollection<Search> result = mainViewModel.GetCurrentSearchInfo();
                if (result.Count > 0)
                {
                    Search search = result[0];
                    tbkStatistics.Text = "共【1】条记录---某个病人";
                    tbxPatientInfo.Text = "【" + search.Name + "】:" + search.Sex + ";"
                        + search.Age.ToString() + "岁;检查部位：" + search.Examine_Part;
                    dgMain.ItemsSource = result;
                }
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);
            }
          
        }
        #endregion


















    }
}
