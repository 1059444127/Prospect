using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.video
{
    public class VideoControllerFactory
    {
        public static VideoController CreateInstance()
        {
            return VideoController.GetInstance();
        }
    }
}
