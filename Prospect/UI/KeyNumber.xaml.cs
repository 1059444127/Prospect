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
using com.cloud.video;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for Key.xaml
    /// </summary>
    public partial class KeyNumber : Window
    {
        public KeyNumber()
        {
            InitializeComponent();
            GenerateKey();
        }

        void GenerateKey()
        {
            StringBuilder sb1 = new StringBuilder(UtilFunctions.GetHardwareNumber());
             StringBuilder sb2 = new StringBuilder(1024);
            PInvoke.GenerateKey(sb1, sb2);
            textBox1.Text = sb2.ToString();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
