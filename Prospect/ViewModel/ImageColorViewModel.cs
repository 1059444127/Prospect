using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Collections.ObjectModel;

namespace com.cloud.prospect
{
    public class ImageColorViewModel : INotifyPropertyChanged
    {
        private static ObservableCollection<ImageColor> ColorTemplates;

        #region Member
        ImageColor imgColor;
        #endregion

        #region Construction
        public ImageColorViewModel()
        {
            imgColor = new ImageColor();
        }
        #endregion
        #region Properties
        public int Unique_Number
        {
            get
            {
                return imgColor.Unique_Number;
            }
            set
            {
                if (value != imgColor.Unique_Number)
                {
                    imgColor.Unique_Number = value;
                    RaisePropertyChanged("Unique_Number");
                }
            }
        }
        public string Name
        {
            get
            {
                return imgColor.Name;
            }
            set
            {
                if (value != imgColor.Name)
                {
                    imgColor.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public int? Red
        {
            get
            {
                return imgColor.Red == default(int?) ? 0 : imgColor.Red;
            }
            set
            {
                if (value != imgColor.Red)
                {
                    imgColor.Red = value;
                    RaisePropertyChanged("Red");
                }
            }
        }
        public int? Green
        {
            get
            {
                return imgColor.Green == default(int?) ? 0 : imgColor.Green;
            }
            set
            {
                if (value != imgColor.Green)
                {
                    imgColor.Green = value;
                    RaisePropertyChanged("Green");
                }
            }
        }
        public int? Blue
        {
            get
            {
                return imgColor.Blue == default(int?) ? 0 : imgColor.Blue;
            }
            set
            {
                if (value != imgColor.Blue)
                {
                    imgColor.Blue = value;
                    RaisePropertyChanged("Blue");
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Method
        void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = Interlocked.CompareExchange(ref PropertyChanged, null, null);
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public ObservableCollection<ImageColor> GetColorTemplates()
        {
            if (ColorTemplates == null)
                ColorTemplates = imgColor.SelectAll();
            return ColorTemplates;
        }
        public void Save()
        {
            imgColor.Insert();
            imgColor.SelectLatestColor();
        }
        public void AddColorToTemplate()
        {
            ImageColor newColor = new ImageColor()
            {
                Unique_Number = imgColor.Unique_Number,
                Name = imgColor.Name,
                Red = imgColor.Red,
                Green = imgColor.Green,
                Blue = imgColor.Blue
            };
            ColorTemplates.Add(newColor);
        }
        public void RemoveColorFrpmTemplate(ImageColor color)
        {
            ColorTemplates.Remove(color);
        }
      
        #endregion
    }
}
