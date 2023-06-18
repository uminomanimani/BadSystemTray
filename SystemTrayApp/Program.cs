using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;

namespace SystemTrayApp
{
    internal static class Program
    {
        static bool OSRequirementMet()
        {
            Version osVersion = Environment.OSVersion.Version;
            if (osVersion.Major == 10 && osVersion.Minor == 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!OSRequirementMet()) 
            {
                MessageBox.Show("Oops，你的操作系统不是Windows10...");
                Environment.Exit(0);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
