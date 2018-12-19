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
using System.Windows.Forms;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for OptionSettingWindow.xaml
    /// </summary>
    public partial class OptionSettingWindow : Window
    {
        public OptionSettingWindow()
        {
            InitializeComponent();
            Init();
        }

        #region Event
        private void btnImagePath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult reslut = dlg.ShowDialog();
            if (!String.IsNullOrEmpty(dlg.SelectedPath))
            {
                tbxImagePath.Text = dlg.SelectedPath;
                Config.PatientImageBaseDirectory = tbxImagePath.Text;
            }

        }
        private void tbxHospitalName_LostFocus(object sender, RoutedEventArgs e)
        {
            Config.HostpitalName = tbxHospitalName.Text;
        }
        private void tbxReportName_LostFocus(object sender, RoutedEventArgs e)
        {
            Config.ReportName = tbxReportName.Text;
        }
        private void tbxReportFoot_LostFocus(object sender, RoutedEventArgs e)
        {
            Config.ReportFoot = tbxReportFoot.Text;
        }
        private void tbxImagePath_LostFocus(object sender, RoutedEventArgs e)
        {
            Config.PatientImageBaseDirectory = tbxImagePath.Text;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnDefautConfig_Click(object sender, RoutedEventArgs e)
        {
            Config.LoadDefautConfig();
            System.Windows.MessageBox.Show("还原到出厂设置成功！");
        }
        private void btnSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            Config.InsertLastConfig();
            Config.SaveConfig();
            System.Windows.MessageBox.Show("保存当前设置成功！");
        }
        private void btnLastConfig_Click(object sender, RoutedEventArgs e)
        {
            Config.LoadLastConfig();
            System.Windows.MessageBox.Show("还原上次设置成功！");
        }

        private void rbnA4_Checked(object sender, RoutedEventArgs e)
        {
            Config.ReportFormat = "A4";
            cbxDefautReport.Text = "";
            cbxDefautReport.Items.Clear();
            cbxDefautReport.Items.Add("标准样式A4-一图.frx");
            cbxDefautReport.Items.Add("标准样式A4-二图.frx");
            cbxDefautReport.Items.Add("标准样式A4-三图.frx");
            cbxDefautReport.Items.Add("标准样式A4-四图.frx");
            cbxDefautReport.Items.Add("标准样式A4-两图颅脑.frx");
            cbxDefautReport.Items.Add("标准样式A4-心脏.frx");
            cbxDefautReport.Items.Add("标准样式A4-文字.frx");
        }

        private void rbnB5_Checked(object sender, RoutedEventArgs e)
        {
            Config.ReportFormat = "B5";
            cbxDefautReport.Text = "";
            cbxDefautReport.Items.Clear();
            cbxDefautReport.Items.Add("标准样式B5-一图.frx");
            cbxDefautReport.Items.Add("标准样式B5-两图.frx");
            cbxDefautReport.Items.Add("标准样式B5-四图.frx");
            cbxDefautReport.Items.Add("标准样式B5-两图颅脑.frx");
            cbxDefautReport.Items.Add("标准样式B5-心脏.frx");
            cbxDefautReport.Items.Add("标准样式B5-文字.frx");
        }

        private void rbn16K_Checked(object sender, RoutedEventArgs e)
        {
            Config.ReportFormat = "16k";
            cbxDefautReport.Text = "";
            cbxDefautReport.Items.Clear();
            cbxDefautReport.Items.Add("标准样式16K-一图.frx");
            cbxDefautReport.Items.Add("标准样式16K-两图.frx");
            cbxDefautReport.Items.Add("标准样式16K-四图.frx");
            cbxDefautReport.Items.Add("标准样式16K-两图颅脑.frx");
            cbxDefautReport.Items.Add("标准样式16K-心脏.frx");
            cbxDefautReport.Items.Add("标准样式16K-文字.frx");
        }

        private void rbnOther_Checked(object sender, RoutedEventArgs e)
        {
            Config.ReportFormat = "其他";
            cbxDefautReport.Text = "";
            cbxDefautReport.Items.Clear();
            cbxDefautReport.Items.Add("标准样式16K-一图.frx");
            cbxDefautReport.Items.Add("标准样式16K-文字.frx");
            cbxDefautReport.Items.Add("标准样式A4-一图.frx");
            cbxDefautReport.Items.Add("标准样式A4-二图.frx");
            cbxDefautReport.Items.Add("标准样式A4-文字.frx");
            cbxDefautReport.Items.Add("标准样式B5-一图.frx");
            cbxDefautReport.Items.Add("标准样式B5-两图.frx");
            cbxDefautReport.Items.Add("标准样式B5-文字.frx");

        }
        private void cbxDefautReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxDefautReport.SelectedItem != null)
                Config.DefautReport = cbxDefautReport.SelectedItem.ToString();
        }
        private void cbxSerialPortName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxSerialPortName.SelectedItem != null)
            {
                ComboBoxItem item = (ComboBoxItem)cbxSerialPortName.SelectedItem;
                Config.SerialPortName = item.Content.ToString();
            }
        }
        #endregion

        #region Method
        void Init()
        {
            tbxImagePath.Text = Config.PatientImageBaseDirectory;
            tbxHospitalName.Text = Config.HostpitalName;
            tbxReportName.Text = Config.ReportName;
            tbxReportFoot.Text = Config.ReportFoot;
            switch (Config.ReportFormat)
            {
                case "A4":
                    rbnA4.IsChecked = true;
                    break;
                case "B5":
                    rbnB5.IsChecked = true;
                    break;
                case "16K":
                    rbn16K.IsChecked = true;
                    break;
                case "其他":
                    rbnOther.IsChecked = true;
                    break;
            }
            cbxDefautReport.Text = Config.DefautReport;
            cbxSerialPortName.Text = Config.SerialPortName;
        }
        #endregion

        private void btnBackupDatabase_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"D:\";
            sfd.Filter = "数据库备份文件|*.bak";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string bakFile = sfd.FileName;
                    System.IO.FileInfo file = new System.IO.FileInfo(bakFile);
                    if (file.Exists) file.Delete();
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = "cmd.exe";//要执行的程序名称 
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;//
                    p.StartInfo.RedirectStandardOutput = true;//
                    p.Start();
                    p.StandardInput.WriteLine("sqlcmd -S .\\SQLEXPRESS -Q \"BACKUP DATABASE [Prospect] TO  DISK = N'" + bakFile + "' WITH NOFORMAT, NOINIT,  NAME = N'Prospect-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10\"");
                    p.StandardInput.WriteLine("exit");
                    p.WaitForExit();
                }
                catch (Exception)
                {

                    System.Windows.MessageBox.Show("备份失败！");
                }

                //System.Diagnostics.Process.Start("cmd", "sqlcmd -S .\\SQLEXPRESS -Q \"BACKUP DATABASE [Prospect] TO  DISK = N'D:\\Prospect.bak' WITH NOFORMAT, NOINIT,  NAME = N'Prospect-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10\"");

            }
        }






    }
}
