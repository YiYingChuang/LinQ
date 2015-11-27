using Lab_01_HelloLINQ;
using Lab_02_CSharp_3._0;
using Lab_03;
using LINQ.StartupLabs;
using LinqLabs;
using MyLINQ;
using MyLINQ.StartupLabs;
using Starter;
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

            //Application.Run(new FrmLINQ_To_XXX());
            //Application.Run(new FrmLinqToDataSet());

            Application.Run(new FrmHelloLINQ());
        }
    }
}
