using System;
using System.Windows.Forms;

namespace OrderView
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += MyHandler;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception) args.ExceptionObject;
            Console.WriteLine(@"Exception caught: " + e.Message);
            MessageBox.Show(e.Message, @"´íÎó", MessageBoxButtons.OK);
        }
    }
}