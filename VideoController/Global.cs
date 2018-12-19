using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.video
{
    public class Global
    {
        internal const int
            MSG_VIDEO_START = 0x0,
            MSG_VIDEO_STOP = 0x1,
            MSG_VIDEO_PAUSE = 0x2,
            MSG_RECORD_START = 0x3,
            MSG_RECORD_STOP = 0x4,
            MSG_CAPTURE_IMG = 0x5;
    }
}
