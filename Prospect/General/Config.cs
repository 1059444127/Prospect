using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public class Config
    {
        #region TempValue
        public static bool IsClip = false;
        public static int ClipTop = 0;
        public static int ClipLeft = 0;
        public static int ClipRight = 100;
        public static int ClipBottom = 100;
        #endregion


        #region CurrentValue
        public static string PatientImageBaseDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\Patient\";
        public static SelectedFields SelectedColunms = new SelectedFields();
        public static int FileFormat = 1;
        public static int CodeRate = 2000;
        public static string SignalSys = "PAL";
        public static int DeviceIndex = 0;
        public static int DurationVideoRecord = 10;
        public static string HostpitalName = "医院名称";
        public static string ReportName = "报告单名称";
        public static string ReportFoot = "备注：此报告仅供临床参考，不作证明。";
        public static string ReportFormat = "A4";
        public static string DefautReport = "标准样式A4-一图.frx";
        public static string SerialPortName = "COM1";
        public static bool EnabledDecomp = false;
        public static bool CompressPic = false;
        public static bool EnableFtp = false;
        public static string ftpIP = string.Empty;
        public static string ftpUser = string.Empty;
        public static string ftpPassword = string.Empty;
        public static string ftpWebServiceIP = string.Empty;
        public static float DisplayBrightnessValue = 9999999;
        public static float DisplayContrastValue = 9999999;
        public static float DisplayHueValue = 9999999;
        public static float DisplaySaturationValue = 9999999;
        #endregion

        public static void LoadConfig(string table_name)
        {
            try
            {
                List<Parameter> paramters = new Parameter(table_name).SelectAll();
                foreach (Parameter param in paramters)
                {

                    switch (param.Parameter_Name)
                    {
                        case "图片保存地址":
                            PatientImageBaseDirectory = param.Parameter_Value1;
                            break;
                        case "录像格式":
                            int value = 1;
                            string result = param.Parameter_Value1;
                            int.TryParse(result, out value);
                            FileFormat = value;
                            break;
                        case "录像码率":
                            value = 2000;
                            result = param.Parameter_Value1;
                            int.TryParse(result, out value);
                            CodeRate = value;
                            break;
                        case "信号制式":
                            SignalSys = param.Parameter_Value1;
                            break;
                        case "采集设备":
                            value = 0;
                            result = param.Parameter_Value1;
                            int.TryParse(result, out value);
                            DeviceIndex = value;
                            break;
                        case "录像时长":
                            value = 0;
                            result = param.Parameter_Value1;
                            int.TryParse(result, out value);
                            DurationVideoRecord = value;
                            break;
                        case "医院名称":

                            HostpitalName = param.Parameter_Value1;
                            break;
                        case "报告单名称":

                            ReportName = param.Parameter_Value1;
                            break;
                        case "报告单脚注":

                            ReportFoot = param.Parameter_Value1;
                            break;
                        case "报表格式":

                            ReportFormat = param.Parameter_Value1;
                            break;
                        case "默认报表":

                            DefautReport = param.Parameter_Value1;
                            break;
                        case "串口名称":
                            SerialPortName = param.Parameter_Value1;
                            break;
                        case "激活解码器":
                            bool bValue = false;
                            bool.TryParse(param.Parameter_Value1, out bValue);
                            EnabledDecomp = bValue;
                            break;
                        case "压缩图片":
                            bValue = false;
                            bool.TryParse(param.Parameter_Value1, out bValue);
                            CompressPic = bValue;
                            break;
                        case "亮度":
                            float fValue = 9999999;
                            float.TryParse(param.Parameter_Value1, out fValue);
                            DisplayBrightnessValue = fValue;
                            break;
                        case "对比度":
                            fValue = 9999999;
                            float.TryParse(param.Parameter_Value1, out fValue);
                            DisplayContrastValue = fValue;
                            break;
                        case "色度":
                            fValue = 9999999;
                            float.TryParse(param.Parameter_Value1, out fValue);
                            DisplayHueValue = fValue;
                            break;
                        case "饱和度":
                            fValue = 9999999;
                            float.TryParse(param.Parameter_Value1, out fValue);
                            DisplaySaturationValue = fValue;
                            break;
                    }


                }
                SelectedColunms.Select();
            }
            catch (Exception e)
            {

                ErrorHandle.HandleException(e, false);
            }

        }

        public static void LoadCurrentConfig()
        {
            LoadConfig("S_Parameter");
        }

        public static void LoadLastConfig()
        {
            LoadConfig("S_Parameter_Last");
        }

        public static void LoadDefautConfig()
        {
            LoadConfig("S_Parameter_Defaut");

        }

        public static void InsertLastConfig()
        {
            new Parameter().InsertLastConfig();
        }

        public static void SaveConfig()
        {
            try
            {
                Parameter p = new Parameter()
                {
                    Parameter_Name = "图片保存地址",
                    Parameter_Value1 = Config.PatientImageBaseDirectory
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "录像格式",
                    Parameter_Value1 = Config.FileFormat.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "录像码率",
                    Parameter_Value1 = Config.CodeRate.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "信号制式",
                    Parameter_Value1 = Config.SignalSys.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "采集设备",
                    Parameter_Value1 = Config.DeviceIndex.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "录像时长",
                    Parameter_Value1 = Config.DurationVideoRecord.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "医院名称",
                    Parameter_Value1 = Config.HostpitalName.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "报告单名称",
                    Parameter_Value1 = Config.ReportName.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "报告单脚注",
                    Parameter_Value1 = Config.ReportFoot.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "报表格式",
                    Parameter_Value1 = Config.ReportFormat.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "默认报表",
                    Parameter_Value1 = Config.DefautReport.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "串口名称",
                    Parameter_Value1 = Config.SerialPortName.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "激活解码器",
                    Parameter_Value1 = Config.EnabledDecomp.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "压缩图片",
                    Parameter_Value1 = Config.CompressPic.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "亮度",
                    Parameter_Value1 = Config.DisplayBrightnessValue.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "对比度",
                    Parameter_Value1 = Config.DisplayContrastValue.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "色度",
                    Parameter_Value1 = Config.DisplayHueValue.ToString()
                };
                p.Update();
                p = new Parameter()
                {
                    Parameter_Name = "饱和度",
                    Parameter_Value1 = Config.DisplaySaturationValue.ToString()
                };
                p.Update();

                SelectedColunms.Update();

            }
            catch (Exception e)
            {
                ErrorHandle.HandleException(e, false);
            }

        }
    }
}
