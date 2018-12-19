using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using com.cloud.video;


namespace com.cloud.prospect
{
    public class VMR9ViewModel : INotifyPropertyChanged
    {

        #region Members
        VMR9Control _VMR9Control;
        VideoController videoController;
        #endregion

        #region Construction
        public VMR9ViewModel()
        {
            _VMR9Control = new VMR9Control();
            videoController = VideoControllerFactory.CreateInstance();
        }
        #endregion

        #region Properties
        public VMR9Control VMR9Control
        {
            get
            {
                return _VMR9Control;
            }
            set
            {
                _VMR9Control = value;
               
            }
        }
        public Single Contrast
        {
            get
            {
                return _VMR9Control.Contrast;
            }
            set
            {
                if (value != _VMR9Control.Contrast)
                {
                    _VMR9Control.Contrast = value;
                    RaisePropertyChanged("Contrast");
                }
            }
        }
        public Single DisplayContrastValue
        {
            get
            {
                //float temp = _VMR9Control.Contrast;
                //return (temp - _VMR9Control.ContrastMinValue) * 255 / (_VMR9Control.ContrastMaxValue - _VMR9Control.ContrastMinValue);
                return _VMR9Control.Contrast;
            }
            set
            {
                _VMR9Control.Contrast = value; //_VMR9Control.ContrastMinValue + value * (_VMR9Control.ContrastMaxValue - _VMR9Control.ContrastMinValue) / 255;
            }
        }
        public Single ContrastMaxValue
        {
            get
            {
                return _VMR9Control.ContrastMaxValue;
            }
            set
            {
                _VMR9Control.ContrastMaxValue = value;
            }
        }
        public Single ContrastMinValue
        {
            get
            {
                return _VMR9Control.ContrastMinValue;
            }
            set
            {
                _VMR9Control.ContrastMinValue = value;
            }
        }
        public Single Brightness
        {
            get
            {
                return _VMR9Control.Brightness;
            }
            set
            {
                if (value != _VMR9Control.Brightness)
                {
                    _VMR9Control.Brightness = value;
                    RaisePropertyChanged("Brightness");
                }
            }
        }
        public Single DisplayBrightnessValue
        {
            get
            {
                //float temp = _VMR9Control.Brightness;
                //return (temp - _VMR9Control.BrightnessMinValue) * 255 / (_VMR9Control.BrightnessMaxValue - _VMR9Control.BrightnessMinValue);
                return _VMR9Control.Brightness;
            }
            set
            {
                _VMR9Control.Brightness = value;//_VMR9Control.BrightnessMinValue + value * (_VMR9Control.BrightnessMaxValue - _VMR9Control.BrightnessMinValue) / 255;
            }
        }
        public Single BrightnessMaxValue
        {
            get
            {
                return _VMR9Control.BrightnessMaxValue;
            }
            set
            {
                if (value != _VMR9Control.BrightnessMaxValue)
                    _VMR9Control.BrightnessMaxValue = value;
            }
        }
        public Single BrightnessMinValue
        {
            get
            {
                return _VMR9Control.BrightnessMinValue;
            }
            set
            {
                if (value != _VMR9Control.BrightnessMinValue)
                    _VMR9Control.BrightnessMinValue = value;
            }
        }
        public Single Hue
        {
            get
            {
                return _VMR9Control.Hue;
            }
            set
            {
                if (value != _VMR9Control.Hue)
                {
                    _VMR9Control.Hue = value;
                    RaisePropertyChanged("Hue");
                }
            }
        }
        public Single DisplayHueValue
        {
            get
            {
                //float temp = _VMR9Control.Hue;
                //return (temp - _VMR9Control.HueMinValue) * 255 / (_VMR9Control.HueMaxValue - _VMR9Control.HueMinValue);
                return _VMR9Control.Hue;
            }
            set
            {
                _VMR9Control.Hue = value;//_VMR9Control.HueMinValue + value * (_VMR9Control.HueMaxValue - _VMR9Control.HueMinValue) / 255;
            }
        }
        public Single HueMaxValue
        {
            get
            {
                return _VMR9Control.HueMaxValue;
            }
            set
            {
                if (value != _VMR9Control.HueMaxValue)
                    _VMR9Control.HueMaxValue = value;
            }
        }
        public Single HueMinValue
        {
            get
            {
                return _VMR9Control.HueMinValue;
            }
            set
            {
                if (value != _VMR9Control.HueMinValue)
                    _VMR9Control.HueMinValue = value;
            }
        }
        public Single Saturation
        {
            get
            {
                return _VMR9Control.Saturation;
            }
            set
            {
                if (value != _VMR9Control.Saturation)
                {
                    _VMR9Control.Saturation = value;
                    RaisePropertyChanged("Saturation");
                }
            } 
        }
        public Single DisplaySaturationValue
        {
            get
            {
                //float temp = _VMR9Control.Saturation;
                //return (temp - _VMR9Control.SaturationMinValue) * 255 / (_VMR9Control.SaturationMaxValue - _VMR9Control.SaturationMinValue);
                return _VMR9Control.Saturation;
            }
            set
            {
                _VMR9Control.Saturation = value;// _VMR9Control.SaturationMinValue + value * (_VMR9Control.SaturationMaxValue - _VMR9Control.SaturationMinValue) / 255;
            }
        }
        public Single SaturationMaxValue
        {
            get
            {
                return _VMR9Control.SaturationMaxValue;
            }
            set
            {
                _VMR9Control.SaturationMaxValue = value;
            }
        }
        public Single SaturationMinValue
        {
            get
            {
                return _VMR9Control.SaturationMinValue;
            }
            set
            {
                _VMR9Control.SaturationMinValue = value;
            }
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Method
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = System.Threading.Interlocked.CompareExchange(ref PropertyChanged, null, null); 
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
