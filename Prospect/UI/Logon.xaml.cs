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

namespace com.cloud.prospect.XAML
{
    /// <summary>
    /// Interaction logic for Logon.xaml
    /// </summary>
    public partial class Logon : Window
    {
        LastLogon lastLogon;

        public Logon()
        {
            InitializeComponent();
            string regCode = System.Configuration.ConfigurationManager.AppSettings["RegisterCode"];
          
            if (!String.IsNullOrEmpty(Information.GetDate()) && !string.IsNullOrEmpty(regCode))
            {
                lblRegistered.Visibility = System.Windows.Visibility.Visible;
                tbkRegister.IsEnabled = false;
                tbkRegister.Visibility = System.Windows.Visibility.Hidden;
            }

            Department dept = new Department();
            cbxDepartment.ItemsSource = new ObservableCollection<Department>(dept.SelectAll());
            cbxDepartment.DisplayMemberPath = "Name";
            cbxDepartment.SelectedValuePath = "Name";

            lastLogon = new LastLogon();
            lastLogon.Select();
            int? deptNumber = lastLogon.Department_Number;
            if (deptNumber != default(int?))
            {
                dept = (from department in Department.departments
                        where department.Department_Number == (int)lastLogon.Department_Number
                        select department).First();
                cbxDepartment.SelectedItem = dept;
                Doctor doctor = new Doctor()
                {
                    Department_Number = dept.Department_Number
                };
                ObservableCollection<Doctor> doctors = new ObservableCollection<Doctor>(doctor.SelectDepartmentDoctor());
                cbxDoctor.ItemsSource = doctors;
                cbxDoctor.DisplayMemberPath = "Name";
                cbxDoctor.SelectedValuePath = "Name";
                if (doctors.Count > 0)
                {
                    var tmp = from user in doctors
                              where user.Doctor_Number == (int)lastLogon.User_Number
                              select user;
                    if (tmp.Count() > 0)
                    {
                        doctor = tmp.First();
                        cbxDoctor.SelectedItem = doctor;
                    }
                }
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
                cbxDoctor.ItemsSource = new ObservableCollection<Doctor>(doctor.SelectDepartmentDoctor());
                cbxDoctor.DisplayMemberPath = "Name";
                cbxDoctor.SelectedValuePath = "Name";
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogon_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cbxDoctor.Text))
            {
                MessageBox.Show("请录入用户名！");
                return;
            }
            Doctor doctor = cbxDoctor.SelectedItem as Doctor;
            doctor.Select();
            if (!doctor.Password.Equals(pwdDoctor.Password))
            {
                MessageBox.Show("用户名或密码不正确！");
                return;
            }
            if (cbxDepartment.SelectedItem != null)
                lastLogon.Department_Number = ((Department)cbxDepartment.SelectedItem).Department_Number;
            lastLogon.User_Number = doctor.Doctor_Number;
            lastLogon.Delete();
            lastLogon.Insert();
            Startup win = new Startup();
            win.ShowInTaskbar = false;
            win.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            win.Show();
            Close();

        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cbxDoctor.Text))
            {
                MessageBox.Show("请录入用户名！");
                return;
            }
            Doctor doctor = cbxDoctor.SelectedItem as Doctor;
            doctor.Select();
            ChangePassword win = new ChangePassword(doctor);
            win.Owner = this;
            win.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            win.ShowDialog();
        }

        private void tbkRegister_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Register win = new Register();
            win.Owner = this;
            win.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            win.ShowDialog();
        }
    }
}
