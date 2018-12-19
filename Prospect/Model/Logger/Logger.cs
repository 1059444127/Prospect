using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace com.cloud.prospect
{
    public enum LogServerity
    {
        Information = 0,
        Debug = 1,
        Warning = 2,
        Error = 4
    }

    public class Logger
    {
        public static string LogPath
        {
            get
            {
                
                return AppDomain.CurrentDomain.BaseDirectory + "\\" + ConfigurationManager.AppSettings["LogPath"];
            }
            private set
            {
            }
        }

        public static void WriteLogs(LogServerity serverity, string content)
        {
            switch (serverity)
            {
                case LogServerity.Information:
                case LogServerity.Debug:
                case LogServerity.Warning:
                    UtilFunctions.WriteFile(LogPath + "\\Log\\" + DateTime.Now.ToString("yyyyMMdd") + ".log", true, DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + content + "\r\n");
                    break;
                case LogServerity.Error:
                    UtilFunctions.WriteFile(LogPath + "\\Error\\" + DateTime.Now.ToString("yyyyMMdd") + ".log", true, DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + content + "\r\n");
                    break;
            }
        }

        public static void WriteLogs(LogServerity serverity, string target, string content)
        {
            switch (serverity)
            {
                case LogServerity.Information:
                case LogServerity.Debug:
                case LogServerity.Warning:
                    UtilFunctions.WriteFile(LogPath + "\\Log\\" + DateTime.Now.ToString("yyyyMMdd") + ".log", true, DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + target + ":" + content + "\r\n");
                    break;
                case LogServerity.Error:
                    UtilFunctions.WriteFile(LogPath + "\\Error\\" + DateTime.Now.ToString("yyyyMMdd") + ".log", true, DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + target + ":" + content + "\r\n");
                    break;

            }
        }

        public static void WriteException(Exception e)
        {
            WriteLogs(LogServerity.Error, e.Message + "\r\n" + e.StackTrace);
        }
    }
}
