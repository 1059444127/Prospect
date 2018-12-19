using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
   public partial class Doctor
    {
       public int Doctor_Number
       { get; set; }
       public int? Department_Number
       { get; set; }
       public string Name
       { get; set; }
       public string Profile
       { get; set; }
       public string Account_Name
       { get; set; }
       public string Password
       { get; set; }
    }
}
