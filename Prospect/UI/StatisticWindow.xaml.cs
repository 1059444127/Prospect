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
using FastReport;
using Microsoft.Win32;
using System.IO;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class StatisticWindow : Window
    {
        private string reportDir;
        private string defautReport;
        private string currentReport;

        public StatisticWindow(MainViewModel patient,
            string strDir, string reportName)
        {
            InitializeComponent();
            ucReport.PatientInfo = patient;
            reportDir = strDir;
            defautReport = reportName;
            DirectoryInfo dir = new DirectoryInfo(reportDir);
            foreach (FileInfo file in dir.GetFiles("*.frx"))
            {
                lbxReport.Items.Add(file);
                lbxReport.DisplayMemberPath = "Name";
            }
        }

        #region Event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (object obj in lbxReport.Items)
            {
                FileInfo file = obj as FileInfo;
                if (defautReport.Equals(file.Name))
                {
                    lbxReport.SelectedItem = obj;
                    ucReport.PreviewReport(file.FullName);
                }
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ucReport.Dispose();
        }

        private void lbxImage_Loaded(object sender, RoutedEventArgs e)
        {


        }
        private void btnDesignReport_Click(object sender, RoutedEventArgs e)
        {
            ucReport.DesignReport();
        }
        private void lbxReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxReport.SelectedItem != null)
            {
                FileInfo file = (FileInfo)lbxReport.SelectedItem;
                StatisticReport statisitc = new StatisticReport()
                {
                    StartDate = DateTime.Parse(dteStart.Text),
                    EndDate = DateTime.Parse(dteEnd.Text)
                };
                ucReport.PreviewStatisticReport(file.FullName, statisitc);
                currentReport = file.FullName;
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            StatisticReport statisitc = new StatisticReport()
            {
                StartDate = DateTime.Parse(dteStart.Text),
                EndDate = DateTime.Parse(dteEnd.Text)
            };
            ucReport.PreviewStatisticReport(currentReport, statisitc);
        }
        #endregion


        #region Method

        #endregion





    }
}
