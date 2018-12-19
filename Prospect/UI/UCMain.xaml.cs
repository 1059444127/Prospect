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
using System.Windows.Interop;
using System.Runtime.InteropServices;
using com.cloud.video;
using System.Timers;
using System.Windows.Threading;
using System.Xml;
using System.Windows.Markup;
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for ucMain.xaml
    /// </summary>
    public partial class UCMain : UserControl, IDisposable
    {

        public bool IsMultiThreadMode
        { get; set; }
        public bool IsSecondPatient
        { get; set; }
        public Patient Patient2
        { get; private set; }


        public delegate void ImageAddedEventHandler(object sender,
            PatientImage imgPatient);
        public event ImageAddedEventHandler ImageAddedEvent;
        public delegate void PlayVideoEventHandler(object sender, EventArgs e);
        public event PlayVideoEventHandler PlayVideoEvent;
        public delegate void ShowImageEventHandler(object sender,
            PatientImage imgPatient);
        public event ShowImageEventHandler ShowImageEvent;

        #region Members
        VideoControlHost videoControl;
        VideoController videoController;
        Timer aTimer;
        Timer tmrVideoRecord;
        bool isStartDrag;
        bool isFirstEnterDS;
        ToolTip dsTip;
        #endregion

        #region Properties
        public IntPtr HwndVidoeControl
        { get; private set; }
        public Window Owner
        { get; set; }
        public MainViewModel mainViewModel
        { get; private set; }
        #endregion

        public UCMain()
        {
            InitializeComponent();
            InitViewModel();
            IsSecondPatient = false;
            DataObject.AddPastingHandler(tbxFinding, new DataObjectPastingEventHandler(OnPaste_Finding));
            DataObject.AddPastingHandler(tbxPrompt, new DataObjectPastingEventHandler(OnPaste_Prompt));
        }

        private void OnPaste_Finding(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(System.Windows.DataFormats.Text, true);
            if (!isText) return;



            var text = e.SourceDataObject.GetData(DataFormats.Text) as string;

            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, text);
            tbxFinding.SelectionStart = start + text.Length;
            e.CancelCommand();
        }

        private void OnPaste_Prompt(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(System.Windows.DataFormats.Text, true);
            if (!isText) return;

            var text = e.SourceDataObject.GetData(DataFormats.Text) as string;

            int start = tbxPrompt.SelectionStart;
            tbxPrompt.Text = tbxPrompt.Text.Insert(start, text);
            tbxPrompt.SelectionStart = start + text.Length;
            e.CancelCommand();
        }


        #region Event
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
           
        }
        private void cbxDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxDepartment.SelectedItem != null)
            {
                Department dept = (Department)cbxDepartment.SelectedItem;
                Doctor doctor = new Doctor()
                {
                    Department_Number = dept.Department_Number
                };
                cbxApplyDoctor.ItemsSource = new ObservableCollection<Doctor>(doctor.SelectApplyDoctors());
                cbxApplyDoctor.DisplayMemberPath = "Name";
                cbxApplyDoctor.SelectedValuePath = "Doctor_Number";

                cbxRecordDoctor.ItemsSource = new ObservableCollection<Doctor>(doctor.SelectRecordDoctors());
                cbxRecordDoctor.DisplayMemberPath = "Name";
                cbxRecordDoctor.SelectedValuePath = "Doctor_Number";

                cbxDiagnosisDoctor.ItemsSource = new ObservableCollection<Doctor>(doctor.SelectDiagnosisDoctors());
                cbxDiagnosisDoctor.DisplayMemberPath = "Name";
                cbxDiagnosisDoctor.SelectedValuePath = "Doctor_Number";

            }
        }

        private void btnFinding_Click(object sender, RoutedEventArgs e)
        {
            SimpleTemplate1 win = new SimpleTemplate1();
            win.Owner = this.Owner;
            win.ApplyTemplateEvent += new SimpleTemplate1.ApplyTemplateEventHandler(template1_ApplyTemplateEvent);
            win.ShowDialog();
        }
        private void btnPrompt_Click(object sender, RoutedEventArgs e)
        {
            SimpleTemplate2 win = new SimpleTemplate2();
            win.Owner = this.Owner;
            win.ApplyTemplateEvent += new SimpleTemplate2.ApplyTemplateEventHandler(template2_ApplyTemplateEvent);
            win.ShowDialog();
        }
        public void cbxPositive_CheckedChanged(object sender, EventArgs args)
        {
            cbxNegative.IsChecked = cbxPostive.IsChecked == true ? false : cbxNegative.IsChecked;
            if (cbxPostive.IsChecked == true)
                mainViewModel.IsPositive = true;
            else if (cbxNegative.IsChecked == true)
                mainViewModel.IsPositive = false;
            else
                mainViewModel.IsPositive = default(bool?);
        }
        public void cbxNegative_CheckedChanged(object sender, EventArgs args)
        {
            cbxPostive.IsChecked = cbxNegative.IsChecked == true ? false : cbxPostive.IsChecked;
            if (cbxPostive.IsChecked == true)
                mainViewModel.IsPositive = true;
            else if (cbxNegative.IsChecked == true)
                mainViewModel.IsPositive = false;
            else
                mainViewModel.IsPositive = default(bool?);
        }
        void template2_ApplyTemplateEvent(object sender, string prompt)
        {
            mainViewModel.Ultrasound_Prompt += prompt + "\r\n";
        }
        void template1_ApplyTemplateEvent(object sender, string finding)
        {
            mainViewModel.Ultrasound_Finding += finding + "\r\n";
        }
        private void tbxFinding_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                int start = tbxFinding.SelectionStart;
                tbxFinding.Text = tbxFinding.Text.Insert(start, "\r\n");
                tbxFinding.SelectionStart = start + 1;
            }
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                if (tbxFinding.Text.IndexOf("____") >= 0)
                    if (tbxFinding.SelectedText.Equals("____"))
                    {
                        if (tbxFinding.Text.IndexOf("____", tbxFinding.SelectionStart + tbxFinding.SelectionLength) >= 0)
                            tbxFinding.Select(tbxFinding.Text.IndexOf("____",
                                tbxFinding.SelectionStart + tbxFinding.SelectionLength), 4);
                        else
                            tbxFinding.Select(tbxFinding.Text.IndexOf("____"), 4);
                    }
                    else
                    {
                        tbxFinding.Select(tbxFinding.Text.IndexOf("____"), 4);
                    }
            }
        }
        private void tbxPrompt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                int start = tbxPrompt.SelectionStart;
                tbxPrompt.Text = tbxPrompt.Text.Insert(start, "\r\n");
                tbxPrompt.SelectionStart = start + 1;
            }
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                if (tbxPrompt.Text.IndexOf("____") >= 0)
                    if (tbxPrompt.SelectedText.Equals("____"))
                    {
                        if (tbxPrompt.Text.IndexOf("____", tbxPrompt.SelectionStart + tbxPrompt.SelectionLength) >= 0)
                            tbxPrompt.Select(tbxPrompt.Text.IndexOf("____",
                                tbxPrompt.SelectionStart + tbxPrompt.SelectionLength), 4);
                        else
                            tbxPrompt.Select(tbxPrompt.Text.IndexOf("____"), 4);
                    }
                    else
                    {
                        tbxPrompt.Select(tbxPrompt.Text.IndexOf("____"), 4);
                    }
            }
        }
        private void trvTemplate_Selected(object sender, RoutedEventArgs e)
        {
            if (trvTemplate.SelectedItem != null)
            {
                MainTemplate template = (MainTemplate)trvTemplate.SelectedItem;
                if (template.NodeLevel == 3)
                {
                    mainViewModel.Ultrasound_Finding += template.GetUltrasoundFinding() + "\r\n";
                    mainViewModel.Ultrasound_Prompt += template.GetUltrasoundPrompt() + "\r\n";
                }
            }
        }
        #region Char Event
        private void btnChar1_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "～");
            tbxFinding.SelectionStart = start + 1;
        }
        private void btnChar2_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "%");
            tbxFinding.SelectionStart = start + 1;
        }
        private void btnChar3_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "×");
            tbxFinding.SelectionStart = start + 1;
        }
        private void btnChar4_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "cm");
            tbxFinding.SelectionStart = start + 2;
        }
        private void btnChar5_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "mm");
            tbxFinding.SelectionStart = start + 2;
        }
        private void btnChar6_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "cm/s");
            tbxFinding.SelectionStart = start + 4;
        }
        private void btnChar7_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "RI");
            tbxFinding.SelectionStart = start + 2;
        }
        private void btnChar8_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "PI");
            tbxFinding.SelectionStart = start + 2;
        }
        private void btnChar9_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "Vmax");
            tbxFinding.SelectionStart = start + 4;
        }
        private void btnChar10_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "Vmin");
            tbxFinding.SelectionStart = start + 4;
        }
        private void btnChar11_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "℃");
            tbxFinding.SelectionStart = start + 1;
        }
        private void btnChar12_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "α");
            tbxFinding.SelectionStart = start + 1;
        }
        private void btnChar13_Click(object sender, RoutedEventArgs e)
        {
            int start = tbxFinding.SelectionStart;
            tbxFinding.Text = tbxFinding.Text.Insert(start, "β");
            tbxFinding.SelectionStart = start + 1;
        }
        #endregion

        #region Event Hook
        private IntPtr ControlMsgFilter(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;
            switch (msg)
            {
                case 0x200:
                    if (isFirstEnterDS)
                    {
                        System.Threading.Thread td = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(delegate(object obj)
                        {
                            System.Threading.Thread.Sleep(300);
                            ControlHostElement.Dispatcher.Invoke(new Action<object>(delegate(object obj2)
                            {
                                ((ToolTip)obj2).IsOpen = true;
                            }), new object[] { obj });
                            System.Threading.Thread.Sleep(2000);
                            ControlHostElement.Dispatcher.Invoke(new Action<object>(delegate(object obj2)
                            {
                                ((ToolTip)obj2).IsOpen = false;
                            }), new object[] { obj });
                        }));
                        td.IsBackground = true;
                        td.Start(dsTip);
                    }
                    isFirstEnterDS = false;
                    break;
                case 0x201:
                    btnCaptureImage_Click(this, null);
                    break;
            }
            return IntPtr.Zero;
        }
        #endregion

        #region Video Event
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (videoController.Status != VideoStatus.Pause)
            {
                videoController.Pause();
            }
            else
            {
                videoController.StartPlayVideo();
            }
        }
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "图像文件(*.jpg,*.bmp,*.png,*.wmv,*.avi)|*.jpg;*.bmp;*.png;*.wmv;*.avi";
                dlg.Multiselect = false;
                bool? result = dlg.ShowDialog();
                if (result == true)
                {

                    FileInfo file = new FileInfo(dlg.FileName);
                    string type = String.Empty;
                    if (file.Extension.Equals(".jpg")
                        || file.Extension.Equals(".bmp")
                         || file.Extension.Equals(".png"))
                    {
                        type = "Image";
                    }
                    else
                    {
                        type = "Video";
                    }
                    string baseDir = Config.PatientImageBaseDirectory + @"\" 
                        + ((DateTime)mainViewModel.Examine_Time).ToString("yyyyMMdd") + @"\"
                        + mainViewModel.Patient_Number + @"\";
                    PatientImage img = mainViewModel.AddImage(baseDir,
                        file.Extension.Substring(1, file.Extension.Length - 1), type);
                    file.CopyTo(img.Image_Path);
                    OnImageAdded(img);
                }
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }

        }
        private void btnStopVideo_Click(object sender, RoutedEventArgs e)
        {
            videoController.ChooseDevice(Config.DeviceIndex);
        }
        private void btnPauseVideo_Click(object sender, RoutedEventArgs e)
        {
            if (videoController.Status != VideoStatus.Pause)
                videoController.Pause();
            else
                videoController.StartPlayVideo();
        }
        private void tmrVideoRecord_Elapsed(object source, ElapsedEventArgs e)
        {
            try
            {
                double overtime = (double)pgbRecordTime.Dispatcher.Invoke(new Func<double>
                    (
                        () =>
                        {
                            double duration = ++pgbRecordTime.Value;
                            txbRecordTime.Text = DateTime.Parse("00:00:00").AddSeconds(duration).ToString("HH:mm:ss");
                            return duration;
                        }
                    )
                );

                if (overtime >= Config.DurationVideoRecord)
                {
                    StopRecordVideo();
                }
            }
            catch (Exception e1)
            {

                ErrorHandle.HandleException(e1, false);
            }
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Int64 now = 0, stop = 0;
            videoController.GetVideoPostion(ref now, ref stop);
            sldPlay.Dispatcher.Invoke(new Action(delegate
            {
                sldPlay.Value = now;
                if (now >= stop)
                {
                    aTimer.Stop();
                }
            }), new object[] { });
        }
        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            popSetting.IsOpen = popSetting.IsOpen ? false : true;
        }
        private void mniCard_Click(object sender, RoutedEventArgs e)
        {
            popSetting.IsOpen = false;
            CardSettingWindow win = new CardSettingWindow();
            win.Owner = this.Owner;
            win.ShowInTaskbar = false;
            win.ShowDialog();
        }
        private void mniSystem_Click(object sender, RoutedEventArgs e)
        {
            popSetting.IsOpen = false;
            SystemSetting window = new SystemSetting();
            window.Owner = this.Owner;
            window.ShowInTaskbar = false;
            window.ShowDialog();
        }
        private void mniOption_Click(object sender, RoutedEventArgs e)
        {
            popSetting.IsOpen = false;
            OptionSettingWindow window = new OptionSettingWindow();
            window.Owner = this.Owner;
            window.ShowInTaskbar = false;
            window.ShowDialog();
        }
        private void mniServer_Click(object sender, RoutedEventArgs e)
        {
            popSetting.IsOpen = false;
            ServerSettingWindow window = new ServerSettingWindow();
            window.Owner = this.Owner;
            window.ShowInTaskbar = false;
            window.ShowDialog();
        }
        private void btnPlayVideo_Click(object sender, RoutedEventArgs e)
        {
            OnPlayVideo();
        }
        private void sldPlay_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            aTimer.Stop();
            isStartDrag = true;
        }
        private void sldPlay_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isStartDrag = false;
            aTimer.Start();
        }
        private void sldPlay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isStartDrag)
            {
                Int64 now = 0, stop = 0;
                stop = (Int64)sldPlay.Maximum;
                now = (Int64)sldPlay.Value;
                videoController.SetVideoPostion(ref now, ref stop);
            }
        }
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            isFirstEnterDS = true;
        }
        private void btnCaptureImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (videoController != null && videoController.Status != VideoStatus.RecordVideo)
                {
                    if (IsMultiThreadMode)
                    {
                        if (!IsSecondPatient)
                        {
                            Patient2 = mainViewModel.AddNewPatient();
                            PatientImage img = mainViewModel.CreateNewImage(Patient2.Patient_Number,(DateTime)Patient2.Examine_Time);
                            if (!String.IsNullOrEmpty(img.Image_Path))
                            {
                                videoController.CaptureImage(img.Image_Path, Config.CompressPic);
                                OnImageAdded(img);
                            }
                            IsSecondPatient = true;
                        }
                        else
                        {
                            PatientImage img = mainViewModel.CreateNewImage(Patient2.Patient_Number, (DateTime)Patient2.Examine_Time);
                            if (!String.IsNullOrEmpty(img.Image_Path))
                            {
                                videoController.CaptureImage(img.Image_Path, Config.CompressPic);
                                OnImageAdded(img);
                            }
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(mainViewModel.Patient_Number))
                        {
                            PatientImage img = mainViewModel.CreateNewImage();
                            if (!String.IsNullOrEmpty(img.Image_Path))
                            {
                                videoController.CaptureImage(img.Image_Path, Config.CompressPic);
                                OnImageAdded(img);
                            }
                        }
                        else
                        {
                            MessageBox.Show("请先录入病人信息！", "提醒", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception e1)
            {

                ErrorHandle.HandleException(e1, false);
            }

        }
        private void btnStartRecord_Click(object sender, RoutedEventArgs e)
        {
            if (IsMultiThreadMode)
            {
                if (!IsSecondPatient)
                {
                    Patient2 = mainViewModel.AddNewPatient();
                    PatientImage img = null;
                    if (!String.IsNullOrEmpty(mainViewModel.Patient_Number))
                    {
                        btnStartRecord.IsEnabled = false;
                        btnStopRecord.IsEnabled = true;
                        btnCaptureImage.IsEnabled = false;
                        if (Config.FileFormat == 0)
                        {
                            img = mainViewModel.CreateNewVideo(Patient2.Patient_Number,(DateTime)Patient2.Examine_Time, "avi");
                            videoController.RecordViedo(img.Image_Path);
                        }
                        else if (Config.FileFormat == 1)
                        {
                            string profile = Properties.Resources.ResourceManager.GetString("DefautProfile");
                            img = mainViewModel.CreateNewVideo(Patient2.Patient_Number,(DateTime)Patient2.Examine_Time, "wmv");
                            videoController.RecordViedo(img.Image_Path, profile, Config.CodeRate * 1024);
                        }
                        OnImageAdded(img);
                        tmrVideoRecord = new Timer();
                        tmrVideoRecord.Interval = 1000;
                        tmrVideoRecord.Elapsed += new ElapsedEventHandler(tmrVideoRecord_Elapsed);
                        tmrVideoRecord.Start();
                        IsSecondPatient = true;
                    }
                }
                else
                {
                    PatientImage img = null;
                    if (!String.IsNullOrEmpty(mainViewModel.Patient_Number))
                    {
                        btnStartRecord.IsEnabled = false;
                        btnStopRecord.IsEnabled = true;
                        btnCaptureImage.IsEnabled = false;
                        if (Config.FileFormat == 0)
                        {
                            img = mainViewModel.CreateNewVideo("avi");
                            videoController.RecordViedo(img.Image_Path);
                        }
                        else if (Config.FileFormat == 1)
                        {
                            string profile = Properties.Resources.ResourceManager.GetString("DefautProfile");
                            img = mainViewModel.CreateNewVideo("wmv");
                            videoController.RecordViedo(img.Image_Path, profile, Config.CodeRate * 1024);
                        }
                        OnImageAdded(img);
                        tmrVideoRecord = new Timer();
                        tmrVideoRecord.Interval = 1000;
                        tmrVideoRecord.Elapsed += new ElapsedEventHandler(tmrVideoRecord_Elapsed);
                        tmrVideoRecord.Start();
                        IsSecondPatient = true;
                    }
                }
            }
            else
            {
                PatientImage img = null;
                if (!String.IsNullOrEmpty(mainViewModel.Patient_Number))
                {
                    btnStartRecord.IsEnabled = false;
                    btnStopRecord.IsEnabled = true;
                    btnCaptureImage.IsEnabled = false;
                    if (Config.FileFormat == 0)
                    {
                        img = mainViewModel.CreateNewVideo("avi");
                        videoController.RecordViedo(img.Image_Path);
                    }
                    else if (Config.FileFormat == 1)
                    {
                        string profile = Properties.Resources.ResourceManager.GetString("DefautProfile");
                        img = mainViewModel.CreateNewVideo("wmv");
                        videoController.RecordViedo(img.Image_Path, profile, Config.CodeRate * 1024);
                    }
                    OnImageAdded(img);
                    tmrVideoRecord = new Timer();
                    tmrVideoRecord.Interval = 1000;
                    tmrVideoRecord.Elapsed += new ElapsedEventHandler(tmrVideoRecord_Elapsed);
                    tmrVideoRecord.Start();
                }
                else
                {
                    MessageBox.Show("请先录入病人信息！", "提醒", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void btnStopRecord_Click(object sender, RoutedEventArgs e)
        {
            StopRecordVideo();
        }
        private void tbxDuration_LostFocus(object sender, RoutedEventArgs e)
        {
            int value = 0;
            if (int.TryParse(tbxDuration.Text, out value))
                Config.DurationVideoRecord = value;
        }
        #endregion
        #endregion

        #region Method
        public void StopRecordVideo()
        {
            try
            {
                if (tmrVideoRecord != null)
                {
                    tmrVideoRecord.Stop();
                    tmrVideoRecord.Dispose();
                    tmrVideoRecord = null;
                }
                videoController.StopRecordVideo();
                pgbRecordTime.Dispatcher.Invoke(new Action
                    (
                        () =>
                        {
                            txbRecordTime.Text = "";
                            pgbRecordTime.Value = 0;
                            btnStartRecord.IsEnabled = true;
                            btnStopRecord.IsEnabled = false;
                            btnCaptureImage.IsEnabled = true;
                        }
                    )
                );
            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);
            }
        }

        void OnPlayVideo()
        {
            PlayVideoEventHandler handler = System.Threading.Interlocked.CompareExchange(ref PlayVideoEvent, null, null);
            if (handler != null)
            {
                handler(this, null);
            }
        }
        void OnImageAdded(PatientImage img)
        {
            ImageAddedEventHandler handler = System.Threading.Interlocked.CompareExchange(ref ImageAddedEvent, null, null);
            if (handler != null)
            {
                handler(this, img);
            }
        }

        void OnShowImage(PatientImage img)
        {
            ShowImageEventHandler handler = System.Threading.Interlocked.CompareExchange(ref ShowImageEvent, null, null);
            if (handler != null)
            {
                handler(this, img);
            }
        }

        void InitViewModel()
        {
            mainViewModel = new MainViewModel(StartUpWindow.UCMain, null);
            grdMain.DataContext = mainViewModel;


            Department dept = new Department();
            cbxDepartment.ItemsSource = new ObservableCollection<Department>(dept.SelectAll());
            cbxDepartment.DisplayMemberPath = "Name";
            cbxDepartment.SelectedValuePath = "Name";

            Diagnosis diagnosis = new Diagnosis();
            cbxDiagnosis.ItemsSource = new ObservableCollection<Diagnosis>(diagnosis.SelectAll());
            cbxDiagnosis.DisplayMemberPath = "Diagnosis_Name";
            cbxDiagnosis.SelectedValuePath = "Diagnosis_Name";
        }
        void Init()
        {
            //win32 video control init
            videoControl = new VideoControlHost(ControlHostElement.ActualHeight,
               ControlHostElement.ActualWidth);
            ControlHostElement.Child = videoControl;
            videoControl.MessageHook += new HwndSourceHook(ControlMsgFilter);
            HwndVidoeControl = videoControl.HwndHost;
            videoController = VideoControllerFactory.CreateInstance();

            //win32 video control tooltips
            aTimer = new Timer();
            aTimer.Interval = 100;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            isFirstEnterDS = true;
            dsTip = new ToolTip() { Content = "点击左键，进行鼠标模糊抓取。" };


            //设置是否需要解码器
            videoController.SetDecompressor(Config.EnabledDecomp ? 1 : 0);
            //choose device
            if (Config.DeviceIndex <= videoController.Devices.Count)
            {
                InitCaptureCard(Config.DeviceIndex);
                videoController.ChooseDevice(Config.DeviceIndex);
            }
            else
            {
                InitCaptureCard(0);
                videoController.ChooseDevice(0);
            }

            //duration video record
            tbxDuration.Text = Config.DurationVideoRecord.ToString();

            //Positive or Negative == true 
            if (mainViewModel.IsPositive == true)
                cbxPostive.IsChecked = true;
            else if (mainViewModel.IsPositive == false)
                cbxNegative.IsChecked = false;
            else if (mainViewModel.IsPositive == null)
            {
                cbxPostive.IsChecked = null;
                cbxNegative.IsChecked = null;
            }

            //load template
            MainTemplate template = new MainTemplate();
            template.LoadExpressTemplate();
            trvTemplate.ItemsSource = MainTemplate.ExpressTemplate;

        }
        public void Save()
        {
            mainViewModel.Update();
        }
        public void RefreshPatientInformation(string patientNumber)
        {
            mainViewModel.SelectByKey(patientNumber);
            if (mainViewModel.IsPositive == true)
            {
                cbxPostive.IsChecked = true;
                cbxNegative.IsChecked = false;
            }
            else if (mainViewModel.IsPositive == false)
            {
                cbxPostive.IsChecked = false;
                cbxNegative.IsChecked = true;
            }
            else if (mainViewModel.IsPositive == null)
            {
                cbxPostive.IsChecked = null;
                cbxNegative.IsChecked = null;
            }
            mainViewModel.RefreshUI();
        }
        public void ShowPatientImages(string patientNumber)
        {
            foreach (PatientImage img in mainViewModel.GetPatientImages(patientNumber))
            {
                OnShowImage(img);
            }
        }
        public void StartPlayVideo(double duration)
        {
            sldPlay.Maximum = duration;
            aTimer.Start();
        }
        public void CaptureImage()
        {
            btnCaptureImage_Click(this, null);
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            videoControl.Dispose();
        }
        #endregion

        private void grdTest_LostFocus(object sender, RoutedEventArgs e)
        {
            popSetting.IsOpen = false;
        }

        private DependencyObject currentNodeSource;

        private void TextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            var treeViewItem = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem != null)
            {
                currentNodeSource = e.OriginalSource as DependencyObject;
                treeViewItem.IsSelected = true;
                e.Handled = true;
            }
        }

        static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);
            return source;
        }

        private void mniName_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DependencyObject parent = VisualTreeHelper.GetParent(currentNodeSource);
                TextBlock tbk = VisualTreeHelper.GetChild(parent, 1) as TextBlock;
                TextBox tbx = VisualTreeHelper.GetChild(parent, 2) as TextBox;
                tbx.Text = tbk.Text;
                tbk.Text = "";
                tbk.Visibility = Visibility.Hidden;
                tbx.Visibility = Visibility.Visible;
                tbx.Focus();
                tbx.SelectionStart = tbx.Text.Length;
            }
            catch (Exception e1)
            {
                ErrorHandle.HandlerWarning(e1.Message);
            }

        }

        private void mniMoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (trvTemplate.SelectedItem != null)
            {
                try
                {
                    MainTemplate template = (MainTemplate)trvTemplate.SelectedItem;
                    if (template.OrderID != default(int?) && template.NodeLevel != default(int?) && template.Parent_Id != default(int?))
                    {
                        template.MoveNodeUp();
                        ObservableCollection<MainTemplate> parent = FindNodeParent(template, MainTemplate.ExpressTemplate);
                        if (parent != null && parent.IndexOf(template) > 0)
                            parent.Move(parent.IndexOf(template), parent.IndexOf(template) - 1);
                    }
                }
                catch (Exception e1)
                {

                    ErrorHandle.HandlerWarning(e1.Message);
                }

            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    DependencyObject parent = VisualTreeHelper.GetParent(e.OriginalSource as DependencyObject);
                    TextBlock tbk = VisualTreeHelper.GetChild(parent, 1) as TextBlock;
                    TextBox tbx = sender as TextBox;

                    tbk.Text = tbx.Text;
                    tbx.Text = "";
                    tbx.Visibility = Visibility.Hidden;
                    tbk.Visibility = Visibility.Visible;
                    MainTemplate template = (MainTemplate)trvTemplate.SelectedItem;
                    template.Name = tbk.Text;
                    template.UpdateName();
                }
            }
            catch (Exception e1)
            {
                ErrorHandle.HandlerWarning(e1.Message);
            }

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {

                DependencyObject parent = VisualTreeHelper.GetParent(e.OriginalSource as DependencyObject);
                TextBlock tbk = VisualTreeHelper.GetChild(parent, 1) as TextBlock;
                TextBox tbx = sender as TextBox;
                if (!string.IsNullOrEmpty(tbx.Text))
                {

                    tbk.Text = tbx.Text;
                    tbx.Text = "";
                    tbx.Visibility = Visibility.Hidden;
                    tbk.Visibility = Visibility.Visible;
                    ObservableCollection<MainTemplate> col = (ObservableCollection<MainTemplate>)trvTemplate.ItemsSource;
                    MainTemplate template = (MainTemplate)trvTemplate.SelectedItem;
                    template.Name = tbk.Text;
                    template.UpdateName();
                }
            }
            catch (Exception e1)
            {
                ErrorHandle.HandlerWarning(e1.Message);
            }
        }

        private void mniMoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (trvTemplate.SelectedItem != null)
            {
                try
                {
                    MainTemplate template = (MainTemplate)trvTemplate.SelectedItem;
                    if (template.OrderID != default(int?) && template.NodeLevel != default(int?) && template.Parent_Id != default(int?))
                    {
                        template.MoveNodeDown();
                        ObservableCollection<MainTemplate> parent = FindNodeParent(template, MainTemplate.ExpressTemplate);
                        if (parent != null && parent.IndexOf(template) < parent.Count - 1)
                            parent.Move(parent.IndexOf(template), parent.IndexOf(template) + 1);
                    }
                }
                catch (Exception e1)
                {

                    ErrorHandle.HandlerWarning(e1.Message);
                }
            }
        }

        private ObservableCollection<MainTemplate> FindNodeParent(MainTemplate template, ObservableCollection<MainTemplate> Parent)
        {
            ObservableCollection<MainTemplate> result = null;
            foreach (MainTemplate obj in Parent)
            {
                if (template == obj)
                {
                    result = Parent;
                    break;
                }
                else if (obj.Children != null)
                {
                    result = FindNodeParent(template, obj.Children);
                    if (result != null)
                        break;
                }
            }
            return result;
        }
        void InitCaptureCard(int index)
        {
            try
            {
                VideoController videoController = VideoControllerFactory.CreateInstance();
                videoController.ChooseDevice(index, 1);
                if (Config.DisplayBrightnessValue != 9999999)
                {
                    videoController.SetVMR9ControlInfo(Config.DisplayBrightnessValue, 1);
                }
                if (Config.DisplayContrastValue != 9999999)
                {
                    videoController.SetVMR9ControlInfo(Config.DisplayContrastValue, 2);
                }
                if (Config.DisplayHueValue != 9999999)
                {
                    videoController.SetVMR9ControlInfo(Config.DisplayHueValue, 3);
                }
                if (Config.DisplaySaturationValue != 9999999)
                {
                    videoController.SetVMR9ControlInfo(Config.DisplaySaturationValue, 4);
                }

            }
            catch (Exception e1)
            {

                ErrorHandle.HandleException(e1);
            }
        }

    }


}
