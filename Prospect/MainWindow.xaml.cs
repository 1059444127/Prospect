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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Runtime.InteropServices;
using System.Windows.Markup;
using System.Windows.Interop;
using System.Threading;
using com.cloud.video;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Collections;
using FastReport;
using System.Configuration;


namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProspectWin : Window
    {
        bool isPressed = false;
        UCSearch ucSearch;
        UCMain ucMain;
        PatientImageUC focusImg;
        VideoController videoController;
        DiagnoseTemplate winTemplate;
        ImageProcessWindow winImage;
        System.Timers.Timer aTimer = new System.Timers.Timer();
        System.Timers.Timer bTimer = new System.Timers.Timer();
        System.IO.Ports.SerialPort serialPort = new System.IO.Ports.SerialPort();
        bool IsMultiThreadMode = false;


        public List<PatientImage> PatientImages;

        public bool IsClosing
        { get; set; }

        public ProspectWin()
        {

            InitializeComponent();
            Init();
        }

        #region PrintXaml
        string GetTemplateXamlCode(Control ctrl)
        {

            FrameworkTemplate template = ctrl.Template;

            string xaml = "";

            if (template != null)
            {

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = new string(' ', 4);
                settings.NewLineOnAttributes = true;

                StringBuilder strbuild = new StringBuilder();
                XmlWriter xmlwrite = XmlWriter.Create(strbuild, settings);

                try
                {
                    XamlWriter.Save(template, xmlwrite);
                    xaml = strbuild.ToString();
                }
                catch (Exception exc)
                {
                    xaml = exc.Message;
                }
            }
            else
            {
                xaml = "no template";
            }

            return xaml;
        }
        //string GetStyleXamlCode(Control ctrl)
        //{

        //    Style template = ctrl.Style;

        //    string xaml = "";

        //    if (template != null)
        //    {

        //        XmlWriterSettings settings = new XmlWriterSettings();
        //        settings.Indent = true;
        //        settings.IndentChars = new string(' ', 4);
        //        settings.NewLineOnAttributes = true;

        //        StringBuilder strbuild = new StringBuilder();
        //        XmlWriter xmlwrite = XmlWriter.Create(strbuild, settings);

        //        try
        //        {
        //            XamlWriter.Save(template, xmlwrite);
        //            xaml = strbuild.ToString();
        //        }
        //        catch (Exception exc)
        //        {
        //            xaml = exc.Message;
        //        }
        //    }
        //    else
        //    {
        //        xaml = "no template";
        //    }

        //    return xaml;
        //}
        #endregion

        #region Event
        private void muiPaste_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (PatientImage img in Clipboard.lstImage)
                {

                    if (img != null && !string.IsNullOrEmpty(img.Image_Path))
                    {
                        FileInfo file = new FileInfo(img.Image_Path);
                        if (file.Exists)
                        {
                            string baseDir = Config.PatientImageBaseDirectory + @"\" +
                                ((DateTime)ucMain.mainViewModel.Examine_Time).ToString("yyyyMMdd") + @"\" + ucMain.mainViewModel.Patient_Number + @"\";
                            PatientImage newImage = ucMain.mainViewModel.AddImage(baseDir, img.Image_Format, img.Image_Type);
                            file.CopyTo(newImage.Image_Path);
                            file.Delete();
                            img.DeleteByKey();
                            AddImage(newImage);
                        }
                    }
                }
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
            finally
            {
                Clipboard.lstImage.Clear();
            }

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tc.Content = ucMain;
            ucMain.Margin = tc.Margin;
            ucMain.Height = ControlHeight.ActualHeight;
            ucMain.ShowPatientImages(ucMain.mainViewModel.Patient_Number);
            ucMain.Owner = this;
            ucSearch.Height = ControlHeight.ActualHeight;
            aTimer.Start();
            bTimer.Start();
            
        }

        void winTemplate_ApplyTemplateEvent(object sender, string finding, string prompt)
        {
            if (!String.IsNullOrEmpty(finding.Trim()))
                ucMain.mainViewModel.Ultrasound_Finding += finding + "\r\n";
            if (!String.IsNullOrEmpty(prompt.Trim()))
                ucMain.mainViewModel.Ultrasound_Prompt += prompt + "\r\n";

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IsClosing = true;
            winTemplate.Close();
            if (winImage != null)
            {
                winImage.Close();
            }
            aTimer.Close();
            serialPort.Close();
            Config.SaveConfig();
        }

        void ucSearch_SelectPatientEvent(object sender, string patientNumber)
        {
            RefreshPatientInformation(patientNumber);
        }

        void ucSearch_DoubleClickPatientEvent(object sender, EventArgs args)
        {
            btnReturn.Visibility = System.Windows.Visibility.Hidden;
            btnDelete.IsEnabled = true;
            tc.Content = ucMain;
        }

        #region Image Listbox Event
        void uc_PlayVideoEvent(object sender, EventArgs e)
        {
            if (focusImg != null && focusImg.ImgPatient.Image_Type.Equals("Video"))
            {
                Int64 duration = 0;
                videoController.PlayVideo(focusImg.ImgPatient.Image_Path);
                videoController.GetDuration(ref duration);
                ucMain.StartPlayVideo(duration);
            }
        }

        void uc_ImageAddedEventHandler(object sender, PatientImage imgPatient)
        {
            if (IsMultiThreadMode)
            {
                AddImage2(imgPatient);
            }
            else
            {
                AddImage(imgPatient);
            }
        }
        void ucMain_ShowImageEvent(object sender, PatientImage imgPatient)
        {
            AddImage(imgPatient);
        }


        void img_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            focusImg = sender as PatientImageUC;
        }

        void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            focusImg = sender as PatientImageUC;
            if (e.ClickCount == 2)
            {
                PatientImageUC img = sender as PatientImageUC;
                if (img.ImgPatient.Image_Path == null) return;
                if (img.ImgPatient.Image_Type.Equals("Image"))
                {
                    if (winImage == null || !winImage.IsLoaded)
                    {
                        winImage = new ImageProcessWindow(img.ImgPatient);
                        winImage.Owner = this;
                        winImage.Closed += new EventHandler(imgWin_Closed);
                        winImage.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                        winImage.Show();
                    }
                    else if (winImage.IsLoaded)
                    {
                        winImage.LoadImage(img.ImgPatient);
                    }
                }
                else if (img.ImgPatient.Image_Type.Equals("Video"))
                {
                    if (videoController.Status != VideoStatus.RecordVideo)
                    {
                        Int64 duration = 0;
                        videoController.PlayVideo(img.ImgPatient.Image_Path);
                        videoController.GetDuration(ref duration);
                        ucMain.StartPlayVideo(duration);
                    }
                    else
                    {
                        MessageBox.Show("录像中，不能播放视频！", "提醒", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        void imgWin_Closed(object sender, EventArgs e)
        {
            RefreshImages(ucMain.mainViewModel.Patient_Number);
        }


        void item4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object[] selected_objs = new object[stkImage.SelectedItems.Count];
                stkImage.SelectedItems.CopyTo(selected_objs, 0);
                foreach (var obj in selected_objs)
                {
                    if (obj != null)
                    {
                        StackPanel stk = obj as StackPanel;
                        PatientImageUC img = stk.Children[0] as PatientImageUC;
                        if (img.Source == null) continue;
                        Clipboard.lstImage.Add(img.ImgPatient);
                    }
                }
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
        }

        void item3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(focusImg.ImgPatient.Image_Path)) return;
                string filePath = focusImg.ImgPatient.Image_Path;
                FileInfo file = new FileInfo(filePath);
                System.Diagnostics.Process.Start("Explorer.exe", file.Directory.FullName);
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
        }

        void item2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object[] selected_objs = new object[stkImage.SelectedItems.Count];
                stkImage.SelectedItems.CopyTo(selected_objs, 0);
                foreach (var obj in selected_objs)
                {
                    if (obj != null)
                    {
                        StackPanel stk = obj as StackPanel;
                        PatientImageUC img = stk.Children[0] as PatientImageUC;
                        if (img.Source == null) continue;
                        if (!String.IsNullOrEmpty(img.ImgPatient.Image_Path))
                        {
                            string file = img.ImgPatient.Image_Path;
                            System.IO.File.Delete(file);
                        }
                        img.ImgPatient.DeleteByKey();
                        img.Source = null;
                        stkImage.Items.Remove(obj);
                        PatientImages.Remove(img.ImgPatient);
                        img = null;
                    }
                }
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
        }

        void item_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (focusImg != null)
                {
                    if (!String.IsNullOrEmpty(focusImg.ImgPatient.Image_Path))
                    {
                        string file = focusImg.ImgPatient.Image_Path;
                        System.IO.File.Delete(file);
                    }
                    focusImg.ImgPatient.DeleteByKey();
                    focusImg.Source = null;
                    stkImage.Items.Remove(focusImg.Parent);
                    PatientImages.Remove(focusImg.ImgPatient);
                    focusImg = null;
                }
            }
            catch (FileNotFoundException e1)
            {
                ErrorHandle.HandlerWarning(e1.Message + "\r\n" + e1.StackTrace);
            }
            catch (Exception e2)
            {
                ErrorHandle.HandleException(e2, false);
            }
        }
        #endregion

        #region ToolButton Event
        private void btnMultipleThread_Click(object sender, RoutedEventArgs e)
        {
            IsMultiThreadMode = IsMultiThreadMode ? false : true;
            if (IsMultiThreadMode)
            {
                scrolls2.Width = 120;
                ucMain.IsMultiThreadMode = true;
            }
            else
            {
                scrolls2.Width = 0;
                stkImage2.Items.Clear();
                ucMain.IsMultiThreadMode = false;
                ucMain.IsSecondPatient = false;
                btnLatestRecord_Click(null, null);
            }
        }
        private void btnReportTemplate_Click(object sender, RoutedEventArgs e)
        {

            winTemplate.Owner = this;
            winTemplate.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            winTemplate.Top = 50;
            winTemplate.Show();
            winTemplate.WindowState = System.Windows.WindowState.Normal;
            winTemplate.Visibility = System.Windows.Visibility.Visible;
        }
        private void btnLastRecord_Click(object sender, RoutedEventArgs e)
        {
            ucMain.mainViewModel.GetLastRecord();
            RefreshPatientInformation(ucMain.mainViewModel.Patient_Number);
            ucSearch.SearchCurrentInfo();
        }
        private void btnNextRecord_Click(object sender, RoutedEventArgs e)
        {
            ucMain.mainViewModel.GetNextRecord();
            RefreshPatientInformation(ucMain.mainViewModel.Patient_Number);
            ucSearch.SearchCurrentInfo();
        }
        private void btnPrintReport_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ucMain.mainViewModel.Patient_Number))
            {
                ErrorHandle.HandlerWarning("请先录入一个病人信息!");
                return;
            }
            try
            {
                string dir = AppDomain.CurrentDomain.BaseDirectory + @"\reports\" + Config.ReportFormat + @"\";
                string name = Config.DefautReport;
                ReportWindow winReport = new ReportWindow(ucMain.mainViewModel, PatientImages,
                    dir, name);
                winReport.Owner = this;
                winReport.ShowDialog();
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
        }
        private void btnStatic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string dir = AppDomain.CurrentDomain.BaseDirectory + @"\reports\统计\";
                string name = "所有科室月总收入总人数.frx";
                StatisticWindow winReport = new StatisticWindow(ucMain.mainViewModel,
                    dir, name);
                winReport.Owner = this;
                winReport.ShowDialog();
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
        }
        private void btnLatestRecord_Click(object sender, RoutedEventArgs e)
        {
            ucMain.mainViewModel.GetLatestRecord();
            RefreshPatientInformation(ucMain.mainViewModel.Patient_Number);
            ucSearch.SearchCurrentInfo();
        }
        private void btnFirstPatient_Click(object sender, RoutedEventArgs e)
        {
            ucMain.mainViewModel.GetFirstRecord();
            RefreshPatientInformation(ucMain.mainViewModel.Patient_Number);
            ucSearch.SearchCurrentInfo();
        }
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            btnReturn.Visibility = System.Windows.Visibility.Hidden;
            btnDelete.IsEnabled = true;
            tc.Content = ucMain;
            RefreshPatientInformation(ucMain.mainViewModel.Patient_Number);
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ucMain.Dispose();
            btnReturn.Visibility = System.Windows.Visibility.Visible;
            btnDelete.IsEnabled = false;
            tc.Content = ucSearch;
        }
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            bTimer.Stop();
            HelpWindow win = new HelpWindow();
            win.Owner = this;
            win.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            bTimer.Start();

        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            GetTemplateXamlCode(stkImage);
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ucMain.mainViewModel.Patient_Number))
            {
                ErrorHandle.HandlerWarning("请先录入一个病人信息!");
                return;
            }
            try
            {
                ucMain.Save();
                ExportPDF();
                MessageBox.Show("保存信息成功!");
            }
            catch (Exception e1)
            {
                ErrorHandle.HandlerWarning("保存信息失败!");
                ErrorHandle.HandleException(e1, false);
            }

        }
        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("请确认是否要退出系统？",
                                         "退出确认", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
        private void btnNewPatient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ucMain.mainViewModel.Patient_Number != null)
                {
                    ucMain.Save();
                    ExportPDF();
                }
                AddPatient window = new AddPatient();
                window.Owner = this;
                Window.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
                window.Left = 200;
                window.Top = 10;
                window.ShowDialog();
            }
            catch (Exception e1)
            {

                ErrorHandle.HandleException(e1, false);
            }

        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ucMain.mainViewModel.Patient_Number))
            {
                ErrorHandle.HandlerWarning("请先录入一个病人信息!");
                return;
            }
            UpdatePatient window = new UpdatePatient(ucMain.mainViewModel.Patient_Number);
            window.Owner = this;
            window.ShowDialog();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(ucMain.mainViewModel.Patient_Number))
            {
                MessageBoxResult result = MessageBox.Show("请确认是否删除" + ucMain.mainViewModel.Name + "的信息？",
                                             "删除确认", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        ucMain.StopRecordVideo();
                        string patientNumber = ucMain.mainViewModel.Patient_Number;
                        ucMain.mainViewModel.DeletePatientImages();
                        DirectoryInfo dir = new DirectoryInfo(Config.PatientImageBaseDirectory + @"\" 
                            + ((DateTime)ucMain.mainViewModel.Examine_Time).ToString("yyyyMMdd") + @"\"
                            + patientNumber);
                        if (dir.Exists)
                            dir.Delete(true);
                        ucMain.mainViewModel.DeleteByKey(patientNumber);
                        ucMain.mainViewModel.GetLatestRecord();
                        RefreshPatientInformation(ucMain.mainViewModel.Patient_Number);
                    }
                    catch (Exception e1)
                    {
                        ErrorHandle.HandleException(e1, false);
                    }

                }
            }
        }
        #endregion

        #endregion

        #region Method
        void AddImage(PatientImage imgPatient)
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
                        img.Source = new BitmapImage(
                            new Uri("pack://application:,,,/Prospect;Component/img/video.png"));
                    }
                else
                {
                    img.Source = new BitmapImage(
                        new Uri("pack://application:,,,/Prospect;Component/img/noimage.png"));
                    imgPatient.Image_Path = null;
                }
                img.ImgPatient = imgPatient;
                // img.MouseRightButtonDown += new MouseButtonEventHandler(img_MouseRightButtonDown);

                //item.InputGestureText = "Ctrl+X";
                //KeyBinding keyBin = new KeyBinding();
                //keyBin.Command = UserCommands.DeleteImage;
                //keyBin.Gesture = new KeyGesture(Key.X,ModifierKeys.Control);
                //menu.InputBindings.Add(keyBin);

                //border.ContextMenu = menu;
                ContextMenu menu = new System.Windows.Controls.ContextMenu();
                MenuItem item = new MenuItem();
                item.Click += item_Click;
                item.Header = "删除";
                menu.Items.Add(item);
                MenuItem item2 = new MenuItem();
                item2.Click += item2_Click;
                item2.Header = "删除所选图片";
                menu.Items.Add(item2);
                MenuItem item3 = new MenuItem();
                item3.Click += item3_Click;
                item3.Header = "打开图像文件目录";
                menu.Items.Add(item3);
                MenuItem item4 = new MenuItem();
                item4.Click += new RoutedEventHandler(item4_Click);
                item4.Header = "复制所选图像";
                menu.Items.Add(item4);
                MenuItem item5 = new MenuItem();
                item5.Click += new RoutedEventHandler(muiPaste_Click);
                item5.Header = "粘贴";
                item5.Loaded += new RoutedEventHandler(item5_Loaded);
                menu.Items.Add(item5);

                img.ContextMenu = menu;
                img.MouseRightButtonDown += new MouseButtonEventHandler(img_MouseRightButtonDown);
                img.MouseLeftButtonDown += new MouseButtonEventHandler(img_MouseLeftButtonDown);

                StackPanel stk = new StackPanel();
                stk.Orientation = Orientation.Vertical;
                stk.Children.Add(img);
                TextBlock tbk = new TextBlock();
                tbk.Width = img.Width;
                tbk.TextAlignment = TextAlignment.Center;
                tbk.FontSize = 12;
                tbk.FontWeight = FontWeights.Bold;
                tbk.Foreground = imgPatient.Image_Path != null ?
                    imgPatient.Image_Type.Equals("Image") ?
                    System.Windows.Media.Brushes.Green :
                    System.Windows.Media.Brushes.Blue :
                    System.Windows.Media.Brushes.Red;
                tbk.Text = imgPatient.Image_Number.ToString() + "." + imgPatient.Image_Format;
                stk.Children.Add(tbk);
                stkImage.Items.Add(stk);
                PatientImages.Add(imgPatient);
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);
            }

        }

        void item5_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem mui = sender as MenuItem;
                if (Clipboard.lstImage.Count > 0)
                    mui.IsEnabled = true;
                else
                    mui.IsEnabled = false;
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }

        }



        void AddImage2(PatientImage imgPatient)
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
                        img.Source = new BitmapImage(
                            new Uri("pack://application:,,,/Prospect;Component/img/video.png"));
                    }
                else
                {
                    img.Source = new BitmapImage(
                        new Uri("pack://application:,,,/Prospect;Component/img/noimage.png"));
                    imgPatient.Image_Path = null;
                }

                StackPanel stk = new StackPanel();
                stk.Orientation = Orientation.Vertical;
                stk.Children.Add(img);
                stkImage2.Items.Add(stk);

            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);
            }

        }
        public void RefreshPatientInformation(string patientNumber)
        {
            RefreshImages(patientNumber);
            ucMain.RefreshPatientInformation(patientNumber);
        }
        public void RefreshImages(string patientNumber)
        {
            while (stkImage.Items.Count > 0)
            {
                StackPanel stk = (StackPanel)stkImage.Items[0];
                PatientImageUC img = (PatientImageUC)stk.Children[0];
                img.Source = null;
                stkImage.Items.RemoveAt(0);
            }
            PatientImages.Clear();
            ucMain.ShowPatientImages(patientNumber);
        }
        public void Init()
        {
            try
            {
                videoController = VideoControllerFactory.CreateInstance();
                ucMain = new UCMain();
                ucMain.ImageAddedEvent += new UCMain.ImageAddedEventHandler(uc_ImageAddedEventHandler);
                ucMain.ShowImageEvent += new UCMain.ShowImageEventHandler(ucMain_ShowImageEvent);
                ucMain.PlayVideoEvent += new UCMain.PlayVideoEventHandler(uc_PlayVideoEvent);
                ucSearch = new UCSearch(ucMain.mainViewModel);
                ucSearch.SelectPatientEvent += new UCSearch.SelectPatientEventHandler(ucSearch_SelectPatientEvent);
                ucSearch.DoubleClickPatientEvent +=
                    new UCSearch.DoubleClickPatientEventHandler(ucSearch_DoubleClickPatientEvent);
                winTemplate = new DiagnoseTemplate();
                winTemplate.ApplyTemplateEvent += new DiagnoseTemplate.ApplyTemplateEventHandler(winTemplate_ApplyTemplateEvent);
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e);
            }

            PatientImages = new List<PatientImage>();
            try
            {
                Config.LoadCurrentConfig();
                Config.EnableFtp = bool.Parse(ConfigurationManager.AppSettings["EnableFtp"]);
                Config.ftpIP = ConfigurationManager.AppSettings["FtpIP"];
                Config.ftpUser = ConfigurationManager.AppSettings["FtpUser"];
                Config.ftpPassword = ConfigurationManager.AppSettings["FtpPassword"];
                Config.ftpWebServiceIP = ConfigurationManager.AppSettings["FtpWebServiceIP"];
            }
            catch (Exception e)
            {

                ErrorHandle.HandleException(e, false);
            }


            //SerialComm Monitor
            serialPort.PortName = Config.SerialPortName;
            serialPort.DtrEnable = true;
            try
            {
                serialPort.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(serialPort.PortName + "不可用！请在选项设置界面里面重新选择COM端口。");
            }
            aTimer.Interval = 100;
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(aTimer_Elapsed);
            bTimer.Interval = 2000;
            bTimer.Elapsed += new System.Timers.ElapsedEventHandler(bTimer_Elapsed);


            Information.ExitEvent += new Information.ExitEventHandler(Information_ExitEvent);

        }

        void Information_ExitEvent()
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                if (!IsClosing)
                {
                    MessageBox.Show("检测U盾失败，请检查U盾是否插入？");
                    this.Close();
                }
            }), new object[] { });


        }

        void aTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            aTimer.Stop();
            if (serialPort != null && serialPort.IsOpen)
            {
                if (serialPort.DsrHolding && isPressed == false)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ucMain.CaptureImage();
                    }));
                    isPressed = true;
                }
                if (!serialPort.DsrHolding)
                {
                    isPressed = false;
                }
            }
            aTimer.Start();
        }
        void bTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Thread.Sleep(1000);
            if (!Information.IsWriteRead)
            {
                Information.GetInformation();
            }
        }
        void ExportPDF()
        {
            string pdfFileName = string.Empty;
            if (PatientImages.Count == 0)
                pdfFileName = AppDomain.CurrentDomain.BaseDirectory + @"\Reports\A4\标准样式A4-文字.frx";
            if (PatientImages.Count == 1)
                pdfFileName = AppDomain.CurrentDomain.BaseDirectory + @"\Reports\A4\标准样式A4-一图.frx";
            else
                pdfFileName = AppDomain.CurrentDomain.BaseDirectory + @"\Reports\A4\标准样式A4-二图.frx";

            Report rpt = new Report();
            try
            {

                MainViewModel PatientInfo = ucMain.mainViewModel;
                string rptFile = Config.PatientImageBaseDirectory + @"\" + ((DateTime)PatientInfo.Examine_Time).ToString("yyyyMMdd") + @"\"
                    + PatientInfo.Patient_Number + @"\" + PatientInfo.Patient_Number + ".pdf";
                FileInfo file = new FileInfo(rptFile);
                if (!file.Directory.Exists) file.Directory.Create();

                rpt.Load(pdfFileName);
                rpt.SetParameterValue("HospitalName", Config.HostpitalName);
                if (PatientInfo != null)
                {
                    rpt.SetParameterValue("PatientName", PatientInfo.Name);
                    rpt.SetParameterValue("Sex", PatientInfo.Sex);
                    rpt.SetParameterValue("Age", PatientInfo.Age.ToString() + PatientInfo.AgeType);
                    rpt.SetParameterValue("Body", PatientInfo.Examine_Part);
                    rpt.SetParameterValue("Department", PatientInfo.Apply_Department);
                    rpt.SetParameterValue("UltrasoundNumber", PatientInfo.Ultrasound_Number);
                    rpt.SetParameterValue("Finding", PatientInfo.Ultrasound_Finding);
                    rpt.SetParameterValue("Prompt", PatientInfo.Ultrasound_Prompt);
                    rpt.SetParameterValue("ReportName", Config.ReportName);
                    rpt.SetParameterValue("Foot", Config.ReportFoot);
                    rpt.SetParameterValue("ExaminDate", PatientInfo.Examine_Time == null ? "" : ((DateTime)PatientInfo.Examine_Time).ToString("yyyy-MM-dd"));
                    if (PatientImages.Count == 1)
                    {
                        PictureObject pic = (PictureObject)rpt.FindObject("pic" + 1);
                        pic.ImageLocation = PatientImages[0].Image_Path;
                    }
                    else if (PatientImages.Count > 1)
                    {
                        PictureObject pic = (PictureObject)rpt.FindObject("pic" + 1);
                        PictureObject pic2 = (PictureObject)rpt.FindObject("pic" + 2);
                        pic.ImageLocation = PatientImages[0].Image_Path;
                        pic2.ImageLocation = PatientImages[1].Image_Path;
                    }

                }
                rpt.Prepare();
                FastReport.Export.Pdf.PDFExport export = new FastReport.Export.Pdf.PDFExport();
                rpt.Export(export, rptFile);
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);
            }
            finally
            {
                rpt.Dispose();
            }
        }
        
        #endregion

      

        private void cm_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Clipboard.lstImage.Count > 0)
                    muiPaste.IsEnabled = true;
                else
                    muiPaste.IsEnabled = false;
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }

        }

     
    }
}
