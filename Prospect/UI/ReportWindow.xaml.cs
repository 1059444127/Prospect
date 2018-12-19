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
    public partial class ReportWindow : Window
    {
        private List<PatientImage> pictures;
        private string reportDir;
        private string defautReport;
        private MainViewModel _patient;

        public ReportWindow(MainViewModel patient, List<PatientImage> _pictures,
            string strDir, string reportName)
        {
            InitializeComponent();
            ucReport.PatientInfo = patient;
            _patient = patient;
            pictures = _pictures;

            reportDir = strDir;
            defautReport = reportName;
            DirectoryInfo dir = new DirectoryInfo(reportDir);
            foreach (FileInfo file in dir.GetFiles("*.frx"))
            {
                lbxReport.Items.Add(file);
                lbxReport.DisplayMemberPath = "Name";
            }

            foreach (PatientImage img in pictures)
            {
                AddPatientImage(img);
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
                    lbxImage.UnselectAll();
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
            lbxImage.Height = this.ActualHeight - (this.ActualHeight - ucReport.Height + 10);

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
                lbxImage.UnselectAll();
                ucReport.PreviewReport(file.FullName);
            }
        }
        private void lbxImage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = 1;
            foreach (object obj in lbxImage.Items)
            {
                if (lbxImage.SelectedItems.Contains(obj))
                {
                    StackPanel stk = obj as StackPanel;
                    ((CheckBox)stk.Children[1]).IsChecked = true;
                    ucReport.SetPicture(((PatientImageUC)stk.Children[0]).ImgPatient.Image_Path, i);
                    i++;
                }
                else
                {
                    StackPanel stk = obj as StackPanel;
                    ((CheckBox)stk.Children[1]).IsChecked = false;
                }

            }
            ucReport.Show();
        }
        private void btnSavePDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string rptFile = ucReport.ExportPDF();
                if (Config.EnableFtp)
                {
                    UploadFtp();
                }

                MessageBox.Show("保存pdf成功！");
            }
            catch (Exception ex)
            {
                ErrorHandle.HandlerWarning(ex.Message);

            }

        }
        #endregion

        #region Method
        void AddPatientImage(PatientImage imgPatient)
        {
            PatientImageUC img = null;

            try
            {
                img = new PatientImageUC();
                img.MaxHeight = 80;
                FileInfo file = new FileInfo(imgPatient.Image_Path);
                if (file.Exists)
                    if (imgPatient.Image_Type.Equals("Image"))
                    {

                        using (FileStream fs = new FileStream(imgPatient.Image_Path, FileMode.Open))
                        {
                            using (System.Drawing.Image drawingImage = System.Drawing.Image.FromStream(fs))
                            {
                                using (System.Drawing.Image thumbImage = drawingImage.GetThumbnailImage(50, 50,
                                    () => { return true; }, IntPtr.Zero))
                                {
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        thumbImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                        BitmapImage bi = new BitmapImage();
                                        bi.BeginInit();
                                        bi.CacheOption = BitmapCacheOption.OnLoad;
                                        bi.StreamSource = ms;
                                        bi.EndInit();
                                        img.Source = bi;

                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        return;
                    }
                else
                {
                    return;
                }
                img.ImgPatient = imgPatient;
                StackPanel stk = new StackPanel();
                stk.Orientation = Orientation.Vertical;
                stk.Children.Add(img);
                CheckBox cbx = new CheckBox();
                cbx.Width = img.Width;
                cbx.FontSize = 12;
                cbx.Content = imgPatient.Image_Number.ToString() + "." + imgPatient.Image_Format;
                stk.Children.Add(cbx);
                lbxImage.Items.Add(stk);
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);
            }
        }
        void UploadFtp()
        {
            string dir = "/PIS" + DateTime.Now.ToString("yyyyMMdd") + @"/" + _patient.Patient_Number + @"/PictureDir/";
            FTPUtil ftp = new FTPUtil(Config.ftpIP, "", Config.ftpUser, Config.ftpPassword);
            StringBuilder imgPartXml = new StringBuilder();
            foreach (object obj in lbxImage.Items)
            {
                if (lbxImage.SelectedItems.Contains(obj))
                {

                    StackPanel stk = obj as StackPanel;
                    ((CheckBox)stk.Children[1]).IsChecked = true;
                    PatientImage img = ((PatientImageUC)stk.Children[0]).ImgPatient;
                    string fileName = Guid.NewGuid().ToString() + "." + img.Image_Format;
                    string fileFullName = Config.PatientImageBaseDirectory + @"\" + img.Patient_Number + @"\" + fileName;
                    FileInfo file = new FileInfo(img.Image_Path);
                    if (file.Exists)
                    {
                        try
                        {
                            file.CopyTo(fileFullName, true);
                            ftp.GotoDirectory("", true);
                            if (!ftp.DirectoryExist("PIS" + DateTime.Now.ToString("yyyyMMdd")))
                            {
                                ftp.MakeDir("PIS" + DateTime.Now.ToString("yyyyMMdd"));

                            }
                            ftp.GotoDirectory("PIS" + DateTime.Now.ToString("yyyyMMdd"), false);
                            if (!ftp.DirectoryExist(_patient.Patient_Number))
                            {
                                ftp.MakeDir(_patient.Patient_Number);
                            }
                            ftp.GotoDirectory(_patient.Patient_Number, false);
                            if (!ftp.DirectoryExist("PictureDir"))
                            {
                                ftp.MakeDir("PictureDir");
                            }
                            ftp.GotoDirectory("PictureDir", false);
                            ftp.Upload(fileFullName);
                          
                            imgPartXml.Append(@"<ImagePath><HttpPath>http://");
                            imgPartXml.Append(Config.ftpWebServiceIP);
                            imgPartXml.Append(dir);
                            imgPartXml.Append(fileName);
                            imgPartXml.Append(@"</HttpPath><FtpPath>ftp://");
                            imgPartXml.Append(Config.ftpWebServiceIP);
                            imgPartXml.Append(dir);
                            imgPartXml.Append(fileName);
                            imgPartXml.Append(@"</FtpPath></ImagePath>");
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        finally
                        {
                            FileInfo newFile = new FileInfo(fileFullName);
                            if (newFile.Exists)
                            {
                                newFile.Delete();
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(imgPartXml.ToString()))
            {
                string xml = CreateXML(imgPartXml.ToString());
                Logger.WriteLogs(LogServerity.Debug, "Ftp Web Service:" + xml);
                FtpWebService.WsPacsTransSoapClient client = new FtpWebService.WsPacsTransSoapClient();
                client.SaveCheckImages(xml);
            }

        }
        string CreateXML(string imgPart)
        {
            StringBuilder xmlBuilder = new StringBuilder();
            xmlBuilder.Append(@"<CheckImages>");
            xmlBuilder.Append(@"<CheckItemsID>");
            xmlBuilder.Append("151080");
            xmlBuilder.Append(@"</CheckItemsID>");
            xmlBuilder.Append(@"<UserInfoID>1001</UserInfoID><ImagePathList>");
            xmlBuilder.Append(imgPart);
            xmlBuilder.Append(@"</ImagePathList></CheckImages>");
            return xmlBuilder.ToString();
        }
        #endregion







    }
}
