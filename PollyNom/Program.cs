using System;
using System.Windows.Forms;

using PollyNom.View;

namespace PollyNom
{
#pragma warning disable SA1400 // Access modifier must be declared
    static class Program
#pragma warning restore SA1400 // Access modifier must be declared
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PollyForm());
        }
    }
}
