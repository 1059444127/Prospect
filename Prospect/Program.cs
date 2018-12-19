using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Collections;
using System.IO;
using System.Diagnostics;

using System.Windows;

namespace com.cloud.prospect
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (IsRunningInstance())
            {
                MessageBox.Show("已经启动了彩超图文工作站！");
                return;
            }
            // Your initialization code
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            Assembly asm = Assembly.GetExecutingAssembly();
            Stream sm = asm.GetManifestResourceStream("com.cloud.prospect.Data");
            using (ResourceReader reader = new ResourceReader(sm))
            {
                foreach (DictionaryEntry entry in reader)
                {
                    if ("Data1".Equals(entry.Key))
                        EmbeddedAssembly.Load(Assembly.Load((byte[])entry.Value));
                    else if ("Data2".Equals(entry.Key))
                        EmbeddedAssembly.Load(Assembly.Load((byte[])entry.Value));
                    else if ("Data3".Equals(entry.Key))
                        EmbeddedAssembly.Load(Assembly.Load((byte[])entry.Value));
                    else if ("Data4".Equals(entry.Key))
                        EmbeddedAssembly.Load(Assembly.Load((byte[])entry.Value));
                }

            }

            Prospect.App app = new Prospect.App();
            app.InitializeComponent();
            app.Run();
        }

        static void CreateResource()
        {
            FileInfo file = new FileInfo("FastReport.Bars.dll");
            int length = (int)file.Length;
            byte[] content = new byte[length];
            using (FileStream fs = new FileStream("FastReport.Bars.dll", FileMode.Open))
            {
                fs.Read(content, 0, length);
            }

            file = new FileInfo("FastReport.Editor.dll");
            length = (int)file.Length;
            byte[] content1 = new byte[length];
            using (FileStream fs = new FileStream("FastReport.Editor.dll", FileMode.Open))
            {
                fs.Read(content1, 0, length);
            }

            file = new FileInfo("FastReport.dll");
            length = (int)file.Length;
            byte[] content2 = new byte[length];
            using (FileStream fs = new FileStream("FastReport.dll", FileMode.Open))
            {
                fs.Read(content2, 0, length);
            }
            file = new FileInfo("ProspectReport.dll");
            length = (int)file.Length;
            byte[] content3 = new byte[length];
            using (FileStream fs = new FileStream("ProspectReport.dll", FileMode.Open))
            {
                fs.Read(content3, 0, length);
            }

            using (ResourceWriter rw = new ResourceWriter("Proepect.data"))
            {
                rw.AddResource("Data1", content);
                rw.AddResource("Data2", content1);
                rw.AddResource("Data3", content2);
                rw.AddResource("Data4", content3);
                rw.Generate();
            }
        }

        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return EmbeddedAssembly.Get(args.Name);
        }

        private static bool IsRunningInstance()
        {
            Process currentProcess = Process.GetCurrentProcess(); //获取当前进程 
            //获取当前运行程序完全限定名 
            string currentFileName = currentProcess.MainModule.FileName;
            //获取进程名为ProcessName的Process数组。 
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            //遍历有相同进程名称正在运行的进程 
            foreach (Process process in processes)
            {
                if (process.MainModule.FileName == currentFileName)
                {
                    if (process.Id != currentProcess.Id) //根据进程ID排除当前进程 
                        return true;//返回已运行的进程实例 
                }
            }
            return false;
        }

    }
}
