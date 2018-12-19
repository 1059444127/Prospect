using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FastReport;
using System.IO;

namespace com.cloud.prospect
{
    public partial class ReportControl : UserControl
    {
        private bool FReportRunning;
        private Report FReport;


        public MainViewModel PatientInfo
        { get; set; }



        public ReportControl()
        {
            InitializeComponent();
        }


        #region Event
        void ReportControl_Disposed(object sender, EventArgs e)
        {
            previewControl.Dispose();
            FReport.Dispose();
        }

        private void ReportControl_Load(object sender, EventArgs e)
        {
            FReport = new Report();
            FReport.Preview = previewControl;
            FastReport.Utils.Config.ReportSettings.ShowPerformance = true;
            FastReport.Utils.Config.ReportSettings.StartProgress += new EventHandler(ReportSettings_StartProgress);
            FastReport.Utils.Config.ReportSettings.Progress += new ProgressEventHandler(ReportSettings_Progress);
            FastReport.Utils.Config.ReportSettings.FinishProgress += new EventHandler(ReportSettings_FinishProgress);
            FastReport.Utils.Config.DesignerSettings.Text = "Report";
        }

        private void ReportSettings_StartProgress(object sender, EventArgs e)
        {
        }

        private void ReportSettings_Progress(object sender, ProgressEventArgs e)
        {
            previewControl.ShowStatus(e.Message);
        }

        private void ReportSettings_FinishProgress(object sender, EventArgs e)
        {
        }
        private void evrSetting_DatabaseLogin(object sender, DatabaseLoginEventArgs e)
        {
            e.ConnectionString = DatabaseFactory.CreateDatabase().ConnectionStr;
        }
        #endregion
        #region Method
        public void DesignReport()
        {
            FReport.SetParameterValue("HospitalName", Config.HostpitalName);
            FReport.Design();
        }

        public void PreviewReport(string reportName)
        {
            FReport.Load(reportName);

            if (FReportRunning)
                return;

            FReportRunning = true;
            try
            {
                FReport.SetParameterValue("HospitalName", Config.HostpitalName);
                if (PatientInfo != null)
                {
                    FReport.SetParameterValue("PatientName", PatientInfo.Name);
                    FReport.SetParameterValue("Sex", PatientInfo.Sex);
                    FReport.SetParameterValue("Age", PatientInfo.Age.ToString() + PatientInfo.AgeType);
                    FReport.SetParameterValue("Body", PatientInfo.Examine_Part);
                    FReport.SetParameterValue("Department", PatientInfo.Apply_Department);
                    FReport.SetParameterValue("UltrasoundNumber", PatientInfo.Ultrasound_Number);
                    FReport.SetParameterValue("Finding", PatientInfo.Ultrasound_Finding);
                    FReport.SetParameterValue("Prompt", PatientInfo.Ultrasound_Prompt);
                    FReport.SetParameterValue("ReportName", Config.ReportName);
                    FReport.SetParameterValue("Foot", Config.ReportFoot);
                    FReport.SetParameterValue("ExaminDate", PatientInfo.Examine_Time == null ? "" : ((DateTime)PatientInfo.Examine_Time).ToString("yyyy-MM-dd"));
                }
                try
                {
                    FReport.Show();
                }
                catch (Exception e)
                {
                    System.Threading.Thread.Sleep(500);
                    FReport.Show();
                }

            }
            finally
            {
                FReportRunning = false;
            }
        }
        public void PreviewStatisticReport(string reportName, StatisticReport statisitc)
        {
            FReport.Load(reportName);

            if (FReportRunning)
                return;

            FReportRunning = true;
            try
            {
               
                DataSet ds = statisitc.GetDataSet();
                FReport.RegisterData(ds.Tables[0], "Patient");
                //FReport.GetDataSource("Patient").Enabled = true;
                //(FReport.FindObject("Data1") as DataBand).DataSource = FReport.GetDataSource("Patient");
                try
                {
                    FReport.Show();
                }
                catch (Exception e)
                {
                    System.Threading.Thread.Sleep(500);
                    FReport.Show();
                }

            }
            finally
            {
                FReportRunning = false;
            }
        }
        public void Show()
        {
            try
            {
                FReportRunning = true;
                FReport.Show();
            }
            catch (Exception e)
            {
                System.Threading.Thread.Sleep(500);
                FReport.Show();
            }
            finally
            {
                FReportRunning = false;
            }
        }
        public void SetPicture(string path, int index)
        {
            if (FReportRunning)
                return;
            PictureObject pic = (PictureObject)FReport.FindObject("pic" + index);
            if (pic != null)
            {
                pic.ImageLocation = path;

            }
        }
        public string ExportPDF()
        {
            string rptFile = string.Empty;
            try
            {
                rptFile = Config.PatientImageBaseDirectory + @"\" + ((DateTime)PatientInfo.Examine_Time).ToString("yyyyMMdd") + @"\"
                    + PatientInfo.Patient_Number + @"\" + PatientInfo.Patient_Number + ".pdf";

                FastReport.Export.Pdf.PDFExport export = new FastReport.Export.Pdf.PDFExport();
                FReport.Export(export, rptFile);
            }
            catch (Exception e)
            {
                MessageBox.Show("保存pdf失败。错误！");
                ErrorHandle.HandleException(e, false);
                return null;
            }
            
            return rptFile;
        }
        #endregion


    }
}
