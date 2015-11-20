using Lab_01_HelloLINQ;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace LINQLabs
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new  FrmHelloLINQ());
        }
    }
}
