using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace com.cloud.prospect
{
    public partial class ImageColor : INotifyPropertyChanged
    {
        public int Unique_Number
        { get; set; }
        public string Name
        { get; set; }
        public int? Red
        { get; set; }
        public int? Green
        { get; set; }
        public int? Blue
        { get; set; }

        public override string ToString()
        {
            return Name;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = Interlocked.CompareExchange(ref PropertyChanged, null, null);
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
