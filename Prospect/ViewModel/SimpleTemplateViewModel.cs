using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Collections.ObjectModel;


namespace com.cloud.prospect
{
    public class SimpleTemplateViewModel : INotifyPropertyChanged
    {
        #region Member
        SimpleTemplate template;
        #endregion

        #region Construction
        public SimpleTemplateViewModel(StartUpWindow window)
        {
            template = new SimpleTemplate();
            switch (window)
            {
                case StartUpWindow.SimpleTemplate1:
                    break;
                case StartUpWindow.SimpleTemplate2:
                    break;
            }

        }
        #endregion

        #region Properties
        public int? Template_Number
        {
            get
            {
                return template.Template_Number;
            }
            set
            {
                if (value != template.Template_Number)
                {
                    template.Template_Number = value;
                    RaisePropertyChanged("Template_Number");
                }
            }
        }
        public string Name
        {
            get
            {
                return template.Name;
            }
            set
            {
                if (value != template.Name)
                {
                    template.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public string Description
        {
            get
            {
                return template.Description;
            }
            set
            {
                if (value != template.Description)
                {
                    template.Description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }
        public string Type
        {
            get
            {
                return template.Type;
            }
            set
            {
                if (value != template.Type)
                {
                    template.Type = value;
                    RaisePropertyChanged("Type");
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
        public ObservableCollection<SimpleTemplate> LoadTemplate(int type)
        {
            if (type == 1)
                return new SimpleTemplate().LoadTemplate1();
            else if (type == 2)
                return new SimpleTemplate().LoadTemplate2();
            else
                return null;
        }

        public void Insert(SimpleTemplate template)
        {
            template.Insert();
        }

        public void DeleteByKey(int templateNumber)
        {
            new SimpleTemplate()
            {
                Template_Number = templateNumber
            }.Delete();
        }
     
        #endregion
    }
}
