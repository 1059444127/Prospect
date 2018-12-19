using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace com.cloud.prospect
{
    public partial class MainTemplate
    {
        public int Template_Id
        { get; set; }
        public int Parent_Id
        { get; set; }
        public string Name
        { get; set; }
        public string Description
        { get; set; }
        public int? OrderID
        { get; set; }
        public int? NodeLevel
        { get; set; }
        public string ImagePath
        { get; set; }
        public ObservableCollection<MainTemplate> Children
        { get; set; }
       
    }
}
