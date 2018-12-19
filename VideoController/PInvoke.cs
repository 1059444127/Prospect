using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace com.cloud.video
{
    #region C++ Struct
    [StructLayout(LayoutKind.Sequential)]
    public struct VMR9ProcAmpControlRange
    {
        public uint dwSize;
        public uint dwProperty;
        public Single MinValue;
        public Single MaxValue;
        public Single DefaultValue;
        public Single StepSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VMR9ProcAmpControl
    {
        public uint dwSize;
        public uint dwFlags;
        public Single Brightness;
        public Single Contrast;
        public Single Hue;
        public Single Saturation;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DeviceNames
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName5;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName6;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName7;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName8;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName9;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName10;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName11;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName12;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName13;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName14;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName15;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName16;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName17;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName18;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName19;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 500)]
        public string devName20;

    }

    #endregion

    public class PInvoke
    {
        public const string AppCommonFileDir = @"C:\Program Files\Common Files\Prospect";
        #region C++ Function
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hwnd);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam,
            IntPtr lParam);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "Init",
        CharSet = CharSet.Ansi,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Init(IntPtr hwnd, int x, int y, StringBuilder hardware, StringBuilder serialNumber);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "GetDevicesNames",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetDevicesNames(ref DeviceNames devNames);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "ReleaseIEnumMoniker",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ReleaseIEnumMoniker();

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "ChooseDevice",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ChooseDevice(int index, int flag);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "Clean",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool Clean();

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
       EntryPoint = "Stop",
       CharSet = CharSet.Auto,
       CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool Stop();

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "ShowProperty",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ShowProperty();

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
            EntryPoint = "GetVMRPRocAmpControl",
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetVMRPRocAmpControl(ref VMR9ProcAmpControl Control);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
            EntryPoint = "GetVMRBrightnessRange",
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetVMRBrightnessRange(ref VMR9ProcAmpControlRange Control);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
           EntryPoint = "GetVMRContrastRange",
           CharSet = CharSet.Auto,
           CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetVMRContrastRange(ref VMR9ProcAmpControlRange Control);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
           EntryPoint = "GetVMRHueRange",
           CharSet = CharSet.Auto,
           CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetVMRHueRange(ref VMR9ProcAmpControlRange Control);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
           EntryPoint = "GetVMRSaturationRange",
           CharSet = CharSet.Auto,
           CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetVMRSaturationRange(ref VMR9ProcAmpControlRange Control);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "SetVMRPRocAmpControl",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SetVMRPRocAmpControl(Single value, int flag);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "PlayVideo",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool PlayVideo(string path);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "SetVideoPostion",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SetVideoPostion(ref Int64 now, ref Int64 stop);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "GetVideoPostion",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetVideoPostion(ref Int64 now, ref Int64 stop);


        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "GetDuration",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetDuration(ref Int64 Duration);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "CaptureImage",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool CaptureImage(string file);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "RecordVideo",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool RecordVideo(string file);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "RecordWMVVideo",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool RecordVideo(string file, string profile, long bitrate);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "StopRecordVideo",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern void StopRecordVideo();

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "GetVideoSource",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern void GetVideoSource();

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "ShowPreviewPinPropertyFrame",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ShowPreviewPinPropertyFrame();

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "ShowCapturePinPropertyFrame",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ShowCapturePinPropertyFrame();

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "ShowCapPropertyFrame",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ShowCapPropertyFrame();

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "SetSignalSys",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetSignalSys(int flag);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "ClipVideo",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ClipVideo(int left, int top, int right, int bottom, bool fullWindow);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
        EntryPoint = "Pause",
        CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool Pause();

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
       EntryPoint = "StartPlayVideo",
       CharSet = CharSet.Auto,
       CallingConvention = CallingConvention.Cdecl)]
        internal static extern void StartPlayVideo();

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
       EntryPoint = "GenerateKey",
       CharSet = CharSet.Ansi,
       CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateKey(StringBuilder sb1, StringBuilder sb2);

        [DllImport(AppCommonFileDir + @"\VideoCapture.dll",
       EntryPoint = "SetDecompressor",
       CharSet = CharSet.Auto,
       CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetDecompressor(int flag);


        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);


        #endregion
    }
}
