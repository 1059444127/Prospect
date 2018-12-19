using System;
using System.Windows;
using System.Windows.Media.Imaging;


namespace RisCaptureLib
{
    public class ScreenCaputre
    {
        MaskWindow mask;

        System.Drawing.Rectangle rect;

        public void CancelCapture()
        {
            if (mask != null)
            {
                mask.Dispose();
                mask = null;
            }
        }

        public void SetCaputreRect(int x, int y, int width, int height)
        {
            rect = new System.Drawing.Rectangle(x, y, width, height);
        }


        public void StartCaputre(int timeOutSeconds, Window owners)
        {
            StartCaputre(timeOutSeconds, null, owners);
        }

        public void StartCaputre(int timeOutSeconds, Size? defaultSize, Window owner)
        {
            mask = new MaskWindow(this, rect, owner);
            mask.Show(timeOutSeconds, defaultSize);
        }

        public event EventHandler<ScreenCaputredEventArgs> ScreenCaputred;
        public event EventHandler<EventArgs> ScreenCaputreCancelled;

        internal void OnScreenCaputred(object sender, System.Drawing.Bitmap caputredBmp)
        {
            if (ScreenCaputred != null)
            {
                ScreenCaputred(sender, new ScreenCaputredEventArgs(caputredBmp));
            }
        }

        internal void OnScreenCaputreCancelled(object sender)
        {
            if (ScreenCaputreCancelled != null)
            {
                ScreenCaputreCancelled(sender, EventArgs.Empty);
            }
        }
    }


}
