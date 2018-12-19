using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.video
{
    public class Device
    {
        public string FriendlyName
        { get; set; }
        public string Description
        { get; set; }
        public string DevicePath
        { get; set; }
        public int Index
        { get; set; }

        public override string ToString()
        {
            return FriendlyName;
        }
    }
}
