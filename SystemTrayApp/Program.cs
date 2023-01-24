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
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            int ID = 0;
            if (args.Length != 1)
                return;
            Regex r = new Regex(@"\b([0-9]|[1-9][0-9])\b");
            if (!r.IsMatch(args[0]) && args[0].Length > 2)
                return;
            ID = int.Parse(args[0]);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main(ID));
        }
    }
}
