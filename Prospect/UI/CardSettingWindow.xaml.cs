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
using System.Windows.Interop;
using System.Configuration;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class CardSettingWindow : Window
    {
        #region Members
        VideoController videoController;
        VMR9ViewModel _VMR9ViewModel;
        #endregion

        public CardSettingWindow()
        {
            Init();
            InitializeComponent();
            InitViewModel();
        }


        #region Event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitConfiguration();
        }

        private void cbxDevice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Device dev = (Device)cbxDevice.SelectedItem;
            videoController.ChooseDevice(dev.Index);
            Config.DeviceIndex = dev.Index;
            Config.IsClip = false;
            cbxClip.IsChecked = false;
        }

        private void Image_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender == slider)
            {
                videoController.SetVMR9ControlInfo(_VMR9ViewModel.Brightness, 1);
                Config.DisplayBrightnessValue = _VMR9ViewModel.Brightness;
            }
            else if (sender == slider2)
            {
                videoController.SetVMR9ControlInfo(_VMR9ViewModel.Contrast, 2);
                Config.DisplayContrastValue = _VMR9ViewModel.Contrast;
            }
            else if (sender == slider3)
            {
                videoController.SetVMR9ControlInfo(_VMR9ViewModel.Hue, 3);
                Config.DisplayHueValue = _VMR9ViewModel.Hue;
            }
            else if (sender == slider4)
            {
                videoController.SetVMR9ControlInfo(_VMR9ViewModel.Saturation, 4);
                Config.DisplaySaturationValue = _VMR9ViewModel.Saturation;
            }
            videoController.GetVMR9ControlInfo(_VMR9ViewModel.VMR9Control);
        }

        private void btnSource_Click(object sender, RoutedEventArgs e)
        {
            videoController.GetVideoSource();
        }

        private void rbnAVI_Checked(object sender, RoutedEventArgs e)
        {
            Config.FileFormat = 0;
        }

        private void rbnWMV_Checked(object sender, RoutedEventArgs e)
        {
            Config.FileFormat = 1;
        }

        private void tbxCodeRate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Int32.TryParse(tbxCodeRate.Text, out Config.CodeRate))
                ErrorHandle.HandlerWarning("字符串“" + tbxCodeRate.Text + "”转换数字失败。", "CardSettingWindow.tbxCodeRate_LostFocus");
        }

        private void btnPreviewConfig_Click(object sender, RoutedEventArgs e)
        {
            videoController.ShowPreviewPinPropertyFrame();
        }
        private void btnOtherConfig_Click(object sender, RoutedEventArgs e)
        {
            videoController.ShowCapPropertyFrame();
        }
        private void btnCaptureConfig_Click(object sender, RoutedEventArgs e)
        {
            videoController.ShowCapturePinPropertyFrame();
        }

        private void cbxSignalSys_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (cbxSignalSys.SelectedIndex)
            {
                case 0:
                    videoController.SetSignalSys(0);
                    Config.SignalSys = "PAL";
                    break;
                case 1:
                    videoController.SetSignalSys(1);
                    Config.SignalSys = "NTSC";
                    break;
            }
        }

        private void cbxClip_Checked(object sender, RoutedEventArgs e)
        {
            Config.IsClip = true;
            videoController.ClipVideo(Config.ClipLeft, Config.ClipTop, Config.ClipRight, Config.ClipBottom, false);
        }
        private void cbxClip_Unchecked(object sender, RoutedEventArgs e)
        {
            videoController.ClipVideo(Config.ClipLeft, Config.ClipTop, Config.ClipRight, Config.ClipBottom, true);
            Config.IsClip = false;
        }
        private void tbxClip_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = (TextBox)sender;
            int value = 0;
            switch (tbx.Name)
            {
                case "tbxClipTop":
                    if (Int32.TryParse(tbx.Text, out value))
                        Config.ClipTop = value;
                    break;
                case "tbxClipLeft":
                    if (Int32.TryParse(tbx.Text, out value))
                        Config.ClipLeft = value;
                    break;
                case "tbxClipRight":
                    if (Int32.TryParse(tbx.Text, out value))
                        Config.ClipRight = value;
                    break;
                case "tbxClipBottom":
                    if (Int32.TryParse(tbx.Text, out value))
                        Config.ClipBottom = value;
                    break;
            }
        }

        private void cbxEnableDecomp_Checked(object sender, RoutedEventArgs e)
        {
            videoController.SetDecompressor(1);
            videoController.ChooseDevice(Config.DeviceIndex);
            Config.EnabledDecomp = true;
        }
        private void cbxEnableDecomp_Unchecked(object sender, RoutedEventArgs e)
        {
            videoController.SetDecompressor(0);
            videoController.ChooseDevice(Config.DeviceIndex);
            Config.EnabledDecomp = false;
        }

        private void cbxCompressPic_Checked(object sender, RoutedEventArgs e)
        {
            Config.CompressPic = true;
        }

        private void cbxCompressPic_Unchecked(object sender, RoutedEventArgs e)
        {
            Config.CompressPic = false;
        }
        #endregion

        #region Methods
        void Init()
        {
            videoController = VideoControllerFactory.CreateInstance();
        }
        void InitViewModel()
        {
            _VMR9ViewModel = new VMR9ViewModel();
            grdSignal.DataContext = _VMR9ViewModel;
        }
        void InitConfiguration()
        {
            //设备
            foreach (Device dev in videoController.Devices)
            {
                cbxDevice.Items.Add(dev);
            }

            //视频参数
            //videoController.GetVMR9ControlRange(_VMR9ViewModel.VMR9Control);
            //videoController.GetVMR9ControlInfo(_VMR9ViewModel.VMR9Control);

            //保存文件格式
            if (Config.FileFormat == 0)
            {
                rbnAVI.IsChecked = true;
            }
            else if (Config.FileFormat == 1)
            {
                rbnWMV.IsChecked = true;
            }

            //码率
            tbxCodeRate.Text = Config.CodeRate.ToString();

            //视频制式
            switch (Config.SignalSys)
            {
                case "PAL":
                    cbxSignalSys.SelectedIndex = 0;
                    break;
                case "NTSC":
                    cbxSignalSys.SelectedIndex = 1;
                    break;
            }

            //选择的设备
            cbxDevice.SelectedIndex = Config.DeviceIndex;

            //剪裁画面
            cbxClip.IsChecked = Config.IsClip;
            tbxClipBottom.Text = Config.ClipBottom.ToString();
            tbxClipLeft.Text = Config.ClipLeft.ToString();
            tbxClipTop.Text = Config.ClipTop.ToString();
            tbxClipRight.Text = Config.ClipRight.ToString();
            cbxEnableDecomp.IsChecked = Config.EnabledDecomp;
            cbxCompressPic.IsChecked = Config.CompressPic;
        }
        #endregion

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = tabControl1.SelectedItem as TabItem;
                if (item.Header.Equals("信号"))
                {
                    Device dev = (Device)cbxDevice.SelectedItem;
                    videoController.ChooseDevice(dev.Index, 1);
                    videoController.GetVMR9ControlRange(_VMR9ViewModel.VMR9Control);
                    videoController.GetVMR9ControlInfo(_VMR9ViewModel.VMR9Control);
                    _VMR9ViewModel.RaisePropertyChanged("Contrast");
                    _VMR9ViewModel.RaisePropertyChanged("DisplayContrastValue");
                    _VMR9ViewModel.RaisePropertyChanged("ContrastMaxValue");
                    _VMR9ViewModel.RaisePropertyChanged("ContrastMinValue");
                    _VMR9ViewModel.RaisePropertyChanged("Brightness");
                    _VMR9ViewModel.RaisePropertyChanged("DisplayBrightnessValue");
                    _VMR9ViewModel.RaisePropertyChanged("BrightnessMaxValue");
                    _VMR9ViewModel.RaisePropertyChanged("BrightnessMinValue");
                    _VMR9ViewModel.RaisePropertyChanged("Hue");
                    _VMR9ViewModel.RaisePropertyChanged("DisplayHueValue");
                    _VMR9ViewModel.RaisePropertyChanged("HueMaxValue");
                    _VMR9ViewModel.RaisePropertyChanged("HueMinValue");
                    _VMR9ViewModel.RaisePropertyChanged("Saturation");
                    _VMR9ViewModel.RaisePropertyChanged("DisplaySaturationValue");
                    _VMR9ViewModel.RaisePropertyChanged("SaturationMaxValue");
                    _VMR9ViewModel.RaisePropertyChanged("SaturationMinValue");

                }
            }
            catch (Exception e1)
            {

                ErrorHandle.HandleException(e1);
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Device dev = (Device)cbxDevice.SelectedItem;
                videoController.ChooseDevice(dev.Index, 0);
            }
            catch (Exception e1)
            {

                ErrorHandle.HandleException(e1);
            }

        }




















    }
}
