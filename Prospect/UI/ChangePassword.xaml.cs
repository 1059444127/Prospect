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

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        Doctor doctor;

        public ChangePassword(Doctor user)
        {
            InitializeComponent();
            doctor = user;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            doctor.Select();
            if (!doctor.Password.Equals(pwdOriginal.Password))
            {
                MessageBox.Show("原密码不正确！");
                return;
            }
            if (!pwdNew.Password.Equals(pwdComfirm.Password))
            {
                MessageBox.Show("两次输入的密码不一致！");
                return;
            }
            doctor.Password = pwdNew.Password;
            doctor.UpdatePassword();
            MessageBox.Show("密码修改成功");
            Close();
        }
    }
}
