using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using com.cloud.video;
using System.Configuration;
using System.Management;


namespace com.cloud.prospect
{
    public class VideoControlHost : HwndHost
    {
        bool IsFinishInit = false; 

        private VideoController videoController;
        public int HostHeight
        { get; private set; }
        public int HostWidth
        { get; private set; }
        public IntPtr HwndHost
        { get; private set; }

        public VideoControlHost(double height, double width)
        {
            HostHeight = (int)height;
            HostWidth = (int)width;
        }
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            StringBuilder sb = new StringBuilder(UtilFunctions.GetHardwareNumber());
            StringBuilder sb2 = new StringBuilder(ConfigurationManager.AppSettings["RegisterCode"]);
            videoController = VideoControllerFactory.CreateInstance();
            HwndHost = videoController.Init(hwndParent.Handle, HostWidth, HostHeight,sb,sb2);

            Information.IsWriteRead = true;
            Information.WriteDate();
            Information.Date = Information.GetDate();
            Information.ID = Information.GetID();
            Information.IsWriteRead = false;
            IsFinishInit = true;
            return new HandleRef(this, HwndHost);
        }
        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;
            switch (msg)
            {
                case 0x0210:
                    break;
            }
            return IntPtr.Zero;
        }
        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            while (!IsFinishInit) System.Threading.Thread.Sleep(200);
            videoController.Clean();
        }
    }
}
