using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class BodyArea
    {
        #region Properties
        public int? BodyArea_Number
        { get; set; }
        public string Modality_Type
        { get; set; }
        public string BodyArea_Name
        { get; set; }
        public decimal? Charge1
        { get; set; }
        public decimal? Charge2
        { get; set; }
        #endregion
    }
}
