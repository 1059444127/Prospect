using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;

namespace RisCaptureLib
{
    internal class BitmapCursor : SafeHandle,IDisposable
    {

        public override bool IsInvalid
        {
            get
            {
                return handle == (IntPtr)(-1);
            }
        }


        public Cursor CreateBmpCursor()
        {

            return CursorInteropHelper.Create(this);
        }



        public BitmapCursor()
            : base((IntPtr)(-1), true)
        {
            using (Bitmap bmp = CreateCrossImage())
            {
                handle = bmp.GetHicon();
            }
        }

        public new void Dispose()
        {
            ReleaseHandle();
        }


        protected override bool ReleaseHandle()
        {
            bool result = DestroyIcon(handle);

            handle = (IntPtr)(-1);

            return result;
        }

        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr hIcon);

        private Bitmap CreateCrossImage()
        {
            const int w = 25;
            const int h = 25;

            Bitmap bmp = new Bitmap(w, h);


            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.Default;
                g.InterpolationMode = InterpolationMode.High;

                using (Pen pen = new Pen(Brushes.Black, 2))
                {
                    g.DrawLine(pen, new Point(12, 0), new Point(12, 8)); // vertical line 
                    g.DrawLine(pen, new Point(12, 17), new Point(12, 25)); // vertical line
                    g.DrawLine(pen, new Point(0, 12), new Point(8, 12)); // horizontal line 
                    g.DrawLine(pen, new Point(16, 12), new Point(24, 12)); // horizontal line
                    g.DrawLine(pen, new Point(12, 12), new Point(12, 13)); // Middle dot 
                }

                g.Flush();
            }

            return bmp;
        }
    }
}
