using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCFConverter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {           
            System.Diagnostics.Process[] pcProcess = System.Diagnostics.Process.GetProcessesByName("EXCEL");
            if (pcProcess.Length > 0)
            {
                foreach (Process p in pcProcess)
                {
                    p.Kill();
                }                           
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Converter());

            System.Diagnostics.Process[] pcProcess2 = System.Diagnostics.Process.GetProcessesByName("EXCEL");
            if (pcProcess2.Length > 0)
            {
                foreach (Process p2 in pcProcess2)
                {
                    p2.Kill();
                }
            }


        }

        
    }
}
