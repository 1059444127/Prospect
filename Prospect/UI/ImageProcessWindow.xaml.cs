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
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using RisCaptureLib;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for ImageProcessWindow.xaml
    /// </summary>
    public partial class ImageProcessWindow : Window
    {
        ScreenCaputre screenCaputre;
        private System.Windows.Size? lastSize;
        #region Member
        ImageColorViewModel vmColor;
        PatientImage imgInfo;
        Bitmap orginalImage = null;
        Bitmap bmpTmp = null;
        int currRVal = 0;
        int currGVal = 0;
        int currBVal = 0;
        #endregion

        public ImageProcessWindow(PatientImage imgobj)
        {
            InitializeComponent();
            Init(imgobj);
            screenCaputre = new ScreenCaputre();
            screenCaputre.ScreenCaputred += OnScreenCaputred;
            screenCaputre.ScreenCaputreCancelled += OnScreenCaputreCancelled;

            vmColor = new ImageColorViewModel();
            this.DataContext = vmColor;

            lbxColor.ItemsSource = vmColor.GetColorTemplates();
        }

        #region Event
        private void btnNextImage_Click(object sender, RoutedEventArgs e)
        {
            ProspectWin owner = (ProspectWin)this.Owner;
            int index = owner.PatientImages.IndexOf(imgInfo);
            if (index >= 0 && index < owner.PatientImages.Count)
            {
                while (index < owner.PatientImages.Count - 1)
                {
                    PatientImage imgobj = owner.PatientImages[index + 1];
                    if ("Image".Equals(imgobj.Image_Type))
                    {
                        LoadImage(imgobj);
                        owner.stkImage.SelectedIndex = index + 1;
                        break;
                    }
                    index++;
                }
            }
        }

        private void btnPreviousImage_Click(object sender, RoutedEventArgs e)
        {
            ProspectWin owner = (ProspectWin)this.Owner;
            int index = owner.PatientImages.IndexOf(imgInfo);
            if (index > 0 && index <= owner.PatientImages.Count)
            {
                while (index > 0)
                {
                    PatientImage imgobj = owner.PatientImages[index - 1];
                    if ("Image".Equals(imgobj.Image_Type))
                    {
                        LoadImage(imgobj);
                        owner.stkImage.SelectedIndex = index - 1;
                        break;
                    }
                    index--;
                }
            }
        }

        private void btnColorSave_Click(object sender, RoutedEventArgs e)
        {
            ColorTemplateName window = new ColorTemplateName();
            window.Owner = this.Owner;
            window.ShowInTaskbar = false;
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            System.Windows.Point point = new System.Windows.Point();

            window.Left = btnColorSave.PointToScreen(point).X - 200;
            window.Top = btnColorSave.PointToScreen(point).Y + 30;
            if (window.ShowDialog() == true)
            {
                vmColor.Name = window.Name;
                vmColor.Save();
                vmColor.AddColorToTemplate();
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void imgMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point p = srvImage.PointToScreen(new System.Windows.Point(0, 0));
            screenCaputre.SetCaputreRect((int)p.X, (int)p.Y, (int)srvImage.ActualWidth,
                (int)srvImage.ActualHeight);
            screenCaputre.StartCaputre(30, lastSize, this);
        }
        private void btnColorDefaut_Click(object sender, RoutedEventArgs e)
        {
            IntPtr pImg = IntPtr.Zero;
            try
            {
                pImg = orginalImage.GetHbitmap();
                imgMain.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(pImg, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                if (bmpTmp != null)
                {
                    DeleteObject(bmpTmp.GetHbitmap());
                    bmpTmp.Dispose();
                    bmpTmp = null;
                }
                bmpTmp = (Bitmap)Bitmap.FromFile(imgInfo.Image_Path);
                currRVal = 0;
                currGVal = 0;
                currBVal = 0;
                sldBlue.Value = 0;
                sldRed.Value = 0;
                sldGreen.Value = 0;
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
            finally
            {
                if (pImg != IntPtr.Zero)
                {
                    DeleteObject(pImg);
                }
            }


        }
        private void btnShear_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Point p = srvImage.PointToScreen(new System.Windows.Point(0, 0));
            screenCaputre.SetCaputreRect((int)p.X, (int)p.Y, (int)srvImage.ActualWidth,
                (int)srvImage.ActualHeight);
            screenCaputre.StartCaputre(30, lastSize, this);
        }
        private void btnSharpen_Click(object sender, RoutedEventArgs e)
        {
            IntPtr pImg = IntPtr.Zero;
            try
            {
                bmpTmp = KiSharpen(bmpTmp, (float)0.5);
                pImg = bmpTmp.GetHbitmap();
                imgMain.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(pImg, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
            finally
            {
                if (pImg != IntPtr.Zero)
                {
                    DeleteObject(pImg);

                }
            }
        }
        private void btnEnchance_Click(object sender, RoutedEventArgs e)
        {
            IntPtr pImg = IntPtr.Zero;
            try
            {
                bmpTmp = KiSharpen(bmpTmp, (float)1.5);
                pImg = bmpTmp.GetHbitmap();
                imgMain.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(pImg, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
            finally
            {
                if (pImg != IntPtr.Zero)
                {
                    DeleteObject(pImg);

                }
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            IntPtr pImg = IntPtr.Zero;
            try
            {
                pImg = orginalImage.GetHbitmap();
                imgMain.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(pImg, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                if (bmpTmp != null)
                {
                    DeleteObject(bmpTmp.GetHbitmap());
                    bmpTmp.Dispose();
                    bmpTmp = null;
                }
                bmpTmp = (Bitmap)Bitmap.FromFile(imgInfo.Image_Path);
                currRVal = 0;
                currGVal = 0;
                currBVal = 0;
                sldBlue.Value = 0;
                sldRed.Value = 0;
                sldGreen.Value = 0;
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
            finally
            {
                if (pImg != IntPtr.Zero)
                {
                    DeleteObject(pImg);
                }
            }

        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("请确认是否保存图片？",
                                       "保存确认", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmpTmp.Save(ms, ImageFormat.Bmp);
                        if (bmpTmp != null)
                        {
                            DeleteObject(bmpTmp.GetHbitmap());
                            bmpTmp.Dispose();
                            bmpTmp = null;
                        }
                        if (orginalImage != null)
                        {
                            DeleteObject(orginalImage.GetHbitmap());
                            orginalImage.Dispose();
                            orginalImage = null;
                        }
                        using (FileStream fs = new FileStream(imgInfo.Image_Path, FileMode.Create))
                        {
                            byte[] buffer = new byte[ms.Length];
                            buffer = ms.GetBuffer();
                            fs.Write(buffer, 0, buffer.Length);
                        }
                    }

                }
                catch (Exception e1)
                {
                    ErrorHandle.HandleException(e1, false);
                }
                this.Close();
            }
        }
        void item2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lbxColor.SelectedItem == null) return;
                ImageColor imgColor = (ImageColor)lbxColor.SelectedItem;
                ColorTemplateName window = new ColorTemplateName(imgColor.Name);
                window.Owner = this.Owner;
                window.ShowInTaskbar = false;
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
                System.Windows.Point point = new System.Windows.Point();
                window.Left = btnColorSave.PointToScreen(point).X - 200;
                window.Top = btnColorSave.PointToScreen(point).Y + 30;
                if (window.ShowDialog() == true)
                {
                    imgColor.Name = window.Name;
                    imgColor.Update();
                    lbxColor.Items.Refresh();
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
                if (lbxColor.SelectedItem == null) return;
                ImageColor imgColor = (ImageColor)lbxColor.SelectedItem;
                imgColor.Delete();
                vmColor.RemoveColorFrpmTemplate(imgColor);
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
        }
        private void sldRed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            IntPtr pImg = IntPtr.Zero;
            try
            {
                bmpTmp = KiColorBalance(bmpTmp, (int)sldRed.Value - currRVal, 0, 0);
                pImg = bmpTmp.GetHbitmap();
                imgMain.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(pImg, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                currRVal = (int)sldRed.Value;

            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
            finally
            {
                if (pImg != IntPtr.Zero)
                {
                    DeleteObject(pImg);
                }
            }
        }
        private void sldGreen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            IntPtr pImg = IntPtr.Zero;
            try
            {
                bmpTmp = KiColorBalance(bmpTmp, 0, (int)sldGreen.Value - currGVal, 0);
                pImg = bmpTmp.GetHbitmap();
                imgMain.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(pImg, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                currGVal = (int)sldGreen.Value;

            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
            finally
            {
                if (pImg != IntPtr.Zero)
                {
                    DeleteObject(pImg);
                }
            }
        }
        private void sldBlue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            IntPtr pImg = IntPtr.Zero;
            try
            {
                bmpTmp = KiColorBalance(bmpTmp, 0, 0, (int)sldBlue.Value - currBVal);
                pImg = bmpTmp.GetHbitmap();
                imgMain.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(pImg, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                currBVal = (int)sldBlue.Value;

            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
            finally
            {
                if (pImg != IntPtr.Zero)
                {
                    DeleteObject(pImg);
                }
            }
        }
        private void InkCanvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                imgMain.Width = imgMain.ActualWidth;
                imgMain.Height = imgMain.ActualHeight;
                imgMain.Width *= 0.8;
                imgMain.Height *= 0.8;
            }
            else
            {
                imgMain.Width = imgMain.ActualWidth;
                imgMain.Height = imgMain.ActualHeight;
                imgMain.Width *= 1.25;
                imgMain.Height *= 1.25;
            }
            e.Handled = true;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisposeImage();
        }
        private void OnScreenCaputreCancelled(object sender, System.EventArgs e)
        {
            if (screenCaputre != null)
            {
                screenCaputre.CancelCapture();
            }
        }
        private void OnScreenCaputred(object sender, RisCaptureLib.ScreenCaputredEventArgs e)
        {
            IntPtr pImg = IntPtr.Zero;
            try
            {
                //set last size
                lastSize = new System.Windows.Size(e.Bmp.Width, e.Bmp.Height);
                //test
                bmpTmp = e.Bmp;
                pImg = bmpTmp.GetHbitmap();
                imgMain.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(pImg, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                if (screenCaputre != null)
                {
                    screenCaputre.CancelCapture();
                }
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
            finally
            {
                if (pImg != IntPtr.Zero)
                {
                    DeleteObject(pImg);
                }
            }

        }
        private void lbxColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxColor.SelectedItem == null) return;
            ImageColor imgColor = (ImageColor)lbxColor.SelectedItem;
            vmColor.Unique_Number = imgColor.Unique_Number;
            vmColor.Name = imgColor.Name;
            vmColor.Red = imgColor.Red;
            vmColor.Green = imgColor.Green;
            vmColor.Blue = imgColor.Blue;
        }
        #endregion

        #region Methods
        void Init(PatientImage imgobj)
        {
            //图像初始化
            IntPtr pImg = IntPtr.Zero;
            try
            {
                imgInfo = imgobj;
                bmpTmp = (Bitmap)Bitmap.FromFile(imgobj.Image_Path);
                orginalImage = (Bitmap)bmpTmp.Clone();
                pImg = bmpTmp.GetHbitmap();
                imgMain.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(pImg, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                imgMain.Height = srvImage.Height;
                imgMain.Width = srvImage.Width;


            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);
            }
            finally
            {
                if (pImg != IntPtr.Zero)
                {
                    DeleteObject(pImg);
                }
            }

        }
        Bitmap KiSharpen(Bitmap b, float val)
        {
            if (b == null)
            {
                return null;
            }

            int w = b.Width;
            int h = b.Height;
            Bitmap bmpRtn = null;
            try
            {
                bmpRtn = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                System.Drawing.Imaging.BitmapData srcData = b.LockBits(new System.Drawing.Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                System.Drawing.Imaging.BitmapData dstData = bmpRtn.LockBits(new System.Drawing.Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* pIn = (byte*)srcData.Scan0.ToPointer();
                    byte* pOut = (byte*)dstData.Scan0.ToPointer();
                    int stride = srcData.Stride;
                    byte* p;

                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            //取周围9点的值。位于边缘上的点不做改变。
                            if (x == 0 || x == w - 1 || y == 0 || y == h - 1)
                            {
                                //不做
                                pOut[0] = pIn[0];
                                pOut[1] = pIn[1];
                                pOut[2] = pIn[2];
                            }
                            else
                            {
                                int r1, r2, r3, r4, r5, r6, r7, r8, r0;
                                int g1, g2, g3, g4, g5, g6, g7, g8, g0;
                                int b1, b2, b3, b4, b5, b6, b7, b8, b0;

                                float vR, vG, vB;

                                //左上
                                p = pIn - stride - 3;
                                r1 = p[2];
                                g1 = p[1];
                                b1 = p[0];

                                //正上
                                p = pIn - stride;
                                r2 = p[2];
                                g2 = p[1];
                                b2 = p[0];

                                //右上
                                p = pIn - stride + 3;
                                r3 = p[2];
                                g3 = p[1];
                                b3 = p[0];

                                //左侧
                                p = pIn - 3;
                                r4 = p[2];
                                g4 = p[1];
                                b4 = p[0];

                                //右侧
                                p = pIn + 3;
                                r5 = p[2];
                                g5 = p[1];
                                b5 = p[0];

                                //右下
                                p = pIn + stride - 3;
                                r6 = p[2];
                                g6 = p[1];
                                b6 = p[0];

                                //正下
                                p = pIn + stride;
                                r7 = p[2];
                                g7 = p[1];
                                b7 = p[0];

                                //右下
                                p = pIn + stride + 3;
                                r8 = p[2];
                                g8 = p[1];
                                b8 = p[0];

                                //自己
                                p = pIn;
                                r0 = p[2];
                                g0 = p[1];
                                b0 = p[0];

                                vR = (float)r0 - (float)(r1 + r2 + r3 + r4 + r5 + r6 + r7 + r8) / 8;
                                vG = (float)g0 - (float)(g1 + g2 + g3 + g4 + g5 + g6 + g7 + g8) / 8;
                                vB = (float)b0 - (float)(b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8) / 8;

                                vR = r0 + vR * val;
                                vG = g0 + vG * val;
                                vB = b0 + vB * val;

                                if (vR > 0)
                                {
                                    vR = Math.Min(255, vR);
                                }
                                else
                                {
                                    vR = Math.Max(0, vR);
                                }

                                if (vG > 0)
                                {
                                    vG = Math.Min(255, vG);
                                }
                                else
                                {
                                    vG = Math.Max(0, vG);
                                }

                                if (vB > 0)
                                {
                                    vB = Math.Min(255, vB);
                                }
                                else
                                {
                                    vB = Math.Max(0, vB);
                                }

                                pOut[0] = (byte)vB;
                                pOut[1] = (byte)vG;
                                pOut[2] = (byte)vR;

                            }

                            pIn += 3;
                            pOut += 3;
                        }// end of x

                        pIn += srcData.Stride - w * 3;
                        pOut += srcData.Stride - w * 3;
                    } // end of y
                }

                b.UnlockBits(srcData);
                bmpRtn.UnlockBits(dstData);
                DeleteObject(b.GetHbitmap());
                b.Dispose();
                return bmpRtn;
            }
            catch
            {
                if (bmpRtn != null)
                    bmpRtn.Dispose();
                return null;
            }

        }
        Bitmap KiColorBalance(Bitmap bmp, int rVal, int gVal, int bVal)
        {

            if (bmp == null)
            {
                return null;
            }


            int h = bmp.Height;
            int w = bmp.Width;

            try
            {
                if (rVal > 255 || rVal < -255 || gVal > 255 || gVal < -255 || bVal > 255 || bVal < -255)
                {
                    return null;
                }

                System.Drawing.Imaging.BitmapData srcData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* p = (byte*)srcData.Scan0.ToPointer();

                    int nOffset = srcData.Stride - w * 3;
                    int r, g, b;

                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {

                            b = p[0] + bVal;
                            if (bVal >= 0)
                            {
                                if (b > 255) b = 255;
                            }
                            else
                            {
                                if (b < 0) b = 0;
                            }

                            g = p[1] + gVal;
                            if (gVal >= 0)
                            {
                                if (g > 255) g = 255;
                            }
                            else
                            {
                                if (g < 0) g = 0;
                            }

                            r = p[2] + rVal;
                            if (rVal >= 0)
                            {
                                if (r > 255) r = 255;
                            }
                            else
                            {
                                if (r < 0) r = 0;
                            }

                            p[0] = (byte)b;
                            p[1] = (byte)g;
                            p[2] = (byte)r;

                            p += 3;
                        }

                        p += nOffset;


                    }
                } // end of unsafe

                bmp.UnlockBits(srcData);
                return bmp;
            }
            catch
            {
                return null;
            }

        }
        void DisposeImage()
        {
            try
            {
                if (bmpTmp != null)
                {
                    DeleteObject(bmpTmp.GetHbitmap());
                    bmpTmp.Dispose();
                    bmpTmp = null;
                }
                if (orginalImage != null)
                {
                    DeleteObject(orginalImage.GetHbitmap());
                    orginalImage.Dispose();
                    orginalImage = null;
                }
                if (screenCaputre != null)
                {
                    screenCaputre.CancelCapture();
                }
            }
            catch (Exception e1)
            {
                ErrorHandle.HandleException(e1, false);
            }
        }
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        public void LoadImage(PatientImage imgobj)
        {
            DisposeImage();
            Init(imgobj);
        }
        #endregion



    }
}
