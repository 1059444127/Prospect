using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace com.cloud.prospect
{
    public class ErrorHandle
    {

        public static void HandleException(Exception e)
        {
            Logger.WriteException(e);
            MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(-1);
        }

        public static void HandleException(Exception e, bool ExitApplication)
        {
            if (ExitApplication)
                HandleException(e);
            else
            {
                Logger.WriteException(e);
               // MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void HandleException(Exception e, string message)
        {
            Logger.WriteException(e);
            MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(-1);
        }

        public static void DefautHandleException(Exception e)
        {
            Logger.WriteException(e);
            MessageBox.Show("发生未知错误！程序将终止。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(-1);
        }

        public static void HandlerWarning(string message)
        {
            Logger.WriteLogs(LogServerity.Warning, message);
            MessageBox.Show(message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void HandlerWarning(string message, string module)
        {
            Logger.WriteLogs(LogServerity.Warning, module, message);
            MessageBox.Show(message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
