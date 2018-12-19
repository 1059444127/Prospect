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
    /// Interaction logic for ColorTemplateName.xaml
    /// </summary>
    public partial class ColorTemplateName : Window
    {

        public string Name
        { get; private set; }

        public ColorTemplateName()
        {
            InitializeComponent();
        }

        public ColorTemplateName(string name)
        {
            InitializeComponent();
            tbxColorName.Text = name;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxColorName.Text))
            {
                MessageBox.Show("请录入伪彩名称！");
                return;
            }
            Name = tbxColorName.Text;
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
