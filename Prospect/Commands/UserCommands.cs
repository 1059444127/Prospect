using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace com.cloud.prospect
{
    public class UserCommands
    {
        public static readonly RoutedUICommand DeleteImage = new RoutedUICommand("删除", "DeleteImage", typeof(UserCommands));

    }
}
