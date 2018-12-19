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
using System.Timers;
using System.Windows.Threading;
using com.cloud.video;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for Startup.xaml
    /// </summary>
    public partial class Startup : Window
    {
        Timer aTimer = new Timer();

        public Startup()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                new Address().SelectAll();
                new ExamineMethod().SelectAll();
                new UserDefine1().SelectAll();
                new UserDefine2().SelectAll();
                new MedicalDevice().SelectAll();
                new Diagnosis().SelectAll();
                new Department().SelectAll();
                ProspectWin win = new ProspectWin();
                win.Show();
                Close();
            }),
                DispatcherPriority.ContextIdle, null);
        }



    }
}
