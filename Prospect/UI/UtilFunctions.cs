using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Management;

namespace com.cloud.prospect
{
    public class UtilFunctions
    {
        public static object locker = new object();
        public static void SaveAppSettings(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception e)
            {
                Logger.WriteLogs(LogServerity.Warning, e.Message + "\r\n" + e.StackTrace);
            }
        }

        public static void WriteFile(string filePath, bool append, string content)
        {
            lock (locker)
            {
                FileInfo file = new FileInfo(filePath);
                if (!file.Directory.Exists)
                {
                    file.Directory.Create();
                }
                using (StreamWriter sw = new StreamWriter(filePath, append))
                {
                    sw.Write(content);
                }
            }
        }

        public static string ReadFileToEnd(string filePath)
        {
            string result = String.Empty;
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
                using (StreamReader sr = new StreamReader(filePath))
                {
                    result = sr.ReadToEnd();
                }
            else
                ErrorHandle.HandlerWarning("文件" + filePath + "不存在。", "UtilFunctions.ReadFileToEnd");
            return result;
        }

        public static string GetHardwareNumber()
        {
            ManagementClass mc = new ManagementClass("WIN32_BaseBoard");
            ManagementObjectCollection moc = mc.GetInstances();
            string SerialNumber = "";
            foreach (ManagementObject mo in moc)
            {
                SerialNumber = mo["SerialNumber"].ToString();
            }
            if (string.IsNullOrEmpty(SerialNumber))
                mc = new ManagementClass("Win32_PhysicalMedia");
            moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                SerialNumber = mo.Properties["SerialNumber"].Value.ToString();
                break;
            }
            return SerialNumber;
        }

    }
}
