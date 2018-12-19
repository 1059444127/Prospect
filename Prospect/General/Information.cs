using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace com.cloud.prospect
{
    public class Information
    {
        public static bool IsEnable = false;
        public delegate void ExitEventHandler();
        public static event ExitEventHandler ExitEvent; 

        private static bool IsOpen = false;
        private static bool IsLogon = false;
        private static IntPtr m_handle = IntPtr.Zero;
        public static string Date = "2010-01-01";
        public static string ID = "00000";
        public static bool IsWriteRead = false;

        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112FindDev(ref byte pPidData);



        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr NT112OpenDev(ref byte pPidData, short Index);


        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112DevWrite(IntPtr handle, short StartAdd, ref byte pWriteBuff, short nBuffLen);



        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112DevRead(IntPtr handle, short StartAdd, ref  byte pReadBuff, short nBuffLen);




        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112LedOpen(IntPtr handle);




        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112LedClose(IntPtr handle);

        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112GetDID(IntPtr handle, ref byte pDidData);

        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112VerifySuperPassword(IntPtr handle, ref byte pSuperPassword, short nLen);

        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112VerifyUserPassword(IntPtr handle, ref byte pUserPassword, short nLen);

        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112SetSuperPassword(IntPtr handle, ref byte pSuperPassword, short nLen);

        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112SetUserPassword(IntPtr handle, ref byte pSuperPassword, short nLen);

        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112SetVerifyNum(IntPtr handle, short SuperNum, short UserNum);

        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112GetAuthorityData(IntPtr handle, ref byte pRWSwitch);

        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112SetDispInfo(IntPtr handle, ref byte pDispData, short nLen);

        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112GetPID(IntPtr handle, ref byte pPidData);

        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112SetPID(IntPtr handle, ref byte pProductPassword, short nBuffLen, ref byte pPidData);

        [DllImport("Info.dll", CharSet = CharSet.Auto)]
        private static extern short NT112CloseDev(IntPtr handle);

        private static string byteTostring(byte[] byTemp, int len)
        {
            string strtemp = "";
            for (int j = 0; j < len; j++)
            {
                if (byTemp[j] == 0)
                {
                    break;
                }
                strtemp += string.Format("{0}", (char)byTemp[j]);
            }
            return strtemp;
        }

        private static void OpenDevice()
        {
            try
            {
                byte[] PidData = new byte[16];
                char[] cPidData = new char[16];

                "F03EF800A427BD93".CopyTo(0, cPidData, 0, 16);
                for (int i = 0; i < 16; i++)
                {
                    PidData[i] = (byte)cPidData[i];
                }
                m_handle = NT112OpenDev(ref PidData[0], 0);
                if ((int)m_handle < 1)
                {
                    return;
                }
                IsOpen = true;
            }
            catch (Exception)
            {
                IsOpen = false;
            }

           
        }

        public static string GetID()
        {
            try
            {
                if (!IsLogon) ValidateUser();
                byte[] ReadData = new byte[512];
                string strtemp = string.Empty;
                short StartAdd;
                short nLen;

                StartAdd = 0;
                nLen = 5;
                short m_st = NT112DevRead(m_handle, StartAdd, ref ReadData[0], nLen);
                if (m_st == 0)
                {
                    strtemp = byteTostring(ReadData, nLen);
                }
                return strtemp;
            }
            catch (Exception)
            {
                return "99999";
            }
           
        }

        private static void ValidateUser()
        {
            try
            {
                if (!IsOpen) OpenDevice();
                short VerifyNum;
                short nlen;
                int i;
                byte[] Password = new byte[16];
                char[] cPassword = new char[16];

                string pwd = "hmldchina.com";
                nlen = (short)pwd.Length;

                pwd.CopyTo(0, cPassword, 0, nlen);
                for (i = 0; i < nlen; i++)
                {
                    Password[i] = (byte)cPassword[i];
                }
                VerifyNum = NT112VerifyUserPassword(m_handle, ref Password[0], nlen);

                if (VerifyNum == 0)
                {
                    Logger.WriteLogs(LogServerity.Error, "E002");
                    //Environment.Exit(-1);
                    return;
                }

                if (VerifyNum < 0)
                {
                    Logger.WriteLogs(LogServerity.Error, "E002");
                    //Environment.Exit(-1);
                    return;
                }
                IsLogon = true;
            }
            catch (Exception e)
            {
                IsLogon = false;
            }
           
        }
        public static string GetDate()
        {
            try
            {
                if (!IsLogon) ValidateUser();
                byte[] ReadData = new byte[512];
                string strtemp = string.Empty;
                short StartAdd;
                short nLen;

                StartAdd = 5;
                nLen = 10;
                short m_st = NT112DevRead(m_handle, StartAdd, ref ReadData[0], nLen);
                if (m_st == 0)
                {
                    if (ReadData[0] == 50 && ReadData[1] == 48)
                    {
                        strtemp = byteTostring(ReadData, nLen);
                    }
                }
                return strtemp;
            }
            catch (Exception)
            {
                return "2014-01-01";
            }
           
        }

        public static void WriteDate()
        {
            if (!IsLogon) ValidateUser();
            if (!string.IsNullOrEmpty(GetDate())) return;
            short StartAdd;
            short nLen;
            byte[] WriteData = new byte[512];
            char[] cWriteData = new char[512];

            nLen = 10;
            StartAdd = 5;

            DateTime.Now.ToString("yyyy-MM-dd").CopyTo(0, cWriteData, 0, nLen);
            for (int i = 0; i < nLen; i++)
            {
                WriteData[i] = (byte)cWriteData[i];
            }
            short m_st = NT112DevWrite(m_handle, StartAdd, ref WriteData[0], nLen);
        }

        public static void GetInformation()
        {

            int m_count = 0;
            int i;
            byte[] PidData = new byte[16];
            char[] cPidData = new char[16];


            string productId = "F03EF800A427BD93";

            productId.CopyTo(0, cPidData, 0, 16);
            for (i = 0; i < 16; i++)
            {
                PidData[i] = (byte)cPidData[i];
            }

            try
            {
                m_count = NT112FindDev(ref PidData[0]);
            }
            catch (Exception)
            {

            }

            if (m_count == 0)
            {
                if (errorCount > 2)
                {
                    if (IsEnable)
                    {
                        Logger.WriteLogs(LogServerity.Error, "E001");
                        OnExit();
                    }
                    else
                    {
                        errorCount = 0;
                    }

                }
                errorCount++;
                return;
            }
            errorCount = 0;
            if (!IsOpen) OpenDevice();
        }

        private static void OnExit()
        {
            ExitEventHandler handler = System.Threading.Interlocked.CompareExchange(ref ExitEvent, null, null);
            if (handler != null)
            {
                handler();
            }
        }

        private static int errorCount = 0;

    }
}
