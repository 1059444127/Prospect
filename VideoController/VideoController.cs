using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace com.cloud.video
{

    public enum VideoStatus
    {
        Stop = 0,
        Pause = 2,
        Preview = 1,
        RecordVideo = 3,
        PlayVideo = 4
    }

    public class VideoController : IVideoController
    {


        private static VideoController Instance;

        public List<Device> Devices
        { get; private set; }
        public IntPtr HwndVideoWin
        { get; private set; }
        public VideoStatus Status
        { get; private set; }
        private VideoController()
        {
            Devices = new List<Device>();
        }

        internal static VideoController GetInstance()
        {
            if (Instance == null)
            {
                Instance = new VideoController();
                Instance.GetDevicesNames();
            }
            return Instance;
        }
        public virtual int ChooseDevice(int index, int flag = 0)
        {
            PInvoke.ChooseDevice(index, flag);
            Status = VideoStatus.Preview;
            return 0;
        }
        public virtual IntPtr Init(IntPtr hwnd, int x, int y, StringBuilder sb1, StringBuilder sb2)
        {
            IntPtr hwndWindow = PInvoke.Init(hwnd, x, y, sb1, sb2);
            HwndVideoWin = hwndWindow;

            return HwndVideoWin;
        }
        public virtual int Clean()
        {
            PInvoke.Clean();
            return 0;
        }
        public virtual int ShowProperty()
        {
            if (!PInvoke.ShowProperty())
                return -1;
            return 0;
        }
        public virtual int GetVMR9ControlInfo(VMR9Control vmr9)
        {
            VMR9ProcAmpControl VMR9Control = new VMR9ProcAmpControl();

            PInvoke.GetVMRPRocAmpControl(ref VMR9Control);
            vmr9.Brightness = VMR9Control.Brightness;
            vmr9.Contrast = VMR9Control.Contrast;
            vmr9.Saturation = VMR9Control.Saturation;
            vmr9.Hue = VMR9Control.Hue;
            return 0;
        }
        public virtual int GetVMR9ControlRange(VMR9Control vmr9)
        {
            VMR9ProcAmpControlRange VMR9ControlRange = new VMR9ProcAmpControlRange();
            PInvoke.GetVMRBrightnessRange(ref VMR9ControlRange);
            vmr9.BrightnessMaxValue = VMR9ControlRange.MaxValue;
            vmr9.BrightnessMinValue = VMR9ControlRange.MinValue;
            PInvoke.GetVMRContrastRange(ref VMR9ControlRange);
            vmr9.ContrastMaxValue = VMR9ControlRange.MaxValue;
            vmr9.ContrastMinValue = VMR9ControlRange.MinValue;
            PInvoke.GetVMRSaturationRange(ref VMR9ControlRange);
            vmr9.SaturationMaxValue = VMR9ControlRange.MaxValue;
            vmr9.SaturationMinValue = VMR9ControlRange.MinValue;
            PInvoke.GetVMRHueRange(ref VMR9ControlRange);
            vmr9.HueMaxValue = VMR9ControlRange.MaxValue;
            vmr9.HueMinValue = VMR9ControlRange.MinValue;
            return 0;
        }
        public virtual int SetVMR9ControlInfo(Single value, int flag)
        {
            PInvoke.SetVMRPRocAmpControl(value, flag);
            return 0;
        }
        public virtual int PlayVideo(string path)
        {
            PInvoke.PlayVideo(path);
            Status = VideoStatus.PlayVideo;
            return 0;
        }
        public virtual int GetVideoPostion(ref Int64 now, ref Int64 stop)
        {
            PInvoke.GetVideoPostion(ref now, ref stop);
            return 0;
        }
        public virtual int GetDuration(ref Int64 Duration)
        {
            PInvoke.GetDuration(ref Duration);
            return 0;
        }
        public virtual int SetVideoPostion(ref Int64 now, ref Int64 stop)
        {
            PInvoke.SetVideoPostion(ref now, ref stop);
            return 0;
        }
        public virtual string CaptureImage(string file, bool compress)
        {
            file = file.Replace("jpg", "bmp");
            string result = file;
            try
            {

                PInvoke.CaptureImage(file.Replace("jpg", "bmp"));
                if (compress)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Exists)
                    {
                        result = fi.DirectoryName + "\\" + fi.Name.Replace("bmp", "jpg");
                        BMPToJPG(file, result);
                        fi.Delete();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        public virtual int RecordViedo(string file)
        {
            PInvoke.RecordVideo(file);
            Status = VideoStatus.RecordVideo;
            return 0;
        }
        public virtual int Stop()
        {
            PInvoke.Stop();
            return 0;
        }
        public virtual int RecordViedo(string file, string profile, long bitrate)
        {
            PInvoke.RecordVideo(file, profile, bitrate);
            Status = VideoStatus.RecordVideo;
            return 0;
        }
        public virtual int StopRecordVideo()
        {
            PInvoke.StopRecordVideo();
            Status = VideoStatus.Preview;
            return 0;
        }
        public virtual int GetVideoSource()
        {
            PInvoke.GetVideoSource();
            return 0;
        }
        public virtual int ShowPreviewPinPropertyFrame()
        {
            PInvoke.ShowPreviewPinPropertyFrame();
            return 0;
        }
        public virtual int ShowCapturePinPropertyFrame()
        {
            PInvoke.ShowCapturePinPropertyFrame();
            return 0;
        }
        public virtual int ShowCapPropertyFrame()
        {
            PInvoke.ShowCapPropertyFrame();
            return 0;
        }
        public virtual int SetSignalSys(int flag)
        {
            PInvoke.SetSignalSys(flag);
            return 0;
        }
        public virtual int ClipVideo(int left, int top, int right, int bottom, bool fullWindow)
        {
            PInvoke.ClipVideo(left, top, right, bottom, fullWindow);
            return 0;
        }
        public virtual int Pause()
        {
            PInvoke.Pause();
            Status = VideoStatus.Pause;
            return 0;
        }
        public virtual int StartPlayVideo()
        {
            PInvoke.StartPlayVideo();
            Status = VideoStatus.PlayVideo;
            return 0;
        }

        public virtual int SetDecompressor(int flag)
        {
            PInvoke.SetDecompressor(flag);
            return 0;
        }

        protected virtual int GetDevicesNames()
        {
            DeviceNames devNames = new DeviceNames();
            PInvoke.GetDevicesNames(ref devNames);
            AddDevice(devNames.devName1, 0);
            AddDevice(devNames.devName2, 1);
            AddDevice(devNames.devName3, 2);
            AddDevice(devNames.devName4, 3);
            AddDevice(devNames.devName5, 4);
            AddDevice(devNames.devName6, 5);
            AddDevice(devNames.devName7, 6);
            AddDevice(devNames.devName8, 7);
            AddDevice(devNames.devName9, 8);
            AddDevice(devNames.devName10, 9);
            AddDevice(devNames.devName11, 10);
            AddDevice(devNames.devName12, 11);
            AddDevice(devNames.devName13, 12);
            AddDevice(devNames.devName14, 13);
            AddDevice(devNames.devName15, 14);
            AddDevice(devNames.devName16, 15);
            AddDevice(devNames.devName17, 16);
            AddDevice(devNames.devName18, 17);
            AddDevice(devNames.devName19, 18);
            AddDevice(devNames.devName20, 19);
            return 0;
        }

        private void AddDevice(string name, int index)
        {
            if (!String.IsNullOrEmpty(name))
            {
                Device dev = new Device();
                dev.FriendlyName = name;
                dev.Index = index;
                Devices.Add(dev);
            }
        }
        private void BMPToJPG(string bmpFileName, string jpgFileName)
        {

            System.Drawing.Image img;
            img = ReturnPhoto(bmpFileName);

            img.Save(jpgFileName, ImageFormat.Jpeg);
        }

        private System.Drawing.Image ReturnPhoto(string bmpFileName)
        {
            System.IO.FileStream stream;
            stream = File.OpenRead(bmpFileName);
            Bitmap bmp = new Bitmap(stream);
            System.Drawing.Image image = bmp;//得到原图
            //创建指定大小的图
            System.Drawing.Image newImage = image.GetThumbnailImage(bmp.Width, bmp.Height, null, new IntPtr());
            Graphics g = Graphics.FromImage(newImage);
            g.DrawImage(newImage, 0, 0, newImage.Width, newImage.Height); //将原图画到指定的图上
            g.Dispose();
            stream.Close();
            return newImage;
        }
    }
}
