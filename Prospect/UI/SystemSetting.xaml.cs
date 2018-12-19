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
using System.Data.SqlClient;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for SystemSetting.xaml
    /// </summary>
    public partial class SystemSetting : Window
    {
        public SystemSetting()
        {
            InitializeComponent();
            BodyArea body = new BodyArea();
            dgBodyArea.ItemsSource = new ObservableCollection<BodyArea>(body.SelectUSModality());
            Address addr = new Address();
            dgAddress.ItemsSource = new ObservableCollection<Address>(addr.SelectAll());
            ExamineMethod method = new ExamineMethod();
            dgExamineMethod.ItemsSource = new ObservableCollection<ExamineMethod>(method.SelectAll());
            MedicalDevice dev = new MedicalDevice();
            dgDevice.ItemsSource = new ObservableCollection<MedicalDevice>(dev.SelectAll());
            Diagnosis diagnosis = new Diagnosis();
            dgDiagnosis.ItemsSource = new ObservableCollection<Diagnosis>(diagnosis.SelectAll());
            UserDefine1 def1 = new UserDefine1();
            dgUserDefine1.ItemsSource = new ObservableCollection<UserDefine1>(def1.SelectAll());
            UserDefine2 def2 = new UserDefine2();
            dgUserDefine2.ItemsSource = new ObservableCollection<UserDefine2>(def2.SelectAll());
            Department dept = new Department();
            dgDepartment.ItemsSource = new ObservableCollection<Department>(dept.SelectAll());
            if (((ObservableCollection<Department>)dgDepartment.ItemsSource).Count > 0)
            {
                dgDepartment.SelectedIndex = 0;
            }
        }

        #region Event
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxName.Text))
            {
                MessageBox.Show("请录入部位名称！");
                return;
            }
            decimal charge1 = 0;
            decimal charge2 = 0;
            decimal.TryParse(tbxCharge1.Text, out charge1);
            decimal.TryParse(tbxCharge1.Text, out charge2);
            BodyArea body = new BodyArea()
            {
                Modality_Type = "US",
                BodyArea_Name = tbxName.Text,
                Charge1 = charge1,
                Charge2 = charge2
            };
            body.Insert();
            ((ObservableCollection<BodyArea>)dgBodyArea.ItemsSource).Add(body);

        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resutl = MessageBox.Show("请确认删除该身体部位!", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resutl == MessageBoxResult.Yes)
            {
                if (dgBodyArea.SelectedItem != null)
                {
                    BodyArea body = (BodyArea)dgBodyArea.SelectedItem;
                    body.Delete();
                    ((ObservableCollection<BodyArea>)dgBodyArea.ItemsSource).Remove(body);
                }
            }
        }
        private void btnAddAddress_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxAddress.Text))
            {
                MessageBox.Show("请录入地址名称！");
                return;
            }
            Address addr = new Address()
            {
                DetailAddress = tbxAddress.Text
            };
            addr.Insert();
            ((ObservableCollection<Address>)dgAddress.ItemsSource).Add(addr);

        }
        private void btnDeleteAddress_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resutl = MessageBox.Show("请确认删除该地址!", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resutl == MessageBoxResult.Yes)
            {
                if (dgAddress.SelectedItem != null)
                {
                    Address addr = (Address)dgAddress.SelectedItem;
                    addr.Delete();
                    ((ObservableCollection<Address>)dgAddress.ItemsSource).Remove(addr);
                }
            }
        }
        private void btnAddMethod_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxMethod.Text))
            {
                MessageBox.Show("请录入检查方法名称！");
                return;
            }
            ExamineMethod method = new ExamineMethod()
            {
                Name = tbxMethod.Text
            };
            method.Insert();
            ((ObservableCollection<ExamineMethod>)dgExamineMethod.ItemsSource).Add(method);
        }
        private void btnDeleteMethod_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resutl = MessageBox.Show("请确认删除该检查方法!", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resutl == MessageBoxResult.Yes)
            {
                if (dgExamineMethod.SelectedItem != null)
                {
                    ExamineMethod method = (ExamineMethod)dgExamineMethod.SelectedItem;
                    method.Delete();
                    ((ObservableCollection<ExamineMethod>)dgExamineMethod.ItemsSource).Remove(method);
                }
            }
        }
        private void btnAddDev_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxDevice.Text))
            {
                MessageBox.Show("请录入设备型号！");
                return;
            }
            MedicalDevice dev = new MedicalDevice()
            {
                Device_Name = tbxDevice.Text
            };
            dev.Insert();
            ((ObservableCollection<MedicalDevice>)dgDevice.ItemsSource).Add(dev);
        }
        private void btnDeleteDev_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resutl = MessageBox.Show("请确认删除该设备型号!", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resutl == MessageBoxResult.Yes)
            {
                if (dgDevice.SelectedItem != null)
                {
                    MedicalDevice dev = (MedicalDevice)dgDevice.SelectedItem;
                    dev.Delete();
                    ((ObservableCollection<MedicalDevice>)dgDevice.ItemsSource).Remove(dev);
                }
            }
        }
        private void btnAddDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxDiagnosis.Text))
            {
                MessageBox.Show("请录入诊断条码！");
                return;
            }
            Diagnosis diagnosisi = new Diagnosis()
            {
                Diagnosis_Name = tbxDiagnosis.Text
            };
            diagnosisi.Insert();
            ((ObservableCollection<Diagnosis>)dgDiagnosis.ItemsSource).Add(diagnosisi);
        }
        private void btnDeleteDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resutl = MessageBox.Show("请确认删除该诊断条码!", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resutl == MessageBoxResult.Yes)
            {
                if (dgDiagnosis.SelectedItem != null)
                {
                    Diagnosis diagnosisi = (Diagnosis)dgDiagnosis.SelectedItem;
                    diagnosisi.Delete();
                    ((ObservableCollection<Diagnosis>)dgDiagnosis.ItemsSource).Remove(diagnosisi);
                }
            }
        }
        private void btnAddUserDef1_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxUserDef1.Text))
            {
                MessageBox.Show("请录入自定义一！");
                return;
            }
            UserDefine1 def = new UserDefine1()
            {
                UserDef_Name = tbxUserDef1.Text
            };
            def.Insert();
            ((ObservableCollection<UserDefine1>)dgUserDefine1.ItemsSource).Add(def);
        }
        private void btDeleteUserDef1_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resutl = MessageBox.Show("请确认删除该自定义一条目!", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resutl == MessageBoxResult.Yes)
            {
                if (dgUserDefine1.SelectedItem != null)
                {
                    UserDefine1 def = (UserDefine1)dgUserDefine1.SelectedItem;
                    def.Delete();
                    ((ObservableCollection<UserDefine1>)dgUserDefine1.ItemsSource).Remove(def);
                }
            }
        }
        private void btnAddUserDef2_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxUserDef2.Text))
            {
                MessageBox.Show("请录入自定义二！");
                return;
            }
            UserDefine2 def = new UserDefine2()
            {
                UserDef_Name = tbxUserDef2.Text
            };
            def.Insert();
            ((ObservableCollection<UserDefine2>)dgUserDefine2.ItemsSource).Add(def);
        }
        private void btnDeleteUserDef2_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resutl = MessageBox.Show("请确认删除该自定义一条目!", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resutl == MessageBoxResult.Yes)
            {
                if (dgUserDefine2.SelectedItem != null)
                {
                    UserDefine2 def = (UserDefine2)dgUserDefine2.SelectedItem;
                    def.Delete();
                    ((ObservableCollection<UserDefine2>)dgUserDefine2.ItemsSource).Remove(def);
                }
            }
        }
        private void btnAddDept_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxDept.Text))
            {
                MessageBox.Show("请录入科室名称！");
                return;
            }
            Department dept = new Department()
            {
                Name = tbxDept.Text
            };
            dept.Insert();
            ((ObservableCollection<Department>)dgDepartment.ItemsSource).Add(dept);
        }
        private void btnDeleteDept_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resutl = MessageBox.Show("请确认删除该科室!", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resutl == MessageBoxResult.Yes)
            {
                if (dgDepartment.SelectedItem != null)
                {
                    Department dept = (Department)dgDepartment.SelectedItem;
                    dept.Delete();
                    ((ObservableCollection<Department>)dgDepartment.ItemsSource).Remove(dept);
                }
            }
        }
        private void dgDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDepartment.SelectedItem != null)
            {
                Department dept = (Department)dgDepartment.SelectedItem;
                Doctor doctor = new Doctor()
                {
                    Department_Number = dept.Department_Number
                };
                dgDoctor.ItemsSource = new ObservableCollection<Doctor>(doctor.SelectDepartmentDoctor());
            }
        }
        private void btnAddDoctor_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxDoctorName.Text))
            {
                MessageBox.Show("请录入医生名称！");
                return;
            }
            if (String.IsNullOrEmpty(tbxDoctorProfile.Text))
            {
                MessageBox.Show("请录入医生角色！");
                return;
            }
            if (dgDepartment.SelectedItem == null)
            {
                MessageBox.Show("请选择一个科室！");
                return;
            }
            if (!tbxPassword.Password.Equals(tbxPassword2.Password))
            {
                MessageBox.Show("两次录入的密码不一致！");
                return;
            }
            Doctor doctor = new Doctor()
            {
                Name = tbxDoctorName.Text,
                Department_Number = ((Department)dgDepartment.SelectedItem).Department_Number,
                Account_Name = tbxDoctorName.Text,
                Profile = tbxDoctorProfile.Text,
                Password = tbxPassword.Password
            };
            try
            {
                if (doctor.IsExist())
                {
                    MessageBox.Show("已存在该账户名！");
                    return;
                }
                doctor.Insert();
                ((ObservableCollection<Doctor>)dgDoctor.ItemsSource).Add(doctor);
            }
            catch (SqlException e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
        }

        private void btnDeleteDoctor_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resutl = MessageBox.Show("请确认删除该医生!", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resutl == MessageBoxResult.Yes)
            {
                if (dgDoctor.SelectedItem != null)
                {
                    Doctor doctor = (Doctor)dgDoctor.SelectedItem;
                    doctor.Delete();
                    ((ObservableCollection<Doctor>)dgDoctor.ItemsSource).Remove(doctor);
                }
            }
        }
        #endregion


    }
}
