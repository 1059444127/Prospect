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
using System.Configuration;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["RegisterCode"].Value = tbxCode.Text;
            cfa.Save();
            ConfigurationManager.RefreshSection("appSettings");
            Close();
        }

        private void tbkKey_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            KeyNumber win = new KeyNumber();
            win.Owner = this;
            win.ShowDialog();
        }
    }
}
