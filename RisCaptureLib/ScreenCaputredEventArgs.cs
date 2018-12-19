using System;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace RisCaptureLib
{
    public class ScreenCaputredEventArgs : EventArgs
    {
        public Bitmap Bmp
        {
            get;
            private set;
        }

        public ScreenCaputredEventArgs(Bitmap bmp)
        {
            Bmp = bmp;
        }
    }
}
